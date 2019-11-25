using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecapi.Models;
using System.Net.Http;
using static ecapi.Models.category;
using static ecapi.Models.eci;
using Newtonsoft.Json;
using static ecapi.Models.variable;

namespace ecapi.Controllers
{
    public class HomeController : Controller
    {
        //
        ApplicationDbContext dbContext;


        static string BASE_URL = "https://api.ers.usda.gov/data";
      

        static string API_KEY = "B77RPajCgNtGRkD2Sob9pLJxyI6bslvgV1C3HDcm"; //Add your API key here inside ""

          HttpClient httpClient;

        public ApplicationDbContext DbContext { get => dbContext; set => dbContext = value; }
        
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
            string API_PATH = BASE_URL + "/arms/surveydata?&year=2015,2016&state=all&report=income+statement&farmtype=operator+households&category=collapsed+farm+typology&category_value=commercial";
            string Api_Data = "";
            RootObject rootobject = null;
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
        public Base GetCategory()
        {

            string API_PATH = BASE_URL + "/arms/category?api_key=YOUR_API_KEY&id=age,ftypll" ; 
            string Api_Data = "";
                       
           Base rootobject = null;


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

                    rootobject = JsonConvert.DeserializeObject<Base>(Api_Data);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


            return rootobject;
        }


        public Base_a Getvariable()
        {

            string API_PATH = BASE_URL + "/arms/variable";
            string Api_Data = "";

            Base_a rootobjectb = null;

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

                    rootobjectb = JsonConvert.DeserializeObject<Base_a>(Api_Data);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


            return rootobjectb;
        }

        public IActionResult Index()
        {
           return View();
        }
        public IActionResult Parks()
        {
           ViewBag.dbSuccessComp = 0;
             RootObject rootobject = GetDetails();
             List<Datum>datums = rootobject.data;
             TempData["datums"] = JsonConvert.SerializeObject(datums);
                      
            return View(datums);
        }
        public IActionResult Var()
        {
            ViewBag.dbSuccessComp = 0;
            RootObject rootobject = GetDetails();
            List<Datum> datums = rootobject.data;
            TempData["datums"] = JsonConvert.SerializeObject(datums);
            return View(datums);

        }
        public IActionResult PopulateData()
        {
            List<Datum>datums = JsonConvert.DeserializeObject<List<Datum>>(TempData["datums"].ToString());
            
            foreach (Datum datum in datums)
            {
                if (dbContext.Datums.Where(c => c.variable_id.Equals(datum.variable_id)).Count() == 0)
                {
                    dbContext.Datums.Add(datum);
                }
            }
            //

            dbContext.SaveChanges();
            ViewBag.dbSuccessComp = 1;
            return View("Parks",datums);
        }
        public IActionResult Category()
        {
            ViewBag.dbSuccessComp = 0;
            Base rootobject1 = GetCategory();

            List<Base_Datum>base_datums =rootobject1.data;

            //TempData["datums"] = JsonConvert.SerializeObject(base_datums);

            return View(base_datums);
        }
        public IActionResult Variable()
        {
            ViewBag.dbSuccessComp = 0;
            Base_a rootobjecta = Getvariable();
            List<Base_aDatum> datumsa = rootobjecta.data;
            TempData["datumsa"] = JsonConvert.SerializeObject(datumsa);

            return View(datumsa);
        }
        public IActionResult Aboutus()
        {
            return View();
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
