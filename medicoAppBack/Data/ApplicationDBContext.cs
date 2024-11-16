using Microsoft.EntityFrameworkCore;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    // Definición de la clase ApplicationDBContext que hereda de DbContext
    public class ApplicationDBContext : DbContext
    {
        // Constructor de la clase que acepta un parámetro DbContextOptions y lo pasa al constructor base de DbContext
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            // Aquí puedes inicializar cualquier propiedad adicional si es necesario
        }

        // Definición de una propiedad DbSet para la entidad Usuario
        // DbSet representa una colección de todas las entidades en el contexto, o que pueden ser consultadas desde la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
    }

}
