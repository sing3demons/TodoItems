using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TodoItems.Models;

namespace TodoItems.Repositories
{
    public class TodoItemRepository : IItemsRepository
    {
        private const string databaseName = "todoItems";
        private const string collectionName = "items";
        private readonly IMongoCollection<TodoItem> _itemCollection;

        private readonly FilterDefinitionBuilder<TodoItem> filterBuilder = Builders<TodoItem>.Filter;

        public TodoItemRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            _itemCollection = database.GetCollection<TodoItem>(collectionName);
        }


        public async Task CreateItem(TodoItem item)
        {
            await _itemCollection.InsertOneAsync(item);
        }

        public async Task DeletItem(string id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            await _itemCollection.DeleteOneAsync(filter);
        }

        public async Task<TodoItem> findOne(string id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            var result = await _itemCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<TodoItem>> FindAll()
        {
            List<TodoItem> result = await _itemCollection.Find(new BsonDocument()).ToListAsync();
            return result;
        }

        public async Task UpdateItem(TodoItem item)
        {
            var filter = filterBuilder.Eq(existItem => existItem.Id, item.Id);
            await _itemCollection.ReplaceOneAsync(filter, item);
        }
    }
}