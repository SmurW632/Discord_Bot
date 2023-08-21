using Discord_Bot_SmurW.Engine.DiscordGuildEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordChannelEngine;
using Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordRoleEngine;
using Discord_Bot_SmurW.Engine.Models;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.TestData
{
    public class SavingTestData : BaseEngine
    {
        public SavingTestData(DiscordGuild guild) : base(guild) { }
        public List<GuildModel> SetAllGuilds()
        {
            ulong guildRelax = 824195873620623421;
            ulong guildSmurw = 359947892288520194;

            // Guild Relax
           //var channelsRelax = SetAllChannels(guildRelax);
            var usersRelax = SetAllUsers(guildRelax);
            var rolesRelax = SetAllRoles(guildRelax);

            // Guild SmurW
           // var channelsSmurW = SetAllChannels(guildSmurw);
            var usersSmurW = SetAllUsers(guildSmurw);
            var rolesSmurW = SetAllRoles(guildSmurw);

            var listGuilds = new List<GuildModel>()
            {
                new GuildModel()
                {
                    GuildId = 824195873620623421,
                    GuildName = "Relax",
                   // Channels = channelsRelax,
                    Users = usersRelax,
                    Roles = rolesRelax,
                },

                new GuildModel()
                {
                    GuildId = 359947892288520194,
                    GuildName = "SmurW",
                  //  Channels = channelsSmurW,
                    Users = usersSmurW,
                    Roles = rolesSmurW,
                },

            };

            if (listGuilds.Count < 1 || listGuilds == null) return null!;

            return listGuilds;
        }

        public List<UserModel> SetAllUsers(ulong guildId)
        {
            var listUsers = new List<UserModel>()
            {
                new UserModel()
                {
                    UserId = 809063055115157564,
                    UserName = "alexup",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 573,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "",
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 545277950941986816,
                    UserName = "HaraldSemmelLauch",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 444,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/545277950941986816/8f0ef9145847e0c43c6f60c533f279c8.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 377564756636074015,
                    UserName = "PE3HOB",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 506,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/377564756636074015/52c157419694a5ceec002b5425833326.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 377152579496574976,
                    UserName = "! 𝓐𝔃𝓪𝔃𝓮𝓵",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 436,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/377152579496574976/21267497c97d8517b67085f73e9741f0.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 279651247315746817,
                    UserName = "EvGeN",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 476,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/279651247315746817/a_5d918799e76bee70299160a993d4d53a.gif?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 330178891673239555,
                    UserName = "id",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 758,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/330178891673239555/85bae6ed8103a0ff27754aefa804e3f3.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 422471031756947467,
                    UserName = ".Crystal",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 416,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/422471031756947467/ef7e4bf34fbf6f4cf9970044cb9c8d93.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 376269965776650241,
                    UserName = "Gard1an",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 365,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/376269965776650241/a_e9d37a5710856e437d81ea359be5e18c.gif?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 854085572720066620,
                    UserName = "ssx",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 589,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/854085572720066620/a_4af69ce457845e70ff57fa2efb8e3a8b.gif?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 381203097604587524,
                    UserName = "ZERO",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 530,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/381203097604587524/c446d190905748fa07e6b48cdb8f9a77.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 512801032317960193,
                    UserName = "! 𝒪𝒹𝒾𝓃",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 709,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/512801032317960193/b7168af1eed98efbf4d4e1f41e8e852c.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 1036375006382407803,
                    UserName = "Halvalas",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 43,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/1036375006382407803/5d001b662280d2ec4ca2e9a2923d2f44.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 553998773844443136,
                    UserName = "Прокурор",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 518,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/553998773844443136/fa49153e377790114e06feec4b662b1b.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 325254369194934273,
                    UserName = "馬克斯",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 451,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/325254369194934273/a_ee38a88da9935b125447d1d298f54a59.gif?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 376433053607657473,
                    UserName = "19.1.22.5_13.5",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 264,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/376433053607657473/95d65c5207dd4e998a5d752b682673ff.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 359947424975814656,
                    UserName = "𝓢𝓶𝓾𝓻𝓦",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 530,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/359947424975814656/a_64ea9686e033202a88c726d6f872338c.gif?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 748150844507291728,
                    UserName = "𝓢𝓬𝓻𝓮𝓪𝓶",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 475,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/748150844507291728/6d0faa16a4ac260204f1849ad6fdcc47.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 428468076753715201,
                    UserName = "brokenchill",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 520,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/428468076753715201/535a27d49e6dc4cc80aab232b8901bb6.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 487275008834535424,
                    UserName = ".Crystal",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 419,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/487275008834535424/4f4ed12b977abf2f518540ab434b2354.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 366117437898358786,
                    UserName = "!𝕭𝖔𝖔",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 788,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/366117437898358786/36f0fb1d9ef3201ee806ab48a131edcc.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 190552846175961089,
                    UserName = "Zonda",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 311,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/190552846175961089/becde14df93e5dc7987a05481f046c29.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 526407737454624770,
                    UserName = "8bit_t",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 573,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/embed/avatars/0.png?size=1024",
                    
                    GuildModelId = guildId,
                },

            };

            if (guildId == 359947892288520194)
                listUsers = new List<UserModel>()
                {
                   new UserModel()
                {
                    UserId = 1108053946905411634,
                    UserName = "SmurW",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 5,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/1108053946905411634/79ee349b6511e2000af8a32fb8a6974e.png?size=1024",
                    
                    GuildModelId = guildId,
                },

                new UserModel()
                {
                    UserId = 359947424975814656,
                    UserName = "𝓢𝓶𝓾𝓻𝓦",
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    CountMessage= 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = 2070,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = "https://cdn.discordapp.com/avatars/359947424975814656/a_64ea9686e033202a88c726d6f872338c.gif?size=1024",
                    
                    GuildModelId = guildId,
                },
                };

            return listUsers;
        }

        public List<RoleModel> SetAllRoles(ulong guildId)
        {
            var listRolesServerRelax = new List<RoleModel>()
            {
                new RoleModel()
                {

                    RoleId = 824201239613014133,
                    Name = "Relax",
                    RoleTypeName = RoleTypeEnum.Senior.ToString().ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1102901707698098258,
                    Name = "🕹️Топ разраб 🖲",
                    RoleTypeName = RoleTypeEnum.Individual.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1095248603854950420,
                    Name = "smurw",
                    RoleTypeName = RoleTypeEnum.Bot.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1108771356155326524,
                    Name = "Relative",
                    RoleTypeName = RoleTypeEnum.Individual.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1093788682332405901,
                    Name = "🛠️модератор🛠",
                    RoleTypeName = RoleTypeEnum.Senior.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 926608746589876274,
                    Name = "Семья❤️",
                    RoleTypeName = RoleTypeEnum.Individual.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 852788848495820802,
                    Name = "Relax Nitro",
                    RoleTypeName = RoleTypeEnum.Senior.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 909411214508060732,
                    Name = "#Friend",
                    RoleTypeName = RoleTypeEnum.Individual.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 937084747144921148,
                    Name = "Kleiner Nazi",
                    RoleTypeName = RoleTypeEnum.Individual.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 902236718726516767,
                    Name = "#4urka",
                    RoleTypeName = RoleTypeEnum.Individual.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 906963736839999558,
                    Name = "#Извращенец",
                    RoleTypeName = RoleTypeEnum.Individual.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 983804653122752553,
                    Name = "Пуся",
                    RoleTypeName = RoleTypeEnum.Individual.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 901930294083461140,
                    Name = "Мажор в КсГо💎",
                    RoleTypeName = RoleTypeEnum.Individual.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1109504662358458470,
                    Name = "Художник",
                    RoleTypeName = RoleTypeEnum.Hobby.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1109506192344100964,
                    Name = "Музыкант",
                    RoleTypeName = RoleTypeEnum.Hobby.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1109507127942316094,
                    Name = "Блогер",
                    RoleTypeName = RoleTypeEnum.Hobby.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1109508168033259593,
                    Name = "Геймер",
                    RoleTypeName = RoleTypeEnum.Hobby.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1109508957724233871,
                    Name = "Программист",
                    RoleTypeName = RoleTypeEnum.Hobby.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1109509715773366444,
                    Name = "Монтажер",
                    RoleTypeName = RoleTypeEnum.Hobby.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 824196020077461574,
                    Name = "ℂ𝕊﹣𝔾𝕆",
                    RoleTypeName = RoleTypeEnum.Game.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 824199697723424768,
                    Name = "🅶🆃🅰𝐕",
                    RoleTypeName = RoleTypeEnum.Game.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 928244382639980586,
                    Name = "𝓓𝓑𝓓",
                    RoleTypeName = RoleTypeEnum.Game.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 1094913928099794955,
                    Name = "𝔥𝔬𝔯𝔯𝔬𝔯",
                    RoleTypeName = RoleTypeEnum.Game.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 989038636639072257,
                    Name = "𝒮𝓮𝒶 𝒪𝒻  𝒯𝒽𝒾𝑒𝓋𝑒𝓈",
                    RoleTypeName = RoleTypeEnum.Game.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 964168151908626524,
                    Name = "𝘈𝘗𝘌𝘟",
                    RoleTypeName = RoleTypeEnum.Game.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 931415649153060864,
                    Name = "𝓦𝓸𝓣",
                    RoleTypeName = RoleTypeEnum.Game.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 934357129693913141,
                    Name = "𝔻𝕖𝕔𝕖𝕚𝕥",
                    RoleTypeName = RoleTypeEnum.Game.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 824199752400371732,
                    Name = "🅳🅾🆃🅰２",
                    RoleTypeName = RoleTypeEnum.Game.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 825340227223683113,
                    Name = "Bot",
                    RoleTypeName = RoleTypeEnum.Bot.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 934059373188251679,
                    Name = "JuniperBot",
                    RoleTypeName = RoleTypeEnum.Bot.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 934060643840040960,
                    Name = "Гость",
                    RoleTypeName = RoleTypeEnum.Guest.ToString(),
                },

                new RoleModel()
                {
                    
                    RoleId = 824195873620623421,
                    Name = "@everyone",
                    RoleTypeName = RoleTypeEnum.None.ToString(),
                },
            };

            if (guildId == 359947892288520194)
                listRolesServerRelax = new List<RoleModel>()
                {
                    new RoleModel()
                    {
                        
                        RoleId = 1096793247910727764,
                        Name = "smurw",
                        RoleTypeName = RoleTypeEnum.Bot.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 928430217368854598,
                        Name = "Moderator",
                        RoleTypeName = RoleTypeEnum.Senior.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094550978801434684,
                        Name = "Steam",
                        RoleTypeName = RoleTypeEnum.Hobby.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094551824033382472,
                        Name = "YouTube",
                        RoleTypeName = RoleTypeEnum.Hobby.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094909997835624468,
                        Name = "Dead By Daylight",
                        RoleTypeName = RoleTypeEnum.Game.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 928425101731454987,
                        Name = "CS GO",
                        RoleTypeName = RoleTypeEnum.Game.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094911444367192144,
                        Name = "Sea Of Thieves",
                        RoleTypeName = RoleTypeEnum.Game.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094910500661375007,
                        Name = "Grand Theft Auto",
                        RoleTypeName = RoleTypeEnum.Game.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094911322447155300,
                        Name = "Horror Game",
                        RoleTypeName = RoleTypeEnum.Game.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094911830993940530,
                        Name = "Мир танков",
                        RoleTypeName = RoleTypeEnum.Game.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094910322835460116,
                        Name = "Apex Legends",
                        RoleTypeName = RoleTypeEnum.Game.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094910413721841764,
                        Name = "Deceit",
                        RoleTypeName = RoleTypeEnum.Game.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1094911687984947231,
                        Name = "Dota 2",
                        RoleTypeName = RoleTypeEnum.Game.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1092347276913020939,
                        Name = "Тайный гость👀",
                        RoleTypeName = RoleTypeEnum.Guest.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1095249858421592224,
                        Name = "бот",
                        RoleTypeName = RoleTypeEnum.Bot.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1098144597529591831,
                        Name = "Akemi",
                        RoleTypeName = RoleTypeEnum.Bot.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 1108285643630583820,
                        Name = "JuniperBot",
                        RoleTypeName = RoleTypeEnum.Bot.ToString(),
                    },

                    new RoleModel()
                    {
                        
                        RoleId = 359947892288520194,
                        Name = "@everyone",
                        RoleTypeName = RoleTypeEnum.None.ToString(),
                    },
                };

            return listRolesServerRelax;
        }

        public List<ChannelModel> SetAllChannels(ulong guildId)
        {
            var listChannels = new List<ChannelModel>()
            {
                new ChannelModel()
                {
                    ChannelId = 937577384633335858,
                    ChannelName = "│🔆⠂welcome",
                    GuildModelId = guildId,
                    ChannelType = ChannelTypeEnum.None.ToString()
                },

                new ChannelModel()
                {
                    ChannelId = 1095628286173913200,
                    ChannelName = "└✚⠂создать",
                    GuildModelId = guildId,
                    ChannelType = ChannelTypeEnum.Private.ToString(),
                },
            };

            if (guildId == 359947892288520194)
                listChannels = new List<ChannelModel>()
                {
                    new ChannelModel()
                    {
                        ChannelId = 1092334594969313384,
                        ChannelName = "wecome👋",
                        GuildModelId = guildId,
                        ChannelType = ChannelTypeEnum.None.ToString(),
                    },

                    new ChannelModel()
                    {
                        ChannelId = 1109358115276730440,
                        ChannelName = "voice",
                        GuildModelId = guildId,
                        ChannelType = ChannelTypeEnum.Private.ToString(),
                    },
                };

            return listChannels;

        }

        public bool SavingAllDataToDb()
        {
            var guilds = SetAllGuilds();
            if (guilds == null) return false;
            
            _db.Guilds.AddRange(guilds);
            _db.SaveChanges();

            return true;
        }
    }
}
