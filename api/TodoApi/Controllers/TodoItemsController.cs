using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Interfaces;

namespace TodoApi.Controllers
{
    [Route("api/TodoItems")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService _service; //the startup injects the service.cs to the iservice

        public TodoItemsController(ITodoService service)
        {
            _service = service;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            var todoList = await _service.GetItemsListAsync();
            var todoDTOList = new List<TodoItemDTO>();

            foreach(var item in todoList)
            {
                todoDTOList.Add(ItemToDTO(item));
            }

            return todoDTOList;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _service.GetItemByIdAsync(id);
            if (todoItem == null)  return NotFound();
            return ItemToDTO(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id) return BadRequest();
           
            var todoItem = await _service.GetItemByIdAsync(id);

            if (todoItem == null) return NotFound();

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {  await _service.SaveChangesAsync(); 
            }

            catch (DbUpdateConcurrencyException) when  (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            _service.AddTodoItem(todoItem);
            await _service.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItemDTO>> DeleteTodoItem(long id)
        {
            var todoItem = await _service.GetItemByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _service.RemoveTodoItem(todoItem);
            await _service.SaveChangesAsync();

            return NoContent();
        }

        private  bool TodoItemExists(long id) => _service.TodoItemExists(id);

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}
