using Discord_Bot_SmurW.ApplicationContext;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordChannelEngine;
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
    public class BaseEngine
    {
        protected DbContextOptions<DataContext> _options;
        protected DataContext _db;
        protected DiscordGuild _guild;

        public BaseEngine(DiscordGuild guild)
        {
            _options = WorkDataBase.OptionsBuider;
            _guild = guild;
            _db = new(_options);
        }

        public BaseEngine()
        {
            _options = WorkDataBase.OptionsBuider;
            _db = new(_options);
        }
    }
}
