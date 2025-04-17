﻿// <auto-generated />
using System;
using CasoModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CasoModels.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250416022156_first_migration")]
    partial class first_migration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CasoModels.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioRegistroId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioRegistroId");

                    b.ToTable("Categorias");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "Eventos de innovación y tecnología",
                            Estado = true,
                            FechaRegistro = new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2082),
                            Nombre = "Tecnología",
                            UsuarioRegistroId = 1
                        },
                        new
                        {
                            Id = 2,
                            Descripcion = "Eventos culturales y artísticos",
                            Estado = true,
                            FechaRegistro = new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2084),
                            Nombre = "Arte",
                            UsuarioRegistroId = 2
                        },
                        new
                        {
                            Id = 3,
                            Descripcion = "Eventos deportivos",
                            Estado = true,
                            FechaRegistro = new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2086),
                            Nombre = "Deportes",
                            UsuarioRegistroId = 1
                        });
                });

            modelBuilder.Entity("CasoModels.Evento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<int>("CupoMaximo")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Duracion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Hora")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ubicacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioRegistroId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("UsuarioRegistroId");

                    b.ToTable("Eventos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoriaId = 1,
                            CupoMaximo = 100,
                            Descripcion = "Feria de tecnología e innovación.",
                            Duracion = "4h",
                            Fecha = new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaRegistro = new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2103),
                            Hora = "09:00",
                            Titulo = "Expo Tecnología 2025",
                            Ubicacion = "Centro de Convenciones",
                            UsuarioRegistroId = 2
                        },
                        new
                        {
                            Id = 2,
                            CategoriaId = 2,
                            CupoMaximo = 50,
                            Descripcion = "Muestra de artistas nacionales.",
                            Duracion = "3h",
                            Fecha = new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaRegistro = new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2106),
                            Hora = "14:00",
                            Titulo = "Galería de Arte Local",
                            Ubicacion = "Museo de Arte",
                            UsuarioRegistroId = 2
                        },
                        new
                        {
                            Id = 3,
                            CategoriaId = 3,
                            CupoMaximo = 200,
                            Descripcion = "Evento deportivo abierto al público.",
                            Duracion = "2h",
                            Fecha = new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaRegistro = new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2108),
                            Hora = "07:00",
                            Titulo = "Carrera 10K",
                            Ubicacion = "Parque Central",
                            UsuarioRegistroId = 1
                        });
                });

            modelBuilder.Entity("CasoModels.InscripcionEvento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EventoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaInscripcion")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventoId");

                    b.HasIndex("UsuarioId", "EventoId")
                        .IsUnique();

                    b.ToTable("InscripcionesEventos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EventoId = 1,
                            FechaInscripcion = new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2122),
                            UsuarioId = 3
                        },
                        new
                        {
                            Id = 2,
                            EventoId = 2,
                            FechaInscripcion = new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2124),
                            UsuarioId = 3
                        },
                        new
                        {
                            Id = 3,
                            EventoId = 3,
                            FechaInscripcion = new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2125),
                            UsuarioId = 2
                        });
                });

            modelBuilder.Entity("CasoModels.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contrasena")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NombreCompleto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Correo")
                        .IsUnique();

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Contrasena = "admin123",
                            Correo = "admin@eventos.com",
                            NombreCompleto = "Administrador General",
                            NombreUsuario = "admin",
                            Rol = "Admin",
                            Telefono = "88888888"
                        },
                        new
                        {
                            Id = 2,
                            Contrasena = "organizador123",
                            Correo = "juan@eventos.com",
                            NombreCompleto = "Juan Pérez",
                            NombreUsuario = "organizador1",
                            Rol = "Organizador",
                            Telefono = "89999999"
                        },
                        new
                        {
                            Id = 3,
                            Contrasena = "usuario123",
                            Correo = "maria@eventos.com",
                            NombreCompleto = "María Gómez",
                            NombreUsuario = "usuario1",
                            Rol = "Usuario",
                            Telefono = "87777777"
                        });
                });

            modelBuilder.Entity("CasoModels.Categoria", b =>
                {
                    b.HasOne("CasoModels.Usuario", "UsuarioRegistro")
                        .WithMany("CategoriasRegistradas")
                        .HasForeignKey("UsuarioRegistroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UsuarioRegistro");
                });

            modelBuilder.Entity("CasoModels.Evento", b =>
                {
                    b.HasOne("CasoModels.Categoria", "Categoria")
                        .WithMany("Eventos")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CasoModels.Usuario", "UsuarioRegistro")
                        .WithMany("EventosRegistrados")
                        .HasForeignKey("UsuarioRegistroId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("UsuarioRegistro");
                });

            modelBuilder.Entity("CasoModels.InscripcionEvento", b =>
                {
                    b.HasOne("CasoModels.Evento", "Evento")
                        .WithMany()
                        .HasForeignKey("EventoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CasoModels.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Evento");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("CasoModels.Categoria", b =>
                {
                    b.Navigation("Eventos");
                });

            modelBuilder.Entity("CasoModels.Usuario", b =>
                {
                    b.Navigation("CategoriasRegistradas");

                    b.Navigation("EventosRegistrados");
                });
#pragma warning restore 612, 618
        }
    }
}
