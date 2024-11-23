using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RabinKit.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "task_components",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    toolbox = table.Column<string>(type: "TEXT", nullable: false),
                    input_vars = table.Column<string>(type: "TEXT", nullable: false),
                    output_vars = table.Column<string>(type: "TEXT", nullable: false),
                    number = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    playground = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время создания записи"),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время изменения записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_components", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "performance_tests",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    task_description_id = table.Column<long>(type: "INTEGER", nullable: false),
                    prepare_script = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время создания записи"),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время изменения записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_performance_tests", x => x.id);
                    table.ForeignKey(
                        name: "fk_performance_tests_task_components_task_components_id",
                        column: x => x.task_description_id,
                        principalSchema: "public",
                        principalTable: "task_components",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_attempts",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    code = table.Column<string>(type: "TEXT", nullable: false),
                    playground = table.Column<string>(type: "TEXT", nullable: true),
                    parameters = table.Column<string>(type: "TEXT", nullable: true),
                    type = table.Column<int>(type: "INTEGER", nullable: false),
                    is_succeeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    task_description_id = table.Column<long>(type: "INTEGER", nullable: false),
                    result = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время создания записи"),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время изменения записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_attempts", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_attempts_task_components_task_components_id",
                        column: x => x.task_description_id,
                        principalSchema: "public",
                        principalTable: "task_components",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_tests",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    input_vars = table.Column<string>(type: "TEXT", nullable: false),
                    output_vars = table.Column<string>(type: "TEXT", nullable: false),
                    task_id = table.Column<long>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время создания записи"),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время изменения записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_tests", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_tests_task_components_task_id",
                        column: x => x.task_id,
                        principalSchema: "public",
                        principalTable: "task_components",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "performance_test_attempts",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    runs = table.Column<string>(type: "TEXT", nullable: false),
                    performance_test_id = table.Column<long>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время создания записи"),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "now()", comment: "Время изменения записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_performance_test_attempts", x => x.id);
                    table.ForeignKey(
                        name: "fk_performance_test_attempts_performance_tests_performance_test_id",
                        column: x => x.performance_test_id,
                        principalSchema: "public",
                        principalTable: "performance_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_test_attempt_relations",
                schema: "public",
                columns: table => new
                {
                    test_id = table.Column<long>(type: "INTEGER", nullable: false),
                    attempt_id = table.Column<long>(type: "INTEGER", nullable: false),
                    result = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_test_attempt_relations", x => new { x.attempt_id, x.test_id });
                    table.ForeignKey(
                        name: "fk_task_test_attempt_relations_task_attempts_attempt_id",
                        column: x => x.attempt_id,
                        principalSchema: "public",
                        principalTable: "task_attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_task_test_attempt_relations_task_tests_test_id",
                        column: x => x.test_id,
                        principalSchema: "public",
                        principalTable: "task_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_performance_test_attempts_performance_test_id",
                schema: "public",
                table: "performance_test_attempts",
                column: "performance_test_id");

            migrationBuilder.CreateIndex(
                name: "ix_performance_tests_task_components_id",
                schema: "public",
                table: "performance_tests",
                column: "task_components_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_attempts_task_components_id",
                schema: "public",
                table: "task_attempts",
                column: "task_components_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_test_attempt_relations_test_id",
                schema: "public",
                table: "task_test_attempt_relations",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_tests_task_id",
                schema: "public",
                table: "task_tests",
                column: "task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "performance_test_attempts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "task_test_attempt_relations",
                schema: "public");

            migrationBuilder.DropTable(
                name: "performance_tests",
                schema: "public");

            migrationBuilder.DropTable(
                name: "task_attempts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "task_tests",
                schema: "public");

            migrationBuilder.DropTable(
                name: "task_components",
                schema: "public");
        }
    }
}
