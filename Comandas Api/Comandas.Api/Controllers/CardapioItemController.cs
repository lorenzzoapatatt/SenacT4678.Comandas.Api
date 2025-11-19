using Comandas.Api.DTOs;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace Comandas.Api.Controllers
{
    // CRIA A ROTA DO CONTROLADOR
    [Route("api/[controller]")]
    [ApiController] // DEFINE QUE ESSA CLASSE É UM CONTROLADOR DE API
    public class CardapioItemController : ControllerBase // HERDA DE ControllerBase para PODER RESPONDER A REQUISICOES HTTP
    {
        public ComandasDbContext _context { get; set; }
        public CardapioItemController(ComandasDbContext context)
        {
            _context = context;
        }

        [HttpGet] // Anotação que indica se o metodo responde a requisicoes GET
        public IResult GetCardapios()
        {
            var cardapios = _context.CardapioItens.ToList();
            // Crua uma lista estatica de cardapio e transforma em JSON
            return Results.Ok(cardapios);
        }

        // GET api/<CardapioItemController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            // Buscar na lista de cardapios o cardapio com o id informado
            var cardapio = _context.CardapioItens.FirstOrDefault(c => c.Id == id);
            if (cardapio is null)
            {
                return Results.NotFound("Cardapio não encontrado");
            }
            // retorna o valor para o endpoint da api
            return Results.Ok(cardapio);
        }

        // POST api/<CardapioItemController>
        [HttpPost]
        public IResult  Post([FromBody] CardapioItemCreateRequest cardapio)
        {
            if (cardapio.Descricao.Length < 5)
                return Results.BadRequest("A descrição deve ter no mínimo 5 caracteres.");
            if (cardapio.Preco <= 0)
                return Results.BadRequest("O preço deve ser maior que zero.");
            if (cardapio.Titulo.Length < 3)
                return Results.BadRequest("O título deve ter no mínimo 3 caracteres.");
            var cardapioItem = new CardapioItem
            {
                Titulo = cardapio.Titulo,
                Descricao = cardapio.Descricao,
                Preco = cardapio.Preco,
                PossuiPreparo = cardapio.PossuiPreparo,
                CategoriaCardapioId = cardapio.CategoriaCardapioId
            };
            //adiciona o cardapio na lista
            _context.CardapioItens.Add(cardapioItem);
            _context.SaveChanges();
            return Results.Created($"/api/cardapio/{cardapioItem.Id}", cardapioItem);
        }



        // PUT api/<CardapioItemController>/5
        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] CardapioItemUpdateRequest cardapio)
        {
            var cardapioItem = _context.CardapioItens.FirstOrDefault(c => c.Id == id);

            if (cardapioItem is null)
                return Results.NotFound("Cardapio não encontrado");
            cardapioItem.Titulo = cardapio.Titulo;
            cardapioItem.Descricao = cardapio.Descricao;
            cardapioItem.Preco = cardapio.Preco;
            cardapioItem.PossuiPreparo = cardapio.PossuiPreparo;
            return Results.Ok(cardapio);
        }

        // DELETE api/<CardapioItemController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            var cardapio = _context.CardapioItens.FirstOrDefault(u => u.Id == id);
            if (cardapio is null)
                return Results.NotFound($"Cardapio do id {id} não encontrado");

            _context.CardapioItens.Remove(cardapio);
            var removido = _context.SaveChanges();
            if (removido > 0)
            {
                return Results.NoContent();
            }
            return Results.StatusCode(500);
        }
    }
}
