using DepthChart.Service;
using DepthChart.Model;
using FluentAssertions;

namespace DepthChartBackend.Tests.Service
{
    public class NFLDepthChartServiceTests
    {
        private NFLDepthChartService service;

        public NFLDepthChartServiceTests()
        {
            service = new NFLDepthChartService();
            service.Initialize();
        }

        [Fact]
        public void Should_Return_Full_DepthChart()
        {
            var result = service.GetFullDepthChart();
            var qb = result.Data.ContainsKey("QB");

            qb.Should().BeTrue();
        }

        [Fact]
        public void Should_Add_Player()
        {
            service.AddPlayerToDepthChart("LB", new NFLPlayer(1, "Joe Montana"), null);
            var result = service.GetFullDepthChart();
            var qb = result.Data.ContainsKey("QB");
            var lb = result.Data.ContainsKey("LB");

            qb.Should().BeTrue();
            lb.Should().BeTrue();
        }

        [Fact]
        public void Should_Remove_Player()
        {
            if (File.Exists("./data.json"))
            {
                File.Delete("./data.json");
            }

            service.Initialize();
            var chart = service.GetFullDepthChart();

            _ = chart.Data.TryGetValue("QB", out NFLPositionDepth? listBefore);
            var countBefore = listBefore?.List.Count ?? 0;
            _ = service.RemovePlayerFromDepthChart("QB", new NFLPlayer(15, "Donovan Smith"));

            _ = chart.Data.TryGetValue("QB", out NFLPositionDepth? listAfter);
            var countAfter = listAfter?.List.Count ?? 0;

            countBefore.Should().BeGreaterThan(countAfter);

        }
    }
}