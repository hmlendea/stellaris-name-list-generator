using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class StationNames
    {
        public MilitaryStationNames MilitaryStations { get; set; }

        public StarbaseNames StarbaseNames { get; set; }

        public List<NameGroup> MiningStations { get; set; }
        public List<NameGroup> ResearchStations { get; set; }
        public List<NameGroup> ObservationStations { get; set; }

        public StationNames()
        {
            MilitaryStations = new MilitaryStationNames();

            StarbaseNames = new StarbaseNames();

            MiningStations = new List<NameGroup>();
            ResearchStations = new List<NameGroup>();
            ObservationStations = new List<NameGroup>();
        }
    }
}
