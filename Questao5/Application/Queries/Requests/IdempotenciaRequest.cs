namespace Questao5.Application.Queries.Requests
{
    public class IdempotenciaRequest
    {
        public string ChaveIdempotencia { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }
    }
}
