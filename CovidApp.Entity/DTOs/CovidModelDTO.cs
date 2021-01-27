using CovidApp.Entity.Parameters;
using System.Collections.Generic;

namespace CovidApp.Entity.DTOs
{
    public class CovidModelDTO
    {
        public CovidModelDTO()
        {
            CovidData = new List<CovidDTO>();
            SearchModelParameters = new SearchModelParameters();
        }

        public List<CovidDTO> CovidData { get; set; }
        public SearchModelParameters SearchModelParameters { get; set; }
    }
}
