using MauiApp3.Services;
using MauiApp3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MauiApp3.View
{
    public partial class ConfiguratorPage : ContentPage
    {
        private readonly ComponentService _componentService;

        private readonly SimplifiedFilterService _filterService;


        public ConfiguratorPage()
        {
            InitializeComponent();
            _componentService = new ComponentService(App.Database);
            _filterService = new SimplifiedFilterService(App.Database);
        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _componentService.LoadTestDataAsync();  // ����� ������
            await LoadComponents(); // ��������� ���������� ����� �����
        }


        private async Task LoadComponents()
        {
            bool simplifiedMode = SimplifiedModeSwitch.IsToggled;

            // ������� ������ ������ � Picker ����� ��������� �����
            CPUSelector.ItemsSource = null;
            GPUSelector.ItemsSource = null;
            RAMSelector.ItemsSource = null;
            StorageSelector.ItemsSource = null;
            PSUSelector.ItemsSource = null;
            CaseSelector.ItemsSource = null;
            MotherboardSelector.ItemsSource = null;
            CoolingSelector.ItemsSource = null;

            if (!simplifiedMode)
            {
                // ������� �����: ������ ��� ����������
                var cpus = await App.Database.GetComponentsByTypeAsync("CPU");
                CPUSelector.ItemsSource = cpus.Select(c => c.Name).ToList();

                var gpus = await App.Database.GetComponentsByTypeAsync("GPU");
                GPUSelector.ItemsSource = gpus.Select(c => c.Name).ToList();

                var rams = await App.Database.GetComponentsByTypeAsync("RAM");
                RAMSelector.ItemsSource = rams.Select(c => c.Name).ToList();

                var storages = await App.Database.GetComponentsByTypeAsync("Storage");
                StorageSelector.ItemsSource = storages.Select(c => c.Name).ToList();

                var psus = await App.Database.GetComponentsByTypeAsync("PSU");
                PSUSelector.ItemsSource = psus.Select(c => c.Name).ToList();

                var cases = await App.Database.GetComponentsByTypeAsync("Case");
                CaseSelector.ItemsSource = cases.Select(c => c.Name).ToList();

                var motherboards = await App.Database.GetComponentsByTypeAsync("Motherboard");
                MotherboardSelector.ItemsSource = motherboards.Select(c => c.Name).ToList();

                var coolings = await App.Database.GetComponentsByTypeAsync("Cooling");
                CoolingSelector.ItemsSource = coolings.Select(c => c.Name).ToList();
            }
            else
            {
                // ���������� �����: ������ ������ ����������� ����������
                var selectedComponents = new List<Component>();
                // �� ������ ������ ��� ��������� ���� ������ ���� ������������, ������� ��� �������

                var cpus = await _filterService.GetCompatibleComponentsAsync(selectedComponents, "CPU");
                CPUSelector.ItemsSource = cpus.Select(c => c.Name).ToList();

                var gpus = await _filterService.GetCompatibleComponentsAsync(selectedComponents, "GPU");
                GPUSelector.ItemsSource = gpus.Select(c => c.Name).ToList();

                var rams = await _filterService.GetCompatibleComponentsAsync(selectedComponents, "RAM");
                if (SimplifiedModeSwitch.IsToggled && MotherboardSelector.SelectedItem != null)
                {
                    var selectedMBName = MotherboardSelector.SelectedItem.ToString();
                    var selectedMB = await App.Database.GetComponentByNameAsync(selectedMBName);
                    rams = rams.Where(ram => ram.RamType == selectedMB.RamType).ToList();
                }
                RAMSelector.ItemsSource = rams.Select(c => c.Name).ToList();


                var storages = await _filterService.GetCompatibleComponentsAsync(selectedComponents, "Storage");
                StorageSelector.ItemsSource = storages.Select(c => c.Name).ToList();

                var psus = await App.Database.GetComponentsByTypeAsync("PSU");
                if (SimplifiedModeSwitch.IsToggled && GPUSelector.SelectedItem != null)
                {
                    var selectedGPUName = GPUSelector.SelectedItem.ToString();
                    var selectedGPU = await App.Database.GetComponentByNameAsync(selectedGPUName);
                    psus = psus.Where(psu => psu.Wattage >= selectedGPU.RecommendedPSUWattage).ToList();
                }
                PSUSelector.ItemsSource = psus.Select(c => c.Name).ToList();


                var cases = await _filterService.GetCompatibleComponentsAsync(selectedComponents, "Case");
                CaseSelector.ItemsSource = cases.Select(c => c.Name).ToList();

                var motherboards = await _filterService.GetCompatibleComponentsAsync(selectedComponents, "Motherboard");
                if (SimplifiedModeSwitch.IsToggled && CPUSelector.SelectedItem != null)
                {
                    var selectedCPUName = CPUSelector.SelectedItem.ToString();
                    var selectedCPU = await App.Database.GetComponentByNameAsync(selectedCPUName);
                    motherboards = motherboards.Where(mb => mb.Socket == selectedCPU.Socket).ToList();
                }
                MotherboardSelector.ItemsSource = motherboards.Select(c => c.Name).ToList();


                var coolings = await _filterService.GetCompatibleComponentsAsync(selectedComponents, "Cooling");
                CoolingSelector.ItemsSource = coolings.Select(c => c.Name).ToList();
            }

            if (SimplifiedModeSwitch.IsToggled)
            {
                OnCPUChanged(null, null);
                OnGPUChanged(null, null);
                OnRAMChanged(null, null);
                OnStorageChanged(null, null);
                OnPSUChanged(null, null);
                OnCaseChanged(null, null);
                OnMotherboardChanged(null, null);
                OnCoolingChanged(null, null);
            }

        }



        private async void OnSaveBuildClicked(object sender, EventArgs e)
        {
            string cpu = CPUSelector.SelectedItem?.ToString();
            string gpu = GPUSelector.SelectedItem?.ToString();
            string ram = RAMSelector.SelectedItem?.ToString();
            string storage = StorageSelector.SelectedItem?.ToString();
            string psu = PSUSelector.SelectedItem?.ToString();
            string pcCase = CaseSelector.SelectedItem?.ToString();
            string motherboard = MotherboardSelector.SelectedItem?.ToString();
            string cooling = CoolingSelector.SelectedItem?.ToString();

            if (cpu == null || gpu == null || ram == null || storage == null ||
                psu == null || pcCase == null || motherboard == null || cooling == null)
            {
                await DisplayAlert("������", "�������� ��� �������������!", "OK");
                return;
            }

            // ������ �������� ������
            string buildName = await DisplayPromptAsync("�������� ������", "������� ��������:", "OK", "������", "��� ������");

            if (string.IsNullOrWhiteSpace(buildName))
            {
                buildName = "��� ��������";
            }

            var newBuild = new PCBuild
            {
                Name = buildName, // ��������� ��� ������
                CPU = cpu,
                GPU = gpu,
                RAM = ram,
                Storage = storage,
                PSU = psu,
                Case = pcCase,
                Motherboard = motherboard,
                Cooling = cooling
            };

            await App.Database.SaveBuildAsync(newBuild);

            await DisplayAlert("���������", $"������ \"{buildName}\" ���������!", "OK");
        }
        public class CompatibilityService
        {
            public List<string> GetCompatibilityIssues(List<Component> selectedComponents)
            {
                var issues = new List<string>();

                var cpu = selectedComponents.FirstOrDefault(c => c.Type == "CPU");
                var motherboard = selectedComponents.FirstOrDefault(c => c.Type == "Motherboard");
                var ram = selectedComponents.FirstOrDefault(c => c.Type == "RAM");
                var gpu = selectedComponents.FirstOrDefault(c => c.Type == "GPU");
                var storage = selectedComponents.FirstOrDefault(c => c.Type == "Storage");
                var psu = selectedComponents.FirstOrDefault(c => c.Type == "PSU");
                var cooling = selectedComponents.FirstOrDefault(c => c.Type == "Cooling");
                var caseComponent = selectedComponents.FirstOrDefault(c => c.Type == "Case");

                if (cpu != null && motherboard != null && cpu.Socket != motherboard.Socket)
                    issues.Add($"CPU ����� ({cpu.Socket}) �� ��������� � ������� ����������� ����� ({motherboard.Socket})");

                if (ram != null && motherboard != null)
                {
                    if (ram.RamType != motherboard.RamType)
                        issues.Add($"��� RAM ({ram.RamType}) �� ��������� � ����������� ������ ({motherboard.RamType})");

                    if (ram.RamCapacity > motherboard.MaxRam)
                        issues.Add($"����� RAM ({ram.RamCapacity} ��) ��������� �������� ����������� ����� ({motherboard.MaxRam} ��)");

                    if (ram.RamModules > motherboard.RamSlots)
                        issues.Add($"���������� ������� RAM ({ram.RamModules}) ��������� ���������� ������ ({motherboard.RamSlots})");
                }

                if (gpu != null && motherboard != null)
                {
                    if (gpu.GpuLength > motherboard.GPUMaxLength)
                        issues.Add($"����� ���������� ({gpu.GpuLength} ��) ��������� ���������� ��� ����������� ����� ({motherboard.GPUMaxLength} ��)");
                }

                if (storage != null && motherboard != null)
                {
                    //if (!motherboard.StorageInterfaces.Contains(storage.ConnectionType))
                    //    issues.Add($"����������� ����� �� ������������ ��������� ���������� ({storage.ConnectionType})");
                }

                if (psu != null)
                {
                    int totalPower = selectedComponents.Sum(c => c.PowerConsumption);
                    if (totalPower > psu.Wattage)
                        issues.Add($"������������ �������� ({totalPower} ��) ��������� �������� ����� ������� ({psu.Wattage} ��)");
                }

                if (cooling != null && cpu != null && caseComponent != null)
                {
                    if (!cooling.SocketSupport.Contains(cpu.Socket))
                        issues.Add($"���������� �� ������������ ����� ���������� ({cpu.Socket})");

                    if (cpu.TDP > cooling.MaxTDP)
                        issues.Add($"TDP ���������� ({cpu.TDP} ��) ��������� ����� ���������� ({cooling.MaxTDP} ��)");

                    if (cooling.CoolingClearance > caseComponent.CoolingClearance)
                        issues.Add($"���������� �� ���������� � ������ (��������� {cooling.CoolingClearance} ��, �������� {caseComponent.CoolingClearance} ��)");
                }

                if (caseComponent != null && motherboard != null)
                {
                    if (!caseComponent.SupportedFormFactors.Contains(motherboard.FormFactor))
                        issues.Add($"����-������ ����������� ����� ({motherboard.FormFactor}) �� �������������� ��������");
                }

                if (caseComponent != null && gpu != null)
                {
                    if (gpu.GpuLength > caseComponent.GPUMaxLength)
                        issues.Add($"����� ���������� ({gpu.GpuLength} ��) ��������� �������� ������� ({caseComponent.GPUMaxLength} ��)");
                }

                return issues;
            }

            
            public bool CheckCPUCompatibility(Component cpu, Component motherboard)
            {
                if (cpu.Socket != motherboard.Socket)
                {
                    Console.WriteLine($"��������� {cpu.Name} {cpu.Socket} �� ��������� � ����������� ������ {motherboard.Name} {motherboard.Socket}. ���������������� �����.");
                    return false;
                }
                return true;
            }

            
            public bool CheckRAMCompatibility(Component ram, Component motherboard)
            {
                if (ram.RamType != motherboard.RamType)
                {
                    Console.WriteLine($"����������� ������ {ram.Name} �� ���������� � ����������� ������ {motherboard.Name}. ���������������� ��� ������.");
                    return false;
                }

                if (ram.RamCapacity > motherboard.MaxRam)
                {
                    Console.WriteLine($"����������� ������ {ram.Name} ��������� ������������ �������������� ������ ��� ����������� ����� {motherboard.Name}. ��������: {motherboard.MaxRam} ��.");
                    return false;
                }

                if (ram.RamModules > motherboard.RamSlots)
                {
                    Console.WriteLine($"���������� ������ ����������� ������ {ram.Name} ��������� ���������� ������ �� ����������� ����� {motherboard.Name}. �����: {motherboard.RamSlots}.");
                    return false;
                }

                return true;
            }

            
            public bool CheckGPUCompatibility(Component gpu, Component motherboard)
            {
                //if (gpu.SlotType != "PCIe x16")
                //{
                //    Console.WriteLine($"���������� {gpu.Name} ������� ���� PCIe x16, �� ����������� ����� {motherboard.Name} �� ������������ ���.");
                //    return false;
                //}

                // ������������� ��������� �������������� ����� ���������� (� ������ � �������� � �����������)
                if (gpu.GpuLength > motherboard.GPUMaxLength)
                {
                    Console.WriteLine($"���������� {gpu.Name} ������� ������� ��� ����������� ����� {motherboard.Name}. ������������ �����: {motherboard.GPUMaxLength} ��.");
                    return false;
                }

                return true;
            }

            
            public bool CheckStorageCompatibility(Component storage, Component motherboard)
            {
                if (!motherboard.StorageInterfaces.Contains(storage.ConnectionType))
                {
                    Console.WriteLine($"���������� {storage.Name} �� ��������� � ����������� ������ {motherboard.Name}. ���������������� ��������� �����������.");
                    return false;
                }

                return true;
            }

            
            public bool CheckPSUCompatibility(Component psu, List<Component> selectedComponents)
            {
                // ������� ����������� �������� ��� ���� �����������
                int totalPower = 0;
                foreach (var component in selectedComponents)
                {
                    totalPower += component.PowerConsumption;
                }

                if (totalPower > psu.Wattage)
                {
                    Console.WriteLine($"���� ������� {psu.Name} �� ���������� ������ ��� ���� ������. ���������: {totalPower}W, � �������� ����� �������: {psu.Wattage}W.");
                    return false;
                }

                return true;
            }

            // �������� ������������� ������� ���������� � ����������� � ��������
            public bool CheckCoolingCompatibility(Component cooling, Component cpu, Component caseComponent)
            {
                // �������� ��������� ������ ����������
                if (!cooling.SocketSupport.Contains(cpu.Socket))
                {
                    Console.WriteLine($"������� ���������� {cooling.Name} �� ���������� � ����������� {cpu.Name}. �� �������������� �����.");
                    return false;
                }

                // �������� ������������� TDP ������� ����������
                if (cpu.TDP > cooling.MaxTDP)
                {
                    Console.WriteLine($"������� ���������� {cooling.Name} �� ��������� � ����������� {cpu.Name}. �������������� ���������� {cpu.TDP}W ��������� ����������� �������������� ����������� {cooling.MaxTDP}W.");
                    return false;
                }

                // �������� ������������� � �������� �� ��������
                if (cooling.CoolingClearance > caseComponent.CoolingClearance)
                {
                    Console.WriteLine($"������� ���������� {cooling.Name} �� ���������� � ������ {caseComponent.Name}. �� ������� ����� ��� ���������.");
                    return false;
                }

                return true;
            }

            // �������� ������������� ������� � ����������� � ����������� ������
            public bool CheckCaseCompatibility(Component caseComponent, Component motherboard, Component gpu)
            {
                // �������� ������������� � ����-�������� ����������� �����
                if (!caseComponent.SupportedFormFactors.Contains(motherboard.FormFactor))
                {
                    Console.WriteLine($"������ {caseComponent.Name} �� ��������� � ����������� ������ {motherboard.Name}. �� �������������� ����-������.");
                    return false;
                }

                // ��������, ���������� �� ���������� � ������
                if (gpu.GpuLength > caseComponent.GPUMaxLength)
                {
                    Console.WriteLine($"���������� {gpu.Name} �� ���������� � ������ {caseComponent.Name}. ������������ ����� ����������: {caseComponent.GPUMaxLength} ��.");
                    return false;
                }

                return true;
            }

            // ����� �������� ������������� ���� ��������� �����������
            public bool CheckAllComponentsCompatibility(List<Component> selectedComponents)
            {
                var cpu = selectedComponents.FirstOrDefault(c => c.Type == "CPU");
                var motherboard = selectedComponents.FirstOrDefault(c => c.Type == "Motherboard");
                var ram = selectedComponents.FirstOrDefault(c => c.Type == "RAM");
                var gpu = selectedComponents.FirstOrDefault(c => c.Type == "GPU");
                var storage = selectedComponents.FirstOrDefault(c => c.Type == "Storage");
                var psu = selectedComponents.FirstOrDefault(c => c.Type == "PSU");
                var cooling = selectedComponents.FirstOrDefault(c => c.Type == "Cooling");
                var caseComponent = selectedComponents.FirstOrDefault(c => c.Type == "Case");

                // �������� ������������� ���� �����������
                if (cpu != null && motherboard != null && !CheckCPUCompatibility(cpu, motherboard)) return false;
                if (ram != null && motherboard != null && !CheckRAMCompatibility(ram, motherboard)) return false;
                //if (gpu != null && motherboard != null && !CheckGPUCompatibility(gpu, motherboard)) return false;
                //if (storage != null && motherboard != null && !CheckStorageCompatibility(storage, motherboard)) return false;
                if (psu != null && !CheckPSUCompatibility(psu, selectedComponents)) return false;
                if (cooling != null && cpu != null && caseComponent != null && !CheckCoolingCompatibility(cooling, cpu, caseComponent)) return false;
                if (caseComponent != null && motherboard != null && gpu != null && !CheckCaseCompatibility(caseComponent, motherboard, gpu)) return false;

                return true; // ��� ����������
            }
        }

        private async void OnCheckCompatibilityClicked(object sender, EventArgs e)
        {
            var selectedComponents = new List<Component>
    {
        await GetSelectedComponent("CPU", CPUSelector.SelectedItem?.ToString()),
        await GetSelectedComponent("GPU", GPUSelector.SelectedItem?.ToString()),
        await GetSelectedComponent("RAM", RAMSelector.SelectedItem?.ToString()),
        await GetSelectedComponent("Storage", StorageSelector.SelectedItem?.ToString()),
        await GetSelectedComponent("PSU", PSUSelector.SelectedItem?.ToString()),
        await GetSelectedComponent("Case", CaseSelector.SelectedItem?.ToString()),
        await GetSelectedComponent("Motherboard", MotherboardSelector.SelectedItem?.ToString()),
        await GetSelectedComponent("Cooling", CoolingSelector.SelectedItem?.ToString())
    };


            var motherboard = selectedComponents.FirstOrDefault(c => c.Type == "Motherboard");
            var gpu = selectedComponents.FirstOrDefault(c => c.Type == "GPU");

            Console.WriteLine($"���������: {motherboard?.Name}, GPUMaxLength = {motherboard?.GPUMaxLength}");
            Console.WriteLine($"����������: {gpu?.Name}, Length = {gpu?.GpuLength}");

            var compatibilityService = new CompatibilityService();
            var issues = compatibilityService.GetCompatibilityIssues(selectedComponents.Where(c => c != null).ToList());

            if (issues.Count == 0)
            {
                await DisplayAlert("�������������", "��� ���������� ����������!", "��");
            }
            else
            {
                string message = "���������� ��������� ��������:\n\n" + string.Join("\n� ", issues);
                await DisplayAlert("������ �������������", message, "��");
            }
        }


        private async Task<Component> GetSelectedComponent(string componentType, string componentName)
        {
            if (string.IsNullOrWhiteSpace(componentName))
                return null;

            Console.WriteLine($"[DEBUG] �������� �� ���������: \"{componentName}\"");

            var component = await App.Database.GetComponentByNameAsync(componentName);

            if (component == null)
                Console.WriteLine($"[DEBUG] ��������� � ������ \"{componentName}\" �� ������ � ����");
            else
                Console.WriteLine($"[DEBUG] ������ ���������: {component.Name} (ID = {component.Id})");

            return component;
        }

        private async void OnComponentChanged(object sender, EventArgs e)
        {
            await UpdateTotalPriceAsync();
        }

        private async void OnGoToSavedBuildsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedBuildsPage());
        }

        private async Task UpdateTotalPriceAsync()
        {
            var selectedNames = new List<string>
            {
                CPUSelector.SelectedItem?.ToString(),
                GPUSelector.SelectedItem?.ToString(),
                RAMSelector.SelectedItem?.ToString(),
                StorageSelector.SelectedItem?.ToString(),
                PSUSelector.SelectedItem?.ToString(),
                CaseSelector.SelectedItem?.ToString(),
                MotherboardSelector.SelectedItem?.ToString(),
                CoolingSelector.SelectedItem?.ToString()
            };

            decimal total = 0;
            string currency = "MDL";

            foreach (var name in selectedNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var component = await App.Database.GetComponentByNameAsync(name);
                if (component != null)
                {
                    var price = await App.Database.GetPriceByComponentIdAsync(component.Id);
                    if (price != null)
                    {
                        total += price.Value;
                        currency = price.Currency ?? "MDL";
                    }
                }
            }

            //TotalPriceLabel.Text = $"����� ���������: {total} {currency}";
        }
        private async void SimplifiedModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            await LoadComponents();
        }

        private async void OnCPUChanged(object sender, EventArgs e)
        {
            if (!SimplifiedModeSwitch.IsToggled)
                return; 

            var selectedCPUName = CPUSelector.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCPUName))
                return;

            var cpu = await App.Database.GetComponentByNameAsync(selectedCPUName);
            if (cpu == null)
                return;

            
            var allMotherboards = await App.Database.GetComponentsByTypeAsync("Motherboard");

            
            var compatibleMotherboards = allMotherboards.Where(mb => mb.Socket == cpu.Socket).ToList();

            MotherboardSelector.ItemsSource = compatibleMotherboards.Select(mb => mb.Name).ToList();
        }

        private async void OnMotherboardChanged(object sender, EventArgs e)
        {
            if (!SimplifiedModeSwitch.IsToggled)
                return;

            var selectedMotherboardName = MotherboardSelector.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedMotherboardName))
                return;

            var motherboard = await App.Database.GetComponentByNameAsync(selectedMotherboardName);
            if (motherboard == null)
                return;

            
            var cpus = await App.Database.GetComponentsByTypeAsync("CPU");
            var compatibleCPUs = cpus
                .Where(cpu => cpu.Socket == motherboard.Socket)
                .Select(cpu => cpu.Name)
                .ToList();
            CPUSelector.ItemsSource = compatibleCPUs;

            
            var rams = await App.Database.GetComponentsByTypeAsync("RAM");
            var compatibleRAMs = rams
                .Where(ram => ram.RamType == motherboard.RamType)
                .Select(ram => ram.Name)
                .ToList();
            RAMSelector.ItemsSource = compatibleRAMs;

            
            var storages = await App.Database.GetComponentsByTypeAsync("Storage");
            var compatibleStorages = storages
                .Where(storage => !string.IsNullOrEmpty(motherboard.StorageInterfaces) &&
                                  motherboard.StorageInterfaces.Split(',').Any(i => storage.ConnectionType.Contains(i.Trim())))
                .Select(storage => storage.Name)
                .ToList();
            StorageSelector.ItemsSource = compatibleStorages;
        }


        private async void OnGPUChanged(object sender, EventArgs e)
        {
            if (!SimplifiedModeSwitch.IsToggled)
                return;

            var selectedGPUName = GPUSelector.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedGPUName))
                return;

            var gpu = await App.Database.GetComponentByNameAsync(selectedGPUName);
            if (gpu == null)
                return;

            var allPSUs = await App.Database.GetComponentsByTypeAsync("PSU");

            
            var compatiblePSUs = allPSUs.Where(psu => psu.Wattage >= gpu.RecommendedPSUWattage).ToList();

            PSUSelector.ItemsSource = compatiblePSUs.Select(psu => psu.Name).ToList();
        }

        private async void OnCaseChanged(object sender, EventArgs e)
        {
            if (!SimplifiedModeSwitch.IsToggled)
                return;

            var selectedCaseName = CaseSelector.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCaseName))
                return;

            var pcCase = await App.Database.GetComponentByNameAsync(selectedCaseName);
            if (pcCase == null)
                return;

            var allGPUs = await App.Database.GetComponentsByTypeAsync("GPU");

            var compatibleGPUs = allGPUs.Where(gpu => gpu.GpuLength <= pcCase.GPUMaxLength).ToList();

            GPUSelector.ItemsSource = compatibleGPUs.Select(gpu => gpu.Name).ToList();
        }

        private async void OnCoolingChanged(object sender, EventArgs e)
        {
            if (!SimplifiedModeSwitch.IsToggled)
                return;

            var selectedCoolingName = CoolingSelector.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCoolingName))
                return;

            var cooling = await App.Database.GetComponentByNameAsync(selectedCoolingName);
            if (cooling == null)
                return;

            var allCPUs = await App.Database.GetComponentsByTypeAsync("CPU");

            var compatibleCPUs = allCPUs.Where(cpu =>
                cooling.SocketSupport.Contains(cpu.Socket) && cpu.TDP <= cooling.MaxTDP
            ).ToList();

            CPUSelector.ItemsSource = compatibleCPUs.Select(cpu => cpu.Name).ToList();
        }

        private async void OnPSUChanged(object sender, EventArgs e)
        {
            if (!SimplifiedModeSwitch.IsToggled)
                return;

            var selectedPSUName = PSUSelector.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedPSUName))
                return;

            var psu = await App.Database.GetComponentByNameAsync(selectedPSUName);
            if (psu == null)
                return;

            var allComponents = await App.Database.GetAllComponentsAsync();
            var compatible = allComponents
                .Where(c => c.Type != "PSU") 
                .GroupBy(c => c.Type)
                .Select(g => g.OrderBy(c => c.PowerConsumption).FirstOrDefault()) // �� ������������ �����������
                .Where(c => c != null)
                .ToList();

            var totalRequiredPower = compatible.Sum(c => c.PowerConsumption);

            if (totalRequiredPower > psu.Wattage)
            {
                await DisplayAlert("��������", $"��������� �� ����� ���� ������� ������ (��������� ������� {totalRequiredPower} ��)", "OK");
            }
        }

        private async void OnRAMChanged(object sender, EventArgs e)
        {
            if (!SimplifiedModeSwitch.IsToggled)
                return;

            var selectedMotherboard = await GetSelectedComponent("Motherboard", MotherboardSelector.SelectedItem?.ToString());
            var allRAM = await App.Database.GetComponentsByTypeAsync("RAM");

            var compatibleRAM = allRAM
                .Where(ram =>
                    selectedMotherboard == null ||
                    (ram.RamType == selectedMotherboard.RamType &&
                     ram.RamCapacity <= selectedMotherboard.MaxRam &&
                     ram.RamModules <= selectedMotherboard.RamSlots))
                .Select(ram => ram.Name)
                .ToList();

            RAMSelector.ItemsSource = compatibleRAM;
        }

        private async void OnStorageChanged(object sender, EventArgs e)
        {
            if (!SimplifiedModeSwitch.IsToggled)
                return;

            var selectedMotherboard = await GetSelectedComponent("Motherboard", MotherboardSelector.SelectedItem?.ToString());
            var allStorage = await App.Database.GetComponentsByTypeAsync("Storage");

            var compatibleStorage = allStorage
                .Where(storage =>
                    selectedMotherboard == null ||
                    string.IsNullOrEmpty(storage.ConnectionType) ||
                    selectedMotherboard.StorageInterfaces?.Contains(storage.ConnectionType) == true)
                .Select(s => s.Name)
                .ToList();

            StorageSelector.ItemsSource = compatibleStorage;
        }

    }
}

