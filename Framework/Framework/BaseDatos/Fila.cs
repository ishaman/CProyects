
namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// 
     /// </summary>
     public class Fila
     {
          // property is a class that will create dynamic properties at runtime
          /// <summary>
          /// 
          /// </summary>
          private Columna _property = new Columna();
          //rvt [DataMember]
          /// <summary>
          /// 
          /// </summary>
          public Columna ValorFila
          {
               get { return _property; }
               set { _property = value; }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="psValor"></param>
          /// <returns></returns>
          public object this[string psValor]
          {
               get { return _property[psValor.ToLower()]; }
               set { _property[psValor.ToLower()] = value; }
          }
     }
}
