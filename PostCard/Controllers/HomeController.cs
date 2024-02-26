using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PostCard.Models;
using PostCard.Models.ViewModel;

namespace PostCard.Controllers;

public class HomeController : Controller
{
    private readonly AplicationContext _context;

    public HomeController(AplicationContext contexto)
    {
       
        _context = contexto;
    }
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await base.OnActionExecutionAsync(context, next);
        var userLogged = await LookForUser();
        ViewData["User"] = userLogged?.Name;
        ViewBag.Usuario = userLogged?.UsuarioId;
    }
    public async Task<IActionResult> Index(int? id)
    {

        ViewBag.Post = await GetPosts();
    
        
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CreateModel postModel)
    {

        if (ModelState.IsValid)
        {
            try
            {
                var post = new Post
                {
                    Title = postModel.Title,
                    Menssage = postModel.Menssage,
                    Tags = postModel.Tags,
                    UsuarioId = postModel.UsuarioId,
                    File = postModel.File
                };


                var postUpdate = await Convertir(post);
                if (postUpdate != null)
                {
                    _context.Add(postUpdate);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
            return RedirectToAction("Index");
        }


        ViewBag.Post = await GetPosts();
        return View(postModel);
    }
    public async Task<IActionResult> Editar(int? id)
    {

        ViewBag.Post = await GetPosts();


        if (id != null)
        {

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null) { return NotFound(); }
            var postModel = new EditPostModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Menssage = post.Menssage,
                Tags = post.Tags,
                Image = post.Image,
                UsuarioId = post.UsuarioId

            };
            return View(postModel);
        }
        else
        {
            return NotFound();
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(EditPostModel post_edit)
    {

        if (ModelState.IsValid)
        {
            var postConvertir = new Post
            {
                PostId = post_edit.PostId,
                Title = post_edit.Title,
                Menssage = post_edit.Menssage,
                Tags = post_edit.Tags,
                Image = post_edit.Image,
                UsuarioId = post_edit.UsuarioId,
                File = post_edit.File
            };


            var post_convertir = await Convertir(postConvertir);
            if (post_convertir != null)
            {
                _context.Update(post_convertir);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
        ViewBag.Post = await GetPosts();
        return View(post_edit);
    }
    public async Task AddLike(int postId)
    {

        var post = await _context.Posts.FindAsync(postId);
        if (post != null)
        {
            var userLogged = await LookForUser();

            if (userLogged != null)
            {
                var like = await _context.Likes.FirstOrDefaultAsync(p => p.PostId == postId && p.UsuarioId == userLogged.UsuarioId);
                if (like == null)
                {
                    var createLike = new Likes
                    {
                        PostId = post.PostId,
                        UsuarioId = userLogged.UsuarioId
                    };
                    _context.Add(createLike);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    _context.Likes.Remove(like);
                    await _context.SaveChangesAsync();
                }
            }
        }


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var post_delete = await _context.Posts.FindAsync(id);
        if (post_delete == null) { return NotFound(); }
        else
        {
            _context.Posts.Remove(post_delete);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

    //recibiendo los datos 

    public async Task<List<Post>> GetPosts()
    {
        var datos = await _context.Posts
            .Include(p => p.Usuarios)
            .Include(l => l.Likes)
            .ToListAsync();

        return datos;
    }



    //cerrar seccion
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Cookies.Delete("user");
        return RedirectToAction("Login", "Usuarios");
    }
    //Conviertiendo la imagen a binario
    public async Task<Post?> Convertir(Post postConvertModel)
    {


        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == postConvertModel.UsuarioId);
        if (user == null) { return null; }
        postConvertModel.Usuarios = user;
        byte[] bytes;
        if (postConvertModel.File != null)
        {
            using (Stream fs = postConvertModel.File.OpenReadStream())
            {
                using (BinaryReader br = new(fs))
                {
                    bytes = br.ReadBytes((int)fs.Length);
                    postConvertModel.Image = Convert.ToBase64String(bytes, 0, bytes.Length);
                    return postConvertModel;

                }
            }
        }
        return postConvertModel;

    }

    //Buscar el usuario logeado
    public async Task<Usuario?> LookForUser()
    {
       var userLoggedEmail = Request.Cookies["user"];
        Console.WriteLine(userLoggedEmail);
        var userFound = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == userLoggedEmail);
        return userFound;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
