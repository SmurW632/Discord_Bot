using Discord_Bot_SmurW.Engine.DiscordGuildEngine;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.DataBaseEngine
{
    public class DataBaseEngine : BaseEngine
    {
        public DataBaseEngine() { }

        public DataBaseEngine(DiscordGuild guild) : base(guild) { }

        public string Token
        {
            get
            {
                var token = _db.Configs.Select(t => t.Token).FirstOrDefault();
                if (string.IsNullOrEmpty(token)) return null!;

                return token;
            }
        }
        public string Prefix
        {
            get
            {
                var token = _db.Configs.Select(c => c.Prefix).FirstOrDefault();
                if (string.IsNullOrEmpty(token)) return null!;

                return token;
            }
        }

        public bool DeleteDataFromDb()
        {
            var guild = _db.Guilds.FirstOrDefault(g => g.GuildId == _guild.Id);
            if(guild == null) return false;

            _db.Remove(guild);
            _db.SaveChanges();

            return true;
        }
    }
}
