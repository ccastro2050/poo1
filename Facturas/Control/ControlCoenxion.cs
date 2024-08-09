using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Facturas.Control
{
    public class ControlConexion
    {
        private SqlConnection _conn;
        private SqlCommand _comando;
        private SqlDataReader _reader;
        public string Mensaje { get; private set; } = "ok";

        public string AbrirBd(string user, string password, string host, int port, string db)
        {
            string connectionString = $"Server={host},{port};Database={db};User Id={user};Password={password};";
            try
            {
                _conn = new SqlConnection(connectionString);
                _conn.Open();
            }
            catch (SqlException ex)
            {
                Mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return Mensaje;
        }

        public string CerrarBd()
        {
            try
            {
                _conn.Close();
            }
            catch (SqlException ex)
            {
                Mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return Mensaje;
        }

        public SqlDataReader EjecutarComandoSql(string comandoSql, SqlParameter[] parametros = null)
        {
            try
            {
                _comando = new SqlCommand(comandoSql, _conn);
                if (parametros != null)
                {
                    _comando.Parameters.AddRange(parametros);
                }
                _reader = _comando.ExecuteReader();
            }
            catch (SqlException ex)
            {
                Mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return _reader;
        }

        public List<string> ObtenerListaTablas()
        {
            string sql = "SELECT table_name FROM information_schema.tables WHERE table_schema='dbo' AND table_type='BASE TABLE';";
            var cursor = EjecutarComandoSql(sql);
            var listaTablas = new List<string>();

            try
            {
                while (cursor.Read())
                {
                    listaTablas.Add(cursor.GetString(0));
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Algo salió mal: {ex.Message}";
                Console.WriteLine(Mensaje);
            }
            return listaTablas;
        }

        public List<string> ObtenerListaGeoTablas()
        {
            var listaGeoTablas = new List<string>();

            try
            {
                var cursor = ObtenerListaTablas();
                foreach (var tabla in cursor)
                {
                    var listaCampos = ObtenerListaCampos(tabla);
                    if (listaCampos.Contains("geom"))
                    {
                        listaGeoTablas.Add(tabla);
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Algo salió mal: {ex.Message}";
                Console.WriteLine(Mensaje);
            }

            return listaGeoTablas;
        }

        public List<string> ObtenerListaCampos(string tabla)
        {
            string sql = $"SELECT column_name FROM information_schema.columns WHERE table_name = '{tabla}';";
            var cursor = EjecutarComandoSql(sql);
            var listaCampos = new List<string>();

            try
            {
                while (cursor.Read())
                {
                    listaCampos.Add(cursor.GetString(0));
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Algo salió mal: {ex.Message}";
                Console.WriteLine(Mensaje);
            }
            return listaCampos;
        }
    }
}
