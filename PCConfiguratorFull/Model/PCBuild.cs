using SQLite;

namespace MauiApp3.Models
{
    [Table("PCBuild")]
    public class PCBuild
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CPU { get; set; }
        public string GPU { get; set; }
        public string RAM { get; set; }
        public string Storage { get; set; }
        public string PSU { get; set; }
        public string Case { get; set; }
        public string Motherboard { get; set; }
        public string Cooling { get; set; }
    }
}
