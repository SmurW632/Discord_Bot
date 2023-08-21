using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.Models
{
    [Table("Каналы")]
    public class ChannelModel
    {
        public int Id { get; set; }
        public string? ChannelName { get; set; }
        public ulong ChannelId { get; set; }
        public string? ChannelType { get; set; }

        public ulong GuildModelId { get; set; }
        public GuildModel? GuildModel { get; set; }

    }

    public enum ChannelTypeEnum
    {
        None,
        Private,
        Rules,
    }
}
