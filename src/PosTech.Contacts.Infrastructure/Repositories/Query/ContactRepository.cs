using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using PosTech.Contacts.ApplicationCore.Entities.Query;
using PosTech.Contacts.ApplicationCore.Repositories.Query;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization.Conventions;
using PosTech.Contacts.Infrastructure.Settings;

namespace PosTech.Contacts.Infrastructure.Repositories.Query
{
    public class ContactRepository : IContactRepository
    {
        private readonly IMongoCollection<Contact> _contactCollection;

        static ContactRepository()
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            BsonClassMap.RegisterClassMap<Contact>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdProperty(p => p.Id);
            });
        }

        public ContactRepository(MongoDbSettings mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);

            _contactCollection = mongoDatabase.GetCollection<Contact>(DbCollections.Contacts);
        }

        public async Task AddAsync(Contact contact) => await _contactCollection.InsertOneAsync(contact);

        public async Task<Contact> GetAsync(Guid id)
            => await _contactCollection.Find(contact => contact.Id == id).SingleOrDefaultAsync();

        public async Task UpdateAsync(Contact contact)
            => await _contactCollection.ReplaceOneAsync(storedContact => storedContact.Id == contact.Id, contact);

        public async Task DeleteAsync(Guid id) 
            => await _contactCollection.DeleteOneAsync(storedContact => storedContact.Id == id);

        public async Task<List<Contact>> FindContactsAsync(Expression<Func<Contact, bool>> predicate)
        {
            var result = await _contactCollection.Find(predicate).ToListAsync();

            return result;
        }

        /// <summary>
        /// Versão com suporte a paginação. 
        /// Skip((pageNumber - 1) * pageSize): Ignora os registros das páginas anteriores.
        /// Limit(pageSize): Define o número máximo de registros retornados.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<Contact>> FindContactsAsync(Expression<Func<Contact, bool>> predicate, int pageNumber, int pageSize)
        {
            var result = await _contactCollection.Find(predicate)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return result;
        }
    }
}
