using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.Models
{
    [Table("Роли")]
    public class RoleModel
    {
        public int Id { get; set; }
        public ulong RoleId { get; set; }
        public string? Name { get; set; }
        public string? RoleTypeName { get; set; }

        public ulong GuildModelId { get; set; }
        public GuildModel? GuildModel { get; set; }
    }

   public enum RoleTypeEnum
    {
        None, // нет типа
        Senior, // Старший
        Game, // Игровой
        Hobby, // Хобби
        Individual, // Индивидуальный
        Guest, // Гость
        Bot, // Бот
    }
}
