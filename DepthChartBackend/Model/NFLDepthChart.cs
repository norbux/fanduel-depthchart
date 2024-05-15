using System.Text.Json.Serialization;

namespace DepthChart.Model
{
    public class NFLDepthChart
    {
        [JsonPropertyName("team")]
        public NFLTeam? Team { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, NFLPositionDepth> Data { get; set; } = [];

        public NFLDepthChart(NFLTeam team)
        {
            Team = team;
        }
    }
}