using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Snake.Migrations
{
    public partial class secon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_AspNetUsers_UserModelId",
                table: "Channels");

            migrationBuilder.RenameColumn(
                name: "UserModelId",
                table: "Channels",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Channels_UserModelId",
                table: "Channels",
                newName: "IX_Channels_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_AspNetUsers_UserID",
                table: "Channels",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_AspNetUsers_UserID",
                table: "Channels");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Channels",
                newName: "UserModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Channels_UserID",
                table: "Channels",
                newName: "IX_Channels_UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_AspNetUsers_UserModelId",
                table: "Channels",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
