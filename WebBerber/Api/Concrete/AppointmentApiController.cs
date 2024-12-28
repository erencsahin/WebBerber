using Microsoft.AspNetCore.Mvc;
using WebBerber.Api.Abstract;
using WebBerber.Models;

namespace WebBerber.Api.Concrete
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentApiController : ControllerBase
    {
        private readonly IRepository<Appointment> _appointmentRepository;

        public AppointmentApiController(IRepository<Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            var appointments = _appointmentRepository.GetAll();
            return Ok(appointments);
        }

        [HttpGet("{appointmentId}")]
        public IActionResult GetAppointmentById(int appointmentId)
        {
            var appointment = _appointmentRepository.GetById(appointmentId);

            if (appointment == null)
            {
                return NotFound("Böyle bir randevu bulunamadı.");
            }

            return Ok(appointment);
        }
    }

}
