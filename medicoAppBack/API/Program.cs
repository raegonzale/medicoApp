using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra el contexto de la base de datos ApplicationDBContext en el contenedor de servicios de dependencia.
// Configura el contexto para usar SQL Server como proveedor de base de datos y le pasa la cadena de conexi�n.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

//Esta l�nea registra el servicio CORS en el contenedor de servicios de la aplicaci�n. Es necesario para que
//la aplicaci�n pueda configurar y usar pol�ticas de CORS.
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Esta l�nea aplica una pol�tica de CORS global que permite cualquier origen, cualquier encabezado y cualquier m�todo.
app.UseCors(x => x.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
