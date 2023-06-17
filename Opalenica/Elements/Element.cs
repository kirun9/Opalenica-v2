namespace Opalenica.Elements;

using Kirun9.CommandParser.Readers;
using Kirun9.CommandParser.Results;
using Kirun9.CommandParser;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Linq;

public class Element
{
    internal readonly Guid internalGuid = Guid.NewGuid();
    public String Name { get; set; }
    public Dictionary<string, object> PermanentData { get; set; } = new Dictionary<string, object>();
    public static Element? SelectedElement { get; set; }
    public bool IsSelected => SelectedElement?.internalGuid.Equals(internalGuid) ?? false;

    private static List<Element> registeredElements = new List<Element>();

    public void Select()
    {
        SelectedElement = this;
    }

    public static void Unselect()
    {
        SelectedElement = null;
    }

    public void RegisterElement()
    {
        RegisterElement(this);
    }

    public static T RegisterElement<T>(T element) where T : Element
    {
        var foundElement = registeredElements.FirstOrDefault(e => e.internalGuid == element.internalGuid && e.GetType() == element.GetType(), null);
        if (foundElement is null)
        {
            registeredElements.Add(element);
            return element;
        }
        var index = registeredElements.IndexOf(foundElement);
        var needReplace = foundElement.UpdateElement(element);
        if (needReplace) foundElement = element;
        registeredElements[index] = foundElement;
        return registeredElements[index] as T;
    }

    public static T RegisterElement<T>(T element, bool useName = false) where T : Element
    {
        var foundElement = registeredElements.FirstOrDefault(e => (useName ? e.Name == element.Name : e.internalGuid == element.internalGuid) && e.GetType() == element.GetType(), null);
        if (foundElement is null)
        {
            registeredElements.Add(element);
            return element;
        }
        var index = registeredElements.IndexOf(foundElement);
        var needReplace = foundElement.UpdateElement(element);
        if (needReplace) foundElement = element;
        registeredElements[index] = foundElement;
        return registeredElements[index] as T;
    }

    public static T GetElement<T>(Guid guid) where T : Element
    {
        return registeredElements.FirstOrDefault(e => e.internalGuid == guid && e.GetType() == typeof(T), null) as T;
    }

    public static T GetElement<T>(string Name) where T : Element
    {
        return registeredElements.FirstOrDefault(e => e.Name == Name && e.GetType() == typeof(T), null) as T;
    }

    /// <summary>
    /// Updates element data
    /// </summary>
    /// <param name="element">Data of new element</param>
    /// <returns><code>true</code> when data was updated successfully and no replacement is needed. <code>false</code> when replace action should be taken</returns>
    public virtual bool UpdateElement<T>(T element) where T : Element
    {
        return false;
    }

    // Serializer related class
    public class ElementJsonConverter : JsonConverter<Element>
    {
        public override Element? ReadJson(JsonReader reader, Type objectType, Element? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var element = new Element();
            element.Name = (string)jsonObject["Name"];

            // Do not deserialize PermanentData for now, it will be handled during Tile deserialization

            return element;
        }

        public override void WriteJson(JsonWriter writer, Element? value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteValue(value.Name);

            // Do not serialize PermanentData for now, it will be handled during Tile serialization

            writer.WriteEndObject();
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }


    // Command related class
    public class ElementReader : TypeReader
    {
        public override TypeReaderResult Read(ICommandContext context, String input, IServiceProvider provider)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty.", nameof(input));
            var commandService = provider.GetService<CommandService>();
            var found = registeredElements.Select(e =>
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
}

public static class EnumerableElementExtensions
{
    public static bool Contains<T>(this IEnumerable<T> enumerable, Guid guid) where T : Element
    {
        return enumerable.Select(e => e.internalGuid).Contains(guid);
    }

    public static bool Contains<T>(this IEnumerable<T> enumerable, T elem) where T : Element
    {
        return enumerable.Select(e => e.internalGuid).Contains(elem.internalGuid);
    }
}
