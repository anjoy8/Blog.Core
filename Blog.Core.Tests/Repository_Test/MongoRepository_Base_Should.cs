using Blog.Core.Model.Models;
using Xunit;
using System;
using System.Linq;
using Autofac;
using Blog.Core.IRepository.Base;
using Blog.Core.Repository.MongoRepository;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Blog.Core.Tests
{
    public class MongoRepository_Base_Should
    {
        public class MongoTest
        {
            [BsonId]
            public ObjectId id { get; set; }
            public string name { get; set; }
            public bool isDel { get; set; }
            public DateTime time { get; set; }
        }

        private IMongoBaseRepository<MongoTest> baseRepository;
        DI_Test dI_Test = new DI_Test();

        public MongoRepository_Base_Should()
        {

            var container = dI_Test.DICollections();

            baseRepository = container.Resolve<IMongoBaseRepository<MongoTest>>();
        }


        [Fact]
        public async void Add_Test()
        {
            await baseRepository.AddAsync(new MongoTest { isDel = false, name = "test", time = DateTime.UtcNow });
        }

        [Fact]
        public async void GetObjectId_Test()
        {
            var data = await baseRepository.GetByObjectIdAsync("612b9b0be677976fa0f0cfa2");

            Assert.NotNull(data);
        }


        [Fact]
        public async void GetListFilter_Test()
        {
            var data = await baseRepository.GetListFilterAsync(new FilterDefinitionBuilder<MongoTest>().Gte("time", DateTime.Parse("2022-06-01")));

            Assert.NotNull(data);
        }
    }
}
