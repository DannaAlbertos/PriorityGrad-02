using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PriorityGrad.infrastructure.Data;
using PriorityGrad.domain.Models;
using PriorityGrad.web.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace PriorityGrad.web.Controllers
{
    [Route("Perfil")]
    public class PerfilController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public PerfilController(IWebHostEnvironment env, IConfiguration configuration, AppDbContext context)
        {
            _env = env;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            if (string.IsNullOrEmpty(correo))
            {
                return RedirectToAction("Login", "Auth");
            }

            var usuario = _context.Usuarios
                                  .Include(u => u.Materias)
                                  .FirstOrDefault(u => u.Email == correo);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Usuario = usuario;

            var materiasParaVista = usuario.Materias != null
                ? usuario.Materias.Select(m => new MateriaInfo
                {
                    Nombre = m.Nombre,
                    Profesor = m.NombreProfesor,
                    Color = m.ColorHex
                }).ToList()
                : new List<MateriaInfo>();

            ViewBag.MisMaterias = materiasParaVista;

            // Creamos el modelo para la vista de perfil asegurando que pase la foto actual
            var modelo = new PerfilViewModel
            {
                FotoPerfilUrl = usuario.FotoPerfil
            };

            return View(modelo);
        }

        [HttpPost("ActualizarFoto")]
        public IActionResult ActualizarFoto(string fotoPerfilUrl)
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            if (string.IsNullOrEmpty(correo))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!string.IsNullOrWhiteSpace(fotoPerfilUrl))
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == correo);
                if (usuario != null)
                {
                    usuario.FotoPerfil = fotoPerfilUrl.Trim();
                    _context.SaveChanges();
                }

                HttpContext.Session.SetString("UsuarioFoto", fotoPerfilUrl.Trim());
            }

            return RedirectToAction("Index");
        }

        [HttpPost("SubirFoto")]
        public async Task<IActionResult> SubirFoto(IFormFile archivoFoto)
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            if (string.IsNullOrEmpty(correo))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (archivoFoto != null && archivoFoto.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + archivoFoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await archivoFoto.CopyToAsync(fileStream);
                }

                string rutaRelativa = "/uploads/" + uniqueFileName;

                var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == correo);
                if (usuario != null)
                {
                    usuario.FotoPerfil = rutaRelativa;
                    _context.SaveChanges();
                }

                HttpContext.Session.SetString("UsuarioFoto", rutaRelativa);
            }

            return RedirectToAction("Index");
        }

        [HttpPost("ActualizarMaterias")]
        public IActionResult ActualizarMaterias(List<string> materiaEditada, List<string> profesorEditado, List<string> colorEditado)
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            if (string.IsNullOrEmpty(correo))
            {
                return RedirectToAction("Login", "Auth");
            }

            var usuario = _context.Usuarios
                                  .Include(u => u.Materias)
                                  .FirstOrDefault(u => u.Email == correo);

            if (usuario != null)
            {
                if (usuario.Materias != null && usuario.Materias.Any())
                {
                    _context.RemoveRange(usuario.Materias);
                }

                usuario.Materias = new List<MateriaConfig>();

                if (materiaEditada != null)
                {
                    for (int i = 0; i < materiaEditada.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(materiaEditada[i]))
                        {
                            usuario.Materias.Add(new MateriaConfig
                            {
                                Nombre = materiaEditada[i],
                                NombreProfesor = profesorEditado != null && profesorEditado.Count > i ? (profesorEditado[i] ?? string.Empty) : string.Empty,
                                ColorHex = colorEditado != null && colorEditado.Count > i ? colorEditado[i] : "#0d6efd",
                                ColorTexto = "#ffffff"
                            });
                        }
                    }
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}