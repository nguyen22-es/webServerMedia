using System.Collections.Generic;
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
    public class IndexModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        public IEnumerable<TopicTypeDTO> Items = new List<TopicTypeDTO>();
        [TempData] public string Alert { get; set; }

        #endregion

        #region Constructor
        public IndexModel(IAdminService db)
        {
            _db = db;
        }
        #endregion

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Items = await _db.GetAsync<TopicType, TopicTypeDTO>(true);
                return Page();
            }
            catch
            {
                Alert = "You do not have access to this page.";
                return RedirectToPage("/Index");
            }
        }

    }
}