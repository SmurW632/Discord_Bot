using Discord_Bot_SmurW.ApplicationContext;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordChannelEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordRoleEngine;
using Discord_Bot_SmurW.Engine.LevelSystem;
using Discord_Bot_SmurW.Engine.Models;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.DiscordGuildEngine
{
    public class GuildEngine : BaseEngine
    {
        public GuildEngine(DiscordGuild guild) : base(guild) { }
        public bool CheckGuildExistToDb()
        {
            var guild = _db.Guilds.FirstOrDefault(g => g.GuildId == _guild.Id);

            if (guild != null) return true;

            return false;
        }

        public bool SavingGuildsToDb(List<DiscordMember> u, List<DiscordChannel> ch, List<DiscordRole> r)
        {
            if (u == null || ch == null || r == null) return false;
            if(_db.Guilds.Any(g => g.GuildId == _guild.Id)) return false;

            var engineUsers = new RaitingUsersEngine(_guild);
            var engineChannels = new ChannelEngine(_guild);
            var engineRoles = new RoleEngine(_guild);


            var guildModel = new GuildModel()
            {
                GuildId = _guild.Id,
                GuildName = _guild.Name,
                Users = null,
                Channels = null,
                Roles = null,
            };

            _db.Guilds.Add(guildModel);
            _db.SaveChanges();

            var savingUsers = engineUsers.SavingUsersToDb(u);
            var savingChannels = engineChannels.SavingChannelsToDb(ch);
            var savingRoles = engineRoles.SavingRolesToDb(r);

            if(savingUsers == false || savingChannels == false || savingRoles == false) return false;

            return true;
        }

        public GuildModel GetGuildFromDb()
        {
            var guild = _db.Guilds.First(g => g.GuildId == _guild.Id);
            if (guild == null) return null!;

            return new GuildModel
            {
                GuildName = guild.GuildName,
                Users = guild.Users,
                Channels = guild.Channels,
            };
        }

         
    }
}
