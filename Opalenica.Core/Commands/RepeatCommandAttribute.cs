// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.Commands;

using System;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Attributes;
using Kirun9.CommandParser.Results;

[AttributeUsage(AttributeTargets.Method, AllowMultiple =true, Inherited = true)]
public class RepeatCommandAttribute : PreconditionAttribute
{
    public int Count { get; set; }
    public bool SameParameters { get; set; }
    public RepeatCommandAttribute(int count = 2, bool sameParameters = false)
    {
        Count = count;
        SameParameters = sameParameters;
    }

    public override PreconditionResult CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        return CheckPermissions(context, command, services, 1);
    }

    private PreconditionResult CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services, int inheritance)
    {
        if (context is null) throw new InvalidOperationException("Context cannot be null.");
        if (context is CommandContext) return PreconditionResult.FromError("Contect cannot be CommandContext");
        if (context is ChainedCommandContext ccc)
        {
            var condition = SameParameters ? ccc.IsSameContent : ccc.IsSameCommand;
            if (!condition && ccc.PrevContext is not null) return PreconditionResult.FromError("Commands do not match");
            if (ccc.PrevContext is not ChainedCommandContext && inheritance == Count) return PreconditionResult.FromSuccess();
            else if (ccc.PrevContext is not ChainedCommandContext && inheritance != Count) return StoppedPreconditionResult.FromError("Too few repeats");
            else if (ccc.PrevContext is ChainedCommandContext) return CheckPermissions(ccc.PrevContext, command, services, inheritance + 1);
        }
        return PreconditionResult.FromError("Unknown");
    }
}