using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    public partial class connectingupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectingToRooms_Rooms_RoomsenderId",
                table: "ConnectingToRooms");

            migrationBuilder.RenameColumn(
                name: "RoomsenderId",
                table: "ConnectingToRooms",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_ConnectingToRooms_RoomsenderId",
                table: "ConnectingToRooms",
                newName: "IX_ConnectingToRooms_RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectingToRooms_Rooms_RoomId",
                table: "ConnectingToRooms",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectingToRooms_Rooms_RoomId",
                table: "ConnectingToRooms");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "ConnectingToRooms",
                newName: "RoomsenderId");

            migrationBuilder.RenameIndex(
                name: "IX_ConnectingToRooms_RoomId",
                table: "ConnectingToRooms",
                newName: "IX_ConnectingToRooms_RoomsenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectingToRooms_Rooms_RoomsenderId",
                table: "ConnectingToRooms",
                column: "RoomsenderId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
