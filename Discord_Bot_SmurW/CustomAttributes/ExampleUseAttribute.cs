using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ExampleUseAttribute : CheckBaseAttribute
    {
        public string Description { get; private set; }
        public ExampleUseAttribute(string description)
        {
            Description = description;
        }
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            return Task.FromResult(String.IsNullOrEmpty(Description));
        }
    }
}
