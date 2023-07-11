using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication2.Models;
using static System.Net.WebRequestMethods;

namespace WebApplication2.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            List<Post> posts = new List<Post>();
            using (MyBlogContext context = new MyBlogContext())
            {
                posts = context.Posts
                    .Include((p) => p.Author).ToList();
            }
            return View(posts);
        }

        public IActionResult Search(String title)
        {
            List<Post> posts = new List<Post>();
            using (MyBlogContext context = new MyBlogContext())
            {
                posts = context.Posts
                    .Where(p => p.Title.ToLower().Contains(title.ToLower()))
                    .Include((p) => p.Author).ToList();
            }
            return View("/Views/Blog/Index.cshtml", posts);
        }


        public IActionResult Post(int id = 0)
        {

            Post post = null;
            using (MyBlogContext context = new MyBlogContext())
            {
                post = context.Posts
                    .Include((p) => p.Author)
                    .FirstOrDefault((post) => post.Id == id);
            }

            return View(post);
        }

        public IActionResult Add()
        {
            return View();
        }

       //Upload Post 

        [HttpPost]
        public IActionResult UploadPost(Post post, IFormFile multipartFile)
        {
            var claims = User.Claims;
            String claim = claims.ElementAt(0).Subject.Name;
            if (ModelState.IsValid)
            {
                using (MyBlogContext context = new MyBlogContext())
                {
                    try
                    {
                        string location = UploadPhoto(multipartFile);
                        User author = context.Users.FirstOrDefault((user) => user.Email.Equals(claim));
                        post.Author = author;
                        post.AuthorId = author.Id;
                        post.CreateAt = DateTime.Now;
                        if(location != null) {
                        post.ImagePreview = location;
                        } else
                        {
                            post.ImagePreview = "https://as1.ftcdn.net/v2/jpg/02/48/42/64/1000_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg";
                        }
                        context.Posts.Add(post);

                        if (context.SaveChanges() > 0)
                        {
                            Console.WriteLine("Save succefully");

                        }
                        else
                        {
                            Console.WriteLine("Save failed");

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
                Console.WriteLine(errors);
                return View("Add");
            }

            return RedirectToAction("Index");
        }

        //Upload file using in rich text editor


        [HttpPost]
        public async Task<JsonResult> Upload(IFormFile file)
        {

            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileSrteam);

                }
                return Json(new { location = "/images/" + fileName });
            }
            return Json(new { });
        }

        //Upload file using in uploading preview image when submiting form

        public String UploadPhoto(IFormFile file)
        {


            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileSrteam);

                }
                return "/images/" + fileName;
            }
            return null;
        }

        //Get Edit form
        public IActionResult Edit(int id)
        {
            using(MyBlogContext ctx = new MyBlogContext())
            {
                Post post = ctx.Posts.FirstOrDefault(p => p.Id == id);
                return View(post);
            }
        }

        //Update Post 
        [HttpPost]
        public IActionResult Edit(Post post, IFormFile? multipartFile)
        {
            if(ModelState.IsValid)
            {
                using(var ctx = new MyBlogContext())
                {
                    
                  
                    Post postUpdate = ctx.Posts.FirstOrDefault(p => p.Id == post.Id);
                    if (multipartFile != null)
                    {
                        postUpdate.ImagePreview = UploadPhoto(multipartFile);
                    } 
                    postUpdate.Summary = post.Summary;
                    postUpdate.Content = post.Content;
                    postUpdate.Title = post.Title;
                    ctx.SaveChanges();
                }

                return RedirectToAction("Post", new {id = post.Id});
            } else
            {
                using (var ctx = new MyBlogContext())
                { 
                    Post postUpdate = ctx.Posts.FirstOrDefault(p => p.Id == post.Id);
                    post.ImagePreview = postUpdate.ImagePreview;
                }
                return View(post);
            }
        }





    }
}
