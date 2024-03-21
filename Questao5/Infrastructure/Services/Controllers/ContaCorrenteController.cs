using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Repository;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaCorrenteReporitory _contaCorrenteReporitory;

        public ContaCorrenteController(IContaCorrenteReporitory contaCorrenteReporitory)
        {
            _contaCorrenteReporitory = contaCorrenteReporitory;
        }

        /// <summary>
        /// Movimentacao
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("Movimentacao")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult Movimentacao(MovimentacaoRequest request)
        {
            if (!_contaCorrenteReporitory.ValidarContaCorrenteCadastrada(request.IdContaCorrente))
                return new BadRequestObjectResult("INVALID_ACCOUNT");

            else if (!_contaCorrenteReporitory.ValidarContaCorrenteAtiva(request.IdContaCorrente))
                return new BadRequestObjectResult("INACTIVE_ACCOUNT");

            else if (request.Valor < 0)
                return new BadRequestObjectResult("INVALID_VALUE");

            else if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                return new BadRequestObjectResult("INVALID_TYPE");

            else
            {
                var idempotencia = _contaCorrenteReporitory.ValidarIdempotencia(request.IdRequisicao);

                if (string.IsNullOrEmpty(idempotencia))
                {
                    var resultado = _contaCorrenteReporitory.Movimentacao(request);

                    _contaCorrenteReporitory.Idempotencia(request.IdRequisicao, JsonConvert.SerializeObject(request), resultado.ToString());

                    return Ok(resultado);
                }
                else
                    return Ok(idempotencia);
            }
        }

        /// <summary>
        /// Saldo
        /// </summary>
        /// <param name="idContaCorrente"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [Route("Saldo")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult Saldo(string idContaCorrente)
        {
            if (!_contaCorrenteReporitory.ValidarContaCorrenteCadastrada(idContaCorrente))
                return new BadRequestObjectResult("INVALID_ACCOUNT");

            else if (!_contaCorrenteReporitory.ValidarContaCorrenteAtiva(idContaCorrente))
                return new BadRequestObjectResult("INACTIVE_ACCOUNT");

            else
                return Ok(_contaCorrenteReporitory.Saldo(idContaCorrente));
        }
    }
}
