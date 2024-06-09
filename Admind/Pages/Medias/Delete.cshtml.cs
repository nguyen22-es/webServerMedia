using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Common.DTOModels.Admin;
using Common.Entities;
using Common.Services;

namespace Admin.Pages.Medias
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public MediaDTO Input { get; set; } = new MediaDTO();
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public DeleteModel(IAdminService db)
        {
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<IActionResult> OnGetAsync(int id, int TopicId, int moduleId)
          {
            try
            {
                Alert = string.Empty;
                Input = await _db.SingleAsync<Media, MediaDTO>(s => s.Id.Equals(id) && s.ModuleId.Equals(moduleId) && s.TopicId.Equals(TopicId), true);
                return Page();
            }
            catch
            {
                return RedirectToPage("/Index", new { alert = "You do not have access to this page." });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int id = Input.Id, topicId = Input.TopicId, moduleId = Input.ModuleId;

          
                var succeeded = await _db.DeleteAsync<Media>(d => d.Id.Equals(id) && d.ModuleId.Equals(moduleId) && d.TopicId.Equals(topicId));
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Deleted Media: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            

            // Something failed, redisplay the form.
            Input = await _db.SingleAsync<Media, MediaDTO>(s => s.Id.Equals(id) && s.ModuleId.Equals(moduleId) && s.TopicId.Equals(topicId), true);
            return Page();
        }
        #endregion
    }
}