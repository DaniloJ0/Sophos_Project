using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "facultad",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facultad", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "periodo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    year = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    semestre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_periodo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "alumno",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    apellido = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    semestre = table.Column<int>(type: "int", nullable: true),
                    credt_disp = table.Column<int>(type: "int", nullable: false),
                    id_dept = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alumno", x => x.id);
                    table.ForeignKey(
                        name: "FK_alumno_facultad",
                        column: x => x.id_dept,
                        principalTable: "facultad",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "profesor",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    apellido = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    max_titulo = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    exp_year = table.Column<int>(type: "int", nullable: false),
                    id_dept = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profesor", x => x.id);
                    table.ForeignKey(
                        name: "FK_profesor_facultad",
                        column: x => x.id_dept,
                        principalTable: "facultad",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cursos_realizados",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_alumno = table.Column<int>(type: "int", nullable: false),
                    id_curso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cursos_realizados", x => x.id);
                    table.ForeignKey(
                        name: "FK_cursos_realizados_alumno",
                        column: x => x.id_alumno,
                        principalTable: "alumno",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "curso",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    creditos = table.Column<int>(type: "int", nullable: false),
                    cupos = table.Column<int>(type: "int", nullable: false),
                    id_profesor = table.Column<int>(type: "int", nullable: false),
                    id_periodo = table.Column<int>(type: "int", nullable: false),
                    id_curso_pre = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_curso", x => x.id);
                    table.ForeignKey(
                        name: "FK_curso_periodo",
                        column: x => x.id_periodo,
                        principalTable: "periodo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_curso_profesor",
                        column: x => x.id_profesor,
                        principalTable: "profesor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "matricula_alumno",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_curso = table.Column<int>(type: "int", nullable: false),
                    id_alumno = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matricula_alumno", x => x.id);
                    table.ForeignKey(
                        name: "FK_matricula_alumno_alumno",
                        column: x => x.id_alumno,
                        principalTable: "alumno",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_matricula_alumno_curso",
                        column: x => x.id_curso,
                        principalTable: "curso",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_alumno_id_dept",
                table: "alumno",
                column: "id_dept");

            migrationBuilder.CreateIndex(
                name: "IX_curso_id_periodo",
                table: "curso",
                column: "id_periodo");

            migrationBuilder.CreateIndex(
                name: "IX_curso_id_profesor",
                table: "curso",
                column: "id_profesor");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_realizados_id_alumno",
                table: "cursos_realizados",
                column: "id_alumno");

            migrationBuilder.CreateIndex(
                name: "IX_matricula_alumno_id_alumno",
                table: "matricula_alumno",
                column: "id_alumno");

            migrationBuilder.CreateIndex(
                name: "IX_matricula_alumno_id_curso",
                table: "matricula_alumno",
                column: "id_curso");

            migrationBuilder.CreateIndex(
                name: "IX_profesor_id_dept",
                table: "profesor",
                column: "id_dept");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cursos_realizados");

            migrationBuilder.DropTable(
                name: "matricula_alumno");

            migrationBuilder.DropTable(
                name: "alumno");

            migrationBuilder.DropTable(
                name: "curso");

            migrationBuilder.DropTable(
                name: "periodo");

            migrationBuilder.DropTable(
                name: "profesor");

            migrationBuilder.DropTable(
                name: "facultad");
        }
    }
}
