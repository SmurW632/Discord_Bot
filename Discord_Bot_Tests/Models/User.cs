using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Tests.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ulong GuildId { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime TimeLimit { get; set; }
        public DateTime ConnectUserToChannel { get; set; }
        public double VoiceTime { get; set; }
        public double TotalHoursForSession { get; set; }
        public int DaysOnTheGuild { get; set; }
        public int CountMessage { get; set; }
        public double XP { get; set; }
        public int Level { get; set; }
    }
}
