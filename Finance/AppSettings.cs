namespace Empreendedores
{
    public class AppSettings
    {
        private static string DatabaseName = "Database.db";
        private static string DatabaseDirectory = FileSystem.AppDataDirectory;
        public static string DatabasePath = Path.Combine(DatabaseDirectory, DatabaseName);
    }
}
