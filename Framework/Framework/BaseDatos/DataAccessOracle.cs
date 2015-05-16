//Valorar si todavia se conservaran estas conexiones 
//using System;
//using System.Collections;
//using System.Data;
//using System.Data.SqlClient;
//using System.Diagnostics;
//using System.Linq;
//namespace Solucionic.Framework.BaseDatos
//{
//    internal class DataAccessOracle : IBasesDatos
//    {

//        private bool Cb_MantenenerConexionAbierta;
//        public string RegresaDefinicionTabla(string psTabla)
//        {
//            return "";
//        }
//        public string[] RegresaLlavesTabla(string psTabla)
//        {
//            return null;
//        }
//        public bool MantenenerConexionAbierta
//        {
//            get { return Cb_MantenenerConexionAbierta; }
//            set { Cb_MantenenerConexionAbierta = value; }
//        }

//        public void ActualizaParams(string psspNombre, string psClasificacion, string psEmpresa, string psEmpleado, byte[] Pb_Foto)
//        {
//        }


//        public void CambiaBasedeDatos(string psNombreBaseDatos)
//        {
//        }

//        public System.Data.IDbConnection Conexion
//        {             
//             get { return null; }
//        }
//        public IDbTransaction Transaccion
//        {
//            get { return null; }
//        }


//        public void CreaConsulta(string pscstName, string pscstSQL, bool Pb_Valida = true)
//        {
//        }

//        public  int EjecutaComando(string psQuery)
//        {
//            return 0;
//        }

//        public int EjecutaComando(string psQuery, ref IDbTransaction Po_Transaccion)
//        {
//            return 0;
//        }
//        public object EjecutaEscalar(string psQuery)
//        {
//            return null;
//        }

//        public bool EliminaConsulta(string pscstName)
//        {
//            return false;
//        }

//        public bool ExisteObjetoEnBD(string psObjeto)
//        {
//            return false;
//        }

//        public bool ExisteCampoEnTabla(string psTabla, string psCampo, bool Pb_Azure = false)
//        {
//            return false;
//        }

//        public bool ExisteRegistro(string psTabla, string psCampo, string psValor, ref string psFiltroEspecial )
//        {
//            return false;
//        }


//        public void LlenaDataSet(ref System.Data.DataSet Pds_DataSet, string psQuery, string psTabla)
//        {
//        }

//        public DataTable RegresaDataTable( string psQuery, string psTabla )
//        {
//             return null;
//        }

//        public System.Data.IDataReader RegresaDataReader(string psQuery)
//        {
//            return null;
//        }

//        public System.Data.DataSet RegresaDataSet(string psQuery, string psTabla)
//        {
//            return null;
//        }

//        public System.Data.DataSet RegresaDataSetyDataAdapter( string psComando, string psTabla, ref SqlDataAdapter Po_DataAdapter )
//        {
//            return null;
//        }

//        public System.Data.DataView RegresaDataView(string psQuery, string psTabla)
//        {
//            return null;
//        }

//        public System.Data.ConnectionState RegresaEstado
//        {

//            get 
//            {
//                return System.Data.ConnectionState.Closed;
//            }
//        }


//        public void LlenaDataSet(ref System.Data.DataTable Pds_DataTable, string psQuery)
//        {
//        }

//        public int UpdateDataSetModificable( ref System.Data.DataSet Pds_Dataset, ref SqlDataAdapter Pda_DataAdapter )
//        {
//            return 0;
//        }
//        public System.Data.DataSet RegresaDataSet(string psQuery, string psTabla, ArrayList Po_Parametros)
//        {
//            return null;
//        }

//        public void IniciaTransaccion()
//        {
//        }

//        public void ConfirmarTransaccion()
//        {
//        }

//        public void DeshacerTransaccion()
//        {
//        }


//        public void EjecutaSp(string psNombre, System.Collections.ArrayList Po_Parametros)
//        {
//        }
//        public object EjecutaEscalarSp(string psNombre, System.Collections.ArrayList Po_Parametros)
//        {
//            return null;
//        }
//        public void GrabaLog(string psDescripcion)
//        {
//            string Ls_Query = null;
//            Ls_Query = "Insert into xtsLog (Descripcion) values ('" + psDescripcion.Replace("'", "''") + "')";
//            EjecutaComando(Ls_Query);
//        }
//    }
//}
