using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOPSOhv
{
     public class DatosGenerales
     {
          public int TamanioPoblacion { get; set; }
          public int TamanioArchivo { get; set; }
          public bool ArchivoAcotado { get; set; }
          public FuncionObjetivo TipoFuncion { get; set; }
          public int NumertoGeneraciones { get; set; }
          public double PorcentajeMutacion { get; set; }
     }
}
