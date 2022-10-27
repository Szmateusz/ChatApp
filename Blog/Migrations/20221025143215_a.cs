using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomsenderId",
                table: "ConnectingToRooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserSenderId",
                table: "ConnectingToRooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConnectingToRooms_RoomsenderId",
                table: "ConnectingToRooms",
                column: "RoomsenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectingToRooms_UserSenderId",
                table: "ConnectingToRooms",
                column: "UserSenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectingToRooms_AspNetUsers_UserSenderId",
                table: "ConnectingToRooms",
                column: "UserSenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectingToRooms_Rooms_RoomsenderId",
                table: "ConnectingToRooms",
                column: "RoomsenderId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectingToRooms_AspNetUsers_UserSenderId",
                table: "ConnectingToRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ConnectingToRooms_Rooms_RoomsenderId",
                table: "ConnectingToRooms");

            migrationBuilder.DropIndex(
                name: "IX_ConnectingToRooms_RoomsenderId",
                table: "ConnectingToRooms");

            migrationBuilder.DropIndex(
                name: "IX_ConnectingToRooms_UserSenderId",
                table: "ConnectingToRooms");

            migrationBuilder.DropColumn(
                name: "RoomsenderId",
                table: "ConnectingToRooms");

            migrationBuilder.DropColumn(
                name: "UserSenderId",
                table: "ConnectingToRooms");
        }
    }
}
