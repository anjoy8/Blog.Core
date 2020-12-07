using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Core.Repository.MongoRepository
{


    public class MongoBaseRepository<TEntity> : IMongoBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly MongoDbContext _context;

        public MongoBaseRepository()
        {
            _context = new MongoDbContext();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Db.GetCollection<TEntity>(nameof(TEntity))
               .InsertOneAsync(entity);
        }

        public async Task<TEntity> GetAsync(int Id)
        {
            var filter = Builders<TEntity>.Filter.Eq("Id", Id);

            return await _context.Db.GetCollection<TEntity>(nameof(TEntity))
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetListAsync()
        {
            return await _context.Db.GetCollection<TEntity>(nameof(TEntity))
                .Find(new BsonDocument())
                .ToListAsync();
        }
    }
}
