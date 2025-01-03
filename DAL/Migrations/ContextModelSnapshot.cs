﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Models.BookingData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IncomePaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OutcomeDocumentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("IncomePaymentId");

                    b.HasIndex("OutcomeDocumentId");

                    b.ToTable("BookingData");
                });

            modelBuilder.Entity("Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Emails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InnKpp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Phones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Companies");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Models.Contract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BookingDataId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CarrierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DriverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LoadingPointId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("Number")
                        .HasColumnType("smallint");

                    b.Property<short>("PayPriority")
                        .HasColumnType("smallint");

                    b.Property<float>("Payment")
                        .HasColumnType("real");

                    b.Property<short>("PaymentCondition")
                        .HasColumnType("smallint");

                    b.Property<float>("Prepayment")
                        .HasColumnType("real");

                    b.Property<short>("Status")
                        .HasColumnType("smallint");

                    b.Property<Guid>("TrailerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TruckId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Volume")
                        .HasColumnType("real");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("BookingDataId");

                    b.HasIndex("CarrierId");

                    b.HasIndex("DriverId");

                    b.HasIndex("LoadingPointId");

                    b.HasIndex("TrailerId");

                    b.HasIndex("TruckId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("Models.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BookingDataId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BookingDataId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RecievingDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("Summ")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("BookingDataId");

                    b.HasIndex("BookingDataId1");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("Models.Driver", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CarrierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PaddportId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Phones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TrailerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TruckId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CarrierId");

                    b.HasIndex("PaddportId");

                    b.HasIndex("TrailerId");

                    b.HasIndex("TruckId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("Models.Sub.Passport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfIssue")
                        .HasColumnType("datetime2");

                    b.Property<string>("Issuer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Passport");
                });

            modelBuilder.Entity("Models.Sub.RoutePoint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ContractId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Phones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("Side")
                        .HasColumnType("smallint");

                    b.Property<short>("Type")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("RoutePoints");
                });

            modelBuilder.Entity("Models.Trailer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CarrierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CarrierId");

                    b.HasIndex("Model", "Number")
                        .IsUnique();

                    b.ToTable("Trailer");
                });

            modelBuilder.Entity("Models.Truck", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CarrierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CarrierId");

                    b.HasIndex("Model", "Number")
                        .IsUnique();

                    b.ToTable("Truck");
                });

            modelBuilder.Entity("Models.Carrier", b =>
                {
                    b.HasBaseType("Models.Company");

                    b.Property<short>("Vat")
                        .HasColumnType("smallint");

                    b.ToTable("Carrier");
                });

            modelBuilder.Entity("Models.BookingData", b =>
                {
                    b.HasOne("Models.Document", "IncomePayment")
                        .WithMany()
                        .HasForeignKey("IncomePaymentId");

                    b.HasOne("Models.Document", "OutcomeDocument")
                        .WithMany()
                        .HasForeignKey("OutcomeDocumentId");

                    b.Navigation("IncomePayment");

                    b.Navigation("OutcomeDocument");
                });

            modelBuilder.Entity("Models.Contract", b =>
                {
                    b.HasOne("Models.BookingData", "BookingData")
                        .WithMany()
                        .HasForeignKey("BookingDataId");

                    b.HasOne("Models.Carrier", "Carrier")
                        .WithMany("Contracts")
                        .HasForeignKey("CarrierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Sub.RoutePoint", "LoadingPoint")
                        .WithMany()
                        .HasForeignKey("LoadingPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Trailer", "Trailer")
                        .WithMany()
                        .HasForeignKey("TrailerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Truck", "Truck")
                        .WithMany()
                        .HasForeignKey("TruckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookingData");

                    b.Navigation("Carrier");

                    b.Navigation("Driver");

                    b.Navigation("LoadingPoint");

                    b.Navigation("Trailer");

                    b.Navigation("Truck");
                });

            modelBuilder.Entity("Models.Document", b =>
                {
                    b.HasOne("Models.BookingData", null)
                        .WithMany("IncomeDocuments")
                        .HasForeignKey("BookingDataId");

                    b.HasOne("Models.BookingData", null)
                        .WithMany("OutcomePayments")
                        .HasForeignKey("BookingDataId1");
                });

            modelBuilder.Entity("Models.Driver", b =>
                {
                    b.HasOne("Models.Carrier", "Carrier")
                        .WithMany("Drivers")
                        .HasForeignKey("CarrierId");

                    b.HasOne("Models.Sub.Passport", "Passport")
                        .WithMany()
                        .HasForeignKey("PaddportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Trailer", "Trailer")
                        .WithMany()
                        .HasForeignKey("TrailerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Truck", "Truck")
                        .WithMany()
                        .HasForeignKey("TruckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Carrier");

                    b.Navigation("Passport");

                    b.Navigation("Trailer");

                    b.Navigation("Truck");
                });

            modelBuilder.Entity("Models.Sub.RoutePoint", b =>
                {
                    b.HasOne("Models.Contract", null)
                        .WithMany("UnloadingPoints")
                        .HasForeignKey("ContractId");
                });

            modelBuilder.Entity("Models.Trailer", b =>
                {
                    b.HasOne("Models.Carrier", "Carrier")
                        .WithMany("Trailers")
                        .HasForeignKey("CarrierId");

                    b.Navigation("Carrier");
                });

            modelBuilder.Entity("Models.Truck", b =>
                {
                    b.HasOne("Models.Carrier", "Carrier")
                        .WithMany("Trucks")
                        .HasForeignKey("CarrierId");

                    b.Navigation("Carrier");
                });

            modelBuilder.Entity("Models.Carrier", b =>
                {
                    b.HasOne("Models.Company", null)
                        .WithOne()
                        .HasForeignKey("Models.Carrier", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.BookingData", b =>
                {
                    b.Navigation("IncomeDocuments");

                    b.Navigation("OutcomePayments");
                });

            modelBuilder.Entity("Models.Contract", b =>
                {
                    b.Navigation("UnloadingPoints");
                });

            modelBuilder.Entity("Models.Carrier", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Drivers");

                    b.Navigation("Trailers");

                    b.Navigation("Trucks");
                });
#pragma warning restore 612, 618
        }
    }
}
