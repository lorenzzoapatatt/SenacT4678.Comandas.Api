namespace Comandas.Api.DTOs
{
    public class comandaUpdateRequest
    {
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; } = default!;
        public int[] CardapioItens { get; set; } = default!;
    }
}
