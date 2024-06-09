using Common.DTOModels.UI;


namespace UI.Models.MembershipViewModels
{
    public class MediaViewModel
    {
        public string Title { get; set; }
        public MediaDTO mediaDTO { get; set; }
        public TopicTypeDotDTO  TopicTypeDTO { get; set; }
        public TopicDTO topicDTO { get; set; }
        public LessonInfoDTO LessonInfo { get; set; }
    }
}
