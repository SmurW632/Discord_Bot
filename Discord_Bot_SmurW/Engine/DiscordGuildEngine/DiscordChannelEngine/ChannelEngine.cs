using Discord_Bot_SmurW.ApplicationContext;
using Discord_Bot_SmurW.Engine.Models;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordChannelEngine
{
    public class ChannelEngine : BaseEngine
    {
        public ChannelEngine(DiscordGuild guild) : base(guild) { }

        public bool CheckChannelExistToDb(ChannelTypeEnum type)
        {
            var guild = _db.Guilds.Include(g => g.Channels).FirstOrDefault(g => g.GuildId == _guild.Id);
            if(guild == null) return false;

            return guild.Channels!.Any(ch => ch.ChannelType == type.ToString());
        }

        public List<ChannelModel> GetAllChannels()
        {
            var guild = _db.Guilds.Include(g => g.Channels).FirstOrDefault(g => g.GuildId == _guild.Id);
            if (guild == null) return null!;

            var channels = guild!.Channels;
            if (channels == null) return null!;

            return channels;
        }

        public bool SavingChannelsToDb(List<DiscordChannel> channels)
        {
            if(channels == null) return false;

            foreach (var channel in channels)
            {
                var newChannel = new ChannelModel()
                {
                    ChannelId = channel.Id,
                    ChannelName = channel.Name,
                    GuildModelId = _guild.Id,
                    ChannelType = ChannelTypeEnum.None.ToString()
                };

                _db.Channels.Add(newChannel);
            }
            _db.SaveChanges();

            return true;
        }

        public ChannelModel GetChannelFromDb(ChannelTypeEnum type)
        {
            var channels = _db.Guilds?.Include(g => g.Channels)?.FirstOrDefault(g => g.GuildId == _guild.Id)?.Channels;
            var channel = channels?.FirstOrDefault(ch => ch.ChannelType == type.ToString());

            if (channel == null) return null!;

            return channel;
        }

        public bool CreateChannel(DiscordChannel channel)
        {
            var guild = _db.Guilds.Include(g => g.Channels).FirstOrDefault(g => g.GuildId == _guild.Id);
            if (guild == null) return false;

            var existChannel = guild.Channels!.Any(c => c.ChannelId == channel.Id);
            if (existChannel == true) return false;

            var newChannel = new ChannelModel()
            {
                ChannelName = channel.Name,
                ChannelId = channel.Id,
                ChannelType = ChannelTypeEnum.None.ToString(),
                GuildModelId = _guild.Id,
            };

            _db.Channels.Add(newChannel);
            _db.SaveChanges();

            return true;
        }

        public bool DeleteChannel(DiscordChannel channel)
        {
            var guild = _db.Guilds.Include(g => g.Channels).FirstOrDefault(g => g.GuildId == _guild.Id);
            if (guild == null) return false;

            var delChannel = guild.Channels?.FirstOrDefault(c => c.ChannelId == channel.Id);
            if (delChannel == null) return false;

            _db.Channels.Remove(delChannel);
            _db.SaveChanges();

            return true;
        }

        private bool EqualsChannel(DiscordChannel before, DiscordChannel after)
        {
            if (before == null || after == null)
                return false;

            if (object.ReferenceEquals(before, after))
                return true;

            if (after.GetType() != before.GetType() || before.GetType() != after.GetType())
                return false;

            if (string.Compare(before.Name, after.Name, StringComparison.CurrentCulture) == 0
                && before.PerUserRateLimit == after.PerUserRateLimit 
                && before.PermissionOverwrites == after.PermissionOverwrites
                && before.Position == after.Position)
                return true;
            else
                return false;
        }

        public bool UpdateChannel(DiscordChannel channelAfter, DiscordChannel channelBefore)
        {
            if(EqualsChannel(channelBefore, channelAfter)) return false;

            var guild = _db.Guilds.Include(g => g.Channels).FirstOrDefault(g => g.GuildId == _guild.Id);
            if (guild == null) return false;

            var upChannel = guild.Channels?.FirstOrDefault(c => c.ChannelId == channelBefore.Id);
            if (upChannel == null) return false;

            upChannel.ChannelName = channelAfter.Name;

            _db.SaveChanges();

            return true;
        }

    }
}
