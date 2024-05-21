using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidsApi.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Child_Parent_ParentId",
                table: "Child");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildTask_Child_childrenId",
                table: "ChildTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Child",
                table: "Child");

            migrationBuilder.RenameTable(
                name: "Child",
                newName: "Children");

            migrationBuilder.RenameIndex(
                name: "IX_Child_ParentId",
                table: "Children",
                newName: "IX_Children_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Children",
                table: "Children",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Children_Parent_ParentId",
                table: "Children",
                column: "ParentId",
                principalTable: "Parent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildTask_Children_childrenId",
                table: "ChildTask",
                column: "childrenId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Children_Parent_ParentId",
                table: "Children");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildTask_Children_childrenId",
                table: "ChildTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Children",
                table: "Children");

            migrationBuilder.RenameTable(
                name: "Children",
                newName: "Child");

            migrationBuilder.RenameIndex(
                name: "IX_Children_ParentId",
                table: "Child",
                newName: "IX_Child_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Child",
                table: "Child",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Child_Parent_ParentId",
                table: "Child",
                column: "ParentId",
                principalTable: "Parent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildTask_Child_childrenId",
                table: "ChildTask",
                column: "childrenId",
                principalTable: "Child",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
