
using DepthChart.Model;
using DepthChart.Service;
using DepthChart.Exceptions;
using System.Text.Json;
using System.IO;

namespace DepthChart.Service
{
    public class NFLDepthChartService : INFLDepthChartService
    {
        private NFLDepthChart _depthChart;

        public NFLDepthChartService()
        {
            _depthChart = new NFLDepthChart(new NFLTeam("TB"));
        }

        public void Initialize()
        {
            try
            {
                using StreamReader reader = new(Path.Combine("./", "data.json"));
                string data = reader.ReadToEnd();

                var output = JsonSerializer.Deserialize<NFLDepthChart>(data);

                if (output != null)
                {
                    _depthChart = output;
                }
            }
            catch
            {
                _depthChart = new NFLDepthChart(new NFLTeam("TB"));
                var p1 = new NFLPlayer(11, "Mike Evans");
                var p2 = new NFLPlayer(14, "Tyler Johnson");
                var p3 = new NFLPlayer(15, "Donovan Smith");
                var p4 = new NFLPlayer(16, "Ali Marpet");

                AddPlayerToDepthChart(NFLPosition.QB.ToString(), p1, null);
                AddPlayerToDepthChart(NFLPosition.QB.ToString(), p2, null);
                AddPlayerToDepthChart(NFLPosition.QB.ToString(), p3, null);
                AddPlayerToDepthChart(NFLPosition.QB.ToString(), p4, null);

                var jsonFile = JsonSerializer.Serialize(_depthChart);
                File.WriteAllText(Path.Combine("./", "data.json"), jsonFile);
            }

        }

        public void AddPlayerToDepthChart(string position, NFLPlayer player, int? depth)
        {
            // Check if position exists, if not, create it
            if (!_depthChart.Data.ContainsKey(position))
            {
                // In case the supplied position is not a valid NFL position, return with no error or effect.
                // This is unspecified behaviour but just adding this validation to avoid upstream exceptions.
                if (!Enum.GetNames<NFLPosition>().Where(p => p == position).Any())
                {
                    return;
                }

                var pos = Enum.Parse<NFLPosition>(position);
                _depthChart.Data.Add(position, new NFLPositionDepth());
            }

            // Get position
            NFLPositionDepth? positionList;
            _depthChart.Data.TryGetValue(position, out positionList);

            // Depth validation
            var listLen = positionList?.List.Count;

            if (depth < 0 || depth > listLen + 1)
            {
                throw new DepthOutOfRangeException("Depth out of range");
            }

            // Get data from position
            var p = positionList?.List
                .Where(x => x.Number == player.Number && x.Name == player.Name)
                .FirstOrDefault();
            
            // If player was already there, remove it (it cannot be more that once in one position)
            if (p != null)
            {
                positionList?.List.Remove(p);
            }

            // Add the player at the position it belongs
            string jsonFile;

            if (depth != null)
            {
                positionList?.List.Insert(depth.Value - 1, player);
                
                jsonFile = JsonSerializer.Serialize(_depthChart);
                File.WriteAllText(Path.Combine("./", "data.json"), jsonFile);
                
                return;
            }

            positionList?.List.Add(player);

            // TODO: Persist data in a more robust storage
            jsonFile = JsonSerializer.Serialize(_depthChart);
            File.WriteAllText(Path.Combine("./", "data.json"), jsonFile);
        }

        public List<NFLPlayer> GetBackups(string position, NFLPlayer player)
        {
            var result = new List<NFLPlayer>();
            
            // Check if position and player exist
            if (!_depthChart.Data.ContainsKey(position))
            {
                return result;
            }

            // Check if player is in that position
            NFLPositionDepth? positionList;
            _depthChart.Data.TryGetValue(position, out positionList);

            if (positionList != null)
            {
                var index = positionList.List
                    .FindIndex(x => x.Number == player.Number && x.Name == player.Name);

                if (index < 0)
                {
                    return result;
                }

                result = positionList.List.GetRange(index+1, positionList.List.Count - index-1);
                
                return result;
            }

            return result;
        }

        public NFLDepthChart GetFullDepthChart()
        {
            try
            {
                using StreamReader reader = new("./data.json");
                string data = reader.ReadToEnd();

                var output = JsonSerializer.Deserialize<NFLDepthChart>(data);

                if (output != null)
                {
                    _depthChart = output;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return _depthChart;
        }

        public NFLPlayer? RemovePlayerFromDepthChart(string position, NFLPlayer player)
        {
            // Check if position and player exist
            if (!_depthChart.Data.ContainsKey(position))
            {
                return null;
            }

            // Check if player is in that position
            NFLPositionDepth? positionList;
            _depthChart.Data.TryGetValue(position, out positionList);

            if (positionList != null)
            {
                var p = positionList.List
                    .Where(x => x.Number == player.Number && x.Name == player.Name)
                    .FirstOrDefault();
                
                if (p != null)
                {
                    var chart = _depthChart.Data[position];
                    var list = chart.List;
                    list.Remove(p);
                    _depthChart.Data[position].List = list;
                    
                    var jsonFile = JsonSerializer.Serialize(_depthChart);
                    File.WriteAllText(Path.Combine("./", "data.json"), jsonFile);
                    
                    return p;
                }
            }

            // TODO: Persist data in a mor robust storage
            return null;
        }
    }
}