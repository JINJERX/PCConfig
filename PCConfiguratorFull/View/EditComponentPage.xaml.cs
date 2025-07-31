using MauiApp3.Models;
using System;
using Microsoft.Maui.Controls;

namespace MauiApp3.View
{
    public partial class EditComponentPage : ContentPage
    {
        private Component _component;

        public EditComponentPage(Component component = null)
        {
            InitializeComponent();
            _component = component;

            if (_component != null)
            {
                NameEntry.Text = _component.Name;
                TypeEntry.Text = _component.Type;
                PowerConsumptionEntry.Text = _component.PowerConsumption.ToString();
                SocketEntry.Text = _component.Socket;
                TDPEntry.Text = _component.TDP.ToString();
                RamTypeEntry.Text = _component.RamType;
                RamCapacityEntry.Text = _component.RamCapacity.ToString();
                RamFrequencyEntry.Text = _component.RamFrequency.ToString();
                RamModulesEntry.Text = _component.RamModules.ToString();
                FormFactorEntry.Text = _component.FormFactor;
                ChipsetEntry.Text = _component.Chipset;
                RamSlotsEntry.Text = _component.RamSlots.ToString();
                MaxRamEntry.Text = _component.MaxRam.ToString();
                PCIESlotsEntry.Text = _component.PCIESlots.ToString();
                GpuLengthEntry.Text = _component.GpuLength.ToString();
                RecommendedPSUWattageEntry.Text = _component.RecommendedPSUWattage.ToString();
                WattageEntry.Text = _component.Wattage.ToString();
                EfficiencyRatingEntry.Text = _component.EfficiencyRating;
                PSUFormFactorEntry.Text = _component.PSUFormFactor;
                CoolingTypeEntry.Text = _component.CoolingType;
                MaxTDPEntry.Text = _component.MaxTDP.ToString();
                SocketSupportEntry.Text = _component.SocketSupport;
                DescriptionEditor.Text = _component.Description;

                if (!string.IsNullOrEmpty(_component.ImagePath))
                {
                    ComponentImage.Source = ImageSource.FromFile(_component.ImagePath);
                }

                if (_component.Price != null)
                {
                    PriceEntry.Text = _component.Price.Value.ToString();
                    CurrencyEntry.Text = _component.Price.Currency;
                }
            }
        }

        private async void OnChangeImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Выберите изображение",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    var filePath = result.FullPath;

                    // Подгружаем в UI
                    ComponentImage.Source = ImageSource.FromFile(filePath);

                    // Сохраняем путь
                    if (_component != null)
                        _component.ImagePath = filePath;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось выбрать изображение: {ex.Message}", "OK");
            }
        }


        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _component.Name = NameEntry.Text;
            _component.Type = TypeEntry.Text;
            _component.PowerConsumption = int.TryParse(PowerConsumptionEntry.Text, out var pc) ? pc : 0;
            _component.Socket = SocketEntry.Text;
            _component.TDP = int.TryParse(TDPEntry.Text, out var tdp) ? tdp : 0;
            _component.RamType = RamTypeEntry.Text;
            _component.RamCapacity = int.TryParse(RamCapacityEntry.Text, out var rc) ? rc : 0;
            _component.RamFrequency = int.TryParse(RamFrequencyEntry.Text, out var rf) ? rf : 0;
            _component.RamModules = int.TryParse(RamModulesEntry.Text, out var rm) ? rm : 0;
            _component.FormFactor = FormFactorEntry.Text;
            _component.Chipset = ChipsetEntry.Text;
            _component.RamSlots = int.TryParse(RamSlotsEntry.Text, out var rs) ? rs : 0;
            _component.MaxRam = int.TryParse(MaxRamEntry.Text, out var mr) ? mr : 0;
            _component.PCIESlots = int.TryParse(PCIESlotsEntry.Text, out var pcie) ? pcie : 0;
            _component.GpuLength = int.TryParse(GpuLengthEntry.Text, out var gl) ? gl : 0;
            _component.RecommendedPSUWattage = int.TryParse(RecommendedPSUWattageEntry.Text, out var psu) ? psu : 0;
            _component.Wattage = int.TryParse(WattageEntry.Text, out var watt) ? watt : 0;
            _component.EfficiencyRating = EfficiencyRatingEntry.Text;
            _component.PSUFormFactor = PSUFormFactorEntry.Text;
            _component.CoolingType = CoolingTypeEntry.Text;
            _component.MaxTDP = int.TryParse(MaxTDPEntry.Text, out var mtdp) ? mtdp : 0;
            _component.SocketSupport = SocketSupportEntry.Text;
            _component.Description = DescriptionEditor.Text;

            await App.Database.UpdateComponentAsync(_component);

            if (decimal.TryParse(PriceEntry.Text, out var priceValue))
            {
                var price = new Price
                {
                    ComponentId = _component.Id,
                    Value = priceValue,
                    Currency = string.IsNullOrWhiteSpace(CurrencyEntry.Text) ? "MDL" : CurrencyEntry.Text
                };

                await App.Database.SaveOrUpdatePriceAsync(price);
            }

            await DisplayAlert("Готово", "Изменения сохранены", "OK");
            await Navigation.PopAsync();
        }
    }
}
