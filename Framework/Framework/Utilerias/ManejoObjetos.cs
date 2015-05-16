using System;
namespace Solucionic.Framework.Utilerias
{

     public static class ManejoObjetos 
     {  
          
          public static object Iif( bool poCondicion, object poIzquierda, object poDerecha )
          {
               return poCondicion ? poIzquierda : poDerecha;
          }
          public static T Iif <T>( bool poCondicion, T poIzquierda, T poDerecha )
          {
               return poCondicion ? poIzquierda : poDerecha;
          }   
                 
          
          /// <summary>
        /// The equivalent of Microsoft.VisualBasic.Command() in C# is Environment.CommandLine. 
        /// However, this property includes also the full path of the executable, that isn't returned by Microsoft.VisualBasic.Command.
        /// If you just want to obtain in C# the same value that you have with Microsoft.VisualBasic.Command method
        /// </summary>
        /// <param name="psNombreParametro"></param>
        /// <returns></returns>
          public static string RegresaParametroCommandLine( string psNombreParametro )
          {
             int liPosicionInicio = 0;
             int liPosicionFinal = 0;
             bool lbPosicionInicioEncontrada = false;
             try
             {
                  psNombreParametro = psNombreParametro.ToLower();
                  liPosicionInicio = Environment.GetCommandLineArgs().ToString().ToLower().IndexOf(psNombreParametro);
                  if (liPosicionInicio == -1)
                       return "";
                  for (liPosicionFinal = liPosicionInicio + 1; liPosicionFinal <= Environment.GetCommandLineArgs().Length + 1; liPosicionFinal++)
                  {
                       if (!lbPosicionInicioEncontrada)
                            if (Environment.GetCommandLineArgs().ToString().ToLower().Substring(liPosicionFinal, 1) == "=")
                            {
                                 liPosicionInicio = liPosicionFinal + 1;
                                 lbPosicionInicioEncontrada = true;
                            }
                            else
                                 if (Environment.GetCommandLineArgs().ToString().ToLower().Substring(liPosicionFinal, 1) == " ")
                                      return Environment.GetCommandLineArgs().ToString().Substring(liPosicionInicio, liPosicionFinal - liPosicionInicio);
                  }
                  if (!lbPosicionInicioEncontrada)
                       return "";
                  return Environment.GetCommandLineArgs().ToString().Substring(liPosicionInicio, liPosicionFinal - liPosicionInicio);
             }
             catch
             {
                  return "";
             }
        }

          public static bool ContieneParametroLineadeComandos(string psNombreParametro)
          {
               string[] lasLineaComandos;
               lasLineaComandos = Environment.GetCommandLineArgs();
               foreach (string lsComando in lasLineaComandos)               
                    if (lsComando.ToUpper().Contains(psNombreParametro.ToUpper()))
                         return true;               
               return false;
          }
          public static string RegresaParametroLineadeComandos(string psNombreParametro)
          {
               string[] lasLineaComandos;
               string lsResultado;
               lasLineaComandos = Environment.GetCommandLineArgs();
               lsResultado = "";
               foreach (string lsComando in lasLineaComandos)               
                    if (lsComando.ToUpper().Contains(psNombreParametro.ToUpper()))                   
                         lsResultado = lsComando.Substring(lsComando.IndexOf("=")+1);                                   
               return lsResultado;
          }
          
     }
}

