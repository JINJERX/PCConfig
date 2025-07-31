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
            await _componentService.LoadTestDataAsync();  // Вызов метода
            await LoadComponents(); // Загрузите компоненты после этого
        }


        private async Task LoadComponents()
        {
            bool simplifiedMode = SimplifiedModeSwitch.IsToggled;

            // Очистим старые данные в Picker перед загрузкой новых
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
                // Обычный режим: грузим все компоненты
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
                // Упрощённый режим: грузим только совместимые компоненты
                var selectedComponents = new List<Component>();
                // Ты должен будешь сам наполнять этот список теми компонентами, которые уже выбраны

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
                await DisplayAlert("Ошибка", "Выберите все комплектующие!", "OK");
                return;
            }

            // Запрос названия сборки
            string buildName = await DisplayPromptAsync("Название сборки", "Введите название:", "OK", "Отмена", "Моя сборка");

            if (string.IsNullOrWhiteSpace(buildName))
            {
                buildName = "Без названия";
            }

            var newBuild = new PCBuild
            {
                Name = buildName, // Добавляем имя сборки
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

            await DisplayAlert("Сохранено", $"Сборка \"{buildName}\" сохранена!", "OK");
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
                    issues.Add($"CPU сокет ({cpu.Socket}) не совпадает с сокетом материнской платы ({motherboard.Socket})");

                if (ram != null && motherboard != null)
                {
                    if (ram.RamType != motherboard.RamType)
                        issues.Add($"Тип RAM ({ram.RamType}) не совместим с материнской платой ({motherboard.RamType})");

                    if (ram.RamCapacity > motherboard.MaxRam)
                        issues.Add($"Объём RAM ({ram.RamCapacity} ГБ) превышает максимум материнской платы ({motherboard.MaxRam} ГБ)");

                    if (ram.RamModules > motherboard.RamSlots)
                        issues.Add($"Количество модулей RAM ({ram.RamModules}) превышает количество слотов ({motherboard.RamSlots})");
                }

                if (gpu != null && motherboard != null)
                {
                    if (gpu.GpuLength > motherboard.GPUMaxLength)
                        issues.Add($"Длина видеокарты ({gpu.GpuLength} мм) превышает допустимую для материнской платы ({motherboard.GPUMaxLength} мм)");
                }

                if (storage != null && motherboard != null)
                {
                    //if (!motherboard.StorageInterfaces.Contains(storage.ConnectionType))
                    //    issues.Add($"Материнская плата не поддерживает интерфейс накопителя ({storage.ConnectionType})");
                }

                if (psu != null)
                {
                    int totalPower = selectedComponents.Sum(c => c.PowerConsumption);
                    if (totalPower > psu.Wattage)
                        issues.Add($"Потребляемая мощность ({totalPower} Вт) превышает мощность блока питания ({psu.Wattage} Вт)");
                }

                if (cooling != null && cpu != null && caseComponent != null)
                {
                    if (!cooling.SocketSupport.Contains(cpu.Socket))
                        issues.Add($"Охлаждение не поддерживает сокет процессора ({cpu.Socket})");

                    if (cpu.TDP > cooling.MaxTDP)
                        issues.Add($"TDP процессора ({cpu.TDP} Вт) превышает лимит охлаждения ({cooling.MaxTDP} Вт)");

                    if (cooling.CoolingClearance > caseComponent.CoolingClearance)
                        issues.Add($"Охлаждение не помещается в корпус (требуется {cooling.CoolingClearance} мм, доступно {caseComponent.CoolingClearance} мм)");
                }

                if (caseComponent != null && motherboard != null)
                {
                    if (!caseComponent.SupportedFormFactors.Contains(motherboard.FormFactor))
                        issues.Add($"Форм-фактор материнской платы ({motherboard.FormFactor}) не поддерживается корпусом");
                }

                if (caseComponent != null && gpu != null)
                {
                    if (gpu.GpuLength > caseComponent.GPUMaxLength)
                        issues.Add($"Длина видеокарты ({gpu.GpuLength} мм) превышает максимум корпуса ({caseComponent.GPUMaxLength} мм)");
                }

                return issues;
            }

            
            public bool CheckCPUCompatibility(Component cpu, Component motherboard)
            {
                if (cpu.Socket != motherboard.Socket)
                {
                    Console.WriteLine($"Процессор {cpu.Name} {cpu.Socket} не совместим с материнской платой {motherboard.Name} {motherboard.Socket}. Неподдерживаемый сокет.");
                    return false;
                }
                return true;
            }

            
            public bool CheckRAMCompatibility(Component ram, Component motherboard)
            {
                if (ram.RamType != motherboard.RamType)
                {
                    Console.WriteLine($"Оперативная память {ram.Name} не совместима с материнской платой {motherboard.Name}. Неподдерживаемый тип памяти.");
                    return false;
                }

                if (ram.RamCapacity > motherboard.MaxRam)
                {
                    Console.WriteLine($"Оперативная память {ram.Name} превышает максимальную поддерживаемую память для материнской платы {motherboard.Name}. Максимум: {motherboard.MaxRam} ГБ.");
                    return false;
                }

                if (ram.RamModules > motherboard.RamSlots)
                {
                    Console.WriteLine($"Количество планок оперативной памяти {ram.Name} превышает количество слотов на материнской плате {motherboard.Name}. Слоты: {motherboard.RamSlots}.");
                    return false;
                }

                return true;
            }

            
            public bool CheckGPUCompatibility(Component gpu, Component motherboard)
            {
                //if (gpu.SlotType != "PCIe x16")
                //{
                //    Console.WriteLine($"Видеокарта {gpu.Name} требует слот PCIe x16, но материнская плата {motherboard.Name} не поддерживает его.");
                //    return false;
                //}

                // Дополнительно проверяем поддерживаемую длину видеокарты (в случае с корпусом и охлаждением)
                if (gpu.GpuLength > motherboard.GPUMaxLength)
                {
                    Console.WriteLine($"Видеокарта {gpu.Name} слишком длинная для материнской платы {motherboard.Name}. Максимальная длина: {motherboard.GPUMaxLength} мм.");
                    return false;
                }

                return true;
            }

            
            public bool CheckStorageCompatibility(Component storage, Component motherboard)
            {
                if (!motherboard.StorageInterfaces.Contains(storage.ConnectionType))
                {
                    Console.WriteLine($"Накопитель {storage.Name} не совместим с материнской платой {motherboard.Name}. Неподдерживаемый интерфейс подключения.");
                    return false;
                }

                return true;
            }

            
            public bool CheckPSUCompatibility(Component psu, List<Component> selectedComponents)
            {
                // Считаем необходимую мощность для всех компонентов
                int totalPower = 0;
                foreach (var component in selectedComponents)
                {
                    totalPower += component.PowerConsumption;
                }

                if (totalPower > psu.Wattage)
                {
                    Console.WriteLine($"Блок питания {psu.Name} не достаточно мощный для этой сборки. Требуется: {totalPower}W, а мощность блока питания: {psu.Wattage}W.");
                    return false;
                }

                return true;
            }

            // Проверка совместимости системы охлаждения с процессором и корпусом
            public bool CheckCoolingCompatibility(Component cooling, Component cpu, Component caseComponent)
            {
                // Проверка поддержки сокета процессора
                if (!cooling.SocketSupport.Contains(cpu.Socket))
                {
                    Console.WriteLine($"Система охлаждения {cooling.Name} не совместима с процессором {cpu.Name}. Не поддерживаемый сокет.");
                    return false;
                }

                // Проверка максимального TDP системы охлаждения
                if (cpu.TDP > cooling.MaxTDP)
                {
                    Console.WriteLine($"Система охлаждения {cooling.Name} не справится с процессором {cpu.Name}. Тепловыделение процессора {cpu.TDP}W превышает максимально поддерживаемое охлаждением {cooling.MaxTDP}W.");
                    return false;
                }

                // Проверка совместимости с корпусом по размерам
                if (cooling.CoolingClearance > caseComponent.CoolingClearance)
                {
                    Console.WriteLine($"Система охлаждения {cooling.Name} не поместится в корпус {caseComponent.Name}. Не хватает места для установки.");
                    return false;
                }

                return true;
            }

            // Проверка совместимости корпуса с видеокартой и материнской платой
            public bool CheckCaseCompatibility(Component caseComponent, Component motherboard, Component gpu)
            {
                // Проверка совместимости с форм-фактором материнской платы
                if (!caseComponent.SupportedFormFactors.Contains(motherboard.FormFactor))
                {
                    Console.WriteLine($"Корпус {caseComponent.Name} не совместим с материнской платой {motherboard.Name}. Не поддерживаемый форм-фактор.");
                    return false;
                }

                // Проверка, помещается ли видеокарта в корпус
                if (gpu.GpuLength > caseComponent.GPUMaxLength)
                {
                    Console.WriteLine($"Видеокарта {gpu.Name} не поместится в корпус {caseComponent.Name}. Максимальная длина видеокарты: {caseComponent.GPUMaxLength} мм.");
                    return false;
                }

                return true;
            }

            // Общая проверка совместимости всех выбранных компонентов
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

                // Проверка совместимости всех компонентов
                if (cpu != null && motherboard != null && !CheckCPUCompatibility(cpu, motherboard)) return false;
                if (ram != null && motherboard != null && !CheckRAMCompatibility(ram, motherboard)) return false;
                //if (gpu != null && motherboard != null && !CheckGPUCompatibility(gpu, motherboard)) return false;
                //if (storage != null && motherboard != null && !CheckStorageCompatibility(storage, motherboard)) return false;
                if (psu != null && !CheckPSUCompatibility(psu, selectedComponents)) return false;
                if (cooling != null && cpu != null && caseComponent != null && !CheckCoolingCompatibility(cooling, cpu, caseComponent)) return false;
                if (caseComponent != null && motherboard != null && gpu != null && !CheckCaseCompatibility(caseComponent, motherboard, gpu)) return false;

                return true; // Все совместимо
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

            Console.WriteLine($"Материнка: {motherboard?.Name}, GPUMaxLength = {motherboard?.GPUMaxLength}");
            Console.WriteLine($"Видеокарта: {gpu?.Name}, Length = {gpu?.GpuLength}");

            var compatibilityService = new CompatibilityService();
            var issues = compatibilityService.GetCompatibilityIssues(selectedComponents.Where(c => c != null).ToList());

            if (issues.Count == 0)
            {
                await DisplayAlert("Совместимость", "Все компоненты совместимы!", "Ок");
            }
            else
            {
                string message = "Обнаружены следующие проблемы:\n\n" + string.Join("\n• ", issues);
                await DisplayAlert("Ошибка совместимости", message, "Ок");
            }
        }


        private async Task<Component> GetSelectedComponent(string componentType, string componentName)
        {
            if (string.IsNullOrWhiteSpace(componentName))
                return null;

            Console.WriteLine($"[DEBUG] Получено из селектора: \"{componentName}\"");

            var component = await App.Database.GetComponentByNameAsync(componentName);

            if (component == null)
                Console.WriteLine($"[DEBUG] Компонент с именем \"{componentName}\" НЕ НАЙДЕН в базе");
            else
                Console.WriteLine($"[DEBUG] Найден компонент: {component.Name} (ID = {component.Id})");

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

            //TotalPriceLabel.Text = $"Общая стоимость: {total} {currency}";
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
                .Select(g => g.OrderBy(c => c.PowerConsumption).FirstOrDefault()) // по минимальному потреблению
                .Where(c => c != null)
                .ToList();

            var totalRequiredPower = compatible.Sum(c => c.PowerConsumption);

            if (totalRequiredPower > psu.Wattage)
            {
                await DisplayAlert("Внимание", $"Выбранный БП может быть слишком слабым (требуется минимум {totalRequiredPower} Вт)", "OK");
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

