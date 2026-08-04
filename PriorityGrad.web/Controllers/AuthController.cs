using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PriorityGrad.domain.Models;
using PriorityGrad.infrastructure.Data;
using System.Linq;
using System.Collections.Generic;
using System;

namespace PriorityGrad.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult VerificarLogin(string email, string nombre)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("Login");
            }

            string emailTrimmed = email.Trim();

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == emailTrimmed);

            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioCorreo", usuario.Email);
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetString("UsuarioFoto", usuario.FotoPerfil ?? "");

                return RedirectToAction("Index", "Tarea");
            }
            else
            {
                return RedirectToAction("CompletarRegistro", new { email, nombre });
            }
        }

        [HttpGet]
        public IActionResult CompletarRegistro(string email, string nombre)
        {
            ViewBag.Email = email;
            ViewBag.Nombre = nombre;
            return View();
        }

        [HttpPost]
        public IActionResult GuardarRegistro(string Correo, string Nombre, string Institucion, string Carrera, List<string> NombresMaterias, List<string> Profesores, List<string> Colores)
        {
            if (string.IsNullOrWhiteSpace(Correo))
            {
                return RedirectToAction("Login");
            }

            string correoTrimmed = Correo.Trim();

            var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Email == correoTrimmed);
            if (usuarioExistente != null)
            {
                HttpContext.Session.SetString("UsuarioCorreo", usuarioExistente.Email);
                HttpContext.Session.SetString("UsuarioNombre", usuarioExistente.Nombre);
                HttpContext.Session.SetString("UsuarioFoto", usuarioExistente.FotoPerfil ?? "");
                return RedirectToAction("Index", "Tarea");
            }

            var nuevoUsuario = new Usuario
            {
                Email = correoTrimmed,
                Nombre = Nombre ?? "Estudiante",
                Institucion = Institucion ?? "",
                CarreraGrado = Carrera ?? "",
                FotoPerfil = "https://i.imgur.com/71916rK.png"
            };

            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();

            HttpContext.Session.SetString("UsuarioCorreo", nuevoUsuario.Email);
            HttpContext.Session.SetString("UsuarioNombre", nuevoUsuario.Nombre);
            HttpContext.Session.SetString("UsuarioFoto", nuevoUsuario.FotoPerfil);

            return RedirectToAction("Index", "Tarea");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}