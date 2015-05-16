using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// 
     /// </summary>
     public class ParametrosSQL
     {
          /// <summary>
          /// 
          /// </summary>
          public string Parametro {get; set;}
          /// <summary>
          /// 
          /// </summary>
          public object Valor {get; set;}
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psParametro"></param>
          /// <param name="poValor"></param>
          public ParametrosSQL( string psParametro, object poValor )
          {
               Parametro = psParametro;
               Valor = poValor;
          }
     }
}
