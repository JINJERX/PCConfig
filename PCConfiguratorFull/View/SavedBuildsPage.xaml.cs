using MauiApp3.Models;
using MauiApp3.Services;
using System.Linq;

namespace MauiApp3.View
{
    public partial class SavedBuildsPage : ContentPage
    {
        public SavedBuildsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadBuilds();
        }

        private async Task LoadBuilds()
        {
            var builds = await App.Database.GetAllBuildsAsync();
            BuildsListView.ItemsSource = builds;
        }

        private async void OnDeleteBuildClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is PCBuild buildToDelete)
            {
                bool confirm = await DisplayAlert("������� ������",
                                                  $"�� �������, ��� ������ ������� ������ \"{buildToDelete.Name}\"?",
                                                  "��", "������");
                if (confirm)
                {
                    await App.Database.DeleteBuildAsync(buildToDelete);
                    await LoadBuilds(); // ��������� ������
                }
            }
        }

        private async void OnViewDetailsClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is PCBuild build)
            {
                string details = $"CPU: {build.CPU}\n" +
                                 $"GPU: {build.GPU}\n" +
                                 $"RAM: {build.RAM}\n" +
                                 $"Storage: {build.Storage}\n" +
                                 $"PSU: {build.PSU}\n" +
                                 $"Case: {build.Case}\n" +
                                 $"Motherboard: {build.Motherboard}\n" +
                                 $"Cooling: {build.Cooling}";

                await DisplayAlert($"������: {build.Name}", details, "��");
            }
        }

        private async void OnExportPdfClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var build = button?.CommandParameter as PCBuild;

            if (build != null)
            {
                var filePath = await PdfService.ExportBuildToPdfAsync(build);
                await DisplayAlert("PDF �������", $"���� �������:\n{filePath}", "��");

                // ����� ������� ��� �������������, ���� ������
                // await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(filePath) });
            }
        }

    }
}
