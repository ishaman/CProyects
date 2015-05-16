using System.Text;
using System.IO;
namespace Solucionic.Framework.Utilerias
{
     /// <summary>
     /// One problem with StringWriter is that by default it doesn't let you set the encoding which it advertises - 
     /// so you can end up with an XML document advertising its encoding as UTF-16, which means you need to encode it as UTF-16 if you write it to a file. 
     /// A small class to help with that though
     /// </summary>
     public sealed class StringWriterWithEncoding : StringWriter
     {
          private readonly Encoding encoding;

          public StringWriterWithEncoding( Encoding encoding )
          {
               this.encoding = encoding;
          }

          public override Encoding Encoding
          {
               get { return encoding; }
          }
     }

}