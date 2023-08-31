using Discord_Bot_SmurW.Engine.DiscordGuildEngine;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using Discord_Bot_SmurW.Engine.Models;
using System.Diagnostics.Metrics;

namespace Discord_Bot_SmurW.Engine.LevelSystem
{
    public class RaitingUsersEngine : BaseEngine
    {
        private DiscordMember? _dMember;
        public RaitingUsersEngine(DiscordGuild guild) : base(guild) { }

        public RaitingUsersEngine(DiscordMember dMember, DiscordGuild dGuild) : base(dGuild)
        {
            _dMember = dMember;
        }

        // Получить список топ 3 участников сервера
        public List<UserModel> GetTopUsersRaiting()
        {
            var guild = _db.Guilds.Include(g => g.Users).FirstOrDefault(g => g.GuildId == _guild.Id);
            if (guild is null) return null!;

            var users = guild.Users;
            if(users is null) return null!;

            return users.OrderByDescending(u => u.XP).Take(3).ToList();
        }

        // Существует ли пользователь в бд
        public bool CheckUserExistsToDb()
        {
            try
            {
                var guild = _db.Guilds.Include(g => g.Users).SingleOrDefault(g => g.GuildId == _guild.Id);
                if (guild == null) return false;
                
                var user = guild.Users?.Any(u => u.UserId == _dMember!.Id);
                if (user == false) return false;
            }
            catch (Exception) { return false; }

            return true;
        }

        /// <summary>
        /// Получить пользователя с бд
        /// </summary>
        public UserModel GetUserFromDb(DiscordMember member = null!)
        {
            try
            {
                var guild = _db.Guilds!.Include(g => g.Users).FirstOrDefault(g => g.GuildId == _guild.Id);
                if (guild is null) return null!;

                var user = guild?.Users?.FirstOrDefault(u => u.UserId == _dMember!.Id);

                if (member != null)
                {
                    user = guild?.Users?.FirstOrDefault(u => u.UserId == member!.Id);
                }
                
                if (user is null) return null!;

                return user;
            }
            catch (Exception) { return null!; }
        }

        /// <summary>
        /// Добавление опыта за сообщения
        /// </summary>
        public bool AddingExpPerPost(UserModel user)
        {
            bool levelUp = false;
            try
            {
                if (user is null) return false;

                var userTimeLimit = DateTime.Parse(user.TimeLimit!);
                if (userTimeLimit > DateTime.Now) return false;

                int rndCountXP = new Random().Next(1, 5);

                double currentValueXp = 0.0;
                int limitXpForLevelUP = 500;
                int maxLevel = 100;
                var timeLimit = DateTime.Now + TimeSpan.FromSeconds(30);
                user.CountMessage++;
                user.XP += rndCountXP;
                user.TimeLimit = timeLimit.ToShortTimeString();
                currentValueXp = user.XP / user.Level;

                if (currentValueXp >= limitXpForLevelUP && user.Level < maxLevel)
                {
                    user.Level++;
                    levelUp = true;
                }
                _db.SaveChanges();
            }
            catch (Exception) { }
            return levelUp;
        }

        /// <summary>
        /// Количество дней проведенных на сервере
        /// </summary>
        public void GettingDaysOnTheGuild()
        {
            var user = GetUserFromDb();
            if (user is null) return;

            var member = (DiscordMember?)_dMember;
            user.DaysOnTheGuild = (int)(DateTime.Now - member!.JoinedAt).TotalDays;

            _db.SaveChanges();
        }

        /// <summary>
        /// Сохранение времени подключения к голосовому каналу
        /// </summary>
        public void MemberConnectedVoiceChannel()
        {
            var user = GetUserFromDb();
            if (user is null) return; 

            user.ConnectUserToChannel = DateTime.Now.ToShortTimeString();

            _db.SaveChanges();
        }

        /// <summary>
        /// Расчет опыта за voice
        /// </summary>
        private void EngineExpPerVoice(UserModel user, TimeSpan duration)
        {
            user.TotalMinutesForSession += (int)duration.TotalMinutes; 
            int hours = (int)user.TotalMinutesForSession / 60; 
            int countExp = 50;

            if (hours >= 1)
            {
                user.TotalMinutesForSession -= 60 * hours;
                user.XP += hours * countExp; 
                user.VoiceTime += hours;
            }
        }

        /// <summary>
        /// Добавление опыта за voice
        /// </summary>
        public void AddingExpPerVoice(bool isViewRaiting, DateTime timeRunBot = default)
        {
            var user = GetUserFromDb();
            if (user is null) return;

            var member = (DiscordMember?)_dMember;
            var conUserToChannelString = DateTime.Parse(user.ConnectUserToChannel!);

            if (member!.VoiceState == null && isViewRaiting == true) return;

            if (isViewRaiting == true)// Участник вызвал команду !рейтинг
            {
                if (timeRunBot < conUserToChannelString) // бот подкючился раньше чем юзер
                {
                    TimeSpan duration = DateTime.Now - conUserToChannelString;
                    EngineExpPerVoice(user, duration);
                    conUserToChannelString = DateTime.Now;
                }
                else // юзер подключился раньше чем бот
                {
                    var duration = DateTime.Now - timeRunBot;
                    EngineExpPerVoice(user, duration);
                    BotModel.TimeConnected = DateTime.Now;
                }
            }
            else
            {
                if (timeRunBot < conUserToChannelString)
                {
                    var duration = DateTime.Now - conUserToChannelString;
                    EngineExpPerVoice(user, duration);
                }
                else
                {
                    var duration = DateTime.Now - timeRunBot;
                    EngineExpPerVoice(user, duration);
                }
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// Установить опыт участнику
        /// </summary>
        public bool SetXPUser(string plusOrMin, ulong xp, DiscordMember member = null!)
        {
            try
            {
                var plus = plusOrMin.Contains('+');
                var minus = plusOrMin.Contains('-');
                if (plus is false && minus is false) return false;

                var user = GetUserFromDb(member);
                if (user is null) return false;

                if (plus is true) user.XP += xp;
                if (minus is true) user.XP -= xp;
               
                _db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// Установить уровень участнику
        /// </summary>
        public bool SetLevel(ulong level, DiscordMember member = null!)
        {
            try
            {
                if (level > 100) return false;

                if (member == null)
                {
                    var user = _db?.Guilds?.Include(g => g.Users)?.First(g => g.GuildId == _guild.Id)?
                             .Users?.Single(u => u.UserId == _dMember?.Id);

                    if (user != null)
                        user.Level = (int)level;
                }
                else
                {
                    var user = _db.Guilds.Include(g => g.Users)?.First(g => g.GuildId == _guild.Id)?
                             .Users?.Single(u => u.UserId == member.Id);

                    if (user != null)
                    user.Level = (int)level;

                }

                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Сохранение пользователя в бд
        /// </summary>
        public bool SavingUserToDb(DiscordMember user)
        {
            var newUser = new UserModel()
            {
                UserId = user.Id,
                UserName = user.Username,
                ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                Level = 1,
                XP = 0,
                VoiceTime = 0,
                TotalMinutesForSession = 0,
                DaysOnTheGuild = (int)(DateTime.Now - user.JoinedAt).TotalDays,
                CountMessage = 0,
                TimeLimit = DateTime.Now.ToShortTimeString(),
                AvatarUrl = user.AvatarUrl,
                GuildModelId = _guild.Id,
            };

            if (newUser == null) return false;

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Сохранение список пользователей в бд
        /// </summary>
        public bool SavingUsersToDb(List<DiscordMember> dcMembers)
        {
            if (dcMembers == null) return false;

            var rangeMembers = new List<UserModel>();

            foreach (var user in dcMembers)
            {
                if (user.IsBot) continue;

                var newUser = new UserModel()
                {
                    UserId = user.Id,
                    UserName = user.Username,
                    ConnectUserToChannel = DateTime.Now.ToShortTimeString(),
                    Level = 1,
                    XP = 0,
                    VoiceTime = 0,
                    TotalMinutesForSession = 0,
                    DaysOnTheGuild = (int)(DateTime.Now - user.JoinedAt).TotalDays,
                    CountMessage = 0,
                    TimeLimit = DateTime.Now.ToShortTimeString(),
                    AvatarUrl = user.AvatarUrl,
                    GuildModelId = _guild.Id,
                };
                rangeMembers.Add(newUser);
            }
            _db.Users.AddRange(rangeMembers);
            _db.SaveChanges();

            return true;
        }

    }
}
