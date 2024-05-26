using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidsApi.Migrations
{
    /// <inheritdoc />
    public partial class Haya4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Parent",
                newName: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Parent",
                newName: "Id");
        }
    }
}
