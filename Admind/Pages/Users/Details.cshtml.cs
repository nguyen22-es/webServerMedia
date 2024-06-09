using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Common.DTOModels;
using Common.Entities;
using Common.Extensions;
using Database.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        #region Properties
        private readonly IDbReadService _dbRead;
        private readonly IDbWriteService _dbWrite;
        public IEnumerable<Topic> Topic { get; set; } = new List<Topic>();
        public SelectList AvailableTopics { get; set; }
        [BindProperty, Display(Name = "Available Topic")] public int TopicId { get; set; }
        public UserDTO Customer { get; set; }
        #endregion

        #region Constructor
        public DetailsModel(IDbReadService dbReadService, IDbWriteService dbWriteService)
        {
            _dbRead = dbReadService;
            _dbWrite = dbWriteService;
        }
        #endregion

        #region Helper Methods
        private async Task FillViewData(string userId)
        {
            // Fetch the user/customer
            var user = await _dbRead.SingleAsync<VODUser>(u => u.Id.Equals(userId));
            Customer = new UserDTO { Id = user.Id, Email = user.Email };

            // Fetch the user's courses and course ids
            _dbRead.Include<UserTopic>();
            var userTopics = await _dbRead.GetAsync<UserTopic>(uc => uc.UserId.Equals(userId));
            var usersTopicIds = userTopics.Select(c => c.TopicId).ToList();
            Topic = userTopics.Select(c => c.Topic).ToList();

            // Fetch courses that the user doesn't already have access to
            var availableTopics = await _dbRead.GetAsync<Topic>(uc => !usersTopicIds.Contains(uc.Id));
            AvailableTopics = availableTopics.ToSelectList("Id", "Title");


        }
        #endregion

        #region Actions
        public async Task OnGetAsync(string id)
        {
            await FillViewData(id);
        }
        public async Task<IActionResult> OnPostAddAsync(string userId)
        {
            try
            {
                _dbWrite.Add(new UserTopic
                {
                    TopicId = TopicId,
                    UserId = userId
                });
                var succeeded = await _dbWrite.SaveChangesAsync();
            }
            catch
            {
            }

            await FillViewData(userId);
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveAsync(int courseId, string userId)
        {
            try
            {
                var userCourse = await _dbRead.SingleAsync<UserTopic>(uc =>
                    uc.UserId.Equals(userId) &&
                    uc.TopicId.Equals(courseId));

                if (userCourse != null)
                {
                    _dbWrite.Delete(userCourse);
                    await _dbWrite.SaveChangesAsync();
                }
            }
            catch
            {
            }

            await FillViewData(userId);
            return Page();
        }
        #endregion
    }
}