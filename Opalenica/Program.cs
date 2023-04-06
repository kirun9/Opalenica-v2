// Copyright (c) Krystian Pawe³ek from PKMK. All rights reserved.

namespace Opalenica;

using System;
using System.Reflection;
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

    static ILogger Logger { get; set; } = ServiceProvider.GetService<ILogger>();

    [STAThread]
    private static void Main()
    {
        CommandManager.Initialize(Settings);
        ServiceCollection.AddSingleton(typeof(ILogger), new MultiLogger());
        ServiceProvider.GetRequiredService<ILogger>().Log(new LogMessage($"Starting Opalenica { Assembly.GetAssembly(typeof(Program)).GetName().Version } ...", LOG_SOURCE, MessageLevel.Info, "Program"));

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += Application_ThreadException;
        Application.Run(new MainWindow(ServiceProvider));
        //Console.ReadLine();
    }

    private static void Application_ThreadException(Object sender, ThreadExceptionEventArgs e)
    {
        Console.WriteLine(e.Exception.ToString());
    }
}

public class BasicCommandsModule : ModuleBase<ICommandContext>
{
    [Command("exit")]
    [Alias("koniec")]
    [Alias("wyjœcie")]
    [Alias("wyjscie")]
    [ChainedCommand]
    [RepeatCommand(Count = 2)]
    [BreakChainedCommands]
    public void Exit()
    {
        Console.WriteLine("Exiting program ...");
        Application.ExitThread();
        Environment.Exit(0);
    }

    [Command("debugmode")]
    public void DebugMode()
    {
        Program.Settings.DebugMode = !Program.Settings.DebugMode;
        Console.WriteLine("Debug mode is " + (Program.Settings.DebugMode ? "enabled" : "disabled"));
    }

    [Command("debugmode")]
    public void DebugMode(bool value)
    {
        Program.Settings.DebugMode = value;
        Console.WriteLine("Debug mode is " + (Program.Settings.DebugMode ? "enabled" : "disabled"));
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