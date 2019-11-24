using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static ecapi.Models.eci;

namespace ecapi.Api_handler
{
    public class Api_Handler_manager
    {
        //static string BASE_URL = "https://api.ers.usda.gov/data";

        public ApplicationDbContext dbContext;
        static string BASE_URL = "https://api.ers.usda.gov/data";
        static string API_KEY = "B77RPajCgNtGRkD2Sob9pLJxyI6bslvgV1C3HDcm"; //Add your API key here inside ""

        HttpClient httpClient;

        public Api_Handler_manager(ApplicationDbContext context)
        {
            dbContext = context;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        // public  List<Datum> GetDetails()
        public RootObject GetDetails()
        {
            //string NATIONAL_PARK_API_PATH = BASE_URL + "/arms/state"; //?limit=20
            string API_PATH = BASE_URL + "/arms/surveydata?&year=2015,2016&state=all&report=income+statement&farmtype=operator+households&category=collapsed+farm+typology&category_value=commercial";
            string Api_Data = "";

            RootObject rootobject = null;
           // List<Datum>datum = null;

            httpClient.BaseAddress = new Uri(API_PATH);

            // It can take a few requests to get back a prompt response, if the API has not received
            //  calls in the recent past and the server has put the service on hibernation
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    Api_Data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!Api_Data.Equals(""))
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    //   rootobject = JsonConvert.DeserializeObject<List<Datum>>(Api_Data);
                    rootobject = JsonConvert.DeserializeObject<RootObject>(Api_Data);
                }
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }
            
            return rootobject;
        }
    }
}

