using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Common.DTOModels.UI;
using Common.Entities;
using Database.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.Models.MembershipViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Net;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;


namespace UI.Controllers
{
    public class MembershipController : Controller
    {
        #region Properties
        private readonly string _userId;
        private readonly IMapper _mapper;
        private readonly IUIReadService _db;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructor
        public MembershipController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, UserManager<VODUser> userManager, IMapper mapper, IUIReadService db)
        {
            var user = httpContextAccessor.HttpContext.User;
            _userId = userManager.GetUserId(user);
            _mapper = mapper;
            _db = db;
            _httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Action Methods
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var courseDtoObjects = _mapper.Map<List<TopicDTO>>(await _db.GetTopics(_userId));
            var dashboardModel = new DashboardViewModel();
            dashboardModel.topicDTOs = new List<List<TopicDTO>>();

            var noOfRows = courseDtoObjects.Count <= 3 ? 1 : courseDtoObjects.Count / 3;
            for (var i = 0; i < noOfRows; i++)
            {
                dashboardModel.topicDTOs.Add(courseDtoObjects.Skip(i * 3).Take(3).ToList());
            }

            return View(dashboardModel);
        }

        [HttpGet]
        public async Task<IActionResult> Topic(int id)
        {
            if (ModelState.IsValid)
            {

                var topicDot = await _db.GetTopic(_userId, id);
                var mappedTopicDTO = _mapper.Map<TopicDTO>(topicDot);
                var mappedTopicTypeDTO = _mapper.Map<TopicTypeDotDTO>(topicDot.TopicType);
                var mappedModuleDTOs = _mapper.Map<List<ModuleDTO>>(topicDot.Modules);

                var courseModel = new TopicViewModel
                {
                    TopicDTO = mappedTopicDTO,
                    TopicTypeDTO = mappedTopicTypeDTO,
                    Modules = mappedModuleDTOs
                };

                return View(courseModel);
            }


            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> Media(int id)
        {
            if (ModelState.IsValid)
            {
                var Media = await _db.GetMedia(_userId, id);
                var course = await _db.GetTopic(_userId, Media.TopicId);
                var videoDTO = _mapper.Map<MediaDTO>(Media);
                var courseDTO = _mapper.Map<TopicDTO>(course);
                var instructorDTO = _mapper.Map<TopicTypeDotDTO>(course.TopicType);

                var videos = (await _db.GetMedias(_userId, Media.ModuleId)).OrderBy(o => o.Id).ToList();
                var count = videos.Count();
                var index = videos.FindIndex(v => v.Id.Equals(id));

                var previous = videos.ElementAtOrDefault(index - 1);
                var previousId = previous == null ? 0 : previous.Id;

                var next = videos.ElementAtOrDefault(index + 1);
                var nextId = next == null ? 0 : next.Id;
                var nextTitle = next == null ? string.Empty : next.Title;
                var nextThumb = next == null ? string.Empty : next.Thumbnail;

                var videoModel = new MediaViewModel
                {
                    Title = Media.Module.Title,
                    mediaDTO = videoDTO,
                    TopicTypeDTO = instructorDTO,
                    topicDTO = courseDTO,
                    LessonInfo = new LessonInfoDTO
                    {
                        LessonNumber = index + 1,
                        NumberOfLessons = count,
                        NextVideoId = nextId,
                        PreviousVideoId = previousId,
                        NextVideoTitle = nextTitle,
                        NextVideoThumbnail = nextThumb,
                        CurrentVideoTitle = Media.Title,
                        CurrentVideoThumbnail = Media.Thumbnail

                    }
                };

                return View(videoModel);
            }
            return NoContent();
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> ProcessImage([FromBody] ImageUrlRequest request)
        {
            if (string.IsNullOrEmpty(request.Url))
                return BadRequest("No URL provided.");

            try
            {
                // Tải ảnh từ URL
                using var client = new WebClient();
                byte[] imageData = await client.DownloadDataTaskAsync(request.Url);
                using var originalImage = Image.Load(imageData);

                // Xử lý ảnh theo bộ lọc yêu cầu
                Image processedImage = null;
                switch (request.Filter.ToLower())
                {
                    case "grayscale":
                        processedImage = originalImage.Clone(ctx => ctx.Grayscale());
                        break;
                    case "sharpen":
                        processedImage = originalImage.Clone(ctx => ctx.GaussianSharpen());
                        break;
                    case "blur":
                        processedImage = originalImage.Clone(ctx => ctx.GaussianBlur());
                        break;
                    case "resize":
                        processedImage = originalImage.Clone(ctx => ctx.Resize(100, 100));
                        break;
                    default:
                        processedImage = originalImage;
                        break;
                }

                // Lưu ảnh đã xử lý vào MemoryStream và trả về
                using var resultStream = new MemoryStream();
                processedImage.Save(resultStream, new PngEncoder());
                resultStream.Seek(0, SeekOrigin.Begin);

                return File(resultStream.ToArray(), "image/png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        public Image SharpenImage(Image image)
        {
            image.Mutate(x => x.GaussianSharpen());
            return image;
        }
  

        public class ImageUrlRequest
        {
            public string Url { get; set; }
            public string Filter { get; set; }
        }
    }
}