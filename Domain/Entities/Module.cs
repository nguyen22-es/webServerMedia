using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string Title { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public List<Media> medias { get; set; }
        public List<Download> Downloads { get; set; }
    }
}
