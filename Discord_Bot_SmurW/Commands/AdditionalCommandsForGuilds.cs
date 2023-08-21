using DSharpPlus;
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
    [Category("Build")]
    public class AdditionalCommandsForGuilds : BaseCommandModule
    {
        #region Встроенное сообщение по Правилам сервера
        [Hidden]
        [Command("Rules")]
        [Description("This is rules embed")]
        public async Task RulesCommandAsync(CommandContext ctx, long channelId)
        {
            var generalPrevisionsDesc =
                "🔸 Участники сервера Discord равны перед правилами вне зависимости от опыта и роли.\n" +
                "🔸 Мат разрешается, но без злоупотребления.\n" +
                "🔸 Запрещено оскорбление других пользователей.\n" +
                "🔸 Нельзя использовать NSFW: шок-контент, порнографию.\n" +
                "🔸 Запрещено злоупотребление Caps Lock.\n" +
                "🔸 Запрещены все типы флуда.\n" +
                "🔸 Запрещается жесткий троллинг.";

            var linkPlacementDesc =
                "🔸 Запрещается реклама без согласования с администратором.\n" +
                "🔸 Не допускается спам-рассылка в личных СМС с другими пользователями.\n" +
                "🔸 Нельзя кидать ссылки с доменами на Ютуб, ВК, Роблокс и Вики. Размещение ссылки по согласованию с администратором.";

            var voiceChatDesc =
                "🔸 Нельзя включать музыку в микрофон.\n" +
                "🔸 Не допускается издание громких звуков в микрофон.\n" +
                "🔸 При наличии шума вокруг рекомендуется применение функции Push-To-Talk.";

            var nicksAndAvatarsDesc =
                "🔸 Администратор вправе требовать изменение ника и картинки, если считает, что они оскорбляют кого-либо.\n" +
                "🔸 Запрещены ники типа User, Discord User, NickName и прочие, в том числе Admin, Moderator и т. д.\n" +
                "🔸 Запрещено использование имен с матом, оскорблением, религиозными названиями, рекламой, пропагандой алкоголя / наркотиков.\n" +
                "🔸 Не допускается применение символики террористов и запрещенных организации, призыв к насилию и экстремизму.\n" +
                "🔸 Нельзя использовать бессмысленный набор символов с многократным повторением одной или нескольких букв.\n" +
                "🔸 Не допускаются картинки с ненормативной лексикой, оскорблением и прочими запрещенными вещами, о которых упоминалось выше.";

            var rulesDiscordChannelsDesc =
                "🔸 На название канала распространяются те же требования, что и для сервера Discord.\n" +
                "🔸 В любом канале / подканале запрещена публикация ссылок на донат-сайты, площадки приема платежей, спонсорской помощи, пожертвований и других сервисов.";

            var responsibilityDesc =
                "🔸 При нарушении правил сервера Discord принимаются меры к пользователям вплоть до ограничения доступа.\n" +
                "🔸 Обход бана путем входа под другим идентификатором или иными путями — `бан`.\n" +
                "🔸 Администратор ДС вправе отказать в доступе любому участнику. Он не обязан указывать причины или предупреждать об этом.\n" +
                "🔸 Нарушение упомянутых выше норм — `бан`.\n" +
                "🔸 Неуважительное отношение к другим пользователям и оскорбление — `бан`.\n" +
                "🔸 Разжигание межнациональной розни, конфликтов на политической и религиозном основании — `бан`.\n" +
                "🔸 Трансляция стримов — `бан`.";

            var rulesMessageBuilder = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Общие правила дискорда")
                    .WithDescription("Обязательно ознакомтесь со всеми пунктами, так как не знание правил не избовляет вас от отвественности")
                    .WithColor(new DiscordColor("FF2B00")));

            var generalPrevisions = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Общие положения")
                    .WithDescription(generalPrevisionsDesc)
                    .WithColor(new DiscordColor("FF6600")));

            var linkPlacement = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Размещение ссылок")
                    .WithDescription(linkPlacementDesc)
                    .WithColor(new DiscordColor("FF6600")));

            var voiceChat = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Голосовой чат")
                    .WithDescription(voiceChatDesc)
                    .WithColor(new DiscordColor("FF6600")));

            var nicksAndAvatars = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Ники и Аватары")
                    .WithDescription(nicksAndAvatarsDesc)
                    .WithColor(new DiscordColor("FF6600")));

            var rulesDiscordChannels = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Правила Discord по приминению каналов и подканалов")
                    .WithDescription(rulesDiscordChannelsDesc)
                    .WithColor(new DiscordColor("FF6600")));

            var responsibility = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Отвественность")
                    .WithDescription(responsibilityDesc)
                    .WithColor(new DiscordColor("FF6600")));

            var channel = ctx.Guild.GetChannel((ulong)channelId);

            await channel.SendMessageAsync(rulesMessageBuilder);
            await channel.SendMessageAsync(generalPrevisions);
            await channel.SendMessageAsync(linkPlacement);
            await channel.SendMessageAsync(voiceChat);
            await channel.SendMessageAsync(nicksAndAvatars);
            await channel.SendMessageAsync(rulesDiscordChannels);
            await channel.SendMessageAsync(responsibility);
        }
        #endregion

        #region Навигация сервера
        [Hidden]
        [RequireOwner]
        [Command("nav")]
        [Description("Навигация сервера")]
        public async Task NavigationServerAsync(CommandContext ctx)
        {
            List<DiscordSelectComponentOption> optionList = new List<DiscordSelectComponentOption>();
            optionList.Add(new DiscordSelectComponentOption("📕⠂Правила сервера", "optRules"));
            optionList.Add(new DiscordSelectComponentOption("🎭⠂Роли сервера", "optRoles"));
            optionList.Add(new DiscordSelectComponentOption("💠⠂Команды сервера", "optCommand"));
            optionList.Add(new DiscordSelectComponentOption("🏠⠂Каналы сервера", "optChannel"));

            var option = optionList.AsEnumerable();
            var dropDown = new DiscordSelectComponent("ddlNavigation", "Выберите вариант...", option);
            var urlNavigation = "https://cdn.discordapp.com/attachments/1109668664493482084/1109681630697758720/image.png";
            var msgNavigationBuilder = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithImageUrl(urlNavigation)
                    .WithColor(new DiscordColor(43, 45, 49)))
                .AddComponents(dropDown);

            await ctx.Channel.SendMessageAsync(msgNavigationBuilder);
        }
        #endregion

        #region Встроенное сообщение для сервера Relax для получения ролей
        [Hidden]
        [RequireOwner]
        [Command("getroles")]
        [Description("Получение ролей")]
        public async Task GetRolesAsync(CommandContext ctx)
        {
            var option = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption("🎮⠂Игровые роли", "optSelectedGameR"),
                new DiscordSelectComponentOption("🧶⠂Увлечения", "optSelectedHobbyR"),
            }.AsEnumerable();

            var dropDown = new DiscordSelectComponent("ddlGetRoles", "Выберите вариант...", option);
            var urlNavigation = "https://cdn.discordapp.com/attachments/1109668664493482084/1111560103963938877/getRole1.png";
            var msgNavigationBuilder = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithImageUrl(urlNavigation)
                    .WithColor(new DiscordColor(43, 45, 49)))
                .AddComponents(dropDown);

            await ctx.Channel.SendMessageAsync(msgNavigationBuilder);

        }
        #endregion

        #region test
        [Hidden]
        [RequireOwner]
        [Command("test")]
        [Description("Это тестовая команда")]
        public async Task TestCommandAsync(CommandContext ctx)
        {
            var emojis = new DiscordEmoji[]
            {
                DiscordEmoji.FromName(ctx.Client, ":changed_name:"),
                DiscordEmoji.FromName(ctx.Client, ":open_close_Room:"),
                DiscordEmoji.FromName(ctx.Client, ":hide:"),
                DiscordEmoji.FromName(ctx.Client, ":show:"),
                DiscordEmoji.FromName(ctx.Client, ":rightToSpeak:"),
                DiscordEmoji.FromName(ctx.Client, ":limitMember:"),
                DiscordEmoji.FromName(ctx.Client, ":access:"),
                DiscordEmoji.FromName(ctx.Client, ":newCreator:"),
                DiscordEmoji.FromName(ctx.Client, ":kickMember:"),
            };

            var componentsRow1 = new List<DiscordButtonComponent>()
            {
                new DiscordButtonComponent(ButtonStyle.Secondary, "btnCreator", "", false, new DiscordComponentEmoji(emojis[7])),
                new DiscordButtonComponent(ButtonStyle.Secondary, "btnAccess", "", false, new DiscordComponentEmoji(emojis[6])),
                new DiscordButtonComponent(ButtonStyle.Secondary, "btnLimitMembers", "", false, new DiscordComponentEmoji(emojis[5])),
                new DiscordButtonComponent(ButtonStyle.Secondary, "btnOpenCloseRoom", "", false, new DiscordComponentEmoji(emojis[1])),
            };
            var componentsRow2 = new List<DiscordButtonComponent>()
            {
                new DiscordButtonComponent(ButtonStyle.Secondary, "btnChangedNameRoom", "", false, new DiscordComponentEmoji(emojis[0])),
                new DiscordButtonComponent(ButtonStyle.Secondary, "btnShowHideRoom", "", false, new DiscordComponentEmoji(emojis[3])),
                new DiscordButtonComponent(ButtonStyle.Secondary, "btnKickMember", "", false, new DiscordComponentEmoji(emojis[8])),
                new DiscordButtonComponent(ButtonStyle.Secondary, "btnRightToSpeak", "", false, new DiscordComponentEmoji(emojis[4])),
            };


            var msgBuilder = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Управление приватным каналом")
                    .WithDescription($"{emojis[7]} ⠂ Назначить лидера комнаты\n" +
                                     $"\n{emojis[6]} ⠂ Установка доступа к комнате\n" +
                                     $"\n{emojis[5]} ⠂ Задать лимит участников\n" +
                                     $"\n{emojis[1]} ⠂ Закрыть/Открыть комнату\n" +
                                     $"\n{emojis[0]} ⠂ Изменить название комнаты\n" +
                                     $"\n{emojis[3]} ⠂ Показать/Спрятать комнату\n" +
                                     $"\n{emojis[8]} ⠂ Выгнать участника из комнаты\n" +
                                     $"\n{emojis[4]} ⠂ Выдать/Забрать право говорить")
                    .WithColor(new DiscordColor(43, 45, 49)))
                .AddComponents(componentsRow1)
                .AddComponents(componentsRow2);
            //43, 45, 49 | 103, 230, 167
            await ctx.Channel.SendMessageAsync(msgBuilder);

        }
        #endregion
    }
}
