using Microsoft.EntityFrameworkCore.Migrations;

namespace apidrones.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DroneModel",
                columns: table => new
                {
                    DroneModelId = table.Column<int>(nullable: false),
                    ModelName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DroneModel", x => x.DroneModelId);
                });

            migrationBuilder.CreateTable(
                name: "DroneState",
                columns: table => new
                {
                    DroneStateId = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DroneState", x => x.DroneStateId);
                });

            migrationBuilder.CreateTable(
                name: "Medication",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(60)", unicode: false, nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "varchar(60)", unicode: false, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medication", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Drones",
                columns: table => new
                {
                    Serial_number = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Weight_limit = table.Column<int>(type: "int", maxLength: 500, nullable: false),
                    Battery_capacity = table.Column<int>(type: "int", nullable: false),
                    droneStateId = table.Column<int>(nullable: false, defaultValue: 1),
                    droneModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drones", x => x.Serial_number);
                    table.ForeignKey(
                        name: "FK_Drones_DroneModel_droneModelId",
                        column: x => x.droneModelId,
                        principalTable: "DroneModel",
                        principalColumn: "DroneModelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Drones_DroneState_droneStateId",
                        column: x => x.droneStateId,
                        principalTable: "DroneState",
                        principalColumn: "DroneStateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    IdShipment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DroneId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.IdShipment);
                    table.ForeignKey(
                        name: "FK_Drones_Shipments",
                        column: x => x.DroneId,
                        principalTable: "Drones",
                        principalColumn: "Serial_number",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentsDetails",
                columns: table => new
                {
                    MedicationId = table.Column<string>(nullable: false),
                    IdShipment = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentsDetails", x => new { x.IdShipment, x.MedicationId });
                    table.ForeignKey(
                        name: "FK_Shipment_ShipmentDetails",
                        column: x => x.IdShipment,
                        principalTable: "Shipments",
                        principalColumn: "IdShipment",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medication_ShipmentDetails",
                        column: x => x.MedicationId,
                        principalTable: "Medication",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DroneModel",
                columns: new[] { "DroneModelId", "ModelName" },
                values: new object[,]
                {
                    { 1, "Lightweight" },
                    { 2, "Middleweight" },
                    { 3, "Cruiserweight" },
                    { 4, "Heavyweight" }
                });

            migrationBuilder.InsertData(
                table: "DroneState",
                columns: new[] { "DroneStateId", "StateName" },
                values: new object[,]
                {
                    { 1, "IDLE" },
                    { 2, "LOADING" },
                    { 3, "LOADED" },
                    { 4, "DELIVERING" },
                    { 5, "DELIVERED" },
                    { 6, "RETURNING" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drones_droneModelId",
                table: "Drones",
                column: "droneModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Drones_droneStateId",
                table: "Drones",
                column: "droneStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DroneId",
                table: "Shipments",
                column: "DroneId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentsDetails_MedicationId",
                table: "ShipmentsDetails",
                column: "MedicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipmentsDetails");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Medication");

            migrationBuilder.DropTable(
                name: "Drones");

            migrationBuilder.DropTable(
                name: "DroneModel");

            migrationBuilder.DropTable(
                name: "DroneState");
        }
    }
}
