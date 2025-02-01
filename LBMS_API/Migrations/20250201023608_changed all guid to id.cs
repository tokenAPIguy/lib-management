using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LBMS_API.Migrations
{
    /// <inheritdoc />
    public partial class changedallguidtoid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanBeSubCategory",
                table: "Categories",
                newName: "CanBeMainCategory");

            migrationBuilder.RenameColumn(
                name: "Category_CanBeSubCategory",
                table: "Books",
                newName: "Category_CanBeMainCategory");

            migrationBuilder.AlterColumn<int>(
                name: "BookID",
                table: "Loans",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanBeMainCategory",
                table: "Categories",
                newName: "CanBeSubCategory");

            migrationBuilder.RenameColumn(
                name: "Category_CanBeMainCategory",
                table: "Books",
                newName: "Category_CanBeSubCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookID",
                table: "Loans",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "Books",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }
    }
}
