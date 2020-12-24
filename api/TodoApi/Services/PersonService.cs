using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class PersonService : IPersonService
    {
        private readonly TodoContext _context;

        public PersonService(TodoContext context)
        {
            _context = context;
        }


        public Task<Person> GetItemByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        /*public async Task<IEnumerable<Person>> GetItemsListAsync()
        {
            var listOfPeople = await _context.Persons.ToListAsync();
        }*/

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
