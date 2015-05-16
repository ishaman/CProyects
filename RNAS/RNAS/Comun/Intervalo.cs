using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNAS
{
     public class Intervalo
     {
          public double A { get; set; }
          public double B { get; set; }

          public Intervalo(double pdoValorA, double pdoValorB)
          {
               A = pdoValorA;
               B = pdoValorB;
          }
     }
}
