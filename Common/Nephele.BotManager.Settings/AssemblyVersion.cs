namespace Nephele.BotManager.Settings;

public static class AssemblyVersion
{
    public static string FullVersion
    {
        get
        {
            return ThisAssembly.Git.Commit;
        }
    }
}