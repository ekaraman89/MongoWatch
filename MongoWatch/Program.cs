using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoWatch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("db_test");
            var collection = database.GetCollection<BsonDocument>("testColl");

            var options = new ChangeStreamOptions { FullDocument = ChangeStreamFullDocumentOption.UpdateLookup };
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>().Match("{ operationType: { $in: [ 'insert', 'delete' ] } }");

            //var cursor = collection.Watch(pipeline, options);
            var cursor = collection.Watch();

            var enumerator = cursor.ToEnumerable().GetEnumerator();
            while (enumerator.MoveNext())
            {
                ChangeStreamDocument<BsonDocument> doc = enumerator.Current;
                // Do something here with your document
                Console.WriteLine(doc.DocumentKey);
            }
        }
    }
}
