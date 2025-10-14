using Comandas.Api.DTOs;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //lista usuarios
        static List<Usuario> usuarios = new List<Usuario>() {
            new Usuario
            {
                Id = 1,
                Nome = "Admin",
                Email = "admin@admin.com",
                Senha = "admin",
            },
            new Usuario
            {
                Id = 2,
                Nome = "User",
                Email = "user@user.com",
                Senha = "user",
            }
        };
        // GET: api/<UsuarioController>
        [HttpGet]
        public IResult Get()
        {
            return Results.Ok(usuarios);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario is null)
            {
                return Results.NotFound("Usuário Não Encontrado");
            }
            return Results.Ok(usuario);
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public IResult Post([FromBody] UsuarioCreateRequest usuarioCreate)
        {
            if (usuarioCreate.Senha.Length < 6)
            {
                return Results.BadRequest("A senha deve ter no mínimo 6 caracteres.");
            }
            if (usuarioCreate.Nome.Length < 3)
            {
                return Results.BadRequest("O nome deve ter no mínimo 3 caracteres.");
            }
            if (usuarioCreate.Email.Length < 5 || !usuarioCreate.Email.Contains("@"))
            {
                return Results.BadRequest("O email deve ser válido.");
            }

            var usuario = new Usuario
            {
                Id = usuarios.Count + 1,
                Nome = usuarioCreate.Nome,
                Email = usuarioCreate.Email,
                Senha = usuarioCreate.Senha
            };

            //adiciona o usuario na lista
            usuarios.Add(usuario);
            //retorna o usuario criado
            return Results.Created($"/api/usuario/{usuario.Id}", usuario);
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] usuarioUpdateRequest usuarioUpdate)
        {
            //busca o usuario na lista pelo id
            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            //se nao encontrar retorna not found
            if (usuario is null)
                return Results.NotFound($"Usuario do id {id} não encontrado");
            //atualiza o usuario
            usuario.Nome = usuarioUpdate.Nome;
            usuario.Email = usuarioUpdate.Email;
            usuario.Senha = usuarioUpdate.Senha;
            //retorna o content
            return Results.NoContent();
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
