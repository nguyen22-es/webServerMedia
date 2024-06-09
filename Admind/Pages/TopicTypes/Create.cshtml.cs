using System.Threading.Tasks;
using Common.DTOModels.Admin;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Admin.Pages.TopicTypes
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public TopicTypeDTO Input { get; set; } = new TopicTypeDTO();
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public CreateModel(IAdminService db)
        {
            _db = db;
        }
        #endregion

        #region Actions
        public void OnGetAsync()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var succeeded = await _db.CreateAsync<TopicTypeDTO, TopicType>(Input) > 0;
                if (succeeded)
                {
                    Alert = $"Created a new Instructor: {Input.Name}.";
                    return RedirectToPage("Index");
                }
            }

            return Page();
        }
        #endregion
    }
}