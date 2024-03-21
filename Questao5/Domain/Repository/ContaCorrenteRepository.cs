using Dapper;
using Microsoft.Data.Sqlite;
using Moq;
using NSubstitute.Core;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Infrastructure.Sqlite;
using static Dapper.SqlMapper;

namespace Questao5.Domain.Repository
{
    public class ContaCorrenteRepository : IContaCorrenteReporitory
    {
        private readonly DatabaseConfig _databaseConfig;

        public ContaCorrenteRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;  
        }

        public string Movimentacao(MovimentacaoRequest request)
        {
            Guid guid = Guid.NewGuid();

            var queryRequest = new MovimentacaoQueryRequest
            {
                IdMovimento = guid.ToString(),
                IdContaCorrente = request.IdContaCorrente,
                DataMovimento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                TipoMovimento = request.TipoMovimento,
                Valor = request.Valor
            };

            var query = "INSERT INTO movimento(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES(@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);";

            using var connection = new SqliteConnection(_databaseConfig.Name);
            
            connection.Execute(query, queryRequest);

            return guid.ToString();
        }

        public void Idempotencia(string idRequisicao, string requisicao, string resultado)
        {
            var queryRequest = new IdempotenciaRequest
            {
                ChaveIdempotencia = idRequisicao,
                Requisicao = requisicao,
                Resultado = resultado
            };

            var query = "INSERT INTO idempotencia(chave_idempotencia, requisicao, resultado) VALUES(@ChaveIdempotencia, @Requisicao, @Resultado);";

            using var connection = new SqliteConnection(_databaseConfig.Name);

            connection.Execute(query, queryRequest);
        }

        public string ValidarIdempotencia(string idRequisicao)
        {
            var queryRequest = new ValidarIdempotenciaQueryRequest
            {
                ChaveIdempotencia = idRequisicao
            };

            using var connection = new SqliteConnection(_databaseConfig.Name);

            var table = connection.Query<string>("SELECT resultado FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia;", queryRequest);

            return table != null && table.Count() > 0 && table.ToList()[0] != null ? table.ToList()[0] : string.Empty;
        }

        public double Saldo(string idContaCorrente)
        {
            var queryRequest = new SaldoQueryRequest
            {
                IdContaCorrente = idContaCorrente
            };

            using var connection = new SqliteConnection(_databaseConfig.Name);

            var table = connection.Query<double?>("SELECT SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE valor * -1 END) FROM movimento WHERE idcontacorrente = @IdContaCorrente;", queryRequest);

            return table != null && table.Count() > 0 && table.ToList()[0] != null ? table.ToList()[0].Value : 0;
        }

        public bool ValidarContaCorrenteCadastrada(string idContaCorrente)
        {
            var queryRequest = new SaldoQueryRequest
            {
                IdContaCorrente = idContaCorrente
            };

            using var connection = new SqliteConnection(_databaseConfig.Name);

            var table = connection.Query<string>("SELECT idcontacorrente FROM contacorrente WHERE idcontacorrente = @IdContaCorrente;", queryRequest).ToList();

            return (table is not null && table.Count > 0);
        }

        public bool ValidarContaCorrenteAtiva(string idContaCorrente)
        {
            var queryRequest = new SaldoQueryRequest
            {
                IdContaCorrente = idContaCorrente
            };

            using var connection = new SqliteConnection(_databaseConfig.Name);

            var table = connection.Query<string>("SELECT idcontacorrente FROM contacorrente WHERE idcontacorrente = @IdContaCorrente AND ativo = 1;", queryRequest).ToList();

            return (table is not null && table.Count > 0);
        }
    }
}
