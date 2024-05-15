using DepthChart.Model;

namespace DepthChart.Service
{
    public interface INFLDepthChartService
    {
        public void AddPlayerToDepthChart(string positon, NFLPlayer player, int? depth);
        public NFLPlayer? RemovePlayerFromDepthChart(string positon, NFLPlayer player);
        public List<NFLPlayer> GetBackups(string positon, NFLPlayer player);
        public NFLDepthChart GetFullDepthChart();
        public void Initialize();
    }
}