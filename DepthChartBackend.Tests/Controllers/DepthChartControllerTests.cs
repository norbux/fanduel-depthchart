using FakeItEasy;
using DepthChart.Service;
using DepthChart.Controllers;
using DepthChart.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DepthChartBackend.Tests.Controllers
{
    public class DepthChartControllerTests
    {
        private readonly ILogger<DepthChartController> _logger;
        private readonly INFLDepthChartService _depthChartService;

        public DepthChartControllerTests()
        {
            _depthChartService = A.Fake<INFLDepthChartService>();
            _logger = A.Fake<ILogger<DepthChartController>>();
        }

        [Fact]
        public void Should_Return_Full_DepthChart()
        {
            NFLDepthChart returned = new NFLDepthChart(new NFLTeam("TB"));
            A.CallTo(() => _depthChartService.GetFullDepthChart()).Returns(returned);
            var controller = new DepthChartController(_depthChartService, _logger);

            var result = controller.GetFullDepthChart();

            result.Should().Be(result);
        }

        [Fact]
        public void Sould_Return_Backups()
        {
            var position = "QB";
            var player = new NFLPlayer(12, "Mike Evans");
            List<NFLPlayer> returned = new List<NFLPlayer>();
            var controller = new DepthChartController(_depthChartService, _logger);
            A.CallTo(() => _depthChartService.GetBackups(position, player)).Returns(returned);

            var result = controller.GetBackups(position, 12, "Cameron Brate");
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void Should_Delete_Player()
        {
            var position = "QB";
            var number = 12;
            var name = "Mike Evans";
            var controller = new DepthChartController(_depthChartService, _logger);

            var result = controller.DeletePlayer(position, number, name);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void Should_Add_Player()
        {
            var position = "QB";
            var number = 12;
            var name = "Mike Evans";
            var depth = 2;
            var controller = new DepthChartController(_depthChartService, _logger);

            var result = controller.AddPlayer(position, number, name, depth);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkResult));
        }
    }
}