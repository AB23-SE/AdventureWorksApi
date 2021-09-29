using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AWCustomerDataRetriever.API.Clients
{
    //underlying class for all AdventureWorks API calls.
    public class AdventureWorksClient : IDisposable
    {
        HttpClient _httpclient;

        //let's make sure it's got a slash at the end
        private string ApiUrl => Properties.Settings.Default.ApiUrl + (Properties.Settings.Default.ApiUrl.LastOrDefault() == '/' ? "" : "/");
        private string ApiUsername => Properties.Settings.Default.ApiUsername;
        private string ApiPassword => Properties.Settings.Default.ApiPassword;

        public AdventureWorksClient(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }

        public void Dispose()
        {
            //clean up after ourselves
            _httpclient.Dispose();
        }

        //refactor endpoint into it's own class if AdventureWorks tighten up their authentication.
        protected async Task<TResponseModel> ExecuteAsync<TResponseModel>(HttpMethod method, string endpoint)
        {
            HttpRequestMessage message = new HttpRequestMessage(method, ApiUrl + endpoint);

            //we're expecting a response back in JSON
            message.Headers.Add("ContentType", "application/json");
           
            //basic authentication
            message.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ApiUsername}:{ApiPassword}")));
            
            using (HttpResponseMessage response = await _httpclient.SendAsync(message))
            {
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    //handle DateTimeZone with local format
                    return JsonConvert.DeserializeObject<TResponseModel>(responseString, new JsonSerializerSettings()
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    });
                }
                else
                {
                    throw new Exception($"API Call {endpoint} responded with {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
    }
}
