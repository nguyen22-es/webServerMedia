using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOModels.Admin
{
    public class ModuleDTO
    {
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string? Title { get; set; }
        public int TopicId { get; set; }
        public string? Topic { get; set; }
        public ICollection<MediaDTO>? Videos { get; set; }
        public ICollection<DownloadDTO>? Downloads { get; set; }
        public string TopicAndModule { get { return $"{Title} ({Topic})"; } }
        public ButtonDTO ButtonDTO { get { return new ButtonDTO( Id); } }
    }
}
