﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Slack.Data;
using Slack.Models;
using System;

namespace Slack.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Slack.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Slack.Models.Channel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("General");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerID");

                    b.Property<int>("Type");

                    b.Property<int?>("WorkspaceID");

                    b.HasKey("ID");

                    b.HasIndex("WorkspaceID");

                    b.ToTable("Channel");
                });

            modelBuilder.Entity("Slack.Models.ChannelMembership", b =>
                {
                    b.Property<int>("ChannelMembershipID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserID");

                    b.Property<int>("ChannelID");

                    b.Property<DateTime?>("JoinDate");

                    b.HasKey("ChannelMembershipID");

                    b.HasIndex("ApplicationUserID");

                    b.HasIndex("ChannelID");

                    b.ToTable("ChannelMembership");
                });

            modelBuilder.Entity("Slack.Models.File", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType");

                    b.Property<string>("FilePath");

                    b.Property<string>("OriginalName");

                    b.HasKey("FileId");

                    b.ToTable("File");
                });

            modelBuilder.Entity("Slack.Models.Message", b =>
                {
                    b.Property<int>("MessageID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserID");

                    b.Property<int>("ChannelID");

                    b.Property<int?>("FileId");

                    b.Property<string>("MessageText");

                    b.Property<DateTime>("SendDate");

                    b.HasKey("MessageID");

                    b.HasIndex("ApplicationUserID");

                    b.HasIndex("ChannelID");

                    b.HasIndex("FileId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Slack.Models.Workspace", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("OwnerID");

                    b.HasKey("ID");

                    b.ToTable("Workspace");
                });

            modelBuilder.Entity("Slack.Models.WorkspaceInvitation", b =>
                {
                    b.Property<int>("WorkspaceInvitationID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CloseDate");

                    b.Property<long?>("ExpirationTime");

                    b.Property<Guid>("InvitationGUID");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("UserEmailAddress");

                    b.Property<string>("WorkspaceName");

                    b.HasKey("WorkspaceInvitationID");

                    b.ToTable("WorkspaceInvitation");
                });

            modelBuilder.Entity("Slack.Models.WorkspaceMembership", b =>
                {
                    b.Property<int>("WorkspaceMembershipID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserID");

                    b.Property<DateTime?>("JoinDate");

                    b.Property<int>("WorkspaceID");

                    b.HasKey("WorkspaceMembershipID");

                    b.HasIndex("ApplicationUserID");

                    b.HasIndex("WorkspaceID");

                    b.ToTable("WorkspaceMembership");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Slack.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Slack.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Slack.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Slack.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Slack.Models.Channel", b =>
                {
                    b.HasOne("Slack.Models.Workspace", "Workspace")
                        .WithMany("Channels")
                        .HasForeignKey("WorkspaceID");
                });

            modelBuilder.Entity("Slack.Models.ChannelMembership", b =>
                {
                    b.HasOne("Slack.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("ChannelMemberships")
                        .HasForeignKey("ApplicationUserID");

                    b.HasOne("Slack.Models.Channel", "Channel")
                        .WithMany("ChannelMemberships")
                        .HasForeignKey("ChannelID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Slack.Models.Message", b =>
                {
                    b.HasOne("Slack.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserID");

                    b.HasOne("Slack.Models.Channel", "Channel")
                        .WithMany("Messages")
                        .HasForeignKey("ChannelID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Slack.Models.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId");
                });

            modelBuilder.Entity("Slack.Models.WorkspaceMembership", b =>
                {
                    b.HasOne("Slack.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("WorkspaceMemberships")
                        .HasForeignKey("ApplicationUserID");

                    b.HasOne("Slack.Models.Workspace", "Workspace")
                        .WithMany("WorkspaceMemberships")
                        .HasForeignKey("WorkspaceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
