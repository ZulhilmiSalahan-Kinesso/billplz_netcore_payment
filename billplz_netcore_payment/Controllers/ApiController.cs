using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using billplz_netcore_payment.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace billplz_netcore_payment.Controllers
{
    public class ApiController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        public void CallApi() {

            var repositories = ProcessRepositories().Result;

            foreach (var repo in repositories)
                Console.WriteLine(repo.Name);
            
        }

        private static async Task<List<Repository>> ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
            var serializer = new DataContractJsonSerializer(typeof(List<Repository>));

            var msg = await stringTask;
            Console.Write(msg);

            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = serializer.ReadObject(await streamTask) as List<Repository>;

            foreach (var repo in repositories)
                Console.WriteLine(repo.Name);

            return repositories;
        }
    }
}
