using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.Models
{
    public class ConfigModel
    {
        public int Id { get; set; }
        public string? Token { get; private set; }
        public string? Prefix { get; private set; }
    }
}
