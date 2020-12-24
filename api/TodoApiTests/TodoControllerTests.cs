using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi;
using TodoApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TodoApiTests
{
    public class Tests
    {
        private Mock<ITodoService> _mockService;
        private TodoItemsController _sut;

        [Test]
        [Category("GetItemTest")]
        public async Task GetTodoItem_WhenGivenAValidId_ReturnsAnItem()
        {
            //arrange
            _mockService = new Mock<ITodoService>(MockBehavior.Strict);
            _mockService.Setup(s => s.GetItemByIdAsync(2))
                .ReturnsAsync(
                new TodoItem() { Id = 2, IsComplete = false, Name = "Feed Fish", Secret = "I hate fish" }
                );

            _sut = new TodoItemsController(_mockService.Object);

            //act
            var result = await _sut.GetTodoItem(2);

            //Assert
            Assert.That(result, Is.InstanceOf<ActionResult<TodoItemDTO>>());
            Assert.That(result.Value, Is.InstanceOf<TodoItemDTO>());
            Assert.That(result.Value.Name, Is.EqualTo("Feed Fish"));

        }

        [Test]
        [Category("GetItemTest")]
        public async Task GetTodoItem_Returns_Null_WhenInvalIdIsGiven()
        {
            //arrange
            _mockService = new Mock<ITodoService>(MockBehavior.Strict);
            _mockService.Setup(s => s.GetItemByIdAsync(3)).ReturnsAsync((TodoItem)null);
             _sut = new TodoItemsController(_mockService.Object);

            //act
            var result = await _sut.GetTodoItem(3);

            //Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        [Category("GetItemsTest")]
        public async Task GetTodoItems_ReturnsAListOfItems_WithCorrectValues()
        {
            //Arrange
            IEnumerable<TodoItem> todoList = new List<TodoItem>()
            {
                new TodoItem() { Id = 1, IsComplete = false, Name = "Feed Fish"},
                new TodoItem() { Id = 2, IsComplete = false, Name = "Feed Cat"},
                new TodoItem() { Id = 3, IsComplete = false, Name = "Feed Dog"}
            };
            _mockService = new Mock<ITodoService>(MockBehavior.Strict);
            _mockService.Setup(s => s.GetItemsListAsync()).ReturnsAsync(todoList);
            _sut = new TodoItemsController(_mockService.Object);

            //Act
            var result = await _sut.GetTodoItems();
            //var itemTDOList = (List<TodoItemDTO>)result.Value;  //same as below but longer
            var itemTDOList = result.Value.ToList();
            
            //Assert
            Assert.That(itemTDOList.Count, Is.EqualTo(3));
            Assert.That(itemTDOList[0].Name, Is.EqualTo("Feed Fish"));
            Assert.That(itemTDOList[1].Name, Is.EqualTo("Feed Cat"));
            Assert.That(itemTDOList[2].Name, Is.EqualTo("Feed Dog"));
        }

        [Test]
        [Category("UpdateTodoTest")]
        public async Task UpdateTodoItem_ReturnsNotFound_GivenNonExistantItemId()
        {
            //Arrange
            _mockService = new Mock<ITodoService>();
            _mockService.Setup(s => s.GetItemByIdAsync(1)).ReturnsAsync((TodoItem)null);
            _sut = new TodoItemsController(_mockService.Object);

            //Act
            var actionResult = await _sut.UpdateTodoItem(1, new TodoItemDTO() { Id = 1, IsComplete = false, Name = "Feed Fish" });

            //Assert
            Assert.That(actionResult, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        [Category("UpdateTodoTest")]
        public async Task UpdateTodoItem_SaveChanges_ReturnsNotFountResult_WhenTodoExistReturnsFalse()
        {
            var todoDTO = new TodoItemDTO() { Id = 1, IsComplete = false, Name = "Feed Fish" };
            var todoItem = new TodoItem() { Id = 1, IsComplete = false, Name = "Feed Fish" };
            //Arrange
            _mockService = new Mock<ITodoService>(MockBehavior.Strict);
            _mockService.Setup(s => s.GetItemByIdAsync(1)).ReturnsAsync(todoItem);
            _mockService.Setup(s => s.SaveChangesAsync()).ThrowsAsync(new DbUpdateConcurrencyException());
            _mockService.Setup(s => s.TodoItemExists(It.IsAny<long>())).Returns(false);
            _sut = new TodoItemsController(_mockService.Object);

            //Act
            var actionResult = await _sut.UpdateTodoItem(1, todoDTO);

            //Assert
            Assert.That(actionResult, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        [Category("UpdateTodoTest")] 
        public void UpdateTodoItem_SaveChanges_ThrowsException_WhenTodoItemReturnsTrue()
        { //Arrange
            var todoDTO = new TodoItemDTO() { Id = 1, IsComplete = false, Name = "Feed Fish" };
            var todoItem = new TodoItem() { Id = 1, IsComplete = false, Name = "Feed Fish" };


            _mockService = new Mock<ITodoService>(MockBehavior.Strict);
            _mockService.Setup(s => s.GetItemByIdAsync(1)).ReturnsAsync(todoItem);
            _mockService.Setup(s => s.SaveChangesAsync()).ThrowsAsync(new DbUpdateConcurrencyException());
            _mockService.Setup(s => s.TodoItemExists(It.IsAny<long>())).Returns(true);
            _sut = new TodoItemsController(_mockService.Object);

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _sut.UpdateTodoItem(1, todoDTO)); //shoundt it result noContent?
        }


        [Test]
        [Category("UpdateTodoTest")]
        public async Task UpdateTodoItem_ReturnsBadRequest_GivenInvalidId()
        {
            //Arrange
            _mockService = new Mock<ITodoService>();
            _mockService.Setup(s => s.GetItemByIdAsync(1)).ReturnsAsync((TodoItem)null);
            _sut = new TodoItemsController(_mockService.Object);

            //Act
            var actionResult = await _sut.UpdateTodoItem(2, new TodoItemDTO() { Id = 1, IsComplete = false, Name = "Feed Fish" });

            //Assert
            Assert.That(actionResult, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        [Category("UpdateTodoTest")]
        public async Task UpdateTodoItem_ReturnsNoContent_GivenCorrectId()
        {
            //Arrange
            var todoItem = new TodoItem() { Id = 1, IsComplete = false, Name = "Feed Fish" };
            _mockService = new Mock<ITodoService>();
            _mockService.Setup(s => s.GetItemByIdAsync(1)).ReturnsAsync(todoItem);
            _sut = new TodoItemsController(_mockService.Object);

            //Act
            var actionResult = await _sut.UpdateTodoItem(1, new TodoItemDTO() { Id = 1, IsComplete = false, Name = "Feed Fish" });

            //Assert
            Assert.That(actionResult, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        [Category("CreateItemTest")]
        public async Task GivenItemDTO_ReturnCreatedAtActionResult_IfSuccessful()
        {
            //Arrange
            var todoItemDTO = new TodoItemDTO() { Id = 1, IsComplete = false, Name = "Feed Fish" };
            _mockService = new Mock<ITodoService>();
            _mockService.Setup(s => s.AddTodoItem(It.IsAny<TodoItem>()));
            _sut = new TodoItemsController(_mockService.Object);

            //Act
            var actionResult = await _sut.CreateTodoItem(todoItemDTO);

            //Assert
            Assert.That(actionResult.Result, Is.InstanceOf<CreatedAtActionResult>());
            _mockService.Verify(v => v.SaveChangesAsync(), Times.Exactly(1));        //spy
            _mockService.Verify(s => s.AddTodoItem(It.IsAny<TodoItem>()), Times.Exactly(1));

        }

        [Test]
        [Category("DeleteItemTest")]
        public async Task DeleteTodoItem_ReturnsNoContentResult_WhenDeleted()
        {
            //Arrange
            var todoItem= new TodoItem() { Id = 1, IsComplete = false, Name = "Feed Fish" };
            _mockService = new Mock<ITodoService>();
            _mockService.Setup(s => s.GetItemByIdAsync(1)).ReturnsAsync(todoItem);
            _mockService.Setup(s => s.RemoveTodoItem(It.IsAny<TodoItem>()));
            _sut = new TodoItemsController(_mockService.Object);
            
            //Act
            var actionResult = await _sut.DeleteTodoItem(1);

            //Assert
            Assert.That(actionResult.Result, Is.InstanceOf<NoContentResult>());
            _mockService.Verify(s => s.RemoveTodoItem(It.IsAny<TodoItem>()), Times.Exactly(1)); //spy
        }

        [Test]
        [Category("DeleteItemTest")]
        public async Task DeleteTodoItem_ReturnsNotFound_GivenNonExistantId()
        {
            //Arrange
            _mockService = new Mock<ITodoService>();
            _mockService.Setup(s => s.GetItemByIdAsync(1)).ReturnsAsync((TodoItem)null);
            _sut = new TodoItemsController(_mockService.Object);

            //Act
            var actionResult = await _sut.DeleteTodoItem(2);

            //Assert
            Assert.That(actionResult.Result, Is.InstanceOf<NotFoundResult>());
        }
    }
}