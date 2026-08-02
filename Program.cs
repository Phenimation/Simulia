using System.Runtime.CompilerServices;

Random rng = new();

int lifeExpextancy = 30;
int turn =1;

City city = new City();

Start();

void Start()
{
    city.Name = SelectName();
    DisplayCityInfos();
    PopulationManagement();
}

void PopulationManagement()
{
    PopulationGrowth();
}

void PopulationGrowth()
{
    city.Society?.LifeExpectancy = CalculateLifeExpectancy();
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