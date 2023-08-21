using Discord_Bot_SmurW.Engine.Models;
using Discord_Bot_SmurW.Enums;
using Discord_Bot_SmurW.TestData; 
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.DiscordGuildEngine.DiscordRoleEngine
{
    public class RoleEngine : BaseEngine
    {
        public RoleEngine(DiscordGuild guild) : base(guild) { }

        public bool IsExistsRoleToDb()
        {
            var guild = _db.Guilds.Include(g => g.Roles).FirstOrDefault(g => g.GuildId == _guild.Id);
            if(guild == null) return false;

            var role = guild?.Roles?.FirstOrDefault(r => r.RoleId == _guild.GetRole(r.RoleId).Id);
            if(role == null) return false;

            return true;
        }

        public RoleModel GetRoleFromDb()
        {
            var guild = _db.Guilds.Include(g => g.Roles).FirstOrDefault(g => g.GuildId == _guild.Id);
            var role = guild?.Roles?.FirstOrDefault(r => r.RoleId == _guild.GetRole(r.RoleId).Id);
            
            return new RoleModel()
            {
                RoleId = role!.RoleId,
                Name = role.Name,
                RoleTypeName = role.RoleTypeName,
            };
        }

        public List<RoleModel> GetListRolesFromDb()
        {
            var guild = _db.Guilds.Include(g => g.Roles).FirstOrDefault(g => g.GuildId == _guild.Id);
            if (guild == null) return null!;

            var roles = guild!.Roles;
            if (roles == null) return null!;

            return roles;
        }

        public bool SavingTestRolesToDb()
        {
            var savingTestData = new SavingTestData(_guild);
            var testData = savingTestData.SetAllRoles(_guild.Id);

            if(testData == null) return false;

            _db.Roles.AddRange(testData);
            _db.SaveChanges();

            return true;
        }

        public bool SavingRolesToDb(List<DiscordRole> roles)
        {
            if(roles == null) return false;

            foreach (var role in roles)
            {
                var newRole = new RoleModel()
                {
                    RoleId = role.Id,
                    Name = role.Name,
                    RoleTypeName = RoleTypeEnum.None.ToString(),
                    GuildModelId = _guild.Id,
                };

                _db.Roles.Add(newRole);
            }
            _db.SaveChanges();

            return true;
        }


        public bool CreateRole(DiscordRole dRole)
        {
            var guild = _db.Guilds.Include(g => g.Roles).FirstOrDefault(g => g.GuildId == _guild.Id);
            if (guild == null) return false;

            var role = guild.Roles?.FirstOrDefault(r => r.RoleId == dRole.Id);
            if (role != null) return false;

            var newRole = new RoleModel()
            {
                RoleId = dRole.Id,
                Name = dRole.Name,
                RoleTypeName = RoleTypeEnum.None.ToString(),
                GuildModelId = _guild.Id,
            };

            _db.Roles.Add(newRole);
            _db.SaveChanges();

            return true;
        }

        public bool DeleteRole(DiscordRole dRole)
        {
            var guild = _db.Guilds.Include(g => g.Roles).FirstOrDefault(g => g.GuildId == _guild.Id);
            if (guild == null) return false;

            var role = guild.Roles?.FirstOrDefault(r => r.RoleId == dRole.Id);
            if (role == null) return false;

            _db.Roles.Remove(role);
            _db.SaveChanges();

            return true;
        }

        public DiscordEmbedBuilder SendChengedMessageToPrivateChannel(ChangedGuildRolesEnum type, DiscordRole roleAfter, DiscordRole roleBefore = null!)
        {
            string? description = "Никаких состояний не произошло!";
            switch (type)
            {
                case ChangedGuildRolesEnum.Create:
                    {
                        description = $"```Роль \"{roleAfter.Name}\" была создана!\n" +
                                      $"ID: {roleAfter.Id}\n" +
                                      $"Color: {roleAfter.Color}\n" +
                                      $"Position: {roleAfter.Position}\n" +
                                      $"Permissions: {roleAfter.Permissions}```";
                    }
                    break;
                case ChangedGuildRolesEnum.Update:
                    {
                        description = $"```Ролль \"{roleBefore.Name}\" была изменена!\n" +
                                      $"ID: {roleBefore.Id}\n" +
                                      $"Color: {roleBefore.Color}\n" +
                                      $"Position: {roleBefore.Position}\n" +
                                      $"Permissions: {roleBefore.Permissions}```\n" +
                                      $"```Новые значения роли:\n" +
                                      $"Name: {roleAfter.Name}\n" +
                                      $"Color: {roleAfter.Color}\n" +
                                      $"Position: {roleAfter.Position}\n" +
                                      $"Permissions: {roleAfter.Permissions}```";
                    }
                    break;
                case ChangedGuildRolesEnum.Delete:
                    {
                        description = $"```Роль \"{roleAfter.Name}\" была удалена!\n" +
                                      $"ID: {roleAfter.Id}\n" +
                                      $"Color: {roleAfter.Color}\n" +
                                      $"Position: {roleAfter.Position}\n" +
                                      $"Permissions: {roleAfter.Permissions}```";
                    }
                    break;
            }

            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Azure,
                Title = $"Изменения роли: {roleAfter.Mention}",
                Description = description,
            };
        }
        private bool EqualsDiscordRole(DiscordRole before, DiscordRole after)
        {
            if (before == null || after == null)
                return false;

            if (object.ReferenceEquals(before, after))
                return true;

            if (after.GetType() != before.GetType() || before.GetType() != after.GetType())
                return false;

            if (string.Compare(before.Name, after.Name, StringComparison.CurrentCulture) == 0
                && before.Color.Value == after.Color.Value && before.Position == after.Position
                && before.Permissions == after.Permissions)
                return true;
            else
                return false;
        }

        public bool UpdateRole(DiscordRole roleAfter, DiscordRole roleBefore)
        {
            if (EqualsDiscordRole(roleBefore, roleAfter)) return false;

            var guild = _db.Guilds.Include(g => g.Roles).FirstOrDefault(g => g.GuildId == _guild.Id);
            if (guild == null) return false;

            var role = guild.Roles?.FirstOrDefault(r => r.RoleId == roleBefore.Id);
            if (role == null) return false;

            role.Name = roleAfter.Name;

            _db.SaveChanges();

            return true;
        }

        public ulong GuestRole
        {
            get 
            {
                var guild = _db.Guilds.Include(g => g.Roles).FirstOrDefault(g => g.GuildId == _guild.Id);
                var role = guild?.Roles?.FirstOrDefault(r => r.RoleTypeName == RoleTypeEnum.Guest.ToString());

                if (role == null) return 0;

                return role.RoleId;
            }
        }
    }
}
