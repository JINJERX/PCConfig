using MauiApp3.Models;
using System;
using Microsoft.Maui.Controls;

namespace MauiApp3.View
{
    public partial class AddComponentPage : ContentPage
    {
        public AddComponentPage()
        {
            InitializeComponent();
        }

        private void OnTypeChanged(object sender, EventArgs e)
        {
            string selectedType = ComponentTypePicker.SelectedItem?.ToString();

            CPUFields.IsVisible = selectedType == "CPU";
            GPUFields.IsVisible = selectedType == "GPU";
            RAMFields.IsVisible = selectedType == "RAM";
            StorageFields.IsVisible = selectedType == "Storage";
            PSUFields.IsVisible = selectedType == "PSU";
            CaseFields.IsVisible = selectedType == "Case";
            MotherboardFields.IsVisible = selectedType == "Motherboard";
            CoolingFields.IsVisible = selectedType == "Cooling";
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var type = ComponentTypePicker.SelectedItem?.ToString();

            var component = new Component
            {
                Name = NameEntry.Text,
                Type = type,
                PowerConsumption = int.TryParse(PowerEntry.Text, out int power) ? power : 0
            };

            // Типоспецифичные поля
            switch (type)
            {
                case "CPU":
                    component.Socket = SocketEntry.Text;
                    component.TDP = int.TryParse(TDPEntry.Text, out int tdp) ? tdp : 0;
                    break;

                case "GPU":
                    component.GpuLength = int.TryParse(GpuLengthEntry.Text, out int gl) ? gl : 0;
                    component.RecommendedPSUWattage = int.TryParse(RecommendedPSUEntry.Text, out int rpsu) ? rpsu : 0;
                    break;

                case "RAM":
                    component.RamType = RamTypeEntry.Text;
                    component.RamCapacity = int.TryParse(RamCapacityEntry.Text, out int ramCap) ? ramCap : 0;
                    component.RamFrequency = int.TryParse(RamFreqEntry.Text, out int ramFreq) ? ramFreq : 0;
                    component.RamModules = int.TryParse(RamModulesEntry.Text, out int ramMod) ? ramMod : 0;
                    break;

                case "Storage":
                    component.ConnectionType = ConnectionTypeEntry.Text;
                    component.StorageCapacity = int.TryParse(StorageCapacityEntry.Text, out int sCap) ? sCap : 0;
                    component.StorageFormFactor = StorageFormFactorEntry.Text;
                    break;

                case "PSU":
                    component.Wattage = int.TryParse(WattageEntry.Text, out int wattage) ? wattage : 0;
                    component.EfficiencyRating = EfficiencyEntry.Text;
                    component.PSUFormFactor = PSUFormEntry.Text;
                    break;

                case "Case":
                    component.SupportedFormFactors = SupportedFormEntry.Text;
                    component.GPUMaxLength = int.TryParse(MaxGpuLengthEntry.Text, out int maxLen) ? maxLen : 0;
                    component.CoolingClearance = int.TryParse(CoolingClearanceEntry.Text, out int coolClear) ? coolClear : 0;
                    component.PSUFormFactorSupport = CasePSUFormEntry.Text;
                    break;

                case "Motherboard":
                    component.Socket = SocketEntry2.Text;
                    component.FormFactor = FormFactorEntry.Text;
                    component.Chipset = ChipsetEntry.Text;
                    component.RamType = RamTypeEntry2.Text;
                    component.RamSlots = int.TryParse(RamSlotsEntry.Text, out int ramSlots) ? ramSlots : 0;
                    component.MaxRam = int.TryParse(MaxRamEntry.Text, out int maxRam) ? maxRam : 0;
                    component.PCIESlots = int.TryParse(PCIESlotsEntry.Text, out int pcie) ? pcie : 0;
                    component.StorageInterfaces = StorageInterfacesEntry.Text;
                    component.GPUMaxLength = int.TryParse(GPUMaxLengthEntry.Text, out int gmax) ? gmax : 0;
                    break;

                case "Cooling":
                    component.CoolingType = CoolingTypeEntry.Text;
                    component.SocketSupport = SocketSupportEntry.Text;
                    component.MaxTDP = int.TryParse(MaxCoolingTDPEntry.Text, out int mtdp) ? mtdp : 0;
                    break;
            }

            await App.Database.AddComponentAsync(component);

            // Сохраняем цену (если указана)
            if (decimal.TryParse(PriceEntry.Text, out decimal priceValue))
            {
                var price = new Price
                {
                    ComponentId = component.Id,
                    Value = priceValue,
                    Currency = string.IsNullOrWhiteSpace(CurrencyEntry.Text) ? "MDL" : CurrencyEntry.Text
                };

                await App.Database.SaveOrUpdatePriceAsync(price);
            }

            await DisplayAlert("Готово", "Компонент добавлен", "OK");
            await Navigation.PopAsync();
        }
    }
}
