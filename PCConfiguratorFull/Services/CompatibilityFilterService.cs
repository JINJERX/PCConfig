using System.Collections.Generic;
using System.Linq;
using MauiApp3.Models;

namespace MauiApp3.Services
{
    public class SimplifiedFilterService
    {
        private readonly DatabaseService _database;

        public SimplifiedFilterService(DatabaseService database)
        {
            _database = database;
        }

        public async Task<List<Component>> GetCompatibleComponentsAsync(List<Component> selectedComponents, string targetType)
        {
            var allComponents = await _database.GetComponentsByTypeAsync(targetType);

            
            if (targetType == "Motherboard")
            {
                var cpu = selectedComponents.FirstOrDefault(c => c.Type == "CPU");
                if (cpu != null)
                {
                    return allComponents.Where(mb => mb.Socket == cpu.Socket).ToList();
                }
            }

            if (targetType == "RAM")
            {
                var motherboard = selectedComponents.FirstOrDefault(c => c.Type == "Motherboard");
                if (motherboard != null)
                {
                    return allComponents.Where(ram => ram.RamType == motherboard.RamType).ToList();
                }
            }

            

            return allComponents; 
        }
    }
}
