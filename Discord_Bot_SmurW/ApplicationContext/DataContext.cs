using Discord_Bot_SmurW.Engine.LevelSystem;
using Discord_Bot_SmurW.Engine.Models;
using Microsoft.EntityFrameworkCore;

namespace Discord_Bot_SmurW.ApplicationContext
{
    public class DataContext : DbContext
    {
        public DbSet<ConfigModel> Configs { get; private set; } = null!;
        public DbSet<GuildModel> Guilds { get; set; } = null!;
        public DbSet<ChannelModel> Channels { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;
        public DbSet<RoleModel> Roles { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
