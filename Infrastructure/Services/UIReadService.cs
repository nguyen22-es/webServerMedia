using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Database.Services;

public class UIReadService : IUIReadService
{

    private readonly IDbReadService _db;
  
    public UIReadService(IDbReadService db)
    {
        _db = db;
    }

    public async Task<Media> GetMedia(string userId, int MediaId)
    {

        var video = await _db.SingleAsync<Media>(v => v.Id.Equals(MediaId));
        if (video == null) return default;


        return video;
    }

    public async Task<IEnumerable<Media>?> GetMedias(string userId, int moduleId = 0)
    {
        _db.Include<Media>();

        var module = await _db.SingleAsync<Module>(m => m.Id.Equals(moduleId));
        if (module == null) return default(List<Media>);

        return module.medias;
    }

    public async Task<Topic> GetTopic(string userId, int topicId)
    {
        _db.Include<Topic, Module>();
        var userCourse = await _db.SingleAsync<UserTopic>(c => c.UserId.Equals(userId) && c.TopicId.Equals(topicId));
        if (userCourse == null) return default;
        return userCourse.Topic;
    }

    public async Task<IEnumerable<Topic>> GetTopics(string userId)
    {
        _db.Include<UserTopic>();
        var userCourses = await _db.GetAsync<UserTopic>(uc => uc.UserId.Equals(userId));
        return userCourses.Select(c => c.Topic);
    }
}





