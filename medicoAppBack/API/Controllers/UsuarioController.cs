using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;

namespace API.Controllers
{
    //Clase 8
    // Define la ruta base para el controlador como "api/[controller]".
    // [controller] es un marcador de posición que se reemplaza con el nombre del controlador, en este caso, "usuario".
    [Route("api/[controller]")]
    // Indica que este controlador responde a solicitudes HTTP y se adhiere a las convenciones del controlador de API.
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // Campo privado para el contexto de la base de datos.
        private readonly ApplicationDBContext _db;

        // Constructor del controlador que recibe una instancia de ApplicationDBContext a través de la inyección de dependencias.
        public UsuarioController(ApplicationDBContext db)
        {
            _db = db; // Asigna la instancia del contexto de la base de datos al campo privado _db.
        }

        // Método que responde a las solicitudes HTTP GET en "api/usuario".
        // Este método devuelve una lista de todos los usuarios.
        [HttpGet]   // api/usuario
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            // Obtiene todos los usuarios de la base de datos.
            var Usuarios = await _db.Usuarios.ToListAsync();
            // Devuelve la lista de usuarios con un estado HTTP 200 OK.
            return Ok(Usuarios);
        }

        // Método que responde a las solicitudes HTTP GET en "api/usuario/{id}".
        // Este método devuelve un usuario específico basado en el ID proporcionado.
        [HttpGet("{id}")] // api/usuario/1
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            // Busca un usuario en la base de datos por ID.
            var usuario = await _db.Usuarios.FindAsync(id);
            // Devuelve el usuario con un estado HTTP 200 OK.
            return Ok(usuario);
        }
    }

}
