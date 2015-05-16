using System;
using System.Data.SqlClient;
using System.Threading;  
namespace Solucionic.Framework.BaseDatos
{
     public class ManejaConexion : IDisposable
     {
          #region Objetos de conexion
          /// <summary>
          /// 
          /// </summary>
          [ThreadStatic]
          static ManejaConexion _currentScope;
          /// <summary>
          /// 
          /// </summary>
          [ThreadStatic]
          static SqlTransaction _oTransaccionActual;
          /// <summary>
          /// 
          /// </summary>
          [ThreadStatic] 
          static SqlConnection _oConexionActual; 
          /// <summary>
          /// 
          /// </summary>
          static string _sConexion;
          /// <summary>
          /// Determinar si la conexion esta abierta anteriormente
          /// </summary>
          //static bool _bConexionAbierta;          

          #endregion
          #region Variables
          /// <summary>
          /// 
          /// </summary>
          private bool _isDisposed;
          /// <summary>
          /// Esta variable es para determinar quien fue la primera instancia. Ya que esta tendra un false como valor.
          /// Y se encargara de hacer el Dispose de la conexion
          /// </summary>
          private bool _isNested;
          
          #endregion

          #region Propiedades
          /// <summary>
          /// 
          /// </summary>
          public static ManejaConexion CurrentScope { get { return _currentScope; } }
          /// <summary>
          /// 
          /// </summary>
          public static SqlTransaction TransaccionActual { get { return _oTransaccionActual; } }
          /// <summary>
          /// 
          /// </summary>
          public static SqlConnection ConexionActual { get { return _oConexionActual; } }
          #endregion

          #region Constructores
          /// <summary>
          /// 
          /// </summary>
          public ManejaConexion()
               : this(_sConexion)
          {
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psCadenaConexion"></param>
          public ManejaConexion(string psCadenaConexion)
          {
               _sConexion = psCadenaConexion;
               if (!Object.Equals(_currentScope, null) && !_currentScope._isDisposed)
               {
                    //this.ConexionActual = _currentScope.ConexionActual;
                    _isNested = true;
                    //_bConexionAbierta = true;
               }
               else
               {
                    //Crear la nueba conecion de datos
                    //_currentConexion = new DbConnection();
                    //this.ConexionActual = new SqlConnection(psCadenaConexion);
                    _oConexionActual = new SqlConnection(psCadenaConexion);
                    //if (_bConexionAbierta)
                    //{
                    //     this.ConexionActual.Open();
                    //     _bConexionAbierta = true;
                    //}
                    if (!( _oConexionActual.State == System.Data.ConnectionState.Open ))
                         _oConexionActual.Open();  
                    Thread.BeginThreadAffinity();
                    _currentScope = this;
               }
          }
          #endregion
         
          /// <summary>
          /// 
          /// </summary>
          public static void IniciaTransaccion()
          {
               if (ManejaConexion.CurrentScope != null)
               {
                    if (_oTransaccionActual == null)
                    {
                         //var connection = ManejaConexion.CurrentScope.ConexionActual;
                         var connection = ManejaConexion.ConexionActual;
                         //if (!_bConexionAbierta)                       
                         if(!(connection.State == System.Data.ConnectionState.Open))
                              connection.Open();                                                       
                         _oTransaccionActual = connection.BeginTransaction();
                    }
               }
          }
          /// <summary>
          /// 
          /// </summary>
          public static void ConfirmarTransaccion()
          {
               if (_oTransaccionActual != null)
               {
                    _oTransaccionActual.Commit();                    
                    _oTransaccionActual = null;
               }
          }

          //public void Commit()
          //{
          //     if (_oTransaccionActual != null)
          //     {
          //          if (this == _currentScope)
          //          {
          //               _oTransaccionActual.Commit();
          //               _oTransaccionActual = null;
          //          }
          //     }
          //}
          /// <summary>
          /// 
          /// </summary>
          public static void DeshacerTransaccion()
          {
               if (_oTransaccionActual != null)
               {
                    _oTransaccionActual.Rollback();
                    //_oTransaccionActual.Connection.Close();
                    _oTransaccionActual = null;
                    
               }
          }
          /// <summary>
          /// 
          /// </summary>
          public void Dispose()
          {
               if (!_isNested && !_isDisposed)
               {
                    if (_oTransaccionActual != null)
                    {
                         _oTransaccionActual.Dispose();
                         _oTransaccionActual = null;
                    }
                    if (_oConexionActual != null)
                    {
                         _oConexionActual.Dispose();
                         _oConexionActual = null;
                    }                    
                    _currentScope = null;
                    //_bConexionAbierta = false;
                    Thread.EndThreadAffinity();                    
                    _isDisposed = true;
               }
          }
     }
}
