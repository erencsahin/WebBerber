using Microsoft.AspNetCore.Mvc;
using WebBerber.Api.Abstract;
using WebBerber.Models;

namespace WebBerber.Api.Concrete
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeApiController : Controller
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeApiController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("{employeeId}")]
        public IActionResult GetShopById(int employeeId)
        {
            var employee = _employeeRepository.GetById(employeeId);

            if (employee == null)
            {
                return NotFound("Böyle bir dükkan bulunamadı.");
            }

            return Ok(employee);
        }

        [HttpGet]
        public IActionResult GetAllShops()
        {
            var employees = _employeeRepository.GetAll();
            return Ok(employees);
        }
    }
}
