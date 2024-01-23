using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.Json;
using ZZTDotNetCore.Log4netRestApi.Models;

namespace ZZTDotNetCore.Log4netRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILog _logger;
        //log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(BlogController));

		public BlogController(AppDbContext context,ILog logger)
		{
			_context = context;
            _logger = logger;
		}

		[HttpGet]
        public IActionResult GetBlogs()
        {
            //try
            //{
            //    AppDbContext db = new AppDbContext();
            //    List<BlogDataModel> lst =_context.Blogs.ToList();
            //    BlogListResponseModel model = new BlogListResponseModel
            //    {
            //        IsSuccess = true,
            //        Message = "Success",
            //        Data = lst
            //    };
            //    return Ok(model);
            //}
            //catch (Exception ex)
            //{
            //    return Ok(new BlogListResponseModel
            //    {
            //        IsSuccess = false,
            //        Message = ex.Message // [summary error]
            //     // Message = ex.ToString() [detail error]
            //    });
            //}

            List<BlogDataModel> lst =_context.Blogs.ToList();
            _logger.Info(JsonSerializer.Serialize(lst));
            BlogListResponseModel model = new BlogListResponseModel
            {
                IsSuccess = true,
                Message = "Success",
                Data = lst
            };
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            _logger.Info(JsonSerializer.Serialize(item));
            if (item is null)
            {
                // return NotFound(new { IsSuccess = false, Message="No data found."}) ;
                var response = new { IsSuccess = false, Message = "No data found." };
                _logger.Error(JsonSerializer.Serialize(response));
                return NotFound(response);
            }
            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = true,
                Message = "Success",
                Data = item
            };
            return Ok(model);
        }

        [HttpPost]
        public IActionResult CreateBlog(BlogDataModel blog)
        {
            _logger.Info(JsonSerializer.Serialize(blog));
            _context.Blogs.Add(blog);
            var result = _context.SaveChanges();
            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Saving Successful." : "Saving Failed.",
                Data = blog
            };
            _logger.Info(JsonSerializer.Serialize(model));
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogDataModel blog)
        {
            _logger.Info(JsonSerializer.Serialize(blog));
            var item = _context.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item is null)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                _logger.Info(JsonSerializer.Serialize(response));
                return NotFound(response);
            }

            item.Blog_Title = blog.Blog_Title;
            item.Blog_Author = blog.Blog_Author;
            item.Blog_Content = blog.Blog_Content;

            var result = _context.SaveChanges();
            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Update Successful." : "Updating Failed.",
                Data = blog
            };
            _logger.Info(JsonSerializer.Serialize(model));
            return Ok(model);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogDataModel blog)
        {
            _logger.Info(JsonSerializer.Serialize(blog));
            var item = _context.Blogs.FirstOrDefault(x => x.Blog_Id == id);

            if (item is null)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                _logger.Error(JsonSerializer.Serialize(response));
                return NotFound(response);
            }

            if (!string.IsNullOrEmpty(blog.Blog_Title))
            {
                item.Blog_Title = blog.Blog_Title;
            }
            if (!string.IsNullOrEmpty(blog.Blog_Author))
            {
                item.Blog_Author = blog.Blog_Author;
            }
            if (!string.IsNullOrEmpty(blog.Blog_Content))
            {
                item.Blog_Content = blog.Blog_Content;
            }

            var result = _context.SaveChanges();
            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Updating Successful." : "Updating Failed.",
                Data = item
            };
            _logger.Info(JsonSerializer.Serialize(model));
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            _logger.Info(JsonSerializer.Serialize(id));
            var item = _context.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item is null)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                _logger.Info(JsonSerializer.Serialize(response));
                return NotFound(response);
            }

            _context.Blogs.Remove(item);
            var result = _context.SaveChanges();
            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Deleting Successful." : "Deleting Failed."
            };
            _logger.Info(JsonSerializer.Serialize(model));
            return Ok(model);
        }
    }
}
