using SQLite;

namespace MauiApp3.Models
{
    public class Price
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ComponentId { get; set; } // Связь с компонентом

        public decimal Value { get; set; }   // Цена
        public string Currency { get; set; } // Валюта (например, "₽", "$", "€")
    }
}
