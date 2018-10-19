using System;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace todo
{
    public class DocumentDBRepository<T> where T : class
    {
        private string DatabaseId;
        private string CollectionId;
        public DocumentClient client;

        public DocumentDBRepository(string endpoint, string authKey, string databaseId, string collectionId)
        {
            this.DatabaseId = databaseId;
            this.CollectionId = collectionId;
            this.client = new DocumentClient(new Uri(endpoint), authKey);
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();

            while(query.HasMoreResults){
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }
    }
}