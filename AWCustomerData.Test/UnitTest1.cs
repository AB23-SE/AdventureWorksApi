using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using AWCustomerDataRetriever.API;
using AWCustomerDataRetriever.API.Clients;
using FluentAssertions;
using AWCustomerDataRetriever.Objects;

namespace AWCustomerData.Test
{
    [TestClass]
    public class UnitTest1
    {
        //using a mix of assert, FluentAssert and basic Mock here
        //probably could just use one of Fluent/Assert and mock but they both work.

        [TestMethod]
        public async Task TestResponse()
        {
            //generate mock from another method, we don't want to be writing the same code all over the place.
            var handlerMock = GenerateMock();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://api.adventureworks.com"),
            };

            var subjectUnderTest = new AdventureWorksCustomerClient(httpClient);

            //let's get some data out of the response
            CustomerWrapper result = await subjectUnderTest.GetCustomers();

            // also check the 'https' call was like we expected it
            var expectedUri = new Uri("https://api.adventureworks.com/v1/customers");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1), // we expected a single external request
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get  // it should be a GET request
                  && req.RequestUri == expectedUri // is it to the correct uri?
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestMethod]
        public async Task Task_IsNotNull()
        {
            // ARRANGE
            Mock<HttpMessageHandler> handlerMock = GenerateMock();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://api.adventureworks.com"),
            };

            var subjectUnderTest = new AdventureWorksCustomerClient(httpClient);

            //let's get some data out of the response
            CustomerWrapper result = await subjectUnderTest.GetCustomers();

            //result shouldn't be null.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Task_IsTwo()
        {
            // ARRANGE
            Mock<HttpMessageHandler> handlerMock = GenerateMock();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://api.adventureworks.com"),
            };

            var subjectUnderTest = new AdventureWorksCustomerClient(httpClient);

            //let's get some data out of the response
            CustomerWrapper result = await subjectUnderTest.GetCustomers();

            //should be two customers as there are in the array.
            result.Data.Should().HaveCount(2, "because we thought we put two items in the collection");
        }

        public Mock<HttpMessageHandler> GenerateMock()
        {
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
                   // Setup the PROTECTED method to mock
                   .Setup<Task<HttpResponseMessage>>(
                      "SendAsync",
                      ItExpr.IsAny<HttpRequestMessage>(),
                      ItExpr.IsAny<CancellationToken>()
                   )
                   // prepare the expected response of the mocked http call
                   .ReturnsAsync(new HttpResponseMessage()
                   {
                       StatusCode = HttpStatusCode.OK,
                       Content = new StringContent("{" +
                        "'TotalRecords': 2," +
                        "'Data':[" +
                        "{ 'Id': 12345, 'Name': 'Adventure Works Limited', 'Address': '1 Adventureworks Way', 'Town': 'Testville', 'Postcode': 'T1 1TT', 'CreationDate':'2013-10-21T13:28:06.419Z' }," +
                        "{ 'Id': 54321, 'Name': 'Adventure Works Inc', 'Address': '2 Adventureworks Way', 'Town': 'Testville', 'Postcode': 'T1 1TT', 'CreationDate':'2015-02-13T09:45:02.217Z'}" +
                        "]" +
                        "}"),
                   })
                   .Verifiable();

            return handlerMock;
        }



    }
}
