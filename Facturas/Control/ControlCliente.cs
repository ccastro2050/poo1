using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Facturas.Modelo;

namespace Facturas.Control
{
    public class ControlCliente
    {
        private Cliente _objCliente;
        private ControlConexion _controlConexion;

        public ControlCliente(Cliente objCliente, ControlConexion controlConexion)
        {
            _objCliente = objCliente;
            _controlConexion = controlConexion;
        }

        public List<Cliente> Listar()
        {
            List<Cliente> arregloClientes = new List<Cliente>();
            string comandoSql = "SELECT * FROM cliente INNER JOIN persona ON cliente.fkcodpersona=persona.codigo";

            SqlDataReader reader = _controlConexion.EjecutarComandoSql(comandoSql);

            try
            {
                while (reader.Read())
                {
                    Cliente objCliente = new Cliente
                    {
                        Credito = reader["credito"].ToString(),
                        FkCodPersona = Convert.ToInt32(reader["fkcodpersona"]),
                        FkCodEmpresa = Convert.ToInt32(reader["fkcodempresa"]),
                        Codigo = Convert.ToInt32(reader["codigo"]),
                        Nombre = reader["nombre"].ToString(),
                        Email = reader["email"].ToString(),
                        Telefono = reader["telefono"].ToString()
                    };
                    arregloClientes.Add(objCliente);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Algo salió mal: {ex.Message}");
            }

            return arregloClientes;
        }

        public Cliente Consultar()
        {
            string comandoSql = "SELECT * FROM cliente INNER JOIN persona ON cliente.fkcodpersona=persona.codigo WHERE fkcodpersona = @fkCodPersona";
            SqlParameter[] parametros = {
                new SqlParameter("@fkCodPersona", _objCliente.FkCodPersona)
            };

            SqlDataReader reader = _controlConexion.EjecutarComandoSql(comandoSql, parametros);

            try
            {
                if (reader.Read())
                {
                    _objCliente.Credito = reader["credito"].ToString();
                    _objCliente.FkCodPersona = Convert.ToInt32(reader["fkcodpersona"]);
                    _objCliente.FkCodEmpresa = Convert.ToInt32(reader["fkcodempresa"]);
                    _objCliente.Codigo = Convert.ToInt32(reader["codigo"]);
                    _objCliente.Nombre = reader["nombre"].ToString();
                    _objCliente.Email = reader["email"].ToString();
                    _objCliente.Telefono = reader["telefono"].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Algo salió mal: {ex.Message}");
            }

            return _objCliente;
        }

        public string Crear()
        {
            string comandoSql = "INSERT INTO cliente (credito, fkcodpersona, fkcodempresa) VALUES (@credito, @fkCodPersona, @fkCodEmpresa)";
            SqlParameter[] parametros = {
                new SqlParameter("@credito", _objCliente.Credito),
                new SqlParameter("@fkCodPersona", _objCliente.FkCodPersona),
                new SqlParameter("@fkCodEmpresa", _objCliente.FkCodEmpresa)
            };

            try
            {
                _controlConexion.EjecutarComandoSql(comandoSql, parametros);
                return "Cliente creado exitosamente";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Algo salió mal: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public string Modificar()
        {
            string comandoSqlPersona = "UPDATE persona SET nombre = @nombre, email = @Email, telefono = @Telefono WHERE codigo = @fkCodPersona";
            SqlParameter[] parametrosPersona = {
                new SqlParameter("@nombre", _objCliente.Nombre),
                new SqlParameter("@Email", _objCliente.Email),
                new SqlParameter("@Telefono", _objCliente.Telefono),
                new SqlParameter("@fkCodPersona", _objCliente.FkCodPersona)
            };

            string comandoSqlCliente = "UPDATE cliente SET credito = @credito, fkcodempresa = @fkCodEmpresa WHERE fkcodpersona = @fkCodPersona";
            SqlParameter[] parametrosCliente = {
                new SqlParameter("@credito", _objCliente.Credito),
                new SqlParameter("@fkCodEmpresa", _objCliente.FkCodEmpresa),
                new SqlParameter("@fkCodPersona", _objCliente.FkCodPersona)
            };

            try
            {
                _controlConexion.EjecutarComandoSql(comandoSqlPersona, parametrosPersona);
                _controlConexion.EjecutarComandoSql(comandoSqlCliente, parametrosCliente);
                return "Cliente modificado exitosamente";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Algo salió mal: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public string Eliminar()
        {
            string comandoSqlCliente = "DELETE FROM cliente WHERE fkcodpersona = @fkCodPersona";
            SqlParameter[] parametrosCliente = {
                new SqlParameter("@fkCodPersona", _objCliente.FkCodPersona)
            };

            string comandoSqlPersona = "DELETE FROM persona WHERE codigo = @fkCodPersona";
            SqlParameter[] parametrosPersona = {
                new SqlParameter("@fkCodPersona", _objCliente.FkCodPersona)
            };

            try
            {
                _controlConexion.EjecutarComandoSql(comandoSqlCliente, parametrosCliente);
                _controlConexion.EjecutarComandoSql(comandoSqlPersona, parametrosPersona);
                return "Cliente eliminado exitosamente";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Algo salió mal: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }
    }
}
