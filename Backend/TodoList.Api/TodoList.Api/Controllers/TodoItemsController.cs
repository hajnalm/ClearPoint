using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Data;
using TodoList.Api.Dtos;
using TodoList.Api.Models;

namespace TodoList.Api.Controllers
{
    /// <summary>
    /// The controller responsible for handling requests related to <see cref="TodoItem"/>s.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoItemsController> _logger;
        private readonly IMapper _mapper;

        public TodoItemsController(ITodoRepository repository, ILogger<TodoItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the todo items that aren't flagged as 'completed'.
        /// </summary>
        /// <returns>A list of all the todo items not yet completed.</returns>
        /// <response code="200">Returns the list of TodoItemReadDto objects.</response>
        /// <response code="500">If there was an error retrieving the items.</response>
        // GET: api/TodoItems
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TodoItemReadDto>>> GetTodoItems()
        {
            try
            {
                var items = await _repository.GetIncompleteTodoItems();
                var dtos = _mapper.Map<List<TodoItemReadDto>>(items);
                return Ok(dtos);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving items.");
            }
        }

        /// <summary>
        /// Get the todo item with the given ID.
        /// </summary>
        /// <param name="id">The unique identifier of the todo item.</param>
        /// <returns>The todo item with the given ID.</returns>
        /// <response code="200">Returns the item.</response>
        /// <response code="404">If there is no item with the given ID.</response>
        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemReadDto>> GetTodoItem(Guid id)
        {
            try
            {
                var item = await _repository.GetTodoItem(id);

                if (item == null)
                {
                    return NotFound();
                }
                var dto = _mapper.Map<TodoItemReadDto>(item);
                return Ok(dto);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving item with the ID {id}.");
            }
            
        }

        /// <summary>
        /// Creates or updates a todo item. This operation is idempotent.
        /// </summary>
        /// <param name="todoItem">The item to create or update.</param>
        /// <returns>The created or updated item.</returns>
        /// <response code="200">Returns the updated item.</response>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">If the provided item is invalid.</response>
        /// <response code="500">If there was an error creating or updating the item.</response>
        // PUT: api/TodoItems/... 
        [HttpPut]
        public async Task<ActionResult<TodoItemReadDto>> PutTodoItem(TodoItemWriteDto todoItem)
        {
            if (string.IsNullOrWhiteSpace(todoItem?.Description))
            {
                return BadRequest("Invalid request. Provide a non-empty item description");
            }
            try
            {
                var newOrUpdatedItem = await _repository.CreateOrUpdateTodoItem(_mapper.Map<TodoItem>(todoItem));
                var dto = _mapper.Map<TodoItemReadDto>(newOrUpdatedItem);
                return newOrUpdatedItem.Id == todoItem.Id
                    ? Ok(dto)
                    : CreatedAtAction(nameof(GetTodoItem), new { id = dto.Id }, dto);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating or updating the item.");
            }
            
        }

        /// <summary>
        /// Creates a new todo item.
        /// </summary>
        /// <param name="todoItem">The item to create.</param>
        /// <returns>The created item.</returns>
        /// <remarks>If an item with the same description already exists and isn't marked as completed,
        /// no new item will be created.</remarks>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">If the item description is null, empty or whitespace, or if an item with the same description already exists and isn't marked as completed.</response>
        /// <response code="500">If an error occurs while creating the item.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TodoItemWriteDto), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> PostTodoItem(TodoItemWriteDto todoItem)
        {
            if (string.IsNullOrWhiteSpace(todoItem?.Description))
            {
                return BadRequest("Invalid request. Provide a non-empty item description");
            }
            try
            {
                if (await _repository.IncompleteTodoItemDescriptionExists(todoItem.Description))
                {
                    return BadRequest("Description already exists");
                }

                var createdItem = _repository.CreateTodoItem(_mapper.Map<TodoItem>(todoItem));
                return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating or updating the item.");
            }
            
        }
    }
}
