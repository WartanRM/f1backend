using System.ComponentModel.DataAnnotations;

namespace F1Backend.Models
{
    public class Accomplishments
    {
        [Key]
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string ModelName { get; set; }
    }
}
