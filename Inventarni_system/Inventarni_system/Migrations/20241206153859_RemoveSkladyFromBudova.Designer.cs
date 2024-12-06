﻿// <auto-generated />
using Inventarni_system.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inventarni_system.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241206153859_RemoveSkladyFromBudova")]
    partial class RemoveSkladyFromBudova
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Inventarni_system.Models.Budova", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nazev")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Typ")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Budovy");
                });

            modelBuilder.Entity("Inventarni_system.Models.Predmet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("CenaZaKus")
                        .HasColumnType("TEXT");

                    b.Property<int>("Mnozstvi")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nazev")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SkladId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SkladId");

                    b.ToTable("Predmety");
                });

            modelBuilder.Entity("Inventarni_system.Models.Sklad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BudovaId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nazev")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BudovaId");

                    b.ToTable("Sklady");
                });

            modelBuilder.Entity("Inventarni_system.Models.Predmet", b =>
                {
                    b.HasOne("Inventarni_system.Models.Sklad", "Sklad")
                        .WithMany("Predmety")
                        .HasForeignKey("SkladId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sklad");
                });

            modelBuilder.Entity("Inventarni_system.Models.Sklad", b =>
                {
                    b.HasOne("Inventarni_system.Models.Budova", "Budova")
                        .WithMany()
                        .HasForeignKey("BudovaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Budova");
                });

            modelBuilder.Entity("Inventarni_system.Models.Sklad", b =>
                {
                    b.Navigation("Predmety");
                });
#pragma warning restore 612, 618
        }
    }
}
