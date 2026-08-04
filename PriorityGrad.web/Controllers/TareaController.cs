using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PriorityGrad.domain.Interfaces;
using PriorityGrad.domain.Models;
using PriorityGrad.web.Models;
using PriorityGrad.infrastructure.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace PriorityGrad.web.Controllers
{
    [Route("Tarea")]
    public class TareaController : Controller
    {
        private readonly ITareaRepository _repository;
        private readonly AppDbContext _context;

        public TareaController(ITareaRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index(string ordenarPor)
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            if (string.IsNullOrEmpty(correo))
            {
                return RedirectToAction("Login", "Auth");
            }

            var tareasDelUsuario = _repository.ObtenerTodas()
                                              .Where(t => t.UsuarioEmail == correo)
                                              .ToList();

            return View(AplicarOrdenamiento(tareasDelUsuario, ordenarPor));
        }

        [HttpGet("Vencidas")]
        public IActionResult Vencidas()
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            if (string.IsNullOrEmpty(correo))
            {
                return RedirectToAction("Login", "Auth");
            }

            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var tareasVencidas = _repository.ObtenerTodas()
                                            .Where(t => t.UsuarioEmail == correo && t.Fecha < hoy)
                                            .ToList();

            return View(tareasVencidas);
        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            var tarea = _repository.ObtenerTodas().FirstOrDefault(t => t.Id == id && t.UsuarioEmail == correo);

            return tarea == null ? NotFound() : View(tarea);
        }

        [HttpGet("Create")]
        public IActionResult Create()
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

            var listaMateriasInfo = usuario.Materias != null
                ? usuario.Materias.Select(m => new MateriaInfo
                {
                    Nombre = m.Nombre,
                    Profesor = m.NombreProfesor,
                    Color = m.ColorHex
                }).ToList()
                : new List<MateriaInfo>();

            ViewBag.MateriasDisponibles = listaMateriasInfo;

            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tarea nuevaTarea)
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            if (string.IsNullOrEmpty(correo))
            {
                return RedirectToAction("Login", "Auth");
            }

            nuevaTarea.UsuarioEmail = correo;

            if (!ModelState.IsValid)
            {
                var usuario = _context.Usuarios.Include(u => u.Materias).FirstOrDefault(u => u.Email == correo);
                ViewBag.MateriasDisponibles = usuario?.Materias?.Select(m => new MateriaInfo
                {
                    Nombre = m.Nombre,
                    Profesor = m.NombreProfesor,
                    Color = m.ColorHex
                }).ToList() ?? new List<MateriaInfo>();

                return View(nuevaTarea);
            }

            if (string.IsNullOrEmpty(nuevaTarea.ColorHex))
            {
                nuevaTarea.ColorHex = "#0d6efd";
            }

            _repository.Guardar(nuevaTarea);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            var tarea = _repository.ObtenerTodas().FirstOrDefault(t => t.Id == id && t.UsuarioEmail == correo);

            return tarea == null ? NotFound() : View(tarea);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Tarea tareaEditada, string? returnUrl)
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            if (string.IsNullOrEmpty(correo)) return RedirectToAction("Login", "Auth");

            if (id != tareaEditada.Id)
            {
                return NotFound();
            }

            tareaEditada.UsuarioEmail = correo;

            if (!ModelState.IsValid)
            {
                return View(tareaEditada);
            }

            _repository.Actualizar(tareaEditada);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id, string? returnUrl)
        {
            var correo = HttpContext.Session.GetString("UsuarioCorreo");
            var tarea = _repository.ObtenerTodas().FirstOrDefault(t => t.Id == id && t.UsuarioEmail == correo);

            if (tarea != null)
            {
                _repository.Eliminar(id);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        private List<Tarea> AplicarOrdenamiento(List<Tarea> lista, string criterio) => criterio switch
        {
            "valor" => lista.OrderByDescending(t => t.Valor).ThenByDescending(t => t.Dificultad).ToList(),
            "dificultad" => lista.OrderByDescending(t => t.Dificultad).ThenByDescending(t => t.Valor).ToList(),
            "fecha" => lista.OrderBy(t => t.Fecha).ToList(),
            _ => lista.OrderBy(t => t.Materia).ToList()
        };
    }
}