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
    public class DeleteModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public TopicTypeDTO Input { get; set; } = new TopicTypeDTO();
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public DeleteModel(IAdminService db)
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
                Input = await _db.SingleAsync<TopicType, TopicTypeDTO>(s => s.Id.Equals(id));
                return Page();
            }
            catch
            {
                return RedirectToPage("/Index", new { alert = "You do not have access to this page." });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = Input.Id;

            if (ModelState.IsValid)
            {
                var succeeded = await _db.DeleteAsync<TopicType>(d => d.Id.Equals(id));
                if (succeeded)
                {
                    Alert = $"Deleted Instructor: {Input.Name}.";
                    return RedirectToPage("Index");
                }
            }

            Input = await _db.SingleAsync<TopicType, TopicTypeDTO>(s => s.Id.Equals(id));
            return Page();
        }
        #endregion
    }
}