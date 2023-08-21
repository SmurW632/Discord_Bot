using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.Models
{
    [Table("Участники")]
    public class UserModel
    {
        public int Id { get; set; }
        public ulong UserId { get; set; }
        public string? UserName { get; set; }
        public string? ConnectUserToChannel { get; set; }
        public int Level { get; set; }
        public double XP { get; set; }
        public int CountMessage { get; set; }
        public double VoiceTime { get; set; }
        public double TotalMinutesForSession { get; set; }
        public int DaysOnTheGuild { get; set; }
        public string? TimeLimit { get; set; }
        public string? AvatarUrl { get; set; }

        public ulong GuildModelId { get; set; }
        public GuildModel? GuildModel { get; set; }

    }
}
