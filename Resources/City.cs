public class City
{
    public string? Name { get; set; }
    public CitizensStats CitizensStats { get; set; } = new CitizensStats();
    public NaturalsResources? NaturalsResources { get; set; } = new NaturalsResources();
    public Society? Society { get; set; }
    public Research? Research { get; set; }
    public Economy? Economy { get; set; }
    public CityTraits? Traits { get; set; }
}