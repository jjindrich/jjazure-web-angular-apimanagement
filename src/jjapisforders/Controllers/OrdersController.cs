using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Newtonsoft.Json;

namespace jjapisforders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private static  Uri serviceUri;
        private static readonly TimeSpan backoffQueryDelay;

        private static readonly FabricClient fabricClient;

        private static readonly HttpCommunicationClientFactory communicationFactory;

        private static DocumentClient _cosmosDbClient;


        static OrdersController()
        {
            backoffQueryDelay = TimeSpan.FromSeconds(3);
            fabricClient = new FabricClient();
            communicationFactory = new HttpCommunicationClientFactory(ServicePartitionResolver.GetDefault());
        }

        private IConfiguration _configuration;
        public OrdersController(IConfiguration configuration, DocumentClient client )
        {
            serviceUri = new Uri(_configuration.GetSection("ConnectionStrings").GetValue<string>("SFAppName") + _configuration.GetSection("ConnectionStrings").GetValue<string>("ServicePath"));
            _configuration = configuration;
            _cosmosDbClient = client;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            try
            {
                ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();
                ServicePartitionClient<HttpCommunicationClient> partitionClient
                    = new ServicePartitionClient<HttpCommunicationClient>(communicationFactory, serviceUri, new ServicePartitionKey());

                string content = null;
                await partitionClient.InvokeWithRetryAsync(
                    async (client) =>
                    {
                        var path = _configuration.GetSection("ConnectionStrings").GetValue<string>("BookServicePath");
                        HttpResponseMessage response = await client.HttpClient.GetAsync(new Uri(client.Url + path));
                        content = await response.Content.ReadAsStringAsync();
                    });

                dynamic document = new
                {
                    name = "Admin",
                    address = "address",
                };
                //Creating Document - you can obtain id from result
                var result = await _cosmosDbClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosDBName"), _configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosCollectionName")), document);

                return content;
            }
            catch (Exception ex)
            {
                // Sample code: print exception
               

            }
            
            return null;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
