using Discord_Bot_SmurW.Engine.DiscordGuildEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordChannelEngine;
using Discord_Bot_SmurW.Engine.LevelSystem;
using Discord_Bot_SmurW.Engine.Models;
using Discord_Bot_SmurW.Engine.PersonalityMessages;
using Discord_Bot_SmurW.HelpFormatter;
using Discord_Bot_SmurW.TestData;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Discord_Bot_SmurW.Commands
{
    [RequireGuild]
    [Category("Модерация")]
    public class ModerationPrefixCommands : BaseCommandModule
    {
        #region Удаление сообщений
        [Hidden]
        [Command("удалить")]
        [RequirePermissions(Permissions.Administrator | Permissions.ModerateMembers)]
        [Description("Удаление сообщений с текущего канала")]
        public async Task DelMessagesAsync(CommandContext ctx, ulong count = 100)
        {

            TypeMessage.CountDeleteMessages = (int)count;

            var messages = await ctx.Channel.GetMessagesAsync((int)count);

            var btnYesDeletedMessage = new DiscordButtonComponent(ButtonStyle.Danger, "btnYesDeletedMessage", "Да");
            var btnNoDeletedMessage = new DiscordButtonComponent(ButtonStyle.Primary, "btnClose", "Нет");

            var warningMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle("Вы действительно хотите удалить сообщения в этом канале?")
                .WithDescription($"Сообщений к удалению: {messages.Count}")
                .WithColor(DiscordColor.Orange)
                )
                .AddComponents(btnYesDeletedMessage, btnNoDeletedMessage);

            await ctx.Channel.SendMessageAsync(warningMessage);
        }
        #endregion

        #region Получить роли #устаревшее
        [Hidden]
        [RequireOwner]
        [Command("роли")]
        [Description("Получение роли при нажатии на реакцию")]
        public async Task GetRoleAsync(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            DiscordEmoji[] emojis =
            {
                DiscordEmoji.FromName(ctx.Client, ":csgo:", true),
                DiscordEmoji.FromName(ctx.Client, ":gta5:", true),
                DiscordEmoji.FromName(ctx.Client, ":dbd:", true),
                DiscordEmoji.FromName(ctx.Client, ":horror:", true),
                DiscordEmoji.FromName(ctx.Client, ":sot:", true),
                DiscordEmoji.FromName(ctx.Client, ":apexlegends:", true),
                DiscordEmoji.FromName(ctx.Client, ":tanki:", true),
                DiscordEmoji.FromName(ctx.Client, ":deceit:", true),
                DiscordEmoji.FromName(ctx.Client, ":dota2:", true),

            };

            var d = new DiscordEmoji[]
            {
                DiscordEmoji.FromName(ctx.Client, ":small_orange_diamond:", true),
                DiscordEmoji.FromName(ctx.Client, ":small_blue_diamond:", true)
            };

            string description = $"{emojis[0]} Counter Strike Global Offensive {emojis[0]}\n\n" +
                                 $"{emojis[1]} Grand Theft Auto V {emojis[1]}\n\n" +
                                 $"{emojis[2]} Dead By Daylight {emojis[2]}\n\n" +
                                 $"{emojis[3]} Horror Games  {emojis[3]}\n\n" +
                                 $"{emojis[4]} Sea Of Thieves  {emojis[4]}\n\n" +
                                 $"{emojis[5]} Apex Legends  {emojis[5]}\n\n" +
                                 $"{emojis[6]} Мир танков  {emojis[6]}\n\n" +
                                 $"{emojis[7]} Deceit  {emojis[7]}\n\n" +
                                 $"{emojis[8]} Dota 2 {emojis[8]}";

            var embedMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle(":small_orange_diamond: Игровые роли :small_blue_diamond:")
                .WithDescription(description.ToUpper())
                .WithColor(DiscordColor.Orange)
                );

           var putReactOn =  await ctx.Channel.SendMessageAsync(embedMessage);

            foreach (var emoji in emojis)
            {
                await putReactOn.CreateReactionAsync(emoji);
            }

            var result = await interactivity.CollectReactionsAsync(putReactOn);
        }
        #endregion

        #region Временная роль
        [Command("времроль")]
        [Description("дает участнику временную роль")]
        [RequirePermissions(Permissions.Administrator | Permissions.ModerateMembers)]
        public async Task TemporaryRole(CommandContext ctx, DiscordMember member, DiscordRole role, ulong duration)
        {
            var seconds = duration * 1000;
            if (member != null && role != null && duration != 0)
            {
                await member.GrantRoleAsync(role);
                string title = $"{member.Mention} тебе была выдана **временная** роль: {role.Mention} на {duration} c.";

                await ctx.Channel.SendMessageAsync(title);

                await Task.Delay((int)seconds);
                await member.RevokeRoleAsync(role);

                title = $"{member.Mention} **Временная** роль: {role.Mention}, была удалена!";

                await ctx.Channel.SendMessageAsync(title);
            }
        }
        #endregion

        #region Установить уровень
        [Hidden]
        [Command("уровень")]
        [RequirePermissions(Permissions.Administrator | Permissions.ModerateMembers)]
        [Description("Установите себе или указанному участнику уровень (макс. значение 100")]
        public async Task SetLevelAsync(CommandContext ctx, ulong level, DiscordMember secMember = null!)
        {
            var member = ctx.Member!;
            var guild = ctx.Guild;

            if (secMember != null) member = secMember;
            var raitingUsersEngine = new RaitingUsersEngine(member, guild);

            var isSetLevel = raitingUsersEngine.SetLevel(level, member);
            if (isSetLevel is false)
            {
                await ctx.Channel.SendMessageAsync("Неудалось установить уровень!!!");
                return;
            }

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = $"Для {member.Mention} получен уровень: {level}",
                Color = new DiscordColor("#7ee79c"),
            };

            await ctx.Channel.SendMessageAsync(embedMessage);
        }
        #endregion

        #region Добавить опыт
        [Hidden]
        [Command("опыт")]
        [RequirePermissions(Permissions.Administrator | Permissions.ModerateMembers)]
        [Description("Добавьте опыт себе или указанному участнику")]
        public async Task SetXPAsync(CommandContext ctx, string plusOrMin, ulong xp, DiscordMember member = null!)
        {
            var firstMember = (DiscordMember)ctx.User;
            var guild = ctx.Guild;

            var raitingUsersEngine = new RaitingUsersEngine(firstMember, guild);

            var isExistUser = raitingUsersEngine.CheckUserExistsToDb();
            if (isExistUser is false) return;

            var isSetXp = raitingUsersEngine.SetXPUser(plusOrMin, xp, member);

            if (isSetXp is false)
            {
                await ctx.Channel.SendMessageAsync("Не удалось установить опыт");
                return;
            }

            string? title;
            switch (plusOrMin.Replace(" ", ""))
            {
                case "+":
                    {
                        if(member is not null)
                        {
                            title = $"Для {member.Username} получен опыт в размере: {(int)xp}";
                        }

                        title = $"Получен опыт в размере: {(int)xp}";
                    }
                    break;
                case "-":
                    {
                        if (member is not null)
                        {
                            title = $"Для {member.Username} потерян опыт в размере: {(int)xp}";
                        }

                        title = $"Потерян опыт в размере: {(int)xp}";
                    }
                    break;
                default: title = $"Такого знака нет, используйте [+]прибавить, [-]отнять. Ваш знак \"{plusOrMin}\"";
                    break;
            }

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = title,
                Color = new DiscordColor("#7ee79c"),
            };

            await ctx.Channel.SendMessageAsync(embedMessage);
        }
        #endregion

        #region Команды Войс для приватного голосового канала

        [Command("войс")]
        [Description("Команда для настройки личного голосового канала")]
        public async Task VoiceAsync(CommandContext ctx, string key, DiscordMember? member = null)
        {
            ulong id = 0;

            if (ctx.Channel.Type != ChannelType.Voice && ctx.Channel.Id != id) return;
            if (key == null) return;

            switch (key.Replace(" ", ""))
            {
                case "кик":
                    {
                        if (member == null) return;
                        if (member.VoiceState == null && member?.VoiceState?.Channel == null) return;

                    }
                    break;
                default:
                    break;
            }
            await Task.CompletedTask;
        }

        [Command("войс")]
        [Description("Команда для настройки личного голосового канала")]
        public async Task VoiceAsync(CommandContext ctx, string key, [RemainingText] string? value = null)
        {
            ulong id = 0;

            if (ctx.Channel.Type != ChannelType.Voice && ctx.Channel.Id != id) return;
            if (key == null)
            {
                await ctx.Channel.SendMessageAsync($"`{key}` неверный или пустой");
                return;
            }
            if (value == null)
            {
                await ctx.Channel.SendMessageAsync($"значение для `{key}` неверный или пустой");
                return;
            }

            switch (key.Replace(" ", ""))
            {
                case "имя":
                    {
                        if (value == null) return;

                        await ctx.Channel?.ModifyAsync(ch => ch.Name = value)!;
                    }
                    break;
                case "кол-во":
                    {
                        if (value == null) return;

                        await ctx.Channel?.ModifyAsync(ch => ch.Userlimit = int.Parse(value))!;
                    }
                    break;
                default:
                    {
                        var msg = $"Введен неверный ключ, примеры:\n" +
                                  $"`!войс` `имя` `новое имя канала`\n" +
                                  $"`!войс` `кол-во` `новое количество участников`";

                        await ctx.Channel.SendMessageAsync(msg);
                    }
                    break;
            }


        }

        #endregion


    }
}
