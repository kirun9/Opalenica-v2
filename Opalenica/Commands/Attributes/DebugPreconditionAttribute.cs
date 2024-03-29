﻿// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Commands.Attributes;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Attributes;
using Kirun9.CommandParser.Results;

using Microsoft.Extensions.DependencyInjection;

using System;

public class DebugCommandAttribute : PreconditionAttribute
{
    public override PreconditionResult CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        var settings = services.GetService<Settings>();
        return context.Sender is not null and InternalCommandSender and { IsAdmin: true } || settings.DebugMode
            ? PreconditionResult.FromSuccess()
            : PreconditionResult.FromError("Debug mode is not enabled.");
    }
}