using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class PersonDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string JobType { get; set; }
        public int Age { get; set; }
    }
}
