using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.Json;
using ZZTDotNetCore.RestApi.Models;

namespace ZZTDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BlogController> _logger;

        public BlogController(AppDbContext context, ILogger<BlogController> logger)
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

            List<BlogDataModel> lst = _context.Blogs.ToList();
            _logger.LogInformation("Blog List => {@lst}", lst);
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
            _logger.LogInformation("Single Blog => {@item}", item);
            if (item is null)
            {
                // return NotFound(new { IsSuccess = false, Message="No data found."}) ;
                var response = new { IsSuccess = false, Message = "No data found." };
                _logger.LogError("User ID => {@response}", response);
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
            _logger.LogInformation("User input => {@blog}", blog);
            _context.Blogs.Add(blog);
            var result = _context.SaveChanges();
            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Saving Successful." : "Saving Failed.",
                Data = blog
            };
            _logger.LogInformation("Blog Create response message =>" + JsonSerializer.Serialize(model));
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogDataModel blog)
        {
            _logger.LogInformation("User input => {@blog}", blog);
            var item = _context.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item is null)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                _logger.LogError("User ID => {@response}", response);
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
            _logger.LogInformation("Blog Update response message =>" + JsonSerializer.Serialize(model));
            return Ok(model);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogDataModel blog)
        {
            _logger.LogInformation("User input => {@blog}", blog);
            var item = _context.Blogs.FirstOrDefault(x => x.Blog_Id == id);

            if (item is null)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                _logger.LogError("User ID => {@response}", response);
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
            _logger.LogInformation("Blog Patch response message =>", model);
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            _logger.LogInformation("User input ID=> {@id}", id);
            var item = _context.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item is null)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                _logger.LogError("User ID => {@response}", response);
                return NotFound(response);
            }

            _context.Blogs.Remove(item);
            var result = _context.SaveChanges();
            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Deleting Successful." : "Deleting Failed."
            };
            _logger.LogInformation("Blog Delete response message =>{@model}", model);
            return Ok(model);
        }
    }
}
