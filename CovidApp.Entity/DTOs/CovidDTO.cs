using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CovidApp.Entity.DTOs
{
    public class CovidDTO
    {
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        [DisplayName("Confirmed")]
        public int Confirmed { get; set; }
        [DisplayName("Deaths")]
        public int Deaths { get; set; }
        [DisplayName("Recovered")]
        public int Recovered { get; set; }
        [DisplayName("Active")]
        public int Active { get; set; }
        [Key]
        [DisplayName("Date")]
        public DateTime Date { get; set; }
    }
}
