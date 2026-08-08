using System.Runtime.CompilerServices;

Random rng = new();

int lifeExpextancy = 30;
int turn =1;
int targetTurn = 50;
bool wantToChoseName = false;
string defaultName = "Ferdonia";

City city = new City();

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
        DisplayCityInfos();
        PopulationManagement();
        turn++;
    }
}

void PopulationManagement()
{
    PopulationGrowth();
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

        newNumberOfCitizens += (int)(city.CitizensStats.ageRepartition[i].numberOfCitizens * ((int)Math.Round(ratioToReproduct) / 100.0))+8;
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

        int newNumberOfCitizens = (int)(numberOfCitizens - numberOfCitizens * ((int)Math.Round(ratioToDeath) / 100.0));

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
    Console.WriteLine($"Workers: {city.CitizensStats?.Workers}");
    Console.WriteLine($"Scientists: {city.CitizensStats?.Scientists}");
    Console.WriteLine($"Soldiers: {city.CitizensStats?.Soldiers}");
    Console.WriteLine($"Ingeniers: {city.CitizensStats?.Ingeniers}");

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