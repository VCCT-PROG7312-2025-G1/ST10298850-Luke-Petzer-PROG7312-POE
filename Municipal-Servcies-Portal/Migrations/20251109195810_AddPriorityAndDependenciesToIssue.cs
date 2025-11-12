using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Municipal_Servcies_Portal.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityAndDependenciesToIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DependenciesJson",
                table: "Issues",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Issues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DependenciesJson",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Issues");
        }
    }
}
