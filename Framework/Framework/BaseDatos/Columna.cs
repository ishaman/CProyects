using System.Collections.Generic;

namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// 
     /// </summary>
     public class Columna
     {           
          /// <summary>
          ///Dictionary that hold all the dynamic property values 
          /// </summary>
          private Dictionary<string, object> ValorColumna = new Dictionary<string, object>();          
          /// <summary>
          /// the property call to get any dynamic property in our Dictionary, or "" if none found. rvt [DataMember]          
          /// </summary>
          /// <param name="name"></param>
          /// <returns></returns>
          public object this[string name]
          {
               get
               {
                    if (ValorColumna.ContainsKey(name.ToLower()))                    
                         return ValorColumna[name.ToLower()];                               
                    return "";
               }
               set
               {
                    ValorColumna[name.ToLower()] = value;
               }
          }
     }
}
