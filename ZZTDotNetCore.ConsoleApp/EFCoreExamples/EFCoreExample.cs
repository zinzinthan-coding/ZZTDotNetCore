using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using ZZTDotNetCore.ConsoleApp.Models;

namespace ZZTDotNetCore.ConsoleApp.EFCoreExamples
{
    public class EFCoreExample
    {
        private readonly AppDbContext _dbContext;
        public EFCoreExample()
        { 
           _dbContext = new AppDbContext();
        }

        public void Run()
        {
            Read();
            //Edit(9);
            //Create("test 1", "test 1", "test 1");
            //Update(1007, "test", "test", "test");
            //Delete(1007);
        }

        private void Read()
        {
            #region Read/Retrieve

            List<BlogDataModel> lst = _dbContext.Blogs.ToList();
            Log.Information("Blog List => " + JsonSerializer.Serialize(lst));
            foreach (var item in lst)
            {
                Console.WriteLine(item.Blog_Id);
                Console.WriteLine(item.Blog_Title);
                Console.WriteLine(item.Blog_Author);
                Console.WriteLine(item.Blog_Content);
            }

            #endregion
        }

        private void Edit(int id)
        {
            #region Edit

            //BlogDataModel? item =_dbContext.Blogs.Where(x => x.Blog_Id == id).FirstOrDefault();
            BlogDataModel? item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            Log.Information("Single Blog => " + JsonSerializer.Serialize(item));
            /* 
            foreach (var x in _dbContext.Blogs.ToList())
            {
                if (x.Blog_Id == id)
                    return;
            }
            */

            if (item is null)
            {
                Console.WriteLine("No data found.");
                return;
            }

            Console.WriteLine(item.Blog_Id);
            Console.WriteLine(item.Blog_Title);
            Console.WriteLine(item.Blog_Author);
            Console.WriteLine(item.Blog_Content);

            #endregion 
        }

        private void Create(string title, string author, string content)
        {
            #region Create

            BlogDataModel blog = new BlogDataModel
            {
                Blog_Title = title,
                Blog_Author = author,
                Blog_Content = content
            };
            Log.Information("User input blog => " + JsonSerializer.Serialize(blog));
            //_dbContext.Add(blog);
            _dbContext.Blogs.Add(blog);
            int result = _dbContext.SaveChanges();
            string message = result > 0 ? "Saving Successful." : "Saving Failed.";
            Console.WriteLine(message);
            Console.WriteLine(blog.Blog_Id);
            Log.Information("Blog Create => {message}", message);
            #endregion
        }

        private void Update(int id, string title, string author, string content)
        {
            #region Update

            BlogDataModel? blog = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if(blog is null)
            {
                Console.WriteLine("No data found.");
                return;
            }
            blog.Blog_Title = title;
            blog.Blog_Author = author;
            blog.Blog_Content = content;
            Log.Information("User input blog => " + JsonSerializer.Serialize(blog));
            int result = _dbContext.SaveChanges();
            string message = result > 0 ? "Update Successful." : "Update Failed.";
            Console.WriteLine(message);
            Log.Information("Blog Update => {message}", message);
            #endregion
        }

        private void Delete(int id)
        {
             #region Delete

            var blog = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            Log.Information("User input blog => " + JsonSerializer.Serialize(blog));
            if (blog is null)
            {
                Console.WriteLine("No data found.");
                return;
            }

            _dbContext.Blogs.Remove(blog);
            int result = _dbContext.SaveChanges();
            string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";
            Console.WriteLine(message);
            Log.Information("Blog Update => {message}", message);
            #endregion
        }
    }
}
