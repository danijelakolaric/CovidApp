using CovidApp.Entity.DTOs;
using CovidApp.Entity.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CovidApp.Services.Interface
{
    public interface ICovidService
    {
        Task<List<CovidDTO>> GetCovidDataAsync(CovidParameters covidParameters);
    }
}
