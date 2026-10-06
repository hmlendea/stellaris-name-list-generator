using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class StationNames
    {
        public MilitaryStationNames MilitaryStations { get; set; }
        public List<NameGroup> MiningStations { get; set; }
        public List<NameGroup> ResearchStations { get; set; }
        public List<NameGroup> ObservationStations { get; set; }

        public StationNames()
        {
            MilitaryStations = new MilitaryStationNames();
            MiningStations = [];
            ResearchStations = [];
            ObservationStations = [];
        }
    }
}
