using System;
using System.Collections;
using System.Text;

namespace Solucionic.Framework.Utilerias
{
     public static class ManejoCadenas
     {
          public static string VerificaNull(object pse, string psString)
          {
               if (pse == null)
                    psString = "";
               else
                    psString = pse.ToString();
               return psString;
          }

          public static ArrayList RegresaMascaras(bool pbElementoVacio = false)
          {
               ArrayList lasMascaras = null;
               lasMascaras = new ArrayList();
               if (pbElementoVacio)
                    lasMascaras.Add(",");
               lasMascaras.Add("Texto, X");
               lasMascaras.Add("Entero, 9999");
               lasMascaras.Add("Decimal, 9999.00");
               lasMascaras.Add("Moneda, 9,999.00");
               lasMascaras.Add("Personalizada, @");
               return lasMascaras;
          }

          /// <summary>
          /// Esta función sirve para el formateo de un string, como en clipper
          /// </summary>
          /// <param name="psCadena"></param>
          /// <param name="psCaracterRelleno"></param>
          /// <param name="pbOrientacionDerecho"></param>
          /// <param name="liLongitud"></param>
          /// <returns></returns>
          public static string Pad( string psCadena, string psCaracterRelleno, bool pbOrientacionDerecho, int liLongitud )
          {
               string lsFunctionReturnValue;
               string lsEspacio;

               if (String.IsNullOrEmpty(psCadena))
                    psCadena = "";
               if (psCadena.Length > liLongitud)
                    lsFunctionReturnValue = psCadena.Substring(1, liLongitud);
               else
               {
                    if (pbOrientacionDerecho)
                    {
                         lsEspacio = new String(' ', liLongitud - psCadena.ToString().Length);
                         lsEspacio = lsEspacio.Replace(" ", psCaracterRelleno);
                         lsFunctionReturnValue = lsEspacio + psCadena;
                    }
                    else
                    {
                         if (liLongitud == psCadena.Length)
                              lsFunctionReturnValue = psCadena;
                         else
                         {
                              lsEspacio = new String(' ', liLongitud - psCadena.Length);
                              lsEspacio = lsEspacio.Replace(" ", psCaracterRelleno);
                              lsFunctionReturnValue = psCadena + lsEspacio;
                         }
                    }
               }
               return lsFunctionReturnValue;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psMensaje"></param>
          /// <returns></returns>
          public static string LimpiaMensaje(this string psMensaje)
          {
               string lsPatron = null;
               System.Text.RegularExpressions.Regex Lo_RegularExp = null;
               psMensaje = psMensaje.Replace("\\", "/");
               lsPatron = "&[^a-zA-Z0-9()@!¡?¿ñÑ.:,;áéíóúÁÉÍÓÚ\\/ " + (char)34 + "]";
               Lo_RegularExp = new System.Text.RegularExpressions.Regex(lsPatron);
               psMensaje = psMensaje.Replace(System.Environment.NewLine, "  ");
               psMensaje = psMensaje.Replace("\r", "  ");
               psMensaje = psMensaje.Replace("'", "");
               return Lo_RegularExp.Replace(psMensaje, "  ");
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psMensaje"></param>
          /// <returns></returns>
          public static string CambiaMensaje(this string psMensaje)
          {
               psMensaje = psMensaje.Replace("Ñ", "N");
               psMensaje = psMensaje.Replace("ñ", "n");
               return psMensaje;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psValue"></param>
          /// <param name="piMaxLength"></param>
          /// <returns></returns>
          public static string StringRight(this string psValue, int piMaxLength)
          {
               //Check if the value is valid
               if (string.IsNullOrEmpty(psValue))
                    //Set valid empty string as string could be null
                    psValue = string.Empty;
               else if (psValue.Length > piMaxLength)
                    //Make the string no longer than the max length
                    psValue = psValue.Substring(psValue.Length - piMaxLength, piMaxLength);
               //Return the string
               return psValue;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psValue"></param>
          /// <param name="piMaxLength"></param>
          /// <returns></returns>
          public static string StringLeft(this string psValue, int piMaxLength)
          {
               //Check if the value is valid
               if (string.IsNullOrEmpty(psValue))
                    //Set valid empty string as string could be null
                    psValue = string.Empty;
               psValue = psValue.Substring(0, Math.Min(piMaxLength, psValue.Length));
               //Return the string
               return psValue;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psTextoALimpiar"></param>
          /// <param name="pbSoportaEñe"></param>
          /// <param name="pbCambiaÑporN"></param>
          /// <returns></returns>
          public static string LimpiaTexto(this string psTextoALimpiar, bool pbSoportaEñe = false, bool pbCambiaÑporN = false)
          {
               string lsTextoLimpio = null;
               int liContador = 0;
               string lsCaracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 abcdefghijklmnopqrstuvwxyz";
               lsTextoLimpio = "";
               psTextoALimpiar = psTextoALimpiar.Trim();
               for (liContador = 1; liContador <= psTextoALimpiar.Length; liContador++)
               {
                    if ((lsCaracteresPermitidos + (pbCambiaÑporN || pbSoportaEñe ? "Ññ" : "")).IndexOf(psTextoALimpiar.Substring(liContador, 1), 1) > 0)
                    {
                         lsTextoLimpio = lsTextoLimpio + psTextoALimpiar.Substring(liContador, 1);
                    }
               }
               if (pbCambiaÑporN)
               {
                    lsTextoLimpio = lsTextoLimpio.Replace("Ñ", "N");
                    lsTextoLimpio = lsTextoLimpio.Replace("ñ", "n");
               }
               lsTextoLimpio = lsTextoLimpio.Trim();
               return lsTextoLimpio;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psTextoALimpiar"></param>
          /// <param name="psCaracteresPermitidos"></param>
          /// <param name="pbSoportaEñe"></param>
          /// <param name="pbCambiaÑporN"></param>
          /// <returns></returns>
          public static string LimpiaTextoPersonalizado(
               string psTextoALimpiar, string psCaracteresPermitidos, bool pbSoportaEñe = false, bool pbCambiaÑporN = false)
          {
               string lsTextoLimpio = null;
               int liContador;
               lsTextoLimpio = "";
               psTextoALimpiar = psTextoALimpiar.Trim();
               for (liContador = 0; liContador < psTextoALimpiar.Length - 1; liContador++)
               {
                    //int i = (psCaracteresPermitidos + (pbCambiaÑporN || pbSoportaEñe ? "Ññ" : "")).IndexOf(psTextoALimpiar.Substring(liContador, 1),1);
                    if ((psCaracteresPermitidos + (pbCambiaÑporN || pbSoportaEñe ? "Ññ" : "")).IndexOf(psTextoALimpiar.Substring(liContador, 1), 1) > 0)
                         lsTextoLimpio = lsTextoLimpio + psTextoALimpiar.Substring(liContador, 1);
               }
               if (pbCambiaÑporN)
               {
                    lsTextoLimpio = lsTextoLimpio.Replace("Ñ", "N");
                    lsTextoLimpio = lsTextoLimpio.Replace("ñ", "n");
               }
               lsTextoLimpio = lsTextoLimpio.Trim();
               return lsTextoLimpio;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psTexto"></param>
          /// <returns></returns>
          public static string QuitaAcento(this string psTexto)
          {
               psTexto = psTexto.Replace("Á", "A");
               psTexto = psTexto.Replace("É", "E");
               psTexto = psTexto.Replace("Í", "I");
               psTexto = psTexto.Replace("Ó", "O");
               psTexto = psTexto.Replace("Ú", "U");
               return psTexto;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psRFC"></param>
          /// <returns></returns>
          public static bool ValidaRFC(this string psRFC)
          {
               int liIndex = 0;
               int liresult;
               switch (psRFC.Trim().Length)
               {
                    case 13:
                         break;
                    case 12:
                         psRFC = "X" + psRFC;
                         //Se completan las 13 posiciones
                         break;
                    default:
                         return false;
               }

               for (liIndex = 0; liIndex <= 13; liIndex++)
               {
                    switch (liIndex)
                    {
                         case 1:
                         case 2:
                         case 3:
                         case 4:
                              if (Int32.TryParse(psRFC.Substring(liIndex, 1), out liresult))
                                   return false;
                              break;
                         case 5:
                         case 6:
                         case 7:
                         case 8:
                         case 10:
                              if (!Int32.TryParse(psRFC.Substring(liIndex, 1), out liresult))
                                   return false;
                              break;
                    }
               }

               return true;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psCP"></param>
          /// <returns></returns>
          public static bool ValidaCP(this string psCP)
          {
               int liresult;
               return Int32.TryParse(psCP, out liresult) && psCP.Trim().Length == 5;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psRFC"></param>
          /// <returns></returns>
          public static int IdentificaTipoRFC(this string psRFC)
          {
               if (psRFC.Equals("XAXX010101000"))
                    return 1;
               else if (psRFC.Equals("XEXX010101000"))
                    return 2;
               else if (ValidaRFC(psRFC))
                    return 0;
               else
                    return 1;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="psRFC"></param>
          /// <returns></returns>
          public static bool ValidaCaracteresRFC(this string psRFC)
          {
               int liInteger = 0;
               string lsABC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789&";
               psRFC = psRFC.ToUpper();
               for (liInteger = 1; liInteger <= psRFC.Length; liInteger++)
                    if (lsABC.IndexOf(psRFC.Substring(liInteger, 1)) == 0)
                         return false;
               return true;
          }

          /// <summary>
          /// UUID son las siglas en inglés del Identificador Universalmente Único. Esto es un código identificador 
          /// estándar que se utiliza en el proceso de construcción de software. Su intención es la de habilitar 
          /// un código de información único sin que tenga que haber una coordinación central para su generación, 
          /// esto quiere decir que cualquiera debe poder generar un UUID con cierta información desde cualquier 
          /// lugar sin tener que estar conectados a un dispositivo central que asigne los códigos. El archivo 
          /// resultante se podrá mezclar en bases de datos sin tener conflictos de duplicados.
          /// 
          /// El UUID del Sistema de Administración Tributaria es el equivalente al Folio que antes se otorgaba 
          /// a los contribuyentes, la diferencia es que ahora no se tiene que solicitar al SAT, sino que los 
          /// Proveedores Autorizados de Certificación (PAC) lo asignan al momento de realizar la validación del 
          /// documento.
          /// 
          /// El código se forma por una cadena compuesta por 32 dígitos hexadecimales (del 0 al 9 y las 
          /// primeras 6 letras del alfabeto) mostrados en 5 grupos separados por guiones de la forma 
          /// (8 – 4 – 4 – 4 – 12).
          /// 
          /// 560a8451-a29c-41d4-a716-544676554400
          /// 
          /// La cantidad de caracteres hace muy complicada la digitalización y búsqueda por medio de este 
          /// código. La mejor manera de administrar los documentos para control interno es utilizando 
          /// series y folios opcionales que se administran de manera libre y permiten un mejor control 
          /// interno y administrativo.
          /// </summary>
          /// <param name="psUUid"></param>
          /// <returns></returns>
          /// <remarks></remarks>
          public static bool ValidaUUid(this string psUUid)
          {
               string[] lasElementosUuid = null;
               lasElementosUuid = psUUid.Split('-');
               if (lasElementosUuid.Length != 5)
                    return false;
               if (lasElementosUuid[0].Length != 8)
                    return false;
               if (lasElementosUuid[1].Length != 4)
                    return false;
               if (lasElementosUuid[2].Length != 4)
                    return false;
               if (lasElementosUuid[3].Length != 4)
                    return false;
               if (lasElementosUuid[4].Length != 12)
                    return false;
               return true;
          }

          /// <summary>
          /// Determina si la primera letra de una cadena de 
          /// caracteres es una letra mayúscula (capitalizada):
          /// </summary>
          /// <param name="cadena"></param>
          /// <returns></returns>
          public static bool EstaCapitalizada(this String cadena)
          {
               if (String.IsNullOrEmpty(cadena))
                    return false;
               return Char.IsUpper(cadena[0]);
          }

          /// <summary>
          /// Pluraliza una palabra:
          /// </summary>
          /// <param name="cadena"></param>
          /// <returns></returns>
          public static string Pluralizar(this String cadena)
          {
               if (String.IsNullOrEmpty(cadena))
                    return "";
               return String.Format("{0}s", cadena);
          }

          public static string GeneraNombreAleatorio(string psNombre, string psExtension)
          {
               StringBuilder lsNombreArchivo;
               Random loGeneraNumeroRandom;
               lsNombreArchivo = new StringBuilder();
               loGeneraNumeroRandom = new Random();
               lsNombreArchivo.AppendFormat("tmp{0}{1}{2}{3}", psNombre,
                    (DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Second.ToString()),
                    loGeneraNumeroRandom.Next(1000, 9999),
                    psExtension);
               return lsNombreArchivo.ToString();
          }

          public static string RegresaValorEntreCorchetes(this string psCadena)
          {
               if (psCadena.Contains("[") && psCadena.Contains("]"))
                    return psCadena.Substring(psCadena.IndexOf("[") + 1, psCadena.IndexOf("]") - 1).Trim();
               return "";
          }

          public static string RegresaValorDespuesDeCorchetes(this string psCadena)
          {
               if (psCadena.Contains("]"))
                    return psCadena.Substring(psCadena.IndexOf("]") + 1).Trim();
               return "";
          }
     }
}