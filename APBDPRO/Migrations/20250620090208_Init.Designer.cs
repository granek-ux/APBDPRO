﻿// <auto-generated />
using System;
using APBDPRO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APBDPRO.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250620090208_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("APBDPRO.Models.Agreement", b =>
                {
                    b.Property<int>("OfferId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSigned")
                        .HasColumnType("bit");

                    b.Property<string>("SoftwareVersion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("YearsOfAssistance")
                        .HasColumnType("int");

                    b.HasKey("OfferId");

                    b.ToTable("Agreements");
                });

            modelBuilder.Entity("APBDPRO.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adres")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PhoneNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("APBDPRO.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("KRS")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("APBDPRO.Models.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("APBDPRO.Models.DiscountSoftware", b =>
                {
                    b.Property<int>("SoftwareId")
                        .HasColumnType("int");

                    b.Property<int>("DiscountsId")
                        .HasColumnType("int");

                    b.HasKey("SoftwareId", "DiscountsId");

                    b.HasIndex("DiscountsId");

                    b.ToTable("Discount_Software");
                });

            modelBuilder.Entity("APBDPRO.Models.Offer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("SoftwareId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("SoftwareId");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("APBDPRO.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("OfferId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Refunded")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("OfferId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("APBDPRO.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PESEL")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("APBDPRO.Models.Software", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ActualVersion")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("SoftwareCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SoftwareCategoryId");

                    b.ToTable("Software");
                });

            modelBuilder.Entity("APBDPRO.Models.SoftwareCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Software_Category");
                });

            modelBuilder.Entity("APBDPRO.Models.StatusSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Status_Subscription");
                });

            modelBuilder.Entity("APBDPRO.Models.Subscription", b =>
                {
                    b.Property<int>("OfferId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("PriceForFirstInstallment")
                        .HasColumnType("float");

                    b.Property<int>("RenewalPeriodDurationInMonths")
                        .HasColumnType("int");

                    b.Property<int>("StatusSubscriptionId")
                        .HasColumnType("int");

                    b.HasKey("OfferId");

                    b.HasIndex("StatusSubscriptionId");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("APBDPRO.Models.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUser"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("RefreshTokenExp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("IdUser");

                    b.HasIndex("UserRoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("APBDPRO.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Users_Roles");
                });

            modelBuilder.Entity("APBDPRO.Models.Agreement", b =>
                {
                    b.HasOne("APBDPRO.Models.Offer", "Offer")
                        .WithOne("Agreement")
                        .HasForeignKey("APBDPRO.Models.Agreement", "OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("APBDPRO.Models.Company", b =>
                {
                    b.HasOne("APBDPRO.Models.Client", "Client")
                        .WithOne("Company")
                        .HasForeignKey("APBDPRO.Models.Company", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("APBDPRO.Models.DiscountSoftware", b =>
                {
                    b.HasOne("APBDPRO.Models.Discount", "Discount")
                        .WithMany("DiscountSoftwares")
                        .HasForeignKey("DiscountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APBDPRO.Models.Software", "Software")
                        .WithMany()
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("Software");
                });

            modelBuilder.Entity("APBDPRO.Models.Offer", b =>
                {
                    b.HasOne("APBDPRO.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APBDPRO.Models.Software", "Software")
                        .WithMany()
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Software");
                });

            modelBuilder.Entity("APBDPRO.Models.Payment", b =>
                {
                    b.HasOne("APBDPRO.Models.Offer", "Offer")
                        .WithMany("Payments")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("APBDPRO.Models.Person", b =>
                {
                    b.HasOne("APBDPRO.Models.Client", "Client")
                        .WithOne("Person")
                        .HasForeignKey("APBDPRO.Models.Person", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("APBDPRO.Models.Software", b =>
                {
                    b.HasOne("APBDPRO.Models.SoftwareCategory", "SoftwareCategory")
                        .WithMany("Softwares")
                        .HasForeignKey("SoftwareCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SoftwareCategory");
                });

            modelBuilder.Entity("APBDPRO.Models.Subscription", b =>
                {
                    b.HasOne("APBDPRO.Models.Offer", "Offer")
                        .WithOne("Subscription")
                        .HasForeignKey("APBDPRO.Models.Subscription", "OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APBDPRO.Models.StatusSubscription", "StatusSubscription")
                        .WithMany("Subscriptions")
                        .HasForeignKey("StatusSubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offer");

                    b.Navigation("StatusSubscription");
                });

            modelBuilder.Entity("APBDPRO.Models.User", b =>
                {
                    b.HasOne("APBDPRO.Models.UserRole", "UserRole")
                        .WithMany("Users")
                        .HasForeignKey("UserRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("APBDPRO.Models.Client", b =>
                {
                    b.Navigation("Company");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("APBDPRO.Models.Discount", b =>
                {
                    b.Navigation("DiscountSoftwares");
                });

            modelBuilder.Entity("APBDPRO.Models.Offer", b =>
                {
                    b.Navigation("Agreement");

                    b.Navigation("Payments");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("APBDPRO.Models.SoftwareCategory", b =>
                {
                    b.Navigation("Softwares");
                });

            modelBuilder.Entity("APBDPRO.Models.StatusSubscription", b =>
                {
                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("APBDPRO.Models.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
