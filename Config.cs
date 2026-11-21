using System.Text;

namespace stms;

public static class Config
{
    public readonly static string? hub_url = DeHash(Environment.GetEnvironmentVariable("HUB_URL"));
    public readonly static string? file_path = DeHash(Environment.GetEnvironmentVariable("FILE_PATH"));
    public readonly static string? task_name = DeHash(Environment.GetEnvironmentVariable("TASK_NAME"));

    private static string? DeHash(string? link)
    {
        if (string.IsNullOrEmpty(link))
            return null;

        try
        {
            link = link.Replace(" ", "");

            if (link.Length % 8 != 0)
                return null;

            byte[] data = new byte[link.Length / 8];
            for (int i = 0; i < data.Length; i++)
            {
                string dataSegment = link.Substring(i * 8, 8);
                data[i] = Convert.ToByte(dataSegment, 2);
            }

            return Encoding.UTF8.GetString(data);
        }
        catch
        {
            return null;
        }
    }
}