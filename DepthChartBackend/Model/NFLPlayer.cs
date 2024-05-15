namespace DepthChart.Model
{
    public class NFLPlayer(int number, string name)
    {
        public int Number { get; set; } = number;
        public string Name { get; set; } = name;
    }
}