using System.ComponentModel.DataAnnotations;

namespace CMWebAPI.Models
{
    public class CMApplication
    {
        [Key]
        public int  CI_ID { get; set; }
        public string Name { get; set; }
        public string AdminCategories { get; set; }
    }
}
