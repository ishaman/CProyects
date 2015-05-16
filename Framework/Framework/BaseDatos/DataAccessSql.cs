using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// internal class DataAccessSql : IBasesDatos. En las modificaciones esta clase hereda de DataAcces
     /// </summary>
     [Serializable]          
     public class DataAccessSql : DataAccess
     {
          #region Variables
          private bool _bConexionAbiertaUnaVez = false;
          private bool _bExistextsLog;      
          private bool _bGrabandoLog;
          
          #endregion

          #region Propiedades
                            
          #endregion

          #region Constructores         

          public DataAccessSql( string psCadConexion, string psBDBitacora, Solucionic.Framework.Enumeraciones.enmMotor penMotor)
               : base(psCadConexion,psBDBitacora,penMotor)
          {
               _bGrabandoLog = false;
               try
               {
                    if (ExisteObjetoEnBD("xtsBitacoraErrores"))                    
                         _bExistextsLog = true;                    
                    else                    
                         _bExistextsLog = false;                    
               }
               catch
               {
                    _bExistextsLog = false;
               }                                                     
          }
          #endregion

          #region Funciones

          protected override void ActualizaParams_( string psspNombre, string psClasificacion, string psEmpresa, string psEmpleado, byte[] pbyFoto, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               SqlParameter[] loParms = new SqlParameter[5];
               try
               {                    
                    loParms[0] = new SqlParameter("@Clasificacion", psClasificacion);
                    loParms[1] = new SqlParameter("@Empresa", psEmpresa);
                    loParms[3] = new SqlParameter("@Empleado", psEmpleado);
                    loParms[4] = new SqlParameter("@Foto", pbyFoto);
                    if (poTransaccion == null)                    
                         SqlHelper.ExecuteNonQuery((SqlConnection)poConeccion, CommandType.StoredProcedure, psspNombre, loParms);                
                    else                
                         SqlHelper.ExecuteNonQuery((SqlTransaction)poTransaccion, CommandType.StoredProcedure, psspNombre, loParms);                
               }
               catch (Exception ex)
               {
                    loParms = null;
                //Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
                    GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {                    
               }
          }

          protected override void CreaConsulta_( string pscstName, string pscstSQL, bool pbValida = true, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               string lsQuery = "";               
               try
               {
                    if (pbValida)                    
                         if (ExisteObjetoEnBD(pscstName,"","",poConeccion,poTransaccion))
                              if (!EliminaConsulta_(pscstName, poConeccion, poTransaccion))                         
                                   return;                                                                                
                    lsQuery = "Create View dbo." + pscstName + " as " + pscstSQL;
                    if (poTransaccion == null)                    
                         SqlHelper.ExecuteNonQuery((SqlConnection)poConeccion, CommandType.Text, lsQuery);                
                    else                
                         SqlHelper.ExecuteNonQuery((SqlTransaction)poTransaccion, CommandType.Text, lsQuery);                
               }
               catch (Exception ex)
               {                
                    GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {                                     
               }
          }

          protected override void LlenaDataSet_( ref System.Data.DataSet Pds_DataSet, string psQuery, string psTabla, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               try
               {                                 
                    if (poTransaccion == null)                
                         SqlHelper.FillDataset((SqlConnection)poConeccion, CommandType.Text, psQuery, Pds_DataSet, new string[] { psTabla });               
                    else                
                         SqlHelper.FillDataset((SqlTransaction)poTransaccion, CommandType.Text, psQuery, Pds_DataSet, new string[] { psTabla });                
               }
               catch (Exception ex)
               {
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {                
               }
          }

          protected override System.Data.IDataReader RegresaDataReader_( string psQuery, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               try
               {
                    if (poTransaccion == null)                    
                         return SqlHelper.ExecuteReader(poConeccion.ConnectionString, CommandType.Text, psQuery);                    
                    else
                    {
                         if (poConeccion.State == ConnectionState.Closed)                         
                              poConeccion.Open();                         
                         return SqlHelper.ExecuteReader((SqlTransaction)poTransaccion, CommandType.Text, psQuery);
                    }
               }
               catch (Exception ex)
               {             
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
          }

          protected override object EjecutaEscalar_( string psQuery, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               try
               {                    
                    if (poTransaccion == null)                    
                         return SqlHelper.ExecuteScalar((SqlConnection)poConeccion, CommandType.Text, psQuery);                    
                    else                
                         return SqlHelper.ExecuteScalar((SqlTransaction)poTransaccion, CommandType.Text, psQuery);                
               }
               catch (Exception ex)
               {
                    GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
          }

          protected override bool EliminaConsulta_( string pscstName, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               StringBuilder lsQuery;
               try
               {
                    if (ExisteObjetoEnBD(pscstName,"", "" ,poConeccion,poTransaccion))
                    {
                         lsQuery = new StringBuilder();
                         lsQuery.AppendFormat("DROP VIEW {0}", pscstName);
                         if (poTransaccion == null)                         
                              SqlHelper.ExecuteNonQuery((SqlConnection)poConeccion, CommandType.Text, lsQuery.ToString());                    
                         else                         
                              SqlHelper.ExecuteNonQuery((SqlTransaction)poTransaccion, CommandType.Text, lsQuery.ToString ());                    
                         return true;
                    }
                    else                    
                         return false;                    
               }
               catch (Exception ex)
               {
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
          }

          protected override bool ExisteObjetoEnBD_( string psObjeto,string psTipo = "", string psBaseDatos = "", IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
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
               lbTemporal = psObjeto.Substring(0, Math.Min(1,psObjeto.Length)) == "#";
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
                    if (poTransaccion == null)                    
                         loValor = SqlHelper.ExecuteScalar((SqlConnection)poConeccion, CommandType.Text, lsQuery.ToString());                
                    else                    
                         loValor = SqlHelper.ExecuteScalar((SqlTransaction)poTransaccion, CommandType.Text, lsQuery.ToString());                    
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
                          GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                         throw new Exception(ex.Message);
                    }
                    else
                        return false;
               }
               finally
               {                   
               }
          }
          

          protected override bool ExisteCampoEnTabla_( string psTabla, string psCampo, bool pbAzure = false, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
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
                    lsQuery.AppendFormat(" WHERE      UPPER(sys.objects.name) = '{0}' ",psTabla.ToUpper());
                    lsQuery.AppendFormat(" AND UPPER(sys.columns.name) = '{0}'",psCampo.ToUpper());
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
                    if (poConeccion.State == ConnectionState.Closed)
                         poConeccion.Open();
                    if (poTransaccion == null)
                         return (Int32.Parse(SqlHelper.ExecuteScalar((SqlConnection)poConeccion, CommandType.Text, lsQuery.ToString()).ToString()) > 0);
                    else
                         return (Int32.Parse(SqlHelper.ExecuteScalar((SqlTransaction)poTransaccion, CommandType.Text, lsQuery.ToString()).ToString()) > 0);                                       
               }
               catch (Exception ex)
               {               
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }               
          }

          protected override bool ExisteRegistro_( string psTabla, string psCampo, string psValor, string psFiltroEspecial = "", IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               StringBuilder lsQuery;
               String lsResultado;
               if (string.IsNullOrEmpty(psTabla.Trim()) || string.IsNullOrEmpty(psCampo.Trim()) || string.IsNullOrEmpty(psValor.Trim()))            
                    return false;
               lsQuery = new StringBuilder();
               lsQuery.AppendFormat ("SELECT {0} FROM {1} WHERE {2}= {3} {4} ",psCampo,psTabla,psCampo,psValor,psFiltroEspecial);
               try
               {
                    if (poConeccion.State == ConnectionState.Closed)                
                         poConeccion.Open();
                    if (poTransaccion == null)                
                         lsResultado = Convert.ToString(SqlHelper.ExecuteScalar((SqlConnection)poConeccion, CommandType.Text, lsQuery.ToString()));                
                    else
                         lsResultado = Convert.ToString(SqlHelper.ExecuteScalar((SqlTransaction)poTransaccion, CommandType.Text, lsQuery.ToString()));                
                    if (!string.IsNullOrEmpty(lsResultado))                
                         return true;                
                    else                
                         return false;                
               }
               catch (Exception ex)
               {
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {                    
               }
          }

          protected override System.Data.DataSet RegresaDataSet_( string psQuery, string psTabla, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               DataSet ldtsResultado = new DataSet();
               try
               {
                    if (poConeccion.State == ConnectionState.Closed)                
                         poConeccion.Open();                
                    if (poTransaccion == null)               
                         SqlHelper.FillDataset((SqlConnection)poConeccion, CommandType.Text, psQuery, ldtsResultado, new string[] { psTabla });               
                    else                
                         SqlHelper.FillDataset((SqlTransaction)poTransaccion, CommandType.Text, psQuery, ldtsResultado, new string[] { psTabla });                
                    return ldtsResultado;
               }
               catch (Exception ex)
               {
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {                                
               }
          }

          protected override System.Data.DataTable RegresaDataTable_( string psQuery, string psTabla, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               DataTable ldtResultado = new DataTable();
               try
               {
                    if (poConeccion.State == ConnectionState.Closed)
                         poConeccion.Open();
                    if (poTransaccion == null)
                         SqlHelper.FillDataTable((SqlConnection)poConeccion, CommandType.Text, psQuery, ldtResultado, new string[] { psTabla });
                    else
                         SqlHelper.FillDataTable((SqlTransaction)poTransaccion, CommandType.Text, psQuery, ldtResultado, new string[] { psTabla });
                    return ldtResultado;
               }
               catch (Exception ex)
               {
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {                    
               }
          }

          protected override System.Data.DataSet RegresaDataSet_( string psQuery, string psTabla, System.Collections.ArrayList poParametros, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               int Li_Indice = 0;
               DataSet Ldts_Resultado = new DataSet();
               SqlDataAdapter loAdaptador = null;
               try
               {
                    if (poConeccion.State == ConnectionState.Closed)                    
                         poConeccion.Open();                    
                    loAdaptador = new SqlDataAdapter(psQuery, (SqlConnection)poConeccion);
                    loAdaptador.SelectCommand.CommandTimeout = 0;     
                    if (poTransaccion != null)
                         loAdaptador.SelectCommand.Transaction = (System.Data.SqlClient.SqlTransaction)poTransaccion;                     
                    for (Li_Indice = 0; Li_Indice <= poParametros.Count - 1; Li_Indice++)                    
                         loAdaptador.SelectCommand.Parameters.Add(
                              new SqlParameter(Convert.ToString(((_stParametros)poParametros[Li_Indice]).sParametro), (object)(((_stParametros)poParametros[Li_Indice]).sValor)));
                    loAdaptador.Fill(Ldts_Resultado, psTabla);
                    return Ldts_Resultado;
               }
               catch (Exception ex)
               {                
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {
                    //<@> Cambio la manera de liberar este objeto ya que el paso por referencia solo se permite para objetos asiganles
                    //y al hacer el cast de tipo objeto y no permite hacer asignacion a conversion unboxing
                    //http://msdn.microsoft.com/es-es/library/aa691159(v=vs.71).aspx
                    //Solucionic.Framework.Utilerias.Utilerias.LiberaObjeto(ref ((object)loAdaptador));
                    LiberaObjeto(ref loAdaptador);                    
               }
          }

          protected override System.Data.DataView RegresaDataView_( string psQuery, string psTabla, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               DataSet ldtsResultado = new DataSet();
               try
               {
                    if (poConeccion.State == ConnectionState.Closed)                    
                         poConeccion.Open();                    
                    if (poTransaccion == null)                    
                         SqlHelper.FillDataset((SqlConnection)poConeccion, CommandType.Text, psQuery, ldtsResultado, new string[] { psTabla });                    
                    else                    
                         SqlHelper.FillDataset((SqlTransaction)poTransaccion, CommandType.Text, psQuery, ldtsResultado, new string[] { psTabla });                    
                    return ldtsResultado.Tables[psTabla].DefaultView;
               }
               catch (Exception ex)
               {                
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {
                    LiberaObjeto(ref ldtsResultado);                    
               }
          }

          protected override int EjecutaComando_( string psQuery, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               try
               {
                    if (poConeccion.State == ConnectionState.Closed)                    
                         poConeccion.Open();                    
                    if (poTransaccion == null)                    
                         return SqlHelper.ExecuteNonQuery((SqlConnection)poConeccion, CommandType.Text, psQuery);                    
                    else                
                         return SqlHelper.ExecuteNonQuery((SqlTransaction)poTransaccion, CommandType.Text, psQuery);                
               }
               catch (Exception ex)
               {                
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {                    
               }
          }

          protected override string RegresaDefinicionTabla_( string psTabla,IDbConnection poConeccion = null, IDbTransaction poTransaccion = null  )
          {
               StringBuilder lsQuery;
               DataView ldvDatos = null;
               int liContador = 0;
               string lsDefinicion = "";
               string lsCampo = null;
               string lsTipo = null;
               lsQuery = new StringBuilder();
               lsQuery.Append("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH ");
               lsQuery.Append(" from information_schema.columns");
               lsQuery.AppendFormat(" WHERE TABLE_NAME='{0}'", psTabla);
               ldvDatos = RegresaDataView_(lsQuery.ToString(), psTabla, poConeccion, poTransaccion);
               if (ldvDatos.Table.Rows.Count > 0)
               {
                    lsCampo = ldvDatos.Table.Rows[0][0].ToString();
                    lsTipo = ldvDatos.Table.Rows[0][1].ToString();
                    if (lsTipo == "varchar")
                         lsTipo = "varchar(" + ldvDatos.Table.Rows[0][2].ToString() + ")";
                    if (lsTipo == "char")
                         lsTipo = "char(" + ldvDatos.Table.Rows[0][2].ToString() + ")";
                    lsDefinicion = " [" + lsCampo + "] " + lsTipo;
                    for (liContador = 1; liContador <= ldvDatos.Table.Rows.Count - 1; liContador++)
                    {
                         lsCampo = ldvDatos.Table.Rows[liContador][0].ToString();
                         lsTipo = ldvDatos.Table.Rows[liContador][1].ToString();
                         if (lsTipo == "varchar")
                              lsTipo = "varchar(" + ldvDatos.Table.Rows[liContador][2].ToString() + ")";
                         if (lsTipo == "char")
                              lsTipo = "char(" + ldvDatos.Table.Rows[liContador][2].ToString() + ")";
                         lsDefinicion += " , ";
                         lsDefinicion += " [" + lsCampo + "] " + lsTipo;
                    }
               }
               return lsDefinicion;
          }

          //public override int EjecutaComando_( string psQuery, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          //{
          //     try
          //     {
          //          if (poConeccion.State == ConnectionState.Closed)                    
          //               poConeccion.Open();                    
          //          if (poTransaccion == null)                
          //               return SqlHelper.ExecuteNonQuery((SqlConnection)poConeccion, CommandType.Text, psQuery);                
          //          else                
          //               return SqlHelper.ExecuteNonQuery((SqlTransaction)poTransaccion, CommandType.Text, psQuery);                
          //     }
          //     catch (Exception ex)
          //     {
          //           GrabaLog(ex.ToString(),poConeccion,poTransaccion);
          //          throw new Exception(ex.Message);
          //     }
          //     finally
          //     {                    
                    
          //     }
          //}

          //protected override DataSet RegresaDataSetyDataAdapter_( string psQuery, string psTabla, ref SqlDataAdapter pdaDataAdapter, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          //{
          //     System.Data.SqlClient.SqlCommandBuilder loComBuilder = null;
          //     DataSet ldsDatos = null;
          //     ldsDatos = new DataSet();
          //     loComBuilder = null;
          //     //<@>TODO: Checar si es necesario, regresar por referencia el CommandBuilder
          //     try
          //     {
          //          pdaDataAdapter = new SqlDataAdapter(psQuery, (SqlConnection)poConeccion);
          //          loComBuilder = new System.Data.SqlClient.SqlCommandBuilder(pdaDataAdapter);
          //          pdaDataAdapter.SelectCommand.CommandTimeout = 0;
          //          //Da tiempo indefinido para cargar lo que viene del select
          //          if (poTransaccion != null)
          //          {
          //               if ((pdaDataAdapter.SelectCommand != null))                         
          //                    pdaDataAdapter.SelectCommand.Transaction = (SqlTransaction)poTransaccion;                         
          //               if ((pdaDataAdapter.UpdateCommand != null))                         
          //                    pdaDataAdapter.UpdateCommand.Transaction = (SqlTransaction)poTransaccion;                        
          //               if ((pdaDataAdapter.InsertCommand != null))                         
          //                    pdaDataAdapter.InsertCommand.Transaction = (SqlTransaction)poTransaccion;                         
          //               if ((pdaDataAdapter.DeleteCommand != null))                         
          //                    pdaDataAdapter.DeleteCommand.Transaction = (SqlTransaction)poTransaccion;                         
          //          }
          //          pdaDataAdapter.Fill(ldsDatos, psTabla);
          //          return ldsDatos;
          //     }
          //     catch (Exception ex)
          //     {
          //          LiberaObjeto(ref ldsDatos);                
          //           GrabaLog(ex.ToString(),poConeccion,poTransaccion);
          //          throw new Exception(ex.Message);
          //     }
          //     finally
          //     {                    
          //     }
          //}
          
          //public string RegresaDefinicionTabla(string psTabla)
          //{
          //     StringBuilder lsQuery;
          //     DataView Ldv_Datos = null;
          //     int Li_Contador = 0;
          //     string lsDefinicion = "";
          //     string lsCampo = null;
          //     string lsTipo = null;               
          //     lsQuery = new StringBuilder();
          //     lsQuery.Append ("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH ");
          //     lsQuery.Append (" from information_schema.columns");
          //     lsQuery.AppendFormat(" WHERE TABLE_NAME='{0}'",psTabla);
          //     Ldv_Datos = RegresaDataView(lsQuery.ToString(), psTabla);
          //     if (Ldv_Datos.Table.Rows.Count > 0)
          //     {
          //          lsCampo = Ldv_Datos.Table.Rows[0][0].ToString();
          //          lsTipo = Ldv_Datos.Table.Rows[0][1].ToString();
          //          if (lsTipo == "varchar")                    
          //               lsTipo = "varchar(" + Ldv_Datos.Table.Rows[0][2].ToString() + ")";                    
          //          if (lsTipo == "char")                    
          //               lsTipo = "char(" + Ldv_Datos.Table.Rows[0][2].ToString() + ")";                
          //          lsDefinicion = " [" + lsCampo + "] " + lsTipo;
          //          for (Li_Contador = 1; Li_Contador <= Ldv_Datos.Table.Rows.Count - 1; Li_Contador++)
          //          {
          //               lsCampo = Ldv_Datos.Table.Rows[Li_Contador][0].ToString();
          //               lsTipo = Ldv_Datos.Table.Rows[Li_Contador][1].ToString();
          //               if (lsTipo == "varchar")                         
          //                    lsTipo = "varchar(" + Ldv_Datos.Table.Rows[Li_Contador][2].ToString() + ")";                         
          //               if (lsTipo == "char")                         
          //                    lsTipo = "char(" + Ldv_Datos.Table.Rows[Li_Contador][2].ToString() + ")";                    
          //               lsDefinicion += " , ";
          //               lsDefinicion += " [" + lsCampo + "] " + lsTipo;
          //          }
          //     }
          //     return lsDefinicion;
          //}
          
          //public string[] RegresaLlavesTabla(string psTabla)
          //{            
          //     StringBuilder lsQuery;
          //     DataView Ldv_Datos = null;
          //     int Li_Contador = 0;
          //     string[] Las_Llaves = null;
          //     Las_Llaves = null;
          //     lsQuery = new StringBuilder();
          //     lsQuery.Append(" Select c.COLUMN_NAME ");
          //     lsQuery.Append(" from     INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,");
          //     lsQuery.Append(" INFORMATION_SCHEMA.KEY_COLUMN_USAGE c");
          //     lsQuery.AppendFormat(" where     pk.TABLE_NAME ='{0}'", psTabla);
          //     lsQuery.Append(" and    CONSTRAINT_TYPE = 'PRIMARY KEY'");
          //     lsQuery.Append(" and    c.TABLE_NAME = pk.TABLE_NAME");
          //     lsQuery.Append(" and    c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME");
          //     Ldv_Datos = RegresaDataView(lsQuery.ToString(), psTabla);
          //     if (Ldv_Datos.Table.Rows.Count > 0)
          //     {
          //          Las_Llaves = new string[Ldv_Datos.Table.Rows.Count];
          //          for (Li_Contador = 0; Li_Contador <= Ldv_Datos.Table.Rows.Count - 1; Li_Contador++)                    
          //               Las_Llaves[Li_Contador] = Ldv_Datos.Table.Rows[Li_Contador][0].ToString();                    
          //     }
          //     return Las_Llaves;
          //}

          protected override void LlenaDataSet_( ref System.Data.DataTable Pds_DataTable, string psQuery, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               System.Data.SqlClient.SqlCommandBuilder loComBuilder = null;
               System.Data.SqlClient.SqlDataAdapter loDataAdapter = null;               
               try
               {
                    loDataAdapter = new SqlDataAdapter(psQuery, (SqlConnection)poConeccion);
                    loComBuilder = new System.Data.SqlClient.SqlCommandBuilder(loDataAdapter);
                    loDataAdapter.SelectCommand.CommandTimeout = 0;
                    //Da tiempo indefinido para cargar lo que viene del select
                    if (poTransaccion != null)                    
                         loDataAdapter.SelectCommand.Transaction = (SqlTransaction)poTransaccion;                    
                    loDataAdapter.Fill(Pds_DataTable);
               }
               catch (Exception ex)
               {               
                      GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                     throw new Exception(ex.Message);
               }
               finally
               {
                    LiberaObjeto(ref loDataAdapter);
                    LiberaObjeto(ref loComBuilder);
               }
          }

          //protected override void UpdateDataSetModificable_( ref System.Data.DataSet Pds_Dataset, ref SqlDataAdapter Pda_DataAdapter)
          //{
          //     SqlCommandBuilder loCommandBuilder = null;
          //     loCommandBuilder = null;
          //     try
          //     {
          //          loCommandBuilder = new SqlCommandBuilder(Pda_DataAdapter);
          //          Pda_DataAdapter.Update(Pds_Dataset.Tables[0]);
          //     }
          //     catch (Exception ex)
          //     {
          //          throw new Exception(ex.Message);
          //     }
          //     finally
          //     {
          //          LiberaObjeto(ref loCommandBuilder);
          //     }               
          //}

          protected override IDbTransaction IniciaTransaccion_( ref IDbConnection poConeccion )
          {
               if (poConeccion.State == ConnectionState.Closed)               
                    poConeccion.Open();               
               return poConeccion.BeginTransaction();
          }

          protected override void ConfirmarTransaccion_( ref IDbTransaction poTransaccion )
          {
               poTransaccion.Commit();
               poTransaccion.Dispose();
               poTransaccion = null;               
          }

          protected override void DeshacerTransaccion_( ref IDbTransaction poTransaccion )
          {
               poTransaccion.Rollback();
               poTransaccion.Dispose();
               poTransaccion = null;
          }

          protected override void EjecutaSp_( string psNombre, System.Collections.ArrayList poParametros, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               int Li_Indice = 0;
               SqlDataAdapter loAdaptador = null;
               loAdaptador = null;
               try
               {
                    if (poConeccion.State == ConnectionState.Closed)                    
                         poConeccion.Open();                    
                    loAdaptador = new SqlDataAdapter(psNombre, (SqlConnection)poConeccion);
                    loAdaptador.SelectCommand.CommandTimeout = 0;
                    loAdaptador.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (poTransaccion != null)
                         loAdaptador.SelectCommand.Transaction = (SqlTransaction)poTransaccion;               
                    for (Li_Indice = 0; Li_Indice <= poParametros.Count - 1; Li_Indice++)                
                         loAdaptador.SelectCommand.Parameters.Add(
                              new SqlParameter(Convert.ToString(((_stParametros)poParametros[Li_Indice]).sParametro), (object)((_stParametros)poParametros[Li_Indice]).sValor));
                    loAdaptador.SelectCommand.ExecuteNonQuery();
               }
               catch (Exception ex)
               {
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {
                    LiberaObjeto(ref loAdaptador);                    
               }
          }

          protected override object EjecutaEscalarSp_( string psNombre, System.Collections.ArrayList poParametros, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               int Li_Indice = 0;
               SqlDataAdapter loAdaptador = null;
               loAdaptador = null;
               try
               {
                    if (poConeccion.State == ConnectionState.Closed)                
                         poConeccion.Open();               
                    loAdaptador = new SqlDataAdapter(psNombre, (SqlConnection)poConeccion);
                    loAdaptador.SelectCommand.CommandTimeout = 0;
                    loAdaptador.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (poTransaccion != null)
                         loAdaptador.SelectCommand.Transaction = (SqlTransaction)poTransaccion;               
                    for (Li_Indice = 0; Li_Indice <= poParametros.Count - 1; Li_Indice++)               
                         loAdaptador.SelectCommand.Parameters.Add(
                              new SqlParameter(Convert.ToString(((_stParametros)poParametros[Li_Indice]).sParametro), ((_stParametros)poParametros[Li_Indice]).sValor)); 
                    return loAdaptador.SelectCommand.ExecuteScalar();
               }  
               catch (Exception ex)
               {
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
                    throw new Exception(ex.Message);
               }
               finally
               {
                    LiberaObjeto(ref loAdaptador);                   
               }
          }

          protected override void GrabaLog_( string psDescripcion, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          {
               StringBuilder lsQuery;
               if (_bGrabandoLog)                            
                //con esta bandera controlamos que no se llame de forma ciclica esta función
                    return;
               try
               {
                    _bGrabandoLog = true;
                    if (poConeccion.State != ConnectionState.Open && !_bConexionAbiertaUnaVez)                
                         return;                
                    //Que cuando no exista la tabla xtsLog la cree
                    //If Not ExisteObjetoEnBD("xtsLog") Then
                    if (!_bExistextsLog)
                    {
                         lsQuery = new StringBuilder();
                         if(!String.IsNullOrEmpty(BDBitacora))
                              lsQuery.AppendFormat("CREATE TABLE [{0}].[dbo].[xtsBitacoraErrores](",BDBitacora);
                         else
                              lsQuery.Append("CREATE TABLE dbo.xtsBitacoraErrores(");                         
                         lsQuery.Append(" ID int IDENTITY(1,1) NOT NULL,");
                         lsQuery.Append(" Fecha datetime NULL CONSTRAINT [DF_xtsBitacoraErrores_Fecha]  DEFAULT (getdate()),");
                         lsQuery.Append(" Descripcion text NULL,");
                         lsQuery.Append(" Origen text NULL,");
                         lsQuery.Append(" CONSTRAINT [PK_xtsBitacoraErrores] PRIMARY KEY CLUSTERED ");
                         lsQuery.Append(" (ID Asc)");
                         lsQuery.Append(" )");
                         EjecutaComando(lsQuery.ToString());
                    }                    
                    psDescripcion = psDescripcion.Replace("'", "''");
                    lsQuery = new StringBuilder();
                    //TODO:Revisar como implementar el origen
                    if (!String.IsNullOrEmpty(BDBitacora))
                         lsQuery.AppendFormat("Insert into [{0}].[dbo].[xtsBitacoraErrores] (Descripcion,Origen) values ('{1}','{2}')",BDBitacora, psDescripcion, BDServidor); 
                    else
                         lsQuery.AppendFormat("Insert into [dbo].[xtsBitacoraErrores] (Descripcion,Origen) values ('{0}','{1}')", psDescripcion, BDServidor); 
                    EjecutaComando(lsQuery.ToString ());
               }
               catch (Exception ex)
               {                    
                     GrabaLog(ex.ToString(),poConeccion,poTransaccion);
               }
               finally
               {
                    //Siempre desactiva la bandera para evitar que se quede prendida
                    _bGrabandoLog = false;                    
               }
          }

          //protected override XmlReader RegresaXmlReader_( string psQuery, IDbConnection poConeccion = null, IDbTransaction poTransaccion = null )
          //{
          //     try
          //     {
          //          if (poConeccion.State == ConnectionState.Closed)
          //               poConeccion.Open();
          //          if (poTransaccion == null)
          //               return SqlHelper.ExecuteXmlReader((SqlConnection)poConeccion, CommandType.Text, psQuery);
          //          else
          //               return SqlHelper.ExecuteXmlReader((SqlTransaction)poTransaccion, CommandType.Text, psQuery);
          //     }
          //     catch (Exception ex)
          //     {
          //           GrabaLog(ex.ToString(),poConeccion,poTransaccion);
          //          throw new Exception(ex.Message);
          //     }
          //     finally
          //     {                    
          //     }
          //     //_oMotorBD.RegresaXmlReader(psQuery);
          //}
          /// <summary>
          /// Funcion que libera objetos del tipo SqlDataAdapter
          /// </summary>
          /// <param name="poObjeto">Objeto del tipo SqlDataAdapter</param>
          private void LiberaObjeto(ref SqlDataAdapter poObjeto)
          {             
               if (poObjeto != null)
               {
                    poObjeto.Dispose();
                    poObjeto = null;
               }
          }

          /// <summary>
          /// Funcion que libera objetos del tipo dataset
          /// </summary>
          /// <param name="poObjeto">Objeto del tipo dataset</param>
          private void LiberaObjeto( ref DataSet poObjeto )
          {
               if (poObjeto != null)
               {
                    poObjeto.Dispose();                    
                    poObjeto = null;
               }
          }

          /// <summary>
          /// Funcion que libera objetos del tipo SqlCommandBuilder
          /// </summary>
          /// <param name="poObjeto">Objeto del tipo SqlCommandBuilder</param>
          private void LiberaObjeto( ref SqlCommandBuilder poObjeto )
          {
               if (poObjeto != null)
               {
                    poObjeto.Dispose();                    
                    poObjeto = null;
               }
          }

          protected override TablaGenerica RegresaTablaGenerica_( string psQuery, string psTabla )
          {
               DataTable loDataTable =null;
               TablaGenerica loTabla = null;
               try
               {                    
                    loDataTable = RegresaDataTable(psQuery, psTabla);
                    loTabla = new TablaGenerica(psTabla);
                    loTabla.ConvertDataTable2List2(loDataTable, psQuery);
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
          #endregion
     }
}
