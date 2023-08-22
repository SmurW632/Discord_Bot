using Discord_Bot_SmurW.Engine.LevelSystem;
using Discord_Bot_SmurW.Engine.Models;
using Discord_Bot_SmurW.HelpFormatter;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Commands
{
    [RequireGuild]
    [Category("Информация")]
    public class InformationCommands : BaseCommandModule
    {
        #region Топ 3 участника
        [RequireOwner]
        [Command("Топ")]
        [Description("Отображает топ 3 участника данного сервера")]
        public async Task TopThreeAsync(CommandContext ctx)
        {
            var member = (DiscordMember)ctx.User;
            var guild = ctx.Guild;

            var raiting = new RaitingUsersEngine(member, guild);
            var topusers = raiting.GetTopUsersRaiting();

            if(topusers == null)
            {
                await ctx.Channel.SendMessageAsync("На вашем сервере отключен рейтинг");
                return;
            }

            if(topusers.Count == 0)
            {
                await ctx.Channel.SendMessageAsync("Лидирующих участников пока что нет!");
                return;
            }

            var embedMsg = new DiscordEmbedBuilder();
            DiscordEmoji[] emojis =
            {
                DiscordEmoji.FromName(ctx.Client, ":first_place:"),
                DiscordEmoji.FromName(ctx.Client, ":second_place:"),
                DiscordEmoji.FromName(ctx.Client, ":third_place:")
            };

            int i = 0;
            foreach (var topuser in topusers)
            {
                var value = $"**Опыт:** {topuser.XP}\n" +
                            $"Уровень: {topuser.Level}\n" +
                            $"Сообщенй: {topuser.CountMessage}\n" +
                            $"Часов в войсе: {topuser.VoiceTime}\n" +
                            $"На сервере: {topuser.DaysOnTheGuild} д.";

                embedMsg.AddField($"{emojis[i]} {topuser.UserName}", value, true);

                i++;
            }

            var time = ctx.Message.Timestamp.ToUnixTimeSeconds();
            var msgBuilder = new DiscordMessageBuilder()
                .AddEmbed(embedMsg
                .WithTitle("Топ участников этого сервера по опыту")
                .WithColor(DiscordColor.Gold)
                .AddField("╭‹время›", $"<t:{time}:R>"));

            await ctx.RespondAsync(msgBuilder);
        }
        #endregion

        #region Инфо об участнике
        [Command("юзер")]
        [Description("Отображает детальную информацию участника")]
        public async Task InfoUserAsync(CommandContext ctx)
        {
            await GetUserInfoAsync(ctx);
        }

        [Command("юзер")]
        [Description("Отображает детальную информацию участника")]
        public async Task InfoUserAsync(CommandContext ctx, DiscordMember? user = null)
        {
            await GetUserInfoAsync(ctx, user, 0);
        }

        [Command("юзер")]
        [Description("Отображает детальную информацию участника")]
        public async Task InfoUserAsync(CommandContext ctx, ulong id = 0)
        {
            await GetUserInfoAsync(ctx, null, id);
        }

        private async Task GetUserInfoAsync(CommandContext ctx, DiscordMember? discordMember = null, ulong id = 0)
        {
            DiscordMember? member = null;

            if (discordMember == null && id == 0) member = ctx.Member!;
            else if (discordMember != null) member = discordMember;
            else if (id != 0) member = ctx.Guild.Members.First(m => m.Value.Id == id).Value;

            var joinedAt = member!.JoinedAt.ToString("dd.MM.yyyy");
            var dateReg = member.CreationTimestamp.ToString("dd MMMM yyyy");
            UserStatus? status;

            if (member.Presence != null) status = member.Presence.Status;
            else status = UserStatus.Offline;

            var stringStatus = "";

            if (status == UserStatus.Online)
                stringStatus = "Онлайн";
            else if (status == UserStatus.Offline)
                stringStatus = "Офлайн";
            else if (status == UserStatus.Idle)
                stringStatus = "Неактивен";
            else if (status == UserStatus.DoNotDisturb)
                stringStatus = "Не беспокоить";

            await new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor(164, 122, 255))
                    .WithThumbnail(member.AvatarUrl)
                    .WithTitle($"Информация о !{member.Username}")
                    .WithDescription("Вы можете сюда добавить какую нибудь полезную\n" +
                    "информацию о себе командой `!осебе`\n\n" +
                    "**Основная информация**")
                    .AddField("Имя пользователя:", $"{member.Username}#{member.Discriminator} (!{member.Username})")
                    .AddField("Статус:", $"{stringStatus}")
                    .AddField("Присоединился:", $"{joinedAt}")
                    .AddField("Дата регистрации:", $"{dateReg}")
                    .WithFooter($"ID: {member.Id}"))
                .WithReply(ctx.Message.Id, true)
                .SendAsync(ctx.Channel);
        }
        #endregion

        #region Информация о сервере
        [Command("сервер")]
        [Description("Показывает информацию о сервере")]
        public async Task AboutServerAsync(CommandContext ctx)
        {
            // Общее количество участников, количество людей и количество ботов
            var membersCount = ctx.Guild.MemberCount; 
            var peopleCount = ctx.Guild.Members.Where(p => !p.Value.IsBot).Count();
            var botsCount = ctx.Guild.Members.Where(p => p.Value.IsBot).Count();
            // Владелец
            var ownerGuild = $"{ctx.Guild.Owner.Username}#{ctx.Guild.Owner.Discriminator}"; 
            // Уровень проверки
            var verificationLevel = ctx.Guild.VerificationLevel; 
            string stringVerificationLevel = ""; 
            // количество каналов и количество каналов каждого типа
            var voiceChannelsCount = ctx.Guild.Channels.Where(ch => ch.Value.Type == ChannelType.Voice).Count(); 
            var txtChannelsCount = ctx.Guild.Channels.Where(ch => ch.Value.Type == ChannelType.Text).Count(); 
            var forumChannelsCouont = ctx.Guild.Channels.Where(ch => ch.Value.Type == ChannelType.GuildForum).Count(); 
            var channelsCount = voiceChannelsCount + txtChannelsCount + forumChannelsCouont; 
            // дата создания сервера
            var dateCreateGuild = ctx.Guild.CreationTimestamp.ToString("dd:MM:yyyy");
            // количество участников по статусам
            var onlineMembersCount = ctx.Guild.Members.Where(m => m.Value.Presence != null && m.Value.Presence.Status == UserStatus.Online).Count();
            var doNotDustrubMembersCount = ctx.Guild.Members.Where(m => m.Value.Presence != null && m.Value.Presence.Status == UserStatus.DoNotDisturb).Count();
            var offlineMembersCount = ctx.Guild.Members.Where(m => m.Value.Presence == null).Count();
            var idleMembersCount = ctx.Guild.Members.Where(m => m.Value.Presence != null && m.Value.Presence.Status == UserStatus.Idle).Count();

            DiscordEmoji[] emojis =
            {
                DiscordEmoji.FromName(ctx.Client, ":status_online:", true),
                DiscordEmoji.FromName(ctx.Client, ":status_offline:", true),
                DiscordEmoji.FromName(ctx.Client, ":status_dnd:", true),
                DiscordEmoji.FromName(ctx.Client, ":status_idle:", true),
                DiscordEmoji.FromName(ctx.Client, ":bot:", true),
                DiscordEmoji.FromName(ctx.Client, ":user_icon:", true),
                DiscordEmoji.FromName(ctx.Client, ":users:", true),
                DiscordEmoji.FromName(ctx.Client, ":channels:", true),
                DiscordEmoji.FromName(ctx.Client, ":voice_channel:", true),
                DiscordEmoji.FromName(ctx.Client, ":txt_channel:", true),
                DiscordEmoji.FromName(ctx.Client, ":forum_channel:", true),
            };

            if (verificationLevel == VerificationLevel.None)
                stringVerificationLevel = "Отсутствует";
            else if (verificationLevel == VerificationLevel.Low)
                stringVerificationLevel = "Низкий";
            else if (verificationLevel == VerificationLevel.Medium)
                stringVerificationLevel = "Средний";
            else if (verificationLevel == VerificationLevel.High)
                stringVerificationLevel = "Высокий";
            else if (verificationLevel == VerificationLevel.Highest)
                stringVerificationLevel = "Самый высокий";

            await new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle($"Информация о сервере {ctx.Guild.Name}")
                    .WithColor(new DiscordColor(164, 122, 255))
                    .WithThumbnail(ctx.Guild.IconUrl)
                    .AddField("Участники:", $"{emojis[6]} Всего: {membersCount}\n" +
                                            $"{emojis[5]} Людей: {peopleCount}\n" +
                                            $"{emojis[4]} Ботов: {botsCount}", true)
                    .AddField("По статусам:", $"{emojis[0]} В сети: {onlineMembersCount}\n" +
                                              $"{emojis[3]} Не активен: {idleMembersCount}\n" +
                                              $"{emojis[2]} Не беспокоить: {doNotDustrubMembersCount}\n" +
                                              $"{emojis[1]} Не в сети: {offlineMembersCount}", true)
                    .AddField("Каналы:", $"{emojis[7]} Всего: {channelsCount}\n" +
                                         $"{emojis[8]} Голосовых: {voiceChannelsCount}\n" +
                                         $"{emojis[9]} Текстовых: {txtChannelsCount}\n" +
                                         $"{emojis[10]} Форумов: {forumChannelsCouont}", true)
                    .AddField("Владелец:", $"{ownerGuild}", true)
                    .AddField("Уровень проверки:", $"{stringVerificationLevel}", true)
                    .AddField("Дата создания:", $"{dateCreateGuild}", true)
                    .WithFooter($"ID: {ctx.Guild.Id}"))
                .WithReply(ctx.Message.Id, true)
                .SendAsync(ctx.Channel);

        }
        #endregion

        #region помощь
        //[RequireGuild]
        [Command("хелп")] 
        [Description("Справка по всем доступным категориям и командам")]
        public async Task HelpCommand(CommandContext ctx, string? details = null)
        {
            var allCmd = ctx.CommandsNext.RegisteredCommands.Values;
            var countCommand = allCmd.Count();

            var cmdsCategoryFun         = allCmd.Where(c => c.Category?.ToLower() == "фан").ToList();
            var cmdsByCategoryInfo      = allCmd.Where(c => c.Category?.ToLower() == "информация").ToList(); 
            var cmdsByCategoryMusic     = allCmd.Where(c => c.Category?.ToLower() == "музыка").ToList(); 
            var cmdsCategoryModeration  = allCmd.Where(c => c.Category?.ToLower() == "модерация").ToList(); 

            var btn = new DiscordButtonComponent(ButtonStyle.Secondary, "btn", $"Отправлено с {ctx.Guild.Name}", true,
                new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":incoming_envelope:")));

            
            var listInfo = string.Join(" ", cmdsByCategoryInfo.Select(c => $"`!{c.Name}`"));
            var listFun = string.Join(" ", cmdsCategoryFun.Select(c => $"`!{c.Name}`"));
            var listMusic = string.Join(" ", cmdsByCategoryMusic.Select(c => $"`!{c.Name}`"));
            var listModer = string.Join(" ", cmdsCategoryModeration.Select(c => $"`!{c.Name}`"));


            if (details == null)
            {
                var listCommandsMessage = new DiscordMessageBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle("Доступные команды:")
                            .WithDescription("Вы можете получить детальную справку по каждой\n" +
                                             " команде, указав название. Например: `!помоги пауза`")
                            .AddField($"📝 Информация (!хелп Информация)", $"{listInfo}")
                            .AddField($"🛡 ️Модерация (!хелп Модерация)", $"{listModer}")
                            .AddField($"🎵 Музыка (!хелп Музыка)", $"{listMusic}")
                            .AddField($"🥳 Фан (!хелп Фан)", $"{listFun}")
                            .WithThumbnail(ctx.Client.CurrentUser.AvatarUrl)
                            .WithColor(new DiscordColor(43, 45, 49))
                            .WithFooter("Relax © 2023 Все права почти защищены"))
                        .AddComponents(btn);

                await ctx.Member!.SendMessageAsync(listCommandsMessage);
                await ctx.RespondAsync($"{ctx.Member.Mention} я отправил тебе список команд в личку");
            }
            else
            {
                var cmdDetails = allCmd.Where(c => c.Name.ToLower() == details.Replace(" ", "").ToLower() ||
                c.Category!.ToLower() == details.Replace(" ", "").ToLower());

                if (!cmdDetails.Any())
                {
                    await ctx.RespondAsync($"{details} - такой команды или категории не существует");
                }
                else
                {
                    Command? executeCmd = cmdDetails.FirstOrDefault();
                    DiscordEmbed? embedMessage;
                    
                    switch (details)
                    {
                        case "модерация":
                            {
                                await HelperForCategories(ctx, cmdsCategoryModeration, btn, "Модерация");
                            }
                            break;
                        case "информация":
                            {
                                await HelperForCategories(ctx, cmdsByCategoryInfo, btn, "Информация");
                            }
                            break;
                        case "музыка":
                            {
                                await HelperForCategories(ctx, cmdsByCategoryMusic, btn, "Музыка");
                            }
                            break;
                        case "фан":
                            {
                               await HelperForCategories(ctx, cmdsCategoryFun, btn, "Фан");
                            }
                            break;
                        default:
                            {
                                var helpFormatter = new CustomHelpFormatter(ctx);
                                    
                                embedMessage = helpFormatter.WithCommand(executeCmd!).Build().Embed;
                                await new DiscordMessageBuilder()
                                    .AddEmbed(embedMessage)
                                    .WithReply(ctx.Message.Id, true)
                                    .SendAsync(ctx.Channel);
                            }
                            break;
                    }
                }
            }
        }
        private async Task HelperForCategories(CommandContext ctx, List<Command> commands, DiscordButtonComponent btn, string category)
        {
            var helpFormatter = new CustomHelpFormatter(ctx);

            var embedMessage = helpFormatter.WithSubcommands(commands.AsEnumerable()).Build();
            var msgBuilder = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle($"Доступные команды по категории {category}")
                .WithDescription($"Вы можете получить детальную справку" +
                                 $" по каждой команде, выполнив её со знаком `?`" +
                                 $" или указав название. " +
                                 $"Например: `!хелп ?` или `!хелп инфо`\n" +
                                 $"```ansi{embedMessage.Content}```")
                .WithColor(new DiscordColor(43, 45, 49))
                .WithThumbnail(ctx.Client.CurrentUser.AvatarUrl))
                .AddComponents(btn);

            await ctx.Member!.SendMessageAsync(msgBuilder);
            await ctx.RespondAsync($"{ctx.Member.Mention} я отправил тебе список команд по категории {category} в личку ");
        }
        #endregion

        #region Рейтинг 2.0
        [RequireOwner]
        [Command("Рейтинг")]
        [Description("Отображает текущий рейтинг участника")]
        public async Task RatingUsersCommand(CommandContext ctx, DiscordMember secondMember = null!)
        {
            var firstMember = (DiscordMember)ctx.User;
            var dcGuild = ctx.Guild;

            if (secondMember != null) firstMember = secondMember;
            var ratingUsersEngine = new RaitingUsersEngine(firstMember, dcGuild);

            ratingUsersEngine.GettingDaysOnTheGuild();
            ratingUsersEngine.AddingExpPerVoice(true, BotModel.TimeConnected);

            var currentUser = ratingUsersEngine.GetUserFromDb();

            var viewRaiting = new DiscordMessageBuilder()
                       .AddEmbed(new DiscordEmbedBuilder()
                       .WithColor(DiscordColor.Wheat)
                       .WithImageUrl(firstMember.AvatarUrl)
                       .WithTitle($"Профиль {currentUser.UserName}")
                       .WithThumbnail(currentUser.AvatarUrl)
                       .AddField("Дней на сервере", $"📅 {currentUser.DaysOnTheGuild} д.", true)
                       .AddField("Сообщений ", $"📨 {currentUser.CountMessage}", true)
                       .AddField("Часов в voice", $"⏳ {(int)currentUser.VoiceTime}ч. {currentUser.TotalMinutesForSession}м.", false)
                       .AddField("Опыт", $"⭐ {(int)currentUser.XP} xp", true)
                       .AddField("Уровень", $"😎 {currentUser.Level}", false));

            await ctx.Channel.SendMessageAsync(viewRaiting);
        }
        #endregion

    }
}
