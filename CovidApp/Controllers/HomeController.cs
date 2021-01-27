using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CovidApp.Entity.DTOs;
using CovidApp.Entity.Models;
using CovidApp.Entity.Parameters;
using CovidApp.Persistence;
using CovidApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CovidApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICovidService _covidService;
        private readonly CovidDbContext _covidDbContext;
        private const string uriDayOneTotal = "total/dayone/country/croatia";
        private const string uriByCountryTotal = "/country/croatia";

        public HomeController(ILogger<HomeController> logger, ICovidService covidService, CovidDbContext covidDbContext)
        {
            _logger = logger;
            _covidService = covidService;
            _covidDbContext = covidDbContext;
        }
        private async Task<List<CovidDTO>> GetCovidAsync(CovidParameters covidParameters)
        {
            return await _covidService.GetCovidDataAsync(covidParameters);
        }

        [HttpGet]
        public IActionResult Index(CovidModelDTO covidModel)
        {
            return View(covidModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SearchModelParameters searchModelParameters)
        {
            var saved = await SaveCovidDataIntoDb();

            var covidData = _covidDbContext.CovidData.ToList();

            covidData = covidData.Where(d => FilterDataBySearchModelParameters(d, searchModelParameters)).OrderByDescending(d => d.Date).ToList();

            var covidModel = new CovidModelDTO();
            covidModel.CovidData = covidData;
            covidModel.SearchModelParameters = searchModelParameters;

            return View(covidModel);
        }

        private async Task<bool> SaveCovidDataIntoDb()
        {
            var covidParameters = new CovidParameters();
            var haveToSaveData = false;

            if (_covidDbContext.CovidData.Count() == 0)
            {
                covidParameters.Uri = uriDayOneTotal;
                haveToSaveData = true;
            }
            else
            {
                var lastSavedDate = _covidDbContext.CovidData.OrderByDescending(d => d.Date).First().Date;

                if (lastSavedDate < DateTime.Now)
                {
                    covidParameters.Uri = uriByCountryTotal;
                    covidParameters.From = lastSavedDate.AddDays(1);
                    covidParameters.To = DateTime.Now;
                    haveToSaveData = true;
                }
            }

            if (haveToSaveData)
            {
                var covidData = await GetCovidAsync(covidParameters);

                if (covidData.Count > 0)
                {
                    covidData.OrderByDescending(d => d.Date);
                    _covidDbContext.AddRange(covidData);
                    _covidDbContext.SaveChanges();
                    return true;
                }

                return false;
            }

            return false;
        }

        private bool FilterDataBySearchModelParameters(CovidDTO covidData, SearchModelParameters searchModelParameters)
        {
            var valid = true;

            if (searchModelParameters.FromDate.HasValue)
            {
                valid = valid && covidData.Date >= searchModelParameters.FromDate;
            }

            if (searchModelParameters.ToDate.HasValue)
            {
                valid = valid && covidData.Date <= searchModelParameters.ToDate;
            }

            if (searchModelParameters.ConfirmedMin.HasValue)
            {
                valid = valid && covidData.Confirmed >= searchModelParameters.ConfirmedMin;
            }

            if (searchModelParameters.ConfirmedMax.HasValue)
            {
                valid = valid && covidData.Confirmed <= searchModelParameters.ConfirmedMax;
            }

            return valid;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
