using MauiApp3.View;

namespace MauiApp3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGoToConfiguratorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConfiguratorPage());
        }

        private async void OnAdminPageClicked(object sender, EventArgs e)
        {
            // Переход на AdminPage
            await Navigation.PushAsync(new AdminPage());
        }
        private async void OnLanguageClicked(object sender, EventArgs e)
        {
            string selected = await DisplayActionSheet("Выберите язык", "Отмена", null,
                "🇷🇺 Русский", "🇷🇴 Română", "🇬🇧 English");

            // Ничего не делаем — только визуально
            if (selected == "🇷🇴 Română")
                await DisplayAlert("Язык", "Выбран румынский", "OK");
            else if (selected == "🇬🇧 English")
                await DisplayAlert("Language", "English selected", "OK");
        }

    }
}

