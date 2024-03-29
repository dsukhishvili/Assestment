﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserManagement.Service.DAL;

namespace UserManagement.Service.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UserManagement.Service.Models.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("City");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "TBilisi"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Batumi"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Kutaisi"
                        });
                });

            modelBuilder.Entity("UserManagement.Service.Models.Phone", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Number")
                        .HasMaxLength(50);

                    b.Property<int>("Type");

                    b.Property<int?>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Phone");
                });

            modelBuilder.Entity("UserManagement.Service.Models.RelatedUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RelatedUserID");

                    b.Property<int>("RelationType");

                    b.Property<int?>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("RelatedUser");
                });

            modelBuilder.Entity("UserManagement.Service.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityID");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Gender");

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasMaxLength(11);

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PictureRelativePath");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("UserManagement.Service.Models.Phone", b =>
                {
                    b.HasOne("UserManagement.Service.Models.User")
                        .WithMany("Phones")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("UserManagement.Service.Models.RelatedUser", b =>
                {
                    b.HasOne("UserManagement.Service.Models.User", "User")
                        .WithMany("ContactPersons")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("UserManagement.Service.Models.User", b =>
                {
                    b.HasOne("UserManagement.Service.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
