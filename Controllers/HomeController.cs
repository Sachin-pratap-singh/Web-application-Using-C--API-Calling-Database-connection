using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecapi.Models;
using System.Net.Http;


using static ecapi.Models.eci;
//using ecapi.Api_handler;
using Newtonsoft.Json;

namespace ecapi.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext dbContext;
       // ApplicationDbContext Context;


        static string BASE_URL = "https://api.ers.usda.gov/data";
          static string API_KEY = "B77RPajCgNtGRkD2Sob9pLJxyI6bslvgV1C3HDcm"; //Add your API key here inside ""

          HttpClient httpClient;
   
        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
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
    
    

        
        public IActionResult Index()
        {
            //ViewBag.dbSuccessComp = 0;
           // Api_Handler_manager webHandler = new Api_Handler_manager(Context);
            // List<Datum> Datums1 = webHandler.GetDetails();
            //RootObject rootobject = webHandler.GetDetails();
            RootObject rootobject = GetDetails();
            return View(rootobject);
        }
        public IActionResult Parks()
        {
           // ViewBag.dbSuccessComp = 0;
            //Api_Handler_manager webHandler = new Api_Handler_manager(Context);

           // RootObject rootobject = webHandler.GetDetails();
             RootObject rootobject = GetDetails();
             List<Datum>datums = rootobject.data;
             TempData["datums"] = JsonConvert.SerializeObject(datums);
            //List<Datum>Datums1 = webHandler.GetDetails();
            //TempData["RootObject"] = JsonConvert.SerializeObject(rootobject);


            //return View(rootobject);
            return View(datums);
        }
        public IActionResult PopulateSymbols()
        {
            List<Datum>datums = JsonConvert.DeserializeObject<List<Datum>>(TempData["datums"].ToString());
            foreach (Datum datum in datums)
            {
                if (dbContext.Datums.Where(c => c.variable_id.Equals(datum.variable_id)).Count() == 0)
                {
                    dbContext.Datums.Add(datum);
                }
            }

            dbContext.SaveChanges();
            ViewBag.dbSuccessComp = 1;
            return View("Parks",datums);
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
