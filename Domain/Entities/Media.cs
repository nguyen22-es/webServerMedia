﻿using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class Media
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string Title { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }
        public int Duration { get; set; }
        [MaxLength(1024)]
        public string Thumbnail { get; set; }
        [MaxLength(1024)]
        public string Url { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }

    }
}
