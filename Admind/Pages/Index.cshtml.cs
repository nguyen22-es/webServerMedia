using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models;
using Database.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Admin.Pages
{
    public class IndexModel : PageModel
    {
        public (CardViewModel TopicTypes, CardViewModel Users,
                CardViewModel Topics, CardViewModel Modules,
                CardViewModel Medias, CardViewModel Downloads) Cards;

        private readonly IDbReadService _db;
        [TempData] public string Alert { get; set; }

        public IndexModel(IDbReadService db)
        {
            _db = db;
        }

        public void OnGet()
        {
            var (topics, downloads, topicTypes, modules, medias, users) = _db.Count();

            Cards = (
                TopicTypes: new CardViewModel
                {
                    BackgroundColor = "#9c27b0",
                    Count = topicTypes,
                    Description = "topicTypes",
                    Icon = "person",
                    Url = "./TopicTypes/Index"
                },
                Users: new CardViewModel
                {
                    BackgroundColor = "#414141",
                    Count = users,
                    Description = "Users",
                    Icon = "people",
                    Url = "./Users/Index"
                },
                Topics: new CardViewModel
                {
                    BackgroundColor = "#009688",
                    Count = topics,
                    Description = "topics",
                    Icon = "subscriptions",
                    Url = "./Topics/Index"
                },
                Modules: new CardViewModel
                {
                    BackgroundColor = "#f44336",
                    Count = modules,
                    Description = "Modules",
                    Icon = "list",
                    Url = "./Modules/Index"
                },
                Videos: new CardViewModel
                {
                    BackgroundColor = "#3f51b5",
                    Count = medias,
                    Description = "Medias",
                    Icon = "theaters",
                    Url = "./Medias/Index"
                },
                Downloads: new CardViewModel
                {
                    BackgroundColor = "#ffcc00",
                    Count = downloads,
                    Description = "Downloads",
                    Icon = "import_contacts",
                    Url = "./Downloads/Index"
                }
            );

        }
    }
}
