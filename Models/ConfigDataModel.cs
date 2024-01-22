using System.ComponentModel.DataAnnotations;

namespace ConfigToDBConverter.Models
{
    public class ConfigDataModel
    {
        [Key]
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }

    }
}
