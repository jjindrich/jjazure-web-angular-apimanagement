using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jjapisfbooks.Model;
using Microsoft.AspNetCore.Mvc;

namespace jjapisfbooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {
            List<Book> retBook = new List<Book>();
            retBook.Add(new Book() { Id = 1, Name = "Book1" });
            retBook.Add(new Book() { Id = 2, Name = "Book2" });
            retBook.Add(new Book() { Id = 3, Name = "Book3" });

            return retBook;
        }

    }
}