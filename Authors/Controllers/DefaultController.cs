using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Authors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {

        [HttpPost("[action]")]
        public IActionResult Index([FromBody] AuthorDto model)
        {
            return model.Id <= 0
                ? BadRequest()
                : (IActionResult)Ok("selam");
        }

    }
}