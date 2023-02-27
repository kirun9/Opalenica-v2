namespace Opalenica;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Readers;
using Kirun9.CommandParser.Results;

using Microsoft.Extensions.DependencyInjection;

using Opalenica.ModuleBase;

using System;

public class Element : IElement
{
    internal static List<Element> RegisteredElements = new List<Element>();

    public string Name { get; set; }
    public Guid Id { get; set; }
    public bool IsSelected => SelectedElement?.Id.Equals(Id) ?? false;
    public static Element? SelectedElement { get; internal set; }

    public void Select()
    {
        SelectedElement = this;
    }

    public void Deselect()
    {
        SelectedElement = null;
    }

    public void RegisterElement(Element element)
    {
        RegisteredElements.Add(element);
    }

    public void UnregisterElement(Element element)
    {
        RegisteredElements.Remove(element);
    }
}

public class ElementReader : TypeReader
{
    public override TypeReaderResult Read(ICommandContext context, String input, IServiceProvider provider)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));
        var commandService = provider.GetService<CommandService>();
        var found = Element.RegisteredElements.Select(e =>
        {
            return Settings.Default.CaseSensitiveCommands ? e.Name.Equals(input) : e.Name.ToLowerInvariant().Equals(input.ToLowerInvariant());
        });
        if (found.Any())
        {
            if (Settings.Default.IgnoreMultiMatchElements)
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