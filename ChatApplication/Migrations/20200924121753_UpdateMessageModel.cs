using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApplication.Migrations
{
    public partial class UpdateMessageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserId",
                table: "Messages");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "79b11c31-f4e6-4959-b35c-0d770a5691bd");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aae57ae2-a883-4070-9425-429fc2009ff2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cf3adf18-94b6-47b4-82d3-b304dee8393a");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Messages",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "925df1c3-f1e5-4f12-89a4-bd8508bd495f", 0, "cbf2976f-f05d-4042-94f7-a95bb40f2402", "M.Sheykhveysi4680@Gmail.com", true, false, null, "M.SHEYKHVEYSI4680@GMAIL.COM", "M.SHEYKHVEYSI4680@GMAIL.COM", "AQAAAAEAACcQAAAAEGzCF0acla+1fH+xg44buxW0qT6fF7WUhIRvxdt6JoKZpUNyhCoY+/7cCpSMrDFQ2A==", "09307514680", true, "8aff915c-1f2b-4fe2-b068-a3727d1b4777", false, "M.Sheykhveysi4680@Gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c63e4417-e901-42c2-a7cc-63d1456b07ea", 0, "9c38aa01-6948-4e85-8791-ac6311e95424", "a.alavi1234@Gmail.com", true, false, null, "A.ALAVI1234@GMAIL.COM", "A.ALAVI1234@GMAIL.COM", "AQAAAAEAACcQAAAAECoxW6bVf9Cb9pS8JkYZJlOBSRUIUB88C7+ZBvMLZ7DTzZQN+CbvvViSiklJ/yv9Pw==", "09368560182", true, "7cbee101-f1e8-43ef-8c6c-47a29a62c3fa", false, "a.alavi1234@Gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a806a7a8-ae27-4fda-823a-641810297cea", 0, "0d673da5-e0c5-4dea-9d52-33c1a8387410", "t.taheri1234@gmail.com", true, false, null, "T.TAHERI1234@GMAIL.COM", "T.TAHERI1234@GMAIL.COM", "AQAAAAEAACcQAAAAEH4sqj2D1iYNLk+JTCafjUt+Xc1ddIoK6X9xNLPTJMkITnJwqvaCaBwkpt03oSObGw==", "09309303030", true, "4eda6fb2-5f1c-4709-918c-3ac319d6a84d", false, "t.taheri1234@gmail.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderId",
                table: "Messages");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "925df1c3-f1e5-4f12-89a4-bd8508bd495f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a806a7a8-ae27-4fda-823a-641810297cea");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c63e4417-e901-42c2-a7cc-63d1456b07ea");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cf3adf18-94b6-47b4-82d3-b304dee8393a", 0, "755d681a-f126-4fb2-a758-82addac4c0eb", "M.Sheykhveysi4680@Gmail.com", true, false, null, "M.SHEYKHVEYSI4680@GMAIL.COM", "M.SHEYKHVEYSI4680@GMAIL.COM", "AQAAAAEAACcQAAAAEKIFhxpDMZBWXEBKo/1pd46+H12fj3pmZOgeOBov6e32j3yFYiw7a4INT9LlmSKoXg==", "09307514680", true, "817a5d4c-e7aa-4cc5-b46a-79fa868de3a4", false, "M.Sheykhveysi4680@Gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "aae57ae2-a883-4070-9425-429fc2009ff2", 0, "4cc59098-5418-4af3-a9bb-29437a802d53", "a.alavi1234@Gmail.com", true, false, null, "A.ALAVI1234@GMAIL.COM", "A.ALAVI1234@GMAIL.COM", "AQAAAAEAACcQAAAAEHDgQqIOHcPecteL/oDIGRFinDjULMMVizY1ndhIUTWz9Mnqo9+w615mTVMsd3lxRQ==", "09307514680", true, "d361fc8d-f9ed-40e8-8a72-9cd0f4b817fc", false, "a.alavi1234@Gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "79b11c31-f4e6-4959-b35c-0d770a5691bd", 0, "af3597f0-069f-4480-b7ff-3dba216a99ab", "t.taheri1234@gmail.com", true, false, null, "T.TAHERI1234@GMAIL.COM", "T.TAHERI1234@GMAIL.COM", "AQAAAAEAACcQAAAAEFzsnLV30iRwPydvVIKWM0falVkXdorw8a9d24H+Cy88j8CV8WKfsbyBok58widTYA==", "09307514680", true, "540afbdf-3125-494d-bff0-046a612b66a1", false, "t.taheri1234@gmail.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
