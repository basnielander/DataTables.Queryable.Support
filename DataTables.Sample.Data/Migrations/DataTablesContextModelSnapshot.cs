﻿// <auto-generated />
using System;
using DataTables.Sample.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataTables.Sample.Data.Migrations
{
    [DbContext(typeof(DataTablesContext))]
    partial class DataTablesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Datatables.Samples.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullName");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Datatables.Samples.Models.ToDo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CreatedById");

                    b.Property<DateTime?>("Deadline");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int?>("EstimatedDuration");

                    b.Property<decimal?>("ExpectedCosts");

                    b.Property<bool>("Finished");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("ToDos");
                });

            modelBuilder.Entity("Datatables.Samples.Models.ToDo", b =>
                {
                    b.HasOne("Datatables.Samples.Models.Person", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
