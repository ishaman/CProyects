using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Collections.Generic;
using Ionic.Zip;
namespace Solucionic.Framework.Utilerias
{
     public static class ManejoArchivos
     {
          /// <summary>
          /// ABRE EL ARCHIVO EXPORTADO CON SU APLICACION ASOCIADA (COMO CON LA API SHELLEXECUTE)
          /// </summary>
          /// <param name="psTemporal"></param>
          /// <param name="psEsperaProceso"></param>
          public static void AbreArchivo( string psTemporal, bool psEsperaProceso = true )
          {
               Process loProceso = new Process();
               ProcessStartInfo loInformacionProceso = null;
               if (File.Exists(psTemporal) == true)
               {
                    if (psEsperaProceso == true)
                    {
                         loInformacionProceso = new ProcessStartInfo(psTemporal);
                         var _with1 = loProceso;
                         _with1.StartInfo = loInformacionProceso;
                         _with1.Start();
                         _with1.EnableRaisingEvents = true;
                         _with1.WaitForExit();
                         _with1.Close();
                    }
                    else
                         System.Diagnostics.Process.Start(psTemporal);
               }
          }
          public static string RutaTemporalWindows()
          {
               string lsRuta = null;
               lsRuta = System.IO.Path.GetFullPath(System.IO.Path.GetTempPath());
               lsRuta = ( ManejoCadenas.StringRight(lsRuta, 1) != "\\" ? lsRuta + "\\" : lsRuta );
               return lsRuta;
          }

          /// <summary>
          /// Funcion que verfica si tenemos permisos de acceso totales y que no tenga atributos de solo lectura
          /// </summary>
          /// <param name="psRutaArchivo"> Ruta de la carpeta o directorio</param>
          /// <remarks></remarks>        
          public static void ValidaSiPuedeGenerarArchivoEnRuta( string psRutaArchivo )
          {
               System.IO.DirectoryInfo loInformaAcceso = null;
               System.Security.AccessControl.DirectorySecurity loReglasdeAcceso = null;
               System.Security.AccessControl.AuthorizationRuleCollection loColecciondeReglas = null;
               FileSystemAccessRule loReglas = null;
               bool lbPermitido = false;
               bool lbDenegado = false;
               //Validamos si el directorio existe
               if (!Directory.Exists(psRutaArchivo))
                    throw new ApplicationException("No existe el directorio: " + psRutaArchivo);
               //Nota: Si no existe la carpeta se puede crear y darle los permisos de seguridad de control total
               //Verificamos los permisos del directorio             
               loInformaAcceso = new DirectoryInfo(psRutaArchivo);
               if (loInformaAcceso == null)
                    throw new ApplicationException("No tienes permisos en la carpeta: " + psRutaArchivo);
               //Obtenemos las reglas de acceso sobre la carpeta
               loReglasdeAcceso = loInformaAcceso.GetAccessControl();
               loColecciondeReglas = loReglasdeAcceso.GetAccessRules(true, false, typeof(System.Security.Principal.NTAccount));
               if (loColecciondeReglas == null)
                    throw new ApplicationException("No tienes permisos en la carpeta: " + psRutaArchivo);
               //Para obter el nombre de usuario actual: System.Security.Principal.WindowsIdentity.GetCurrent()
               //Verificamos las reglas de la carpeta
               foreach (FileSystemAccessRule loReglas_loopVariable in loColecciondeReglas)
               {
                    loReglas = loReglas_loopVariable;
                    //Preguntamos si la carpeta tiene permisos de escritura
                    if (( FileSystemRights.Write & loReglas.FileSystemRights ) != FileSystemRights.Write)
                         continue;
                    //Se pueden agregar mas reglas de acceso: lectura: escritura, etc ...                  
                    //Verificamos en tipo de control de acceso sobre la carpeta.
                    if (loReglas.AccessControlType == AccessControlType.Allow)
                         lbPermitido = true;
                    else if (loReglas.AccessControlType == AccessControlType.Deny)
                         lbDenegado = true;
               }
               if (!( lbPermitido & !lbDenegado ))
                    throw new ApplicationException("No tienes permisos en la carpeta: " + psRutaArchivo);
               //Por ultimo verificamos los atributos de la carpeta, si es de solo lectura.
               if (( loInformaAcceso.Attributes & System.IO.FileAttributes.ReadOnly ) > 0)
                    throw new ApplicationException("El directorio " + psRutaArchivo + " es de solo lectura");
          }

          public static byte[] ComprimeArchivos(List<Archivo> poArchivos)
          {
              MemoryStream loMemoria;
               using (Ionic.Zip.ZipFile loZip = new Ionic.Zip.ZipFile())
               {
                    foreach (Archivo loArchivo in poArchivos)
                    {
                         if(!Object.Equals(loArchivo.Buffer,null))
                              loZip.AddEntry(loArchivo.Nombre, loArchivo.Buffer);
                    }
                    loMemoria = new MemoryStream();
                    loZip.Save(loMemoria); //Hacer un stream de retorno :)
                    loMemoria.Seek(0, SeekOrigin.Begin);
                    loMemoria.Flush();
               }
               return loMemoria.ToArray();
          }
          public static byte[] ComprimeArchivo( byte[] pabyArchivo, string psNombre )
          {
               MemoryStream loMemoria;
               using (Ionic.Zip.ZipFile loZip = new Ionic.Zip.ZipFile())
               {

                    if (!Object.Equals(pabyArchivo, null))
                         loZip.AddEntry(psNombre, pabyArchivo);                    
                    loMemoria = new MemoryStream();
                    loZip.Save(loMemoria); //Hacer un stream de retorno :)
                    loMemoria.Seek(0, SeekOrigin.Begin);
                    loMemoria.Flush();
               }
               return loMemoria.ToArray();
          }
     }
}
