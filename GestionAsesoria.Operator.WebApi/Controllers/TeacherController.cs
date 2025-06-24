using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("practice-count")]
        public async Task<IActionResult> GetTeachersWithPracticeCount()
        {
            var result = await _teacherService.GetTeachersWithPracticeCountAsync();
            return Ok(result);
        }
    }
}
