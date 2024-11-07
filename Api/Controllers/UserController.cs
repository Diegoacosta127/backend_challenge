using Microsoft.AspNetCore.Mvc;
//using Data;
using Core;
using Services;

namespace Api.Controllers{
    [Route("Api/User")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly UserService _userService;
        public UserController(UserService userService){
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll(){
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id){
            var user = _userService.GetUserById(id); //_context.User.Find(id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user){
            var createdUser = _userService.CreateUser(user);
            return CreatedAtAction(nameof(GetById), new {id = createdUser.Id}, createdUser);
        }
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult Update([FromRoute] int id, [FromBody] User user){
            var updatedUser = _userService.UpdateUser(id, user);
            return updatedUser != null ? Ok(updatedUser) : NotFound();
        }
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete([FromRoute] int id){
            return _userService.DeleteUser(id) ? NoContent() : NotFound();
        }
    }
}