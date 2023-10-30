using FullApp.Server.Data;
using FullApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FullApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly Data.DbContext _dbContext;

        public UsuarioController(Data.DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _dbContext.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _dbContext.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            _dbContext.Usuarios.Add(usuario);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            var existingUsuario = await _dbContext.Usuarios.FindAsync(id);
            if (existingUsuario == null)
            {
                return NotFound();
            }

            existingUsuario.Nombre = usuario.Nombre;
            existingUsuario.Apellido1 = usuario.Apellido1;
            existingUsuario.Apellido2 = usuario.Apellido2;
            existingUsuario.TelefonoMovil = usuario.TelefonoMovil;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _dbContext.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _dbContext.Usuarios.Remove(usuario);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


        private bool UsuarioExists(int id)
        {
            return _dbContext.Usuarios.Any(e => e.Id == id);
        }
    }
}
