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
            //create a single http client so we don't have to create one for each client.
            HttpClient httpClient = new HttpClient();

            AdventureWorksCustomerClient client = new AdventureWorksCustomerClient(httpClient);

            //get customers, wrapper.Data will hold the customers for us to do whatever we want too.
            CustomerWrapper wrapper = await client.GetCustomers();
        }
    }
}
