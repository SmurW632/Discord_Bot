using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.PersonalityMessages
{
    public struct TypeMessage
    {
        public static int CountDeleteMessages { get; set; }
        public static DiscordUser? After { get; set; }
    }
}
