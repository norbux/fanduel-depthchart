using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DepthChart.Service;
using DepthChart.Exceptions;
using DepthChart.Model;

namespace DepthChart.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepthChartController : ControllerBase
    {
        private readonly INFLDepthChartService _depthChartService;
        public DepthChartController(INFLDepthChartService depthChartService)
        {
            _depthChartService = depthChartService;
            _depthChartService.Initialize();
        }

        [HttpGet]
        public ActionResult<NFLDepthChart> GetFullDepthChart()
        {
            return new JsonResult(_depthChartService.GetFullDepthChart());
        }

        [HttpGet("{position}/{number}/{name}")]
        public IActionResult GetBackups(string position, int number, string name)
        {
            var player = new NFLPlayer(number, name);
            var result = _depthChartService.GetBackups(position, player);

            return Ok(result);
        }

        [HttpDelete("{position}/{number}/{name}")]
        public IActionResult DeletePlayer(string position, int number, string name)
        {
            var player = new NFLPlayer(number, name);
            var result = _depthChartService.RemovePlayerFromDepthChart(position, player);
            
            if (result == null)
            {
                return new JsonResult("{}");
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddPlayer(string positon, int number, string name, int? depth)
        {
            var player = new NFLPlayer(number, name);

            try
            {
                _depthChartService.AddPlayerToDepthChart(positon, player, depth);
            }
            catch (DepthOutOfRangeException ex)
            {
                return BadRequest($"The supplied position depth is incorrect: {ex.Message}");
            }

            return Ok();
        }
    }
}