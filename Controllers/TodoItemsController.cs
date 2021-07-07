using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TodoItems.Models;
using TodoItems.Models.DTOs;
using TodoItems.Repositories;

namespace TodoItems.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;

        public TodoItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var items = await repository.FindAll();
            return Ok(new { todoItems = items });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(string id)
        {
            var item = await repository.findOne(id);
            if (item == null) return NotFound();

            return Ok(new { todoItems = item });
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemAsync([FromForm] CreateItemDto request)
        {
            TodoItem item = new TodoItem()
            {
                Name = request.Name,
                Desc = request.Desc,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
            };

            await repository.CreateItem(item);
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemAsync(string id, [FromForm] UpdateItemDto request)
        {
            var item = await repository.findOne(id);
            if (item == null) return NotFound();

            if (request.Name == null) request.Name = item.Name;

            if (request.Desc == null) request.Desc = item.Desc;


            item.Name = request.Name;
            item.Desc = request.Desc;
            item.UpdatedOn = DateTime.Now;


            await repository.UpdateItem(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingItem = await repository.findOne(id);
            if (existingItem is null) return NotFound();
            
            await repository.DeletItem(existingItem.Id);
            return NoContent();
        }
    }
}