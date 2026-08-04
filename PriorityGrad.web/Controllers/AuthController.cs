using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PriorityGrad.domain.Models; // Ajusta según tu espacio de nombres de Usuario
using PriorityGrad.infrastructure.Data; // Tu AppDbContext
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

            // Buscar el usuario en PostgreSQL usando Entity Framework
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == emailTrimmed);

            if (usuario != null)
            {
                // Cargar sesión con los datos reales de la BD
                HttpContext.Session.SetString("UsuarioCorreo", usuario.Email);
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);

                return RedirectToAction("Index", "Tarea");
            }
            else
            {
                // Si no existe, redirigir al onboarding de registro inicial
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

            // Verificar si ya existe para evitar duplicados
            var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Email == correoTrimmed);
            if (usuarioExistente != null)
            {
                // Opcional: actualizar o redirigir directamente
                HttpContext.Session.SetString("UsuarioCorreo", usuarioExistente.Email);
                HttpContext.Session.SetString("UsuarioNombre", usuarioExistente.Nombre);
                return RedirectToAction("Index", "Tarea");
            }

            // Crear el nuevo usuario para la Base de Datos
            var nuevoUsuario = new Usuario
            {
                Email = correoTrimmed,
                Nombre = Nombre ?? "Estudiante",
                Institucion = Institucion ?? "",
                CarreraGrado = Carrera ?? "",
                FotoPerfil = "/uploads/default.png" // O tu ruta por defecto
            };

            // Guardar el usuario en PostgreSQL
            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges(); // ¡Esto lo escribe permanentemente en la BD!

            // Establecer sesión activa
            HttpContext.Session.SetString("UsuarioCorreo", nuevoUsuario.Email);
            HttpContext.Session.SetString("UsuarioNombre", nuevoUsuario.Nombre);

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