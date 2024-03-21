using Questao5.Application.Commands.Requests;

namespace Questao5.Domain.Repository
{
    public interface IContaCorrenteReporitory
    {
        string Movimentacao(MovimentacaoRequest request);
        void Idempotencia(string idRequisicao, string requisicao, string resultado);
        string ValidarIdempotencia(string idRequisicao);
        double Saldo(string idContaCorrente);
        bool ValidarContaCorrenteCadastrada(string idContaCorrente);
        bool ValidarContaCorrenteAtiva(string idContaCorrente);
    }
}