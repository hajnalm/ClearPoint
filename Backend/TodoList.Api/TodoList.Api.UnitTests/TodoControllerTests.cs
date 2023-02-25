using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.Data;
using TodoList.Api.Dtos;
using TodoList.Api.Mapping;
using TodoList.Api.Models;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoControllerTests
    {
        private List<TodoItem> _todoItems;
        private List<TodoItem> _incompleteTodoItems;
        private readonly Mock<ILogger<TodoItemsController>> _loggerMock;
        private readonly Mock<ITodoRepository> _repoMock;
        private readonly TodoItemsController _controller;
        private readonly IMapper _mapper;

        public TodoControllerTests()
        {
            PopulateFakeData();

            var myProfile = new TodoProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _loggerMock = new Mock<ILogger<TodoItemsController>>();
            _repoMock = new Mock<ITodoRepository>();
            _controller = new TodoItemsController(_repoMock.Object, _loggerMock.Object, _mapper);

            
        }

        private void PopulateFakeData()
        {
            _todoItems = new List<TodoItem>
            {
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Wake up",
                    IsCompleted = true
                },
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Eat breakfast",
                    IsCompleted = true
                },
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Work 4 hours",
                    IsCompleted = false
                },
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Eat lunch",
                    IsCompleted = false
                },
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Work another 4 hours",
                    IsCompleted = false
                },
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Sleep",
                    IsCompleted = false
                }
            };

            _incompleteTodoItems = _todoItems.Where(i => !i.IsCompleted).ToList();
        }

        [Fact]
        public async void GetIncompleteTodoItems_ReturnsOk_WithAllIncompleteTodoItems()
        {
            //Arrange
            _repoMock
                .Setup(r => r.GetIncompleteTodoItems())
                .ReturnsAsync(_incompleteTodoItems);
            var expectedDtos = _mapper.Map<List<TodoItemReadDto>>(_incompleteTodoItems);

            //Act
            var result = await _controller.GetTodoItems();

            //Assert
            _repoMock.Verify(r => r.GetIncompleteTodoItems());
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualDtos = Assert.IsType<List<TodoItemReadDto>>(okResult.Value);
            Assert.Equal(expectedDtos, actualDtos);
        }


        [Fact]
        public async Task GetTodoItem_WithExistingId_ReturnsOk_WithTodoItemReadDto()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var itemToReturn = new TodoItem 
            { 
                Id = itemId,
                Description = "Task",
                IsCompleted = false 
            };
            var expectedDto = new TodoItemReadDto { Id = itemId, Description = itemToReturn.Description };
            _repoMock.Setup(r => r.GetTodoItem(itemId)).ReturnsAsync(itemToReturn);

            // Act
            var result = await _controller.GetTodoItem(itemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualDto = Assert.IsType<TodoItemReadDto>(okResult.Value);
            Assert.Equal(expectedDto, actualDto);
        }

        [Fact]
        public async Task GetTodoItem_WithNonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            _repoMock.Setup(r => r.GetTodoItem(itemId)).ReturnsAsync((TodoItem)null);

            // Act
            var result = await _controller.GetTodoItem(itemId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutTodoItem_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var itemWriteDto = new TodoItemWriteDto { Id = Guid.NewGuid(), Description = "Do coding assignment", IsCompleted = true };
            var itemMappedFromWriteDto = _mapper.Map<TodoItem>(itemWriteDto);
            var itemToReturn = new TodoItem { Id = itemWriteDto.Id.Value, Description = itemWriteDto.Description, IsCompleted = itemWriteDto.IsCompleted };
            var expectedReadDto = _mapper.Map<TodoItemReadDto>(itemToReturn);
            
            _repoMock.Setup(r => r.CreateOrUpdateTodoItem(itemMappedFromWriteDto)).ReturnsAsync(itemToReturn);

            // Act
            var result = await _controller.PutTodoItem(itemWriteDto);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(expectedReadDto, okResult.Value);
        }

    }
}
