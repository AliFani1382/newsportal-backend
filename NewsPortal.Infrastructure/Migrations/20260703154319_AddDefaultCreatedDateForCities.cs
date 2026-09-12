using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsPortal.Infrastructure.Migrations
{
    public partial class AddDefaultCreatedDateForCities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE dbo.Cities
                ADD CONSTRAINT DF_Cities_CreatedDate
                DEFAULT (SYSUTCDATETIME()) FOR CreatedDate;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE dbo.Cities
                DROP CONSTRAINT DF_Cities_CreatedDate;
            ");
        }
    }
}
