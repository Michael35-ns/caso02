using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasoModels
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<InscripcionEvento> InscripcionesEventos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Correo).IsUnique();

            // Mantenemos solo esta como cascade
            modelBuilder.Entity<Usuario>().HasMany(u => u.CategoriasRegistradas)
                .WithOne(c => c.UsuarioRegistro)
                .HasForeignKey(c => c.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.Cascade);

            // Estas se ajustan
            modelBuilder.Entity<Usuario>().HasMany(u => u.EventosRegistrados)
                .WithOne(e => e.UsuarioRegistro)
                .HasForeignKey(e => e.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InscripcionEvento>()
                .HasOne(i => i.Usuario)
                .WithMany()
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // <- clave

            modelBuilder.Entity<InscripcionEvento>()
                .HasOne(i => i.Evento)
                .WithMany()
                .HasForeignKey(i => i.EventoId)
                .OnDelete(DeleteBehavior.Cascade); // esta puede quedar

            modelBuilder.Entity<Categoria>().HasMany(c => c.Eventos)
                .WithOne(e => e.Categoria)
                .HasForeignKey(e => e.CategoriaId);

            modelBuilder.Entity<InscripcionEvento>()
                .HasIndex(i => new { i.UsuarioId, i.EventoId }).IsUnique();

            base.OnModelCreating(modelBuilder);

            // =================== Seeders ===================

            // Usuarios
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, NombreUsuario = "admin", NombreCompleto = "Administrador General", Correo = "admin@eventos.com", Telefono = "88888888", Contrasena = "admin123", Rol = "Admin" },
                new Usuario { Id = 2, NombreUsuario = "organizador1", NombreCompleto = "Juan Pérez", Correo = "juan@eventos.com", Telefono = "89999999", Contrasena = "organizador123", Rol = "Organizador" },
                new Usuario { Id = 3, NombreUsuario = "usuario1", NombreCompleto = "María Gómez", Correo = "maria@eventos.com", Telefono = "87777777", Contrasena = "usuario123", Rol = "Usuario" }
            );

            // Categorías
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Tecnología", Descripcion = "Eventos de innovación y tecnología", Estado = true, FechaRegistro = DateTime.Now, UsuarioRegistroId = 1 },
                new Categoria { Id = 2, Nombre = "Arte", Descripcion = "Eventos culturales y artísticos", Estado = true, FechaRegistro = DateTime.Now, UsuarioRegistroId = 2 },
                new Categoria { Id = 3, Nombre = "Deportes", Descripcion = "Eventos deportivos", Estado = true, FechaRegistro = DateTime.Now, UsuarioRegistroId = 1 }
            );

            // Eventos
            modelBuilder.Entity<Evento>().HasData(
                new Evento { Id = 1, Titulo = "Expo Tecnología 2025", Descripcion = "Feria de tecnología e innovación.", CategoriaId = 1, Fecha = new DateTime(2025, 5, 20), Hora = "09:00", Duracion = "4h", Ubicacion = "Centro de Convenciones", CupoMaximo = 100, FechaRegistro = DateTime.Now, UsuarioRegistroId = 2 },
                new Evento { Id = 2, Titulo = "Galería de Arte Local", Descripcion = "Muestra de artistas nacionales.", CategoriaId = 2, Fecha = new DateTime(2025, 6, 10), Hora = "14:00", Duracion = "3h", Ubicacion = "Museo de Arte", CupoMaximo = 50, FechaRegistro = DateTime.Now, UsuarioRegistroId = 2 },
                new Evento { Id = 3, Titulo = "Carrera 10K", Descripcion = "Evento deportivo abierto al público.", CategoriaId = 3, Fecha = new DateTime(2025, 7, 15), Hora = "07:00", Duracion = "2h", Ubicacion = "Parque Central", CupoMaximo = 200, FechaRegistro = DateTime.Now, UsuarioRegistroId = 1 }
            );

            // Inscripciones
            modelBuilder.Entity<InscripcionEvento>().HasData(
                new InscripcionEvento { Id = 1, UsuarioId = 3, EventoId = 1, FechaInscripcion = DateTime.Now },
                new InscripcionEvento { Id = 2, UsuarioId = 3, EventoId = 2, FechaInscripcion = DateTime.Now },
                new InscripcionEvento { Id = 3, UsuarioId = 2, EventoId = 3, FechaInscripcion = DateTime.Now }
            );
        }

    }
}

