using CommunityToolkit.Mvvm.ComponentModel;


namespace MauiApp3.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _title = "Конфигуратор ПК";
    }

}
