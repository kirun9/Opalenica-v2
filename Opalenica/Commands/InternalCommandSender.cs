namespace Opalenica.Commands;

using Kirun9.CommandParser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class InternalCommandSender : ICommandSender
{
    public string ID { get; set; }
    public bool IsAdmin { get; internal set; }
}
