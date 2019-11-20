using MongoDB.Driver;
using Newtonsoft.Json;
using ProfileAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProfileAPI.Services
{
    public class PeopleGraphService
    {
        private readonly IMongoCollection<Profile> _profiles;

        private HttpClient graphClient;

        private const string URL = "https://graph.microsoft.com/v1.0/users";

        // TODO: remove hardcoded token
        private string token = "eyJ0eXAiOiJKV1QiLCJub25jZSI6InE5RENkb1dmOVgya1dCMHpIQ0R4dlZFNk5aemRkcC1JOGhwVndEbXhGMTQiLCJhbGciOiJSUzI1NiIsIng1dCI6IkJCOENlRlZxeWFHckdOdWVoSklpTDRkZmp6dyIsImtpZCI6IkJCOENlRlZxeWFHckdOdWVoSklpTDRkZmp6dyJ9.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC83MmY5ODhiZi04NmYxLTQxYWYtOTFhYi0yZDdjZDAxMWRiNDcvIiwiaWF0IjoxNTc0MjMzNTE5LCJuYmYiOjE1NzQyMzM1MTksImV4cCI6MTU3NDIzNzQxOSwiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IkFWUUFxLzhOQUFBQUoxbUZoelMvL1FQY2hxVndYQnlWdTJtMURiZU5Gb1VydjQzVmlqV2tRa1pKcFVrZUxGdVYxdnFmNW9tTFdReWtxdXlhYkkwRGdnT0UxaWsrSS9ONmpvenFPR3NYalRhbjRnTXZCUVk1MXhJPSIsImFtciI6WyJ3aWEiLCJtZmEiXSwiYXBwX2Rpc3BsYXluYW1lIjoiR3JhcGggZXhwbG9yZXIiLCJhcHBpZCI6ImRlOGJjOGI1LWQ5ZjktNDhiMS1hOGFkLWI3NDhkYTcyNTA2NCIsImFwcGlkYWNyIjoiMCIsImNvbnRyb2xzIjpbImFwcF9yZXMiXSwiY29udHJvbHNfYXVkcyI6WyIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAiLCIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiXSwiZmFtaWx5X25hbWUiOiJCYWtvdmljIiwiZ2l2ZW5fbmFtZSI6Ik1haWRhIiwiaXBhZGRyIjoiMTY3LjIyMC4xOTcuMTIiLCJuYW1lIjoiTWFpZGEgQmFrb3ZpYyIsIm9pZCI6Ijc2Y2VkZDY0LTA5NDQtNDI0YS04NTFiLWVkYjRmM2VhMjhiMSIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS0xNzIxMjU0NzYzLTQ2MjY5NTgwNi0xNTM4ODgyMjgxLTQwOTMyOTkiLCJwbGF0ZiI6IjMiLCJwdWlkIjoiMTAwMzIwMDAzREI4RjE3NyIsInJoIjoiSSIsInNjcCI6IkNhbGVuZGFycy5SZWFkV3JpdGUgQ29udGFjdHMuUmVhZFdyaXRlIERldmljZU1hbmFnZW1lbnRBcHBzLlJlYWQuQWxsIERldmljZU1hbmFnZW1lbnRBcHBzLlJlYWRXcml0ZS5BbGwgRGV2aWNlTWFuYWdlbWVudENvbmZpZ3VyYXRpb24uUmVhZC5BbGwgRGV2aWNlTWFuYWdlbWVudENvbmZpZ3VyYXRpb24uUmVhZFdyaXRlLkFsbCBEZXZpY2VNYW5hZ2VtZW50TWFuYWdlZERldmljZXMuUHJpdmlsZWdlZE9wZXJhdGlvbnMuQWxsIERldmljZU1hbmFnZW1lbnRNYW5hZ2VkRGV2aWNlcy5SZWFkLkFsbCBEZXZpY2VNYW5hZ2VtZW50TWFuYWdlZERldmljZXMuUmVhZFdyaXRlLkFsbCBEZXZpY2VNYW5hZ2VtZW50UkJBQy5SZWFkLkFsbCBEZXZpY2VNYW5hZ2VtZW50UkJBQy5SZWFkV3JpdGUuQWxsIERldmljZU1hbmFnZW1lbnRTZXJ2aWNlQ29uZmlnLlJlYWQuQWxsIERldmljZU1hbmFnZW1lbnRTZXJ2aWNlQ29uZmlnLlJlYWRXcml0ZS5BbGwgRGlyZWN0b3J5LkFjY2Vzc0FzVXNlci5BbGwgRGlyZWN0b3J5LlJlYWRXcml0ZS5BbGwgRmlsZXMuUmVhZFdyaXRlLkFsbCBHcm91cC5SZWFkV3JpdGUuQWxsIElkZW50aXR5Umlza0V2ZW50LlJlYWQuQWxsIE1haWwuUmVhZFdyaXRlIE1haWxib3hTZXR0aW5ncy5SZWFkV3JpdGUgTm90ZXMuUmVhZFdyaXRlLkFsbCBvcGVuaWQgUGVvcGxlLlJlYWQgcHJvZmlsZSBSZXBvcnRzLlJlYWQuQWxsIFNpdGVzLlJlYWRXcml0ZS5BbGwgVGFza3MuUmVhZFdyaXRlIFVzZXIuUmVhZEJhc2ljLkFsbCBVc2VyLlJlYWRXcml0ZSBVc2VyLlJlYWRXcml0ZS5BbGwgZW1haWwiLCJzaWduaW5fc3RhdGUiOlsiaW5rbm93bm50d2siLCJrbXNpIl0sInN1YiI6InBzQVh3RDdMSWlObVg1bUd4bFB6ZlQ2cmprWDhja0xzdVRjNmdvUUh4TVUiLCJ0aWQiOiI3MmY5ODhiZi04NmYxLTQxYWYtOTFhYi0yZDdjZDAxMWRiNDciLCJ1bmlxdWVfbmFtZSI6Im1hYmFrb3ZpQG1pY3Jvc29mdC5jb20iLCJ1cG4iOiJtYWJha292aUBtaWNyb3NvZnQuY29tIiwidXRpIjoiT0FJbkNmOFZwMGFFWURFbFR4NEhBQSIsInZlciI6IjEuMCIsInhtc19zdCI6eyJzdWIiOiJXYkdxOFZ0djE0SDF2OWNkSURKM1hERmNnMDBtUzZ4dnNoVXNINktxczBjIn0sInhtc190Y2R0IjoxMjg5MjQxNTQ3fQ.KdktbpmXO2p0dRdBIrW7GUjwWEQH787gcg6L9Bl5regZWgBefUdsa0aHeMDntNgOGww0bhuK_xJt4566Jiz09z3U8vKBAOCkVvE6g_mmWCMMDe77HCvx-Q-gtd5nMkTf3381AWPquWPs_sMXCdsRO2eJmBR_AE1Okg3EcwBNOeamrVzv85hDVwa2f7AYkXQA3q_b7GyVMuz2P2cwPfH91SG1YCdek1f1tWIin3JBa1J5iJJb7Ka197tYGcr39Bg3pe7ngNge7tnUSZrUceIO8VOLSExX0gOOLAOc8Mm_6_E_zlJuc7VH-cWBM9xqHYIhc2iyGiQHTUtLRDbp-E5-NQ";

        public PeopleGraphService(Configuration.IProfileDatabaseSettings settings)
        {
            graphClient = new HttpClient();
            graphClient.BaseAddress = new Uri(URL);

            graphClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _profiles = database.GetCollection<Profile>(settings.ProfileCollectionName);
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
