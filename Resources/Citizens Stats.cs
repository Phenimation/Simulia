public class CitizensStats
{
    public int Population { get; set; } = 100;

    public class AgeRepartition
    {
        public int Children { get; set; } = 20;
        public int Teenagers { get; set; } = 20;
        public int Adults { get; set; } = 40;
        public int Elders { get; set; } = 20;
    }
    public int Unemployed { get; set; } = 100;
    public int Workers { get; set; } = 0;
    public int Scientists { get; set; } = 0;
    public int Soldiers { get; set; } = 0;
    public int Ingeniers { get; set; } = 0;
}