using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/Persons")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly TodoContext _context;

        public PeopleController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPersons()
        {
            return await _context.Persons.Select(x => PersonDTO(x)).ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDTO>> GetPerson(long id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return PersonDTO(person);
        }

        // PUT: api/People/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, PersonDTO personDTO)
        {
            if (id != personDTO.Id)
            {
                return BadRequest();
            }

            var currentPerson = await _context.Persons.FindAsync(id);
            if (currentPerson == null) return NotFound();

            currentPerson.Name = personDTO.Name;
            currentPerson.JobType = personDTO.JobType;
            currentPerson.Age = personDTO.Age;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PersonExists(id)) // if its false then just return noContent otherwise return notfound
            {
                
                 return NotFound();
                
            }

            return NoContent();
        }

        // POST: api/People
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PersonDTO>> PostPerson(PersonDTO personDTO)
        {
            var person = new Person
            {
                Name = personDTO.Name,
                JobType = personDTO.JobType,
                Age = personDTO.Age
            };

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetPerson", new { id = person.Id }, person);
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, PersonDTO(person));
            //createAtaction returns status code

        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonDTO>> DeletePerson(long id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return PersonDTO(person);
        }

        private bool PersonExists(long id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }

        //return PersonDTO
        private static PersonDTO PersonDTO(Person person) => new PersonDTO
            {
                Id = person.Id,
                Name = person.Name,
                JobType = person.JobType,
                Age = person.Age
            };
    }
}
