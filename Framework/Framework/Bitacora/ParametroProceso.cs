using System;
using System.Xml.Serialization;
using System.Xml;

namespace Solucionic.Framework.Bitacora
{
     [Serializable]
     [XmlRootAttribute("Parametro", Namespace = "", IsNullable = false)]
     public class ParametroProceso
     {
          [XmlAttribute("Nombre")]
          public string Nombre { get; set; }
          [XmlElementAttribute("Valor")]
          public string Valor { get; set; }

          public ParametroProceso() { } //Debe de existir para la serializacion
          public ParametroProceso( string psNombre, string psValor )
          {
               Nombre = psNombre;
               Valor = Convert.ToString(psValor);
          }
     }
}
