
using Comandas.Api.DTOs;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesaController : ControllerBase
    {
        public ComandasDbContext _context { get; set; }
        public MesaController(ComandasDbContext context)
        {
            _context = context;
        }

        // GET: api/<MesaController>
        [HttpGet]
        public IResult Get()
        {
            var mesas = _context.CardapioItens.ToList();
            return Results.Ok(mesas);
        }

        // GET api/<MesaController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var mesa = _context.Mesas.FirstOrDefault(m => m.Id == id);
            if (mesa == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(mesa);
        }

        // POST api/<MesaController>
        [HttpPost]
        public IResult Post([FromBody] MesaCreateRequest mesaCreate)
        {
            //valida se o numero da mesa é maior que zero
            var novaMesa = new Mesa
            {
                NumeroMesa = mesaCreate.NumeroMesa,
                SituacaoMesa = (int)SituacaoMesa.Livre
            };
            //adiciona a nova mesa na lista
            _context.Mesas.Add(novaMesa);
            _context.SaveChanges();
            //retorna a nova mesa criada e o codigo 201 CREATED
            return Results.Created($"/api/mesa/{novaMesa.Id}", novaMesa);
        }

        // PUT api/<MesaController>/5
        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] MesaUpdateRequest mesaUpdate)
        {
            if (mesaUpdate.NumeroMesa <= 0)
                return Results.BadRequest("O número da mesa deve ser maior que zero.");
            if (mesaUpdate.SituacaoMesa < 1 || mesaUpdate.SituacaoMesa > 3)
                return Results.BadRequest("A situação da mesa deve ser 1 (Livre), 2 (Ocupada) ou 3 (Reservada).");

            var mesa = _context.Mesas.FirstOrDefault(m => m.Id == id);   
            if (mesa is null)
                return Results.NotFound($"Mesa {id} não encontrada");
            mesa.NumeroMesa = mesaUpdate.NumeroMesa;
            mesa.SituacaoMesa = mesaUpdate.SituacaoMesa;
            return Results.NoContent();
        }

        // DELETE api/<MesaController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            var mesas = _context.Mesas.FirstOrDefault(u => u.Id == id);
            if (mesas is null)
                return Results.NotFound($"Mesas do id {id} não encontrado");

            _context.Mesas.Remove(mesas);
            var removido = _context.SaveChanges();
            if (removido > 0)
            {
                return Results.NoContent();
            }
            return Results.StatusCode(500);
        }
    }
}
