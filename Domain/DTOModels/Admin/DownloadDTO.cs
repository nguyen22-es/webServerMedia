using System.ComponentModel.DataAnnotations;

namespace Common.DTOModels.Admin
{
    public class DownloadDTO
    {
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string? Title { get; set; }
        [MaxLength(1024)]
        public string Url { get; set; }
        public int TopicId { get; set; }
        public string? Topic { get; set; }
        public int ModuleId { get; set; }
        public string? Module { get; set; }

        public ButtonDTO ButtonDTO { get { return new ButtonDTO(ModuleId, Id); } }
    }
}
