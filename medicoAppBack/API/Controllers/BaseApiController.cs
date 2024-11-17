using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Define la ruta base para el controlador como "api/[controller]".
    // [controller] es un marcador de posición que se reemplaza con el nombre del controlador.
    [Route("api/[controller]")]
    // Indica que este controlador responde a solicitudes HTTP y se adhiere a las convenciones del controlador de API.
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
