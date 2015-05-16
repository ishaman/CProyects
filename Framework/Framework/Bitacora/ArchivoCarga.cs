using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solucionic.Framework.Bitacora
{
     [Serializable]
     public class ArchivoCarga
     {
          public string Extension { get; set; }
          public bool EsSalida { get; set; }
          public byte[] Buffer { get; set; }
     }
}
