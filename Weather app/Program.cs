using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Accuweather 
{
    internal class Program
    {
        private static string userInput;
        static string cityKey = "";
        static int weatherCity = 0;
        private static string key = "ZSibzxpT4hCNYDe1faz7j8His06i1lGk";
        private static string locationKeyUrl = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={key}&q=";
       // static public void foo(String c) { cityKey = c; }
        private static string weatherValueUrl = $"http://dataservice.accuweather.com/currentconditions/v1/{cityKey}?apikey={key}";

       
        static void Main(string[] args)
        {
            Console.WriteLine($"Provide city: {userInput}");
            userInput = Console.ReadLine();
            locationKeyUrl += userInput; 

            HttpClient clientLocation = new HttpClient();
            clientLocation.BaseAddress = new Uri(locationKeyUrl);

            clientLocation.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseLocation = clientLocation.GetAsync(locationKeyUrl).Result;

            if (responseLocation.IsSuccessStatusCode)
            {
                var jsonLocation = responseLocation.Content.ReadAsStringAsync().Result;
                dynamic lineLocation = JsonConvert.DeserializeObject(jsonLocation);

                cityKey = lineLocation[0]["Key"];
                weatherValueUrl = $"http://dataservice.accuweather.com/currentconditions/v1/{cityKey}?apikey={key}";
                HttpClient clientWeather = new HttpClient();
                clientWeather.BaseAddress = new Uri(weatherValueUrl);
                clientWeather.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseWeather = clientWeather.GetAsync(weatherValueUrl).Result;

                var jsonWeather = responseWeather.Content.ReadAsStringAsync().Result;
                dynamic lineWeather = JsonConvert.DeserializeObject(jsonWeather);

                weatherCity = lineWeather[0]["Temperature"]["Metric"]["Value"];

                Console.WriteLine($"The temperature in {userInput} is {weatherCity} degrees Celsius.");
            }
        }      
    }
}