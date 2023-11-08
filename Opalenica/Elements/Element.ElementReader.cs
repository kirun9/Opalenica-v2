// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Elements;

using Kirun9.CommandParser.Readers;
using Kirun9.CommandParser.Results;
using Kirun9.CommandParser;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;

public partial class Element
{
    // Command related class
    public class ElementReader : TypeReader
    {
        public override TypeReaderResult Read(ICommandContext context, string input, IServiceProvider provider)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty.", nameof(input));
            var commandService = provider.GetService<CommandService>();
            var found = registeredElements.Select(e =>
            {
                return false ? e.Name.Equals(input) : e.Name.ToLowerInvariant().Equals(input.ToLowerInvariant());
            });
            if (found.Any())
            {
                if (found.Count() == 1)
                    return TypeReaderResult.FromSuccess(found.First());
                else
                    return TypeReaderResult.FromError(CommandError.MultipleMatches, "Multiple elements matched.");
            }
            else
            {
                return TypeReaderResult.FromError(CommandError.ParseFailed, "Element not found.");
            }
        }
    }
}
