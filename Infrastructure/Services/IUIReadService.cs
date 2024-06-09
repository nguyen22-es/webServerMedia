using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Database.Services
{
    public interface IUIReadService
    {
        Task<IEnumerable<Topic>> GetTopics(string userId);
        Task<Topic> GetTopic(string userId, int topicId);
        Task<Media> GetMedia(string userId, int MediaId);
        Task<IEnumerable<Media>> GetMedias(string userId, int moduleId = default);
    }
}
