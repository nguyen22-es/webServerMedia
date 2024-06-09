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
    public class CreateModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public TopicDTO Input { get; set; } = new TopicDTO();
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public CreateModel(IAdminService db)
        {
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ViewData["TopicType"] = (await _db.GetAsync<Common.Entities.TopicType, TopicTypeDTO>()).ToSelectList("Id", "Name");
                return Page();
            }
            catch
            {
                return RedirectToPage("/Index", new { alert = "You do not have access to this page." });
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var succeeded = await _db.CreateAsync<TopicDTO, Topic>(Input) > 0;
                if (succeeded)
                {
                    Alert = $"Created a new Course: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }

            ViewData["TopicType"] = (await _db.GetAsync<Common.Entities.TopicType, TopicTypeDTO>()).ToSelectList("Id", "Name");
            return Page();
        }
        #endregion
    }
}