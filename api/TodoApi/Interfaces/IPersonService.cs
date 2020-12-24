using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface IPersonService
    {
        //Task<IEnumerable<Person>> GetItemsListAsync();

        Task<Person> GetItemByIdAsync(long id);

        Task SaveChangesAsync();

    }
}
