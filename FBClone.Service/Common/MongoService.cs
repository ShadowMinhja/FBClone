using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Service
{
    public interface IMongoService
    {
        //Sync
        IEnumerable<TDocument> GetAll<TDocument>(string collectionName, string userId, int? itemsToReturn);
        TDocument Single<TDocument>(string collectionName, string foreignId);
        TDocument InsertOne<TDocument>(TDocument document);
        IEnumerable<TDocument> InsertMany<TDocument>(IEnumerable<TDocument> documents);
        long Count<TDocument>(TDocument document);

        //ASync
        Task<IEnumerable<TDocument>> GetAllAsync<TDocument>(string collectionName, string userId, int? itemsToReturn);
        Task<TDocument> SingleAsync<TDocument>(string collectionName, string foreignId);
        Task<TDocument> InsertOneAsync<TDocument>(TDocument document);
        Task<IEnumerable<TDocument>> InsertManyAsync<TDocument>(IEnumerable<TDocument> documents);
        Task<long> CountAsync<TDocument>(TDocument document);

    }

    public class MongoService : IMongoService
    {
        private MongoClient client { get; set; }
        private IMongoDatabase db { get; set; }

        public MongoService()
        {
            string mongoDbURI = ConfigurationManager.AppSettings["MongoDb"];
            int mongoDbPort = Convert.ToInt32(ConfigurationManager.AppSettings["MongoDbPort"]);
            string mongoDbUser = ConfigurationManager.AppSettings["MongoDbUser"];
            string mongoDbPwd = ConfigurationManager.AppSettings["MongoDbPwd"];
            string mongoDbName = ConfigurationManager.AppSettings["MongoDbName"];
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(mongoDbURI, mongoDbPort);
            if (!mongoDbURI.Contains("localhost"))
            {
                //settings.UseSsl = true;
                //settings.SslSettings = new SslSettings();
                //settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
                //MongoIdentity identity = new MongoInternalIdentity(mongoDbName, mongoDbUser);
                //MongoIdentityEvidence evidence = new PasswordEvidence(mongoDbPwd);
                //settings.Credentials = new List<MongoCredential>()
                //{
                //    new MongoCredential("SCRAM-SHA-1", identity, evidence)
                //};
                this.client = new MongoClient(String.Format("mongodb://{0}:{1}@{2}:{3}/{4}", mongoDbUser, mongoDbPwd, mongoDbURI, mongoDbPort.ToString(), mongoDbName));
                this.db = client.GetDatabase(mongoDbName);
            }
            else
            {
                this.client = new MongoClient(settings);
                this.db = client.GetDatabase(mongoDbName);
            }
        }

        //Private Functions
        private string GetCollectionName<TDocument>(TDocument t)
        {
            return t.GetType().Name;
        }
        private IMongoCollection<TDocument> GetCollection<TDocument>(TDocument t)
        {
            return this.db.GetCollection<TDocument>(GetCollectionName(t));
        }
        private IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
        {
            return this.db.GetCollection<TDocument>(collectionName);
        }
        //Implemented Methods
        public IEnumerable<TDocument> GetAll<TDocument>(string collectionName, string userId, int? itemsToReturn)
        {
            var collection = GetCollection<TDocument>(collectionName);
            var filter = Builders<TDocument>.Filter.Eq("UserId", userId);
            FindOptions findOptions = new FindOptions();
            if(itemsToReturn != null)
            {
                findOptions.BatchSize = itemsToReturn;
            }
            var cursor = collection.Find(filter, findOptions).ToCursor();
            return cursor.ToEnumerable();
        }

        public async Task<IEnumerable<TDocument>> GetAllAsync<TDocument>(string collectionName, string userId, int? itemsToReturn)
        {
            var collection = GetCollection<TDocument>(collectionName);
            var filter = Builders<TDocument>.Filter.Eq("UserId", userId);
            FindOptions<TDocument> findOptions = new FindOptions<TDocument>();
            if (itemsToReturn != null)
            {
                findOptions.BatchSize = itemsToReturn;
            }
            var cursor = await collection.FindAsync(filter, findOptions);
            return cursor.ToEnumerable();
        }

        public TDocument Single<TDocument>(string collectionName, string foreignId)
        {
            TDocument document = default(TDocument);
            var collection = GetCollection<TDocument>(collectionName);
            try
            {
                var filter = Builders<TDocument>.Filter.Eq("_id", new ObjectId(foreignId));
                var results = collection.Find(filter);
                if (results.Any())
                    document = results.First();
                return document;
            }
            catch
            {
                return document;
            }
        }

        public async Task<TDocument> SingleAsync<TDocument>(string collectionName, string foreignId)
        {
            TDocument document = default(TDocument);
            var collection = GetCollection<TDocument>(collectionName);
            var filter = Builders<TDocument>.Filter.Eq("_id", new ObjectId(foreignId));
            var results = await collection.FindAsync(filter);
            if(results.Current.Any())
                document = results.First();
            return document;
        }

        public TDocument InsertOne<TDocument>(TDocument document)
        {
            //var collection = this.db.GetCollection<T>(GetCollectionName(document));
            var collection = GetCollection(document);
            collection.InsertOne(document);
            return document;
        }

        public async Task<TDocument> InsertOneAsync<TDocument>(TDocument document)
        {
            //var collection = this.db.GetCollection<T>(GetCollectionName(document));
            var collection = GetCollection(document);
            await collection.InsertOneAsync(document);
            return document;
        }

        public IEnumerable<TDocument> InsertMany<TDocument>(IEnumerable<TDocument> documents)
        {
            var collection = GetCollection(documents.FirstOrDefault());
            collection.InsertMany(documents);
            return documents;
        }

        public async Task<IEnumerable<TDocument>> InsertManyAsync<TDocument>(IEnumerable<TDocument> documents)
        {
            var collection = GetCollection(documents.FirstOrDefault());
            await collection.InsertManyAsync(documents);
            return documents;
        }

        public long Count<TDocument>(TDocument document)
        {
            var collection = GetCollection(document);
            return collection.Count(new BsonDocument());
        }

        public Task<long> CountAsync<TDocument>(TDocument document)
        {
            var collection = GetCollection(document);
            return collection.CountAsync(new BsonDocument());
        }

    }
}
