using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Common.DTOModels.Admin;
using Common.Entities;
using Common.Extensions;
using Common.Services;

namespace Admin.Pages.Medias
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public MediaDTO Input { get; set; } = new MediaDTO();
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public EditModel(IAdminService db)
        {
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<IActionResult> OnGetAsync(int id, int moduleId, int topicId)
        {
            try
            {
                Alert = string.Empty;
                ViewData["Modules"] = (await _db.GetAsync<Module, ModuleDTO>(true)).ToSelectList("Id", "TopicAndModule");
                Input = await _db.SingleAsync<Media, MediaDTO>(s => s.Id.Equals(id) && s.ModuleId.Equals(moduleId) && s.TopicId.Equals(topicId), true);
                return Page();
            }
            catch
            {
                return RedirectToPage("/Index", new { alert = "You do not have access to this page." }); ;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var id = Input.ModuleId;
                Input.TopicId = (await _db.SingleAsync<Module, ModuleDTO>(s => s.Id.Equals(id))).TopicId;
                var succeeded = await _db.UpdateAsync<MediaDTO, Media>(Input);
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Updated Topic: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }

            // Reload the modules when the page is reloaded
            ViewData["Modules"] = (await _db.GetAsync<Module, ModuleDTO>(true)).ToSelectList("Id", "TopicAndModule");
            // Something failed, redisplay the form.
            return Page();
        }
        #endregion
    }
}