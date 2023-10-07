using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IReturnNodePointerProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginViewModel",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", maxLength: 20, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 20, nullable: true),
                    ReturnURL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RememberMe = table.Column<bool>(type: "nvarchar(max)", nullable: true)
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
