public class CitizensStats
{
    public int Population { get; set; } = 100;
    public List<(int numberOfCitizens, double ratioMaxDeath, double ratioMaxReproduct)> ageRepartition = new List<(int numberOfCitizens, double rationMaxDeath, double ratioMaxReproduct)>()
    {
        (10, 0, 0),//0-10
        (20, 5, 10),//10-20
        (30, 10, 30),//20-30
        (20, 15, 50),//30-40
        (10, 20, 30),//40-50
        (10, 30, 10),//50-60
        (0, 45, 0)//60+
    };
    public int Unemployed { get; set; } = 100;
    public List<(int min, int max, int numberOfEmployed)> EmployementRatioAndEmployed = new List<(int min, int max, int numberOfEmployed)>()
        {
            //public double Unemployed { get; set; } = 0.1;
            (20, 45, 0),//Workers
            (10, 35, 0),//Scientists
            (15, 25, 0),//Soldiers
            (5, 15, 0)//Ingeniers

        };
    
}

