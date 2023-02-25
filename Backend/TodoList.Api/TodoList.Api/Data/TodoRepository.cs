using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Data
{
    /// <summary>
    /// A repository of <see cref="TodoItem"/>s that operates in a <see cref="TodoContext"/>.
    /// </summary>
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;

        
        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<TodoItem>> GetIncompleteTodoItems()
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<TodoItem> GetTodoItem(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<TodoItem> CreateOrUpdateTodoItem(TodoItem todoItemToPut)
        {
            var existingItem = _context.TodoItems.FirstOrDefault(i => i.Id == todoItemToPut.Id);
            if (existingItem == null)
            {
                await _context.TodoItems.AddAsync(todoItemToPut);
            }
            else
            {
                _context.Entry(existingItem).State = EntityState.Modified;
                existingItem = todoItemToPut;
            }
            await _context.SaveChangesAsync();
            return existingItem;
        }

        /// <inheritdoc />
        public async Task<TodoItem> CreateTodoItem(TodoItem todoItemToPost)
        {
            await _context.TodoItems.AddAsync(todoItemToPost);
            await _context.SaveChangesAsync();
            return todoItemToPost;
        }

        /// <inheritdoc />
        public async Task<bool> TodoItemIdExists(Guid id)
        {
            return await _context.TodoItems.AnyAsync(i => i.Id == id);
        }

        /// <inheritdoc />
        public async Task<bool> IncompleteTodoItemDescriptionExists(string description)
        {
            string descriptionUppercase = description.ToUpperInvariant();
            return await _context.TodoItems.AnyAsync(i =>
                !i.IsCompleted
                && i.Description.ToUpperInvariant() == descriptionUppercase);
        }
    }
}
