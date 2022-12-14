using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly ShopContext _context;
        public BuggyController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        [Authorize]

        public ActionResult<string>getSecretText()
        {
            return "secret";
        }

        [HttpGet("notfound")]

        public IActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(21);
            
            if(thing==null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok();
        }

         [HttpGet("servererror")]

        public IActionResult GetServerError()
        {
            var thing = _context.Products.Find(42);

            var thingToReturn = thing.ToString();

            return Ok();
        }

         [HttpGet("badrequest")]

        public IActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

         [HttpGet("badrequest/{id}")]

        public IActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }
        
    }
}