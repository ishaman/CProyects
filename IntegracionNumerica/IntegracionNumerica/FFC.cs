using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Microsoft.VisualBasic;

using System.IO;
using System.Diagnostics;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
 
namespace IntegracionNumerica
{
     public partial class FFC : Form
     {
          public Configuraciones _oConfiguraciones;
          public FileInfo[] _oListaArchivoSistemaBandas;
          public FFC()
          {
               InitializeComponent();
          }

          private void btnSalir_Click( object sender, EventArgs e )
          {
               Close();
          }

          private void FFC_Load( object sender, EventArgs e )
          {
               try
               {
                    _oConfiguraciones = new Configuraciones(Application.StartupPath);
                    CargaComboAlgoritmosIntegracion();
                    CargaComboOtrosAlgoritmosIntegracion();
                    CargaSistemaBandas();
               }
               catch (Exception ex)
               {
                    ManejaExcepcion(ex);
               }
          }
          private void CargaSistemaBandas()
          {                              
               DirectoryInfo loCarpetadeArchivos;               
               if (!Directory.Exists(_oConfiguraciones.RutaLayout))
                    throw new ApplicationException("El directorio" + _oConfiguraciones.RutaLayout + " de formatos no existe");
               loCarpetadeArchivos = new DirectoryInfo(_oConfiguraciones.RutaLayout);
               _oListaArchivoSistemaBandas = loCarpetadeArchivos.GetFiles("*.SB", SearchOption.AllDirectories);
               cmbSistemaBanda.Items.Clear();
               cmbSistemaBanda.Items.Add("Ninguno"); 
               foreach (FileInfo loInformacionArchivo in _oListaArchivoSistemaBandas )
               {
                    cmbSistemaBanda.Items.Add(loInformacionArchivo.Name.Replace(".SB","").ToUpper()); 
               }
               cmbSistemaBanda.SelectedIndex = 0;
          }

          private void CargaComboAlgoritmosIntegracion()
          {
               cmbAlgoritmosRNAS.Items.Add("Ninguno");
               cmbAlgoritmosRNAS.Items.Add("Back Propagation");
               cmbAlgoritmosRNAS.Items.Add("Factor Momentum");
               cmbAlgoritmosRNAS.Items.Add("Gradientes Conjugados");
               cmbAlgoritmosRNAS.Items.Add("Gradientes Conjugados Residual");
               cmbAlgoritmosRNAS.Items.Add("Newton");
               cmbAlgoritmosRNAS.Items.Add("Newton Truncado");
               cmbAlgoritmosRNAS.SelectedIndex = 0;
          }
          private void CargaComboOtrosAlgoritmosIntegracion()
          {
               cmbOtrosMetodos.Items.Add("Ninguno");
               cmbOtrosMetodos.Items.Add("Simpson");
               cmbOtrosMetodos.SelectedIndex = 0;
          }
          public void ManejaExcepcion( Exception loExcepcion )//
          {
               if (loExcepcion.GetType() == Type.GetType("System.ApplicationException"))
               {
                    MessageBox.Show(loExcepcion.Message);
               }
               else
               {
                    MessageBox.Show("Ocurrio lo siguiente:" + loExcepcion.ToString());
                    //Grabar un registro de eventos ocurrido en un archivo de texto
               }

          }

          private void cmbAlgoritmosRNAS_MouseClick( object sender, MouseEventArgs e )
          {
               if (cmbOtrosMetodos.SelectedIndex > 0)
                    cmbOtrosMetodos.SelectedIndex = 0;
          }

          private void cmbOtrosMetodos_MouseDoubleClick( object sender, MouseEventArgs e )
          {
               if (cmbAlgoritmosRNAS.SelectedIndex > 0)
                    cmbAlgoritmosRNAS.SelectedIndex = 0;
          }

          private void btnGraficar_Click( object sender, EventArgs e )
          {
               bool lbImprime;
               MessageBoxButtons buttons = MessageBoxButtons.YesNo;
               DialogResult result;
               lbImprime = false;
               result = MessageBox.Show("¿Deseas graficar todos los estados?", "Mensaje del Sistema", buttons);
               if (result == System.Windows.Forms.DialogResult.Yes)
                    lbImprime = true;               
               //<@> preguntar si se quieren imprimir todas las graficas segun los intervalos de las funcion de onda o solo por los valores minimos               
               try
               {
                    if (!ValidaCampos())
                         return;
                    GraficaFuncionOnda(lbImprime);                    
               }
               catch (Exception Ex)
               {
                    ManejaExcepcion(Ex);
               }
          }

          private void GraficaFuncionOnda(bool pbImprime)
          {
               FuncionOnda loFFC;
               GnuPlot loGrafica;
               Process[] loProcesoGnuplot;
               double ldowe_1;
               double ldowe_2;
               double ldowe_xe_1;
               double ldowe_xe_2;
               double ldore_1;
               double re_2;
               double ldomiu;
               double ldoA;
               double ldoB;
               int liN;
               //double ffc = 0.0;
               int liv1_1;
               int liv1_2;
               int liv2_1;
               int liv2_2;
               int lii;
               int lij;
              
               ldowe_1 = Convert.ToDouble(txtwe_1.Text);
               ldowe_2 = Convert.ToDouble(txtwe_2.Text); ;
               ldowe_xe_1 = Convert.ToDouble(txtwe_xe_1.Text); ;
               ldowe_xe_2 = Convert.ToDouble(txtwe_xe_2.Text); ;
               ldore_1 = Convert.ToDouble(txtre_1.Text); ;
               re_2 = Convert.ToDouble(txtre_2.Text); ;
               ldomiu = Convert.ToDouble(txtMiu.Text); ;

               liv1_1 = Convert.ToInt32(txtV1_min.Text);
               liv1_2 = Convert.ToInt32(txtV1_max.Text);
               liv2_1 = Convert.ToInt32(txtV2_min.Text);
               liv2_2 = Convert.ToInt32(txtV2_max.Text);

               ldoA = Convert.ToDouble(txtA.Text);
               ldoB = Convert.ToDouble(txtB.Text);
               liN = Convert.ToInt32(txtNumNodos.Text);

               loFFC = new FuncionOnda(ldowe_1, ldowe_2, ldowe_xe_1, ldowe_xe_2, ldore_1, re_2, ldomiu, ldoA, ldoB, liN);

               loProcesoGnuplot = Process.GetProcessesByName("gnuplot");
               if (loProcesoGnuplot.Count() > 0) //determinamos si ya existe una ventana abierta
               {
                    //Si existe detenemos el proceso asociado (inmediatamente)
                    loProcesoGnuplot[0].Kill();
               }
               loGrafica = new GnuPlot(_oConfiguraciones.RutaComponentes);
               loGrafica.HoldOn();
               if (pbImprime)
               {
                    for (lii = liv2_1; lii <= liv2_2; lii++)
                         for (lij = liv1_1; lij <= liv1_2; lij++)
                         {
                              loFFC.genera_fn_integracion(lii, lij);
                              loGrafica.Plot(loFFC.X, loFFC.Y, "with linespoints " + "title '" + txtNombreSisBanda.Text + "(" + Convert.ToString(lii) + "," + Convert.ToString(lij) + ")" + "'");
                         }
               }
               else
               {
                    loFFC.genera_fn_integracion(liv1_1, liv2_2);

                    loGrafica.Plot(loFFC.X, loFFC.Y, "with linespoints " + "title '" + txtNombreSisBanda.Text + "(" + Convert.ToString(liv1_1) + "," + Convert.ToString(liv2_1) + ")" + "'");
               }
               
               
          }

          private bool ValidaCampos()
          {
               double ldoValorDouble;
               int liValorEntero;
               if (txtNombreSisBanda.Text == "")
               {
                    MessageBox.Show("El nombre de sistema de nadas no puede ir vacio");
                    return false;
               }
               if (txtV1_max.Text == "")
               {
                    MessageBox.Show("El intervalo de numero cuantico v1_max no puede ir vacio");
                    return false;
               }
               if (txtV1_min.Text == "")
               {
                    MessageBox.Show("El intervalo de numero cuantico v1_min no puede ir vacio");
                    return false;
               }
               if (txtV2_max.Text == "")
               {
                    MessageBox.Show("El intervalo de numero cuantico v2_max no puede ir vacio");
                    return false;
               }
               if (txtV2_min.Text == "")
               {
                    MessageBox.Show("El intervalo de numero cuantico v1_min no puede ir vacio");
                    return false;
               }
               if (txtwe_1.Text == "")
               {
                    MessageBox.Show("El intervalo de numero cuantico we_1 no puede ir vacio");
                    return false;
               }
               if (txtwe_2.Text == "")
               {
                    MessageBox.Show("La constante we_2 no puede ir vacio");
                    return false;
               }
               if(txtwe_xe_1.Text == "")
               {
                    MessageBox.Show("La constante xe_1 no puede ir vacio");
                    return false;
               }
               if(txtwe_xe_2.Text == "")
               {
                    MessageBox.Show("La constante xe_2 no puede ir vacio");
                    return false;
               }
               if(txtre_1.Text == "")
               {
                    MessageBox.Show("La constante re_1 no puede ir vacio");
                    return false;
               }
               if(txtre_2.Text == "")
               {
                    MessageBox.Show("La constante re_2 no puede ir vacio");
                    return false;
               }
               if(txtMiu.Text =="")
               {
                    MessageBox.Show("La constante miu no puede ir vacio");
                    return false;
               }
               if(txtA.Text == "")
               {
                    MessageBox.Show("El intervalo de integracion A no puede ir vacio");
                    return false;
               }
               if(txtB.Text == "")
               {
                    MessageBox.Show("El intervalo de integracion B no puede ir vacio");
                    return false;
               }
               if(txtNumNodos.Text == "")
               {
                    MessageBox.Show("El número de nodos no puede ir vacio");
                    return false;
               }
               if (!Int32.TryParse(txtV1_max.Text, out liValorEntero))
               {
                    MessageBox.Show("El intervalo de numero cuantico v1_max tiene que ser entero");
                    return false;
               }
               if (!Int32.TryParse(txtV1_min.Text, out liValorEntero))
               {
                    MessageBox.Show("El intervalo de numero cuantico v1_main tiene que ser enterio");
                    return false;
               }
               if (!Int32.TryParse(txtV2_max.Text, out liValorEntero))
               {
                    MessageBox.Show("El intervalo de numero cuantico v2_max tiene que ser entero");
                    return false;
               }
               if (!Int32.TryParse(txtV2_min.Text, out liValorEntero))
               {
                    MessageBox.Show("El intervalo de numero cuantico v2_min tiene que ser entero");
                    return false;
               }
               if (!double.TryParse(txtwe_1.Text, out ldoValorDouble))
               {
                    MessageBox.Show("La constante we_1 tiene que ser real");
                    return false;
               }
               if (!double.TryParse(txtwe_2.Text, out ldoValorDouble))
               {
                    MessageBox.Show("La constante we_2 tiene que ser real");
                    return false;
               }
               if (!double.TryParse(txtwe_xe_1.Text, out ldoValorDouble))
               {
                    MessageBox.Show("La constante xe_1 tiene que ser real");
                    return false;
               }
               if (!double.TryParse(txtwe_xe_2.Text, out ldoValorDouble))
               {
                    MessageBox.Show("La constante xe_2 tiene que ser real");
                    return false;
               }
               if (!double.TryParse(txtre_1.Text , out ldoValorDouble))
               {
                    MessageBox.Show("La constante re_1 tiene que ser real");
                    return false;
               }
               if (!double.TryParse(txtre_2.Text , out ldoValorDouble))
               {
                    MessageBox.Show("La constante re_2 tiene que ser real");
                    return false;
               }
               if (!double.TryParse(txtMiu.Text , out ldoValorDouble))
               {
                    MessageBox.Show("La constante miu tiene que ser real");
                    return false;
               }
               if (!double.TryParse(txtA.Text , out ldoValorDouble))
               {
                    MessageBox.Show("El intervalo de integracion A tiene que ser real");
                    return false;
               }
               if (!double.TryParse(txtB.Text, out ldoValorDouble))
               {
                    MessageBox.Show("El intervalo de integracion B tiene que ser real");
                    return false;
               }
               if (!Int32.TryParse(txtNumNodos.Text, out liValorEntero))
               {
                    MessageBox.Show("El numero de nodos tiene que ser entero");
                    return false;
               }

               if(Convert.ToInt32(txtNumNodos.Text ) < 5)
               {
                    MessageBox.Show("El numero de nodos tiene que ser mayor o igual a 5");
                    return false;
               }

               if (Convert.ToDouble(txtV1_max.Text) < Convert.ToDouble(txtV1_min.Text))
               {
                    MessageBox.Show("El intervalo V1_max no puede ser menos que V1_min");
                    return false;
               }
               if (Convert.ToDouble(txtV2_max.Text) < Convert.ToDouble(txtV2_min.Text))
               {
                    MessageBox.Show("El intervalo V2_max no puede ser menos que V2_min");
                    return false;
               }
               return true;
          }


          private void btnIniciar_Click( object sender, EventArgs e )
          {
               try
               {
                    HabilitaControles(false);
                    if (!ValidaCampos())
                         return;
                    if (cmbAlgoritmosRNAS.SelectedIndex == 0 && cmbOtrosMetodos.SelectedIndex == 0)
                         throw new ApplicationException("Falta seleccionar un metodo de integración");
                    
                    ImprimeReporteResultados(GeneraResultados());
               }
               catch (Exception ex)
               {
                    ManejaExcepcion(ex);
               }
               finally
               {
                    HabilitaControles(true);
               }
               
          }

          private void HabilitaControles(bool pbHabilita)
          {
               txtNombreSisBanda.Enabled = pbHabilita;
               txtV1_max.Enabled = pbHabilita;
               txtV1_min.Enabled = pbHabilita;
               txtV2_max.Enabled = pbHabilita;
               txtV2_min.Enabled = pbHabilita;
               txtwe_1.Enabled = pbHabilita;
               txtwe_2.Enabled = pbHabilita;
               txtwe_xe_1.Enabled = pbHabilita;
               txtwe_xe_2.Enabled = pbHabilita;
               txtre_1.Enabled = pbHabilita;
               txtre_2.Enabled = pbHabilita;
               txtMiu.Enabled = pbHabilita;
               txtA.Enabled = pbHabilita;
               txtB.Enabled = pbHabilita;
               txtNumNodos.Enabled = pbHabilita;
               cmbAlgoritmosRNAS.Enabled = pbHabilita;
               cmbOtrosMetodos.Enabled = pbHabilita;
               cmbSistemaBanda.Enabled = pbHabilita;
               btnGraficar.Enabled = pbHabilita;
               btnGuardar.Enabled = pbHabilita;
               btnIniciar.Enabled = pbHabilita;
               btnSalir.Enabled = pbHabilita;
          }

          private string[,] GeneraResultados()
          {
               FuncionOnda loFFC;
               double ldowe_1;
               double ldowe_2;
               double ldowe_xe_1;
               double ldowe_xe_2;
               double ldore_1;
               double re_2;
               double ldomiu;
               double ldoA;
               double ldoB;
               string[,] Lsa_Resultados;
               int liN;
               //double ffc = 0.0;
               int liv1_1;
               int liv1_2;
               int liv2_1;
               int liv2_2;
               int lii;
               int lij;
               int liv2;
               int liv1;
                ldowe_1 = Convert.ToDouble(txtwe_1.Text);
               ldowe_2 = Convert.ToDouble(txtwe_2.Text); ;
               ldowe_xe_1 = Convert.ToDouble(txtwe_xe_1.Text); ;
               ldowe_xe_2 = Convert.ToDouble(txtwe_xe_2.Text); ;
               ldore_1 = Convert.ToDouble(txtre_1.Text); ;
               re_2 = Convert.ToDouble(txtre_2.Text); ;
               ldomiu = Convert.ToDouble(txtMiu.Text); ;

               liv1_1 = Convert.ToInt32(txtV1_min.Text);
               liv1_2 = Convert.ToInt32(txtV1_max.Text);
               liv2_1 = Convert.ToInt32(txtV2_min.Text);
               liv2_2 = Convert.ToInt32(txtV2_max.Text);

               ldoA = Convert.ToDouble(txtA.Text);
               ldoB = Convert.ToDouble(txtB.Text);
               liN = Convert.ToInt32(txtNumNodos.Text);

               loFFC = new FuncionOnda(ldowe_1, ldowe_2, ldowe_xe_1, ldowe_xe_2, ldore_1, re_2, ldomiu, ldoA, ldoB, liN);          
               Lsa_Resultados = new string[liv2_2-liv2_1+1,liv1_2-liv1_1+1];               
               loFFC.variables_iniciales();
               for (lii = 0, liv2 = liv2_1; liv2 <= liv2_2; liv2++, lii++)
               {
                    for (lij = 0, liv1 = liv1_1; liv1 <= liv1_2; liv1++, lij++)
                    {
                         loFFC.genera_fn_integracion(liv1, liv2);
                         if (cmbAlgoritmosRNAS.SelectedIndex == 1)
                             loFFC.FFC_RNABP();
                         if (cmbAlgoritmosRNAS.SelectedIndex == 2)
                             loFFC.FFC_RNAFM();
                         if (cmbAlgoritmosRNAS.SelectedIndex == 3)
                             loFFC.FFC_RNAGC();
                         if (cmbAlgoritmosRNAS.SelectedIndex == 4)
                             loFFC.FFC_RNAGCR();
                         if (cmbAlgoritmosRNAS.SelectedIndex == 5)
                             loFFC.FFC_RNAN();
                         if (cmbAlgoritmosRNAS.SelectedIndex == 1)
                             loFFC.FFC_RNANT();
                         if (cmbOtrosMetodos.SelectedIndex == 1)
                              loFFC.Simpson();
                         Lsa_Resultados[lii, lij] = string.Format("{0:0.###E-000}", Convert.ToString(loFFC.Integral)); 
                    }                    
               }
               return Lsa_Resultados;
          }
          private void ImprimeReporteResultados(string[,] pasResultados)
          {
               //Nombre del archivo tempral
               string lsfileName = Path.Combine(_oConfiguraciones.RutaArchivos, txtNombreSisBanda.Text + Guid.NewGuid().ToString() + ".pdf");
               //Documento donde guardaremos los resultados
               Document loDocumento = new Document(PageSize.LEGAL, 10, 10, 10, 10);
               loDocumento.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());
               //Apertura del documento
               PdfWriter.GetInstance(loDocumento, new FileStream(lsfileName, FileMode.Create));
               loDocumento.Open();
               //Agregando informacion del documento
               NombreSistemaBandas(loDocumento);
               IntervalosNumeroCuanticos(loDocumento);
               ConstantesSistemaBandas(loDocumento);                                            
               GenerarTablaResultados(loDocumento, pasResultados);
               IntervalosIntegracion(loDocumento);
               loDocumento.Close();
               Process prc = new System.Diagnostics.Process();
               prc.StartInfo.FileName = lsfileName;
               prc.Start();
          }
         
          private void NombreSistemaBandas( Document poDocumento )
          {
               Paragraph loParrafo = new Paragraph();
               loParrafo.Font = FontFactory.GetFont("Arial", 6);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.BOLD);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.UNDERLINE);
               //nombre del sistema de bandas
               loParrafo.Add("Nombre del Sistema de Bandas:" + txtNombreSisBanda.Text);
               poDocumento.Add(loParrafo);
               //GENERO Y AGREGO SALTO DE LINEA
               poDocumento.Add(new Paragraph(" "));
          }

          private void IntervalosIntegracion( Document poDocumento )
          {
               Paragraph loParrafo = new Paragraph();
               loParrafo.Font = FontFactory.GetFont("Arial", 6);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.BOLD);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.UNDERLINE);
               //nombre del sistema de bandas
               loParrafo.Add("Intervalos de integración (" + txtA.Text + ","+ txtB.Text + "). Numero de Nodos:" + txtNumNodos.Text);
               poDocumento.Add(loParrafo);
               //GENERO Y AGREGO SALTO DE LINEA
               poDocumento.Add(new Paragraph(" "));
          }

          private void IntervalosNumeroCuanticos( Document poDocumento )
          {
               Paragraph loParrafo = new Paragraph();
               PdfPCell locelda1;
               PdfPCell locelda2;
               PdfPTable loTabla;

               loParrafo.Alignment = Element.ALIGN_CENTER;
               loParrafo.Font = FontFactory.GetFont("Arial", 6);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.BOLD);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.UNDERLINE);
               loParrafo.Add("Intervalos de los Número Cuanticos");
               poDocumento.Add(loParrafo);
               //GENERO Y AGREGO SALTO DE LINEA
               poDocumento.Add(new Paragraph(" "));
               //Agregamos la tabla con los intervalos numericos
               loTabla = new PdfPTable(2);
               loTabla.SetWidthPercentage(new float[] { 50, 50 }, PageSize.LEGAL);
               //loTabla.WidthPercentage = 100;
               //Encabezado 1               
               locelda1 = new PdfPCell(new Paragraph("v1 min", FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph("v1 max", FontFactory.GetFont("Arial", 6)));

               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);
               //¿Le damos un poco de formato?
               foreach (PdfPCell locelda in loTabla.Rows[0].GetCells())
               {                    
                    locelda.BackgroundColor = BaseColor.LIGHT_GRAY;
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;                    
               }
               locelda1 = new PdfPCell(new Paragraph(txtV1_min.Text, FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph(txtV1_max.Text, FontFactory.GetFont("Arial", 6)));

               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);


               foreach (PdfPCell locelda in loTabla.Rows[1].GetCells())
               {                    
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }

               locelda1 = new PdfPCell(new Paragraph("v2 min", FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph("v2 max", FontFactory.GetFont("Arial", 6)));

               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);

               //¿Le damos un poco de formato?
               foreach (PdfPCell locelda in loTabla.Rows[2].GetCells())
               {
                    locelda.BackgroundColor = BaseColor.LIGHT_GRAY;
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }
               locelda1 = new PdfPCell(new Paragraph(txtV2_min.Text, FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph(txtV2_max.Text, FontFactory.GetFont("Arial", 6)));


               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);

               foreach (PdfPCell locelda in loTabla.Rows[3].GetCells())
               {                    
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }
               poDocumento.Add(loTabla);
               poDocumento.Add(new Paragraph(" "));

          }

          private void ConstantesSistemaBandas( Document poDocumento )
          {
               Paragraph loParrafo = new Paragraph();
               PdfPCell locelda1;
               PdfPCell locelda2;
               PdfPTable loTabla;

               loParrafo.Alignment = Element.ALIGN_CENTER;
               loParrafo.Font = FontFactory.GetFont("Arial", 6);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.BOLD);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.UNDERLINE);
               loParrafo.Add("Constantes del Sistema de Bandas");
               poDocumento.Add(loParrafo);
               //GENERO Y AGREGO SALTO DE LINEA
               poDocumento.Add(new Paragraph(" "));
               //Agregamos la tabla con los intervalos numericos
               loTabla = new PdfPTable(2);
               loTabla.SetWidthPercentage(new float[] { 50, 50 }, PageSize.LEGAL);
               //loTabla.WidthPercentage = 100;
               //Encabezado 1               
               locelda1 = new PdfPCell(new Paragraph("we_1", FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph("we_2", FontFactory.GetFont("Arial", 6)));

               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);
               //¿Le damos un poco de formato?
               foreach (PdfPCell locelda in loTabla.Rows[0].GetCells())
               {
                    locelda.BackgroundColor = BaseColor.LIGHT_GRAY;
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }
               locelda1 = new PdfPCell(new Paragraph(txtwe_1.Text, FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph(txtwe_2.Text, FontFactory.GetFont("Arial", 6)));

               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);

               foreach (PdfPCell locelda in loTabla.Rows[1].GetCells())
               {
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }

               locelda1 = new PdfPCell(new Paragraph("we_xe_1", FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph("we_xe_2", FontFactory.GetFont("Arial", 6)));

               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);

               //¿Le damos un poco de formato?
               foreach (PdfPCell locelda in loTabla.Rows[2].GetCells())
               {
                    locelda.BackgroundColor = BaseColor.LIGHT_GRAY;
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }
               locelda1 = new PdfPCell(new Paragraph(txtwe_xe_1.Text, FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph(txtwe_xe_2.Text, FontFactory.GetFont("Arial", 6)));


               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);

               foreach (PdfPCell locelda in loTabla.Rows[3].GetCells())
               {
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }


               locelda1 = new PdfPCell(new Paragraph("re_1", FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph("re_2", FontFactory.GetFont("Arial", 6)));

               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);

               //¿Le damos un poco de formato?
               foreach (PdfPCell locelda in loTabla.Rows[4].GetCells())
               {
                    locelda.BackgroundColor = BaseColor.LIGHT_GRAY;
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }
               locelda1 = new PdfPCell(new Paragraph(txtre_1.Text, FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph(txtre_2.Text, FontFactory.GetFont("Arial", 6)));


               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);

               foreach (PdfPCell locelda in loTabla.Rows[5].GetCells())
               {
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }

               locelda1 = new PdfPCell(new Paragraph("miu", FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph("", FontFactory.GetFont("Arial", 6)));

               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);

               //¿Le damos un poco de formato?
               foreach (PdfPCell locelda in loTabla.Rows[6].GetCells())
               {
                    locelda.BackgroundColor = BaseColor.LIGHT_GRAY;
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }
               locelda1 = new PdfPCell(new Paragraph(txtMiu.Text, FontFactory.GetFont("Arial", 6)));
               locelda2 = new PdfPCell(new Paragraph("", FontFactory.GetFont("Arial", 6)));

               loTabla.AddCell(locelda1);
               loTabla.AddCell(locelda2);

               foreach (PdfPCell locelda in loTabla.Rows[7].GetCells())
               {
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
               }

               poDocumento.Add(loTabla);
               poDocumento.Add(new Paragraph(" "));
          }

          //Función que genera el documento Pdf
          public void GenerarTablaResultados( Document poDocumento, string[,] pasResultados )
          {
               //se crea un objeto PdfTable con el numero de columnas del 
               //dataGridView
               PdfPCell locelda;
               PdfPTable loTabla = new PdfPTable(pasResultados.GetLength(1) + 1);
               Paragraph loParrafo = new Paragraph();
               string lsMetodoIntegracion;

               loParrafo.Alignment = Element.ALIGN_CENTER;
               loParrafo.Font = FontFactory.GetFont("Arial", 6);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.BOLD);
               loParrafo.Font.SetStyle(iTextSharp.text.Font.UNDERLINE);

               lsMetodoIntegracion = "Ninguno";
               if (cmbAlgoritmosRNAS.SelectedIndex > 0)
                    lsMetodoIntegracion = Convert.ToString(cmbAlgoritmosRNAS.SelectedItem);
               else if (cmbOtrosMetodos.SelectedIndex > 0 )
                    lsMetodoIntegracion = Convert.ToString(cmbOtrosMetodos.SelectedItem);
               loParrafo.Add("Metodo de Integración: " + lsMetodoIntegracion);

               poDocumento.Add(loParrafo);
               //GENERO Y AGREGO SALTO DE LINEA
               poDocumento.Add(new Paragraph(" "));

               //asignamos algunas propiedades para el diseño del pdf
               loTabla.DefaultCell.Padding = 3;
               //float[] headerwidths = GetTamañoColumnas(dataGridView1);
               //datatable.SetWidths(2);
               loTabla.WidthPercentage = 100;
               loTabla.DefaultCell.BorderWidth = 2;
               loTabla.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
               //SE GENERA EL ENCABEZADO DE LA TABLA EN EL PDF
               locelda = new PdfPCell(new Paragraph("v2/v1", FontFactory.GetFont("Arial", 6)));
               loTabla.AddCell(locelda);
               for (int i = 1; i < pasResultados.GetLength(1) + 1; i++)
               {
                    locelda = new PdfPCell(new Paragraph(Convert.ToString (Convert.ToInt32 (txtV1_min.Text) + i-1) , FontFactory.GetFont("Arial", 6)));
                    loTabla.AddCell(locelda);
               }
               loTabla.HeaderRows = 1;
              loTabla.DefaultCell.BorderWidth = 1;
               //SE GENERA EL CUERPO DEL PDF
              for (int i = 0; i < pasResultados.GetLength(0); i++)
               {
                    locelda = new PdfPCell(new Paragraph(Convert.ToString(Convert.ToInt32(txtV2_min.Text) + i), FontFactory.GetFont("Arial", 6)));
                    locelda.BackgroundColor = BaseColor.LIGHT_GRAY;
                    locelda.HorizontalAlignment = 1;
                    locelda.Padding = 3;
                    loTabla.AddCell(locelda);
                    for (int j = 0; j < pasResultados.GetLength(1); j++)
                    {
                         locelda = new PdfPCell(new Paragraph(pasResultados[i, j], FontFactory.GetFont("Arial", 6)));
                         locelda.HorizontalAlignment = 1;
                         locelda.Padding = 3;
                         loTabla.AddCell(locelda);
                    }
                    loTabla.CompleteRow();
               }
              foreach (PdfPCell locelda1 in loTabla.Rows[0].GetCells())
              {
                   locelda1.BackgroundColor = BaseColor.LIGHT_GRAY;
                   locelda1.HorizontalAlignment = 1;
                   locelda1.Padding = 3;
              }
               //SE AGREGAR LA PDFPTABLE AL DOCUMENTO
               poDocumento.Add(loTabla);
          }
          
          private void cmbSistemaBanda_SelectedIndexChanged( object sender, EventArgs e )
          {
               ArchivoIni _oArchivoLectura;
               if (cmbSistemaBanda.SelectedIndex == 0)
               {
                    txtNombreSisBanda.Text = "";
                    txtV1_max.Text = "";
                    txtV1_min.Text = "";
                    txtV2_max.Text = "";
                    txtV2_min.Text = "";
                    txtwe_1.Text = "";
                    txtwe_2.Text = "";
                    txtwe_xe_1.Text = "";
                    txtwe_xe_2.Text = "";
                    txtre_1.Text = "";
                    txtre_2.Text = "";
                    txtMiu.Text = "";
                    txtA.Text = "";
                    txtB.Text = "";
                    txtNumNodos.Text = "";
               }
               else
               {
                    _oArchivoLectura = new ArchivoIni(_oListaArchivoSistemaBandas[cmbSistemaBanda.SelectedIndex-1].FullName);
                    txtNombreSisBanda.Text = _oArchivoLectura.LeeArchivoIni("NombreSistemaBandas","nombre");
                    txtV1_max.Text = _oArchivoLectura.LeeArchivoIni("Intervalos", "v1max");
                    txtV1_min.Text = _oArchivoLectura.LeeArchivoIni("Intervalos", "v1min");
                    txtV2_max.Text = _oArchivoLectura.LeeArchivoIni("Intervalos", "v2max");
                    txtV2_min.Text = _oArchivoLectura.LeeArchivoIni("Intervalos", "v2min");
                    txtwe_1.Text = _oArchivoLectura.LeeArchivoIni("Constantes", "we_1");
                    txtwe_2.Text = _oArchivoLectura.LeeArchivoIni("Constantes", "we_2");
                    txtwe_xe_1.Text = _oArchivoLectura.LeeArchivoIni("Constantes", "we_xe_1");
                    txtwe_xe_2.Text = _oArchivoLectura.LeeArchivoIni("Constantes", "we_xe_2");
                    txtre_1.Text = _oArchivoLectura.LeeArchivoIni("Constantes", "re_1");
                    txtre_2.Text = _oArchivoLectura.LeeArchivoIni("Constantes", "re_2");
                    txtMiu.Text = _oArchivoLectura.LeeArchivoIni("Constantes", "miu");
                    txtA.Text = _oArchivoLectura.LeeArchivoIni("Integracion", "A");
                    txtB.Text = _oArchivoLectura.LeeArchivoIni("Integracion", "B");
                    txtNumNodos.Text = _oArchivoLectura.LeeArchivoIni("Integracion", "N");
               }
              
          }

          private void btnGuardar_Click( object sender, EventArgs e )
          {              
               try
               {
                    if (!ValidaCampos())
                         return;
                    GuardaSistemaBandas();
               }
               catch (Exception ex)
               {
                    ManejaExcepcion(ex);
               }
               
          }
          private void GuardaSistemaBandas()
          {
               ArchivoIni _oArchivoLectura;
               _oArchivoLectura = new ArchivoIni(_oConfiguraciones.RutaLayout, txtNombreSisBanda.Text + ".SB");
               _oArchivoLectura.EscribeArchivoIni("NombreSistemaBandas", "nombre", txtNombreSisBanda.Text);
               _oArchivoLectura.EscribeArchivoIni("Intervalos", "v1max", txtV1_max.Text);
               _oArchivoLectura.EscribeArchivoIni("Intervalos", "v1min", txtV1_min.Text);
               _oArchivoLectura.EscribeArchivoIni("Intervalos", "v2max", txtV2_max.Text);
               _oArchivoLectura.EscribeArchivoIni("Intervalos", "v2min", txtV2_min.Text);
               _oArchivoLectura.EscribeArchivoIni("Constantes", "we_1", txtwe_1.Text);
               _oArchivoLectura.EscribeArchivoIni("Constantes", "we_2", txtwe_2.Text);
               _oArchivoLectura.EscribeArchivoIni("Constantes", "we_xe_1", txtwe_xe_1.Text);
               _oArchivoLectura.EscribeArchivoIni("Constantes", "we_xe_2", txtwe_xe_2.Text);
               _oArchivoLectura.EscribeArchivoIni("Constantes", "re_1", txtre_1.Text);
               _oArchivoLectura.EscribeArchivoIni("Constantes", "re_2", txtre_2.Text);
               _oArchivoLectura.EscribeArchivoIni("Constantes", "miu", txtMiu.Text);
               _oArchivoLectura.EscribeArchivoIni("Integracion", "A", txtA.Text);
               _oArchivoLectura.EscribeArchivoIni("Integracion", "B", txtB.Text);
               _oArchivoLectura.EscribeArchivoIni("Integracion", "N", txtNumNodos.Text);
               CargaSistemaBandas();
          }
     }
}
