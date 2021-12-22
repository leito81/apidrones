using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using apidrones.Model;
using System.ComponentModel.DataAnnotations;

namespace apidrones.Data
{
    public class ApidronesContext : DbContext
    {

        public virtual DbSet<DroneClass> Drones { get; set; }
        public virtual DbSet<DroneState> DroneState { get; set; }
        public virtual DbSet<DroneModel> DroneModel { get; set; }
        public virtual DbSet<MedicationClass> Medication { get; set; }
        public virtual DbSet<ShipmentClass> Shipments { get; set; }
        public virtual DbSet<ShipmentDetails> ShipmentsDetails { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }

        public ApidronesContext(DbContextOptions<ApidronesContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<DroneClass>(entity =>
            {
                entity.Property(e => e.Serial_number)
                        .HasColumnType("varchar(100)")
                       .IsRequired()
                       .IsUnicode(false);
                entity.HasKey("Serial_number");
                entity.Property(e => e.Weight_limit).HasColumnType("int").HasMaxLength(500);
                entity.Property(e => e.Battery_capacity).HasColumnType("int");
                entity.Property(e => e.droneStateId).HasConversion<int>().HasDefaultValue(DroneStateId.IDLE);
                entity.Property(e => e.droneModelId).HasConversion<int>();

            });

            modelBuilder.Entity<DroneState>(entity =>
            {
                entity.Property(e => e.DroneStateId).HasConversion<int>();
            });

            modelBuilder.Entity<DroneModel>(entity =>
            {
                entity.Property(e => e.DroneModelId).HasConversion<int>();
            });

            modelBuilder.Entity<DroneState>().HasData(
               new DroneState() { DroneStateId = DroneStateId.IDLE, StateName ="IDLE"},
               new DroneState() { DroneStateId = DroneStateId.LOADING, StateName = "LOADING" },
               new DroneState() { DroneStateId = DroneStateId.LOADED, StateName = "LOADED" },
               new DroneState() { DroneStateId = DroneStateId.DELIVERING, StateName = "DELIVERING" },
               new DroneState() { DroneStateId = DroneStateId.DELIVERED, StateName = "DELIVERED" },
               new DroneState() { DroneStateId = DroneStateId.RETURNING, StateName = "RETURNING" }
            );

            modelBuilder.Entity<DroneModel>().HasData(
                new DroneModel() { DroneModelId = DroneModelId.Lightweight, ModelName= "Lightweight" },
                new DroneModel() { DroneModelId = DroneModelId.Middleweight, ModelName = "Middleweight" },
                new DroneModel() { DroneModelId = DroneModelId.Cruiserweight, ModelName = "Cruiserweight" },
                new DroneModel() { DroneModelId = DroneModelId.Heavyweight, ModelName = "Heavyweight" }
            );

            modelBuilder.Entity<MedicationClass>(entity =>
            {
                entity.Property(e => e.Code)
                      .HasColumnType("varchar(60)")
                      .IsRequired()
                      .IsUnicode(false);
                entity.HasKey("Code");

                entity.Property(e=>e.Name)
                      .HasColumnType("varchar(60)")
                      .IsRequired()
                      .IsUnicode(false);
                entity.Property(e=>e.Weight).HasColumnType("int");
                entity.Property(e => e.Image).HasColumnType("text");


                //relations
                entity.HasMany(m => m.Shipments)
                    .WithOne(re => re.Medication)
                    .HasForeignKey(FK => FK.MedicationId)
                    .HasConstraintName("FK_Shipments_Medication");
            });

            modelBuilder.Entity<ShipmentClass>(entity =>
            {
                entity.Property(e => e.IdShipment).HasColumnType("int");
                entity.HasKey("IdShipment");

                entity.HasOne(re => re.Drone)
                      .WithMany(m => m.Shipments)
                      .HasForeignKey(FK => FK.DroneId)
                      .HasConstraintName("FK_Drones_Shipments");
                
            });

            modelBuilder.Entity<ShipmentDetails>(entity =>
            {
                entity.Property(e => e.Quantity).HasColumnType("int");

                entity.HasOne(re => re.Shipment)
                    .WithMany(m => m.Details)
                    .HasForeignKey(FK => FK.IdShipment)
                    .HasConstraintName("FK_Shipment_ShipmentDetails");

                entity.HasOne(re => re.Medication)
                    .WithMany(m => m.Shipments)
                    .HasForeignKey(FK => FK.MedicationId)
                    .HasConstraintName("FK_Medication_ShipmentDetails");

                entity.HasKey("IdShipment", "MedicationId");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.Property(e => e.Date_registered).HasColumnType("datetime");
                entity.HasKey("Date_registered");

                entity.Property(e => e.LogRegister).IsRequired().IsUnicode(false);
            });
           
        }
    }
}
