using Newtonsoft.Json;
using ProfileAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ProfileAPI.Services
{
    public class PeopleGraphService
    {
        private HttpClient graphClient;

        private const string URL = "https://graph.microsoft.com/v1.0/users";

        // TODO: remove hardcoded token
        private string token = "";

        public PeopleGraphService(Configuration.IProfileDatabaseSettings settings)
        {
            graphClient = new HttpClient();
            graphClient.BaseAddress = new Uri(URL);
            graphClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));         
        }

        public List<Profile> Get()
        {
            return new List<Profile>();
        }

        public Profile Get(string upn)
        {
            return GetProfile(upn);
        }

        public Profile GetProfile(string upn)
        {
            try
            {
                var url = URL + "('" + upn + "')";
                graphClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = graphClient.GetAsync(new Uri(url)).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync();
                    var profile = JsonConvert.DeserializeObject<Profile>(json.Result);
                    return profile;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
