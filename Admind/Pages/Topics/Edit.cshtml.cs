using System.Threading.Tasks;
using Common.DTOModels.Admin;
using Common.Entities;
using Common.Extensions;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Admin.Pages.Topics
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public TopicDTO Input { get; set; } = new TopicDTO();
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public EditModel(IAdminService db)
        {
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Alert = string.Empty;
                ViewData["TopicType"] = (await _db.GetAsync<Common.Entities.TopicType, TopicTypeDTO>()).ToSelectList("Id", "Name");
                Input = await _db.SingleAsync<Topic, TopicDTO>(s => s.Id.Equals(id));
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
                var succeeded = await _db.UpdateAsync<TopicDTO, Topic>(Input);
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Updated Topic: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }

            // Reload the modules when the page is reloaded
            ViewData["TopicType"] = (await _db.GetAsync<Common.Entities.TopicType, TopicTypeDTO>()).ToSelectList("Id", "Name");
            // Something failed, redisplay the form.
            return Page();
        }
        #endregion
    }
}