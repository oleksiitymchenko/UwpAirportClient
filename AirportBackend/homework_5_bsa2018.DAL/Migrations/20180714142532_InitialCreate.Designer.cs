﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using homework_5_bsa2018.DAL;

namespace homework_5_bsa2018.PL.Migrations
{
    [DbContext(typeof(AirportContext))]
    [Migration("20180714142532_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Crew", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PilotId");

                    b.HasKey("Id");

                    b.HasIndex("PilotId");

                    b.ToTable("Crews");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Departure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CrewId");

                    b.Property<DateTime>("DepartureTime");

                    b.Property<string>("FlightNumber");

                    b.Property<int?>("PlaneId");

                    b.HasKey("Id");

                    b.HasIndex("CrewId");

                    b.HasIndex("PlaneId");

                    b.ToTable("Departures");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FinishPoint");

                    b.Property<DateTime>("FinishTime");

                    b.Property<string>("Number");

                    b.Property<string>("StartPoint");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Pilot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Experience");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Pilots");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Plane", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<TimeSpan>("LifeTime");

                    b.Property<string>("Name");

                    b.Property<int?>("TypePlaneId");

                    b.HasKey("Id");

                    b.HasIndex("TypePlaneId");

                    b.ToTable("Planes");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.PlaneType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Carrying");

                    b.Property<string>("Model");

                    b.Property<int>("Places");

                    b.HasKey("Id");

                    b.ToTable("PlaneTypes");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Stewardess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CrewId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.HasIndex("CrewId");

                    b.ToTable("Stewardesses");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FlightId");

                    b.Property<string>("FlightNumber");

                    b.Property<double>("Price");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Crew", b =>
                {
                    b.HasOne("homework_5_bsa2018.DAL.Models.Pilot", "Pilot")
                        .WithMany()
                        .HasForeignKey("PilotId");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Departure", b =>
                {
                    b.HasOne("homework_5_bsa2018.DAL.Models.Crew", "Crew")
                        .WithMany()
                        .HasForeignKey("CrewId");

                    b.HasOne("homework_5_bsa2018.DAL.Models.Plane", "Plane")
                        .WithMany()
                        .HasForeignKey("PlaneId");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Plane", b =>
                {
                    b.HasOne("homework_5_bsa2018.DAL.Models.PlaneType", "TypePlane")
                        .WithMany()
                        .HasForeignKey("TypePlaneId");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Stewardess", b =>
                {
                    b.HasOne("homework_5_bsa2018.DAL.Models.Crew")
                        .WithMany("Stewardesses")
                        .HasForeignKey("CrewId");
                });

            modelBuilder.Entity("homework_5_bsa2018.DAL.Models.Ticket", b =>
                {
                    b.HasOne("homework_5_bsa2018.DAL.Models.Flight")
                        .WithMany("Tickets")
                        .HasForeignKey("FlightId");
                });
#pragma warning restore 612, 618
        }
    }
}