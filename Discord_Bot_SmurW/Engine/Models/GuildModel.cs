using Discord_Bot_SmurW.Engine.LevelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.Models
{
    [Table("Гильдия")]
    public class GuildModel
    {
        [Key]
        public ulong GuildId { get; set; }
        public string? GuildName { get; set; }
        public List<ChannelModel>? Channels { get; set; } = new();
        public List<UserModel>? Users { get; set; } = new();
        public List<RoleModel>? Roles { get; set; } = new();
    }
}
