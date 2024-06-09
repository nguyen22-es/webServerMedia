
using Common.DTOModels.UI;
using System.Collections.Generic;


namespace UI.Models.MembershipViewModels
{
    public class TopicViewModel
    {
        public TopicDTO?  TopicDTO { get; set; }
        public TopicTypeDotDTO?  TopicTypeDTO { get; set; }
        public IEnumerable<ModuleDTO>? Modules { get; set; }
    }
}
