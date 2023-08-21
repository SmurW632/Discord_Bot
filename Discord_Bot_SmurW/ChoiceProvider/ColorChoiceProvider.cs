using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.ChoiceProvider
{
    internal class ColorChoiceProvider : IChoiceProvider
    {
       public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
        {
            IEnumerable<DiscordApplicationCommandOptionChoice> choices = new DiscordApplicationCommandOptionChoice[]
            {
                new DiscordApplicationCommandOptionChoice("Красный", "Red"),
                new DiscordApplicationCommandOptionChoice("Ораньжевый", "Orange"),
                new DiscordApplicationCommandOptionChoice("Желтый", "Yellow"),
                new DiscordApplicationCommandOptionChoice("Зеленый", "Green"),
                new DiscordApplicationCommandOptionChoice("Голубой", "LightBlue"),
                new DiscordApplicationCommandOptionChoice("Синий", "Blue"),
                new DiscordApplicationCommandOptionChoice("Фиолетовый", "Purple"),
                new DiscordApplicationCommandOptionChoice("Черный", "Black")
            };

            return Task.FromResult(choices);
        }
    }
}
