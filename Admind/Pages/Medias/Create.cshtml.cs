using System.Threading.Tasks;
using Common.DTOModels.Admin;
using Common.Entities;
using Common.Extensions;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Admin.Pages.Medias
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        private readonly GoogleService _googleService;
        [BindProperty] public MediaDTO Input { get; set; } = new MediaDTO();
        
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public CreateModel(IAdminService db, GoogleService googleService)
        {
            _db = db;
            _googleService = googleService; 
        }
        #endregion

        #region Actions
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ViewData["Modules"] = (await _db.GetAsync<Module, ModuleDTO>(true)).ToSelectList("Id", "TopicAndModule");
                var i = (await _db.GetAsync<Module, ModuleDTO>(true)).ToSelectList("Id", "TopicAndModule");
                foreach (var item in i)
                {
                    Console.WriteLine(item);
                }
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

                var url =   await  _googleService.UploadFileToGoogleCloudStorage(Input.File);
                Input.Url = url.MediaLink;

                var succeeded = await _db.CreateAsync<MediaDTO, Media>(Input) > 0;
                if (succeeded)
                {
                    Alert = $"Created a new Media: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }

            ViewData["Modules"] = (await _db.GetAsync<Module, ModuleDTO>(true)).ToSelectList("Id", "TopicAndModule");
            return Page();
        }
        #endregion
    }
}


//string fileName = Path.GetFileName(fileupload.FileName);
//int fileSize = Convert.ToInt32(fileupload.Length);
//int size = fileSize / 1000;

//string uploadPath = Path.Combine(fileName);
//using (FileStream stream = new FileStream(uploadPath, FileMode.Create))
//{
//    fileupload.CopyTo(stream);
//}