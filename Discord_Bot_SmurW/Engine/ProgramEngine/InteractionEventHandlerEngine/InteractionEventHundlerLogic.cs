using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordChannelEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordRoleEngine;
using Discord_Bot_SmurW.Engine.ImageHandler;
using Discord_Bot_SmurW.Engine.LevelSystem;
using Discord_Bot_SmurW.Engine.Models;
using Discord_Bot_SmurW.Engine.PersonalityMessages;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.ProgramEngine.InteractionEventHandlerEngine
{
    public class InteractionEventHundlerLogic
    {
        private static ComponentInteractionCreateEventArgs? _e;
        private static DiscordEmoji[]? _emojis;
        private static GoogleImageHandler? _imageHandler;
        private static int _imageIdCounter = 0;

        private static Dictionary<ulong, bool> _hideShowPrivateChannels = new();
        private static Dictionary<DiscordChannel, bool> _openClosePersonalChannel = new();
        private static Dictionary<ulong, ulong> _listPrivateChannels = new();


        public InteractionEventHundlerLogic(ComponentInteractionCreateEventArgs e, Dictionary<ulong, ulong> listPrivateChannels, DiscordEmoji[] emoji = null!)
        {
            _e = e;
            _emojis = emoji;
            _imageHandler = new GoogleImageHandler();
            _listPrivateChannels = listPrivateChannels;
        }

        private async Task<DiscordChannel> IsOwnChannel()
        {
            var member = (DiscordMember)_e!.User;
            if (!_listPrivateChannels.ContainsKey(member.Id))
            {
                var msg = new DiscordInteractionResponseBuilder()
                   .AddEmbed(new DiscordEmbedBuilder()
                       .WithColor(new DiscordColor(42, 45, 59))
                       .WithDescription("У вас нет во владении голосового канала!"))
                   .AsEphemeral(true);

                await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, msg);

                return null!;
            }

            var value = _listPrivateChannels[member.Id];
            return _e.Guild.GetChannel(value);
        }

        public async Task ButtonEventHandler()
        {
            var member = (DiscordMember)_e!.User;

            switch (_e!.Interaction.Data.CustomId)
            {
                case "btnClose":
                    {
                        await _e.Message.DeleteAsync();
                    }
                    break;

                case "btnYesDeletedMessage":
                    {
                        var count = TypeMessage.CountDeleteMessages;

                        var messages = await _e.Interaction.Channel.GetMessagesAsync(count);
                        await _e.Interaction.Channel.DeleteMessagesAsync(messages);
                    }
                    break;

                case "btnPreviousButton":
                    {
                        _imageIdCounter--; //Decrement the ID by 1 to get the ID for the previous image
                        string imageURL = Program.ImageHandler!.GetImageAtId(_imageIdCounter); //Get the image from the Dictionary


                        var previousButton = new DiscordButtonComponent(ButtonStyle.Primary, "btnPreviousButton", "Previous", false);

                        var nextButton = new DiscordButtonComponent(ButtonStyle.Primary, "btnNextButton", "Next", false);

                        var imageMessage = new DiscordMessageBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                            .WithColor(DiscordColor.Azure)
                            .WithTitle("Results")
                            .WithImageUrl(imageURL)
                            .WithFooter("Page " + _imageIdCounter)
                            )
                            .AddComponents(previousButton, nextButton);
                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                            new DiscordInteractionResponseBuilder().AddEmbed(imageMessage.Embed).AddComponents(imageMessage.Components));
                    }
                    break;

                case "btnNextButton":
                    {
                        _imageIdCounter++;
                        string imageURL = Program.ImageHandler!.GetImageAtId(_imageIdCounter);

                        var previousButton = new DiscordButtonComponent(ButtonStyle.Primary, "btnPreviousButton", "Previous", false);
                        var nextButton = new DiscordButtonComponent(ButtonStyle.Primary, "btnNextButton", "Next", false);

                        var imageMessage = new DiscordMessageBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                            .WithColor(DiscordColor.Azure)
                            .WithTitle("Results")
                            .WithImageUrl(imageURL)
                            .WithFooter("Page " + _imageHandler)
                            )
                            .AddComponents(previousButton, nextButton);
                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                            new DiscordInteractionResponseBuilder().AddEmbed(imageMessage.Embed).AddComponents(imageMessage.Components));
                    }
                    break;

                case "btnRightToSpeak":
                    {
                        var personalVC = IsOwnChannel().Result;
                        if (personalVC == null) break;
                        
                        var dropDown = new DiscordUserSelectComponent("ddlGrantUserRightToSpeak", "Выберите участника...");

                        var embed = new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithDescription("Выдать право говорить")
                                .WithColor(new DiscordColor(42, 45, 59)))
                            .AddComponents(dropDown)
                            .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, embed);
                    }
                    break;

                case "btnKickMember":
                    {
                        var personalVC = IsOwnChannel().Result;
                        if (personalVC == null) break;

                        var optionList = new List<DiscordUserSelectComponent>();
                        var countMembersVC = personalVC.Users.Count;

                        if (countMembersVC == 1)
                        {
                            var interaction = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription("Вы единственный в этом канале!")
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                            await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interaction);

                            break;
                        }

                        var dropDown = new DiscordUserSelectComponent("ddlAllMembers", "Выберите участника...");

                        var embed = new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithDescription("Выберите участника которого хотите выгнать!")
                                .WithColor(new DiscordColor(42, 45, 59)))
                            .AddComponents(dropDown)
                            .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, embed);
                    }
                    break;

                case "btnShowHideRoom":
                    {
                        var personalVC = IsOwnChannel().Result;
                        if (personalVC == null) break;

                        string description = "";

                        if (_hideShowPrivateChannels.TryGetValue(personalVC.Id, out bool isAccessEnable) && isAccessEnable)
                        {
                            await personalVC.ModifyAsync(c => c.PermissionOverwrites = new List<DiscordOverwriteBuilder>()
                            {
                                new DiscordOverwriteBuilder(_e.Guild.EveryoneRole)
                                .Allow(Permissions.AccessChannels)
                            });
                            description = "Вы открыли доступ к просмотру канала!";
                            _hideShowPrivateChannels[personalVC.Id] = false;
                        }
                        else
                        {
                            await personalVC.ModifyAsync(c => c.PermissionOverwrites = new List<DiscordOverwriteBuilder>()
                            {
                                new DiscordOverwriteBuilder(_e.Guild.EveryoneRole)
                                .Deny(Permissions.AccessChannels)
                            });
                            description = "Вы закрыли доуступ к просмотру канала";

                            _hideShowPrivateChannels[personalVC.Id] = true;
                        }

                        var embed = new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithColor(new DiscordColor(42, 45, 59))
                                .WithDescription(description))
                            .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, embed);
                    }
                    break;

                case "btnChangedNameRoom":
                    {
                        var personalVC = IsOwnChannel().Result;
                        if (personalVC == null) break;

                        var embed = new DiscordInteractionResponseBuilder()
                            .WithCustomId("modalNameRoom")
                            .WithTitle("Название комнаты")
                            .AddComponents(new TextInputComponent("Имя", "inputNameRoomId", "введите новое название комнаты...", null, true, TextInputStyle.Short, 1, 40));

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, embed);
                    }
                    break;

                case "btnOpenCloseRoom":
                    {
                        var personalVC = IsOwnChannel().Result;
                        if (personalVC == null) break;

                        var description = "";
                        if (_openClosePersonalChannel.TryGetValue(personalVC, out var isOpen) && isOpen)
                        {
                            await personalVC.ModifyAsync(v => v.PermissionOverwrites = new List<DiscordOverwriteBuilder>()
                            {
                                new DiscordOverwriteBuilder(_e.Guild.EveryoneRole)
                                    .Allow(Permissions.UseVoice | Permissions.Speak)
                            });
                            description = "Вы открыли комнату!";
                            _openClosePersonalChannel[personalVC] = false;
                        }
                        else
                        {
                            await personalVC.ModifyAsync(v => v.PermissionOverwrites = new List<DiscordOverwriteBuilder>()
                            {
                                new DiscordOverwriteBuilder(_e.Guild.EveryoneRole)
                                    .Deny(Permissions.UseVoice | Permissions.Speak)
                            });
                            description = "Вы закрыли комнату!";
                            _openClosePersonalChannel[personalVC] = true;
                        }

                        var embed = new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithColor(new DiscordColor(42, 45, 59))
                                .WithDescription(description))
                            .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, embed);
                    }
                    break;

                case "btnLimitMembers":
                    {
                        var personalVC = IsOwnChannel().Result;
                        if (personalVC == null) break;

                        var embed = new DiscordInteractionResponseBuilder()
                             .WithCustomId("modalLimitMember")
                             .WithTitle("Лимит участников")
                             .AddComponents(new TextInputComponent("максимальное количество от 1 до 99", "inputLimitNumberId", "введите лимит участников...", null, true, TextInputStyle.Short, 1, 2));

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, embed);
                    }
                    break;

                case "btnAccess":
                    {
                        var personalVC = IsOwnChannel().Result;
                        if (personalVC == null) break;

                        var dropDown = new DiscordUserSelectComponent("ddlAccessUser", "Выберите участника...");

                        var embed = new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithDescription("Запретить участнику присоединяться и видеть комнату!")
                                .WithColor(new DiscordColor(42, 45, 59)))
                            .AddComponents(dropDown)
                            .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, embed);
                    }
                    break;

                case "btnCreator":
                    {
                        var personalVC = IsOwnChannel().Result;
                        if (personalVC == null) break;

                        var countUsers = personalVC.Users.Count;
                        if (countUsers == 1)
                        {
                            var interaction = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription("Вы единственный в этом канале!")
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                            await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interaction);

                            break;
                        }

                        var dropDown = new DiscordUserSelectComponent("ddlGrantUserRightToLeader", "Выберите участника...");

                        var embed = new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithDescription("Передайте право лидера другому участнику!")
                                .WithColor(new DiscordColor(42, 45, 59)))
                            .AddComponents(dropDown)
                            .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, embed);

                    }
                    break;
            }
        }
        public async Task DropDownEventHandler()
        {
            var guild = _e!.Guild;
            var roleEngine = new RoleEngine(guild);
            var channelEngine = new ChannelEngine(guild);
            var member = (DiscordMember)_e.User;

            //game roles
            var horror = guild.GetRole(1094913928099794955);
            var dbd = guild.GetRole(928244382639980586);
            var csgo = guild.GetRole(824196020077461574);
            var sot = guild.GetRole(989038636639072257);
            var gta = guild.GetRole(824199697723424768);

            //hobby roles
            var artist = guild.GetRole(1109504662358458470);
            var musican = guild.GetRole(1109506192344100964);
            var bloger = guild.GetRole(1109507127942316094);
            var gamer = guild.GetRole(1109508168033259593);
            var coder = guild.GetRole(1109508957724233871);
            var montage = guild.GetRole(1109509715773366444);

            if (channelEngine.CheckChannelExistToDb(ChannelTypeEnum.Rules) == false) return;

            var rulesCHId = channelEngine.GetChannelFromDb(ChannelTypeEnum.Rules).ChannelId;
            var rulesCH = guild.GetChannel(rulesCHId);

            switch (_e!.Id)
            {
                case "ddlNavigation":
                    {
                        foreach (var value in _e.Values)
                        {
                            switch (value)
                            {
                                case "optRules":
                                    {
                                        var description = $"Вы так же более подробно можете ознакомиться с ними в канале -\n {rulesCH.Mention}\r\n" +
                                                          $"```ansi\r\n\u001b[2;31m\u001b[2;36m1. Уважение друг к другу\u001b[0m\u001b[2;31m\u001b[0m\u001b[2;31m\u001b[2;31m\u001b[0m\u001b[2;31m\u001b[0m\r\n```" +
                                                          $"```1.1 Оскорбление кого либо, токсичное, неадекватное поведение.\n" +
                                                          $"1.2 Оскорбление сервера.\n" +
                                                          $"1.3 Выдача себя за другую личность.\n" +
                                                          $"1.4 Розжиг конфликтов.\n" +
                                                          $"1.5 Сообщения не по теме.\n" +
                                                          $"╰ Бред, нецензурные сообщения просто так.\n" +
                                                          $"1.6 Запрещены нечитаемые ники, символики или несущие оскорбительный характер.```\n" +
                                                          $"```ansi\r\n\u001b[2;31mНаказание\u001b[0m\r\n```" +
                                                          $"```1 предупреждение - устное или мут на 30 минут.\n" +
                                                          $"2 предупреждения - мут на 6 часов\n" +
                                                          $"3 предупреждения - мут на 1 день\n" +
                                                          $"4 предупреждения - мут на 3 дня\n" +
                                                          $"5 предупреждений - бан на 1 месяц```\n" +
                                                          $"```ansi\r\n\u001b[0;2m\u001b[0m\u001b[2;36m2. Важные пункты\u001b[0m\r\n```" +
                                                          $"```2.1 Контент не для совершеннолетних.\n" +
                                                          $"2.2 Пропаганда запрещенных веществ\n" +
                                                          $"2.3 Использование дыр/уязвимостей дискорда/ботов\n" +
                                                          $"2.4 Оскорбление родственников.```\n" +
                                                          $"```ansi\r\n\u001b[2;31mНаказание\u001b[0m\r\n```" +
                                                          $"```Бан без истечения срока.```\n\n" +
                                                          $"```ansi\r\n\r\n\u001b[2;33m⠂Незнание правил не освобождает от ответственности.\r\n" +
                                                          $"⠂Не прикрывайтесь дырами в правилах.\r\n" +
                                                          $"⠂Прочие правила регламентируются согласно нормам большинства других серверов.\r\n" +
                                                          $"⠂Администрация имеет право закрыть глаза на мелкие или первые нарушения пользователей.\u001b[0m\r\n\r\n```";

                                        var interactionBuilder = new DiscordInteractionResponseBuilder()
                                            .AddEmbed(new DiscordEmbedBuilder()
                                                .WithDescription(description)
                                                .WithColor(new DiscordColor(43, 45, 49))
                                                .Build())
                                            .AsEphemeral(true);

                                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interactionBuilder);
                                    }
                                    break;
                                case "optRoles":
                                    {
                                        if (_e!.Guild.Name != "Relax") return;


                                        var rolesToDb = roleEngine.GetListRolesFromDb();
                                        var discordRoles = guild.Roles.Values;

                                        var gRole = discordRoles.Where(r => rolesToDb.Any(g => r.Id == g.RoleId && g.RoleTypeName == "Game"));
                                        var listGameRoles = string.Join(" ", gRole.Select(g => $"{g.Mention}\n"));

                                        var sRole = discordRoles.Where(r => rolesToDb.Any(g => r.Id == g.RoleId && g.RoleTypeName == "Senior"));
                                        var listSeniorRoles = string.Join(" ", sRole.Select(g => $"{g.Mention}\n"));

                                        var iRole = discordRoles.Where(r => rolesToDb.Any(g => r.Id == g.RoleId && g.RoleTypeName == "Individual"));
                                        var listIndividualRoles = string.Join(" ", iRole.Select(g => $"{g.Mention}\n"));

                                        var hRole = discordRoles.Where(r => rolesToDb.Any(g => r.Id == g.RoleId && g.RoleTypeName == "Hobby"));
                                        var listHobbyRoles = string.Join(" ", hRole.Select(g => $"{g.Mention}\n"));


                                        var description = $"```Старшие роли```\r\n" +
                                                          $"{listSeniorRoles}" +
                                                          $"\r\n```Индивидуальные роли```\r\n" +
                                                          $"{listIndividualRoles}" +
                                                          $"\r\n```Игровые роли```\r\n" +
                                                          $"{listGameRoles}" +
                                                          $"\r\n```Роли по интересам```\r\n" +
                                                          $"{listHobbyRoles}";

                                        var interactionBuilder = new DiscordInteractionResponseBuilder()
                                            .AddEmbed(new DiscordEmbedBuilder()
                                                .WithDescription(description)
                                                .WithColor(new DiscordColor(43, 45, 49))
                                                .Build())
                                            .AsEphemeral(true);

                                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interactionBuilder);
                                    }
                                    break;
                                case "optCommand":
                                    {
                                        var channelPanda = guild.GetChannel(1108734208752484404);
                                        var channelMusicCom = guild.GetChannel(1096013169887023165);

                                        var interactionBuilder = new DiscordInteractionResponseBuilder()
                                            .AddEmbed(new DiscordEmbedBuilder()
                                                .WithDescription("```Основные команды```\r\n" +
                                                                 "❀ `!хелп` - список всех команд отправятся в личку\n" +
                                                                 "╰ Пример: `!хелп`, `!хелп [команда]`, `!хелп [категория]`\n" +
                                                                 "❀ `!топ` - отобразит топ 3 участника по рейтингу, если он включен\n" +
                                                                 "❀ `!сервер` - отобразит информацию о сервере\n" +
                                                                 "❀ `!юзер` - отображет информацию о себе или о выбранном участнике\n" +
                                                                 "╰ Пример: `!юзер`, `!юзер [@участник]`, `!юзер [id участника]`\n" +
                                                                 "❀ `!картинки` - покажет коллекцию картинок с вашего запроса\n" +
                                                                 "╰ Пример: `!картинки [название картинки]`\n" +
                                                                 "❀ `!поцеловать` - отправит поцелуй участнику\n" +
                                                                 "╰ Пример: `!поцеловать [@участник]`\n" +
                                                                 "❀ `!шлепнуть` - ударить участника\n" +
                                                                 "❀ `!пнуть` - дать пинок участнику\n" +
                                                                 "❀ `!обнять` - обнять участника\n" +
                                                                 "❀ `!послать` - все в рамках приличия, иначе АцТань\n" +
                                                                 "❀ `!привет` - поприветствовать участника\n" +
                                                                 "\r\n```Музыкальные команды```\r\n" +
                                                                 "❀ `!играть` - играет музыку с вашего запроса\n" +
                                                                 "╰ Пример: `!играть [название трека]`\n" +
                                                                 "❀ `!пауза` - ставит на паузу трек\n" +
                                                                 "❀ `!прод` - продолжает воспроизводить трек\n" +
                                                                 "❀ `!стоп` - останавливает трек и отключает бота с voice-канала\n" +
                                                                 $"\r\nОсновные команды используются в канале {channelPanda.Mention}\n" +
                                                                 $"Музыкальные команды используются в канале {channelMusicCom.Mention}")
                                                .WithColor(new DiscordColor(43, 45, 49))
                                                .Build())
                                            .AsEphemeral(true);

                                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interactionBuilder);
                                    }
                                    break;
                                case "optChannel":
                                    {
                                        var description = $"```            Полезный раздел```\r\n" +
                                                          $"{guild.GetChannel(1108617891881943072).Mention} - правила сервера\n" +
                                                          $"{guild.GetChannel(1094656074516275330).Mention} - получение ролей\n" +
                                                          $"{guild.GetChannel(825339932000256041).Mention} - для команд лисички\n" +
                                                          $"{guild.GetChannel(1108734208752484404).Mention} - для команд панды\n" +
                                                          $"\r\n```                 Форумы```\r\n" +
                                                          $"{guild.GetChannel(1094951185137541190).Mention} - форум по игре Dead By Daylight\n" +
                                                          $"\r\n```                 Общение```\r\n" +
                                                          $"{guild.GetChannel(824195873620623424).Mention} - общий чат\n" +
                                                          $"{guild.GetChannel(1109486038172315738).Mention} - раздел по увлечениям\n" +
                                                          $"{guild.GetChannel(1094954830222606347).Mention} - чат по игре Dead By Daylight\n" +
                                                          $"\r\n```                 Музыка```\r\n" +
                                                          $"{guild.GetChannel(1096013169887023165).Mention} - команды для управления музыкой\n" +
                                                          $"{guild.GetChannel(1096012816198156288).Mention} - слушать свою любимую музыку\n" +
                                                          $"\r\n```            Личные комнаты```\r\n" +
                                                          $"{guild.GetChannel(1095569256671891487).Mention} - управление привт-комнатой\n" +
                                                          $"{guild.GetChannel(1095628286173913200).Mention} - приват команта\n";

                                        var interactionBuilder = new DiscordInteractionResponseBuilder()
                                            .AddEmbed(new DiscordEmbedBuilder()
                                                .WithDescription(description)
                                                .WithColor(new DiscordColor(43, 45, 49))
                                                .Build())
                                            .AsEphemeral(true);

                                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interactionBuilder);
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                case "ddlGetRoles":
                    {
                        foreach (var value in _e.Values)
                        {
                            switch (value)
                            {
                                case "optSelectedGameR":
                                    {
                                        var optionList = new List<DiscordSelectComponentOption>()
                                        {
                                            new DiscordSelectComponentOption($"⠂{horror.Name}", "optHorrorRole"),
                                            new DiscordSelectComponentOption($"⠂{dbd.Name}", "optDbdRole"),
                                            new DiscordSelectComponentOption($"⠂{csgo.Name}", "optCsgoRole"),
                                            new DiscordSelectComponentOption($"⠂{sot.Name}", "optSotRole"),
                                            new DiscordSelectComponentOption($"⠂{gta.Name}", "optGtaRole")
                                        };

                                        var option = optionList.AsEnumerable();
                                        var dropDown = new DiscordSelectComponent("ddlGameRoles", "Выберите роли...", option, false, 1, 5);

                                        var description = $"```ansi\r\n\u001b[2;36mВ\u001b[0m\u001b[2;36mы можете выбрать\u001b[0m \u001b[2;31mот 1 до 5\u001b[0m \u001b[2;36mролей\u001b[0m\r\n\r\n```\n" +
                                                          $"{dbd.Mention} `- Dead By Daylight (в простонародии - помойка)`\n" +
                                                          $"{gta.Mention} `- что по РП?`\n" +
                                                          $"{csgo.Mention} `- CS GO о да, мы любители пострелять, а ты?`\n" +
                                                          $"{sot.Mention} `- SoT, ты смотри, а то уплывет твой кораблик`\n" +
                                                          $"{horror.Mention} - `Если вы играете в страшные кооперативные игры`";

                                        var interactionBuilder = new DiscordInteractionResponseBuilder()
                                           .AddEmbed(new DiscordEmbedBuilder()
                                               .WithDescription(description)
                                               .WithColor(new DiscordColor(43, 45, 49))
                                               .Build())
                                           .AddComponents(dropDown)
                                           .AsEphemeral(true);

                                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interactionBuilder);
                                    }
                                    break;

                                case "optSelectedHobbyR":
                                    {
                                        var optionList = new List<DiscordSelectComponentOption>()
                                {
                                    new DiscordSelectComponentOption($"⠂{artist.Name}", "optArtistRole"),
                                    new DiscordSelectComponentOption($"⠂{musican.Name}", "optMusicanRole"),
                                    new DiscordSelectComponentOption($"⠂{bloger.Name}", "optBlogerRole"),
                                    new DiscordSelectComponentOption($"⠂{gamer.Name}", "optGamerRole"),
                                    new DiscordSelectComponentOption($"⠂{coder.Name}", "optCoderRole"),
                                    new DiscordSelectComponentOption($"⠂{montage.Name}", "optMontageRole"),
                                };

                                        var option = optionList.AsEnumerable();
                                        var dropDown = new DiscordSelectComponent("ddlHobbyRoles", "Выберите роли...", option, false, 1, 6);

                                        var description = $"```ansi\r\n\u001b[2;36mВ\u001b[0m\u001b[2;36mы можете выбрать\u001b[0m \u001b[2;31mот 1 до 6\u001b[0m \u001b[2;36mролей\u001b[0m\r\n\r\n```\n" +
                                                          $"{artist.Mention} `- любишь рисовать?`\n" +
                                                          $"{musican.Mention} `- спой мне любимую песенку`\n" +
                                                          $"{bloger.Mention} `- ставьте лайки, подписывайтесь на канал`\n" +
                                                          $"{gamer.Mention} `- поиграем?`\n" +
                                                          $"{coder.Mention} - `Heap г**** я наблюдаю в своем Stack-е`\n" +
                                                          $"{montage.Mention} - `покажи, я лайкну`";

                                        var interactionBuilder = new DiscordInteractionResponseBuilder()
                                           .AddEmbed(new DiscordEmbedBuilder()
                                               .WithDescription(description)
                                               .WithColor(new DiscordColor(43, 45, 49))
                                               .Build())
                                           .AddComponents(dropDown)
                                           .AsEphemeral(true);

                                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interactionBuilder);
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                case "ddlGameRoles":
                    {
                        var gRoles = new List<DiscordRole>();

                        foreach (var value in _e.Values)
                        {
                            switch (value)
                            {
                                case "optHorrorRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == horror.Id)) break;

                                        await member.GrantRoleAsync(horror);
                                        gRoles.Add(horror);
                                    }
                                    break;
                                case "optDbdRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == dbd.Id)) break;

                                        await member.GrantRoleAsync(dbd);
                                        gRoles.Add(dbd);
                                    }
                                    break;
                                case "optCsgoRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == csgo.Id)) break;

                                        await member.GrantRoleAsync(csgo);
                                        gRoles.Add(csgo);
                                    }
                                    break;
                                case "optSotRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == sot.Id)) break;

                                        await member.GrantRoleAsync(sot);
                                        gRoles.Add(sot);
                                    }
                                    break;
                                case "optGtaRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == gta.Id)) break;

                                        await member.GrantRoleAsync(gta);
                                        gRoles.Add(gta);
                                    }
                                    break;
                            }
                        }

                        var listGameRoles = "```Вы получили следующие роли:```\n" + string.Join(" ", gRoles.Select(g => $"{g.Mention}\n"));
                        if (gRoles.Count == 0)
                            listGameRoles = "```Выбранные вами роли у вас уже есть!```";

                        var interactionBuilder = new DiscordInteractionResponseBuilder()
                                           .AddEmbed(new DiscordEmbedBuilder()
                                               .WithDescription($"{listGameRoles}")
                                               .WithColor(new DiscordColor(43, 45, 49))
                                               .Build())
                                           .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, interactionBuilder);
                    }
                    break;

                case "ddlHobbyRoles":
                    {
                        var hRoles = new List<DiscordRole>();

                        foreach (var value in _e.Values)
                        {
                            switch (value)
                            {
                                case "optArtistRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == artist.Id)) break;

                                        await member.GrantRoleAsync(artist);
                                        hRoles.Add(artist);
                                    }
                                    break;
                                case "optMusicanRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == musican.Id)) break;

                                        await member.GrantRoleAsync(musican);
                                        hRoles.Add(musican);
                                    }
                                    break;
                                case "optBlogerRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == bloger.Id)) break;

                                        await member.GrantRoleAsync(bloger);
                                        hRoles.Add(bloger);
                                    }
                                    break;
                                case "optGamerRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == gamer.Id)) break;

                                        await member.GrantRoleAsync(gamer);
                                        hRoles.Add(gamer);
                                    }
                                    break;
                                case "optCoderRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == coder.Id)) break;

                                        await member.GrantRoleAsync(coder);
                                        hRoles.Add(coder);
                                    }
                                    break;
                                case "optMontageRole":
                                    {
                                        if (member.Roles.Any(r => r.Id == montage.Id)) break;

                                        await member.GrantRoleAsync(montage);
                                        hRoles.Add(montage);
                                    }
                                    break;
                            }
                        }

                        var listHobbyRoles = "```Вы получили следующие роли:```\n" + string.Join(" ", hRoles.Select(g => $"{g.Mention}\n"));
                        if (hRoles.Count == 0)
                            listHobbyRoles = "```Выбранные вами роли у вас уже есть!```";

                        var interactionBuilder = new DiscordInteractionResponseBuilder()
                                           .AddEmbed(new DiscordEmbedBuilder()
                                               .WithDescription($"{listHobbyRoles}")
                                               .WithColor(new DiscordColor(43, 45, 49))
                                               .Build())
                                           .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, interactionBuilder);
                    }
                    break;

            }
        }

        [Obsolete]
        public async Task DropDownUserEventHandler()
        {
            var member = (DiscordMember)_e!.User;
            switch (_e.Id)
            {
                case "ddlAllMembers":
                    {
                        var personalChannel = _e.Guild.GetChannel(_listPrivateChannels[member.Id]);

                        ulong.TryParse(_e.Values.FirstOrDefault(), out ulong id);
                        var selectedUser = _e.Guild.GetMemberAsync(id).Result;

                        if (selectedUser == member)
                        {
                            var response = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription("Вы не можете кикнуть самого себя!")
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                            await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                            break;
                        }

                        var description = "";

                        if (personalChannel.Users.Any(u => u.Id == id))
                        {
                            await selectedUser.ModifyAsync(u => u.VoiceChannel = null);
                            description = $"Участник {selectedUser.Mention} был выгнан!";
                        }
                        else
                        {
                            description = $"Этого участника нет в вашей голосовой комнате!";
                        }

                        var interaction = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription(description)
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interaction);
                    }
                    break;

                case "ddlGrantUserRightToLeader":
                    {
                        var personalVC = _e.Guild.GetChannel(_listPrivateChannels[member.Id]);

                        ulong.TryParse(_e.Values.FirstOrDefault(), out ulong id);
                        var selectedUser = _e.Guild.GetMemberAsync(id).Result;

                        if (selectedUser == member)
                        {
                            var response = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription("Вы и так лидер этой комнаты!")
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                            await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                            break;
                        }

                        var description = "";
                        if (personalVC.Users.Any(u => u.Id == id))
                        {
                            _listPrivateChannels.Add(selectedUser.Id, personalVC.Id);
                            _listPrivateChannels.Remove(member.Id);

                            description = $"Вы передали право лидера участнику: {selectedUser.Mention}";
                        }
                        else
                        {
                            description = $"Этого участника нет в вашей голосовой комнате!";
                        }

                        var interaction = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription(description)
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interaction);
                    }
                    break;

                case "ddlGrantUserRightToSpeak":
                    {
                        var personalVC = _e.Guild.GetChannel(_listPrivateChannels[member.Id]);

                        ulong.TryParse(_e.Values.FirstOrDefault(), out ulong id);
                        var selectedUser = _e.Guild.GetMemberAsync(id).Result;

                        if (selectedUser == member)
                        {
                            var response = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription("Вы не можете запретить себе, так как вы лидер")
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                            await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                            break;
                        }

                        await personalVC.ModifyAsync(c => c.PermissionOverwrites = new List<DiscordOverwriteBuilder>()
                        {
                             new DiscordOverwriteBuilder()
                             .Deny(Permissions.Speak)
                             .For(selectedUser)
                        });

                        var interaction = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription($"Вы запретили участнику: {selectedUser.Mention} право говорить!")
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interaction);
                    }
                    break;

                case "ddlAccessUser":
                    {
                        var personalVC = _e.Guild.GetChannel(_listPrivateChannels[member.Id]);

                        ulong.TryParse(_e.Values.FirstOrDefault(), out ulong id);
                        var selectedUser = _e.Guild.GetMemberAsync(id).Result;

                        if (selectedUser == member)
                        {
                            var response = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription("Вы не можете запретить себе, так как вы лидер")
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                            await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                            break;
                        }

                        await personalVC.ModifyAsync(c => c.PermissionOverwrites = new List<DiscordOverwriteBuilder>()
                        {
                             new DiscordOverwriteBuilder()
                             .Deny(Permissions.AccessChannels | Permissions.UseVoice)
                             .For(selectedUser)
                        });

                        var interaction = new DiscordInteractionResponseBuilder()
                                .AddEmbed(new DiscordEmbedBuilder()
                                    .WithDescription($"Вы запретили участнику: {selectedUser.Mention} видеть команату и подключаться к ней!")
                                    .WithColor(new DiscordColor(42, 45, 59)))
                                .AsEphemeral(true);

                        await _e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, interaction);
                    }
                    break;
            }
        }
    }
}

