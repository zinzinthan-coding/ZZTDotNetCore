namespace ZZTDotNetCore.Log4netRestApi.Models
{
    public class BlogListResponseModel
    {
        public bool IsSuccess { get; set; } 
        public string Message { get; set; }
        public List<BlogDataModel> Data { get; set; }
    }
    public class BlogResponseModel
    {
        public bool IsSuccess { get; set; } 
        public string Message { get; set; }
        public BlogDataModel Data { get; set; }
    }

}
