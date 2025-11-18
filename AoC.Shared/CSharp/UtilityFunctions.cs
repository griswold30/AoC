namespace AoC.Shared
{
    public class UtilityFunctions
    {
        public static string? FindSolutionDirectory()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !dir.GetFiles("*.sln").Any())
            {
                dir = dir.Parent;
            }
            return dir?.FullName;
        }
    }
}
