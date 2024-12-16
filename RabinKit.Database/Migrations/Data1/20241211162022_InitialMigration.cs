using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RabinKit.Database.Migrations 
{ 
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    group = table.Column<string>(type: "TEXT", nullable: true),
                    year = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "INTEGER", nullable: true),
                    updated_at = table.Column<DateTime>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "task_components",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    toolbox = table.Column<string>(type: "TEXT", nullable: false),
                    input = table.Column<string>(type: "TEXT", nullable: false),
                    output = table.Column<string>(type: "NUMERIC", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    playground = table.Column<string>(type: "NUMERIC", nullable: true),
                    is_test = table.Column<string>(type: "TEXT", nullable: false, defaultValueSql: "0"),
                    created_at = table.Column<DateTime>(type: "INTEGER", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_components", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    task_id = table.Column<int>(type: "INTEGER", nullable: false),
                    is_passed = table.Column<int>(type: "INTEGER", nullable: false),
                    solution_time = table.Column<TimeSpan>(type: "INTEGER", nullable: true),
                    created_at = table.Column<DateTime>(type: "INTEGER", nullable: true),
                    updated_at = table.Column<DateTime>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "performance_tests",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    task_description_id = table.Column<int>(type: "INTEGER", nullable: false),
                    prepare_script = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<string>(type: "TEXT", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<string>(type: "TEXT", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_performance_tests", x => x.id);
                    table.ForeignKey(
                        name: "FK_performance_tests_task_components_task_description_id",
                        column: x => x.task_description_id,
                        principalTable: "task_components",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_attempts",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    task_id = table.Column<int>(type: "INTEGER", nullable: false),
                    code = table.Column<string>(type: "TEXT", nullable: false),
                    inputs = table.Column<string>(type: "TEXT", nullable: true),
                    result = table.Column<string>(type: "TEXT", nullable: false),
                    is_passed = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_attempts", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_attempts_task_components_task_id",
                        column: x => x.task_id,
                        principalTable: "task_components",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_values",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    task_id = table.Column<int>(type: "INTEGER", nullable: false),
                    input_vars = table.Column<string>(type: "TEXT", nullable: false),
                    output_vars = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_values", x => x.id);
                    table.ForeignKey(
                        name: "FK_test_values_task_components_task_id",
                        column: x => x.task_id,
                        principalTable: "task_components",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "performance_test_attempts",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    runs = table.Column<string>(type: "TEXT", nullable: false),
                    performance_test_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<string>(type: "TEXT", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<string>(type: "TEXT", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_performance_test_attempts", x => x.id);
                    table.ForeignKey(
                        name: "FK_performance_test_attempts_performance_tests_performance_test_id",
                        column: x => x.performance_test_id,
                        principalTable: "performance_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_test_attempt_relations",
                columns: table => new
                {
                    test_id = table.Column<int>(type: "INTEGER", nullable: false),
                    attempt_id = table.Column<int>(type: "INTEGER", nullable: false),
                    result = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_test_attempt_relations", x => new { x.attempt_id, x.test_id });
                    table.ForeignKey(
                        name: "FK_task_test_attempt_relations_task_attempts_attempt_id",
                        column: x => x.attempt_id,
                        principalTable: "task_attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_test_attempt_relations_test_values_test_id",
                        column: x => x.test_id,
                        principalTable: "test_values",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_performance_test_attempts_performance_test_id",
                table: "performance_test_attempts",
                column: "performance_test_id");

            migrationBuilder.CreateIndex(
                name: "ix_performance_tests_task_description_id",
                table: "performance_tests",
                column: "task_description_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_Id",
                table: "students",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_attempts_id",
                table: "task_attempts",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_task_attempts_task_components_id",
                table: "task_attempts",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_components_id",
                table: "task_components",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_status_id",
                table: "task_status",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_status_task_id",
                table: "task_status",
                column: "task_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_task_test_attempt_relations_test_id",
                table: "task_test_attempt_relations",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "ix_test_values_task_components_id",
                table: "test_values",
                column: "task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "performance_test_attempts");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "task_status");

            migrationBuilder.DropTable(
                name: "task_test_attempt_relations");

            migrationBuilder.DropTable(
                name: "performance_tests");

            migrationBuilder.DropTable(
                name: "task_attempts");

            migrationBuilder.DropTable(
                name: "test_values");

            migrationBuilder.DropTable(
                name: "task_components");
        }
    }
}
