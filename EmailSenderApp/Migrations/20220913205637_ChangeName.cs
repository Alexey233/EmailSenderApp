using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailSenderApplication.Migrations
{
    public partial class ChangeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "carbon_copy_recipients",
                table: "Email",
                newName: "CarbonCopyRecipients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarbonCopyRecipients",
                table: "Email",
                newName: "carbon_copy_recipients");
        }
    }
}
