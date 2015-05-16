using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class Configuraciones
{
    public string RutaArchivos {get; set;}
    public string RutaComponentes {get;set;}
    public string RutaLayout {get; set;}
    #region Propiedades    
        
    #endregion

    #region Constructores
    public Configuraciones(string psRutaArchivoIni)
    {
        string lsRutaArchivoIni;
        string lsNombreArchivoIni;
        ArchivoIni loArchivoConfiguracion;
        lsRutaArchivoIni = psRutaArchivoIni;
        lsNombreArchivoIni = "Configuracion.ini";
        //lsRutaTotal = Path.Combine(Cs_RutaArchivoIni, Cs_NombreArchivoIni);
        loArchivoConfiguracion = new ArchivoIni(lsRutaArchivoIni, lsNombreArchivoIni);
        RutaArchivos = loArchivoConfiguracion.LeeArchivoIni("RutasIniciales", "RutaArchivoSalida");
        RutaComponentes = loArchivoConfiguracion.LeeArchivoIni("RutasIniciales", "RutaComponentes");
        RutaLayout = loArchivoConfiguracion.LeeArchivoIni("RutasIniciales", "RutaLayout");
    }
    #endregion

    #region Metodos
    #endregion
}

