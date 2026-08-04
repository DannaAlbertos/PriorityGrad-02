using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PriorityGrad.domain.Models;
using PriorityGrad.infrastructure.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult GuardarRegistro(string Correo, string Nombre, string Institucion, string Carrera, string FotoPerfilUrl, List<string> NombresMaterias, List<string> Profesores, List<string> Colores)
        {
            if (string.IsNullOrWhiteSpace(Correo))
            {
                return RedirectToAction("Login");
            }

            string correoTrimmed = Correo.Trim();

            var usuarioExistente = _context.Usuarios
                                  .Include(u => u.Materias)
                                  .FirstOrDefault(u => u.Email == correoTrimmed);

            if (usuarioExistente != null)
            {
                HttpContext.Session.SetString("UsuarioCorreo", usuarioExistente.Email);
                HttpContext.Session.SetString("UsuarioNombre", usuarioExistente.Nombre);
                HttpContext.Session.SetString("UsuarioFoto", usuarioExistente.FotoPerfil ?? "");
                return RedirectToAction("Index", "Tarea");
            }

            // Crear el nuevo usuario incluyendo la foto personalizada y lista de materias
            var nuevoUsuario = new Usuario
            {
                Email = correoTrimmed,
                Nombre = Nombre ?? "Estudiante",
                Institucion = Institucion ?? "",
                CarreraGrado = Carrera ?? "",
                FotoPerfil = !string.IsNullOrWhiteSpace(FotoPerfilUrl) ? FotoPerfilUrl.Trim() : "https://i.imgur.com/71916rK.png",
                Materias = new List<MateriaConfig>()
            };

            // Registrar correctamente las materias dinámicas enviadas desde el formulario
            if (NombresMaterias != null)
            {
                for (int i = 0; i < NombresMaterias.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(NombresMaterias[i]))
                    {
                        nuevoUsuario.Materias.Add(new MateriaConfig
                        {
                            Nombre = NombresMaterias[i],
                            NombreProfesor = Profesores != null && Profesores.Count > i ? (Profesores[i] ?? string.Empty) : string.Empty,
                            ColorHex = Colores != null && Colores.Count > i && !string.IsNullOrEmpty(Colores[i]) ? Colores[i] : "#0d6efd",
                            ColorTexto = "#ffffff"
                        });
                    }
                }
            }

            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();

            // Establecer sesión activa
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