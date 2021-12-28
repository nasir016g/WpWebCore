﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nsr.ActivityLogs.Web.Data;

#nullable disable

namespace Nsr.ActivityLogs.Web.Migrations
{
    [DbContext(typeof(ActivityLogDbContext))]
    [Migration("20211228204950_FixedLogItem")]
    partial class FixedLogItem
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Nsr.RestClient.Models.ActivityLogs.ActivityLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ActivityLogTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("EntityName")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.HasKey("Id");

                    b.HasIndex("ActivityLogTypeId");

                    b.ToTable("ActivityLog", (string)null);
                });

            modelBuilder.Entity("Nsr.RestClient.Models.ActivityLogs.ActivityLogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ActivityLogId")
                        .HasColumnType("int");

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("EntityKey")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<string>("NewValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValue")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ActivityLogId");

                    b.ToTable("ActivityLogItem", (string)null);
                });

            modelBuilder.Entity("Nsr.RestClient.Models.ActivityLogs.ActivityLogType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SystemKeyword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("ActivityLogType", (string)null);
                });

            modelBuilder.Entity("Nsr.RestClient.Models.ActivityLogs.ActivityLog", b =>
                {
                    b.HasOne("Nsr.RestClient.Models.ActivityLogs.ActivityLogType", "ActivityLogType")
                        .WithMany()
                        .HasForeignKey("ActivityLogTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActivityLogType");
                });

            modelBuilder.Entity("Nsr.RestClient.Models.ActivityLogs.ActivityLogItem", b =>
                {
                    b.HasOne("Nsr.RestClient.Models.ActivityLogs.ActivityLog", "ActivityLog")
                        .WithMany("ActivityLogItems")
                        .HasForeignKey("ActivityLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActivityLog");
                });

            modelBuilder.Entity("Nsr.RestClient.Models.ActivityLogs.ActivityLog", b =>
                {
                    b.Navigation("ActivityLogItems");
                });
#pragma warning restore 612, 618
        }
    }
}
