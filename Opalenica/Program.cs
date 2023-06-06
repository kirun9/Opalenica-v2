// Copyright (c) Krystian Pawe³ek from PKMK. All rights reserved.

namespace Opalenica;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Opalenica.Commands;
using Opalenica.Logging;
using Opalenica.UI;

internal class Program
{
    internal static Settings Settings { get; } = Settings.Default;
    public static IServiceCollection ServiceCollection { get; } = new ServiceCollection();
    public static IServiceProvider ServiceProvider => ServiceCollection.BuildServiceProvider();

    internal static string LOG_SOURCE = "Opalenica";
    internal static MainWindow MainWindow { get; set; }

    static ILogger Logger { get; set; } = ServiceProvider.GetService<ILogger>();

    [STAThread]
    private static void Main()
    {
        ServiceCollection.AddSingleton(typeof(ILogger), new MultiLogger());
        CommandManager.Initialize(Settings);
        ServiceProvider.GetRequiredService<ILogger>().Log(new LogMessage($"Starting Opalenica { Assembly.GetAssembly(typeof(Program)).GetName().Version } ...", LOG_SOURCE, MessageLevel.Info, "Program"));

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += Application_ThreadException;
        Application.Run(MainWindow = new MainWindow(ServiceProvider));
    }

    private static void Application_ThreadException(Object sender, ThreadExceptionEventArgs e)
    {
        Logger.Log(new LogMessage(e.Exception.ToString(), "InternalThread", MessageLevel.Error));
    }
}

public class BasicCommandsModule : ModuleBase<ICommandContext>
{
    public ILogger Logger { get; set; }

    [Command("exit")]
    [Alias("koniec")]
    [Alias("wyjœcie")]
    [Alias("wyjscie")]
    [ChainedCommand]
    [RepeatCommand(Count = 2)]
    [BreakChainedCommands]
    public void Exit()
    {
        Log("Exiting program ...");
        Application.ExitThread();
        Environment.Exit(0);
    }

    [Command("debugmode")]
    public void DebugMode()
    {
        Program.Settings.DebugMode = !Program.Settings.DebugMode;
        Log("Debug mode is " + (Program.Settings.DebugMode ? "enabled" : "disabled"));
    }

    [Command("debugmode")]
    public void DebugMode(bool value)
    {
        Program.Settings.DebugMode = value;
        Log("Debug mode is " + (Program.Settings.DebugMode ? "enabled" : "disabled"));
    }

    private void Log(string message, [CallerMemberName] string callerName = "")
    {
        Logger.Log(new LogMessage(message, callerName, MessageLevel.Info));
    }
}

public static class LoremIpsumGenerator
{
    public static string GenerateText(int length)
    {
        string loremIpsumText = File.ReadAllText("loremipsum.txt"); //Read all the text from the file
        int nextDotIndex = loremIpsumText.IndexOf('.', length); //Find the next dot position
        int toIndex = nextDotIndex + 1; //Index of the next dot + 1
        if (toIndex > loremIpsumText.Length) //Check if the result exceeds the string length
            toIndex = loremIpsumText.Length;
        string result = loremIpsumText[..toIndex]; //Substring from 0 to toIndex
        return result;
    }
}