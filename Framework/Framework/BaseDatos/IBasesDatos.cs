using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// 
     /// </summary>
     internal interface IBasesDatos
     {
          ConnectionState RegresaEstado { get; }
          IDbConnection Conexion { get; }
          IDbTransaction Transaccion { get; }
          bool MantenenerConexionAbierta { get; set; }
          void CambiaBasedeDatos( string psNombreBaseDatos );
          void LlenaDataSet( ref DataSet pdsDataSet, string psQuery, string psTabla );
          void LlenaDataSet( ref DataTable pdtDataTable, string psQuery );
          void ActualizaParams( string psSpNombre, string psClasificacion, string psEmpresa, string psEmpleado, byte[] pbFoto );
          void CreaConsulta( string psCstName, string psCstSQL, bool pbValida = true );
          void EjecutaSp( string psNombre, ArrayList poParametros );
          object EjecutaEscalarSp( string psNombre, ArrayList poParametros );
          int EjecutaComando( string psQuery );
          int EjecutaComando( string psQuery, ref IDbTransaction poTransaccion );
          DataView RegresaDataView( string psQuery, string psTabla );
          bool ExisteObjetoEnBD( string psObjeto, string psBaseDatos );
          bool ExisteCampoEnTabla( string psTabla, string psCampo, bool pbAzure = false );
          bool ExisteRegistro( string psTabla, string psCampo, string psValor, ref string psFiltroEspecial );
          bool EliminaConsulta( string psCstName );
          object EjecutaEscalar( string psQuery );
          IDataReader RegresaDataReader( string psQuery );
          DataSet RegresaDataSet( string psQuery, string psTabla);
          TablaGenerica  RegresaTablaGenerica( string psQuery, string psTabla );
          DataTable RegresaDataTable( string psQuery, string psTabla);
          XmlReader RegresaXmlReader( string psQuery);
          DataSet RegresaDataSetyDataAdapter( string psComando, string psTabla, ref SqlDataAdapter pdaDataAdapter );
          int UpdateDataSetModificable( ref DataSet Pds_Dataset, ref SqlDataAdapter pdaDataAdapter );
          string RegresaDefinicionTabla( string psTabla );
          string[] RegresaLlavesTabla( string psTabla );
          System.Data.DataSet RegresaDataSet( string psQuery, string psTabla, ArrayList poParametros );
          void IniciaTransaccion();
          void ConfirmarTransaccion();
          void DeshacerTransaccion();
          void GrabaLog( string psDescripcion );
          string BDServidor {get; set;}
          string BDBitacora {get; set;}
     }
}
