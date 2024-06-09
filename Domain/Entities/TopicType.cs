using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class TopicType
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string Name { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }
        [MaxLength(1024)]
        public string Thumbnail { get; set; }
    }
}
