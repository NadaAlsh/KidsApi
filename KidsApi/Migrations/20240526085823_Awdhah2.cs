using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidsApi.Migrations
{
    /// <inheritdoc />
    public partial class Awdhah2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Categories_CategoryId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Child_childId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Parent_ParentId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Task");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ParentId",
                table: "Task",
                newName: "IX_Task_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_childId",
                table: "Task",
                newName: "IX_Task_childId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_CategoryId",
                table: "Task",
                newName: "IX_Task_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Categories_CategoryId",
                table: "Task",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Task_Categories_CategoryId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Child_childId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Parent_ParentId",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_Task_ParentId",
                table: "Tasks",
                newName: "IX_Tasks_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_childId",
                table: "Tasks",
                newName: "IX_Tasks_childId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_CategoryId",
                table: "Tasks",
                newName: "IX_Tasks_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Categories_CategoryId",
                table: "Tasks",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Child_childId",
                table: "Tasks",
                column: "childId",
                principalTable: "Child",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Parent_ParentId",
                table: "Tasks",
                column: "ParentId",
                principalTable: "Parent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
