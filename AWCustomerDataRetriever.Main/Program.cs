using AWCustomerDataRetriever.API.Clients;
using AWCustomerDataRetriever.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AWCustomerDataRetriever.Main
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();

            AdventureWorksCustomerClient client = new AdventureWorksCustomerClient(httpClient);

            CustomerWrapper wrapper = await client.GetCustomers();
        }
    }
}
