using System;
using System.Collections.Generic;
using System.Linq;

//using System.Text;
//using System.Threading.Tasks;

namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// 
     /// </summary>
     [Serializable]
     public class TablaGenericaRegistro : TablaGenerica
     {
          /// <summary>
          /// 
          /// </summary>
          protected List<Fila> _oRegistrosRespaldoOriginal;
          /// <summary>
          /// 
          /// </summary>
          public CampoBusqueda[] FindFields;
          /// <summary>
          /// 
          /// </summary>
          private ModosDeEdicion _oModoEdicion;

         
          /// <summary>
          /// 
          /// </summary>
          private int _iFila;
          /// <summary>
          /// 
          /// </summary>
          private int _iFilaAntesAdd;
          /// <summary>
          /// 
          /// </summary>       
          private bool _bBOF;
          /// <summary>
          /// 
          /// </summary>
          private bool _bEOF;
          /// <summary>
          /// 
          /// </summary>
          private enmEstado _oState;

          #region Propiedades
          /// <summary>
          /// 
          /// </summary>
          public ModosDeEdicion EditMode
          {
               get
               {
                    return _oModoEdicion;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          public int AbsolutePosition
          {
               get
               {
                    return _iFila + 1;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          public bool BOF
          {
               get
               {
                    return this._bBOF;
               }
               set
               {
                    this._bBOF = value;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          public bool EOF
          {
               get
               {
                    return this._bEOF;
               }
               set
               {
                    this._bEOF = value;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          public enmEstado State
          {
               get
               {
                    //0=Cerrado
                    //1=Abierto
                    return _oState;
               }
          }
          public int Longitud { get; set; }
          #endregion

          #region Constructores
          /// <summary>
          /// 
          /// </summary>
          public TablaGenericaRegistro()
               : base()
          {
               this._bEOF = true;
               this._bBOF = true;
               _oModoEdicion = ModosDeEdicion.AdEditNone;
               _oState = enmEstado.CERRADO;
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psNombreTabla"></param>
          public TablaGenericaRegistro( string psNombreTabla )
               : base(psNombreTabla)
          {
               this._bEOF = true;
               this._bBOF = true;
               _oModoEdicion = ModosDeEdicion.AdEditNone;
               _oState = enmEstado.CERRADO;
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="poTabla"></param>
          public TablaGenericaRegistro( TablaGenerica poTabla )
          {
               base._sNombreTabla = poTabla.NombreTabla;
               base._iTotalFilas = poTabla.TotalFilas;
               base._iTotalColumnas = poTabla.TotalColumnas;
               base._oNombreColumnas = poTabla.NombreColumnas;
               base._oRegistros = poTabla.Registros;
               base._sQueryFuente = poTabla.Fuente;
               this._bEOF = true;
               this._bBOF = true;
               _oModoEdicion = ModosDeEdicion.AdEditNone;
               _oState = enmEstado.CERRADO;
          }

          #endregion

          #region Metodos
          /// <summary>
          /// 
          /// </summary>

          public void Open()
          {
               if (_oState == enmEstado.ABIERTO)
                    //31/08/2011 para liberar la memoria correcamente si se abre y cierra varias veces el registro
                    Close();

               //_sQueryFuente = psQuery;
               //this = new TablaGenericaRegistro(pdaConexion.RegresaTablaGenerica(_sQueryFuente, "Tabla"));
               this._iFila = 0;
               this._bBOF = ( _oRegistros.Count == 0 );
               this._bEOF = ( _oRegistros.Count == 0 );
               _oState = enmEstado.ABIERTO;
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psCampo"></param>
          /// <param name="psValor"></param>
          /// <param name="pbFiltroAnidado"></param>
          /// <param name="psCondicion"></param>
          public void Filter( string psCampo = "", string psValor = "", bool pbFiltroAnidado = false, enmOperaciones  psCondicion = enmOperaciones.IGUAL )
          {
               if (!String.IsNullOrEmpty(psCampo) && !String.IsNullOrEmpty(psValor))
               {
                    if (Object.Equals(_oRegistrosRespaldoOriginal, null))
                         //Si _oRegistroFiltro es nulo nunca se ha ejecutado un filtro.
                         //Se hace un respaldo de los datos originales
                         _oRegistrosRespaldoOriginal = _oRegistros;
                    else if (!pbFiltroAnidado)
                         //Solo si se quiere no se ejecuta un filtro aninado
                         _oRegistros = _oRegistrosRespaldoOriginal;

                    //Se hace filtro sobre el campo y el valor especifico
                    switch (psCondicion)
                    {
                         case enmOperaciones.IGUAL:
                              var loFiltro = from m in _oRegistros
                                             where m.ValorFila[psCampo.ToLower()].ToString() == psValor
                                             select m;
                              _oRegistros = loFiltro.ToList();
                              break;
                         case enmOperaciones.DIFERENTE:
                              var loFiltroD = from m in _oRegistros
                                              where m.ValorFila[psCampo.ToLower()].ToString() != psValor
                                              select m;
                              _oRegistros = loFiltroD.ToList();
                              break;
                         case enmOperaciones.MENORQUE:
                              var loFiltroMe = from m in _oRegistros
                                               where Convert.ToInt32(m.ValorFila[psCampo.ToLower()].ToString()) < Convert.ToInt32(psValor)
                                               select m;
                              _oRegistros = loFiltroMe.ToList();
                              break;
                         case enmOperaciones.MAYORQUE:
                              var loFiltroMa = from m in _oRegistros
                                               where Convert.ToInt32(m.ValorFila[psCampo.ToLower()].ToString()) > Convert.ToInt32(psValor)
                                               select m;
                              _oRegistros = loFiltroMa.ToList();
                              break;
                         case enmOperaciones.MENORIGUALQUE:
                              var loFiltroMeI = from m in _oRegistros
                                                where Convert.ToInt32(m.ValorFila[psCampo.ToLower()].ToString()) <= Convert.ToInt32(psValor)
                                                select m;
                              _oRegistros = loFiltroMeI.ToList();
                              break;
                         case enmOperaciones.MAYORIGUALQUE :
                              var loFiltroMaI = from m in _oRegistros
                                                where Convert.ToInt32(m.ValorFila[psCampo.ToLower()].ToString()) >= Convert.ToInt32(psValor)
                                                select m;
                              _oRegistros = loFiltroMaI.ToList();
                              break;
                         case enmOperaciones.IN:
                              var loFiltroIn = from m in _oRegistros.Where(m => psValor.Contains(m.ValorFila[psCampo].ToString()))
                                               select m;
                              _oRegistros = loFiltroIn.ToList();
                              break;                        
                         default:
                              var loFiltroDe = from m in _oRegistros
                                               where m.ValorFila[psCampo.ToLower()].ToString() == psValor
                                               select m;
                              _oRegistros = loFiltroDe.ToList();
                              break;
                    }
                    //var loFiltro = from m in _oRegistros
                    //               where m.ValorFila[psCampo.ToLower()].ToString() == psValor
                    //               select m;

                    //Se convierte el resultado en el tipo de dato original
                    //Esto es para manejar de manera transparente la estructura que contiene los registros
                    //originales de los datos. es decir, siempre manejar la variable _oRegistros como datos de origen.
                    //_oRegistros = loFiltro.ToList();
                    _iTotalFilas = _oRegistros.Count();

                    return;
               }
               if (!Object.Equals(_oRegistrosRespaldoOriginal, null))
               {
                    //Esta condicion permite quitar el filtro 
                    _oRegistros = _oRegistrosRespaldoOriginal;
                    _iTotalFilas = _oRegistros.Count();
                    _oRegistrosRespaldoOriginal.Clear();
                    _oRegistrosRespaldoOriginal = null;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psCampo"></param>
          /// <param name="psAscDes"></param>
          public void Sort( string psCampo, string psAscDes = "ASC" )
          {
               if (String.IsNullOrEmpty(psCampo) || String.IsNullOrEmpty(psAscDes.ToLower().Trim()))
                    return;
               switch (psAscDes.Trim().ToLower())
               {
                    case "DESC":
                         var loSortDes = from m in _oRegistros
                                         orderby m.ValorFila[psCampo.ToLower()].ToString() descending
                                         select m;
                         _oRegistros = loSortDes.ToList();
                         break;
                    case "ASC":
                         var loSortAsc = from m in _oRegistros
                                         orderby m.ValorFila[psCampo.ToLower()].ToString() ascending
                                         select m;
                         _oRegistros = loSortAsc.ToList();
                         break;
                    default:
                         var loSort = from m in _oRegistros
                                      orderby m.ValorFila[psCampo.ToLower()].ToString() ascending
                                      select m;
                         _oRegistros = loSort.ToList();
                         break;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="poRow"></param>
          /// <param name="piPosicion"></param>
          public void AddRow( Fila poRow, int piPosicion )
          {
               _oRegistros.Insert(piPosicion, poRow);
          }
          /// <summary>
          /// 
          /// </summary>
          public void MoveFirst()
          {
               ValidaQueNoHayaEdicion();
               if (this._bBOF & this._bEOF)
                    throw new ApplicationException("No hay registros");

               this._bBOF = ( _oRegistros.Count == 0 );
               this._bEOF = ( _oRegistros.Count == 0 );

               _iFila = 0;
               _oModoEdicion = ModosDeEdicion.AdEditNone;
          }
          /// <summary>
          /// 
          /// </summary>
          public void MovePrevious()
          {
               ValidaQueNoHayaEdicion();
               if (this._bBOF & this._bEOF)
                    throw new ApplicationException("No hay registros");

               if (this._bBOF)
                    throw new ApplicationException("El registro se encuentra en BOF");

               _iFila -= 1;
               this._bEOF = false;
               //if (Cdv_Dat.RowFilter == "")
               //{
               //    this.Cb_BOF = (_iFila < 0);
               //}
               //else
               //{
               //    this.Cb_BOF = (_iFila < 0);
               //}
               this._bBOF = ( _iFila < 0 );
               _oModoEdicion = ModosDeEdicion.AdEditNone;
          }
          /// <summary>
          /// 
          /// </summary>
          public void MoveNext()
          {
               ValidaQueNoHayaEdicion();
               if (this._bBOF & this._bEOF)
                    throw new ApplicationException("No hay registros");

               if (this._bEOF)
                    throw new ApplicationException("El registro se encuentra en EOF");

               _iFila += 1;
               this._bBOF = false;

               if (Object.Equals(_oRegistrosRespaldoOriginal, null))
                    this._bEOF = ( _iFila > _oRegistrosRespaldoOriginal.Count - 1 );
               else
                    this._bEOF = ( _iFila > _oRegistros.Count - 1 );

               _oModoEdicion = ModosDeEdicion.AdEditNone;
          }
          /// <summary>
          /// 
          /// </summary>
          public void MoveLast()
          {
               ValidaQueNoHayaEdicion();
               if (this._bBOF & this._bEOF)
                    throw new ApplicationException("No hay registros");

               if (Object.Equals(_oRegistrosRespaldoOriginal, null))
                    _iFila = _oRegistrosRespaldoOriginal.Count - 1;
               else
                    _iFila = _oRegistros.Count - 1;

               this._bBOF = false;
               this._bEOF = false;
               _oModoEdicion = ModosDeEdicion.AdEditNone;
          }
          /// <summary>
          /// 
          /// </summary>
          public void Close()
          {
               _oModoEdicion = ModosDeEdicion.AdEditNone;
               _oState = enmEstado.CERRADO;
               if (!Object.Equals(_oRegistros, null))
               {
                    //this.Dispose();
                    _oRegistros = null;
                    //Cdv_Dat.Dispose();
                    //Cdv_Dat = null;
               }
               FindFields = null;
          }
          /// <summary>
          /// 
          /// </summary>
          private void ValidaQueNoHayaEdicion()
          {
               if (_oModoEdicion != ModosDeEdicion.AdEditNone)
               {
                    //Throw New ApplicationException("Cancele la operación de edición pendiente antes de mover el registro")
                    //11/Agosto/2005
                    //Iván
                    //Es mejor que si hubo cambios, simplemente los pierda
                    //y ya si la UI (Interfaz de Usuario)
                    //quisiera validar que no se muevan si hay una actualización
                    //pendiente, mejor que use la propiedad de Status de Registro
                    //y ya en esa capa validarlo.
                    //Si no se validó simplemente la clase pierde todo cambió
                    //que no se haya guardado
                    //en el dataView interno.
                    //CancelUpdate();
               }
          }
          /// <summary>
          /// 
          /// </summary>
          public void Update()
          {
               //Cdv_Dat.Table.AcceptChanges();
               //Pone el modo de edición en modo
               _oModoEdicion = ModosDeEdicion.AdEditNone;
          }
          #endregion
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psCampo"></param>
          /// <returns></returns>


          public bool Contains( string psCampo )
          {
               if (NombreColumnas.Contains(psCampo))
                    return true;
               return false;
          }

          //TODO: Comentar la acción del método Open, ya que hace lo mismo que lo siguiente _oSistema.Conexion.RegresaTablaGenerica(lsQuery,"Tabla");
          /// <summary>
          /// 
          /// </summary>
          /// <param name="campo"></param>
          /// <returns></returns>
          public object this[string campo]
          {
               get
               {
                    if (Object.Equals(_oRegistrosRespaldoOriginal, null))
                    {
                         var loFine = ( from m in _oRegistros
                                        where m.ValorFila[campo].ToString() == campo
                                        select m ).ToList();

                         return this[_iFila, campo]; //Cdv_Dat.Table.Rows[_iFila][Campo];
                    }
                    else
                    {
                         return this[_iFila, campo];
                    }
               }
               set
               {
                    //Puede actualizar ya los valores
                    //Cdv_Dat.Table.Rows[_iFila][Campo] = value;                
                    this[_iFila, campo] = value;
                    if (_oModoEdicion == ModosDeEdicion.AdEditNone)
                    {
                         _oModoEdicion = ModosDeEdicion.AdEditInProgress;
                    }
               }
          }
          /// <summary>
          /// 
          /// </summary>
          public void Delete()
          {
               if (!( EOF | BOF ))
               {
                    _oRegistros.Remove(_oRegistros[_iFila]); //Quita la fila actual
                    //Cdv_Dat.Table.AcceptChanges(); //Para que actualice los cambios

                    _oModoEdicion = ModosDeEdicion.AdEditNone;

                    this._iFila = 0; //Hace un movefirst si es que hay registros
                    this._bBOF = ( _oRegistros.Count == 0 );
                    this._bEOF = ( _oRegistros.Count == 0 );
               }
          }

          //TODO: Analizar está fracción de código ya que a mi parecer el AddNew como ya no se tiene una posición es mejor agregarlo directo
          //a la lista por ejemplo _oRegistro.Add(Fila Item);
          /// <summary>
          /// 
          /// </summary>
          public void AddNew()
          {
               //Fila ldrAdd = new Fila();
               //_oRegistros.Add(ldrAdd);

               _oModoEdicion = ModosDeEdicion.AdEditAdd;

               _iFilaAntesAdd = _iFila;

               _iFila = _oRegistros.Count - 1; //Lo posiciona en la ultima fila
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="s"></param>
          /// <param name="a"></param>
          /// <param name="b"></param>
          /// <returns></returns>
          public static string Mid( string s, int a, int b )
          {
               string temp = s.Substring(a - 1, b);
               return temp;
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="paFind"></param>
          /// <returns></returns>
          public int Find( Fila paFind )
          {
               //_oRegistros.Find(paFind);
               return this.Find(paFind);
          }
          /// <summary>
          /// 
          /// </summary>
          public void Find()
          {
               Find(FindFields);
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="paValor"></param>
          /// <returns></returns>
          public int Find( char paValor )
          {
               return Convert.ToInt32(_oRegistros.Find(p => p.ValorFila.ToString() == paValor.ToString()));
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="paArreglo"></param>
          public void Find( CampoBusqueda[] paArreglo )
          {
               int licontador;
               string lsSort;
               CampoBusqueda[] lpaarreglo = paArreglo;
               object[] lvalores = new object[paArreglo.Length];
               lsSort = "";
               ValidaQueNoHayaEdicion();

               for (licontador = 0 ; licontador <= ( lpaarreglo ).GetUpperBound(1) ; licontador++)
               {
                    lsSort += lpaarreglo[licontador].campo + ",";
                    lvalores[licontador] = lpaarreglo[licontador].valor;
               }

               lsSort = Mid(lsSort, 1, lsSort.Length - 1);
               Sort(lsSort);

               for (int i = 0 ; i <= lvalores.Length ; i++)
                    _iFila = Convert.ToInt32(_oRegistros.Find(p => p.ValorFila[lvalores[i].ToString()].ToString() == lvalores[i].ToString()));
               //_ifila = _oregistros.find(lvalores.cast<fila>);

               if (_iFila == -1)
               {
                    //si no encontró el valor buscado
                    if (!Object.Equals(_oRegistrosRespaldoOriginal, null))
                         _iFila = _oRegistrosRespaldoOriginal.Count;
                    else
                         _iFila = _oRegistros.Count;
                    this._bEOF = true;
               }

               this._bBOF = ( _iFila < 0 );

               if (!Object.Equals(_oRegistrosRespaldoOriginal, null))
                    this._bEOF = ( _iFila > _oRegistrosRespaldoOriginal.Count - 1 );
               else
                    this._bEOF = ( _iFila > _oRegistros.Count - 1 );
               _oModoEdicion = ModosDeEdicion.AdEditNone;
          }
     }
}