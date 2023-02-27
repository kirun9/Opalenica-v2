// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.Commands;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Attributes;
using Kirun9.CommandParser.Results;

public class StoppedPreconditionResult : PreconditionResult
{
    protected StoppedPreconditionResult(CommandError? error, string errorReason) : base(error, errorReason)
    {
    }

    public static new StoppedPreconditionResult FromError(string errorReason) => new StoppedPreconditionResult(CommandError.UnmetPrecondition, errorReason);
    public static new StoppedPreconditionResult FromSuccess() => new StoppedPreconditionResult(null, null);
}