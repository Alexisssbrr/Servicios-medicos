using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioMedico.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Visitas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Carrera = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaVisita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Talla = table.Column<double>(type: "float", nullable: true),
                    Peso = table.Column<double>(type: "float", nullable: true),
                    TieneAlergias = table.Column<bool>(type: "bit", nullable: false),
                    EspecificarAlergia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnfermedadesCronicas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrecuenciaCardiaca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrecuenciaRespiratoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Saturacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temperatura = table.Column<double>(type: "float", nullable: true),
                    PresionArterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visitas");
        }
    }
}
