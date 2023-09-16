using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using MartianTrail.Common;
using MartianTrail.Communications.WebApi;
using MartianTrail.Entities;
using MartianTrail.PlayerInteraction;

namespace MartianTrail.GamePhases
{
    public class SolData
    {
        public int Sol { get; set; }
        public decimal MaxTemp { get; set; }
        public decimal MinTemp { get; set; }
        public string UltraViolet { get; set; }
    }

    public class DisplayMartianWeather : IGamePhase
    {
        private readonly IFetchWebApiData _webApiClient;
        private int firstSol;
        private readonly Func<int, SolData?> _solLookup;

        public DisplayMartianWeather(IFetchWebApiData webApiClient)
        {
            _webApiClient = webApiClient;
            var data =
                this._webApiClient.FetchData<NasaMarsData>("https://mars.nasa.gov/rss/api/?feed=weather&category=msl&feedtype=json").Result
                    .Bind(x => x.soles.OrderByDescending(x => x.id).Take(300))
                    .Bind(x => x.Select(y => new SolData
                    {
                        Sol = int.TryParse(y.sol, out var i) ? i : -1,
                        MaxTemp = decimal.TryParse(y.max_temp, out var mt) ? mt : -1,
                        MinTemp = decimal.TryParse(y.min_temp, out var mt2) ? mt2 : -1,
                        UltraViolet = y.local_uv_irradiance_index
                    }).Where(x => x.Sol != -1 && x.MaxTemp != -1 && x.MinTemp != -1));

            var solData = data is Something<IEnumerable<SolData>> s ? s.Value.ToArray() : Array.Empty<SolData>();
            this.firstSol = solData.LastOrDefault()?.Sol ?? 0;

            this._solLookup = solData
                .ToDictionary(x => x.Sol, x => x)
                .ToLookupWithDefault();

        }

        private static string FormatMarsData(SolData sol) =>
            "Mars Sol " + sol.Sol + Environment.NewLine +
            "\tMin Temp: " + sol.MinTemp + Environment.NewLine +
            "\tMax Temp: " + sol.MaxTemp + Environment.NewLine +
            "\tUV Irradiance Index: " + sol.UltraViolet + Environment.NewLine;




        public GameState DoPhase(IPlayerInteraction playerInteraction, GameState oldState)
        {
            var currentSol = oldState.CurrentSol == 0 ? this.firstSol : oldState.CurrentSol;
            var currentSolData =   this._solLookup(currentSol);
            var formattedData = currentSolData == null ? "" : FormatMarsData(currentSolData);
            playerInteraction.WriteMessageConditional(!string.IsNullOrWhiteSpace(formattedData), formattedData);

            return oldState with
            {
                CurrentSol = currentSol + 1
            };
        }
    }

    public class NasaMarsData
    {
        public IEnumerable<NasaSolData> soles { get; set; }
    }

    public class NasaSolData
    {
        public string id { get; set; }
        public string sol { get; set; }
        public string max_temp { get; set; }
        public string min_temp { get; set; }
        public string local_uv_irradiance_index { get; set; }

    }

}
