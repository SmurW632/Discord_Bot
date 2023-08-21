using Discord_Bot_SmurW;
using Discord_Bot_SmurW.ApplicationContext;
using Discord_Bot_SmurW.Commands;
using Discord_Bot_SmurW.Engine.DataBaseEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordChannelEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordRoleEngine;
using Discord_Bot_SmurW.Engine.ImageHandler;
using Discord_Bot_SmurW.Engine.LevelSystem;
using Discord_Bot_SmurW.Engine.Models;
using Discord_Bot_SmurW.Engine.PersonalityMessages;
using Discord_Bot_SmurW.Engine.ProgramEngine.InteractionEventHandlerEngine;
using Discord_Bot_SmurW.Enums;
using Discord_Bot_SmurW.HelpFormatter;
using Discord_Bot_SmurW.SlashCommands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.SlashCommands;
using DSharpPlus.VoiceNext;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

public sealed class Program
{
    private static DiscordClient? Client { get; set; }
    private static InteractivityExtension? Interactivity { get; set; }
    private static CommandsNextExtension? Commands { get; set; }
    private static WorkDataBase? _workDB { get; set; }

    //private static int ImageIDCounter = 0;
    public static GoogleImageHandler? ImageHandler;
    private static Dictionary<ulong, ulong> _listPrivateChannels = new Dictionary<ulong, ulong>();

    private static async Task Main(string[] args)
    {
        _workDB = new WorkDataBase();
        _workDB.RunDataBase();
        
        ImageHandler = new GoogleImageHandler();

        var configEngine = new DataBaseEngine();
        var config = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = configEngine.Token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Information,
        };

        #region LvaLink config

        var endpoint = new ConnectionEndpoint
        {
            Hostname = "narco.buses.rocks",
            Port = 2269,
            Secured = false,
        };
        var lavaLinkConfiguration = new LavalinkConfiguration
        {
            Password = "glasshost1984",
            RestEndpoint = endpoint,
            SocketEndpoint = endpoint,
        };

        #endregion

        Client = new DiscordClient(config);
        Client.UseInteractivity(new InteractivityConfiguration()
        {
            Timeout = TimeSpan.FromMinutes(2)
        });

        var commandsConfig = new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { configEngine.Prefix },
            EnableMentionPrefix = true,
            EnableDms = true,
            EnableDefaultHelp = false,
        };

        Commands = Client.UseCommandsNext(commandsConfig);
        var slashCommandsConfig = Client.UseSlashCommands();

        //Prefix commands
        Commands.RegisterCommands<ModerationPrefixCommands>();
        Commands.RegisterCommands<FunCommands>();
        Commands.RegisterCommands<MusicCommands>();
        Commands.RegisterCommands<InformationCommands>();
        Commands.RegisterCommands<DataBaseCommands>();
        Commands.RegisterCommands<AdditionalCommandsForGuilds>();
        Commands.SetHelpFormatter<CustomHelpFormatter>();

        //Slash commands
        slashCommandsConfig.RegisterCommands<TestCommands>();
        slashCommandsConfig.RegisterCommands<ModerationCommands>();

        #region Подписка на события

        Client.Ready += OnClientReady;
        Client.MessageCreated += MessageSendHandler;
        Client.ModalSubmitted += ModalEventHandler;
        Commands.CommandErrored += OnCommandError;
        Client.GuildMemberAdded += JoinMemberToGuild;
        Client.VoiceStateUpdated += VoiceStateUpdated;
        Client.ComponentInteractionCreated += InteractionEventHandler;
        // События по Ролям
        Client.GuildRoleCreated += CreatedRole_EventHadler;
        Client.GuildRoleDeleted += DeletedRole_EventHandler;
        Client.GuildRoleUpdated += UpdateRole_EventHandler;
        
        // События по каналам
        Client.ChannelCreated += ChannelCreated_EventHandler;
        Client.ChannelDeleted += ChannelDeleted_EventHandler;
        Client.ChannelUpdated += ChannelUpdated_EventHandler;

        #endregion

        var lavalink = Client.UseLavalink();

        await Client.ConnectAsync();
        await lavalink.ConnectAsync(lavaLinkConfiguration);
        

        await Task.Delay(-1);
    }

    #region События Channels

    private static async Task ChannelUpdated_EventHandler(DiscordClient sender, ChannelUpdateEventArgs e)
    {
        var engine = new ChannelEngine(e.Guild);
        engine.UpdateChannel(e.ChannelAfter, e.ChannelBefore);

        await Task.CompletedTask;
    }

    private static async Task ChannelDeleted_EventHandler(DiscordClient sender, ChannelDeleteEventArgs e)
    {
        var engine = new ChannelEngine(e.Guild);
        engine.DeleteChannel(e.Channel);

        await Task.CompletedTask;
    }

    private static async Task ChannelCreated_EventHandler(DiscordClient sender, ChannelCreateEventArgs e)
    {
        var engine = new ChannelEngine(e.Guild);
        engine.CreateChannel(e.Channel);

        await Task.CompletedTask;
    }

    #endregion

    #region События Roles

    private static async Task UpdateRole_EventHandler(DiscordClient sender, GuildRoleUpdateEventArgs e)
    {
        var engine = new RoleEngine(e.Guild);
        var isValue = engine.UpdateRole(e.RoleAfter, e.RoleBefore);
        if (isValue is false) return;

        var embed = engine.SendChengedMessageToPrivateChannel(ChangedGuildRolesEnum.Update, e.RoleAfter, e.RoleBefore);

        await Client!.GetChannelAsync(1093573898378936421).Result.SendMessageAsync(embed);
        await Task.CompletedTask;
    }

    private static async Task DeletedRole_EventHandler(DiscordClient sender, GuildRoleDeleteEventArgs e)
    {
        var engine = new RoleEngine(e.Guild);
        var isValue = engine.DeleteRole(e.Role);
        if(isValue is false) return;

        var embed = engine.SendChengedMessageToPrivateChannel(ChangedGuildRolesEnum.Delete, e.Role);

        await Client!.GetChannelAsync(1093573898378936421).Result.SendMessageAsync(embed);
        await Task.CompletedTask;
    }

    private static async Task CreatedRole_EventHadler(DiscordClient sender, GuildRoleCreateEventArgs e)
    {
        var engine = new RoleEngine(e.Guild);
        var isValue = engine.CreateRole(e.Role);
        if(isValue is false) return;

        var embed = engine.SendChengedMessageToPrivateChannel(ChangedGuildRolesEnum.Create, e.Role);

        await Client!.GetChannelAsync(1093573898378936421).Result.SendMessageAsync(embed);
        await Task.CompletedTask;
    }

    #endregion

    private static async Task ModalEventHandler(DiscordClient sender, ModalSubmitEventArgs e)
    {
        var member = (DiscordMember)e.Interaction.User;
        var channelId = _listPrivateChannels[member.Id];
        var personalVC = e.Interaction.Guild.GetChannel(channelId);

        if (e.Interaction.Type == InteractionType.ModalSubmit)
        {
            foreach (var value in e.Values)
            {
                switch (value.Key)
                {
                    case "inputNameRoomId":
                        {
                            await personalVC.ModifyAsync(c => c.Name = value.Value);

                            var embed = new DiscordInteractionResponseBuilder()
                               .AddEmbed(new DiscordEmbedBuilder()
                                   .WithColor(new DiscordColor(42, 45, 59))
                                   .WithDescription($"Вы переименовали имя канала на: `{value.Value}`"))
                               .AsEphemeral(true);

                            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, embed);
                        }
                        break;

                    case "inputLimitNumberId":
                        {
                            string description = "Данные введенны некорректно!";

                            if (int.TryParse(value.Value, out int userLimit) && userLimit >= 1 && userLimit <= 99)
                            {
                                await personalVC.ModifyAsync(c => c.Userlimit = userLimit);
                                description = $"Вы установили лимит участников на: `{value.Value}`";
                            }

                            var embed = new DiscordInteractionResponseBuilder()
                               .AddEmbed(new DiscordEmbedBuilder()
                                   .WithColor(new DiscordColor(42, 45, 59))
                                   .WithDescription(description))
                               .AsEphemeral(true);

                            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, embed);
                        }
                        break;
                }
            }
        }
    }

    private static async Task JoinMemberToGuild(DiscordClient client, GuildMemberAddEventArgs e)
    {
        var roleEngine = new RoleEngine(e.Guild);
        var userEngine = new RaitingUsersEngine(e.Member, e.Guild);

        var botChannel = client.GetChannelAsync(1093573898378936421).Result;
        var defaultChannel = e.Guild.GetDefaultChannel();
        if (defaultChannel == null) return;

        var message = $"Приветствуем {e.Member.Mention}! И добро пожаловать на сервер {e.Guild.Name}";
        await defaultChannel.SendMessageAsync(message);

        //Выдача роли Гость
        //var guestRole = roleEngine.GuestRole;
        //if (guestRole == 0) return;

        //var roleNewUser = e.Guild.GetRole(guestRole);
        //await e.Member.GrantRoleAsync(roleNewUser);

        var isExists = userEngine.CheckUserExistsToDb();
        if (isExists is true)
        {
            await botChannel.SendMessageAsync("Участник уже есть в базе данных");
            return;
        }
        var isSaving = userEngine.SavingUserToDb(e.Member);
        if (isSaving is false)
        {
            await botChannel.SendMessageAsync($"Не удалось сохранить участника в базу данных {e.Member.Username}");
            return;
        }

        await botChannel.SendMessageAsync($"Участник {e.Member.Username} успешно сохранен в базу данных");
    }

    private static async Task VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e)
    {
        // если участник - бот, или канал для присод. и откл. - afk
        if (e.User.IsBot || e.After?.Channel == e.Guild.AfkChannel || e.Before?.Channel == e.Guild.AfkChannel) return;
        // если участник перемещен в другой голос. канал
        if (e.Before?.Channel != null && e.After?.Channel != null) return;

        var guild = e.Guild;
        var member = (DiscordMember)e.User;
        var timeRunBot = BotModel.TimeConnected;  
        var userEngine = new RaitingUsersEngine(member, guild);
        var channelEngine = new ChannelEngine(guild);

        var privateChannel = channelEngine.GetChannelFromDb(ChannelTypeEnum.Private);

        if (e.Before?.Channel == null && e.After?.Channel != null) // участ. подключился
        {
            if(e.After?.Channel.Id == privateChannel.ChannelId)
            {
                var personalChannel = await e.After.Channel.Guild.CreateVoiceChannelAsync(member.Username, e.Channel?.Parent);
                await personalChannel.AddOverwriteAsync(member);
                await member.ModifyAsync(u => u.VoiceChannel = personalChannel);

                _listPrivateChannels.Add(member.Id, personalChannel.Id);
            }
            userEngine.MemberConnectedVoiceChannel();
        }
        else if (e.Before?.Channel != null && e.After?.Channel == null) // участ. отключился
        {
            if(_listPrivateChannels.ContainsKey(member.Id))
            {
                if (e.Before?.Channel.Id == _listPrivateChannels[member.Id] && e.Before?.Channel.Users.Count == 0)
                {
                    await e.Before.Channel.DeleteAsync();
                    _listPrivateChannels.Remove(member.Id);
                }
                else if (e.Before?.Channel.Id == _listPrivateChannels[member.Id])
                {
                    _listPrivateChannels.Remove(member.Id);
                    var nextMember = e.Before.Channel.Users.FirstOrDefault();
                    _listPrivateChannels.Add(nextMember!.Id, e.Before.Channel.Id);
                }
            }
            
            userEngine.AddingExpPerVoice(false, timeRunBot);
        }
        await Task.CompletedTask;
    }

    private static async Task MessageSendHandler(DiscordClient sender, MessageCreateEventArgs e)
    {
        if (e.Message.Channel is DiscordDmChannel || e.Message.Content.StartsWith("!")) return;
        if(e.Author.IsBot) return;

        var member = (DiscordMember)e.Author;
        var guild = e.Guild;

        var engine = new RaitingUsersEngine(member, guild);
        var isExistUser = engine.CheckUserExistsToDb();

        if (isExistUser == false) return;
        var gettingUser = engine.GetUserFromDb();

        var levelUp = engine.AddingExpPerPost();

        if (levelUp == true)
        {
            var msgBuilder = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithDescription("Поздравляю с повышением !!!"))
                .WithReply(e.Message.Id, true);

            await e.Channel.SendMessageAsync(msgBuilder);
        }
    }

    private static async Task InteractionEventHandler(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        DiscordEmoji[] emojis =
        {
                DiscordEmoji.FromName(Client, ":shield:", false),
                DiscordEmoji.FromName(Client, ":musical_note:", false),
                DiscordEmoji.FromName(Client, ":tada:", false),
                DiscordEmoji.FromName(Client, ":heavy_multiplication_x:", false),
        };

        var interactionEventHundler = new InteractionEventHundlerLogic(e, _listPrivateChannels);

        if (e.Interaction.Data.ComponentType == ComponentType.Button)
            await interactionEventHundler.ButtonEventHandler();

        if (e.Interaction.Data.ComponentType == ComponentType.StringSelect)
            await interactionEventHundler.DropDownEventHandler();

        if(e.Interaction.Data.ComponentType == ComponentType.UserSelect)
            await interactionEventHundler.DropDownUserEventHandler();
    }

    private static Task OnClientReady(DiscordClient sender, ReadyEventArgs args)
    {
        BotModel.TimeConnected = DateTime.Now;
        return Task.CompletedTask;
    }

    private static async Task OnCommandError(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
        if (e.Exception is ChecksFailedException)
        {
            var castedException = (ChecksFailedException)e.Exception;
            string cooldownTimer = string.Empty;

            foreach (var check in castedException.FailedChecks)
            {
                var cooldown = (CooldownAttribute)check;
                TimeSpan timeLeft = cooldown.GetRemainingCooldown(e.Context);
                cooldownTimer = timeLeft.ToString(@"mm\:ss");
            }

            var cooldownMessage = new DiscordEmbedBuilder()
            {
                Title = "Дождитесь перезарядки команды",
                Description = $"Времени осталось: {cooldownTimer}",
                Color = DiscordColor.Red
            };

            await e.Context.Channel.SendMessageAsync(cooldownMessage);
        }
        else
        {
            var errorMessage = new DiscordEmbedBuilder()
            {
                Description = $"```Вызванная команда: {e.Command?.Name}\n" +
                              $"Формат строки: {e.Context.Message.Content}\n" +
                              $"Вызвана участником: {e.Context.Message.Author.Username}\n" +
                              $"В гильдии: {e.Context.Guild.Name}\n" +
                              $"В канале: {e.Context.Channel.Name}```" +
                              $"```Содержание ошибки:\n" +
                              $"Сообщение: {e.Exception.Message}\n" +
                              $"Source: {e.Exception.Source}```",
                Color = DiscordColor.Red
            };

            var channel = await e.Context.Client.GetChannelAsync(1093573898378936421);
            await channel.SendMessageAsync(errorMessage);
        }
    }
}
