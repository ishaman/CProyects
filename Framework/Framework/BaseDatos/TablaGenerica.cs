using System;
using System.Collections.Generic;
using System.Data;

namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// 
     /// </summary>
     [Serializable]
     public class TablaGenerica: IDisposable 
     {
          #region variables
          /// <summary>
          /// 
          /// </summary>
          protected List<string> _oNombreColumnas;          
          /// <summary>
          /// 
          /// </summary>
          protected List<Fila> _oRegistros;
          /// <summary>
          /// 
          /// </summary>
          protected string _sNombreTabla;
          /// <summary>
          /// 
          /// </summary>
          protected bool _bdisposed;
          /// <summary>
          /// 
          /// </summary>
          protected int _iTotalFilas;
          /// <summary>
          /// 
          /// </summary>
          protected int _iTotalColumnas;
         /// <summary>
         /// 
         /// </summary>
          protected string _sQueryFuente;
          #endregion

          #region Propiedades
          public List<Fila> Registros { get { return _oRegistros; } }
          public List<string> NombreColumnas { get { return _oNombreColumnas; } }
          public string NombreTabla { get { return _sNombreTabla; } }
          public int TotalFilas { get { return _iTotalFilas; } }
          public int TotalColumnas { get { return _iTotalColumnas; } }
          public string Fuente { get { return _sQueryFuente; } }
          public object this[int piFila, int piColumna]
          {
               get { return Registros[piFila].ValorFila[_oNombreColumnas[piColumna]]; }
              set { Registros[piFila].ValorFila[_oNombreColumnas[piColumna]] = value; }
          }

          public object this[int piFila, string psColumna]
          {
               get { return Registros[piFila].ValorFila[psColumna.ToLower()]; }
              set { Registros[piFila].ValorFila[psColumna.ToLower()] = value; }
          }

          #endregion

          #region Constructores
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psNombreTabla"></param>
          public TablaGenerica( string psNombreTabla)
          {
               _sNombreTabla = psNombreTabla;
          }
          /// <summary>
          /// 
          /// </summary>
          public TablaGenerica()
          {
               _sNombreTabla = "TablaGenerica";
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="poTabla"></param>
          public TablaGenerica(DataTable poTabla)
          {
               _sNombreTabla = "TablaGenerica";
               ConvertDataTable2List2(poTabla, "");
          }

          #endregion

          #region Funciones
          /// <summary>
          /// Regresa el valor de una columna. Se utiliza el indice de base cero para retornar el valor
          /// </summary>
          /// <param name="psColumna"></param>
          /// <returns></returns>
          public object Columna( string psColumna )
          {
               return Registros[0].ValorFila[psColumna.ToLower()]; 
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="piColumna"></param>
          /// <returns></returns>
          public object Columna( int piColumna )
          {
               return Registros[0].ValorFila[_oNombreColumnas[piColumna]];
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="piColumna"></param>
          /// <returns></returns>
          public Type TipoDatoColumna( int piColumna )
          {
               return Registros[0].ValorFila[_oNombreColumnas[piColumna]].GetType();
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="dtRecosrds"></param>
          /// <param name="psQuery"></param>
          protected internal void ConvertDataTable2List2(DataTable dtRecosrds, string psQuery)
          {
               List<string> PropertyNames = new List<string>();
               _sQueryFuente = psQuery;
               for (int ier = 0; ier < dtRecosrds.Columns.Count; ier++)
                    PropertyNames.Add(dtRecosrds.Columns[ier].ColumnName.ToLower());
               _oRegistros = new List<Fila>();
               _oNombreColumnas = PropertyNames;
               _iTotalColumnas = dtRecosrds.Columns.Count;
               _iTotalFilas = dtRecosrds.Rows.Count;
               foreach (DataRow Row in dtRecosrds.Rows)
               {
                    var dynamicClass = new Fila();
                    foreach (String propertyName in PropertyNames)
                         dynamicClass.ValorFila[propertyName] = Row[propertyName];
                    _oRegistros.Add(dynamicClass);
               }
               _oNombreColumnas.TrimExcess();
               _oRegistros.TrimExcess();
          }
          #endregion
          #region Disposable
          /// <summary>
          /// 
          /// </summary>
          public void Clear()
          {
               _oRegistros.Clear();
               _oNombreColumnas.Clear();
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
                    if (!Object.Equals(_oNombreColumnas, null))
                    {
                         _oNombreColumnas.Clear();
                         _oNombreColumnas = null;
                    }
                    if (!Object.Equals(_oRegistros, null))
                    {
                         _oRegistros.Clear();
                         _oRegistros= null;
                    }
                    
               }
               this._bdisposed = true;
          }
          public bool ContieneColumna(string psNombreColumna)
          {
               foreach (string lsColumna in _oNombreColumnas)
               {
                    if (lsColumna.ToLower() == psNombreColumna.ToLower())
                         return true;
               }
               return false;
          }
          /// <summary>
          /// Destructor de la instancia
          /// </summary>
          ~TablaGenerica() 
          {
               this.Dispose(false);
          }

          
          #endregion
     }
}
