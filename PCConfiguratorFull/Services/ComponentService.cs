using MauiApp3.Models;
using MauiApp3.Services;
using SQLite;


namespace MauiApp3.Services
{
    public class ComponentService
    {
        private readonly DatabaseService _database;

        public ComponentService(DatabaseService database)
        {
            _database = database;
        }

        public async Task LoadTestDataAsync()
        {
            Console.WriteLine("LoadTestDataAsync вызван!");

            // Получаем все компоненты из базы
            var existingComponents = await _database.GetComponentsAsync();

            // Метод для проверки существования компонента
            bool ComponentExists(string name) => existingComponents.Any(c => c.Name == name);

            Console.WriteLine($"Компонентов в базе: {existingComponents.Count}");

            Console.WriteLine("Добавляем тестовые компоненты...");

            if (!ComponentExists("Intel i5-12400"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Intel i5-12400",
                    Type = "CPU",
                    Socket = "LGA1700",
                    PowerConsumption = 65,
                    TDP = 65,
                    
                });
                Console.WriteLine("Добавлен Intel i5-12400");
            }

            if (!ComponentExists("AMD Ryzen 5 5600X"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "AMD Ryzen 5 5600X",
                    Type = "CPU",
                    Socket = "AM4",
                    PowerConsumption = 65,
                    TDP = 65
                });
                Console.WriteLine("Добавлен AMD Ryzen 5 5600X");
            }
            if (!ComponentExists("Intel Core i9-11900K"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Intel Core i9-11900K",
                    Type = "CPU",
                    Socket = "LGA1200",
                    PowerConsumption = 125,
                    TDP = 125
                });
                Console.WriteLine("Добавлен Intel Core i9-11900K");
            }

            if (!ComponentExists("AMD Ryzen 9 5900X"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "AMD Ryzen 9 5900X",
                    Type = "CPU",
                    Socket = "AM4",
                    PowerConsumption = 105,
                    TDP = 105
                });
                Console.WriteLine("Добавлен AMD Ryzen 9 5900X");
            }

            if (!ComponentExists("NVIDIA RTX 3060"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "NVIDIA RTX 3060",
                    Type = "GPU",
                    PowerConsumption = 170,
                    SlotType = "PCIe x16",
                    GpuLength = 242, // Примерная длина
                    RecommendedPSUWattage = 550
                });
                Console.WriteLine("Добавлен NVIDIA RTX 3060");
            }

            if (!ComponentExists("AMD RX 6700 XT"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "AMD RX 6700 XT",
                    Type = "GPU",
                    PowerConsumption = 230,
                    SlotType = "PCIe x16",
                    GpuLength = 267,
                    RecommendedPSUWattage = 650
                });
                Console.WriteLine("Добавлен AMD RX 6700 XT");
            }
            if (!ComponentExists("NVIDIA RTX 3080"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "NVIDIA RTX 3080",
                    Type = "GPU",
                    PowerConsumption = 320,
                    SlotType = "PCIe x16",
                    GpuLength = 300, // Примерная длина
                    RecommendedPSUWattage = 750
                });
                Console.WriteLine("Добавлен NVIDIA RTX 3080");
            }

            if (!ComponentExists("AMD Radeon RX 6800"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "AMD Radeon RX 6800",
                    Type = "GPU",
                    PowerConsumption = 250,
                    SlotType = "PCIe x16",
                    GpuLength = 267,
                    RecommendedPSUWattage = 750
                });
                Console.WriteLine("Добавлен AMD Radeon RX 6800");
            }

            if (!ComponentExists("Kingston Fury 16GB 3200MHz"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Kingston Fury 16GB 3200MHz",
                    Type = "RAM",
                    PowerConsumption = 5,
                    RamType = "DDR4",
                    RamCapacity = 16,  // В ГБ
                    RamFrequency = 3200, // МГц
                    RamModules = 2
                });
                Console.WriteLine("Добавлен Kingston Fury 16GB 3200MHz");
            }
            if (!ComponentExists("Corsair Vengeance LPX 32GB 3600MHz"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Corsair Vengeance LPX 32GB 3600MHz",
                    Type = "RAM",
                    PowerConsumption = 10,
                    RamType = "DDR4",
                    RamCapacity = 32,  // В ГБ
                    RamFrequency = 3600, // МГц
                    RamModules = 2
                });
                Console.WriteLine("Добавлен Corsair Vengeance LPX 32GB 3600MHz");
            }

            if (!ComponentExists("Samsung 970 EVO Plus 1TB"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Samsung 970 EVO Plus 1TB",
                    Type = "Storage",
                    PowerConsumption = 10,
                    ConnectionType = "M.2 NVMe",
                    StorageCapacity = 1000, // ГБ
                    StorageFormFactor = "M.2"
                });
                Console.WriteLine("Добавлен Samsung 970 EVO Plus 1TB");
            }

            if (!ComponentExists("Seagate 1TB HDD"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Seagate 1TB HDD",
                    Type = "Storage",
                    PowerConsumption = 6,
                    ConnectionType = "SATA",
                    StorageCapacity = 1000, // ГБ
                    StorageFormFactor = "3.5\""
                });
                Console.WriteLine("Добавлен Seagate 1TB HDD");
            }

            if (!ComponentExists("Corsair RM850"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Corsair RM850",
                    Type = "PSU",
                    PowerConsumption = 100,
                    Wattage = 850,
                    EfficiencyRating = "80+ Gold",
                    PSUFormFactor = "ATX"
                });
                Console.WriteLine("Добавлен Corsair RM850");
            }
            if (!ComponentExists("Seasonic Focus GX-850"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Seasonic Focus GX-850",
                    Type = "PSU",
                    PowerConsumption = 100,
                    Wattage = 850,
                    EfficiencyRating = "80+ Gold",
                    PSUFormFactor = "ATX"
                });
                Console.WriteLine("Добавлен Seasonic Focus GX-850");
            }

            if (!ComponentExists("Phanteks Eclipse P400A"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Phanteks Eclipse P400A",
                    Type = "Case",
                    PowerConsumption = 25,
                    SupportedFormFactors = "ATX,mATX,ITX",
                    GPUMaxLength = 420, // мм
                    CoolingClearance = 175, // мм
                    PSUFormFactorSupport = "ATX"
                });
                Console.WriteLine("Добавлен Phanteks Eclipse P400A");
            }

            if (!ComponentExists("NZXT H510"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "NZXT H510",
                    Type = "Case",
                    PowerConsumption = 20,
                    SupportedFormFactors = "ATX,mATX",
                    GPUMaxLength = 381, // мм
                    CoolingClearance = 160, // мм
                    PSUFormFactorSupport = "ATX"
                });
                Console.WriteLine("Добавлен NZXT H510");
            }

            if (!ComponentExists("Asus TUF Gaming B550-PLUS"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Asus TUF Gaming B550-PLUS",
                    Type = "Motherboard",
                    PowerConsumption = 50,
                    Socket = "AM4",
                    FormFactor = "ATX",
                    Chipset = "B550",
                    RamSlots = 4,
                    MaxRam = 64,
                    PCIESlots = 2,
                    StorageInterfaces = "SATA,M.2"                   
                });
                Console.WriteLine("Добавлен Asus TUF Gaming B550-PLUS");
            }
            if (!ComponentExists("MSI MAG B550 TOMAHAWK WIFI"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "MSI MAG B550 TOMAHAWK WIFI",
                    Type = "Motherboard",
                    PowerConsumption = 50,
                    Socket = "AM4",
                    FormFactor = "ATX",
                    Chipset = "B550",
                    RamSlots = 4,
                    MaxRam = 128,
                    PCIESlots = 2,
                    StorageInterfaces = "SATA,M.2"
                });
                Console.WriteLine("Добавлен MSI MAG B550 TOMAHAWK WIFI");
            }
            if (!ComponentExists("NZXT Kraken X73"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "NZXT Kraken X73",
                    Type = "Cooling",
                    PowerConsumption = 30,
                    SocketSupport = "AM4,LGA1200,LGA1700",
                    CoolingType = "Liquid",
                    MaxTDP = 300
                });
                Console.WriteLine("Добавлен NZXT Kraken X73");
            }
            if (!ComponentExists("Cooler Master Hyper 212"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Cooler Master Hyper 212",
                    Type = "Cooling",
                    PowerConsumption = 15,
                    SocketSupport = "AM4,LGA1700",
                    CoolingType = "Air",
                    MaxTDP = 200
                });
                Console.WriteLine("Добавлен Cooler Master Hyper 212");
            }
            if (!ComponentExists("Intel Core i7-12700K"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Intel Core i7-12700K",
                    Type = "CPU",
                    Socket = "LGA1700",
                    PowerConsumption = 125,
                    TDP = 125
                });
                Console.WriteLine("Добавлен Intel Core i7-12700K");
            }
            if (!ComponentExists("AMD Ryzen 7 5800X"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "AMD Ryzen 7 5800X",
                    Type = "CPU",
                    Socket = "AM4",
                    PowerConsumption = 105,
                    TDP = 105
                });
                Console.WriteLine("Добавлен AMD Ryzen 7 5800X");
            }

            // 🎮 GPU
            if (!ComponentExists("NVIDIA RTX 4070 Ti"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "NVIDIA RTX 4070 Ti",
                    Type = "GPU",
                    PowerConsumption = 285,
                    SlotType = "PCIe x16",
                    GpuLength = 285,
                    RecommendedPSUWattage = 700
                });
                Console.WriteLine("Добавлен NVIDIA RTX 4070 Ti");
            }
            if (!ComponentExists("AMD Radeon RX 7900 XT"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "AMD Radeon RX 7900 XT",
                    Type = "GPU",
                    PowerConsumption = 300,
                    SlotType = "PCIe x16",
                    GpuLength = 287,
                    RecommendedPSUWattage = 750
                });
                Console.WriteLine("Добавлен AMD Radeon RX 7900 XT");
            }

            // 🧠 RAM
            if (!ComponentExists("G.Skill Trident Z 64GB 3600MHz"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "G.Skill Trident Z 64GB 3600MHz",
                    Type = "RAM",
                    PowerConsumption = 12,
                    RamType = "DDR4",
                    RamCapacity = 64,
                    RamFrequency = 3600,
                    RamModules = 4
                });
                Console.WriteLine("Добавлен G.Skill Trident Z 64GB 3600MHz");
            }
            if (!ComponentExists("Corsair Dominator Platinum 32GB 5200MHz"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Corsair Dominator Platinum 32GB 5200MHz",
                    Type = "RAM",
                    PowerConsumption = 14,
                    RamType = "DDR5",
                    RamCapacity = 32,
                    RamFrequency = 5200,
                    RamModules = 2
                });
                Console.WriteLine("Добавлен Corsair Dominator Platinum 32GB 5200MHz");
            }

            // 💾 Storage
            if (!ComponentExists("WD Blue SN570 500GB"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "WD Blue SN570 500GB",
                    Type = "Storage",
                    PowerConsumption = 4,
                    ConnectionType = "M.2 NVMe",
                    StorageCapacity = 500,
                    StorageFormFactor = "M.2"
                });
                Console.WriteLine("Добавлен WD Blue SN570 500GB");
            }
            if (!ComponentExists("Crucial MX500 2TB"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Crucial MX500 2TB",
                    Type = "Storage",
                    PowerConsumption = 6,
                    ConnectionType = "SATA",
                    StorageCapacity = 2000,
                    StorageFormFactor = "2.5\""
                });
                Console.WriteLine("Добавлен Crucial MX500 2TB");
            }

            // ⚡ PSU
            if (!ComponentExists("Be Quiet! Straight Power 11 750W"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Be Quiet! Straight Power 11 750W",
                    Type = "PSU",
                    PowerConsumption = 90,
                    Wattage = 750,
                    EfficiencyRating = "80+ Platinum",
                    PSUFormFactor = "ATX"
                });
                Console.WriteLine("Добавлен Be Quiet! Straight Power 11 750W");
            }
            if (!ComponentExists("EVGA SuperNOVA 1000 G5"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "EVGA SuperNOVA 1000 G5",
                    Type = "PSU",
                    PowerConsumption = 110,
                    Wattage = 1000,
                    EfficiencyRating = "80+ Gold",
                    PSUFormFactor = "ATX"
                });
                Console.WriteLine("Добавлен EVGA SuperNOVA 1000 G5");
            }

            // 🏠 Case
            if (!ComponentExists("Fractal Design Meshify C"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Fractal Design Meshify C",
                    Type = "Case",
                    PowerConsumption = 15,
                    SupportedFormFactors = "ATX,mATX,ITX",
                    GPUMaxLength = 315,
                    CoolingClearance = 170,
                    PSUFormFactorSupport = "ATX"
                });
                Console.WriteLine("Добавлен Fractal Design Meshify C");
            }
            if (!ComponentExists("Lian Li Lancool II Mesh"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Lian Li Lancool II Mesh",
                    Type = "Case",
                    PowerConsumption = 18,
                    SupportedFormFactors = "ATX,mATX,ITX",
                    GPUMaxLength = 384,
                    CoolingClearance = 176,
                    PSUFormFactorSupport = "ATX"
                });
                Console.WriteLine("Добавлен Lian Li Lancool II Mesh");
            }

            // 🧩 Motherboard
            if (!ComponentExists("ASRock Z690 Phantom Gaming 4"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "ASRock Z690 Phantom Gaming 4",
                    Type = "Motherboard",
                    PowerConsumption = 55,
                    Socket = "LGA1700",
                    FormFactor = "ATX",
                    Chipset = "Z690",
                    RamSlots = 4,
                    MaxRam = 128,
                    PCIESlots = 3,
                    StorageInterfaces = "SATA,M.2"
                });
                Console.WriteLine("Добавлен ASRock Z690 Phantom Gaming 4");
            }
            if (!ComponentExists("Gigabyte B660M DS3H"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Gigabyte B660M DS3H",
                    Type = "Motherboard",
                    PowerConsumption = 45,
                    Socket = "LGA1700",
                    FormFactor = "mATX",
                    Chipset = "B660",
                    RamSlots = 4,
                    MaxRam = 128,
                    PCIESlots = 2,
                    StorageInterfaces = "SATA,M.2"
                });
                Console.WriteLine("Добавлен Gigabyte B660M DS3H");
            }

            // ❄️ Cooling
            if (!ComponentExists("be quiet! Dark Rock Pro 4"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "be quiet! Dark Rock Pro 4",
                    Type = "Cooling",
                    PowerConsumption = 20,
                    SocketSupport = "AM4,LGA1200,LGA1700",
                    CoolingType = "Air",
                    MaxTDP = 250
                });
                Console.WriteLine("Добавлен be quiet! Dark Rock Pro 4");
            }
            if (!ComponentExists("Arctic Liquid Freezer II 360"))
            {
                await App.Database.AddComponentAsync(new Component
                {
                    Name = "Arctic Liquid Freezer II 360",
                    Type = "Cooling",
                    PowerConsumption = 35,
                    SocketSupport = "AM4,LGA1700,LGA1200",
                    CoolingType = "Liquid",
                    MaxTDP = 320
                });
                Console.WriteLine("Добавлен Arctic Liquid Freezer II 360");
            }
            Console.WriteLine("Данные обновлены!");
        }

        public async Task<int> UpdateComponentAsync(Component component)
        {
            // Выполняем обновление компонента в базе данных
            return await _database.UpdateAsync(component);
        }

        public async Task<List<string>> GetComponentsByTypeAsync(string type)
        {
            var components = await _database.GetComponentsByTypeAsync(type);
            return components.Select(c => c.Name).ToList();
        }

        
    }
}
