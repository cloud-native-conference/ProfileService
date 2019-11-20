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
        private string token = "eyJ0eXAiOiJKV1QiLCJub25jZSI6InFzY3lSNWdPY0ZDM3ZqeVJ1T1JfREtuMHpLYzdwTzBRaEFVMk9HOUhUdDgiLCJhbGciOiJSUzI1NiIsIng1dCI6IkJCOENlRlZxeWFHckdOdWVoSklpTDRkZmp6dyIsImtpZCI6IkJCOENlRlZxeWFHckdOdWVoSklpTDRkZmp6dyJ9.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC83MmY5ODhiZi04NmYxLTQxYWYtOTFhYi0yZDdjZDAxMWRiNDcvIiwiaWF0IjoxNTc0MjQxODAwLCJuYmYiOjE1NzQyNDE4MDAsImV4cCI6MTU3NDI0NTcwMCwiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IkFWUUFxLzhOQUFBQUpQYUpXZlF1L3pyV21ZMnNkalJiSzBmYmd3WHBFSVd0alNnSWdmak1BajhUTTJiQ0FMRHZHeG9oY1VhbVhqN0ZKUWZUNlFRclV1ZUVGYnBucEVQOUVVTWMrNHZjVmNJTWRaMlRNdWlwOXlVPSIsImFtciI6WyJ3aWEiLCJtZmEiXSwiYXBwX2Rpc3BsYXluYW1lIjoiR3JhcGggZXhwbG9yZXIiLCJhcHBpZCI6ImRlOGJjOGI1LWQ5ZjktNDhiMS1hOGFkLWI3NDhkYTcyNTA2NCIsImFwcGlkYWNyIjoiMCIsImNvbnRyb2xzIjpbImFwcF9yZXMiXSwiY29udHJvbHNfYXVkcyI6WyIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAiLCIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiXSwiZmFtaWx5X25hbWUiOiJCYWtvdmljIiwiZ2l2ZW5fbmFtZSI6Ik1haWRhIiwiaXBhZGRyIjoiMTY3LjIyMC4xOTcuMTIiLCJuYW1lIjoiTWFpZGEgQmFrb3ZpYyIsIm9pZCI6Ijc2Y2VkZDY0LTA5NDQtNDI0YS04NTFiLWVkYjRmM2VhMjhiMSIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS0xNzIxMjU0NzYzLTQ2MjY5NTgwNi0xNTM4ODgyMjgxLTQwOTMyOTkiLCJwbGF0ZiI6IjMiLCJwdWlkIjoiMTAwMzIwMDAzREI4RjE3NyIsInJoIjoiSSIsInNjcCI6IkNhbGVuZGFycy5SZWFkV3JpdGUgQ29udGFjdHMuUmVhZFdyaXRlIERldmljZU1hbmFnZW1lbnRBcHBzLlJlYWQuQWxsIERldmljZU1hbmFnZW1lbnRBcHBzLlJlYWRXcml0ZS5BbGwgRGV2aWNlTWFuYWdlbWVudENvbmZpZ3VyYXRpb24uUmVhZC5BbGwgRGV2aWNlTWFuYWdlbWVudENvbmZpZ3VyYXRpb24uUmVhZFdyaXRlLkFsbCBEZXZpY2VNYW5hZ2VtZW50TWFuYWdlZERldmljZXMuUHJpdmlsZWdlZE9wZXJhdGlvbnMuQWxsIERldmljZU1hbmFnZW1lbnRNYW5hZ2VkRGV2aWNlcy5SZWFkLkFsbCBEZXZpY2VNYW5hZ2VtZW50TWFuYWdlZERldmljZXMuUmVhZFdyaXRlLkFsbCBEZXZpY2VNYW5hZ2VtZW50UkJBQy5SZWFkLkFsbCBEZXZpY2VNYW5hZ2VtZW50UkJBQy5SZWFkV3JpdGUuQWxsIERldmljZU1hbmFnZW1lbnRTZXJ2aWNlQ29uZmlnLlJlYWQuQWxsIERldmljZU1hbmFnZW1lbnRTZXJ2aWNlQ29uZmlnLlJlYWRXcml0ZS5BbGwgRGlyZWN0b3J5LkFjY2Vzc0FzVXNlci5BbGwgRGlyZWN0b3J5LlJlYWRXcml0ZS5BbGwgRmlsZXMuUmVhZFdyaXRlLkFsbCBHcm91cC5SZWFkV3JpdGUuQWxsIElkZW50aXR5Umlza0V2ZW50LlJlYWQuQWxsIE1haWwuUmVhZFdyaXRlIE1haWxib3hTZXR0aW5ncy5SZWFkV3JpdGUgTm90ZXMuUmVhZFdyaXRlLkFsbCBvcGVuaWQgUGVvcGxlLlJlYWQgcHJvZmlsZSBSZXBvcnRzLlJlYWQuQWxsIFNpdGVzLlJlYWRXcml0ZS5BbGwgVGFza3MuUmVhZFdyaXRlIFVzZXIuUmVhZEJhc2ljLkFsbCBVc2VyLlJlYWRXcml0ZSBVc2VyLlJlYWRXcml0ZS5BbGwgZW1haWwiLCJzaWduaW5fc3RhdGUiOlsiaW5rbm93bm50d2siLCJrbXNpIl0sInN1YiI6InBzQVh3RDdMSWlObVg1bUd4bFB6ZlQ2cmprWDhja0xzdVRjNmdvUUh4TVUiLCJ0aWQiOiI3MmY5ODhiZi04NmYxLTQxYWYtOTFhYi0yZDdjZDAxMWRiNDciLCJ1bmlxdWVfbmFtZSI6Im1hYmFrb3ZpQG1pY3Jvc29mdC5jb20iLCJ1cG4iOiJtYWJha292aUBtaWNyb3NvZnQuY29tIiwidXRpIjoiSkQ3a1NsTXlOVS1FcVYtYXJoQUlBQSIsInZlciI6IjEuMCIsInhtc19zdCI6eyJzdWIiOiJXYkdxOFZ0djE0SDF2OWNkSURKM1hERmNnMDBtUzZ4dnNoVXNINktxczBjIn0sInhtc190Y2R0IjoxMjg5MjQxNTQ3fQ.fnxaOX-rLkCzC-jC01485XfgS1UNaX1aArZtnWbaw7juO8_qLYFybejFS77IICpo-cWBmTJh8klC8_kINqWAQuygCf3xlj8BG1Up90dKl0mN6oSFeuWSMYRrDYiT_jPStmfhhAiVG2YxgCEYAsqJcA1fASTd6_6kwSv62VQdyb3Ehy4fnIkFivAwa1AG3t-a4tQCpc9heMsCzzZEV97Y8WiqjYpfrglhoE4QzOzrzSfxt6TvqPiEkXICtAOPrDPZCs88dprofLPn-S1Q3sFIyoC5zrHsP6Xpd3w3IRtyBxfHStE5cQtcW3pu3HrrIO6ad7TvX8Mo5lxVDMD664KGfA";
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
