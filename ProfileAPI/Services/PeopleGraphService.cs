namespace ProfileAPI.Services
{
    using Newtonsoft.Json;
    using ProfileAPI.Models;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class PeopleGraphService
    {
        private HttpClient graphClient;

        private const string URL = "https://graph.microsoft.com/";

        public PeopleGraphService(Configuration.IProfileDatabaseSettings settings)
        {
            graphClient = new HttpClient();
            graphClient.BaseAddress = new Uri(URL);
            graphClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Profile> GetProfileAsync(string token)
        {
            graphClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
            var response = await graphClient.GetAsync("/v1.0/me");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Profile>(json);
            }
            else
            {   
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.ReasonPhrase);
                throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<Profile> GetAsync(string token, string upn)
        {
            return await GetProfileAsync(token, upn);
        }

        public async Task<Profile> GetProfileAsync(string token, string upn)
        {
            try
            {
                graphClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);

                var response = await graphClient.GetAsync("/v1.0/users/" + upn);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var profile = JsonConvert.DeserializeObject<Profile>(json);
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
