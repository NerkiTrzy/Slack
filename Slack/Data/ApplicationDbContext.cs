﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Slack.Models;

namespace Slack.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Slack.Models.Workspace> Workspace { get; set; }
        public DbSet<Slack.Models.Channel> Channel { get; set; }
        public DbSet<Slack.Models.Message> Message { get; set; }
        public DbSet<Slack.Models.WorkspaceMembership> WorkspaceMembership { get; set; }
        public DbSet<Slack.Models.ChannelMembership> ChannelMembership { get; set; }
        public DbSet<Slack.Models.WorkspaceInvitation> WorkspaceInvitation { get; set; }


    }
}
