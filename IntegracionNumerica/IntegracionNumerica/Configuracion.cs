using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace IntegracionNumerica
{
     public partial class Configuracion : Form
     {
          public Configuracion()
          {
               InitializeComponent();
          }

          private void Configuracion_Load( object sender, EventArgs e )
          {               
               try
               {
                    CargaArchivoConfiguracion();                                 
               }
               catch (Exception ex)
               {
                    ManejaExcepcion(ex);
               }               
          }


          public void ManejaExcepcion(Exception loExcepcion)//
          {
               if (loExcepcion.GetType() == Type.GetType("ApplicationException"))
               {
                    MessageBox.Show(loExcepcion.Message);
               }
               else
               {
                    MessageBox.Show("Ocurrio lo siguiente:" + loExcepcion.ToString());
                    //Grabar un registro de eventos ocurrido en un archivo de texto
               }
        
          }
        

          private void CargaArchivoConfiguracion()
          {
               ArchivoIni loArchivoConfiguracion;
               string lsRutaArchivoIni;
               string lsNombreArchivoIni;
               string lsRutaTotal;
               loArchivoConfiguracion = null;
               lsRutaArchivoIni = Application.StartupPath; 
               lsNombreArchivoIni = "Configuracion.ini";
               lsRutaTotal = Path.Combine(lsRutaArchivoIni, lsNombreArchivoIni);
               loArchivoConfiguracion = new ArchivoIni(lsRutaArchivoIni, lsNombreArchivoIni);               
               if (!(File.Exists(lsRutaTotal)))
               {
                    //Si no existe le asiganmos las rutas que trae por default
                    
                    //Creamos las rutas en el directorio raiz
                    if (!(Directory.Exists(Application.StartupPath + @"\ArchivosDeSalida")))
                    {
                         Directory.CreateDirectory(Application.StartupPath + @"\ArchivosDeSalida");
                    }
                    if (!(Directory.Exists(Application.StartupPath + @"\Complementos")))
                    {
                         Directory.CreateDirectory(Application.StartupPath + @"\Complementos");
                    }
                    if (!(Directory.Exists(Application.StartupPath + @"\Layouts")))
                    {
                         Directory.CreateDirectory(Application.StartupPath + @"\Layouts");
                    }
                    //el metodo propio crea el archivo
                    loArchivoConfiguracion.EscribeArchivoIni("RutasIniciales", "RutaArchivoSalida", Application.StartupPath + @"\ArchivosDeSalida");
                    loArchivoConfiguracion.EscribeArchivoIni("RutasIniciales", "RutaComponentes", Application.StartupPath + @"\Complementos");
                    loArchivoConfiguracion.EscribeArchivoIni("RutasIniciales", "RutaLayout", Application.StartupPath + @"\Layouts");                    
               }
               txtArchivoDeSalida.Text = loArchivoConfiguracion.LeeArchivoIni("RutasIniciales", "RutaArchivoSalida");
               txtComponentes.Text = loArchivoConfiguracion.LeeArchivoIni("RutasIniciales", "RutaComponentes"); 
               txtLayouts.Text = loArchivoConfiguracion.LeeArchivoIni("RutasIniciales", "RutaLayout"); 
          }

          private void btnCancelar_Click( object sender, EventArgs e )
          {
               Close();
          }

          private void btnGuardar_Click( object sender, EventArgs e )
          {
               string lsRutaArchivoIni;
               string lsNombreArchivoIni;
               string lsRutaTotal;
               ArchivoIni loArchivoConfiguracion;               
               lsRutaArchivoIni = Application.StartupPath;
               lsNombreArchivoIni = "Configuracion.ini";
               lsRutaTotal = Path.Combine(lsRutaArchivoIni, lsNombreArchivoIni);
               loArchivoConfiguracion = new ArchivoIni(lsRutaArchivoIni, lsNombreArchivoIni);

               if (!ValidaCampos())
                    return;

               loArchivoConfiguracion.EscribeArchivoIni("RutasIniciales", "RutaArchivoSalida", @txtArchivoDeSalida.Text);
               loArchivoConfiguracion.EscribeArchivoIni("RutasIniciales", "RutaComponentes", @txtComponentes.Text);
               loArchivoConfiguracion.EscribeArchivoIni("RutasIniciales", "RutaLayout", @txtLayouts.Text);

               Close();
          }

          private bool ValidaCampos()
          {
               if (txtArchivoDeSalida.Text == "")
               {
                    MessageBox.Show("La ruta de archivo de salida no puede estar vacia");
                    return false;
               }
               if (txtComponentes.Text == "")
               {
                    MessageBox.Show("La ruta de archivo de salida no puede estar vacia");
                    return false;
               }
               if (txtLayouts.Text == "")
               {
                    MessageBox.Show("La ruta de archivo de salida no puede estar vacia");
                    return false;
               }
               if (!(Directory.Exists(txtArchivoDeSalida.Text)))
               {
                    MessageBox.Show("La ruta de archivo de salida no es una ruta valida");
                    return false;
               }
               if (!(Directory.Exists(txtComponentes.Text)))
               {
                    MessageBox.Show("La ruta de archivo de salida no es una ruta valida");
                    return false;
               }
               if (!(Directory.Exists(txtLayouts.Text)))
               {
                    MessageBox.Show("La ruta de archivo de salida no es una ruta valida");
                    return false;
               }
               return true;
          }

          private void btnArchivoDeSalida_Click( object sender, EventArgs e )
          {
               FolderBrowserDialog loBuscaCarpeta;
               loBuscaCarpeta = new FolderBrowserDialog() ;
               if (loBuscaCarpeta.ShowDialog() == DialogResult.OK)
               {
                    txtArchivoDeSalida.Text = loBuscaCarpeta.SelectedPath;                    
               }
          }

          private void btnComponentes_Click( object sender, EventArgs e )
          {
               FolderBrowserDialog loBuscaCarpeta;
               loBuscaCarpeta = new FolderBrowserDialog();
               if (loBuscaCarpeta.ShowDialog() == DialogResult.OK)
               {
                    txtComponentes.Text = loBuscaCarpeta.SelectedPath;
               }
          }

          private void btnLayout_Click( object sender, EventArgs e )
          {
               FolderBrowserDialog loBuscaCarpeta;
               loBuscaCarpeta = new FolderBrowserDialog();
               if (loBuscaCarpeta.ShowDialog() == DialogResult.OK)
               {
                    txtLayouts.Text = loBuscaCarpeta.SelectedPath;
               }
          }
     }
}
