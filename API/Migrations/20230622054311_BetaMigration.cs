using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class BetaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_roles", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_rooms",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    floor = table.Column<int>(type: "int", nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_rooms", x => x.guid);
                });

            migrationBuilder.CreateTable(
            name: "tb_m_employees",
            columns: table => new
            {
                EmployeesGuid = table.Column<Guid>(nullable: false),
                FirstName = table.Column<string>(nullable: true),
                LastName = table.Column<string>(nullable: true),
                Nik = table.Column<string>(nullable: true),
                Email = table.Column<string>(nullable: true),
                PhoneNumber = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tb_m_employees", x => x.EmployeesGuid);
            });

            migrationBuilder.CreateTable(
                name: "tb_m_university",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_university", x => x.guid);
                });

            migrationBuilder.CreateTable(
            name: "AccountEmployee",
            columns: table => new
            {
                AccountGuid = table.Column<Guid>(nullable: false),
                EmployeesGuid = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccountEmployee", x => new { x.AccountGuid, x.EmployeesGuid });
                table.ForeignKey(
                    name: "FK_AccountEmployee_tb_m_employees_EmployeesGuid",
                    column: x => x.EmployeesGuid,
                    principalTable: "tb_m_employees",
                    principalColumn: "EmployeesGuid",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false),
                    expired_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_account_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_account_roles", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_account_roles_tb_m_accounts_account_guid",
                        column: x => x.account_guid,
                        principalTable: "tb_m_accounts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_tr_account_roles_tb_m_roles_role_guid",
                        column: x => x.role_guid,
                        principalTable: "tb_m_roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_educations",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    major = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    degree = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    gpa = table.Column<float>(type: "real", nullable: false),
                    university_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_educations", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_educations_tb_m_university_university_guid",
                        column: x => x.university_guid,
                        principalTable: "tb_m_university",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateTable(
           name: "tb_tr_bookings",
           columns: table => new
           {
               BookingsGuid = table.Column<Guid>(nullable: false),
               RoomGuid = table.Column<Guid>(nullable: false),
               EmployeesGuid = table.Column<Guid>(nullable: false),
               CheckInDate = table.Column<DateTime>(nullable: false),
               CheckOutDate = table.Column<DateTime>(nullable: false)
           },
           constraints: table =>
           {
               table.PrimaryKey("PK_tb_tr_bookings", x => x.BookingsGuid);
               table.ForeignKey(
                   name: "FK_tb_tr_bookings_tb_m_employees_EmployeesGuid",
                   column: x => x.EmployeesGuid,
                   principalTable: "tb_m_employees",
                   principalColumn: "EmployeesGuid",
                   onDelete: ReferentialAction.Cascade);
           });

            migrationBuilder.CreateIndex(
                name: "IX_AccountEmployee_EmployeesGuid",
                table: "AccountEmployee",
                column: "EmployeesGuid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_educations_university_guid",
                table: "tb_m_educations",
                column: "university_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_BookingsGuid",
                table: "tb_m_employees",
                column: "BookingGuid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_EmployeesGuid",
                table: "tb_m_employees",
                column: "EmployeesGuid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_nik_email_phone_number",
                table: "tb_m_employees",
                columns: new[] { "nik", "email", "phone_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_account_guid_role_guid",
                table: "tb_tr_account_roles",
                columns: new[] { "account_guid", "role_guid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_role_guid",
                table: "tb_tr_account_roles",
                column: "role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_bookings_employee_guid",
                table: "tb_tr_bookings",
                column: "employee_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_bookings_room_guid_employee_guid",
                table: "tb_tr_bookings",
                columns: new[] { "room_guid", "employee_guid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_bookings_RoomGuid1",
                table: "tb_tr_bookings",
                column: "RoomGuid1",
                unique: true,
                filter: "[RoomGuid1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountEmployee_tb_m_accounts_AccountGuid",
                table: "AccountEmployee",
                column: "AccountGuid",
                principalTable: "tb_m_accounts",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountEmployee_tb_m_employees_EmployeesGuid",
                table: "AccountEmployee",
                column: "EmployeesGuid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_accounts_tb_m_employees_guid",
                table: "tb_m_accounts",
                column: "guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_educations_tb_m_employees_guid",
                table: "tb_m_educations",
                column: "guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_employees_tb_tr_bookings_BookingsGuid",
                table: "tb_m_employees",
                column: "BookingsGuid",
                principalTable: "tb_tr_bookings",
                principalColumn: "guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_bookings_tb_m_employees_employee_guid",
                table: "tb_tr_bookings");

            migrationBuilder.DropTable(
                name: "AccountEmployee");

            migrationBuilder.DropTable(
                name: "tb_m_educations");

            migrationBuilder.DropTable(
                name: "tb_tr_account_roles");

            migrationBuilder.DropTable(
                name: "tb_m_university");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_employees");

            migrationBuilder.DropTable(
                name: "tb_tr_bookings");

            migrationBuilder.DropTable(
                name: "tb_m_rooms");
        }
    }
}
