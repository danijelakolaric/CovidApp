using CovidApp.Entity.DTOs;
using CovidApp.Entity.Parameters;
using CovidApp.Services.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CovidApp.Services.Service
{
    public class CovidService : ICovidService
    {        
        private readonly HttpClient _httpClient;

        public CovidService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<CovidDTO>> GetCovidDataAsync(CovidParameters covidParameters)
        {
            var uriWithParameters = GetUri(covidParameters);

            var httpResponse = await _httpClient.GetAsync(uriWithParameters);

            var covidDataList = new List<CovidDTO>();

            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();

                covidDataList = JsonConvert.DeserializeObject<List<CovidDTO>>(content);
                return covidDataList;
            }

            return covidDataList;
        }

        private string GetUri(CovidParameters covidParameters)
        {
            var uriBuilder = new UriBuilder(_httpClient.BaseAddress.AbsoluteUri + covidParameters.Uri);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            if (covidParameters.From > DateTime.MinValue && covidParameters.To > DateTime.MinValue)
            {
                query["from"] = covidParameters.From.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'");
                query["to"] = covidParameters.To.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'");
                uriBuilder.Query = query.ToString();
            }
            
            return uriBuilder.ToString().Replace(_httpClient.BaseAddress.AbsoluteUri, "");
        }
    }
}
