﻿// <auto-generated />
using System;
using InvoiceJetAPI.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InvoiceJetAPI.Migrations
{
    [DbContext(typeof(FacturilaDbContext))]
    partial class FacturilaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<string>("Iban")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("UserFirmId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserFirmId");

                    b.ToTable("BankAccount");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BankAccountId")
                        .HasColumnType("int");

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("DocumentNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DocumentStatusId")
                        .HasColumnType("int");

                    b.Property<int?>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("UserFirmId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.HasIndex("ClientId");

                    b.HasIndex("DocumentStatusId");

                    b.HasIndex("DocumentTypeId");

                    b.HasIndex("UserFirmId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.DocumentProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("DocumentId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("ProductId");

                    b.ToTable("DocumentProduct");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.DocumentSeries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrentNumber")
                        .HasColumnType("int");

                    b.Property<int?>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<int>("FirstNumber")
                        .HasColumnType("int");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("SeriesName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserFirmId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DocumentTypeId");

                    b.HasIndex("UserFirmId");

                    b.ToTable("DocumentSeries");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.DocumentStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DocumentStatus");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.DocumentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DocumentType");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.Firm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CUI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegCom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Firm");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ContainsTVA")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TVAValue")
                        .HasColumnType("int");

                    b.Property<string>("UnitOfMeasurement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserFirmId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserFirmId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ActiveUserFirmId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ActiveUserFirmId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.UserFirm", b =>
                {
                    b.Property<int>("UserFirmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserFirmId"));

                    b.Property<int>("FirmId")
                        .HasColumnType("int");

                    b.Property<bool>("IsClient")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserFirmId");

                    b.HasIndex("FirmId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFirm");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.BankAccount", b =>
                {
                    b.HasOne("InvoiceJetAPI.Models.Entity.UserFirm", "UserFirm")
                        .WithMany("BankAccounts")
                        .HasForeignKey("UserFirmId");

                    b.Navigation("UserFirm");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.Document", b =>
                {
                    b.HasOne("InvoiceJetAPI.Models.Entity.BankAccount", "BankAccount")
                        .WithMany()
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceJetAPI.Models.Entity.Firm", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("InvoiceJetAPI.Models.Entity.DocumentStatus", "DocumentStatus")
                        .WithMany()
                        .HasForeignKey("DocumentStatusId");

                    b.HasOne("InvoiceJetAPI.Models.Entity.DocumentType", "DocumentType")
                        .WithMany()
                        .HasForeignKey("DocumentTypeId");

                    b.HasOne("InvoiceJetAPI.Models.Entity.UserFirm", "UserFirm")
                        .WithMany("Documents")
                        .HasForeignKey("UserFirmId");

                    b.Navigation("BankAccount");

                    b.Navigation("Client");

                    b.Navigation("DocumentStatus");

                    b.Navigation("DocumentType");

                    b.Navigation("UserFirm");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.DocumentProduct", b =>
                {
                    b.HasOne("InvoiceJetAPI.Models.Entity.Document", "Document")
                        .WithMany("DocumentProducts")
                        .HasForeignKey("DocumentId");

                    b.HasOne("InvoiceJetAPI.Models.Entity.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.Navigation("Document");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.DocumentSeries", b =>
                {
                    b.HasOne("InvoiceJetAPI.Models.Entity.DocumentType", "DocumentType")
                        .WithMany()
                        .HasForeignKey("DocumentTypeId");

                    b.HasOne("InvoiceJetAPI.Models.Entity.UserFirm", "UserFirm")
                        .WithMany("DocumentSeries")
                        .HasForeignKey("UserFirmId");

                    b.Navigation("DocumentType");

                    b.Navigation("UserFirm");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.Product", b =>
                {
                    b.HasOne("InvoiceJetAPI.Models.Entity.UserFirm", "UserFirm")
                        .WithMany("Products")
                        .HasForeignKey("UserFirmId");

                    b.Navigation("UserFirm");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.User", b =>
                {
                    b.HasOne("InvoiceJetAPI.Models.Entity.UserFirm", "ActiveUserFirm")
                        .WithMany()
                        .HasForeignKey("ActiveUserFirmId");

                    b.Navigation("ActiveUserFirm");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.UserFirm", b =>
                {
                    b.HasOne("InvoiceJetAPI.Models.Entity.Firm", "Firm")
                        .WithMany("UserFirms")
                        .HasForeignKey("FirmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceJetAPI.Models.Entity.User", "User")
                        .WithMany("UserFirms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Firm");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.Document", b =>
                {
                    b.Navigation("DocumentProducts");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.Firm", b =>
                {
                    b.Navigation("UserFirms");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.User", b =>
                {
                    b.Navigation("UserFirms");
                });

            modelBuilder.Entity("InvoiceJetAPI.Models.Entity.UserFirm", b =>
                {
                    b.Navigation("BankAccounts");

                    b.Navigation("DocumentSeries");

                    b.Navigation("Documents");

                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}