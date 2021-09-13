using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Models
{
    public class ChoreCount
    {
        public int Id { get; set; }
        public string Roommate { get; set; }
        public int NumberOfChores { get; set; }
    }
}
