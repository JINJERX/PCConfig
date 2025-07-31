using MauiApp3.Models;
using MauiApp3.Services;
using MauiApp3.View;
using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp3
{
    public partial class AdminPage : ContentPage
    {
        private readonly ComponentService _componentService;

        public AdminPage()
        {
            InitializeComponent();
            _componentService = new ComponentService(App.Database);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadComponentsAsync();
        }

        private async Task LoadComponentsAsync()
        {
            try
            {
                var components = await App.Database.GetComponentsAsync();
                ComponentsCollectionView.ItemsSource = components;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", "Не удалось загрузить компоненты: " + ex.Message, "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int componentId)
            {
                try
                {
                    await App.Database.DeleteComponentByIdAsync(componentId);
                    await LoadComponentsAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", "Не удалось удалить компонент: " + ex.Message, "OK");
                }
            }
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            await LoadComponentsAsync();
        }

        private async void OnAddComponentPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddComponentPage());
        }

        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Component selectedComponent)
            {
                await Navigation.PushAsync(new EditComponentPage(selectedComponent));
            }
        }

        private async void OnTypeFilterChanged(object sender, EventArgs e)
        {
            if (TypeFilterPicker.SelectedItem is string selectedType)
            {
                if (selectedType == "Все")
                {
                    ComponentsCollectionView.ItemsSource = await App.Database.GetComponentsAsync();
                }
                else
                {
                    var all = await App.Database.GetComponentsAsync();
                    ComponentsCollectionView.ItemsSource = all.Where(c => c.Type == selectedType).ToList();
                }
            }
        }

        private async void OnExportDatabaseClicked(object sender, EventArgs e)
        {
            try
            {
                var dbPath = App.Database.DatabasePath;
                var exportPath = Path.Combine(FileSystem.Current.AppDataDirectory, "ExportedDatabase.db3");
                File.Copy(dbPath, exportPath, overwrite: true);
                await DisplayAlert("Успешно", $"База данных экспортирована:\n{exportPath}", "Ок");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось экспортировать базу:\n{ex.Message}", "Ок");
            }
        }

        private async void OnCheckDuplicatesClicked(object sender, EventArgs e)
        {
            var all = await App.Database.GetComponentsAsync();
            var duplicates = all.GroupBy(c => c.Name.ToLower().Trim())
                                .Where(g => g.Count() > 1)
                                .Select(g => g.Key)
                                .ToList();

            if (duplicates.Any())
            {
                await DisplayAlert("Дубликаты найдены", "Найдены дубликаты:\n" + string.Join("\n", duplicates), "ОК");
            }
            else
            {
                await DisplayAlert("Дубликаты не найдены", "Все компоненты уникальны.", "ОК");
            }
        }

        private void OnComponentSelected(object sender, SelectionChangedEventArgs e)
        {
            // Пока не нужен
        }
    }
}
