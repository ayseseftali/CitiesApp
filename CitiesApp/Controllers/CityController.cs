using CitiesApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace CitiesApp.Controllers
{
    public class CityController : Controller
    {
        private readonly ILogger<CityController> _logger;
        string baseUrl = "http://localhost:5246/api/";

        public CityController(ILogger<CityController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            DataTable dt = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("Cities");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<DataTable>(results);
                }

                ViewData.Model = dt;
            }

            return View();
        }

        public async Task<ActionResult<String>> AddCity(City city)
        {
            City obj = new City()
            {
                SehirAdi = city.SehirAdi,
                UlkeAdi = city.UlkeAdi
            };

            if (obj.SehirAdi != null && obj.UlkeAdi != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage postData = await client.PostAsJsonAsync("Cities/", obj);

                    if (postData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "City");
                    }


                }
            }

            return View();
        }

        public async Task<IActionResult> UpdateCity(int id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl + "Cities/" + id);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    var dt = JsonConvert.DeserializeObject<City>(results);
                    ViewData.Model = dt;
                }
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCity(City city)
        {
            City obj = new City()
            {
                Id = city.Id,
                SehirAdi = city.SehirAdi,
                UlkeAdi = city.UlkeAdi
            };

            if (obj.SehirAdi != null && obj.UlkeAdi != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl + "Cities/" + city.Id);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage updateData = await client.PutAsJsonAsync("", obj);

                    if (updateData.IsSuccessStatusCode)
                    {
                       return RedirectToAction("Index","City");
                    }
                }
            }


            return View();
        }

        public async Task<IActionResult> DeleteCity(int id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl + "Cities/" + id);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage deleteData = await client.DeleteAsync("");

                if (deleteData.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","City");
                }
            }


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}