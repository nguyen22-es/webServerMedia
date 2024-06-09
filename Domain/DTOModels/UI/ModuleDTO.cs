using System.Collections.Generic;

namespace Common.DTOModels.UI
{
    public class ModuleDTO
    {
        public int Id { get; set; }
        public string ModuleTitle { get; set; }
        public List<MediaDTO>  medias { get; set; }
        public List<DownloadDTO> Downloads { get; set; }
    }
}
