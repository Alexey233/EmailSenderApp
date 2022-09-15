using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailSenderApplication.Migrations
{
    public partial class changeNameInModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsEmailSend",
                table: "Email",
                newName: "IsSuccessfulSend");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSuccessfulSend",
                table: "Email",
                newName: "IsEmailSend");
        }
    }
}
