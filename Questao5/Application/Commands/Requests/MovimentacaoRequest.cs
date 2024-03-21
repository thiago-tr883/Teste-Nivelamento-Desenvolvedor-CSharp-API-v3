using System.Drawing;
using System;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoRequest
    {
        public string IdRequisicao { get; set; }
        public string IdContaCorrente { get; set; }
        public string TipoMovimento { get; set; }
        public double Valor { get; set; }
    }
}
