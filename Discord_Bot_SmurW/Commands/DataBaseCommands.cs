using Discord_Bot_SmurW.Engine.DataBaseEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine;
using Discord_Bot_SmurW.Engine.LevelSystem;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Commands
{
    public class DataBaseCommands : BaseCommandModule
    {
        #region Создание или удаление БД

        [Hidden]
        [RequireOwner]
        [Command("db")]
        [Description("deleted or created db")]
        public async Task DBDeleteOrCreate(CommandContext ctx, string db = null)
        {
            var member = (DiscordMember)ctx.User;
            var guild = ctx.Guild;

            var ratingUsersEngine = new RaitingUsersEngine(member, guild);
            var dataBaseEngine = new DataBaseEngine();
            var isDb = dataBaseEngine.CreateOrDeleteDataBase(db);
            var embedMessage = new DiscordEmbedBuilder();
            embedMessage.Color = DiscordColor.Green;

            if (db == "delet" && isDb == true)
            {
                embedMessage.Title = "Вы успешно удалили базу данных";
            }
            else if (db == "create" && isDb == true)
            {
                embedMessage.Title = "Вы успешно создали базу данных";
            }
            else
            {
                embedMessage.Title = "Неудалось удалить или создать базу данных";
                embedMessage.Color = DiscordColor.Red;
            }

            await ctx.Channel.SendMessageAsync(embedMessage);
        }

        #endregion

        #region Удаление данных
        [Hidden]
        [RequireOwner]
        [Command("delete")] 
        [Description("Удаление данных с базы данных")]
        public async Task DeleteDataFromDbAsync(CommandContext ctx)
        {
            var engine = new DataBaseEngine(ctx.Guild);
            var isDeleted = engine.DeleteDataFromDb();

            if (isDeleted == false)
            {
                await ctx.Channel.SendMessageAsync("Данные не удалось удалить!");
                return;
            }

            await ctx.Channel.SendMessageAsync($"Данные успшно удалены с базы данных по гильдии {ctx.Guild.Name}");
        }
        #endregion

        #region Инициализация базы данных
        [Hidden]
        [RequireOwner]
        [Command("Init")] 
        [Description("Инициализация базы данных")]
        public async Task DataDBAsync(CommandContext ctx)
        {
            /* Сохранение тестовых данных
            var savingEngine = new SavingTestData(ctx.Guild);

            var isSaving = savingEngine.SavingAllDataToDb();
            if (isSaving == true) await ctx.Channel.SendMessageAsync("Данные успешно сохранены!");
            else await ctx.Channel.SendMessageAsync("Данные сохранить не удалось!");
            */

            var guild = ctx.Guild;
            var engineGuild = new GuildEngine(guild);

            var listUsers = guild.Members.Values.ToList();
            var listChannels = guild.Channels.Values.ToList();
            var listRoles = guild.Roles.Values.ToList();

            var isSavingData = engineGuild.SavingGuildsToDb(listUsers, listChannels, listRoles);

            if (isSavingData == false)
            {
                await ctx.Channel.SendMessageAsync("Данные не удалось сохранить или они уже имеются в БД!");
                return;
            }

            await ctx.Channel.SendMessageAsync("Данные успшно сохраненны!");
        }
        #endregion
    }
}
