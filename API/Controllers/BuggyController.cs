using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController:BaseApiController
    {
        // this is used to see the differnt types of errors, so we can see the 
        //differnt kinds of response that the api can see in the event of new errors
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("testauth")]
        public ActionResult<string> GetSecretText()
        {
            return "secret stuff";
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(42);
            //we know there isnt 42 items within the db
            if(thing == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok();
        }

         [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(42);
            var thingToReturn = thing.ToString();
            return Ok();
            //cant execute a toString() method on something that doesnt exist
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }

    }
}