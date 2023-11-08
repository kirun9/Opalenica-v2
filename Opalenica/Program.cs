// Copyright (c) Krystian Pawe³ek from PKMK. All rights reserved.

namespace Opalenica;

using System;
using System.Reflection;
using System.Threading;

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