using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Data
{
    public interface ITodoRepository
    {
        /// <summary>
        /// Retrieves all todo items from that aren't marked as completed.
        /// </summary>
        /// <returns>A list of todo items.</returns>
        Task<List<TodoItem>> GetIncompleteTodoItems();

        /// <summary>
        /// Retrieves a single todo item from the repository by its ID.
        /// </summary>
        /// <param name="id">The ID of the todo item to retrieve.</param>
        /// <returns>The todo item with the specified ID.</returns>
        Task<TodoItem> GetTodoItem(Guid id);

        /// <summary>
        /// Creates or updates a todo item in the repository. This is an idempotent operation.
        /// </summary>
        /// <param name="todoItemToPut">The details of the item to create or update</param>
        /// <returns>The resulting item.</returns>
        Task<TodoItem> CreateOrUpdateTodoItem(TodoItem todoItemToPut);

        /// <summary>
        /// Creates a new todo item and adds it to the repository.
        /// </summary>
        /// <param name="todoItemToPost">The details of the item to create.</param>
        /// <returns>The new item.</returns>
        Task<TodoItem> CreateTodoItem(TodoItem todoItemToPost);

        /// <summary>
        /// Checks if a todo item with the given ID already exists in the repository.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <returns>True if an item with the given ID exists, false otherwise.</returns>
        Task<bool> TodoItemIdExists(Guid id);

        /// <summary>
        /// Checks if a not completed todo item with the given description already exists in the repository.
        /// The comparison is case insensitive.
        /// </summary>
        /// <param name="description">The item description.</param>
        /// <returns>True if an item with the given description already exists, false otherwise.</returns>
        Task<bool> IncompleteTodoItemDescriptionExists(string description);
    }
}
