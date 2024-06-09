using System.Threading.Tasks;
using Common.DTOModels.Admin;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Admin.Pages.Downloads
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public DownloadDTO Input { get; set; } = new DownloadDTO();
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public DeleteModel(IAdminService db)
        {
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<IActionResult> OnGetAsync(int id, int topicId, int moduleId)
        {
            try
            {
                Alert = string.Empty;
                Input = await _db.SingleAsync<Download, DownloadDTO>(s => s.Id.Equals(id) && s.ModuleId.Equals(moduleId) && s.TopicId.Equals(topicId), true);
                return Page();
            }
            catch
            {
                return RedirectToPage("/Index", new { alert = "You do not have access to this page." });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int id = Input.Id, TopicId = Input.TopicId, moduleId = Input.ModuleId;

            if (ModelState.IsValid)
            {
                var succeeded = await _db.DeleteAsync<Download>(d => d.Id.Equals(id) && d.ModuleId.Equals(moduleId) && d.TopicId.Equals(TopicId));
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Deleted Download: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }

            // Something failed, redisplay the form.
            Input = await _db.SingleAsync<Download, DownloadDTO>(s => s.Id.Equals(id) && s.ModuleId.Equals(moduleId) && s.TopicId.Equals(TopicId), true);
            return Page();
        }
        #endregion
    }
}