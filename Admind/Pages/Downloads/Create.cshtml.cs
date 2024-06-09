using System.Threading.Tasks;
using Common.DTOModels.Admin;
using Common.Entities;
using Common.Extensions;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Admin.Pages.Downloads
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public DownloadDTO Input { get; set; } = new DownloadDTO();
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
                ViewData["Modules"] = (await _db.GetAsync<Module, ModuleDTO>(true)).ToSelectList("Id", "TopicAndModule");
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
                var id = Input.ModuleId;
                Input.TopicId = (await _db.SingleAsync<Module, ModuleDTO>(s => s.Id.Equals(id))).TopicId;
                var succeeded = await _db.CreateAsync<DownloadDTO, Download>(Input) > 0;
                if (succeeded)
                {
                    Alert = $"Created a new Download: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }

            ViewData["Modules"] = (await _db.GetAsync<Module, ModuleDTO>(true)).ToSelectList("Id", "TopicAndModule");
            return Page();
        }
        #endregion
    }
}