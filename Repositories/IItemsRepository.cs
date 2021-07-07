using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoItems.Models;

namespace TodoItems.Repositories
{
    public interface IItemsRepository
    {
        Task<TodoItem> findOne(string id);
        Task<IEnumerable<TodoItem>> FindAll();
        Task CreateItem(TodoItem item);
        Task UpdateItem(TodoItem item);
        Task DeletItem(string id);
    }
}