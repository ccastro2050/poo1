using System;

namespace Facturas.Modelo
{
    public class Cliente : Persona
    {
        public Cliente(string credito = null, int fkCodPersona = 0, int fkCodEmpresa = 0)
        {
            Credito = credito;
            FkCodPersona = fkCodPersona;
            FkCodEmpresa = fkCodEmpresa;
        }

        public string Credito { get; set; }
        public int FkCodPersona { get; set; }
        public int FkCodEmpresa { get; set; }
    }
}
