using AWCustomerDataRetriever.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AWCustomerDataRetriever.API.Clients
{
    public class AdventureWorksCustomerClient : AdventureWorksClient
    {
        public AdventureWorksCustomerClient(HttpClient httpClient) : base (httpClient)
        {
            
        }

        public Task<CustomerWrapper> GetCustomers()
        {
           return ExecuteAsync<CustomerWrapper>(HttpMethod.Get, "v1/customers");
        }
    }
}
