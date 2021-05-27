﻿// <auto-generated />
using System;
using LudoAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LudoAPI.Migrations
{
    [DbContext(typeof(LudoContext))]
    partial class LudoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ludo.API.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("Ludo.API.Models.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TokenId");

                    b.ToTable("Route");
                });

            modelBuilder.Entity("Ludo.API.Models.Square", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.ToTable("Square");
                });

            modelBuilder.Entity("Ludo.API.Models.SquareOccupant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("OccupantId")
                        .HasColumnType("int");

                    b.Property<int>("SquareId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OccupantId");

                    b.HasIndex("SquareId");

                    b.ToTable("SquareOccupant");
                });

            modelBuilder.Entity("LudoAPI.Models.Board", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BoardName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PlayerIDLastMadeMove")
                        .HasColumnType("int");

                    b.Property<string>("PlayerTurnName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Board");
                });

            modelBuilder.Entity("LudoAPI.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Color")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("SquareId")
                        .HasColumnType("int");

                    b.Property<int>("Steps")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Token");
                });

            modelBuilder.Entity("Ludo.API.Models.Player", b =>
                {
                    b.HasOne("LudoAPI.Models.Board", null)
                        .WithMany("Players")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ludo.API.Models.Route", b =>
                {
                    b.HasOne("LudoAPI.Models.Token", null)
                        .WithMany("Route")
                        .HasForeignKey("TokenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ludo.API.Models.Square", b =>
                {
                    b.HasOne("LudoAPI.Models.Board", null)
                        .WithMany("Squares")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ludo.API.Models.SquareOccupant", b =>
                {
                    b.HasOne("LudoAPI.Models.Token", "Occupant")
                        .WithMany()
                        .HasForeignKey("OccupantId");

                    b.HasOne("Ludo.API.Models.Square", null)
                        .WithMany("Occupants")
                        .HasForeignKey("SquareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Occupant");
                });

            modelBuilder.Entity("LudoAPI.Models.Token", b =>
                {
                    b.HasOne("Ludo.API.Models.Player", null)
                        .WithMany("Tokens")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ludo.API.Models.Player", b =>
                {
                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("Ludo.API.Models.Square", b =>
                {
                    b.Navigation("Occupants");
                });

            modelBuilder.Entity("LudoAPI.Models.Board", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("Squares");
                });

            modelBuilder.Entity("LudoAPI.Models.Token", b =>
                {
                    b.Navigation("Route");
                });
#pragma warning restore 612, 618
        }
    }
}