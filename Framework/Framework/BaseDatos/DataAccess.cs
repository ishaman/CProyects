using System;
using System.Data;
using System.Data.SqlClient;
namespace Solucionic.Framework.BaseDatos
{
    //Todos lo que no tenga modificador de acceso es Private por defecto
     [Serializable]
     public class DataAccess : IDisposable
     {
          #region Variables
          /// <summary>
          /// Interfaces para el motro de la base de datos
          /// </summary>
          //private IBasesDatos _oMotorBD;
          ////private IBasesDatos _oMotorSolExcepcionesBD;
          //private int _iContadorTransaccion;
          ////private Solucionic.Framework.Enumeraciones.enmSistema _enVersionSistema;
          //private IDbConnection _oConexion;    
          //<@> verificar como utilizan este valor
          public struct _stParametros
          {
               public string sParametro;
               public string sValor;
          }
           protected bool _bdisposed;
          private int _iContadorTransaccion;
          private string _sCadenaConexion;
          private Solucionic.Framework.Enumeraciones.enmMotor _enmMotor;
          private string _sBDServidor;
          private string _sBDBitacora;
          private bool  _bConexionAbiertaParaTransaccion;
          #endregion
                                             
          #region Popiedades
          public string BDServidor { get { return _sBDServidor; } }

          public string BDBitacora { get { return _sBDBitacora; } }
          /// <summary>
          /// Tipo de motor de la base de datos
          /// </summary>
          public Solucionic.Framework.Enumeraciones.enmMotor nMotor { get { return _enmMotor; } }

          public string CadenaConexion { get { return _sCadenaConexion; } }
          /////// <summary>
          /////// Si se está ejecutando un proceso que haga muchas actualizaciones
          /////// y para eficientar el performance deje abierta la conexión
          /////// para no estar abriendo y cerrando la misma en cada query
          /////// </summary>
          ////public bool MantenenerConexionAbierta
          ////{
          ////     get { return _oMotorBD.MantenenerConexionAbierta; }
          ////     set { _oMotorBD.MantenenerConexionAbierta = value; }
          ////}
          ///// <summary>
          ///// Muestra si la conexion esta en una transaccion
          ///// </summary>
          ////public IDbTransaction Transaccion
          ////{
          ////     get {return _oMotorBD.Transaccion; }
          ////}
          ///// <summary>
          ///// Regresa el estado de la conexion a la bas de datos
          ///// </summary>
          ////public System.Data.ConnectionState RegresaEstado
          ////{
          ////     get { return _oMotorBD.RegresaEstado; }
          ////}
          
          ///// <summary>
          ///// Manejo de la conexion a la base de datos
          ///// </summary>
          ////public IDbConnection Conexion { get { return _oConexion; } }

          
          #endregion

          #region Constructores
          
        //  public DataAccess( string psCadConexion, Solucionic.Framework.Enumeraciones.enmMotor penMotor)
        //  {
        //       try
        //       {                
        //            CargaVariablesServidor(psCadConexion);
        //            switch (penMotor)
        //            {
        //                 //<@>Valorar si todavia se conservaran estas conexiones 
        //                 //case Solucionic.Framework.Enumeraciones.enmMotor.Oledb:
        //                 //     _oConexion = new System.Data.OleDb.OleDbConnection(psCadConexion);
        //                 //     _oMotorBD = new DataAccessOleDb((OleDbConnection)Conexion);
        //                 //     nMotor = Solucionic.Framework.Enumeraciones.enmMotor.Oledb;
        //                 //     break;
        //                 //<@>Valorar si todavia se conservaran estas conexiones 
        //                 case Solucionic.Framework.Enumeraciones.enmMotor.Sql:
        //                      _oConexion = new SqlConnection(psCadConexion);
        //                      _oMotorBD = new DataAccessSql((SqlConnection)Conexion);
        //                      nMotor = Solucionic.Framework.Enumeraciones.enmMotor.Sql;                              
        //                      break;
        //                 //case Solucionic.Framework.Enumeraciones.enmMotor.Oracle:
        //                 //     nMotor = Solucionic.Framework.Enumeraciones.enmMotor.Oracle;
        //                 //     _oConexion = new OracleConnection(psCadConexion);
        //                 //     _oMotorBD = new DataAccessOracle();
        //                 //     break;
        //            }
        //       }
        //       catch (Exception ex)
        //       {
        //            GrabaLog(ex.ToString());
        //            throw new Exception(ex.Message);
        //       }
        //}

          public DataAccess( string psCadConexion, string psBDBitacora, Solucionic.Framework.Enumeraciones.enmMotor penMotor)
          {               
               _sBDBitacora = psBDBitacora;
               _enmMotor = penMotor;
               _sCadenaConexion = psCadConexion;
               CargaVariablesServidor(psCadConexion);                                                  
          }
        #endregion

        #region Funciones
          private void CargaVariablesServidor( string psCadConexion)
          {
               SqlConnectionStringBuilder loCadConexion = new SqlConnectionStringBuilder(psCadConexion);               
               _sBDServidor = loCadConexion.DataSource;               
          }

          public IDbConnection InstanciaConexion()
          {
               IDbConnection loDBConn;
               loDBConn = null;
               switch (_enmMotor)
               {
                    case Solucionic.Framework.Enumeraciones.enmMotor.Sql:
                         loDBConn = new SqlConnection(_sCadenaConexion);
                         break;                    
               }
               return loDBConn;
          }

          public void DisposeConexion( ref IDbConnection poConexion )
          {
               if (_iContadorTransaccion == 0)
               {
                    if (poConexion != null)
                    {
                         if (poConexion.State != ConnectionState.Closed)
                         {
                              poConexion.Close();
                              poConexion.Dispose();
                              poConexion = null;
                         }
                    }
               }
          }
          protected void ManejaConexionyTransaccion( ref IDbConnection poConexion, ref IDbTransaction poTransaccion, ref bool pbConexion )
          {

               if (poConexion == null && poTransaccion == null)
               {
                    if (poTransaccion == null && _iContadorTransaccion > 0)
                    
                         //Esto valida que cuando alguien haga cualquier operación
                         //dentro de una transacción, no omita pasar la transacción
                         //para que todos los accesos a base de datos, sean en el contexto
                         //de la transacción que esta vigente
                         //esto es porque ahorita puede quedar código que esta en una transacción
                         //y que por algun error durante la migración al nuevo esquema para Azure
                         //no haya invocado los metodos de la base de datos con una transacción
                         throw new ApplicationException("Se ha solicitado un acceso a base de datos sin asignar transacción cuando hay una transacción activa");
                    
                    poConexion = InstanciaConexion();
                    poConexion.Open();
                    pbConexion = true;
               }
          }
          //public TablaGenerica RegresaTablaGenerica( string psQuery, string psTabla )
          //{
          //     return _oMotorBD.RegresaTablaGenerica(psQuery, psTabla);
          //}
          public void ActualizaParams( string pspNombre, string psClasificacion, string psEmpresa, string psEmpleado, byte[] pbFoto, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          {
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    ActualizaParams_(pspNombre, psClasificacion, psEmpresa, psEmpleado, pbFoto, poConexion, poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                    DisposeConexion(ref poConexion);               
               }
          }

          protected virtual void ActualizaParams_( string PspNombre, string psClasificacion, string psEmpresa, string psEmpleado, byte[] pbFoto, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          { 
          }

          public void CreaConsulta( string pscstName, string pscstSQL, bool pbValida = true, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { 
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    CreaConsulta_(pscstName, pscstSQL, pbValida, poConexion, poTransaccion);
               }
               finally
               {
                    if(lbConexion )               
                         DisposeConexion(ref poConexion);            
               } 
          }
          protected virtual void CreaConsulta_( string pscstName, string pscstSQL, bool Pb_Valida=true, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          {
          }
          public void LlenaDataSet(ref System.Data.DataSet pdsDataSet, string psQuery, string psTabla, ref IDbConnection poConexion, ref IDbTransaction poTransaccion) 
          { //Implements IBasesDatos.LlenaDataSet
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    LlenaDataSet_(ref pdsDataSet, psQuery, psTabla, poConexion, poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                    {
                         DisposeConexion(ref poConexion);
                    }
               }
          }
          protected virtual void LlenaDataSet_( ref System.Data.DataSet pdsDataSet, string psQuery, string psTabla, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          {
          }
          public void LlenaDataSet(ref System.Data.DataTable pdsDataTable, string psQuery, ref IDbConnection poConexion, ref IDbTransaction poTransaccion) 
          { 
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    LlenaDataSet_(ref pdsDataTable, psQuery, poConexion, poTransaccion);
               }finally
               {
                    if(lbConexion)                    
                         DisposeConexion(ref poConexion);                    
               }
          }
          protected virtual void LlenaDataSet_( ref System.Data.DataTable pdsDataTable, string psQuery, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          {
          }

          public object EjecutaEscalar( string psQuery, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { 
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return EjecutaEscalar_(psQuery, poConexion, poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                    {
                         DisposeConexion(ref poConexion);
                    }
               }
          }
          protected virtual object EjecutaEscalar_( string psQuery, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          {
               return null;
          }

          public object EjecutaEscalarSp( string psNombre, System.Collections.ArrayList poParametros, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          {
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return EjecutaEscalarSp_(psNombre,poParametros, poConexion, poTransaccion);
               }
               finally
               {
                    if (lbConexion)
                    {
                         DisposeConexion(ref poConexion);
                    }
               }
          }
          protected virtual object EjecutaEscalarSp_( string psNombre, System.Collections.ArrayList poParametros, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          {
               return null;
          }

          public bool EliminaConsulta( string pscstName, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.EliminaConsulta
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return EliminaConsulta_(pscstName, poConexion, poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                         DisposeConexion(ref poConexion);            
               }
          }
          protected virtual bool EliminaConsulta_( string pscstName, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          {
               return false;
          }

          public bool ExisteObjetoEnBD( string psObjeto, string psTipo = "", string psBaseDatos = "", IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.ExisteObjetoEnBD
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return ExisteObjetoEnBD_(psObjeto, psTipo, psBaseDatos, poConexion, poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                         DisposeConexion(ref poConexion);            
               }
          }
          protected virtual bool ExisteObjetoEnBD_( string psObjeto, string psTipo = "", string psBaseDatos = "", IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.ExisteObjetoEnBD
               return false;
          }
         
          public bool ExisteCampoEnTabla( string psTabla, string psCampo, bool pbAzure = false, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.ExisteCampoEnTabla
               bool lbConexion= false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return ExisteCampoEnTabla_(psTabla, psCampo,pbAzure, poConexion, poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                         DisposeConexion(ref poConexion);            
               }
          }
          protected virtual bool ExisteCampoEnTabla_( string psTabla, string psCampo, bool pbAzure = false, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          {
               return false;
          }

          public bool ExisteRegistro( string psTabla, string psCampo, string psValor, string psFiltroEspecial = "",IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.ExisteRegistro
               bool lbConexion= false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return ExisteRegistro_(psTabla, psCampo, psValor,  psFiltroEspecial, poConexion,  poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                         DisposeConexion(ref poConexion);
               }
          }
          protected virtual bool ExisteRegistro_( string psTabla, string psCampo, string psValor,  string psFiltroEspecial = "",  IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          {
               return false;
          }


          public System.Data.DataSet RegresaDataSet( string psQuery, string psTabla, System.Collections.ArrayList poParametros, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.RegresaDataSet
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion,ref poTransaccion, ref lbConexion);
                    return RegresaDataSet_(psQuery, psTabla, poParametros, poConexion, poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                         DisposeConexion(ref poConexion);            
               }
          }
          protected virtual System.Data.DataSet RegresaDataSet_( string psQuery, string psTabla, System.Collections.ArrayList poParametros, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          {
               return null;
          }

          public System.Data.DataSet RegresaDataSet( string psQuery, string psTabla, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          { //Implements IBasesDatos.RegresaDataSet
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return RegresaDataSet_(psQuery, psTabla, poConexion, poTransaccion);
               }
               finally
               {
                    if (lbConexion)
                         DisposeConexion(ref poConexion);
               }
          }
          protected virtual System.Data.DataSet RegresaDataSet_( string psQuery, string psTabla, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          {
               return null;
          }

          public System.Data.DataTable RegresaDataTable( string psQuery, string psTabla, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          { //Implements IBasesDatos.RegresaDataSet
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return RegresaDataTable_(psQuery, psTabla, poConexion, poTransaccion);
               }
               finally
               {
                    if (lbConexion)
                         DisposeConexion(ref poConexion);
               }
          }
          protected virtual System.Data.DataTable RegresaDataTable_( string psQuery, string psTabla, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          {
               return null;
          }

          public System.Data.IDataReader RegresaDataReader( string psQuery,  IDbConnection poConexion = null,  IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.RegresaDataReader
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return RegresaDataReader_(psQuery,  poConexion,  poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                         DisposeConexion(ref poConexion);            
               }
          }
          protected virtual System.Data.IDataReader RegresaDataReader_( string psQuery,  IDbConnection poConexion = null,  IDbTransaction poTransaccion = null ) 
          {
               return null;
          }

          public System.Data.DataView RegresaDataView( string psQuery, string psTabla,  IDbConnection poConexion = null,  IDbTransaction poTransaccion = null ) 
          { // Implements IBasesDatos.RegresaDataView               
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return RegresaDataView_(psQuery, psTabla,  poConexion,  poTransaccion);
               }finally
               {
                    if( lbConexion )
                         DisposeConexion(ref poConexion);               
               }
          }
          protected virtual System.Data.DataView RegresaDataView_( string psQuery, string psTabla,  IDbConnection poConexion = null,  IDbTransaction poTransaccion = null ) 
          {
               return null;
          }

          public int EjecutaComando( string psQuery,  IDbConnection poConexion = null,  IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.EjecutaComando
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return EjecutaComando_(psQuery,  poConexion,  poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                    {
                         DisposeConexion(ref poConexion);
                    }
               }
          }
          protected virtual int EjecutaComando_( string psQuery,  IDbConnection poConexion = null,  IDbTransaction poTransaccion = null ) 
          {
               return 0;
          }
          public DataSet RegresaDataSetyDataAdapter( string psComando, string psTabla, ref object poDataAdapter, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.RegresaDataSetyDataAdapter
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return RegresaDataSetyDataAdapter_(psComando, psTabla, ref poDataAdapter,  poConexion,  poTransaccion);
               }
               finally
               {
                    if( lbConexion )                
                         poConexion.Close();
               }
          }

          protected virtual DataSet RegresaDataSetyDataAdapter_( string psComando, string psTabla, ref object poDataAdapter, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          {
               return null;
          }
 
          public void UpdateDataSetModificable(ref System.Data.DataSet pdsDataset, ref object Pda_DataAdapter) 
          { //Implements IBasesDatos.UpdateDataSetModificable
               try{ 
                    UpdateDataSetModificable_(ref pdsDataset, ref Pda_DataAdapter);
               }
               finally
               {
               }
          }  
          protected virtual void UpdateDataSetModificable_(ref System.Data.DataSet pdsDataset, ref object Pda_DataAdapter) 
          {
          }

          public IDbTransaction IniciaTransaccion(  IDbConnection poConexion = null ) 
          { //Implements IBasesDatos.IniciaTransaccion
               IDbTransaction loTransaccion = null;
               //El parametro opcional permite que la transacción se haga sobre un objeto conexión que ya
               //estaba abierto, en caso de que no se asigne, se crea una conexión
               //que la misma clase se encargará de liberar
               try
               { 
                    if( poConexion == null )
                    {
                         poConexion = InstanciaConexion();
                         poConexion.Open();
                         _bConexionAbiertaParaTransaccion = true;
                    } 
                    if( _iContadorTransaccion == 0 )                    
                         loTransaccion = IniciaTransaccion_(ref poConexion);                    
                    _iContadorTransaccion += 1;
                    return loTransaccion;
               }
               finally
               {
                    if( _bConexionAbiertaParaTransaccion )                    
                //No cierra la conexion sino solo quita la referencia de esta variable
                //el objeto transaccion tambien tiene una referencia de la conexion
                         poConexion = null;            
               } 
          }  
          protected virtual IDbTransaction IniciaTransaccion_(ref IDbConnection poConexion) 
          {
               return null;
          }
 
          public void ConfirmarTransaccion(ref IDbTransaction poTransaccion) 
          { //Implements IBasesDatos.ConfirmarTransaccion
               IDbConnection loCon = null;
        //Validamos que el contador no se vaya a negativos y deje el contador
        //incoherente ya que este caso se daría sino hubieran iniciado transacción
               if( _iContadorTransaccion == 0 )
                    throw new ApplicationException("No ha iniciado ninguna transacción");        
               _iContadorTransaccion -= 1;
               if( _iContadorTransaccion == 0 )
               {
                    if( _bConexionAbiertaParaTransaccion )
                         loCon = poTransaccion.Connection;            
                    ConfirmarTransaccion_(ref poTransaccion);
                    if( _bConexionAbiertaParaTransaccion )
                    {
                //Esto es para cerrar explicitamente la referencia al objeto conexion que se creó para la transacción
                         DisposeConexion(ref loCon);
                         _bConexionAbiertaParaTransaccion = false;
                    }
                }
          }  
          protected virtual void ConfirmarTransaccion_(ref IDbTransaction poTransaccion) 
          {
          }
 
          public void DeshacerTransaccion(ref IDbTransaction poTransaccion) 
          { //Implements IBasesDatos.DeshacerTransaccion
               IDbConnection loCon = null;
               //Validamos que el contador no se vaya a negativos y deje el contador
               //incoherente ya que este caso se daría sino hubieran iniciado transacción
               if( _iContadorTransaccion == 0 )
                    throw new ApplicationException("No ha iniciado ninguna transacción");        
               _iContadorTransaccion -= 1;
               if( _iContadorTransaccion == 0 )
               {
                    if( _bConexionAbiertaParaTransaccion )
                    {
                         loCon = poTransaccion.Connection;
                    } 
                    DeshacerTransaccion_(ref poTransaccion);
                    if( _bConexionAbiertaParaTransaccion )
                    {
                         //Esto es para cerrar explicitamente la referencia al objeto conexion que se creó para la transacción
                         DisposeConexion(ref loCon);
                         _bConexionAbiertaParaTransaccion = false;
                    }
               }      
          }  
          protected virtual void DeshacerTransaccion_(ref IDbTransaction poTransaccion) 
          {
          }
          public void EjecutaSp( string psNombre, System.Collections.ArrayList poParametros, IDbConnection poConexion = null, IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.EjecutaSp
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion,ref poTransaccion, ref lbConexion);
                    EjecutaSp_(psNombre, poParametros,  poConexion,  poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                         DisposeConexion(ref poConexion);               
               }
          }
          protected virtual void EjecutaSp_( string psNombre, System.Collections.ArrayList poParametros,  IDbConnection poConexion = null,  IDbTransaction poTransaccion = null ) 
          {
          }


          public void GrabaLog( string psDescripcion,  IDbConnection poConexion = null,  IDbTransaction poTransaccion = null ) 
          { //Implements IBasesDatos.GrabaLog
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    GrabaLog_(psDescripcion, poConexion, poTransaccion);
               }
               finally
               {
                    if( lbConexion )
                         DisposeConexion(ref poConexion);
            
               }
          }

          protected virtual void GrabaLog_( string psDescripcion,  IDbConnection poConexion = null,  IDbTransaction poTransaccion = null ) 
          { 
          }

          public string RegresaDefinicionTabla( string psTabla, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          {
               bool lbConexion = false;
               try
               {
                    ManejaConexionyTransaccion(ref poConexion, ref poTransaccion, ref lbConexion);
                    return RegresaDefinicionTabla_(psTabla, poConexion, poTransaccion);
               }
               finally
               {
                    if (lbConexion)
                         DisposeConexion(ref poConexion);

               }
               //return _oMotorBD.RegresaDefinicionTabla(psTabla);
          }
          protected virtual string RegresaDefinicionTabla_( string psTabla, IDbConnection poConexion = null, IDbTransaction poTransaccion = null )
          {
           return "";
          }

          public TablaGenerica RegresaTablaGenerica( string psQuery, string psTabla )
          {
               return RegresaTablaGenerica_(psQuery, psTabla );
          }

          protected virtual TablaGenerica RegresaTablaGenerica_( string psQuery, string psTabla )
          {
               return null;
          }
          #region Disposable
          public void Clear()
          {
              
          }

          /// <summary>
          /// Implementación de IDisposable. No se sobreescribe.
          /// </summary>
          public void Dispose() 
          {
               this.Dispose(true);
               // GC.SupressFinalize quita de la cola de finalización al objeto.
               GC.SuppressFinalize(this);
          }
 
          /// <summary>
          /// Limpia los recursos manejados y no manejados.
          /// </summary>
          /// <param name="poDisposing">
          /// Si es true, el método es llamado directamente o indirectamente
          /// desde el código del usuario.
          /// Si es false, el método es llamado por el finalizador
          /// y sólo los recursos no manejados son finalizados.
          /// </param>
          protected virtual void Dispose(bool poDisposing) 
          {
               // Preguntamos si Dispose ya fue llamado.
               if (!this._bdisposed) 
               {
                    if (poDisposing) 
                    {

                          ;
                         //Llamamos al Dispose de todos los RECURSOS MANEJADOS.                         
                    } 
                    // Acá finalizamos correctamente los RECURSOS NO MANEJADOS
                    
                    
               }
               this._bdisposed = true;
          }
 
          /// <summary>
          /// Destructor de la instancia
          /// </summary>
          ~DataAccess() 
          {
               this.Dispose(false);
          }

          
          #endregion
          //Implements IBasesDatos.GrabaLog
          ///// <summary>
          ///// Funcion que permite cambiar a otra vase de datos en un mismo servidor
          ///// </summary>
          ///// <param name="psNombreBaseDatos"></param>
          //public void CambiaBasedeDatos(string psNombreBaseDatos)
          //{
          //     _oMotorBD.CambiaBasedeDatos(psNombreBaseDatos);
          //}
          ///// <summary>
          ///// Cierra la coenxion a la base de datos
          ///// </summary>
          //public void Close()
          //{
          //     if (_oConexion.State == ConnectionState.Open)
          //     {
          //          _oConexion.Close();
          //          _oConexion = null;
          //          _oMotorBD = null;
          //     }
          //}
          ///// <summary>
          ///// Libera los recursos a la conexiona la base de datos
          ///// </summary>
          //public void Dispose()
          //{
          //     _oConexion.Dispose();
          //}
          ///// <summary>
          ///// Funcion que actualiza registros
          ///// </summary>
          ///// <param name="psSpNombre"></param>
          ///// <param name="psClasificacion"></param>
          ///// <param name="psEmpresa"></param>
          ///// <param name="psEmpleado"></param>
          ///// <param name="pbyFoto"></param>
          //public void ActualizaParams(string psSpNombre, string psClasificacion, string psEmpresa, string psEmpleado, byte[] pbyFoto)
          //{
          //     _oMotorBD.ActualizaParams(psSpNombre, psClasificacion, psEmpresa, psEmpleado, pbyFoto);
          //}
          ///// <summary>
          ///// Funcion que crear una consulta (view)
          ///// </summary>
          ///// <param name="psCstName">nombre de la vista</param>
          ///// <param name="psCstSQL">consulta que crea la vista</param>
          ///// <param name="pbValida">valida que exista la cvista</param>
          //public void CreaConsulta(string psCstName, string psCstSQL, bool pbValida = true)
          //{
          //     _oMotorBD.CreaConsulta(psCstName, psCstSQL, pbValida);
          //}
          ///// <summary>
          ///// 
          ///// </summary>
          ///// <param name="pdsDataSet"></param>
          ///// <param name="psQuery"></param>
          ///// <param name="psTabla"></param>
          //public void LlenaDataSet(ref System.Data.DataSet pdsDataSet, string psQuery, string psTabla)
          //{
          //     _oMotorBD.LlenaDataSet(ref pdsDataSet, psQuery, psTabla);            
          //}
          ///// <summary>
          ///// 
          ///// </summary>
          ///// <param name="pdtDataTable"></param>
          ///// <param name="psQuery"></param>
          //public void LlenaDataSet(ref System.Data.DataTable pdtDataTable, string psQuery)
          //{
          //     _oMotorBD.LlenaDataSet(ref pdtDataTable, psQuery);
          //}
          ///// <summary>
          ///// Funcion que ejecuta un una consulta para retornar un solo valor
          ///// </summary>
          ///// <param name="psQuery">Cadena de la consulta a ejecutar</param>
          ///// <returns></returns>
          //public object EjecutaEscalar(string psQuery)
          //{
          //     return _oMotorBD.EjecutaEscalar(psQuery);
          //}
          ///// <summary>
          ///// Funcion que ejecuta un Store Procedure que retorna un solo valor
          ///// </summary>
          ///// <param name="psNombre">Nombre del Store Procedure</param>
          ///// <param name="poParametros">Lista de parametros del Store Procedure</param>
          ///// <returns></returns>
          //public object EjecutaEscalarSp(string psNombre, System.Collections.ArrayList poParametros)
          //{
          //     return _oMotorBD.EjecutaEscalarSp(psNombre, poParametros);
          //}
          ///// <summary>
          ///// 
          ///// </summary>
          ///// <param name="psCstName"></param>
          ///// <returns></returns>
          //public bool EliminaConsulta(string psCstName)
          //{
          //     return _oMotorBD.EliminaConsulta(psCstName);
          //}
          ///// <summary>
          ///// Funcion que determina si existe un objeto en la base de datos
          ///// </summary>
          ///// <param name="psObjeto">Nombre del objeto</param>
          ///// <returns>Retorna si existe o no el objeto en la base de datos</returns>
          //public bool ExisteObjetoEnBD( string psObjeto, string psBaseDatos = "" )
          //{
          //     return _oMotorBD.ExisteObjetoEnBD(psObjeto, psBaseDatos);
          //}
          ///// <summary>
          ///// Funcion que determina si existe un objeto en la base de datos
          ///// </summary>
          ///// <param name="psTabla">Nombre de la tabla</param>
          ///// <param name="psCampo">Nombre del campo</param>
          ///// <param name="pbAzure">Variable para determinar si la consulta se ejecuta en SQL Azure</param>
          ///// <returns>Retorna un true si el campo existe en la base de datos, False en caso contrario</returns>
          //public bool ExisteCampoEnTabla(string psTabla, string psCampo, bool pbAzure = false)
          //{
          //     return _oMotorBD.ExisteCampoEnTabla(psTabla, psCampo, pbAzure);
          //}
          ///// <summary>
          ///// Funcion que determina si existe un registro en la base de datos
          ///// </summary>
          ///// <param name="psTabla">Nombre de la tabla</param>
          ///// <param name="psCampo">Nombre del campo</param>
          ///// <param name="psValor">Valor del campo (registro)</param>
          ///// <param name="psFiltroEspecial">Permite agregar un filtro especial a la consulta</param>
          ///// <returns>Retorna true si existe el registro en la tabla </returns>
          //public bool ExisteRegistro(string psTabla, string psCampo, string psValor, ref string psFiltroEspecial )
          //{
          //     return _oMotorBD.ExisteRegistro(psTabla, psCampo, psValor,ref  psFiltroEspecial);
          //}
          ///// <summary>
          ///// Funcion que retorna un dataset de de los datos
          ///// </summary>
          ///// <param name="psQuery">Cadena de la consulta</param>
          ///// <param name="psTabla">Nombre de la tabla</param>
          ///// <returns></returns>
          //public System.Data.DataSet RegresaDataSet(string psQuery, string psTabla)
          //{
          //     return _oMotorBD.RegresaDataSet(psQuery, psTabla);
          //}

          ///// <summary>
          ///// Funcion que retorna un datatable de de los datos
          ///// </summary>
          ///// <param name="psQuery">Cadena de la consulta</param>
          ///// <param name="psTabla">Nombre de la tabla</param>
          ///// <returns></returns>
          //public System.Data.DataTable RegresaDataTable( string psQuery, string psTabla )
          //{
          //     return _oMotorBD.RegresaDataTable(psQuery, psTabla);
          //}
          
          ///// <summary>
          ///// Funcion que retorna un IDatReader de los datos
          ///// </summary>
          ///// <param name="psQuery">Cadena de la consulta</param>
          ///// <returns></returns>
          //public System.Data.IDataReader RegresaDataReader(string psQuery)
          //{
          //     return _oMotorBD.RegresaDataReader(psQuery);
          //}
          ///// <summary>
          ///// Funcion que regresa un dataview de los datos
          ///// </summary>
          ///// <param name="psQuery">Cadena de la consulta</param>
          ///// <param name="psTabla">Nombre de la tabla</param>
          ///// <returns></returns>
          //public System.Data.DataView RegresaDataView(string psQuery, string psTabla)
          //{
          //     return _oMotorBD.RegresaDataView(psQuery, psTabla);
          //}
          ///// <summary>
          ///// Funcion que ejecuta un comando en la base de datos
          ///// </summary>
          ///// <param name="psQuery">Cadena de la consulta a ejecutar</param>
          ///// <returns>Retorna el numero de registros afectados</returns>
          //public int EjecutaComando(string psQuery)
          //{
          //     return _oMotorBD.EjecutaComando(psQuery);
          //}
          ///// <summary>
          ///// Funcion que ejecuta un comando en la basde de datos usando una transaccion
          ///// </summary>
          ///// <param name="psQuery"></param>
          ///// <param name="poTransaccion"></param>
          ///// <returns></returns>
          //public int EjecutaComando(string psQuery, ref System.Data.IDbTransaction  poTransaccion)
          //{
          //     return _oMotorBD.EjecutaComando(psQuery, ref poTransaccion);
          //}
          ///// <summary>
          ///// Funcion que retorna un DataSet
          ///// </summary>
          ///// <param name="psComando"></param>
          ///// <param name="psTabla"></param>
          ///// <param name="poDataAdapter"></param>
          ///// <returns></returns>
          //public DataSet RegresaDataSetyDataAdapter( string psComando, string psTabla, ref SqlDataAdapter poDataAdapter )
          //{
          //     return _oMotorBD.RegresaDataSetyDataAdapter(psComando, psTabla, ref poDataAdapter);
          //}
          //public int UpdateDataSetModificable( ref System.Data.DataSet pdsDataset, ref SqlDataAdapter pdaDataAdapter )
          //{
          //     return _oMotorBD.UpdateDataSetModificable(ref pdsDataset,ref  pdaDataAdapter);
          //}
          ///// <summary>
          ///// Funcion que regresa la defincion de una tabla
          ///// </summary>
          ///// <param name="psTabla"></param>
          ///// <returns></returns>
          //public string RegresaDefinicionTabla(string psTabla)
          //{
          //     return _oMotorBD.RegresaDefinicionTabla(psTabla);
          //}
          ///// <summary>
          ///// Funcion que regresa las llaves de una tabla
          ///// </summary>
          ///// <param name="psTabla">NOmbre de la tabla</param>
          ///// <returns>Arreglo de string con las llaves de la tabla</returns>
          //public string[] RegresaLlavesTabla(string psTabla)
          //{
          //     return _oMotorBD.RegresaLlavesTabla(psTabla);
          //}
          ///// <summary>
          ///// Funcion que regresa un DataSet
          ///// </summary>
          ///// <param name="psQuery">Cadena con la consulta </param>
          ///// <param name="psTabla">Nombre de la tabla</param>
          ///// <param name="poParametros">Lista de parametros para el dataset</param>
          ///// <returns></returns>
          //public System.Data.DataSet RegresaDataSet(string psQuery, string psTabla, ArrayList poParametros)
          //{
          //     return _oMotorBD.RegresaDataSet(psQuery, psTabla, poParametros);
          //}
          ///// <summary>
          ///// Funcion que inicia una transaccion
          ///// </summary>
          //public void IniciaTransaccion()
          //{
          //     if (_iContadorTransaccion == 0)            
          //          _oMotorBD.IniciaTransaccion();            
          //     _iContadorTransaccion += 1;
          //}
          ///// <summary>
          ///// Validamos que el contador no se vaya a negativos y deje el contador
          ///// incoherente ya que este caso se daría sino hubieran iniciado transacción
          ///// </summary>
          //public void ConfirmarTransaccion()
          //{ 
          //     if (_iContadorTransaccion == 0)            
          //          throw new ApplicationException("No ha iniciado ninguna transacción");            
          //     _iContadorTransaccion -= 1;
          //     if (_iContadorTransaccion == 0)               
          //          _oMotorBD.ConfirmarTransaccion();               
          //} 
          ///// <summary>
          ///// Validamos que el contador no se vaya a negativos y deje el contador
          ///// incoherente ya que este caso se daría sino hubieran iniciado transacción
          ///// </summary>
          //public void DeshacerTransaccion()
          //{
          //     if (_iContadorTransaccion == 0)               
          //          throw new ApplicationException("No ha iniciado ninguna transacción");            
          //     _iContadorTransaccion -= 1;
          //     if (_iContadorTransaccion == 0)               
          //          _oMotorBD.DeshacerTransaccion();            
          //}
          ///// <summary>
          ///// Funcion que ejecuta un Store Procedure
          ///// </summary>
          ///// <param name="psNombre">Nombre del Store Procedure</param>
          ///// <param name="poParametros">Lista con los parametro del store procedure</param>
          //public void EjecutaSp(string psNombre, System.Collections.ArrayList poParametros)
          //{
          //     _oMotorBD.EjecutaSp(psNombre, poParametros);
          //}
          ///// <summary>
          ///// Funcion que graba en log
          ///// </summary>
          ///// <param name="psDescripcion">Cadena con la descripcion del evento</param>
          //public void GrabaLog(string psDescripcion)
          //{               
          //     _oMotorBD.GrabaLog(psDescripcion);
          //}
          //public XmlReader RegresaXmlReader( string psQuery )
          //{
          //     return _oMotorBD.RegresaXmlReader(psQuery);
          //}

          
          #endregion
     }
}
