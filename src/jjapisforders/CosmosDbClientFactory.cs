using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jjapisforders
{
    public class CosmosDbClientFactory
    {
        private DocumentClient documentClient = null;

        public CosmosDbClientFactory(IConfiguration configuration)
        {
            var endpointUri = configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosEndpointUri");
            var key = configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosDBKey");
            var dbName = configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosDBName");
            var collectionName = configuration.GetSection("ConnectionStrings").GetValue<string>("CosmosCollectionName");

            if (!string.IsNullOrEmpty(endpointUri))
            {
                // Creating a new client instance
                documentClient = new DocumentClient(new Uri(endpointUri), key);
                // Create any database or collection you will work with here.
                documentClient.CreateDatabaseIfNotExistsAsync(new Database
                {
                    Id = dbName
                });
                this.documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(dbName), new DocumentCollection
                {
                    Id = collectionName
                });
            }
        }

        public DocumentClient GetDocumentClient()
        {
            return documentClient;
        }
    }
}
