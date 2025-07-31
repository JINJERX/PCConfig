using SQLite;
using MauiApp3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace MauiApp3.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        private static bool _isInitialized = false;
        public string DatabasePath { get; }

        // Убираем лишний вложенный класс

        public DatabaseService(string dbPath)
        {
            DatabasePath = dbPath; // Сохраняем путь
            _database = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            InitAsync().ConfigureAwait(false); 
        }

        /// <summary>
        /// Инициализация базы данных (создание таблиц)
        /// </summary>
        public async Task InitAsync()
        {
            if (_isInitialized) return; // Чтобы не создавать таблицы повторно

            await _database.CreateTableAsync<Component>();
            await _database.CreateTableAsync<PCBuild>(); // Создание таблицы PCBuild
            await _database.CreateTableAsync<Price>();

            _isInitialized = true;
        }

        /// <summary>
        /// Добавить компонент в базу
        /// </summary>
        public async Task<int> AddComponentAsync(Component component)
        {
            await InitAsync();
            return await _database.InsertAsync(component);
        }

        /// <summary>
        /// Получить все компоненты
        /// </summary>
        public async Task<List<Component>> GetComponentsAsync()
        {
            await InitAsync();
            return await _database.Table<Component>().ToListAsync();
        }

        /// <summary>
        /// Получить компоненты по типу (например, процессоры)
        /// </summary>
        public async Task<List<Component>> GetComponentsByTypeAsync(string type)
        {
            await InitAsync();
            return await _database.Table<Component>().Where(c => c.Type == type).ToListAsync();
        }

        /// <summary>
        /// Сохранить сборку ПК в базе
        /// </summary>
        public async Task<int> SaveBuildAsync(PCBuild build)
        {
            await InitAsync();
            return await _database.InsertAsync(build);
        }

        /// <summary>
        /// Получить все сохраненные сборки
        /// </summary>
        public async Task<List<PCBuild>> GetAllBuildsAsync()
        {
            await InitAsync();
            return await _database.Table<PCBuild>().ToListAsync();
        }

        /// <summary>
        /// Получить список всех таблиц в базе
        /// </summary>
        public async Task<List<string>> GetTablesAsync()
        {
            await InitAsync();
            var tables = await _database.QueryAsync<TableInfo>("SELECT name FROM sqlite_master WHERE type='table'");
            return tables.Select(t => t.Name).ToList();
        }

        private class TableInfo
        {
            public string Name { get; set; }
        }

        public async Task<Component> GetComponentByNameAsync(string name)
        {
            var cleanedName = name?.Trim().ToLower();
            var allComponents = await _database.Table<Component>().ToListAsync();

            var component = allComponents.FirstOrDefault(c => c.Name?.Trim().ToLower() == cleanedName);

            if (component != null)
            {
                System.Diagnostics.Debug.WriteLine($"[DOTNET] [DEBUG] Найден компонент: {component.Name} (ID = {component.Id}, GPUMaxLength = {component.GPUMaxLength})");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[DOTNET] [DEBUG] Компонент не найден: {name}");
            }

            return component;
        }



        public Task<int> DeleteComponentByIdAsync(int id)
        {
            return _database.DeleteAsync<Component>(id);
        }
        public async Task<int> UpdateAsync(Component component)
        {
            await InitAsync();  // Убедитесь, что база данных инициализирована
            return await _database.UpdateAsync(component);  // Выполнение обновления компонента
        }

        public async Task UpdateComponentAsync(Component component)
        {
            await _database.UpdateAsync(component);
        }



        public async Task<Price> GetPriceByComponentIdAsync(int componentId)
        {
            await InitAsync();
            return await _database.Table<Price>().FirstOrDefaultAsync(p => p.ComponentId == componentId);
        }

        public async Task<int> SaveOrUpdatePriceAsync(Price price)
        {
            await InitAsync();
            var existing = await GetPriceByComponentIdAsync(price.ComponentId);
            if (existing != null)
            {
                price.Id = existing.Id;
                return await _database.UpdateAsync(price);
            }
            else
            {
                return await _database.InsertAsync(price);
            }
        }

        public async Task<int> DeleteBuildAsync(PCBuild build)
        {
            return await _database.DeleteAsync(build);
        }

        public Task<List<Component>> GetAllComponentsAsync()
        {
            return _database.Table<Component>().ToListAsync();
        }


    }
}
