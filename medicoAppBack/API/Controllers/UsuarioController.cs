using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entidades;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    

    public class UsuarioController : BaseApiController
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


            
        [HttpPost("registro")] // POST: api/usuarios/registro
        public async Task<ActionResult<Usuario>> Registro(RegistroDto registroDto)
        {
            // Verificar si el usuario ya existe en la base de datos.
            if (await UsuarioExiste(registroDto.Username))
            {
             return BadRequest("Usuario ya registrado");
            }
            // Crear una instancia de HMACSHA512 para generar el hash de la contraseña y la clave de la sal.
            using var hmac = new HMACSHA512();

            // Crear un nuevo objeto Usuario y asignar sus propiedades.
            var usuario = new Usuario
             {
             Username = registroDto.Username.ToLower(), // Almacenar el nombre de usuario en minúsculas.
             PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registroDto.Password)), // Calcular el hash de la contraseña.
             PasswordSalt = hmac.Key // Asignar la clave de la sal.
             };

            // Añadir el nuevo usuario al contexto de la base de datos.
            _db.Usuarios.Add(usuario);

            // Guardar los cambios en la base de datos de forma asincrónica.
            await _db.SaveChangesAsync();

            // Devolver el objeto Usuario creado.
            return usuario;
        }

        [HttpPost("login")] // Este atributo indica que el método maneja solicitudes HTTP POST en la ruta api/usuario/login.
        public async Task<ActionResult<Usuario>> Login(LoginDto loginDto)
        {
            // Buscar al usuario en la base de datos cuyo nombre de usuario coincida con el proporcionado en el objeto loginDto.
            var usuario = await _db.Usuarios.SingleOrDefaultAsync(x => x.Username == loginDto.Username);

            // Si el usuario no existe, devolver un error 401 Unauthorized con el mensaje "Usuario no valido".
            if (usuario == null)
            {
                return Unauthorized("Usuario no valido");
            }

            // Crear una instancia de HMACSHA512 usando la sal almacenada del usuario.
            using var hmac = new HMACSHA512(usuario.PasswordSalt);

            // Calcular el hash de la contraseña proporcionada usando la instancia HMACSHA512.
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            // Comparar el hash calculado con el hash almacenado en la base de datos.
            for (int i = 0; i < computedHash.Length; i++)
            {
                // Si algún byte del hash calculado no coincide con el hash almacenado, devolver un error 401 Unauthorized con el mensaje "Password no valido".
                if (computedHash[i] != usuario.PasswordHash[i])
                {
                    return Unauthorized("Password no valido");
                }
            }

            // Si las contraseñas coinciden, devolver el objeto usuario.
            return usuario;
        }


        // Método auxiliar para comprobar si un nombre de usuario ya existe en la base de datos.
        private async Task<bool> UsuarioExiste(string username)
        {
        // Verificar de forma asincrónica si algún usuario en la base de datos tiene el mismo nombre de usuario en minúsculas.
             return await _db.Usuarios.AnyAsync(x => x.Username == username.ToLower());
          }
        }

}
