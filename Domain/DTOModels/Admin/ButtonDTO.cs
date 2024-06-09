namespace Common.DTOModels.Admin
{
    public class ButtonDTO
    {
        #region Properties
        public int? TopicId { get; set; }
        public int? ModuleId { get; set; }
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string? ItemId
        {
            get { return Id > 0 ? Id.ToString() : UserId; }
        }
        #endregion

        #region Constructors
        public ButtonDTO(int TopicId,int moduleId, int id)
        {
            this.TopicId = TopicId;
            ModuleId = moduleId;
            Id = id;
        }

        public ButtonDTO(int TopicId,int id)
        {
            this.TopicId = TopicId;
            Id = id;
        }

        public ButtonDTO(int id)
        {
            Id = id;
        }

        public ButtonDTO(string userId)
        {
            UserId = userId;
        }
        #endregion
    }
}
