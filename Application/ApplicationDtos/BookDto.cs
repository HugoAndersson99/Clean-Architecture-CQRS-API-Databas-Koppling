using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationDtos
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public AuthorDto? Author { get; set; }
    }
}
