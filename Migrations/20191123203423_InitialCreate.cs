using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ecapi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Datums",
                columns: table => new
                {
                    year = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    state = table.Column<string>(nullable: true),
                    report = table.Column<string>(nullable: true),
                    farmtype = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    category_value = table.Column<string>(nullable: true),
                    category2 = table.Column<string>(nullable: true),
                    category2_value = table.Column<string>(nullable: true),
                    variable_id = table.Column<string>(nullable: true),
                    variable_name = table.Column<string>(nullable: true),
                    variable_sequence = table.Column<int>(nullable: false),
                    variable_level = table.Column<int>(nullable: true),
                    variable_unit = table.Column<string>(nullable: true),
                    variable_description = table.Column<string>(nullable: true),
                    variable_is_invalid = table.Column<bool>(nullable: false),
                    estimate = table.Column<double>(nullable: false),
                    median = table.Column<double>(nullable: true),
                    statistic = table.Column<string>(nullable: true),
                    rse = table.Column<double>(nullable: false),
                    unreliable_estimate = table.Column<int>(nullable: false),
                    decimal_display = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datums", x => x.year);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Datums");
        }
    }
}
