using System.Data;
using System.Runtime.CompilerServices;

Random rng = new();

int lifeExpextancy = 30;
int turn =1;
int targetTurn = 200;
bool wantToChoseName = false;
string defaultName = "Ferdonia";
int populationBooster = 10;
int deathBooster = 0;
int maxDeathBooster = 10;
double maxFoodPerWorker = 1;
int stockFood =0;
string lastFoodAction = "";
int surplusFoodCounter = 0;
(int stock, int deficit) stockAndDeficitCounter= (0,0);
int foodVigilanceLevel =0;

City city = new City();


List<int> populationHistory = new();
List<double> foodHistory = new();
List<int> stockFoodHistory = new();
List<int> deathBoosterHistory = new();
List<int> populationBoosterHistory = new();
List<int> foodVigilanceLevelHistory = new();
List<int> workersHistory = new();

Start();

void Start()
{
    if (wantToChoseName)
    {
        city.Name = SelectName();
    }
    else
    {
        city.Name = defaultName;
    }
    while (turn <= targetTurn)
    {
        DevTriggerEvent();
        PopulationManagement();
        ReactionManagement();
        RessourcesManagement();
        populationHistory.Add(city.CitizensStats.Population);
        foodHistory.Add(city.NaturalsResources!.Food);
        stockFoodHistory.Add(stockFood);
        deathBoosterHistory.Add(deathBooster);
        populationBoosterHistory.Add(populationBooster);
        foodVigilanceLevelHistory.Add(foodVigilanceLevel);
        workersHistory.Add(city.CitizensStats.EmployementRatioAndEmployed[0].numberOfEmployed);
        turn++;
    }
    DisplayGraph();
    DisplayCityInfos();
}

void ReactionManagement()
{
    FoodReactionCounter();
    FoodVigilanceEvaluator();
    FoodReaction();
}

void FoodReaction()
{
    if ((6<=foodVigilanceLevel) && (8>foodVigilanceLevel))
    {
        var newNumberOfWorkers = city.CitizensStats.EmployementRatioAndEmployed[0];
        newNumberOfWorkers.numberOfEmployed+=LaunchCampaignOfRecruitment(1);
        city.CitizensStats.EmployementRatioAndEmployed[0]= newNumberOfWorkers;
    }
    else if (foodVigilanceLevel>=8 && foodVigilanceLevel < 10)
    {
        var newNumberOfWorkers = city.CitizensStats.EmployementRatioAndEmployed[0];
        newNumberOfWorkers.numberOfEmployed+=LaunchCampaignOfRecruitment(2);
        city.CitizensStats.EmployementRatioAndEmployed[0]= newNumberOfWorkers;
    }
    else if (foodVigilanceLevel>10)
    {
        LaunchEmergencyReaffectation("Workers");
    }
}

int LaunchCampaignOfRecruitment(int level)
{
    int min = 0;
    int max = 1;
    if (level ==1)
    {
        min = 5;
        max = 15;
    }
    else if (level == 2)
    {
        min = 15;
        max = 40;
    }
    int newEmployed = rng.Next(min, max)/100*city.CitizensStats.Unemployed;
    city.CitizensStats.Unemployed -= newEmployed;
    return newEmployed;
}

void LaunchEmergencyReaffectation(string typeNeed)
{
    switch (typeNeed)
    {
        case "Workers":
        city.CitizensStats.EmployementRatioAndEmployed[0] = (40,70, city.CitizensStats.EmployementRatioAndEmployed[0].numberOfEmployed);
        for (int i =1; i<4;i++)
        {
            city.CitizensStats.EmployementRatioAndEmployed[i] = (5,10, city.CitizensStats.EmployementRatioAndEmployed[i].numberOfEmployed);
        }
        break;

        default:
        break;
    }
}

void FoodVigilanceEvaluator()
{
    if (stockAndDeficitCounter.stock>3){foodVigilanceLevel++;}
    if (stockAndDeficitCounter.deficit>0){foodVigilanceLevel++;}
}

void FoodReactionCounter()
{
    switch(lastFoodAction)
    {

        case "deficit":
        stockAndDeficitCounter.deficit++;
        if (surplusFoodCounter >0){surplusFoodCounter =0;}
        break;

        case "stock":
        stockAndDeficitCounter.stock++;
        if (surplusFoodCounter >0){surplusFoodCounter =0;}
        break;

        case "surplus":
        if (foodVigilanceLevel>0)
        {
            foodVigilanceLevel--;
        }
        if (stockAndDeficitCounter != (0,0))
        {
            surplusFoodCounter++;
            if (surplusFoodCounter >= 4) {stockAndDeficitCounter = (0,0);}
        }
        break;

        default:
        break;
    }
}

void RessourcesManagement()
{
    UseResources();
    GetResources();
}

void UseResources()
{
    ConsumeFood();
}

void ConsumeFood()
{
    int population = city.CitizensStats.Population;
    double food = city.NaturalsResources!.Food;
    int difference = (int)(food*10)-population;
    if (difference>0) 
    {
        stockFood+=(int)(0.98*difference);
        if (deathBooster > 0)
        {
            deathBooster-=1;
        }
        else
        {
            populationBooster +=1;
        }
        lastFoodAction = "surplus";
    }
    else if (difference < 0)
    {
        while (difference<0 && stockFood>0)
        {
            difference +=1;
            stockFood-=1;
            lastFoodAction = "stock";
        }
        if (difference<0)
        {
            if (deathBooster<maxDeathBooster)
            {
                deathBooster+=1;
            }
            else if (populationBooster>0)
            {
                populationBooster-=1;
            }
            lastFoodAction = "deficit";
        }
    }

}

void GetResources()
{
    city.NaturalsResources!.Food = GetFood(city.CitizensStats.EmployementRatioAndEmployed[0].numberOfEmployed, maxFoodPerWorker);
}

double GetFood(int numOfWorkers, double maxFoodPerWorker)
{
    double foodProduct = 0;

    for (int i = 0; i < numOfWorkers; i++)
    {
        double foodProductByTheWorker = rng.NextDouble() * maxFoodPerWorker;
        foodProduct += foodProductByTheWorker;
    }

    return foodProduct;
}

void PopulationManagement()
{
    PopulationGrowth();
    GetAJob();
}

void GetAJob()
{
    var population = city.CitizensStats?.Population ?? 0;
    var employementRatio = city.CitizensStats?.EmployementRatioAndEmployed;
    List<int> orderJobs = new();
    int chosedNum;
    while(orderJobs.Count != employementRatio!.Count)
    {
        do
        {
            chosedNum = rng.Next(0, employementRatio!.Count);
            if (!orderJobs.Contains(chosedNum)){orderJobs.Add(chosedNum);}
        }while(!orderJobs.Contains(chosedNum));
        
    }
    int totalProportion = 0;
    
    foreach (int actualJobIndex in orderJobs)
    {
        bool correctPart = false;
        int proportionInPopulation;
        do
        {
            proportionInPopulation = rng.Next(employementRatio[actualJobIndex].min, employementRatio[actualJobIndex].max);
            if (totalProportion + proportionInPopulation <= 100)
            {
                correctPart = true;
            }
        } while (!correctPart);

        totalProportion += proportionInPopulation;
        int numberOfEmployed = (int)Math.Round((double)population * proportionInPopulation / 100.0);
        city.CitizensStats!.EmployementRatioAndEmployed[actualJobIndex] = (employementRatio![actualJobIndex].min, employementRatio[actualJobIndex].max, numberOfEmployed);
    }

    city.CitizensStats!.Unemployed = Math.Max(0, (int)Math.Round((double)population * (100 - totalProportion) / 100.0));
}

void PopulationGrowth()
{
    city.Society?.LifeExpectancy = CalculateLifeExpectancy();
    Death();
    Reproduction();
    city.CitizensStats.Population = city.CitizensStats.ageRepartition.Sum(x => x.numberOfCitizens);
}

void Reproduction()
{
    var (newNumberOfCitizens, newRatioMaxDeath, newRatioMaxReproduct) = (0,0,0);
    for (int i =1; i < city.CitizensStats?.ageRepartition.Count; i++)
    {
        int ratioTemp = rng.Next(0, (int)Math.Round(city.CitizensStats.ageRepartition[i].ratioMaxReproduct * 10));
        double ratioToReproduct = ratioTemp / 10.0;

        newNumberOfCitizens += (int)(city.CitizensStats.ageRepartition[i].numberOfCitizens * ((int)Math.Round(ratioToReproduct) / 100.0))+ populationBooster;
    }
    city.CitizensStats?.ageRepartition[0] = (newNumberOfCitizens, city.CitizensStats.ageRepartition[0].ratioMaxDeath, city.CitizensStats.ageRepartition[0].ratioMaxReproduct);
}

void Death()
{
    List<(int numberOfCitizens, double ratioMaxDeath, double ratioMaxReproduct)> newAgeRepartition = [(0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0)];
    for (int i = 0; i < city.CitizensStats?.ageRepartition.Count; i++)
    {
        var (numberOfCitizens, ratioMaxDeath, ratioMaxReproduct) = city.CitizensStats.ageRepartition[i];

        int ratioTemp = rng.Next(0, (int)Math.Round(ratioMaxDeath * 10));
        double ratioToDeath = ratioTemp / 10.0;

        int newNumberOfCitizens = Math.Max(0, (int)(numberOfCitizens - numberOfCitizens * (ratioToDeath / 100.0 + deathBooster / 100.0)));

        double newRatioMaxDeath;
        if (ratioMaxDeath<20){newRatioMaxDeath = ratioMaxDeath+5;}
        else if (ratioMaxDeath==20) {newRatioMaxDeath = ratioMaxDeath+10;}
        else {newRatioMaxDeath = ratioMaxDeath+15;}

        if (newRatioMaxDeath >50){newRatioMaxDeath = 50;}

        double newRatioMaxReproduct;
        if (i==0){newRatioMaxReproduct = ratioMaxReproduct+10;}
        else if (i<3) {newRatioMaxReproduct = ratioMaxReproduct+20;}
        else if(i<5) {newRatioMaxReproduct = ratioMaxReproduct-20;}
        else {newRatioMaxReproduct = ratioMaxReproduct-10;}

        if (newRatioMaxReproduct < 0) { newRatioMaxReproduct = 0; }

        if (i == city.CitizensStats.ageRepartition.Count - 1)
        {
            newAgeRepartition[i] = (newAgeRepartition[i].numberOfCitizens + newNumberOfCitizens, newRatioMaxDeath, newRatioMaxReproduct);
            break;
        }
        else
        {
            newAgeRepartition[i+1] = (newNumberOfCitizens, newRatioMaxDeath, newRatioMaxReproduct);
        }
    }
    city.CitizensStats?.ageRepartition = newAgeRepartition;
}

int CalculateLifeExpectancy()
{
    lifeExpextancy += city.Society.QualityOfLife;

    return lifeExpextancy;
}

string? SelectName()
{
    Console.Write("Choose the name of the city : ");
    string chosedName = Console.ReadLine() ?? string.Empty;
    return chosedName;
}

void DevTriggerEvent()
{

}

void DisplayCityInfos()
{
    Console.WriteLine($"+---------------{city.Name}---------------+");

    Console.WriteLine("");
    Console.WriteLine("          Citizens Stats         ");
    Console.WriteLine($"Population: {city.CitizensStats?.Population}");
    Console.WriteLine($"Age Repartition: ");
    for (int i = 0; i < city.CitizensStats?.ageRepartition.Count; i++)
    {
        var (numberOfCitizens, ratioMaxDeath, ratioMaxReproduct) = city.CitizensStats.ageRepartition[i];
        Console.WriteLine($"Age group {i * 10}-{(i + 1) * 10}: {numberOfCitizens} citizens, Max Death Ratio: {ratioMaxDeath}, Max Reproduction Ratio: {ratioMaxReproduct}");
    }
    Console.WriteLine($"Unemployed: {city.CitizensStats?.Unemployed}");
    Console.WriteLine($"Workers: {city.CitizensStats?.EmployementRatioAndEmployed[0].numberOfEmployed}");
    Console.WriteLine($"Scientists: {city.CitizensStats?.EmployementRatioAndEmployed[1].numberOfEmployed}");
    Console.WriteLine($"Soldiers: {city.CitizensStats?.EmployementRatioAndEmployed[2].numberOfEmployed}");
    Console.WriteLine($"Ingeniers: {city.CitizensStats?.EmployementRatioAndEmployed[3].numberOfEmployed}");

    Console.WriteLine("");
    Console.WriteLine("          City Traits         ");
    Console.WriteLine($"Militarism: {city.Traits?.Militarism}");
    Console.WriteLine($"Ambition: {city.Traits?.Ambition}");
    Console.WriteLine($"Diplomacy: {city.Traits?.Diplomacy}");
    Console.WriteLine($"Prudence: {city.Traits?.Prudence}");
    Console.WriteLine($"Progressiveness: {city.Traits?.Progressiveness}");
    Console.WriteLine($"Spirituality: {city.Traits?.Spirituality}");
    Console.WriteLine($"Conservatism: {city.Traits?.Conservatism}");

    Console.WriteLine("");
    Console.WriteLine("          Natural Resources         ");
    Console.WriteLine($"Water: {city.NaturalsResources?.Water}");
    Console.WriteLine($"Food: {city.NaturalsResources?.Food}");
    Console.WriteLine($"Wood: {city.NaturalsResources?.Wood}");
    Console.WriteLine($"Minerals: {city.NaturalsResources?.Minerals}");
    Console.WriteLine($"Energy: {city.NaturalsResources?.Energy}");

    Console.WriteLine("");
    Console.WriteLine("          Economy         ");
    Console.WriteLine($"Money: {city.Economy?.Money}");
    Console.WriteLine($"Reputation: {city.Economy?.Reputation}");

    Console.WriteLine("");
    Console.WriteLine("          Research         ");
    Console.WriteLine($"Technology Level: {city.Research?.TechnologyLevel}");

    Console.WriteLine("");
    Console.WriteLine("          Society         ");
    Console.WriteLine($"Happiness: {city.Society?.Happiness}");
    Console.WriteLine($"Global Health: {city.Society?.GlobalHealth}");
    Console.WriteLine($"Quality of Life: {city.Society?.QualityOfLife}");
    Console.WriteLine($"Life Expectancy: {city.Society?.LifeExpectancy}");
    Console.WriteLine($"Culture: {city.Society?.Culture}");
    Console.WriteLine($"Housing Capacity: {city.Society?.HousingCapacity}");

    Console.WriteLine("");
    Console.WriteLine($"Turn: {turn}");

    Console.WriteLine($"+-------------------------------+");

}
void DisplayGraph()
{
    double[] xs = Enumerable.Range(1, populationHistory.Count)
                         .Select(x => (double)x)
                         .ToArray();

    double[] ys = populationHistory
        .Select(x => (double)x)
        .ToArray();

    ScottPlot.Plot plot = new();

    plot.Add.Scatter(xs, ys);

    plot.XLabel("Turn");
    plot.YLabel("Population");
    plot.Title("Population evolution");

    plot.SavePng("C:\\Users\\User\\Desktop\\C# ALL\\Simulia\\Graphs\\population.png", 1000, 600);

    double[] xsFood = Enumerable.Range(1, foodHistory.Count)
                         .Select(x => (double)x)
                         .ToArray();
    double[] ysFood = foodHistory
        .Select(x => (double)x)
        .ToArray();
    
    ScottPlot.Plot plotFood = new();

    plotFood.Add.Scatter(xsFood, ysFood);

    plotFood.XLabel("Turn");
    plotFood.YLabel("Food");
    plotFood.Title("Food evolution");

    plotFood.SavePng("C:\\Users\\User\\Desktop\\C# ALL\\Simulia\\Graphs\\food.png", 1000, 600);

    double[] xsStockFood = Enumerable.Range(1, stockFoodHistory.Count)
                         .Select(x => (double)x)
                         .ToArray();
    double[] ysStockFood = stockFoodHistory
        .Select(x => (double)x)
        .ToArray();

    ScottPlot.Plot plotStockFood = new();
    plotStockFood.Add.Scatter(xsStockFood, ysStockFood);
    plotStockFood.XLabel("Turn");
    plotStockFood.YLabel("Stock Food");
    plotStockFood.Title("Stock Food evolution");
    plotStockFood.SavePng("C:\\Users\\User\\Desktop\\C# ALL\\Simulia\\Graphs\\stock_food.png", 1000, 600);

    double[] xsDeathBooster = Enumerable.Range(1, deathBoosterHistory.Count)
                         .Select(x => (double)x)
                         .ToArray();
    double[] ysDeathBooster = deathBoosterHistory
        .Select(x => (double)x)
        .ToArray();

    ScottPlot.Plot plotDeathBooster = new();
    plotDeathBooster.Add.Scatter(xsDeathBooster, ysDeathBooster);
    plotDeathBooster.XLabel("Turn");
    plotDeathBooster.YLabel("Death Booster");
    plotDeathBooster.Title("Death Booster evolution");
    plotDeathBooster.SavePng("C:\\Users\\User\\Desktop\\C# ALL\\Simulia\\Graphs\\death_booster.png", 1000, 600);

    double[] xsPopulationBooster = Enumerable.Range(1, populationBoosterHistory.Count)
                         .Select(x => (double)x)
                         .ToArray();
    double[] ysPopulationBooster = populationBoosterHistory
        .Select(x => (double)x)
        .ToArray();

    ScottPlot.Plot plotPopulationBooster = new();
    plotPopulationBooster.Add.Scatter(xsPopulationBooster, ysPopulationBooster);
    plotPopulationBooster.XLabel("Turn");
    plotPopulationBooster.YLabel("Population Booster");
    plotPopulationBooster.Title("Population Booster evolution");
    plotPopulationBooster.SavePng("C:\\Users\\User\\Desktop\\C# ALL\\Simulia\\Graphs\\population_booster.png", 1000, 600);

    double[] xsFoodVigilanceLevel = Enumerable.Range(1, foodVigilanceLevelHistory.Count)
                         .Select(x => (double)x)
                         .ToArray();
    double[] ysFoodVigilanceLevel = foodVigilanceLevelHistory
        .Select(x => (double)x)
        .ToArray();

    ScottPlot.Plot plotFoodVigilanceLevel = new();
    plotFoodVigilanceLevel.Add.Scatter(xsFoodVigilanceLevel, ysFoodVigilanceLevel);
    plotFoodVigilanceLevel.XLabel("Turn");
    plotFoodVigilanceLevel.YLabel("Food Vigilance Level");
    plotFoodVigilanceLevel.Title("Food Vigilance Level evolution");
    plotFoodVigilanceLevel.SavePng("C:\\Users\\User\\Desktop\\C# ALL\\Simulia\\Graphs\\food_vigilance_level.png", 1000, 600);

    double[] xsWorkers = Enumerable.Range(1, workersHistory.Count)
                         .Select(x => (double)x)
                         .ToArray();
    double[] ysWorkers = workersHistory
        .Select(x => (double)x)
        .ToArray();

    ScottPlot.Plot plotWorkers = new();
    plotWorkers.Add.Scatter(xsWorkers, ysWorkers);
    plotWorkers.XLabel("Turn");
    plotWorkers.YLabel("Workers");
    plotWorkers.Title("Workers evolution");
    plotWorkers.SavePng("C:\\Users\\User\\Desktop\\C# ALL\\Simulia\\Graphs\\workers.png", 1000, 600);
}