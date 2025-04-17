using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CasoModels.Migrations
{
    /// <inheritdoc />
    public partial class first_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioRegistroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_Usuarios_UsuarioRegistroId",
                        column: x => x.UsuarioRegistroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duracion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CupoMaximo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioRegistroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Eventos_Usuarios_UsuarioRegistroId",
                        column: x => x.UsuarioRegistroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InscripcionesEventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscripcionesEventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InscripcionesEventos_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InscripcionesEventos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Contrasena", "Correo", "NombreCompleto", "NombreUsuario", "Rol", "Telefono" },
                values: new object[,]
                {
                    { 1, "admin123", "admin@eventos.com", "Administrador General", "admin", "Admin", "88888888" },
                    { 2, "organizador123", "juan@eventos.com", "Juan Pérez", "organizador1", "Organizador", "89999999" },
                    { 3, "usuario123", "maria@eventos.com", "María Gómez", "usuario1", "Usuario", "87777777" }
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Descripcion", "Estado", "FechaRegistro", "Nombre", "UsuarioRegistroId" },
                values: new object[,]
                {
                    { 1, "Eventos de innovación y tecnología", true, new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2082), "Tecnología", 1 },
                    { 2, "Eventos culturales y artísticos", true, new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2084), "Arte", 2 },
                    { 3, "Eventos deportivos", true, new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2086), "Deportes", 1 }
                });

            migrationBuilder.InsertData(
                table: "Eventos",
                columns: new[] { "Id", "CategoriaId", "CupoMaximo", "Descripcion", "Duracion", "Fecha", "FechaRegistro", "Hora", "Titulo", "Ubicacion", "UsuarioRegistroId" },
                values: new object[,]
                {
                    { 1, 1, 100, "Feria de tecnología e innovación.", "4h", new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2103), "09:00", "Expo Tecnología 2025", "Centro de Convenciones", 2 },
                    { 2, 2, 50, "Muestra de artistas nacionales.", "3h", new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2106), "14:00", "Galería de Arte Local", "Museo de Arte", 2 },
                    { 3, 3, 200, "Evento deportivo abierto al público.", "2h", new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2108), "07:00", "Carrera 10K", "Parque Central", 1 }
                });

            migrationBuilder.InsertData(
                table: "InscripcionesEventos",
                columns: new[] { "Id", "EventoId", "FechaInscripcion", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2122), 3 },
                    { 2, 2, new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2124), 3 },
                    { 3, 3, new DateTime(2025, 4, 15, 20, 21, 56, 96, DateTimeKind.Local).AddTicks(2125), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_UsuarioRegistroId",
                table: "Categorias",
                column: "UsuarioRegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_CategoriaId",
                table: "Eventos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioRegistroId",
                table: "Eventos",
                column: "UsuarioRegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_InscripcionesEventos_EventoId",
                table: "InscripcionesEventos",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_InscripcionesEventos_UsuarioId_EventoId",
                table: "InscripcionesEventos",
                columns: new[] { "UsuarioId", "EventoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InscripcionesEventos");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
