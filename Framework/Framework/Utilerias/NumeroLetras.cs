using System;
using System.Collections;

namespace Solucionic.Framework.Utilerias
{
     public static class NumeroLetras
     {          
          private static bool _bCentavo;          
          private static string _sCanLe;
          private static string _sUno;
          private static string _sUnomil;          
          private static string _sUnob;          
          private static string _sDos;
          private static string _sTres;
          private static string _sUne1;
          private static string _sCb;
          private static string _sCm;
          private static string _sCz;
          private static string _sCy;
          private static string _sCx;
          private static string _sCc;
          private static string _sCT;
          private static string _sItem;
          private static string _sIt;
          /// <summary>
          /// Esta rutina convierte una cantidad en número a letras
          /// </summary>
          /// <param name="psCantidad"></param>
          /// <returns></returns>
          public static string NumLetra( string psCantidad )
          {
               //string functionReturnValue = null;
               //
               string lsmillo_arriba = null;
               int liCont = 0;
               string lsLETRAS = null;
               int liTam = 0;
               int liIndice = 0;
               string lsA = null;
               int liSigno = 0;
               string lsValor = null;               
               //si llega con muchos decimales la cantidad
               //tiene problemas la función
               psCantidad = Convert.ToString(Math.Round(Convert.ToDouble(psCantidad), 2));               
               //Le hago una validación para importes negativos
               if (Convert.ToDouble(psCantidad) < 0)               
                    liSigno = -1;               
               else               
                    liSigno = 1;               
               //Le pone la multiplicación
               psCantidad = Convert.ToString((Convert.ToDecimal(psCantidad) * liSigno));
               lsmillo_arriba = "";
               _sUno = new String(' ', 8);
               _sUnomil = new String(' ', 8);               
               _sUnob = new String(' ', 8);               
               _sDos = new String(' ', 11);               
               _sTres = new String(' ', 13);
               _bCentavo = false;
               liCont = 0;
               lsLETRAS = "";
               if (double.Parse(psCantidad) <= 999999999999999.0)
               {
                    _sCc = new String(' ', 3);
                    liTam = psCantidad.Length;
                    for (liIndice = 0; liIndice <= (psCantidad.Trim().Length - 1); liIndice++)
                    {
                         lsA = psCantidad.Substring(liIndice + 1, 1);
                         liCont = liCont + 1;
                         if (psCantidad.Substring(liIndice + 1, 1) == ".")
                         {
                              _sCc = psCantidad.Substring(liIndice + 2, 2);
                              if (_sCc.Length == 2)
                              {
                                   liIndice = psCantidad.Trim().Length - 1;
                                   psCantidad = psCantidad.Substring(1, liTam - 3);
                                   liTam = psCantidad.Length;
                              }
                              else if (_sCc.Length == 1)
                              {
                                   liIndice = (psCantidad.Trim().Length - 1);
                                   psCantidad = psCantidad.Substring(1, liTam - 2);
                                   liTam = psCantidad.Length;
                              }
                         }
                    }
                    psCantidad = new String(' ', 15 - liTam) + psCantidad;
                    liTam = psCantidad.Length;
                    //18
                    if (liTam == 15)
                    {
                         _sCb = psCantidad.Substring(1, 3);
                         // BILLONES
                         _sCm = psCantidad.Substring(4, 3);
                         // MILES DE MILLONES
                         _sCz = psCantidad.Substring(7, 3);
                         // MILLONES
                         _sCy = psCantidad.Substring(10, 3);
                         // MILES
                         _sCx = psCantidad.Substring(13, 3);
                         // CENTENAS
                    }
                    else                    
                         throw new ApplicationException("Numletra: Cantidad incorrecta");                         
                    
                    if (!string.IsNullOrEmpty(_sCb.Trim()))
                    {
                         _sCanLe = lsLETRAS;
                         _sCT = _sCb;
                         Triple();
                         Impre();
                         if (Int32.Parse(_sCb) > 1)
                              lsLETRAS += " BILLONES ";                         
                         else                         
                              lsLETRAS += " BILLON ";                         
                         if ((_sCm == "000") & (_sCz == "000") & (_sCy == "000") & (_sCx == "000"))                         
                              lsLETRAS += " DE ";
                         
                    }
                    if ((!string.IsNullOrEmpty(_sCm.Trim())) & (_sCm.Trim() != "000"))
                    {
                         _sCT = _sCm;
                         Triple();
                         if ((_sCm.Substring(2, 1) == " ") & (_sCm.Substring(3, 1) == "1"))
                         {
                              _sUnob = new String(' ', 6);
                              _sUno = new String(' ', 8);
                         }
                         Impre();
                         lsLETRAS += " MIL ";
                         if ((_sCz == "000") & (_sCy == "000") & (_sCx == "000"))                         
                              lsLETRAS += " DE ";
                         
                    }
                    if ((!string.IsNullOrEmpty(_sCz.Trim())) & (_sCz.Trim() != "000"))
                    {
                         _sCT = _sCz;
                         Triple();
                         Impre();
                         if (Int32.Parse(_sCz) > 1)                         
                              lsmillo_arriba = _sCanLe + " MILLONES ";                         
                         else                         
                              lsmillo_arriba = _sCanLe + " MILLON";                         
                         if ((_sCy == "000") & (_sCx == "000"))                         
                              lsLETRAS += " DE  ";                         
                    }
                    if (!string.IsNullOrEmpty(_sCm.Trim()))                    
                         lsLETRAS += new String(' ', 2);                    
                    if ((!string.IsNullOrEmpty(_sCy.Trim())) & (_sCy.Trim() != "000"))
                    {
                         _sCT = _sCy;
                         Triple();
                         if ((string.IsNullOrEmpty(_sCy.Substring(2, 1).Trim())) & (_sCy.Substring(3, 1) == "1"))
                         {
                              _sUnomil = new String(' ', 6);
                              _sUno = new String(' ', 8);
                         }
                         Impre();
                         lsLETRAS = _sCanLe + lsLETRAS + " MIL ";
                    }
                    if (!string.IsNullOrEmpty(_sCx.Trim()))
                    {
                         _sCT = _sCx;
                         Triple();
                         Impre();
                         lsLETRAS += _sCanLe;
                    }
                    if (!string.IsNullOrEmpty(lsmillo_arriba.Trim()))                    
                         lsLETRAS = lsmillo_arriba + " " + lsLETRAS;
                    
                    lsLETRAS += " PESOS";

                    if (!string.IsNullOrEmpty(_sCc.Trim()) && Int32.Parse(_sCc) > 0)
                    {
                         if (_sCc.Length == 2)
                         {
                              lsA = _sCc + "/100  M.N.";
                              liTam = lsLETRAS.Length;
                              lsLETRAS = "( " + lsLETRAS + " " + _sCc + "/100  M.N. )";
                         }
                         else if (_sCc.Length == 1)
                         {
                              lsA = _sCc + "0/100  M.N.";
                              liTam = lsLETRAS.Length;
                              lsLETRAS = "( " + lsLETRAS + " " + _sCc + "0/100  M.N. )";
                         }
                    }
                    else                    
                         lsLETRAS = "( " + lsLETRAS + " 00/100  M.N. )";                    
               }

               lsValor = lsLETRAS;
               if (liSigno < 0)               
                    lsValor = "(  MENOS " + lsValor.Substring(2).TrimStart();               
               return lsValor;               
          }
          /// <summary>
          /// 
          /// </summary>
          private static void CENTENA()
          {
               //Rutina auxiliar para la conversión de número a letra
               string lsF = null;

               lsF = _sIt.Substring(1, 1);
               if ((_sIt.Substring(3, 1) == "1") & (_sIt.Substring(2, 1) != "1"))               
                    _sUno = "UN";               
               if (lsF == "1")               
                    if (_sIt.Substring(2, 2) == "00")                    
                         _sTres = "CIEN";                    
                    else                    
                         _sTres = "CIENTO";                                   
               else if (lsF == "2")               
                    _sTres = "DOSCIENTOS";               
               else if (lsF == "3")               
                    _sTres = "TRESCIENTOS";               
               else if (lsF == "4")               
                    _sTres = "CUATROCIENTOS";               
               else if (lsF == "5")               
                    _sTres = "QUINIENTOS";               
               else if (lsF == "6")               
                    _sTres = "SEISCIENTOS";               
               else if (lsF == "7")               
                   _sTres = "SETECIENTOS";               
               else if (lsF == "8")               
                    _sTres = "OCHOCIENTOS";               
               else if (lsF == "9")               
                    _sTres = "NOVECIENTOS";               
          }
          /// <summary>
          /// 
          /// </summary>
          private static void Triple()
          {
               //Rutina auxiliar para la conversión de número a letra
               //al parecer estas dos variables son inservibles
               object d = null;
               object A = null;
               d = 0;
               if (_bCentavo == false)
               {
                    _sItem = _sCT.Substring(3, 1);
                    UNIDAD();
                    if ((_sCT.Substring(2, 1) != "0") & (!string.IsNullOrEmpty(_sCT.Substring(2, 1).Trim())))
                    {
                         _sIt = _sCT.Substring(2, 2);
                         DECENA();
                         A = d;
                    }
                    if ((_sCT.Substring(1, 1) != "0") & (!string.IsNullOrEmpty(_sCT.Substring(1, 1).Trim())))
                    {
                         _sIt = _sCT;
                         CENTENA();
                         A = d;
                    }
               }
               else
               {
                    _sItem = _sCT.Substring(2, 1);
                    UNIDAD();
                    _sIt = _sCT.Substring(1, 1);
                    DECENA();
               }
          }
          /// <summary>
          /// 
          /// </summary>
          private static void Impre()
          {
               //Rutina auxiliar para la conversión de número a letra
               //Parameters tres, dos, une1, uno, CanLe
               _sCanLe = "";
               if (_sTres != new String(' ', 13))               
                    _sCanLe += _sTres + " ";
               
               if (_sDos != new String(' ', 11))               
                    if (!string.IsNullOrEmpty(_sUno.Trim()))
                         if (_sUne1 == " Y ")                         
                              _sCanLe += _sDos + _sUne1;                         
                         else                         
                              if (_sDos.ToUpper().Trim() == "VEINTI")                              
                                   _sCanLe += _sDos + _sUne1;                              
                              else                              
                                   _sCanLe += _sDos + _sUne1 + " Y ";                             
                    else                    
                         _sCanLe += _sDos + _sUne1;
                    
               
               if (_sUno != new String(' ', 8))               
                    if (_sCanLe.ToUpper().Trim() == "VEINTI")                    
                         _sCanLe += _sUno.Trim();                   
                    else                    
                         _sCanLe += _sUno;                                   
               _sTres = new String(' ', 13);
               _sDos = new String(' ', 11);
               _sUno = new String(' ', 8);
          }
          /// <summary>
          /// 
          /// </summary>
          private static void UNIDAD()
          {
               //Rutina auxiliar para la conversión de número a letra (Parameters Item, uno, une1)
               switch (_sItem)
               {
                    case "0":
                         _sUne1 = new String(' ', 0);
                         break;
                    case "1":
                         _sUno = "UN";
                         break;
                    case "2":
                         _sUno = "DOS";
                         break;
                    case "3":
                         _sUno = "TRES";
                         break;
                    case "4":
                         _sUno = "CUATRO";
                         break;
                    case "5":
                         _sUno = "CINCO";
                         break;
                    case "6":
                         _sUno = "SEIS";
                         break;
                    case "7":
                         _sUno = "SIETE";
                         break;
                    case "8":
                         _sUno = "OCHO";
                         break;
                    case "9":
                         _sUno = "NUEVE";
                         break;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          private static void DECENA()
          {
               //Rutina auxiliar para la conversión de número a letra
               //Parameters it, dos, uno, une1
               string F = null;
               if (Int32.Parse(_sIt) > 9 && Int32.Parse(_sIt) <= 19)
              {
                    if (_sIt == "10")                    
                         _sDos = "DIEZ";                    
                    else if (_sIt == "11")                    
                         _sDos = "ONCE";                    
                    else if (_sIt == "12")                    
                         _sDos = "DOCE";                    
                    else if (_sIt == "13")                    
                         _sDos = "TRECE";                    
                    else if (_sIt == "14")                    
                         _sDos = "CATORCE";                    
                    else if (_sIt == "15")                    
                         _sDos = "QUINCE";                    
                    else if (_sIt == "16")                    
                         _sDos = "DIECISEIS";                    
                    else if (_sIt == "17")                    
                         _sDos = "DIECISIETE";                    
                    else if (_sIt == "18")                    
                         _sDos = "DIECIOCHO";                    
                    else if (_sIt == "19")                    
                         _sDos = "DIECINUEVE";                    
                    _sUno = new String(' ', 8);
                    _sUne1 = new String(' ', 0);
               }
               else
               {
                    if (_sIt.Substring(2, 1) == "1")
                    {
                         _sUne1 = " Y ";
                         _sUno = " UN ";
                    }
                    F = _sIt.Substring(1, 1);
                    if (F == "2")                    
                         if (_sIt.Substring(2, 1) == "0")                         
                              _sDos = "VEINTE";                         
                         else
                         {
                              _sUne1 = "";
                              _sDos = "VEINTI";
                         }                    
                    else if (F == "3")                    
                         _sDos = "TREINTA";                    
                    else if (F == "4")                    
                         _sDos = "CUARENTA";                    
                    else if (F == "5")                    
                         _sDos = "CINCUENTA";                    
                    else if (F == "6")                    
                         _sDos = "SESENTA";                    
                    else if (F == "7")                    
                         _sDos = "SETENTA";                    
                    else if (F == "8")                    
                         _sDos = "OCHENTA";                    
                    else if (F == "9")                    
                         _sDos = "NOVENTA";
                    
               }
          }
          public static ArrayList LlenaArregloConEnumeracion( System.Enum Pen_Type, ref string psFormato )
          {
               int liIndex = 0;
               Array laoValores = null;
               ArrayList laoValorEnum = null;
               //Obtenemos los valores de la enumeracion 
               laoValores = System.Enum.GetValues(Pen_Type.GetType());
               laoValorEnum = new ArrayList();
               foreach (int liIndex_loopVariable in laoValores)
               {
                    liIndex = liIndex_loopVariable;
                    laoValorEnum.Add(System.Enum.Format(Pen_Type.GetType(), liIndex, psFormato) + "," + liIndex);
               }
               //Format(Description)
               //"G" or "g" If value is equal to a defined value of enumType , returns the element name defined for value. 
               //If the FlagsAttribute attribute is set on the Enum declaration and value is a built-in integer type and is equal 
               //to a summation of enumeration elements, the return value contains the element names in an unspecified order, 
               //separated by commas (e.g. "Red, Yellow"). Otherwise, value is returned in decimal format.
               //"X" or "x" Returns value in hexadecimal format, without a leading 0x. The value is padded with leading zeroes to ensure the returned value is at least eight digits in length. 
               //"F" or "f" Behaves identically to "G", except the FlagsAttribute is not required to be present on the Enum declaration. 
               //"D" or "d" Returns value in decimal format with no leading zeroes. 
               return laoValorEnum;
               //Regresa el diccionario
          }
          public static ArrayList LlenaArregloConEnumeracionColores( System.Enum Pen_Type, ref string psFormato, bool pbElementoVacio = false )
          {
               Array laoValores = null;
               ArrayList laoValorEnum = null;
               //Obtenemos los valores de la enumeracion 
               laoValores = System.Enum.GetValues(Pen_Type.GetType());
               laoValorEnum = new ArrayList();
               laoValorEnum = LlenaArregloConEnumeracion(Pen_Type, ref psFormato);
               if (pbElementoVacio)
                    laoValorEnum.Insert(0, "Sin Color,");
               return laoValorEnum;
               //Regresa el diccionario
          }
          public static int GeneraNumeroAleatorio( int Pi_ValorMinimo, int Pi_ValorMaximo )
          {
               Random loGeneraNumeroRandom = null;
               try
               {
                    loGeneraNumeroRandom = new Random();
                    return loGeneraNumeroRandom.Next(Pi_ValorMinimo, Pi_ValorMaximo);
               }
               finally
               {
                    loGeneraNumeroRandom = null;
               }
          }

          public static bool EsNumerico(this string psValor )
          {
               Double result;
               return Double.TryParse(psValor, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out result);
          }
     }
}
