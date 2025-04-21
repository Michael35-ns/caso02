using CasoModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CasoEstudio")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/events", async (ApplicationDbContext db) =>
{
    var eventos = await db.Eventos
        .Include(e => e.Categoria)
        .Select(e => new
                     {
                         e.Id,
                         e.Titulo,
                         e.Fecha,
                         e.Hora,
                         e.CupoMaximo,
                         Categoria = e.Categoria.Nombre
                     })
        .ToListAsync();

return Results.Ok(eventos);
});
app.MapGet("/api/events/{id}", async (int id, ApplicationDbContext db) =>
{
    var evento = await db.Eventos
        .Include(e => e.Categoria)
        .Where(e => e.Id == id)
        .Select(e => new
        {
            e.Id,
            e.Titulo,
            e.Descripcion,
            e.Fecha,
            e.Hora,
            e.CupoMaximo,
            Categoria = e.Categoria.Nombre
        })
        .FirstOrDefaultAsync();

    return evento is not null ? Results.Ok(evento) : Results.NotFound();
});


app.Run();


