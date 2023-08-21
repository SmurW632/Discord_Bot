using Discord_Bot_SmurW.Commands;
using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Tests
{
    [TestClass]
    public class FunCommands_TestCommands
    {
        [TestMethod]
        public async Task FunCommands_TestCommand_ReturnMessageThatWasSentToTheUserAndToTheSharedServer(CommandContext ctx)
        {

            ulong userID = ctx.Member.Id;
            ulong idChannelUser = ctx.Channel.Id;
            ulong idTotalGuild = ctx.Guild.Id;

            string expectedMessageSendToChannelUser = "Список команд!";
            string expectedMessageSendToTotalGuild = "@Участинк, я тебе отправил список команд";
            FunCommands funCommands = new FunCommands();

            await funCommands.TestCommand(ctx);

            

        }
    }
}
