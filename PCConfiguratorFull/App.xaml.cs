using MauiApp3.Services;
using MauiApp3.View;

namespace MauiApp3
{
    public partial class App : Application
    {
        private static DatabaseService _database;

        
        public static DatabaseService Database => _database ??= new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "pcconfig.db"));

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());

            // Гарантируем, что таблицы создаются при старте приложения
            Task.Run(async () => await Database.InitAsync());
        }
    }
}
