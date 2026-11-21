
namespace stms;

public class Logging
{
    public static void Log(string? message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        string dataPath = Path.Combine(Environment.CurrentDirectory, "data");

        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
            File.SetAttributes(dataPath, FileAttributes.Hidden | FileAttributes.System);
        }
        
        File.AppendAllText(
            Path.Combine(dataPath, $"{DateTime.Now:yyyy-MM-dd}"),
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}{Environment.NewLine}"
        );
    }
}