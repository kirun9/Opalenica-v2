// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Elements;

using System;
using System.Linq;

public partial class Element
{
    internal readonly Guid internalGuid = Guid.NewGuid();
    public string Name { get; set; }
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
}