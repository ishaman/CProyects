using System.Text;
using System.IO;
namespace Solucionic.Framework.Utilerias
{
    /// <summary>
    ///  only need UTF-8 
    /// </summary>
    public sealed class Utf8StringWriter : StringWriter
    {

        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}
