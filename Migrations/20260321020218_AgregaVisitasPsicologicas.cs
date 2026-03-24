using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicioMedico.Migrations
{
    /// <inheritdoc />
    public partial class AgregaVisitasPsicologicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preinscripciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreinscripcionId = table.Column<int>(type: "int", nullable: false),
                    Folio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarreraSolicitada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Promedio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MedioDifusion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaPreinscripcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoPreinscripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preinscripciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitasPsicologicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaVisita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    TerapiaPrevia = table.Column<bool>(type: "bit", nullable: false),
                    MotivoConsultaPrevia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicacionPsiquiatrica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotivoConsulta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitasPsicologicas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreinscripcionDatosPersonales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreinscripcionId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CURP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoCivil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreinscripcionDatosPersonales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreinscripcionDatosPersonales_Preinscripciones_PreinscripcionId",
                        column: x => x.PreinscripcionId,
                        principalTable: "Preinscripciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreinscripcionDatosPersonales_PreinscripcionId",
                table: "PreinscripcionDatosPersonales",
                column: "PreinscripcionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreinscripcionDatosPersonales");

            migrationBuilder.DropTable(
                name: "VisitasPsicologicas");

            migrationBuilder.DropTable(
                name: "Preinscripciones");
        }
    }
}
