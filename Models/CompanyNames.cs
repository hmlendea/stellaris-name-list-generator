using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class CompanyNames
    {
        public List<NameGroup> RobotManufacturers { get; set; }
        public List<NameGroup> AutomotiveManufacturers { get; set; }
        public List<NameGroup> AircraftManufacturers { get; set; }
        public List<NameGroup> SpacecraftManufacturers { get; set; }
        public List<NameGroup> WeaponManufacturers { get; set; }

        public List<NameGroup> RocketDesigners { get; set; }

        public List<NameGroup> ResearchCompanies { get; set; }
        public List<NameGroup> InvestmentCompanies { get; set; }

        public CompanyNames()
        {
            RobotManufacturers = new List<NameGroup>();
            AutomotiveManufacturers = new List<NameGroup>();
            AircraftManufacturers = new List<NameGroup>();
            SpacecraftManufacturers = new List<NameGroup>();
            WeaponManufacturers = new List<NameGroup>();

            RocketDesigners = new List<NameGroup>();

            ResearchCompanies = new List<NameGroup>();
            InvestmentCompanies = new List<NameGroup>();
        }
    }
}
