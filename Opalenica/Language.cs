// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Core;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

public static class Language
{
    private static ResourceSet rs;
    private static ResourceManager rm;

    public static bool SkipMainNamespace { get; set; } = true;

    public static void InitializeLanguage(CultureInfo selectedLanguage)
    {
        selectedLanguage ??= CultureInfo.CurrentCulture;
        if (selectedLanguage.Name == "pl-SL" && !GetSupportedLanguages().Contains(selectedLanguage))
        {
            RegisterSilesianCultureInfo();
        }
        if (!GetSupportedLanguages().Contains(selectedLanguage)) selectedLanguage = new CultureInfo("en");
        rm = new ResourceManager("Opalenica.Strings", Assembly.GetEntryAssembly());
        rs = rm.GetResourceSet(selectedLanguage, true, false);
        if (rs is null) throw new Exception("Selected Language " + selectedLanguage.EnglishName + " is not suppported");
        Thread.CurrentThread.CurrentCulture = selectedLanguage;
        Thread.CurrentThread.CurrentUICulture = selectedLanguage;
        CultureInfo.DefaultThreadCurrentCulture = selectedLanguage;
        CultureInfo.DefaultThreadCurrentUICulture = selectedLanguage;
        CultureInfo.CurrentCulture = selectedLanguage;
        CultureInfo.CurrentUICulture = selectedLanguage;
    }

    private static void UnregisterSilesianCultureInfo()
    {
        CultureAndRegionInfoBuilder.Unregister("pl-SL");
    }

    private static void RegisterSilesianCultureInfo()
    {
        UnregisterSilesianCultureInfo();
        CultureAndRegionInfoBuilder builder = new CultureAndRegionInfoBuilder("pl-SL", CultureAndRegionModifiers.None);
        var parent = CultureInfo.GetCultureInfo("pl-PL");
        builder.LoadDataFromCultureInfo(parent);
        builder.LoadDataFromRegionInfo(new RegionInfo(parent.LCID));
        builder.RegionEnglishName = "Silesia";
        builder.RegionNativeName = "Ślōnski";
        builder.CultureEnglishName = "Polish (Silesia)";
        builder.CultureNativeName = "Polski (Ślōnski)";
        builder.Register();
    }

    public static string GetString(string name, CultureInfo culture)
    {
        culture ??= CultureInfo.CurrentCulture;
        var s = rm?.GetString(name, culture);
        if (s is null or "") s = GetString(name);
        return s;
    }

    public static string GetString(string name)
    {
        var s = rs?.GetString(name);
        if (s is null or "")
            s = rm?.GetString(name);
        if (s is null or "")
            s = $"<{name}>";
        return s;
    }

    public static string GetString(string name, Type type, bool skipMainNamespace, CultureInfo culture)
    {
        return GetString(type.FullName[(skipMainNamespace ? typeof(Language).Namespace.Length + 1 : 0)..] + "." + name, culture);
    }

    public static string GetString(string name, Type type, bool skipMainNamespace)
    {
        return GetString(type.FullName[(skipMainNamespace ? typeof(Language).Namespace.Length + 1 : 0)..] + "." + name);
    }

    public static string GetString(string name, Type type, CultureInfo culture)
    {
        return GetString(name, type, SkipMainNamespace, culture);
    }

    public static string GetString(string name, Type type)
    {
        return GetString(name, type, SkipMainNamespace);
    }

    public static IReadOnlyCollection<CultureInfo> GetSupportedLanguages()
    {
        List<CultureInfo> supportedCultures = new List<CultureInfo>();
        var rm = new ResourceManager("Opalenica.Strings", Assembly.GetEntryAssembly());
        foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            try
            {
                if (culture.Name is "") continue;
                ResourceSet rs = rm.GetResourceSet(culture, true, false);
                if (rs is null) continue;
                Debug.WriteLine(culture.EnglishName + " (" + culture.Name + ") is supported");
                supportedCultures.Add(culture);
            }
            catch (CultureNotFoundException)
            {
                Debug.WriteLine(culture + " is not available on the machine or is an invalid culture identifier.");
            }
        }
        return supportedCultures.AsReadOnly();
    }

    public static string LangcodeToNativeName(string langCode)
    {
        return LangcodeToCultureInfo(langCode).NativeName;
    }

    public static string LangcodeToEnglishName(string langCode)
    {
        return LangcodeToCultureInfo(langCode).EnglishName;
    }

    public static string LangcodeToLocalName(string langCode)
    {
        return LangcodeToCultureInfo(langCode).DisplayName;
    }

    public static string NativeNameToLangcode(string name)
    {
        var culture = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(e => e.NativeName == name).Select(e => e.Name).FirstOrDefault("Not found");
        return culture is "Not found" ? throw new ArgumentException("Invalid native language name") : culture;
    }

    public static string EnglishNameToLangcode(string name)
    {
        var culture = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(e => e.EnglishName == name).Select(e => e.Name).FirstOrDefault("Not found");
        return culture is "Not found" ? throw new ArgumentException("Invalid english language name") : culture;
    }

    public static string LocalNameToLangcode(string name)
    {
        var culture = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(e => e.DisplayName == name).Select(e => e.Name).FirstOrDefault("Not found");
        return culture is "Not found" ? throw new ArgumentException("Invalid local language name") : culture;
    }

    public static CultureInfo LangcodeToCultureInfo(string langCode)
    {
        string langRegex = @"^([a-z]{2})(?:-([A-Z]{2,3}))?$";
        return Regex.IsMatch(langCode, langRegex)
            ? new CultureInfo(langCode)
            : throw new ArgumentException("Invalid language code");
    }
}
