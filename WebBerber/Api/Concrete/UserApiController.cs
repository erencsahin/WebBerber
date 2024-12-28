using Microsoft.AspNetCore.Mvc;
using WebBerber.Api.Abstract;
using WebBerber.Models;

namespace WebBerber.Api.Concrete
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        public UserApiController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }


        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var user = _userRepository.GetById(userId);

            if (user == null)
            {
                return NotFound("Böyle bir kullanıcı bulunamadı.");
            }

            return Ok(user);
        }
    }
}
