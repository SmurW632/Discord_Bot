using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

namespace Discord_Bot_SmurW.Commands
{
    [Category("Музыка")]
    public class MusicCommands : BaseCommandModule
    {
        #region муз. команда "играть"
        [Command("играть")]
        [Description("Бот подключается к голосовому каналу в котором сидит" +
            " вызвавший команду участник и воспроизводит запрошенный трек\n " +
            "Пример использования: !играть [название трека]")]
        public async Task PlayMusicAsync(CommandContext ctx, [Description("Запрос для поиска музыки")]
                                                   [RemainingText] string search)
        {
            var VoiceChannel = ctx.Member?.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();
            
            if (ctx.Member?.VoiceState == null || VoiceChannel == null)
            {
                await ctx.RespondAsync("Пожалуйста зайдите в голосовой канал!");
                return;
            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("Lavalink не подключен.");
                return;
            }

            if (VoiceChannel.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("Пожалуйста зайдите в голосовой канал!");
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();

            await node.ConnectAsync(VoiceChannel);
            
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            
            if(conn == null)
            {
                await ctx.Channel.SendMessageAsync("Lavalink не смог подключиться к голосовому каналу");
                return;
            }

            if (search == null)
            {
                await ctx.RespondAsync("В зпросе ничего нет, введите запрос!");
                return;
            }

            var loadResult = await node.Rest.GetTracksAsync(search);
            if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await ctx.RespondAsync($"Не удалось выполнить поиск трека для {search}.");
                return;
            }
            
            var track = loadResult.Tracks.First();
            
            await conn.PlayAsync(track);
            var musicDescription = $"**Сейчас играет:** `{track.Title}`\n" +
                                   $"**Автор:** `{track.Author}`\n" +
                                   $"**Длительность:** `{track.Length}`\n" +
                                   $"**Ссылка:** `{track.Uri}`";

            var msgEmbedPlaying = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor("#7ee79c"),
                Title = $"Подключен к каналу `{VoiceChannel.Name}` и воспроизводит музыку",
                Description = musicDescription,
            };

            await ctx.Channel.SendMessageAsync(embed: msgEmbedPlaying);
        }
        #endregion

        [Command("пауза")]
        [Description("Ставит трек на паузу")]
        public async Task Pause(CommandContext ctx)
        {
            var VoiceChannel = ctx.Member?.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member?.VoiceState == null || VoiceChannel == null)
            {
                await ctx.RespondAsync("Пожалуйста зайдите в голосовой канал!");
                return;
            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("Lavalink не подключен.");
                return;
            }

            if (VoiceChannel.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("Пожалуйста зайдите в голосовой канал!");
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null)
            {
                await ctx.Channel.SendMessageAsync("Lavalink не смог подключиться к голосовому каналу");
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await ctx.RespondAsync("Трек не загружен!!!");
                return;
            }

            await conn.PauseAsync();


            var msgEmbedPlaying = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Orange,
                Title = $"Трек `{conn.CurrentState.CurrentTrack.Title} {conn.CurrentState.CurrentTrack.Author}` был поставлен на паузу!",
                Description = $"Текущая позиция: `{conn.CurrentState.PlaybackPosition.ToString(format: "hh\\:mm\\:ss")}`"
            };

            await ctx.Channel.SendMessageAsync(embed: msgEmbedPlaying);
        }

        [Command("стоп")]
        [Description("Останавливает трек и отключает бота с канала")]
        public async Task Stop(CommandContext ctx)
        {
            var VoiceChannel = ctx.Member?.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member?.VoiceState == null || VoiceChannel == null)
            {
                await ctx.RespondAsync("Пожалуйста зайдите в голосовой канал!");
                return;
            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("Lavalink не подключен.");
                return;
            }

            if (VoiceChannel.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("Пожалуйста зайдите в голосовой канал!");
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null)
            {
                await ctx.Channel.SendMessageAsync("Lavalink не смог подключиться к голосовому каналу");
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await ctx.RespondAsync("Трек не загружен!!!");
                return;
            }

            await conn.StopAsync();
            await conn.DisconnectAsync();


            var msgEmbedPlaying = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.DarkRed,
                Title = $"Трек `{conn.CurrentState.CurrentTrack.Title} {conn.CurrentState.CurrentTrack.Author}` был остановлен!",
                Description = "Успешно был отключен с голосового канала!!!"
            };

            await ctx.Channel.SendMessageAsync(embed: msgEmbedPlaying);
            
        }

        [Command("прод")]
        [Description("Продолжает воспроизводить трек")]
        public async Task Resume(CommandContext ctx)
        {
            var VoiceChannel = ctx.Member?.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member?.VoiceState == null || VoiceChannel == null)
            {
                await ctx.RespondAsync("Пожалуйста зайдите в голосовой канал!");
                return;
            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("Lavalink не подключен.");
                return;
            }

            if (VoiceChannel.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("Пожалуйста зайдите в голосовой канал!");
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null)
            {
                await ctx.Channel.SendMessageAsync("Lavalink не смог подключиться к голосовому каналу");
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await ctx.RespondAsync("Трек не загружен!!!");
                return;
            }

            await conn.ResumeAsync();


            var msgEmbedPlaying = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor("#7ee79c"),
                Title = $"Трек `{conn.CurrentState.CurrentTrack.Title} {conn.CurrentState.CurrentTrack.Author}` продолжает воспроизводиться!!!",
                Description = $"С момента: `{conn.CurrentState.PlaybackPosition.ToString(format: "hh\\:mm\\:ss")}`"
            };

            await ctx.Channel.SendMessageAsync(embed: msgEmbedPlaying);
        }
    }
}
