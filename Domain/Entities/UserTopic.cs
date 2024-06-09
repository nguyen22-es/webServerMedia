using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public class UserTopic
    {
        public string UserId { get; set; }
        public VODUser User { get; set; }
        public int TopicId { get; set; }
        public Topic  Topic { get; set; }
    }
}
