using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snake.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Context
{
    public class EFContext : IdentityDbContext<UserModel, IdentityRole<int>, int>
    {
        public EFContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ChannelModel> Channels { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserChannelModel> UserChannels { get; set; }


    }
}
