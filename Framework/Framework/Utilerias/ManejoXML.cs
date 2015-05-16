using System.IO;
using System.Xml;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Solucionic.Framework.Utilerias
{
     public static class ManejoXML
     {
          /// <summary>
          /// Esta funcion permite converti una string que contiene un xml. Al guardar un xml en una cadena se guarda con caracteres raros. NO permite
          /// hacer la conercion directamente. Por lo que hay que configurar la cadena para poder utilizarla de otra manera
          /// y omitir los caracteres especiales. Usando un StringReader
          /// </summary>
          /// <param name="psCadenaXML">Cadena con el xml</param>
          /// <returns>XMLReader de la cadena xml</returns>
          public static XmlReader ConvertirStringXmlReader( string psCadenaXML )
          {
               var loConfiguracion = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true, IgnoreComments = true };
               var loxmlReader = XmlReader.Create(new StringReader(psCadenaXML), loConfiguracion);
               loxmlReader.Read();
               return loxmlReader;
          }

          public static string ConvierteBinarioXMLAStringXML( byte[] pbyArchivo )
          {
               using (var loMemoryStream = new System.IO.MemoryStream(pbyArchivo))
               {
                    using (var lostreamReader = new System.IO.StreamReader(loMemoryStream))
                    {
                         return XDocument.Load(lostreamReader).ToString();
                    }
               }
          }
          
          public static string BuscaElementoenArchivoXML( byte[] poComprobanteTimbrado, string psElemento, string psAtributo, bool pbElementoAtributo )
          {
               //Cargamos el documento xml generado en bytes en memoria para evitar el acceso a disco
               using (Stream loXmlStream = new MemoryStream(poComprobanteTimbrado))
               {
                    using (XmlTextReader loLector = new XmlTextReader(loXmlStream))
                    {
                         if (loLector.Read())
                         {
                              //Buscando el inicio de los datos "ns:WebServiceError"
                              while (( loLector.Name != psElemento.Trim() ))
                              {
                                   if (!loLector.Read())
                                   {
                                        //No hubo error                                   
                                        return "0";
                                   }
                              }
                              if (pbElementoAtributo)
                              {
                                   return loLector.ReadElementString(psElemento);
                              }
                              else
                              {
                                   return loLector.GetAttribute(psAtributo);
                              }
                         }
                         else
                         {
                              //El documento XML está vacio                         
                              return "0";
                         }
                    }
               }
          }
          
          public static void SetAttributeValue( this System.Xml.Linq.XNode loNodo, string lsAttrName, string lsValue )
          {
               ( loNodo as System.Xml.Linq.XElement ).Attributes(System.Xml.Linq.XName.Get(lsAttrName)).First().Value = lsValue;
          }

          public static string GetAttributeValue( this System.Xml.Linq.XNode loNodo, string lsAttrName )
          {
               return ( loNodo as System.Xml.Linq.XElement ).Attributes(System.Xml.Linq.XName.Get(lsAttrName)).First().Value;
          }

          public static string GetString( this byte[] pbyBinario )
          {
               return System.Text.Encoding.UTF8.GetString(pbyBinario);
          }

          public static byte[] ToSHA1( this byte[] pbyArchivo )
          {
               var lvCrypto = System.Security.Cryptography.SHA1.Create();
               return lvCrypto.ComputeHash(pbyArchivo);
          }
          public static byte[] GetBytes( this string loCadena )
          {
               return System.Text.Encoding.UTF8.GetBytes(loCadena);
          }

          public static string ToBase64( this byte[] poBinario )
          {
               return Convert.ToBase64String(poBinario);
          }

          public static byte[] ToXml( this object poFactura )
          {
               using (var lvMemoria = new System.IO.MemoryStream())
               {
                    var loSerializa = new System.Xml.Serialization.XmlSerializer(poFactura.GetType());
                    loSerializa.Serialize(lvMemoria, poFactura);
                    return lvMemoria.ToArray();
               }
          }

          public static object FromXml( this byte[] pbyFactura, Type poTipo )
          {
               using (var loMemoria = new System.IO.MemoryStream(pbyFactura))
               {
                    var loSerializa = new System.Xml.Serialization.XmlSerializer(pbyFactura.GetType());
                    return loSerializa.Deserialize(loMemoria);
               }
          }

          public static byte[] Transformar( this byte[] pbyObjeto, string lsXslt )
          {
               //Cargar el XML
               using (System.IO.MemoryStream loMemoriaIn = new System.IO.MemoryStream(pbyObjeto))
               {
                    System.Xml.XPath.XPathDocument loMyXPathDoc = new System.Xml.XPath.XPathDocument(loMemoriaIn);
                    //Cargando el XSLT
                    System.Xml.Xsl.XslCompiledTransform loMyXslTrans = new System.Xml.Xsl.XslCompiledTransform();
                    loMyXslTrans.Load(lsXslt);
                    using (System.IO.StringWriter loStr = new System.IO.StringWriter())
                    {
                         using (var loWri = new System.Xml.XmlTextWriter(loStr))
                         {
                              //Aplicando transformacion
                              loMyXslTrans.Transform(loMyXPathDoc, null, loWri);
                         }
                         return loStr.ToString().GetBytes();
                    }
               }
          }
     }
}
