using Discord_Bot_SmurW.ChoiceProvider;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordChannelEngine;
using Discord_Bot_SmurW.Engine.Models;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using System.Drawing;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Xml;

namespace Discord_Bot_SmurW.SlashCommands
{
    public class TestCommands : ApplicationCommandModule
    {
        #region тест
        [SlashCommand("тест", "тестовая команда")]
        public async Task TestCmd(InteractionContext ctx)
        {
            await Task.CompletedTask;
        }
        #endregion


        #region поздравить
        [SlashCommand("поздравить", "поздравление участинка")]
        public async Task CongratulationMessage(InteractionContext ctx,
                                                                        [Option("Участник", "Выбор пользователя")] DiscordUser user,
                                                                        [Option("Заголовок","Заголовок для поздравления")] string title,
                                                                        [Option("Описание", "Описание поздравления")] string description,
                                                                        [Option("Картинка", "Ссылка на картинку")] DiscordAttachment url)
        {
          
            await new DiscordMessageBuilder()
                .WithContent(user.Mention)
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle(title)
                    .WithDescription(description)
                    .WithImageUrl(url.Url))
                .WithAllowedMention(new UserMention(user))
                .SendAsync(ctx.Channel);

        }
        #endregion

        #region опрос
        [SlashCommand("опрос", "Создайте опрос содержащий 4 варианта отета")]
        public async Task PollAndResultPoll(InteractionContext ctx,
                                                                    [Option("Вопрос","Введите содержимое вопроса")] string question,
                                                                    [Option("Лимит","Введите лимит времени в секундах")] long timeLimit,
                                                                    [Option("первый", "Введите вариант ответа")] string opt1,
                                                                    [Option("второй", "Введите вариант ответа")] string opt2, 
                                                                    [Option("третий", "Введите вариант ответа")] string opt3 = null!,
                                                                    [Option("четвертый", "Введите вариант ответа")] string opt4 = null!)
        {

            var interactivity = ctx.Client.GetInteractivity();
            TimeSpan timer = TimeSpan.FromSeconds(timeLimit);

            //await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
            //                                                                                .WithContent($"Время на опрос после первой реакции дается {timer.Seconds}сек."));
            await ctx.DeferAsync();

            int countItem = 0;
            DiscordEmoji[] optionEmojis = { DiscordEmoji.FromName(ctx.Client, ":one:", false),
                                             DiscordEmoji.FromName(ctx.Client, ":two:", false),
                                             DiscordEmoji.FromName(ctx.Client, ":three:", false),
                                             DiscordEmoji.FromName(ctx.Client, ":four:", false) };

            string optionsString = "";

            if (opt3 == null)
            {
                optionsString = $"1. {opt1}\n2. {opt2}";
                countItem = 2;
            }
            else if (opt3 != null && opt4 == null)
            {
                optionsString = $"1. {opt1}\n2. {opt2}\n3. {opt3}";
                countItem = 3;
            }
            else if(opt4 != null)
            {
                optionsString = $"1. {opt1}\n" +
                                $"2. {opt2}\n" +
                                $"3. {opt3}\n" +
                                $"4. {opt4}";
                countItem = 4;
            }
            
            var poolMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Azure)
                .WithTitle(string.Join(" ", question))
                .WithDescription(optionsString)
               );

            var putReactOn = await ctx.Channel.SendMessageAsync(poolMessage);

            for (int index = 0; index < countItem; index++)
            {
                await putReactOn.CreateReactionAsync(optionEmojis[index]);
            }

            var result = await interactivity.CollectReactionsAsync(putReactOn, timer);

            int[] count = new int[countItem] ;
            string resultString = "";
            
            if(countItem == 2)
            {
                foreach (var emoji in result)
                {
                    if (emoji.Emoji == optionEmojis[0])
                        count[0]++;
                    if (emoji.Emoji == optionEmojis[1])
                        count[1]++;
                }

                int indexMaxVotes = count[0] > count[1] ? 0 : 1;
                int maxVotes = count[indexMaxVotes];
                resultString = $"Больше всего голосов ({maxVotes}) набрал номер {indexMaxVotes + 1}!";
            }
            else if(countItem == 3)
            {
                foreach (var emoji in result)
                {
                    if (emoji.Emoji == optionEmojis[0])
                        count[0]++;
                    if (emoji.Emoji == optionEmojis[1])
                        count[1]++;
                    if (emoji.Emoji == optionEmojis[2])
                        count[2]++;
                }

                int indexMaxVotes = Array.FindLastIndex(count, delegate (int i) { return i == count.Max(); });
                int maxVotes = count[indexMaxVotes];
                resultString = $"Больше всего голосов ({maxVotes}) набрал номер {indexMaxVotes + 1}!";
            }
            else if(countItem == 4)
            {
                foreach (var emoji in result)
                {
                    if (emoji.Emoji == optionEmojis[0])
                        count[0]++;
                    if (emoji.Emoji == optionEmojis[1])
                        count[1]++;
                    if (emoji.Emoji == optionEmojis[2])
                        count[2]++;
                    if (emoji.Emoji == optionEmojis[3])
                        count[3]++;
                }

               int indexMaxVotes = Array.FindLastIndex(count, delegate (int i) { return i == count.Max(); });
               int maxVotes = count[indexMaxVotes];
               resultString = $"Больше всего голосов ({maxVotes}) набрал номер {indexMaxVotes + 1}!";
            }


            var resultMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Green)
                .WithTitle("Результат опроса")
                .WithDescription(resultString)
                );

            await ctx.Channel.SendMessageAsync(resultMessage);
        }
        #endregion

        #region Поиск картинок в Google
        [SlashCommand("картинки","Поиск картинок в Google")]
        public async Task GoogleImageSearch(InteractionContext ctx, [Option("поиск","ваш поисковой запрос")] string search)
        {
            await ctx.DeferAsync();

            string apiKey = "AIzaSyBxRRvKQ52knzeIDuC42gKxFiDHHNPIdXQ";
            string cseId = "d4c38bee290d14d42";

            var customSearchService = new CustomsearchService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "TestSearchSystem"
            });

            var listRequest = customSearchService.Cse.List();
            listRequest.Cx = cseId;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Q = search;

            var searchRequest = await listRequest.ExecuteAsync();
            var results = searchRequest.Items;

            if(results == null || !results.Any())
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Результатов ненашлось!"));
                return;
            }

            var firstResult = results.First();
            var embed = new DiscordEmbedBuilder()
            {
                Title = $"Результаты по писку: {search}",
                ImageUrl = firstResult.Link,
                Color = DiscordColor.Aquamarine
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
        }
        #endregion

        #region Создать войс 
        [SlashCommand("создать_войс", "Создать собственную голосовую комнату")]
        public async Task CreateVoiceChanel(InteractionContext ctx, [Option("Имя_канала", "Введите имя войса")] string chanelName,
                                                                    [Option("количество_чел", "Введите количество человек, если нужно")] string chanelLimit = null!)
        {
            ulong idChannelSmurw = 1094563236952674335;
            ulong idChannelRelax = 1095569256671891487;

            if (ctx.Channel.Id == idChannelSmurw || ctx.Channel.Id == idChannelRelax)
            {
                await ctx.DeferAsync();

                var member = (DiscordMember)ctx.User;
                var channelUserParse = int.TryParse(chanelLimit, out var channelCount);

                if (chanelLimit != null && channelUserParse == true)
                {
                    var createdChanel = await ctx.Guild.CreateVoiceChannelAsync(chanelName, ctx.Channel.Parent, null, channelCount);
                    
                    if(ctx.Member.VoiceState != null)
                    await member.PlaceInAsync(createdChanel);

                    var successMessage = new DiscordEmbedBuilder()
                    {
                        Title = $"Голосовая комната '{chanelName}' была создана в канале {ctx.Channel.Parent.Name}",
                        Description = $"Количество участников ограничено до {channelCount}",
                        Color = DiscordColor.Green,
                    };

                    await ctx.Channel.SendMessageAsync(new DiscordMessageBuilder().AddEmbed(successMessage));
                }
                else if (chanelLimit == null)
                {
                    var createdChanel = await ctx.Guild.CreateVoiceChannelAsync(chanelName, ctx.Channel.Parent, null, channelCount);

                    if (ctx.Member.VoiceState != null)
                        await member.PlaceInAsync(createdChanel);

                    var successMessage = new DiscordEmbedBuilder()
                    {
                        Title = $"Голосовая комната {chanelName} была создана в канале {ctx.Channel.Parent.Name}",
                        Description = "Количество участников неограничено",
                        Color = DiscordColor.Green,
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(successMessage));
                }
                else if (channelUserParse == false)
                {
                    var failedMessage = new DiscordEmbedBuilder()
                    {
                        Title = "Ошибка! Не верно указан параметр 'Количество человек'",
                        Color = DiscordColor.Red,
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(failedMessage));
                }
            }
            else
            {
                await ctx.DeferAsync();

                var isNotChannel = new DiscordEmbedBuilder()
                {
                    Title = "В этом канале команда запрещена, вводите ее в приват канале",
                    Color = DiscordColor.Red,
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(isNotChannel));
            }

        }
        #endregion

        #region Встроенное сообщение
        [SlashCommand("встрсмс","Создайте встроенное сообщение, можно ввести Название, описание и 2 возможных поля")]
        public async Task SetEmbedMessageAsync(InteractionContext ctx, [Option("Название", "Введите название")] string title,
                                                                       [Option("Описание", "Введите описание")] string description,
                                                                       [ChoiceProvider(typeof(ColorChoiceProvider))]
                                                                       [Option("Цвет", "Выберите цвет из списка")] string color,
                                                                       [Option("ПолеИмя", "Введите название поля")] string? titleField = null,
                                                                       [Option("ПолеОписание", "Введите описание поля")] string? descriptionField = null)
        {
            await ctx.DeferAsync();

            var discordColor = new DiscordColor();
            switch (color)
            {
                case "Red":
                    discordColor = DiscordColor.Red;
                    break;
                case "Orange":
                    discordColor = DiscordColor.Orange;
                    break;
                case "Yellow":
                    discordColor = DiscordColor.Yellow;
                    break;
                case "Green":
                    discordColor = DiscordColor.Green;
                    break;
                case "LightBlue":
                    discordColor = DiscordColor.Azure;
                    break;
                case "Blue":
                    discordColor = DiscordColor.Blue;
                    break;
                case "Purple":
                    discordColor = DiscordColor.Purple;
                    break;
                case "Black":
                    discordColor = DiscordColor.Black;
                    break;
            }
            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = title,
                Description = description,
                Color = discordColor
            };

            if (titleField != null && descriptionField != null)
            {
               embedMessage.AddField(titleField, descriptionField);
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        #endregion
    }
}
