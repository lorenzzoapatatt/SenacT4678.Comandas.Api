using Comandas.Api.DTOs;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComandaController : ControllerBase
    {
        public ComandasDbContext _context { get; set; }
        public ComandaController(ComandasDbContext context)
        {
            _context = context;
        }

        // GET: api/<ComandaController>
        [HttpGet]
        public IResult Get(int id)
        {
            var comanda = _context.Comandas.FirstOrDefault(c => c.Id == id);
            if (comanda is null)
            {
                return Results.NotFound("Comanda não encontrada");
            }
            return Results.Ok(comanda);
        }

        // GET api/<ComandaController>/5
        [HttpGet("{id}")]

        // POST api/<ComandaController>
        [HttpPost]
        public IResult Post([FromBody] ComandaCreateRequest comandaCreate)
        {
            if (comandaCreate.NomeCliente.Length < 3)
            {
                return Results.BadRequest("O nome do cliente deve ter no mínimo 3 caracteres.");
            }
            if (comandaCreate.NumeroMesa <= 0)
            {
                return Results.BadRequest("O número da mesa deve ser maior que zero.");
            }
            var novaComanda = new Comanda
            {
                NomeCliente = comandaCreate.NomeCliente,
                NumeroMesa = comandaCreate.NumeroMesa
            };
            // cria uma variavel do tipo lista de itens
            var comandaItens = new List<ComandaItem>();
            // percorre os ids dos itens do cardapio
            foreach (int cardapioItemId in comandaCreate.CardapioItens)
            {
                // cria um novo item de comanda
                var comandaItem = new ComandaItem
                {
                    ComandaId = novaComanda.Id,
                    CardapioItemId = cardapioItemId
                };
                // adiciona o item na lista de itens
                comandaItens.Add(comandaItem);

                var cardapioItem = _context.CardapioItens.FirstOrDefault(ci => ci.Id == cardapioItemId);

                //if (cardapioItem!.PossuiPreparo)
                    var pedido = new PedidoCozinha
                    {   
                        Comanda = novaComanda
                    };
                    var pedidoItem = new PedidoCozinhaItem
                    {
                        ComandaItem = comandaItem,
                        PedidoCozinha = pedido
                    };
                    _context.pedidoCozinhas.Add(pedido);
                    _context.PedidoCozinhaItems.Add(pedidoItem);
            }
            novaComanda.Itens = comandaItens;
            _context.Comandas.Add(novaComanda);
            _context.SaveChanges();

            var resposta = new ComandaCreateResponse
            {
                Id = novaComanda.Id,
                NomeCliente = novaComanda.NomeCliente,
                NumeroMesa = novaComanda.NumeroMesa,
                Itens = novaComanda.Itens.Select(i => new ComandaItemResponse
                {
                    Id = i.Id,
                    Titulo = _context.CardapioItens.First(ci => ci.Id == i.CardapioItemId).Titulo
                }).ToList()
            };
            return Results.Created($"/api/comanda/{resposta.Id}", resposta);
        }

        // PUT api/<ComandaController>/5
        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] comandaUpdateRequest comandaUpdate)
        {
            var comanda = _context.Comandas.FirstOrDefault(c => c.Id == id);
            if (comanda is null)
                return Results.NotFound("Comanda nao encontrada");
            // atualiza os dados da comanda
            comanda.NomeCliente = comandaUpdate.NomeCliente;
            comanda.NumeroMesa = comandaUpdate.NumeroMesa;

            //foreach serve para quando eu nao tiver um valor especifico diferentemente de for que ja tem o numero definido
            foreach(var item in comandaUpdate.Itens)
            {
                // se id for informado e remover for verdadeiro
                if(item.Id > 0 && item.Remove == true)
                {
                    //removendo
                    RemoverItemComanda(item.Id);
                }
                // se cardapioitemid foir informado
                if(item.CardapioItemId > 0)
                {
                    //inserindo
                    InserirItemComanda(comanda, item.CardapioItemId);
                }
            }

            _context.SaveChanges();
            // retorna 204 sem conteudo
            return Results.NoContent();
        }

        private void InserirItemComanda(Comanda comanda, int cardapioItemId)
        {



        }

        private void RemoverItemComanda(int id)
        {
            // consulta o item da comanda pelo id
            var comandaItem = _context.ComandaItens.FirstOrDefault(ci => ci.Id == id);
            if (comandaItem is not null)
            {
                // remove o item da comanda
                _context.ComandaItens.Remove(comandaItem);
            }
        }

        // DELETE api/<ComandaController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            var comanda = _context.Comandas.FirstOrDefault(u => u.Id == id);
            if (comanda is null)
                return Results.NotFound($"Comanda do id {id} não encontrado");

            _context.Comandas.Remove(comanda);
            var removido = _context.SaveChanges();
            if (removido > 0)
            {
                return Results.NoContent();
            }
            return Results.StatusCode(500);
        }
    }
}
