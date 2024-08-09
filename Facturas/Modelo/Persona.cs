using System;

namespace Facturas.Modelo
{
    public class Persona
    {
        public Persona(string codigo = null, string email = null, string nombre = null, string telefono = null)
        {
            Codigo = codigo;
            Email = email;
            Nombre = nombre;
            Telefono = telefono;
        }

        public string Codigo { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
    }
}
