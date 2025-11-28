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
        // variavel que representa o banco de dados
        public ComandasDbContext _context { get; set; }
        // construtor
        public UsuarioController( ComandasDbContext context)
        {
            _context = context;
        }

        // GET: api/<UsuarioController>
        [HttpGet]
        public IResult Get()
        {
            var usuarios = _context.Usuarios.ToList();
            return Results.Ok(usuarios);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
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
            var emailExistente = _context.Usuarios.FirstOrDefault(u => u.Email == usuarioCreate.Email);
            if (emailExistente is not null)
            {
                return Results.BadRequest("O email já está em uso.");
            }

            var usuario = new Usuario
            {
                Nome = usuarioCreate.Nome,
                Email = usuarioCreate.Email,
                Senha = usuarioCreate.Senha
            };

            //adiciona o usuario no Contexto do banco de dados
            _context.Usuarios.Add(usuario);
            // executa o insert into usuarios (Id, Nome, Email, Senha) VALUES(...)
            _context.SaveChanges();
            //retorna o usuario criado
            return Results.Created($"/api/usuario/{usuario.Id}", usuario);
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] usuarioUpdateRequest usuarioUpdate)
        {
            //busca o usuario na lista pelo id
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            //se nao encontrar retorna not found
            if (usuario is null)
                return Results.NotFound($"Usuario do id {id} não encontrado");
            //atualiza o usuario
            usuario.Nome = usuarioUpdate.Nome;
            usuario.Email = usuarioUpdate.Email;
            usuario.Senha = usuarioUpdate.Senha;
            // update Usuarios set Nome, Email, ...
            _context.SaveChanges();
            //retorna o content
            return Results.NoContent();
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario is null)
                return Results.NotFound($"Usuario do id {id} não encontrado");

            _context.Usuarios.Remove(usuario);
            var removido = _context.SaveChanges();
            if (removido > 0)
            {
                return Results.NoContent();
            }
            return Results.StatusCode(500);
        }

        // criar metodo de login
        [HttpPost("login")]
        public IResult Login([FromBody] LoginRequest loginRequest)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == loginRequest.Email && u.Senha == loginRequest.Senha);

            //401
            if (usuario is null)
                return Results.Unauthorized();
            //200
            return Results.Ok("Usuario atendido");
        }
    }
}
