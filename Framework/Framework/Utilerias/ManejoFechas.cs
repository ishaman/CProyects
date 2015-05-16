using System;
using System.Collections;
using System.Text;

namespace Solucionic.Framework.Utilerias
{
     public static class ManejoFechas
     {
          /// <summary>
          /// 
          /// </summary>
          /// <param name="pemnMes"></param>
          /// <returns></returns>
          public static string ObtenerNombreMes( Mes  pemnMes )
          {
               switch (pemnMes)
               {
                    case Mes.Apertura:
                         return "Apertura";
                    case Mes.Enero:
                         return "Enero";
                    case Mes.Febrero:
                         return "Febrero";
                    case Mes.Marzo:
                         return "Marzo";
                    case Mes.Abril:
                         return "Abril";
                    case Mes.Mayo:
                         return "Mayo";
                    case Mes.Junio:
                         return "Junio";
                    case Mes.Julio:
                         return "Julio";
                    case Mes.Agosto:
                         return "Agosto";
                    case Mes.Septiembre:
                         return "Septiembre";
                    case Mes.Octubre:
                         return "Octubre";
                    case Mes.Noviembre:
                         return "Noviembre";
                    case Mes.Cierre:
                         return "Cierre";
                    default:
                         return "Diciembre";
               }               
          }
          public static string PrimerDiaMes( this DateTime pdtDate )
          {
               return new DateTime(pdtDate.Year, pdtDate.Month, 1).ToString("dd");
          }

          public static string UltimoDiaMes( this DateTime pdtDate )
          {
               if (pdtDate.Month == 12)
                    return "31";
               return new DateTime(pdtDate.Year, pdtDate.Month + 1, 1).AddDays(-1).ToString("dd");
          }
          public static int NumeroDeDiasEnMesActual()
          {
               return DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
          }
          public static int NumeroDeDiasEnMesActual( int piANio, Mes pemMes )
          {
               return DateTime.DaysInMonth(piANio, (int)pemMes);
          }
          /// <summary>
          /// Regresa el ultimo día del mes indicado en la fecha que se le pasa a la funcion
          /// </summary>
          /// <param name="piMes"></param>
          /// <param name="piAño"></param>
          /// <returns></returns>
          public static System.DateTime UltimoDiaDelMes( Mes pemMes, int piAño )
          {
               //La linea siguiente obtiene el ultimo dia del mes(revisar Documentacion de la funcion DateSerial,
               //ya que recalcula la fecha en funcion al día)
               return new DateTime(piAño, (int)( pemMes ) + 1, 1).AddDays(-1);
          }
          /// <summary>
          /// obtiene el ultimo dia del mes(revisar Documentacion de la funcion DateSerial, ya que recalcula la fecha en funcion al día)
          /// </summary>
          /// <param name="pdFecha"></param>
          /// <returns></returns>
          public static System.DateTime UltimoDiaDelMes( System.DateTime pdFecha )
          {
               return new DateTime(pdFecha.Year, pdFecha.Month + 1, 0);
          }
          /// <summary>
          /// Permite saber si el año que se indica en el parametro es bisiesto
          /// </summary>
          /// <param name="piAño"></param>
          /// <returns></returns>
          public static bool EsEjercicioBisisesto( int piAño )
          {
               if (piAño % 4 == 0)
                    if (( piAño % 100 == 0 ) & !( piAño % 400 == 0 ))
                         return false;
                    else
                         return true;
               else
                    return false;
          }
          /// <summary>
          /// Permite saber si el mes indicado en el parametro es bimestre
          /// </summary>
          /// <param name="piMes"></param>
          /// <returns></returns>
          public static bool EsBimestre( int piMes )
          {
               switch (piMes)
               {
                    case 2:
                    case 4:
                    case 6:
                    case 8:
                    case 10:
                    case 12:
                         return true;
                    default:
                         return false;
               }
          }
          /// <summary>
          /// Permite obtener el primer dia del bimestre segun el mes indicado en el parametro
          /// </summary>
          /// <param name="piMes"></param>
          /// <param name="piAño"></param>
          /// <returns></returns>
          public static System.DateTime PrimerDiaBimestre( int piMes, int piAño )
          {
               switch (piMes)
               {
                    case 2:
                    case 4:
                    case 6:
                    case 8:
                    case 10:
                    case 12:
                         return new DateTime(piAño, piMes, 1);
                    default:
                         return DateTime.Now.Date;
               }
          }
          /// <summary>
          /// Permite obtener el primer dia del mes segun el mes indicado en el parametro
          /// </summary>
          /// <param name="piMes"></param>
          /// <param name="piAño"></param>
          /// <returns></returns>
          public static System.DateTime PrimerDiaMes( int piMes, int piAño )
          {
               return new DateTime(piAño, piMes, 1);
          }
          /// <summary>
          /// Permite saber el primer dia habil del mes y año indicados en los parametros
          /// </summary>
          /// <param name="piMes"></param>
          /// <param name="piAño"></param>
          /// <returns></returns>
          public static System.DateTime PrimerDiaHabilDelMes( int piMes, int piAño )
          {
               System.DateTime lddiaHabildelMes = default(System.DateTime);
               lddiaHabildelMes = new DateTime(piAño, piMes, 1);
               if (lddiaHabildelMes.DayOfWeek == System.DayOfWeek.Saturday)
                    return lddiaHabildelMes.AddDays(2);
               if (lddiaHabildelMes.DayOfWeek == DayOfWeek.Sunday)
                    return lddiaHabildelMes.AddDays(1);
               return lddiaHabildelMes;
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psFecha"></param>
          /// <returns></returns>
          public static bool EsFecha( this string psFecha )
          {
               try
               {
                    DateTime ldFecha = DateTime.Parse(psFecha.ToString());
                    if (ldFecha != DateTime.MinValue && ldFecha != DateTime.MaxValue)
                         return true;
                    return false;
               }
               catch
               {
                    return false;
               }
          }

          public static bool EsFecha( string psAnio, string psMes, string psDia )
          {
               try
               {
                    DateTime ldFecha = new DateTime(Convert.ToInt32(psAnio), Convert.ToInt32(psMes), Convert.ToInt32(psDia));
                    if (ldFecha != DateTime.MinValue && ldFecha != DateTime.MaxValue)
                         return true;
                    return false;
               }
               catch
               {
                    return false;
               }
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="piDia"></param>
          /// <returns></returns>
          public static string RegresaDiaSemanaEspañol( ref int piDia )
          {
               string[] lasDias = {
				"DOMINGO",
				"LUNES",
				"MARTES",
				"MIERCOLES",
				"JUEVES",
				"VIERNES",
				"SABADO"
			};
               return lasDias[piDia - 1];
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="piTopeAñosInf"></param>
          /// <param name="piTopeAñosSup"></param>
          /// <returns></returns>
          public static ArrayList RegresaComboAños( int piTopeAñosInf = 0, int piTopeAñosSup = 0 )
          {
               int liCounter = 0;
               ArrayList lasCombo = null;
               int liTope = 0;

               lasCombo = new ArrayList();
               if (piTopeAñosInf == 0)
                    piTopeAñosInf = DateTime.Now.Year - 5;
               if (piTopeAñosSup == 0)
                    piTopeAñosSup = DateTime.Now.Year + 1;
               if (piTopeAñosInf < piTopeAñosSup)
               {
                    for (liCounter = piTopeAñosSup ; liCounter >= piTopeAñosInf ; liCounter += -1)
                    {
                         lasCombo.Add(liCounter.ToString() + "," + liCounter.ToString());
                         liTope += 1;
                         if (liTope == 6)
                              break; // TODO: might not be correct. Was : Exit For

                    }
               }
               else
                    lasCombo.Add(piTopeAñosInf + "," + piTopeAñosInf);

               return lasCombo;
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="pbTodos"></param>
          /// <returns></returns>
          public static ArrayList RegresaListaNombreMeses( bool pbTodos )
          {
               ArrayList lasMeses = null;
               lasMeses = new ArrayList();
               lasMeses.Add("ENERO,1");
               lasMeses.Add("FEBRERO,2");
               lasMeses.Add("MARZO,3");
               lasMeses.Add("ABRIL,4");
               lasMeses.Add("MAYO,5");
               lasMeses.Add("JUNIO,6");
               lasMeses.Add("JULIO,7");
               lasMeses.Add("AGOSTO,8");
               lasMeses.Add("SEPTIEMBRE,9");
               lasMeses.Add("OCTUBRE,10");
               lasMeses.Add("NOVIEMBRE,11");
               lasMeses.Add("DICIEMBRE,12");
               if (pbTodos)
                    lasMeses.Add("TODOS,0");
               return lasMeses;
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="pshAño"></param>
          /// <param name="pbyMes"></param>
          /// <param name="pbyDia"></param>
          /// <returns></returns>
          public static bool EsFechaValida( short pshAño, byte pbyMes, byte pbyDia )
          {
               System.DateTime ldFecha = default(System.DateTime);
               try
               {
                    ldFecha = new System.DateTime(pshAño, pbyMes, pbyDia);
                    return true;
               }
               catch
               {
                    return false;
               }
          }
          /// <summary>
          /// Funcion que retorna las horas entre dos fechas
          /// </summary>
          /// <param name="pdFechaInicial">Fecha Inicial</param>
          /// <param name="pdFechaFinal">Fecha Final</param>
          /// <returns></returns>
          /// <remarks></remarks>
          public static double RegresaNumeroHorasentreFechas( System.DateTime pdFechaInicial, System.DateTime pdFechaFinal )
          {
               return pdFechaFinal.Subtract(pdFechaInicial).Hours;
               //return Math.Abs(DateDiff(DateInterval.Hour, ldFechaFinal, ldFechaInicial, DayOfWeek.Monday));
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="pdFecha"></param>
          /// <param name="pbIncluyeHHmmss"></param>
          /// <returns></returns>
          public static string FechaSQL(this DateTime pdFecha, bool pbIncluyeHHmmss = false )
          {
               StringBuilder lsFechaSQL;
               lsFechaSQL = new StringBuilder();
               lsFechaSQL.AppendFormat("'{0}{1}{2}", pdFecha.Year, ( pdFecha.Month ).ToString().PadLeft(2, '0'), pdFecha.Day.ToString().PadLeft(2, '0'));
               if (pbIncluyeHHmmss == true)
                    lsFechaSQL.AppendFormat(" {0}:{1}:{2}'", pdFecha.Hour.ToString().PadLeft(2, '0'), pdFecha.Minute.ToString().PadLeft(2, '0'), pdFecha.Second.ToString().PadLeft(2, '0'));
               else
                    lsFechaSQL.Append("'");
               return lsFechaSQL.ToString();
          }

          /// <summary>
          /// Esta funcion confierte en formato para base de datos utilizando una cadena con formato dd/MM/yyyy como entrada
          /// </summary>
          /// <param name="psFecha">una cadena con formato dd/MM/yyyy</param>
          /// <returns></returns>
          public static DateTime FechaCortaSQL( this string psFecha )
          {
               string[] lasFecha;
               DateTime ldtFecha;
               try
               {
                    lasFecha = psFecha.Split('/');
                    ldtFecha = new DateTime();
                    if (lasFecha.Length == 3)
                         ldtFecha = new DateTime(Convert.ToInt32(lasFecha[2]), Convert.ToInt32(lasFecha[1]), Convert.ToInt32(lasFecha[0]));
                    return ldtFecha;
               }
               catch
               {
                    return DateTime.Now;
               }
          }         

          #region Quarters

          public static DateTime ObtenerComienzodelCuatrimestre( int piAnio, Cuatrimestre penmCuatrimestre )
          {
               switch (penmCuatrimestre)
               {
                    case Cuatrimestre.Primero:    // 1st Quarter = January 1 to March 31
                         return new DateTime(piAnio, 1, 1, 0, 0, 0, 0);
                    case Cuatrimestre.Segundo: // 2nd Quarter = April 1 to June 30
                         return new DateTime(piAnio, 4, 1, 0, 0, 0, 0);
                    case Cuatrimestre.Tercero: // 3rd Quarter = July 1 to September 30
                         return new DateTime(piAnio, 7, 1, 0, 0, 0, 0);
                    default: // 4th Quarter = October 1 to December 31
                         return new DateTime(piAnio, 10, 1, 0, 0, 0, 0);
               }
          }

          public static DateTime ObtenerelFinaldelCuatrimestre( int piAnio, Cuatrimestre penmCuatrimestre )
          {
               switch (penmCuatrimestre)
               {
                    case Cuatrimestre.Primero:       // 1st Quarter = January 1 to March 31
                         return new DateTime(piAnio, 3, DateTime.DaysInMonth(piAnio, 3), 23, 59, 59, 999);
                    case Cuatrimestre.Segundo: // 2nd Quarter = April 1 to June 30
                         return new DateTime(piAnio, 6, DateTime.DaysInMonth(piAnio, 6), 23, 59, 59, 999);
                    case Cuatrimestre.Tercero:  // 3rd Quarter = July 1 to September 30
                         return new DateTime(piAnio, 9, DateTime.DaysInMonth(piAnio, 9), 23, 59, 59, 999);
                    default: // 4th Quarter = October 1 to December 31
                         return new DateTime(piAnio, 12, DateTime.DaysInMonth(piAnio, 12), 23, 59, 59, 999);
               }
          }

          public static Cuatrimestre ObtenerCuatrimestre( Mes penmMes )
          {
               if (penmMes <= Mes.Marzo)
                    // 1st Quarter = January 1 to March 31
                    return Cuatrimestre.Primero;
               else if (( penmMes >= Mes.Abril ) && ( penmMes <= Mes.Junio ))
                    // 2nd Quarter = April 1 to June 30
                    return Cuatrimestre.Segundo;
               else if (( penmMes >= Mes.Julio ) && ( penmMes <= Mes.Septiembre ))
                    // 3rd Quarter = July 1 to September 30
                    return Cuatrimestre.Tercero;
               else // 4th Quarter = October 1 to December 31
                    return Cuatrimestre.Cuarto;
          }

          public static DateTime ObtenerFinaldelUltimoCuatrimestre()
          {
               if ((Mes)DateTime.Now.Month <= Mes.Marzo)
                    //go to last quarter of previous year
                    return ObtenerelFinaldelCuatrimestre(DateTime.Now.Year - 1, Cuatrimestre.Cuarto);
               else //return last quarter of current year
                    return ObtenerelFinaldelCuatrimestre(DateTime.Now.Year, ObtenerCuatrimestre((Mes)DateTime.Now.Month));
          }

          public static DateTime ObtenerElComienzodelUltimoCuatrimestre()
          {
               if ((Mes)DateTime.Now.Month <= Mes.Marzo)
                    //go to last quarter of previous year
                    return ObtenerComienzodelCuatrimestre(DateTime.Now.Year - 1, Cuatrimestre.Cuarto);
               else //return last quarter of current year
                    return ObtenerelFinaldelCuatrimestre(DateTime.Now.Year, ObtenerCuatrimestre((Mes)DateTime.Now.Month));
          }
          public static DateTime ObtenerElCOmienzodelCuatrimestreActual()
          {
               return ObtenerComienzodelCuatrimestre(DateTime.Now.Year, ObtenerCuatrimestre((Mes)DateTime.Now.Month));
          }

          public static DateTime ObtenerElFinaldelCuatrimestreActual()
          {
               return ObtenerelFinaldelCuatrimestre(DateTime.Now.Year, ObtenerCuatrimestre((Mes)DateTime.Now.Month));
          }
          #endregion

          #region Weeks
          public static DateTime GetStartOfLastWeek()
          {
               int liDaysToSubtract = (int)DateTime.Now.DayOfWeek + 7;
               DateTime ldtFecha = DateTime.Now.Subtract(System.TimeSpan.FromDays(liDaysToSubtract));
               return new DateTime(ldtFecha.Year, ldtFecha.Month, ldtFecha.Day, 0, 0, 0, 0);
          }

          public static DateTime GetEndOfLastWeek()
          {
               DateTime ldtFecha = GetStartOfLastWeek().AddDays(6);
               return new DateTime(ldtFecha.Year, ldtFecha.Month, ldtFecha.Day, 23, 59, 59, 999);
          }

          public static DateTime GetStartOfCurrentWeek()
          {
               int DaysToSubtract = (int)DateTime.Now.DayOfWeek;
               DateTime dt = DateTime.Now.Subtract(System.TimeSpan.FromDays(DaysToSubtract));
               return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
          }

          public static DateTime GetEndOfCurrentWeek()
          {
               DateTime ldtFecha = GetStartOfCurrentWeek().AddDays(6);
               return new DateTime(ldtFecha.Year, ldtFecha.Month, ldtFecha.Day, 23, 59, 59, 999);
          }
          #endregion
          #region Months

          public static DateTime GetStartOfMonth( Mes penmMes, int piAnio )
          {
               return new DateTime(piAnio, (int)penmMes, 1, 0, 0, 0, 0);
          }

          public static DateTime GetEndOfMonth( Mes penmMes, int piAnio )
          {
               return new DateTime(piAnio, (int)penmMes, DateTime.DaysInMonth(piAnio, (int)penmMes), 23, 59, 59, 999);
          }

          public static DateTime GetStartOfLastMonth()
          {
               if (DateTime.Now.Month == 1)
                    return GetStartOfMonth((Mes)12, DateTime.Now.Year - 1);
               else
                    return GetStartOfMonth((Mes)( DateTime.Now.Month - 1 ), DateTime.Now.Year);
          }

          public static DateTime GetEndOfLastMonth()
          {
               if (DateTime.Now.Month == 1)
                    return GetEndOfMonth((Mes)12, DateTime.Now.Year - 1);
               else
                    return GetEndOfMonth((Mes)( DateTime.Now.Month - 1 ), DateTime.Now.Year);
          }

          public static DateTime GetStartOfCurrentMonth()
          {
               return GetStartOfMonth((Mes)DateTime.Now.Month, DateTime.Now.Year);
          }

          public static DateTime GetEndOfCurrentMonth()
          {
               return GetEndOfMonth((Mes)DateTime.Now.Month, DateTime.Now.Year);
          }
          #endregion

          #region Years
          public static DateTime GetStartOfYear( int piAnio )
          {
               return new DateTime(piAnio, 1, 1, 0, 0, 0, 0);
          }

          public static DateTime GetEndOfYear( int piAnio )
          {
               return new DateTime(piAnio, 12, DateTime.DaysInMonth(piAnio, 12), 23, 59, 59, 999);
          }

          public static DateTime GetStartOfLastYear()
          {
               return GetStartOfYear(DateTime.Now.Year - 1);
          }

          public static DateTime GetEndOfLastYear()
          {
               return GetEndOfYear(DateTime.Now.Year - 1);
          }

          public static DateTime GetStartOfCurrentYear()
          {
               return GetStartOfYear(DateTime.Now.Year);
          }

          public static DateTime GetEndOfCurrentYear()
          {
               return GetEndOfYear(DateTime.Now.Year);
          }
          #endregion

          #region Days
          public static DateTime GetStartOfDay( DateTime pdtFecha )
          {
               return new DateTime(pdtFecha.Year, pdtFecha.Month, pdtFecha.Day, 0, 0, 0, 0);
          }

          public static DateTime GetEndOfDay( DateTime pdtFecha )
          {
               return new DateTime(pdtFecha.Year, pdtFecha.Month, pdtFecha.Day, 23, 59, 59, 999);
          }
          #endregion
     }
}
