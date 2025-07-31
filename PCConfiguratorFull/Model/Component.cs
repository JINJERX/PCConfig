using SQLite;

namespace MauiApp3.Models
{
    public class Component
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; } // CPU, GPU, RAM, Storage, PSU, Case, Motherboard, Cooling
        public string ImagePath { get; set; } // Локальный путь к изображению


        // Общие
        public int PowerConsumption { get; set; }

        // CPU
        public string Socket { get; set; }            // Например, AM4, LGA1700
        public int TDP { get; set; }                  // Тепловыделение

        // RAM
        public string RamType { get; set; }           // DDR4, DDR5
        public int RamCapacity { get; set; }          // в ГБ
        public int RamFrequency { get; set; }         // в МГц
        public int RamModules { get; set; }           // количество планок

        // Motherboard
        public string FormFactor { get; set; }        // ATX, Micro-ATX
        public string Chipset { get; set; }
        public int RamSlots { get; set; }
        public int MaxRam { get; set; }
        public int PCIESlots { get; set; }
        public string StorageInterfaces { get; set; } // Например: "SATA,M.2,NVMe"

        // GPU
        public string SlotType { get; set; }          // Например, PCIe x16
        public int GpuLength { get; set; }            // в мм
        public int RecommendedPSUWattage { get; set; }

        // Storage
        public string ConnectionType { get; set; }    // SATA, M.2, NVMe
        public int StorageCapacity { get; set; }      // в ГБ или ТБ
        public string StorageFormFactor { get; set; } // 2.5", 3.5", M.2

        // PSU
        public int Wattage { get; set; }
        public string EfficiencyRating { get; set; }  // 80+ Bronze, Gold и т.д.
        public string PSUFormFactor { get; set; }     // ATX, SFX

        // Cooling
        public string SocketSupport { get; set; }     // Например: "AM4,LGA1700"
        public string CoolingType { get; set; }       // Air, Liquid
        public int MaxTDP { get; set; }

        // Case
        public string SupportedFormFactors { get; set; } // Например: "ATX,mATX"
        public int GPUMaxLength { get; set; }             // мм
        public int CoolingClearance { get; set; }         // мм
        public string PSUFormFactorSupport { get; set; }

        // Дополнительно (по желанию)
        public string Description { get; set; } // Для отображения в UI

        [Ignore] 
        public Price Price { get; set; }

    }
}
