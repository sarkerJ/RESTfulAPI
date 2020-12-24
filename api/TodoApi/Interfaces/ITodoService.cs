using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetItemsListAsync();

        Task<TodoItem> GetItemByIdAsync(long id);

        Task SaveChangesAsync();

        void AddTodoItem(TodoItem todoItem);
        void RemoveTodoItem(TodoItem todoItem);

        bool TodoItemExists(long id);
    }
}
