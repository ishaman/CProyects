using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.IO;

public class ArchivoIni
{
     /// <summary>
     /// Para usarla en las funciones GetSection(s)
     /// </summary>
     private string _sBuffer;
     private string _sRutaTotal;
     public string NombreArchivo { get; set; }
     public string RutaArchivo { get; set; }


     #region Propiedades
     #endregion
     #region Constructor
     public ArchivoIni(string psRutaArchivo, string psNombreArchivo)
     {
          NombreArchivo = psNombreArchivo;
          RutaArchivo = psRutaArchivo;
          _sRutaTotal = Path.Combine(psRutaArchivo, psNombreArchivo);
     }
     public ArchivoIni(string psRutaTotal)
     {
          _sRutaTotal = psRutaTotal;
     }
     #endregion

     #region Metodos

     public void EscribeArchivoIni(string psSeccion, string psClave, string psValor)
     {
          IniWrite(_sRutaTotal, psSeccion, psClave, psValor);
     }
     public string LeeArchivoIni(string psSeccion, string psClave)
     {
          return IniGet(_sRutaTotal, psSeccion, psClave, "");
     }
     //--- Declaraciones para leer ficheros INI ---
     // Leer todas las secciones de un fichero INI, esto seguramente no funciona en Win95
     // Esta función no estaba en las declaraciones del API que se incluye con el VB
     [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
     private static extern int GetPrivateProfileSectionNames(
          string psReturnBuffer,  // address of return buffer
          int pinSize,             // size of return buffer
          string pslpFileName         // address of initialization filename
     );
     // Leer una sección completa
     [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
     private static extern int GetPrivateProfileSection(
          string lpAppName,         // address of section name
          string lpReturnedstring,  // address of return buffer
          int nSize,             // size of return buffer
          string lpFileName         // address of initialization filename
     );
     // Leer una clave de un fichero INI
     [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
     private static extern int GetPrivateProfilestring(
          string lpAppName,        // points to section name
          string lpKeyName,        // points to key name
          string lpDefault,        // points to default string
          string lpReturnedstring, // points to destination buffer
          int nSize,            // size of destination buffer
          string lpFileName        // points to initialization filename
     );
     [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
     private static extern int GetPrivateProfilestring(
          string lpAppName,        // points to section name
          int lpKeyName,        // points to key name
          string lpDefault,        // points to default string
          string lpReturnedstring, // points to destination buffer
          int nSize,            // size of destination buffer
          string lpFileName        // points to initialization filename
          );
     // Escribir una clave de un fichero INI (también para borrar claves y secciones)
     [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
     private static extern int WritePrivateProfilestring(
          string lpAppName,  // pointer to section name
          string lpKeyName,  // pointer to key name
          string lpstring,   // pointer to string to add
          string lpFileName  // pointer to initialization filename
     );
     [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
     private static extern int WritePrivateProfilestring(
          string lpAppName,  // pointer to section name
          string lpKeyName,  // pointer to key name
          int lpstring,   // pointer to string to add
          string lpFileName  // pointer to initialization filename
          );
     [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
     private static extern int WritePrivateProfilestring(
          string lpAppName,  // pointer to section name
          int lpKeyName,  // pointer to key name
          int lpstring,   // pointer to string to add
          string lpFileName  // pointer to initialization filename
          );
     //
     private void IniDeleteKey(string sIniFile, string sSection, string sKey)
     {
          //--------------------------------------------------------------------------
          // Borrar una clave o entrada de un fichero INI                  (16/Feb/99)
          // Si no se indica sKey, se borrará la sección indicada en sSection
          // En otro caso, se supone que es la entrada (clave) lo que se quiere borrar
          //
          // Para borrar una sección se debería usar IniDeleteSection
          //
          if (sKey == "")
          {
               // Borrar una sección
               WritePrivateProfilestring(sSection, 0, 0, sIniFile);
          }
          else
          {
               // Borrar una entrada
               WritePrivateProfilestring(sSection, sKey, 0, sIniFile);
          }
     }
     private void IniDeleteSection(string sIniFile, string sSection)
     {
          //--------------------------------------------------------------------------
          // Borrar una sección de un fichero INI                          (04/Abr/01)
          // Borrar una sección
          WritePrivateProfilestring(sSection, 0, 0, sIniFile);
     }
     private string IniGet(string sFileName, string sSection, string sKeyName, string sDefault)
     {
          //--------------------------------------------------------------------------
          // Devuelve el valor de una clave de un fichero INI
          // Los parámetros son:
          //   sFileName   El fichero INI
          //   sSection    La sección de la que se quiere leer
          //   sKeyName    Clave
          //   sDefault    Valor opcional que devolverá si no se encuentra la clave
          //--------------------------------------------------------------------------
          int ret;
          string sRetVal;
          //
          sRetVal = new string(' ', 255);
          //
          ret = GetPrivateProfilestring(sSection, sKeyName, sDefault, sRetVal, sRetVal.Length, sFileName);
          if (ret == 0)
          {
               return sDefault;
          }
          else
          {
               return sRetVal.Substring(0, ret);
          }
     }
     private void IniWrite(string sFileName, string sSection, string sKeyName, string sValue)
     {
          //--------------------------------------------------------------------------
          // Guarda los datos de configuración
          // Los parámetros son los mismos que en LeerIni
          // Siendo sValue el valor a guardar
          //
          WritePrivateProfilestring(sSection, sKeyName, sValue, sFileName);
     }
     private string[] IniGetSection(string sFileName, string sSection)
     {
          //--------------------------------------------------------------------------
          // Lee una sección entera de un fichero INI                      (27/Feb/99)
          // Adaptada para devolver un array de string                     (04/Abr/01)
          //
          // Esta función devolverá un array de índice cero
          // con las claves y valores de la sección
          //
          // Parámetros de entrada:
          //   sFileName   Nombre del fichero INI
          //   sSection    Nombre de la sección a leer
          // Devuelve:
          //   Un array con el nombre de la clave y el valor
          //   Para leer los datos:
          //       For i = 0 To UBound(elArray) -1 Step 2
          //           sClave = elArray(i)
          //           sValor = elArray(i+1)
          //       Next
          //
          string[] aSeccion;
          int n;
          //
          aSeccion = new string[0];
          //
          // El tamaño máximo para Windows 95
          _sBuffer = new string('\0', 32767);
          //
          n = GetPrivateProfileSection(sSection, _sBuffer, _sBuffer.Length, sFileName);
          //
          if (n > 0)
          {
               // Cortar la cadena al número de caracteres devueltos
               // menos los dos últimos que indican el  final de la cadena
               _sBuffer = _sBuffer.Substring(0, n - 2).TrimEnd();
               //
               // Cada una de las entradas estará separada por un Chr$(0)
               // y cada valor estará en la forma: clave = valor
               aSeccion = _sBuffer.Split(new char[] { '\0', '=' });
          }
          // Devolver el array
          return aSeccion;
     }
     private string[] IniGetSections(string sFileName)
     {
          //--------------------------------------------------------------------------
          // Devuelve todas las secciones de un fichero INI                (27/Feb/99)
          // Adaptada para devolver un array de string                     (04/Abr/01)
          //
          // Esta función devolverá un array con todas las secciones del fichero
          //
          // Parámetros de entrada:
          //   sFileName   Nombre del fichero INI
          // Devuelve:
          //   Un array con todos los nombres de las secciones
          //   La primera sección estará en el elemento 1,
          //   por tanto, si el array contiene cero elementos es que no hay secciones
          //
          int n;
          string[] aSections;
          //
          aSections = new string[0];
          //
          // El tamaño máximo para Windows 95
          _sBuffer = new string('\0', 32767);
          //
          // Esta función del API no está definida en el fichero TXT
          n = GetPrivateProfileSectionNames(_sBuffer, _sBuffer.Length, sFileName);
          //
          if (n > 0)
          {
               // Cortar la cadena al número de caracteres devueltos
               // menos los dos últimos que indican el  final de la cadena
               _sBuffer = _sBuffer.Substring(0, n - 2).TrimEnd();
               aSections = _sBuffer.Split('\0');
          }
          // Devolver el array
          return aSections;
     }
     #endregion
}

