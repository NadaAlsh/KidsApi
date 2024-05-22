using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KidsApi.Migrations
{
    /// <inheritdoc />
    public partial class testingChildren : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Child_Parent_ParentId",
                table: "Child");

            migrationBuilder.DropTable(
                name: "ChildTask");

            migrationBuilder.DropIndex(
                name: "IX_Child_ParentId",
                table: "Child");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Task",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "childId",
                table: "Task",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isCompleted",
                table: "Task",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ChildId",
                table: "Parent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isCompleted",
                table: "Child",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTask",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    TasksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTask", x => new { x.CategoryId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_CategoryTask_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTask_Task_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Clean" },
                    { 2, "Play" },
                    { 3, "Outdoor Activity" },
                    { 4, "Study" },
                    { 5, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_childId",
                table: "Task",
                column: "childId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ParentId",
                table: "Task",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTask_TasksId",
                table: "CategoryTask",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Child_childId",
                table: "Task",
                column: "childId",
                principalTable: "Child",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Parent_ParentId",
                table: "Task",
                column: "ParentId",
                principalTable: "Parent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Child_childId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Parent_ParentId",
                table: "Task");

            migrationBuilder.DropTable(
                name: "CategoryTask");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Task_childId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_ParentId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "childId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "isCompleted",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "Parent");

            migrationBuilder.DropColumn(
                name: "isCompleted",
                table: "Child");

            migrationBuilder.CreateTable(
                name: "ChildTask",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "int", nullable: false),
                    childrenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildTask", x => new { x.TasksId, x.childrenId });
                    table.ForeignKey(
                        name: "FK_ChildTask_Child_childrenId",
                        column: x => x.childrenId,
                        principalTable: "Child",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildTask_Task_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Child_ParentId",
                table: "Child",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildTask_childrenId",
                table: "ChildTask",
                column: "childrenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Child_Parent_ParentId",
                table: "Child",
                column: "ParentId",
                principalTable: "Parent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
