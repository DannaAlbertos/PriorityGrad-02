using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PriorityGrad.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaTarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tarea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Materia = table.Column<string>(type: "text", nullable: false),
                    Valor = table.Column<int>(type: "integer", nullable: false),
                    Dificultad = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Comentario = table.Column<string>(type: "text", nullable: true),
                    NombreProfesor = table.Column<string>(type: "text", nullable: true),
                    Instrucciones = table.Column<string>(type: "text", nullable: true),
                    HoraEntrega = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    ImagenUrl = table.Column<string>(type: "text", nullable: true),
                    ColorHex = table.Column<string>(type: "text", nullable: true),
                    ColorTexto = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarea", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarea");
        }
    }
}
