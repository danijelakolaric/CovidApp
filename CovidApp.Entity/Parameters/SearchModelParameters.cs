using System;
using System.Collections.Generic;
using System.Text;

namespace CovidApp.Entity.Parameters
{
    public class SearchModelParameters
    {
        public int? ConfirmedMin { get; set; }
        public int? ConfirmedMax { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
