using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DSharpPlus.VoiceNext;
using DSharpPlus.CommandsNext.Attributes;
using Discord_Bot_SmurW.Engine.PersonalityMessages;

namespace Discord_Bot_SmurW.SlashCommands
{
    public class ModerationCommands : ApplicationCommandModule
    {
        #region Ban
        [SlashCommand("бан", "Выгоняет участника с данного сервера. (Если указан параметр очистки, то очищает до 7 дней)")]
        public async Task GetBanAsync(InteractionContext ctx, [Option("участник", "Выберите учатсника которого хотите забанить")] DiscordUser user,
                                                              [Option("дни_очистки", "Количество дней для очистки последних сообщений нарушителя")] long countDay = 0,
                                                              [Option("причина", "Причина изгнания участника с данного сервера")] string? reason = null)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var member = (DiscordMember)user;
                await ctx.Guild.BanMemberAsync(member, (int)countDay, reason);

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = $"Участинк {user.Username} забанен!",
                    Description = countDay != 0 ? $"На {countDay} д. \n {reason}" : $"{reason}",
                    Color = DiscordColor.Red

                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(banMessage));
            }
            else
            {
                var isNotAdministrator = new DiscordEmbedBuilder()
                {
                    Title = "Запрещено. Не достаточно прав!",
                    Description = "Вы должны являться администратором севера",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(isNotAdministrator));
            }
        }
        #endregion

        #region UnBan
        [SlashCommand("разбан", "Рзбан участинка")]
        public async Task UnBanAsync(InteractionContext ctx, [Option("участник", "Выберите учатсника которого хотите забанить")] DiscordUser user)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var member = (DiscordMember)user;
                await ctx.Guild.UnbanMemberAsync(member);

                var unbanMessage = new DiscordEmbedBuilder()
                {
                    Title = $"Участинк {user.Username} разбанен!",
                    Description = "Теперь он один из нас, но не стоит забывать где он был до этого!",
                    Color = DiscordColor.Green

                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(unbanMessage));
            }
            else
            {
                var isNotAdministrator = new DiscordEmbedBuilder()
                {
                    Title = "Запрещено. Недостаточно прав!",
                    Description = "Вы должны являться администратором севера",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(isNotAdministrator));
            }
        }
        #endregion

        #region Kick
        [SlashCommand("кик", "Выгоняет участника с данного сервера.")]
        public async Task GetKickAsync(InteractionContext ctx, [Option("участник", "Выберите учатсника которого хотите выгнать")] DiscordUser user)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var member = (DiscordMember)user;
                await member.RemoveAsync();

                var kickMessage = new DiscordEmbedBuilder()
                {
                    Title = $"Участинк {user.Username} изгнан с данного сервера!",
                    Description = $"Кикнул: {ctx.User.Username}",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(kickMessage));
            }
            else
            {
                var isNotAdministrator = new DiscordEmbedBuilder()
                {
                    Title = "Запрещено. Не достаточно прав!",
                    Description = "Вы должны являться администратором севера",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(isNotAdministrator));
            }
        }
        #endregion

        #region TimeOut
        [SlashCommand("отдых", "Отправить участника подумать над своем поведении")]
        public async Task SetTimeoutAsync(InteractionContext ctx, [Option("Участник", "Выберите участника которого хотите отправить в timeout")] DiscordUser user,
                                                                  [Option("Период", "Укажите период в секундах")] long duration)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var timeDuration = DateTime.Now + TimeSpan.FromSeconds(duration);
                var member = (DiscordMember)user;
                await member.TimeoutAsync(timeDuration);

                var timeoutMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + "отправлен в timeout",
                    Description = "Период: " + TimeSpan.FromSeconds(duration).ToString(),
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(timeoutMessage));
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Запрещенно!",
                    Description = "Что бы использовать эту команду вы должны быть Администратором этого сервера",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }
        #endregion
    }
}
