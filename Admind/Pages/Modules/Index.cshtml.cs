using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Common.DTOModels.Admin;
using Common.Entities;
using Common.Services;

namespace Admin.Pages.Modules
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        public IEnumerable<ModuleDTO> Items = new List<ModuleDTO>();
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
                Items = await _db.GetAsync<Module, ModuleDTO>(true);
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