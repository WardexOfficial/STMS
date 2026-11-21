using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;

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
        try
        {
            EnsureAdminRights();
            //HideConsole();

            if (args.Length != 0)
            {
                string argument = args[0].ToLower();
                switch (argument)
                {
                    case "--start":
                        if (Config.hub_url != null)
                            await HttpHandler.Handle(Config.hub_url);
                        break;
                }
            }
            else
            {
                Installation.Install();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.ReadKey();
        }

        Environment.Exit(0);
    }

    static void EnsureAdminRights()
    {
        try
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

            Process.Start(startInfo);
        }
        catch
        {
            throw;
        }
    }

    static void HideConsole()
    {
        try
        {
            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                ShowWindow(handle, SW_HIDE);
            }
        }
        catch
        {
            throw;
        }
    }
}