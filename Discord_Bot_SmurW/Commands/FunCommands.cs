using Discord_Bot_SmurW.CustomAttributes;
using Discord_Bot_SmurW.Engine.LevelSystem;
using Discord_Bot_SmurW.Engine.PersonalityMessages;
using Discord_Bot_SmurW.HelpFormatter;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using System.Diagnostics.Metrics;
using System.Drawing;
using static System.Net.WebRequestMethods;

namespace Discord_Bot_SmurW.Commands
{
    [Category("Фан")]
    public class FunCommands : BaseCommandModule
    {
        #region Поиск картинок
        [Command("картинки")]
        [Description("Выполняет поиск изображений в Google по заданному запросу")]
        public async Task SearchImages(CommandContext ctx, [RemainingText] string query)
        {
            Program.ImageHandler?.images.Clear();
            int IDCount = 0;

            // Replace with your own Custom Search Engine ID and API Key

            string cseId = "714ebb51e77394de8";
            string apiKey = "AIzaSyAaIkRQTQDlt7fqXRLzAN88vgjsL8_hhiE";

            //Initialise the API

            var customSearchService = new CustomsearchService(new BaseClientService.Initializer
            {
                ApplicationName = "DiscordBot",
                ApiKey = apiKey,
            });

            //Create your search request

            var listRequest = customSearchService.Cse.List();
            listRequest.Cx = cseId;
            listRequest.Num = 10;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Q = query;

            //Execute the search request & get the results

            var search = await listRequest.ExecuteAsync();
            var results = search.Items;

            foreach (var result in results)
            {
                Program.ImageHandler?.images.Add(IDCount, result.Link);
                IDCount++;
            }

            if (results == null || !results.Any())
            {
                await ctx.RespondAsync("No results found.");
                return;
            }
            else
            {
                //Create the buttons for this embed
                var previousEmoji = new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":track_previous:"));
                var previousButton = new DiscordButtonComponent(ButtonStyle.Primary, "btnPreviousButton", "Previous", false, previousEmoji);

                var nextEmoji = new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":track_next:"));
                var nextButton = new DiscordButtonComponent(ButtonStyle.Primary, "btnNextButton", "Next", false, nextEmoji);

                //Display the First Result
                var firstResult = results.First();
                var imageMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Azure)
                    .WithTitle("Results for: " + query)
                    .WithImageUrl(firstResult.Link)
                    )
                    .AddComponents(previousButton, nextButton);

                await ctx.Channel.SendMessageAsync(imageMessage);
            }
        }
        #endregion

        private async Task FunnyActions(CommandContext ctx, DiscordMember member, string[] array, string action)
        {
            var authorMessage = ctx.Message;
            await ctx.Channel.DeleteMessageAsync(authorMessage);

            var random = new Random();

            string urlGif = array[random.Next(array.Length)];

            var msgBuilder = new DiscordEmbedBuilder()
            {
                Description = $"{ctx.User.Mention} {action} {member.Mention}",
                ImageUrl = urlGif,
                Color = new DiscordColor("#7ee79c")
            };
                
            var message = new DiscordMessageBuilder().WithEmbed(msgBuilder);
            await ctx.Channel.SendMessageAsync(message);
           
        }

        #region Поцеловать
        [Command("поцеловать")]
        [Description("Поцеловать участника")]
        public async Task KissAsync(CommandContext ctx, DiscordMember member)
        {
            var arrayUrlGif = new[]
            {
                "https://media.tenor.com/1O253ddHmWUAAAAC/kiss-hugekiss.gif",
                "https://media.tenor.com/z-eg2uhf-b0AAAAC/peach-cat-kiss.gif",
                "https://media.tenor.com/uKFCBnAq02MAAAAd/love.gif",
                "https://media.tenor.com/QjMZ6Dx33_QAAAAC/kuss-kussi.gif",
                "https://media.tenor.com/IM4-RWRjSNgAAAAC/kiss.gif"
            };

            await FunnyActions(ctx, member, arrayUrlGif, "поцеловал(а)");
        }
        #endregion

        #region Шлепнуть
        [Command("шлепнуть")]
        [Description("Шлепнуть участника")]
        public async Task SlapAsync(CommandContext ctx, DiscordMember member)
        {
            var arrayUrlGif = new[]
            {
                "https://media.tenor.com/OT2E_JAFvecAAAAi/slap-cat.gif",
                "https://media.tenor.com/fZQYHVUDfckAAAAi/slap.gif",
                "https://media.tenor.com/hjsIlCehQKAAAAAd/nicole-tompkins.gif",
                "https://media.tenor.com/qHRnv-AOmFQAAAAd/slap.gif",
                "https://media.tenor.com/xbeExxbKPDQAAAAC/assslap-slap.gif",
                "https://media.tenor.com/MpmdnEoUxFQAAAAC/slap-cream.gif"
            };

            await FunnyActions(ctx, member, arrayUrlGif, "шлепнул(а)");
        }
        #endregion

        #region Пнуть
        [Command("пнуть")]
        [Description("Пнуть участника")]
        public async Task KickAsync(CommandContext ctx, DiscordMember member)
        {
            var arrayUrlGif = new[]
            {
                "https://media.tenor.com/5iVv64OjO28AAAAC/milk-and-mocha-bear-couple.gif",
                "https://media.tenor.com/5JmSgyYNVO0AAAAC/asdf-movie.gif",
                "https://media.tenor.com/qE3sYVzhFH8AAAAC/kick.gif",
                "https://media.tenor.com/QU8awp3W34AAAAAC/chick-cartoon.gif",
                "https://media.tenor.com/PqTmhDmL_QsAAAAd/lycoris-recoil-kick.gif",
                "https://media.tenor.com/DTDsGIi6Y9EAAAAd/wildfireuv.gif"
            };

            await FunnyActions(ctx, member, arrayUrlGif, "пнул(а)");
        }
        #endregion

        #region Обнять
        [Command("обнять")]
        [Description("Обнять участника")]
        public async Task HuggleAsync(CommandContext ctx, DiscordMember member)
        {
            var arrayUrlGif = new[]
            {
                "https://media.tenor.com/kRXsnDqxCYgAAAAC/cuddle.gif",
                "https://media.tenor.com/rONLuXsZr2kAAAAM/cuddle-couple.gif",
                "https://media.tenor.com/-evl3vUtVhEAAAAC/hugs-cuddle.gif",
                "https://media.tenor.com/fVzK5G68cYcAAAAC/hugs-cuddle.gif",
                "https://media.tenor.com/sSbr1al2-KQAAAAC/so-cute.gif",
                "https://media.tenor.com/vhxp8VLoMOwAAAAC/hug-love.gif"
            };

            await FunnyActions(ctx, member, arrayUrlGif, "обнял(а)");
        }
        #endregion

        #region Послать
        [Command("послать")]
        [Description("Послать участника")]
        public async Task SendToAsync(CommandContext ctx, DiscordMember member)
        {
            var arrayUrlGif = new[]
            {
                "https://media.tenor.com/Az1cQtXDVV8AAAAC/dont-be-thick-stupid.gif",
                "https://media.tenor.com/xan3i8wR8CgAAAAC/sleeping-beauty-disney.gif",
                "https://media.tenor.com/DKl23WXY5YQAAAAC/fuck-you-go-fuck-yourself.gif"
            };

            await FunnyActions(ctx, member, arrayUrlGif, "послал(а)");
        }
        #endregion

        #region Приветствие
        [Command("привет")]
        [Description("Приветствовать участника")]
        public async Task HelloAsync(CommandContext ctx, DiscordMember member)
        {
            var arrayUrlGif = new[]
            {
                "https://media.tenor.com/QpTLQALtdskAAAAi/hii-wave.gif",
                "https://media.tenor.com/FvJr-o-mRgEAAAAC/hello-wave.gif",
                "https://media.tenor.com/2yZ8j2p1YEAAAAAC/hello-wave.gif",
                "https://media.tenor.com/mhLPO2VldCkAAAAC/0001.gif",
                "https://media.tenor.com/YYPY5tPFIE8AAAAd/bad-teeth.gif",
                "https://media.tenor.com/lUFliafCu_MAAAAC/hello.gif",
                "https://media.tenor.com/jq-JqzSvVloAAAAC/hi.gif"
            };

            await FunnyActions(ctx, member, arrayUrlGif, "поздаровался(ась) со");
        }
        #endregion

        #region Дать жестких пиздюлей
        [Command("джп")]
        [Description("Дать жестких пиздюлей")]
        public async Task TakeHitAsync(CommandContext ctx, DiscordMember member)
        {
            var arrayUrlGif = new[]
            {
                "https://media.tenor.com/ExjT2gJPQG8AAAAC/драка-shovel-to-the-face.gif",
                "https://media.tenor.com/-dK24mwTyKwAAAAC/tv-shows-supernatural.gif",
                "https://media.tenor.com/6Cp5tiRwh-YAAAAC/meme-memes.gif",
                "https://media.tenor.com/-wfr09tbkwcAAAAC/discord-anime.gif",
            };

            if (ctx.Member?.Id == 512801032317960193 || ctx.Member?.Id == 377152579496574976)
                arrayUrlGif = new[]
                {
                    "https://media.tenor.com/QLm70LMUupEAAAAi/叩く-怒る.gif",
                    "https://media.tenor.com/1vQ_vh6rlgAAAAAC/no.gif",
                    "https://media.tenor.com/2yJBnYOY_j8AAAAC/tonton-tonton-sticker.gif",
                    "https://media.tenor.com/2phctoJVTscAAAAC/fail-giveup.gif",
                };

            await FunnyActions(ctx, member, arrayUrlGif, "Дал(а) Жёсткой Пизды");
        }
        #endregion
    }
}
