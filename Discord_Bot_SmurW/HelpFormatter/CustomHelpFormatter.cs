using Discord_Bot_SmurW.CustomAttributes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.HelpFormatter
{
    public class CustomHelpFormatter : BaseHelpFormatter
    {
        protected DiscordEmbedBuilder _embed;
        protected StringBuilder _strBilder;

        public CustomHelpFormatter(CommandContext ctx) : base(ctx)
        {
            _embed = new DiscordEmbedBuilder();
            _strBilder = new StringBuilder();
        }

        public override CommandHelpMessage Build()
        {
           // return new CommandHelpMessage(embed: _embed);
           return new CommandHelpMessage(_strBilder.ToString());
        }

        public override BaseHelpFormatter WithCommand(Command cmd)
        {
            _embed
                .WithTitle($"Справка по команде {cmd.Name}")
                .WithDescription(cmd.Description)
                .WithColor(DiscordColor.Azure)
                .WithFooter("Relax © 2023 Все права почти защищены");

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            foreach (var cmd in subcommands)
            {
                _strBilder.AppendLine($"\r\n\u001b[2;36m\u001b[0m\u001b[2;36m!{cmd.Name}\u001b[0m\r\n╰ {cmd.Description}");
            }

            return this;
        }

    }
}
