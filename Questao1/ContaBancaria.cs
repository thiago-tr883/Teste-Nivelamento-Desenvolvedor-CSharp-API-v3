using System.Globalization;

namespace Questao1
{
    class ContaBancaria {
        private readonly int numero = 0;
        private double saldo = 0;
        private readonly double depositoInicial = 0;
        private readonly double taxa = 3.5;

        public int Numero { get { return numero; } }
        public string Titular { get; set; }
        public double DepositoInicial { get { return depositoInicial; } }
        public double Saldo { get { return saldo; } }

        public ContaBancaria(int numero, string titular, double depositoInicial = 0)
        {
            this.numero = numero;
            this.Titular = titular;
            this.depositoInicial = depositoInicial;
            this.saldo = depositoInicial;
        }
       
        public void Deposito(double quantia)
        {
            this.saldo += quantia;
        }

        public void Saque(double quantia)
        {
            this.saldo -= quantia + taxa;
        }
    }
}
