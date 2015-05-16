
namespace Solucionic.Framework.Bitacora
{
     public class MensajeReporte
     {
          public string Comentario { get; set; }

          public string MensajeId { get; set; }
          
          public override bool Equals( object poObj )
          {
               if (poObj == null) 
                    return false;
               MensajeReporte loObjAsPart = poObj as MensajeReporte;
               if (loObjAsPart == null) 
                    return false;
               else 
                    return Equals(loObjAsPart);
          }

          public int SortByNameAscending( string psMensaje1, string psMensaje2 )
          {
               return psMensaje1.CompareTo(psMensaje2);
          }

          // Default comparer for Part type.
          public int CompareTo( MensajeReporte poComparePart )
          {
               // A null value means that this object is greater.
               if (poComparePart == null)
                    return 1;
               else
                    return this.MensajeId.CompareTo(poComparePart.MensajeId);
          }
          public override int GetHashCode()
          {
               return this.MensajeId.GetHashCode();
          }

          public bool Equals( MensajeReporte poOther )
          {
               if (poOther == null) 
                    return false;
               return (this.MensajeId.Equals(poOther.MensajeId));
          }

     }
}
