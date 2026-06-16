using System.Linq;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;
using stms.Install;
using stms.Http;

namespace stms;

class Program
{
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();
    private const int SW_HIDE = 0;

    static async Task Main(string[] args)
    {
        HideConsole();
        if (args.Length == 0)
            return;
        
        string argument = args[0].ToLower();
        EnsureAdminRights();

        switch (argument)
        {
            case "--install":
                Installation.Install();
                break;

            case "--start":
                await HttpHandler.Handle();
                break;
        }

        Environment.Exit(0);
    }

    static void EnsureAdminRights()
    {
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (isAdmin) return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = Environment.ProcessPath,
            Verb = "runas",
            Arguments = string.Join(" ", Environment.GetCommandLineArgs().Skip(1)),
            UseShellExecute = true
        };

        try
        {
            Process.Start(startInfo);
        }
        catch (System.ComponentModel.Win32Exception)
        {
            Environment.Exit(0);
        }

        Environment.Exit(0);
    }

    static void HideConsole()
    {
        IntPtr handle = GetConsoleWindow();
        if (handle != IntPtr.Zero)
        {
            ShowWindow(handle, SW_HIDE);
        }
    }
}