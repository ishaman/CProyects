using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// 
     /// </summary>
     [Serializable]
     public class AccesoDatos
     {
          /// <summary>
          /// 
          /// </summary>
          private string _sCadenaConexion;
          /// <summary>
          /// 
          /// </summary>
          private Solucionic.Framework.Enumeraciones.enmMotor _enmMotor;
          /// <summary>
          /// 
          /// </summary>
          private bool _bExistextsBitacoraErrores;
          /// <summary>
          /// 
          /// </summary>
          private bool _bGrabandoError;
          /// <summary>
          /// 
          /// </summary>
          public string Servidor { get; private set; }
          /// <summary>
          /// 
          /// </summary>
          public string BDServidor { get; private set; }
          /// <summary>
          /// 
          /// </summary>
          public string BDBitacora { get; private set; }
          /// <summary>
          /// Tipo de motor de la base de datos
          /// </summary>
          public struct _stParametros
          {
               public string sParametro;
               public string sValor;
          }
          /// <summary>
          /// 
          /// </summary>
          public Solucionic.Framework.Enumeraciones.enmMotor nMotor { get { return _enmMotor; } }
          /// <summary>
          /// 
          /// </summary>
          public string CadenaConexion { get { return _sCadenaConexion; } }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psCadConexion"></param>
          /// <param name="psBDBitacora"></param>
          /// <param name="penMotor"></param>
          public AccesoDatos( string psCadConexion, string psBDBitacora, Solucionic.Framework.Enumeraciones.enmMotor penMotor )
          {
               _bGrabandoError = false;
               BDBitacora = psBDBitacora;
               _enmMotor = penMotor;
               _sCadenaConexion = psCadConexion;
               CargaVariablesServidor(psCadConexion);
               try
               {
                    if (ExisteObjetoEnBD("xtsBitacoraErrores", "", BDBitacora))
                         _bExistextsBitacoraErrores = true;
                    else
                         _bExistextsBitacoraErrores = false;
               }
               catch
               {
                    _bExistextsBitacoraErrores = false;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psCadConexion"></param>
          private void CargaVariablesServidor( string psCadConexion )
          {
               SqlConnectionStringBuilder loCadConexion = new SqlConnectionStringBuilder(psCadConexion);
               Servidor = loCadConexion.DataSource;
               BDServidor = loCadConexion.InitialCatalog;
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psQuery"></param>
          /// <returns></returns>
          public object EjecutaEscalar( StringBuilder psQuery )
          {
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         if (ManejaConexion.TransaccionActual == null)
                              return SqlHelper.ExecuteScalar(ManejaConexion.ConexionActual, CommandType.Text, psQuery.ToString());
                         else
                              return SqlHelper.ExecuteScalar(ManejaConexion.TransaccionActual, CommandType.Text, psQuery.ToString());
                    }
               }
               catch (Exception ex)
               {
                    if (ex.HResult != -2146232060) // && !( ex.ToString().IndexOf("PRIMAR") > 0 ))
                    {
                         //Se pone condion para evitar mandar un mensaje cuando se repita la llave duplicada
                         //esto es por la concurrencia en la bd al momento de insertar registros.
                         GrabaLog(ex.ToString());
                         throw;
                    }
                    return null;
               }
          }
          /// <summary>
          /// Ejecuta escalar tipado con Generics
          /// </summary>
          /// <typeparam name="T"></typeparam>
          /// <param name="psQuery"></param>
          /// <returns></returns>
          public T EjecutaEscalar<T>( StringBuilder psQuery )
          {               
               object loResultado;
               try
               {                    
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         if (ManejaConexion.TransaccionActual == null)
                              loResultado = SqlHelper.ExecuteScalar(ManejaConexion.ConexionActual, CommandType.Text, psQuery.ToString());
                         else
                              loResultado = SqlHelper.ExecuteScalar(ManejaConexion.TransaccionActual, CommandType.Text, psQuery.ToString());
                    }
                    if (Object.Equals(loResultado, DBNull.Value))
                         return default(T);
                    else
                    {
                        return ( (T)loResultado );
                    }
               }
               catch (Exception ex)
               {
                    if (ex.HResult != -2146232060 && !( ex.ToString().IndexOf("PRIMAR") > 0 ))
                    {
                         //Se pone condion para evitar mandar un mensaje cuando se repita la llave duplicada
                         //esto es por la concurrencia en la bd al momento de insertar registros.
                         GrabaLog(ex.ToString());
                         throw;
                    }
                    return default(T);
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psObjeto"></param>
          /// <param name="psTipo"></param>
          /// <param name="psBaseDatos"></param>
          /// <returns></returns>
          public bool ExisteObjetoEnBD( string psObjeto, string psTipo = "", string psBaseDatos = "" )
          {
               StringBuilder lsQuery = null;
               object loValor = null;
               bool lbTemporal = false;
               StringBuilder lsFiltro;
               lsQuery = new StringBuilder();
               lsFiltro = null;
               if (!String.IsNullOrEmpty(psBaseDatos))
                    //Esto sirve para poder conectarse a la BD de la bitacora, si es que esta, está en otra base
                    //si esta en la misma BD el valor del parametro psBaseDatos es vacio
                    lsQuery.AppendFormat("USE [{0}] ", psBaseDatos);
               if (!String.IsNullOrEmpty(psTipo))
               {
                    lsFiltro = new StringBuilder();
                    lsFiltro.AppendFormat(" and type ='{0}' ", psTipo);
               }
               lbTemporal = psObjeto.Substring(0, Math.Min(1, psObjeto.Length)) == "#";
               if (!lbTemporal)
                    lsQuery.AppendFormat("Select Name From SysObjects  Where Name='{0}'", psObjeto);
               else
                    //Si se encuentra en tempdb
                    //lsQuery = "Select Name From tempdb..SysObjects Where Name='" & psObjeto & "'"
                    //Para las tablas temporales se hace un query a la tabla para validar si truena el query o no, y dependiendo de eso
                    //se supone que la tabla existe o no
                    lsQuery.AppendFormat("Select Count(*) From {0}", psObjeto);
               if (!String.IsNullOrEmpty(psTipo))
                    lsQuery.Append(lsFiltro);
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         if (ManejaConexion.TransaccionActual == null)
                              loValor = SqlHelper.ExecuteScalar(ManejaConexion.ConexionActual, CommandType.Text, lsQuery.ToString());
                         else
                              loValor = SqlHelper.ExecuteScalar(ManejaConexion.TransaccionActual, CommandType.Text, lsQuery.ToString());
                    }
                    if (!lbTemporal)
                    {
                         if (psObjeto.ToUpper().Trim() == Convert.ToString(loValor).Trim().ToUpper())
                              return true;
                         else
                              return false;
                    }
                    else
                         return true;
               }
               catch (Exception ex)
               {
                    if (!lbTemporal)
                    {
                         GrabaLog(ex.ToString());
                         throw new Exception(ex.Message);
                    }
                    else
                         return false;
               }
               finally
               {
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psDescripcion"></param>
          public void GrabaLog( string psDescripcion )
          {
               StringBuilder lsQuery;
               if (_bGrabandoError)
                    //con esta bandera controlamos que no se llame de forma ciclica esta función
                    return;
               try
               {
                    _bGrabandoError = true;
                    //Que cuando no exista la tabla xtsLog la cree
                    //If Not ExisteObjetoEnBD("xtsLog") Then
                    if (!_bExistextsBitacoraErrores)
                    {
                         lsQuery = new StringBuilder();
                         if (!String.IsNullOrEmpty(BDBitacora))
                              lsQuery.AppendFormat("CREATE TABLE [{0}].[dbo].[xtsBitacoraErrores](", BDBitacora);
                         else
                              lsQuery.AppendFormat("CREATE TABLE [{0}].[dbo].[xtsBitacoraErrores](", BDServidor);
                         lsQuery.Append(" ID int IDENTITY(1,1) NOT NULL,");
                         lsQuery.Append(" Fecha datetime NULL CONSTRAINT [DF_xtsBitacoraErrores_Fecha]  DEFAULT (getdate()),");
                         lsQuery.Append(" Descripcion text NULL,");
                         lsQuery.Append(" Origen text NULL,");
                         lsQuery.Append(" CONSTRAINT [PK_xtsBitacoraErrores] PRIMARY KEY CLUSTERED ");
                         lsQuery.Append(" (ID Asc)");
                         lsQuery.Append(" )");
                         EjecutaComando(lsQuery);
                    }
                    psDescripcion = psDescripcion.Replace("'", "''");
                    lsQuery = new StringBuilder();
                    //TODO:Revisar como implementar el origen
                    if (!String.IsNullOrEmpty(BDBitacora))
                         lsQuery.AppendFormat("Insert into [{0}].[dbo].[xtsBitacoraErrores] (Descripcion,Origen) values ('{1}','{2}')", BDBitacora, psDescripcion, BDServidor);
                    else
                         lsQuery.AppendFormat("Insert into [{1}].[dbo].[xtsBitacoraErrores] (Descripcion,Origen) values ('{0}','{1}')", psDescripcion, BDServidor);
                    EjecutaComando(lsQuery);
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
               }
               finally
               {
                    //Siempre desactiva la bandera para evitar que se quede prendida
                    _bGrabandoError = false;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psQuery"></param>
          /// <returns></returns>
          public int EjecutaComando( StringBuilder psQuery )
          {
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         if (ManejaConexion.TransaccionActual == null)
                              return SqlHelper.ExecuteNonQuery(ManejaConexion.ConexionActual, CommandType.Text, psQuery.ToString());
                         else
                              return SqlHelper.ExecuteNonQuery(ManejaConexion.TransaccionActual, CommandType.Text, psQuery.ToString());
                    }
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
               finally
               {
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psQuery"></param>
          /// <param name="poParametros"></param>
          /// <param name="pbIlimitado"></param>
          public void EjecutaComando( StringBuilder psQuery, List<ParametrosSQL> poParametros, bool pbIlimitado = true )
          {
               using (var loConexion = new ManejaConexion(_sCadenaConexion))
               {

                    using (System.Data.SqlClient.SqlCommand loComando = new System.Data.SqlClient.SqlCommand(psQuery.ToString()))
                    {
                         //Si el tiempo de espera es mayor al esperado, Se colaca un cero para que el tiempo sea ilimitado, esto es para carga de archivos
                         //Esto se arregla poniendo un cero en el time out para que sea ilimitado
                         if (pbIlimitado)
                              loComando.CommandTimeout = 0;
                         loComando.Connection = ManejaConexion.ConexionActual;
                         if (!( loComando.Connection.State == System.Data.ConnectionState.Open ))
                              loComando.Connection.Open();
                         foreach (ParametrosSQL loParametro in poParametros)
                              loComando.Parameters.AddWithValue(loParametro.Parametro, loParametro.Valor);
                         loComando.Transaction = ManejaConexion.TransaccionActual;
                         loComando.ExecuteNonQuery();
                    }
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psQuery"></param>
          /// <param name="psTabla"></param>
          /// <returns></returns>
          public System.Data.DataSet RegresaDataSet( StringBuilder psQuery, string psTabla )
          {
               DataSet ldtsResultado = new DataSet();
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         if (ManejaConexion.TransaccionActual == null)
                              SqlHelper.FillDataset(ManejaConexion.ConexionActual, CommandType.Text, psQuery.ToString(), ldtsResultado, new string[] { psTabla });
                         else
                              SqlHelper.FillDataset(ManejaConexion.TransaccionActual, CommandType.Text, psQuery.ToString(), ldtsResultado, new string[] { psTabla });
                    }
                    return ldtsResultado;
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psQuery"></param>
          /// <param name="psTabla"></param>
          /// <returns></returns>
          public System.Data.DataTable RegresaDataTable( StringBuilder psQuery, string psTabla )
          {
               DataTable ldtResultado = new DataTable();
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         if (ManejaConexion.TransaccionActual == null)
                              SqlHelper.FillDataTable(ManejaConexion.ConexionActual, CommandType.Text, psQuery.ToString(), ldtResultado, new string[] { psTabla });
                         else
                              SqlHelper.FillDataTable(ManejaConexion.TransaccionActual, CommandType.Text, psQuery.ToString(), ldtResultado, new string[] { psTabla });
                    }
                    return ldtResultado;
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psQuery"></param>
          /// <param name="psTabla"></param>
          /// <returns></returns>
          public System.Data.DataView RegresaDataView( StringBuilder psQuery, string psTabla )
          {
               DataSet ldtsResultado = new DataSet();
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         if (ManejaConexion.TransaccionActual == null)
                              SqlHelper.FillDataset(ManejaConexion.ConexionActual, CommandType.Text, psQuery.ToString(), ldtsResultado, new string[] { psTabla });
                         else
                              SqlHelper.FillDataset(ManejaConexion.TransaccionActual, CommandType.Text, psQuery.ToString(), ldtsResultado, new string[] { psTabla });
                    }
                    return ldtsResultado.Tables[psTabla].DefaultView;
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psNombre"></param>
          /// <param name="poParametros"></param>
          public void EjecutaSp( string psNombre, System.Collections.ArrayList poParametros )
          {
               int liIndice = 0;
               SqlDataAdapter loAdaptador = null;
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         loAdaptador = new SqlDataAdapter(psNombre, ManejaConexion.ConexionActual);
                         if (ManejaConexion.TransaccionActual != null)
                              loAdaptador.SelectCommand.Transaction = ManejaConexion.TransaccionActual;
                         loAdaptador.SelectCommand.CommandTimeout = 0;
                         loAdaptador.SelectCommand.CommandType = CommandType.StoredProcedure;
                         for (liIndice = 0 ; liIndice <= poParametros.Count - 1 ; liIndice++)
                              loAdaptador.SelectCommand.Parameters.Add(
                                   new SqlParameter(Convert.ToString(( (_stParametros)poParametros[liIndice] ).sParametro), (object)( (_stParametros)poParametros[liIndice] ).sValor));
                         loAdaptador.SelectCommand.ExecuteNonQuery();
                    }
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psNombre"></param>
          /// <param name="poParametros"></param>
          /// <returns></returns>
          public object EjecutaEscalarSp( string psNombre, System.Collections.ArrayList poParametros )
          {
               int liIndice = 0;
               SqlDataAdapter loAdaptador = null;
               loAdaptador = null;
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         loAdaptador = new SqlDataAdapter(psNombre, ManejaConexion.ConexionActual);
                         loAdaptador.SelectCommand.CommandTimeout = 0;
                         loAdaptador.SelectCommand.CommandType = CommandType.StoredProcedure;
                         if (ManejaConexion.TransaccionActual != null)
                              loAdaptador.SelectCommand.Transaction = ManejaConexion.TransaccionActual;
                         for (liIndice = 0 ; liIndice <= poParametros.Count - 1 ; liIndice++)
                              loAdaptador.SelectCommand.Parameters.Add(
                                   new SqlParameter(Convert.ToString(( (_stParametros)poParametros[liIndice] ).sParametro), ( (_stParametros)poParametros[liIndice] ).sValor));
                         return loAdaptador.SelectCommand.ExecuteScalar();
                    }
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
          }

          public object EjecutaEscalarSp( string psNombre, Dictionary<string,string> poParametros )
          {               
               SqlDataAdapter loAdaptador;               
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         loAdaptador = new SqlDataAdapter(psNombre, ManejaConexion.ConexionActual);
                         loAdaptador.SelectCommand.CommandTimeout = 0;
                         loAdaptador.SelectCommand.CommandType = CommandType.StoredProcedure;
                         if (ManejaConexion.TransaccionActual != null)
                              loAdaptador.SelectCommand.Transaction = ManejaConexion.TransaccionActual;
                         foreach(KeyValuePair<string, string> loParametro in poParametros)
                              loAdaptador.SelectCommand.Parameters.Add(new SqlParameter(loParametro.Key, loParametro.Value));
                         return loAdaptador.SelectCommand.ExecuteScalar();
                    }
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psQuery"></param>
          /// <param name="psTabla"></param>
          /// <returns></returns>
          public TablaGenerica RegresaTablaGenerica( StringBuilder psQuery, string psTabla )
          {
               DataTable loDataTable = null;
               TablaGenerica loTabla = null;
               try
               {
                    loDataTable = RegresaDataTable(psQuery, psTabla);
                    loTabla = new TablaGenerica(psTabla);
                    loTabla.ConvertDataTable2List2(loDataTable, psQuery.ToString());
                    return loTabla;
               }
               finally
               {
                    if (!Object.Equals(loDataTable, null))
                    {
                         loDataTable.Clear();
                         loDataTable.Dispose();
                         loDataTable = null;
                    }
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="pscstName"></param>
          /// <param name="pscstSQL"></param>
          /// <param name="pbValida"></param>
          /// <param name="psTipo"></param>
          /// <param name="psBaseDatos"></param>
          public void CreaConsulta( string pscstName, string pscstSQL, bool pbValida = true, string psTipo = "", string psBaseDatos = "" )
          {
               string lsQuery = "";
               try
               {
                    if (pbValida)
                         if (ExisteObjetoEnBD(pscstName, psTipo, psBaseDatos))
                              if (!EliminaConsulta(pscstName, psTipo, psBaseDatos))
                                   return;
                    lsQuery = "Create View dbo." + pscstName + " as " + pscstSQL;
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         if (ManejaConexion.TransaccionActual == null)
                              SqlHelper.ExecuteNonQuery(ManejaConexion.ConexionActual, CommandType.Text, lsQuery);
                         else
                              SqlHelper.ExecuteNonQuery(ManejaConexion.TransaccionActual, CommandType.Text, lsQuery);
                    }
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="pscstName"></param>
          /// <param name="psTipo"></param>
          /// <param name="psBaseDatos"></param>
          /// <returns></returns>
          public bool EliminaConsulta( string pscstName, string psTipo = "", string psBaseDatos = "" )
          {
               StringBuilder lsQuery;
               try
               {
                    if (ExisteObjetoEnBD(pscstName, psTipo, psBaseDatos))
                    {
                         lsQuery = new StringBuilder();
                         lsQuery.AppendFormat("DROP VIEW {0}", pscstName);
                         using (var loConexion = new ManejaConexion(_sCadenaConexion))
                         {
                              if (ManejaConexion.TransaccionActual == null)
                                   SqlHelper.ExecuteNonQuery(ManejaConexion.ConexionActual, CommandType.Text, lsQuery.ToString());
                              else
                                   SqlHelper.ExecuteNonQuery(ManejaConexion.TransaccionActual, CommandType.Text, lsQuery.ToString());
                              return true;
                         }
                    }
                    else
                         return false;
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psTabla"></param>
          /// <param name="psCampo"></param>
          /// <param name="pbAzure"></param>
          /// <returns></returns>
          public bool ExisteCampoEnTabla( string psTabla, string psCampo, bool pbAzure = false )
          {
               StringBuilder lsQuery;
               lsQuery = new StringBuilder();
               if (pbAzure)
               {
                    //version windows azure
                    lsQuery.Append("SELECT    count (sys.columns.name) Columna ");
                    lsQuery.Append(" FROM sys.columns");
                    lsQuery.Append(" INNER JOIN sys.objects ");
                    lsQuery.Append(" ON  (sys.columns.object_id = sys.objects.object_id) ");
                    lsQuery.Append(" LEFT OUTER JOIN sys.index_columns ");
                    lsQuery.Append(" ON  (sys.columns.object_id = sys.index_columns.object_id  ");
                    lsQuery.Append(" AND sys.columns.column_id = sys.index_columns.column_id  ");
                    lsQuery.Append(" AND (sys.index_columns.index_id <= 1)) ");
                    lsQuery.Append(" INNER JOIN sys.types ");
                    lsQuery.Append(" ON  (sys.columns.system_type_id = sys.types.system_type_id  ");
                    lsQuery.Append(" AND sys.types.system_type_id = sys.types.user_type_id) ");
                    //Se hizo un upper para que la función no sea sensible a mayusculas y minusculas
                    lsQuery.AppendFormat(" WHERE      UPPER(sys.objects.name) = '{0}' ", psTabla.ToUpper());
                    lsQuery.AppendFormat(" AND UPPER(sys.columns.name) = '{0}'", psCampo.ToUpper());
               }
               else
               {
                    lsQuery.Append("SELECT     count (syscolumns.name) Columna");
                    lsQuery.Append(" FROM   syscolumns INNER JOIN");
                    lsQuery.Append(" sysobjects ON ");
                    lsQuery.Append(" (syscolumns.id = sysobjects.id) LEFT OUTER JOIN");
                    lsQuery.Append(" sysindexkeys ON ");
                    lsQuery.Append(" (syscolumns.id = sysindexkeys.id ");
                    lsQuery.Append(" AND syscolumns.colid = sysindexkeys.colid ");
                    lsQuery.Append(" AND (sysindexkeys.indid <= 1)) INNER JOIN");
                    lsQuery.Append(" systypes ON ");
                    lsQuery.Append(" (syscolumns.xtype = systypes.xtype ");
                    lsQuery.Append(" AND systypes.xtype = systypes.xusertype)");
                    lsQuery.Append(" WHERE     ");
                    //Se hizo un upper para que la función no sea sensible a mayusculas y minusculas
                    lsQuery.AppendFormat(" UPPER(sysobjects.name) = '{0}' ", psTabla.ToUpper());
                    lsQuery.AppendFormat(" AND UPPER(syscolumns.name) = '{0}'", psCampo.ToUpper());
               }
               try
               {
                    using (var loConexion = new ManejaConexion(_sCadenaConexion))
                    {
                         if (ManejaConexion.TransaccionActual == null)
                              return ( Int32.Parse(SqlHelper.ExecuteScalar(ManejaConexion.ConexionActual, CommandType.Text, lsQuery.ToString()).ToString()) > 0 );
                         else
                              return ( Int32.Parse(SqlHelper.ExecuteScalar(ManejaConexion.TransaccionActual, CommandType.Text, lsQuery.ToString()).ToString()) > 0 );
                    }
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString());
                    throw new Exception(ex.Message);
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psTabla"></param>
          /// <returns></returns>
          public string RegresaDefinicionTabla( string psTabla )
          {
               StringBuilder lsQuery;
               DataView ldvDatos = null;
               int liContador = 0;
               StringBuilder lsDefinicion;
               string lsCampo = null;
               string lsTipo = null;
               lsQuery = new StringBuilder();
               lsQuery.Append("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH ");
               lsQuery.Append(" from information_schema.columns");
               lsQuery.AppendFormat(" WHERE TABLE_NAME='{0}'", psTabla);
               ldvDatos = RegresaDataView(lsQuery, psTabla);
               if (ldvDatos.Table.Rows.Count == 0)
                    return "";
               lsDefinicion = new StringBuilder();
               lsCampo = ldvDatos.Table.Rows[0][0].ToString();
               lsTipo = ldvDatos.Table.Rows[0][1].ToString();
               if (lsTipo == "varchar")
                    lsTipo = "varchar(" + ldvDatos.Table.Rows[0][2].ToString() + ")";
               if (lsTipo == "char")
                    lsTipo = "char(" + ldvDatos.Table.Rows[0][2].ToString() + ")";
               lsDefinicion.AppendFormat(" [{0}] {1}", lsCampo, lsTipo);
               for (liContador = 1 ; liContador <= ldvDatos.Table.Rows.Count - 1 ; liContador++)
               {
                    lsCampo = ldvDatos.Table.Rows[liContador][0].ToString();
                    lsTipo = ldvDatos.Table.Rows[liContador][1].ToString();
                    if (lsTipo == "varchar")
                         lsTipo = "varchar(" + ldvDatos.Table.Rows[liContador][2].ToString() + ")";
                    if (lsTipo == "char")
                         lsTipo = "char(" + ldvDatos.Table.Rows[liContador][2].ToString() + ")";
                    lsDefinicion.Append(" , ");
                    lsDefinicion.AppendFormat(" [{0}] {1}", lsCampo, lsTipo);
               }
               return lsDefinicion.ToString();
          }
     }
}
