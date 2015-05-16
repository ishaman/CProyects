using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using RNAS;
namespace IntegracionNumerica
{
     public partial class IntegracionNumerica : Form
     {
          public Configuraciones _oConfiguraciones;
          public IntegracionNumerica()
          {
               InitializeComponent();
          }

          private void btnConfiguracion_Click( object sender, EventArgs e )
          {
               Configuracion loConfiguracion;
               loConfiguracion = new Configuracion();
               loConfiguracion.ShowDialog();
               //hay que volver a instanciar la clase si no existe el archivo
               _oConfiguraciones = new Configuraciones(Application.StartupPath);
          }

          private void IntegracionNumerica_Load( object sender, EventArgs e )
          {
               try
               {
                    _oConfiguraciones = new Configuraciones(Application.StartupPath);
                    CargaComboFuncionesSimples();
                    CargaComboFuncionesOscilantes();
                    CargaComboFuncionesConSingularidad();
                    CargaComboAlgoritmosIntegracion();
                    CargaComboOtrosAlgoritmosIntegracion();
               }
               catch (Exception ex)
               {
                    ManejaExcepcion(ex);
               }

          }
          private void CargaComboFuncionesSimples()
          {
               cmbFuncionesPrueba.Items.Add("Ninguno");
               //Cargare los tipo de archivos de un archivo de texto simpre
               //asociado a una ruta de archivos
               cmbFuncionesPrueba.Items.Add("x^2");
               cmbFuncionesPrueba.Items.Add("x^4");
               cmbFuncionesPrueba.Items.Add("sin(x)");
               cmbFuncionesPrueba.Items.Add("e^x");
               cmbFuncionesPrueba.Items.Add("1/(x+1)");
               cmbFuncionesPrueba.Items.Add("sqrt(1+x^2)");
               cmbFuncionesPrueba.Items.Add("sqrt(1+cos(x)^2)");
               cmbFuncionesPrueba.SelectedIndex = 0;
          }

          private void CargaComboFuncionesOscilantes()
          {
               cmbFunOscilantes.Items.Add("Ninguno");
               //Cargare los tipo de archivos de un archivo de texto simpre
               //asociado a una ruta de archivos
               cmbFunOscilantes.Items.Add("sin(5*x)");
               cmbFunOscilantes.Items.Add("sin(10*x)");
               cmbFunOscilantes.Items.Add("sin(5*cot(x))*sin(2*x)");
               cmbFunOscilantes.Items.Add("sin(10*cot(x))*sin(2*x)");
               cmbFunOscilantes.Items.Add("sin(50*cot(x))*sin(2*x)");
               cmbFunOscilantes.Items.Add("sin(100*cot(x))*sin(2*x)");
               cmbFunOscilantes.Items.Add("((1-cos(x))^5)*sin(5*x)");
               cmbFunOscilantes.Items.Add("((1-cos(x))^10)*sin(10*x)");
               cmbFunOscilantes.SelectedIndex = 0;
          }
          private void CargaComboFuncionesConSingularidad()
          {
               cmbFuncionesconSngularidad.Items.Add("Ninguno");
               //Cargare los tipo de archivos de un archivo de texto simpre
               //asociado a una ruta de archivos
               cmbFuncionesconSngularidad.Items.Add("x*tan(x)");
               cmbFuncionesconSngularidad.Items.Add("sin(3*x)/cos(x)");
               cmbFuncionesconSngularidad.Items.Add("cos(3*x)/cos(x)");
               cmbFuncionesconSngularidad.Items.Add("cos(11*x)/cos(x)");
               cmbFuncionesconSngularidad.Items.Add("cos(21*x)/cos(x)");
               cmbFuncionesconSngularidad.Items.Add("cos(201*x)/cos(x)");
               cmbFuncionesconSngularidad.SelectedIndex = 0;
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



          private void cmdSalir_Click( object sender, EventArgs e )
          {
               Close();
          }

          private void rbtSimpsonAdaptivo_CheckedChanged( object sender, EventArgs e )
          {
               rbtSimpsonAdaptivo.Checked = !rbtSimpsonAdaptivo.Checked;
          }

          private void btnGraficar_Click( object sender, EventArgs e )
          {
               double ldoA;
               double ldoB;
               double ldoh;
               double ldor;
               int liN;
               int lii;
               GnuPlot loGrafica;
               Process[] loProcesoGnuplot;
               double[] ldoX; //= new double[]  { -10, -8.5, -2, 1, 6, 9, 10, 14, 15, 19 };
               double[] ldoY; //= new double[]  { -4, 6.5, -2, 3, -8, -5, 11, 4, -5, 10 };
               Evaluador loEvaluador;

               //X = null;
               //Y = null;
               loProcesoGnuplot = null;
               ldoX = null;
               ldoY = null;
               try
               {
                    //Se puede determinar si el proceso y si no esta se puede volver a invocar
                    //hay que cachar la excepcion si no existe el rpceso o algun error
                    // hay que validar las controles para evitar que mentan otro tipo de valor
                    if (!ValidaCamposLLenos())
                         return;
                    //validar que los intervalos seas correctos (B >= A)

                    //recuperando los datos necesarios para la graficacion
                    ldoA = Convert.ToDouble(txtA.Text);
                    ldoB = Convert.ToDouble(txtB.Text);
                    liN = Convert.ToInt32(txtNumNodos.Text);
                    ldoX = new double[liN + 1];
                    ldoY = new double[liN + 1];

                    loEvaluador = new Evaluador(txtFuncion.Text);
                    //
                    ldor = ldoA;
                    ldoh = (ldoB - ldoA) / liN;
                    for (lii = 0; lii < liN + 1; lii++)
                    {
                         ldoX[lii] = ldor;
                         ldoY[lii] = loEvaluador.F(ldor);
                         ldor = ldor + ldoh;
                    }


                    loProcesoGnuplot = Process.GetProcessesByName("gnuplot");
                    if (loProcesoGnuplot.Count() > 0) //determinamos si ya existe una ventana abierta
                    {
                         //Si existe detenemos el proceso asociado (inmediatamente)
                         loProcesoGnuplot[0].Kill();
                    }
                    loGrafica = new GnuPlot(_oConfiguraciones.RutaComponentes);
                    loGrafica.Plot(ldoX, ldoY, "with linespoints " + "title '" + txtFuncion.Text + "'");
               }
               catch (Exception ex)
               {
                    ManejaExcepcion(ex);
               }
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

          private bool ValidaCamposLLenos()
          {
               double ldoresult;
               int liNumnodos;
               if (txtA.Text.Trim() == "")
               {
                    MessageBox.Show("El campo del intervalo A no puede estar vacio");
                    return false;
               }
               if (txtB.Text.Trim() == "")
               {
                    MessageBox.Show("El campo del intervalo B no puede estar vacio");
                    return false;
               }
               if (txtFuncion.Text.Trim() == "")
               {
                    MessageBox.Show("El campo de la función no puede estar vacio");
                    return false;
               }
               if (txtNumNodos.Text.Trim() == "")
               {
                    MessageBox.Show("El campo de número de nodos no puede estar vacio");
                    return false;
               }
               if (!double.TryParse(txtA.Text.Trim(), out ldoresult))
               {
                    MessageBox.Show("El valor del intervalo A tiene que ser númerico");
                    return false;
               }
               if (!double.TryParse(txtB.Text.Trim(), out ldoresult))
               {
                    MessageBox.Show("El valor del intervalo B tiene que ser númerico");
                    return false;
               }
               if (Convert.ToDouble(txtB.Text) < Convert.ToDouble(txtA.Text))
               {
                    MessageBox.Show("El valor del intervalo B no puede ser menor que el valor del intervalo A");
                    return false;
               }
               if (!Int32.TryParse(txtNumNodos.Text.Trim(), out liNumnodos))
               {
                    MessageBox.Show("El valor del numero de nodos tiene que ser entero");
                    return false;
               }
               if (Convert.ToInt32(txtNumNodos.Text.Trim()) < 5)
               {
                    MessageBox.Show("Error en el Numero de Nodos n >= 5");
                    return false;
               }
               return true;
          }

          private void cmbFuncionesPrueba_MouseClick( object sender, MouseEventArgs e )
          {
               if (cmbFunOscilantes.SelectedIndex > 0)
                    cmbFunOscilantes.SelectedIndex = 0;
               if (cmbFuncionesconSngularidad.SelectedIndex > 0)
                    cmbFuncionesconSngularidad.SelectedIndex = 0;
          }

          private void cmbFunOscilantes_MouseClick( object sender, MouseEventArgs e )
          {
               if (cmbFuncionesPrueba.SelectedIndex > 0)
                    cmbFuncionesPrueba.SelectedIndex = 0;
               if (cmbFuncionesconSngularidad.SelectedIndex > 0)
                    cmbFuncionesconSngularidad.SelectedIndex = 0;
          }

          private void cmbFuncionesconSngularidad_MouseClick( object sender, MouseEventArgs e )
          {
               if (cmbFunOscilantes.SelectedIndex > 0)
                    cmbFunOscilantes.SelectedIndex = 0;
               if (cmbFuncionesPrueba.SelectedIndex > 0)
                    cmbFuncionesPrueba.SelectedIndex = 0;
          }

          private void btnFFC_Click( object sender, EventArgs e )
          {
               FFC loFCC;
               loFCC = new FFC();
               loFCC.ShowDialog();
          }
          
          private void cmbFuncionesPrueba_SelectedIndexChanged( object sender, EventArgs e )
          {
               

               txtToleranciaFuncion.Text = "1e-6";
               txtToleranciaIntegral.Text = "1e-10";
               txtNumEntrenamientos.Text = "2300";

               if (cmbFuncionesPrueba.SelectedIndex == 1)
               {
                    txtFuncion.Text = FuncionesSimples.XCUADRADA;
                    txtNumNodos.Text = FuncionesSimples.NumeroNodos.ToString();
                    txtA.Text = FuncionesSimples.IntervaloValorCero.ToString();
                    txtB.Text = FuncionesSimples.IntervaloValroDos.ToString();
                    txtIntegralRealFuncion.Text = FuncionesSimples.ValorRealIntegralXCuadrada.ToString();
                    txtSipsonAdaptivo.Text = Simpson.RegresaIntegralFuncionesSimples(EnmFuncionesSimples.XCUADRADA).ToString();            
               }
               if (cmbFuncionesPrueba.SelectedIndex == 2)
               {
                    //txtFuncion.Text = "x^4";
                    //txtNumNodos.Text = "201";
                    //txtA.Text = "0.0";
                    //txtB.Text = "2.0";
                    //txtIntegralRealFuncion.Text = "6.4";
                    //losimp = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = losimp.Integral_Simpson().ToString();
               }
               if (cmbFuncionesPrueba.SelectedIndex == 3)
               {
                    //txtFuncion.Text = "sin(x)";
                    //txtNumNodos.Text = "201";
                    //txtA.Text = "0.0";
                    //txtB.Text = "2.0";
                    //txtIntegralRealFuncion.Text = "1.416146837";
                    //losimp = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = losimp.Integral_Simpson().ToString();
               }
               if (cmbFuncionesPrueba.SelectedIndex == 4)
               {
                    //txtFuncion.Text = "e^x";
                    //txtNumNodos.Text = "201";
                    //txtA.Text = "0.0";
                    //txtB.Text = "2.0";
                    //txtIntegralRealFuncion.Text = "6.389053099";
                    //losimp = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = losimp.Integral_Simpson().ToString();
               }
               if (cmbFuncionesPrueba.SelectedIndex == 5)
               {
                    //txtFuncion.Text = "1/(x+1)";
                    //txtNumNodos.Text = "201";
                    //txtA.Text = "0.0";
                    //txtB.Text = "2.0";
                    //txtIntegralRealFuncion.Text = "1.098612289";
                    //losimp = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = losimp.Integral_Simpson().ToString();
               }
               if (cmbFuncionesPrueba.SelectedIndex == 6)
               {
                    //txtFuncion.Text = "sqrt(1+x^2)";
                    //txtNumNodos.Text = "201";
                    //txtA.Text = "0.0";
                    //txtB.Text = "2.0";
                    //txtIntegralRealFuncion.Text = "2.957885715";
                    //losimp = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = losimp.Integral_Simpson().ToString();
               }
               if (cmbFuncionesPrueba.SelectedIndex == 7)
               {
                    //txtFuncion.Text = "sqrt(1+cos(x)^2)";
                    //txtNumNodos.Text = "201";
                    //txtA.Text = "0.0";
                    //txtB.Text = "48.0";
                    //txtIntegralRealFuncion.Text = "58.47046";
                    //losimp = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = losimp.Integral_Simpson().ToString();
               }
               if (cmbFuncionesPrueba.SelectedIndex == 0)
               {
                    txtFuncion.Text = "";
                    txtNumNodos.Text = "";
                    txtA.Text = "";
                    txtB.Text = "";
                    txtIntegralRealFuncion.Text = "";
                    txtSipsonAdaptivo.Text = "";
                    txtToleranciaFuncion.Text = "";
                    txtToleranciaIntegral.Text = "";
                    txtNumEntrenamientos.Text = "";
               }
               txtErroIntegral.Text = "";
               txtErroFuncion.Text = "";
               txtNumEntrenAlgoritmo.Text = "";
               txtValorIntegral.Text = "";
               txtTiempoEjecucion.Text = "";
          }

          private void cmbFunOscilantes_SelectedIndexChanged( object sender, EventArgs e )
          {
               Simpson loSimpson;
               txtToleranciaFuncion.Text = "1e-6";
               txtToleranciaIntegral.Text = "1e-10";
               txtNumEntrenamientos.Text = "2300";
               txtNumNodos.Text = "201";
               if (cmbFunOscilantes.SelectedIndex == 1)
               {
                    //txtFuncion.Text = "sin(5*x)";
                    //txtA.Text = "0.0";
                    //txtB.Text = "10.0";
                    //txtIntegralRealFuncion.Text = "0.0070067943";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFunOscilantes.SelectedIndex == 2)
               {
                    //txtFuncion.Text = "sin(10*x)";
                    //txtA.Text = "0.0";
                    //txtB.Text = "10.0";
                    //txtIntegralRealFuncion.Text = "0.0137681127";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFunOscilantes.SelectedIndex == 7)
               {
                    //txtFuncion.Text = "((1-cos(x))^5)*sin(5*x)";
                    //txtA.Text = "0.0";
                    //txtB.Text = Convert.ToString(Math.PI * 2);
                    //txtIntegralRealFuncion.Text = "0";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFunOscilantes.SelectedIndex == 8)
               {
                    //txtFuncion.Text = "((1-cos(x))^10)*sin(10*x)";
                    //txtA.Text = "0.0";
                    //txtB.Text = Convert.ToString(Math.PI * 2);
                    //txtIntegralRealFuncion.Text = "0";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFunOscilantes.SelectedIndex == 3)
               {
                    //txtFuncion.Text = "sin(5*cot(x))*sin(2*x)";
                    //txtNumNodos.Text = "201";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI / 2); ;
                    //txtIntegralRealFuncion.Text = "5.291971198151074e-2";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFunOscilantes.SelectedIndex == 4)
               {
                    //txtFuncion.Text = "sin(10*cot(x))*sin(2*x)";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI / 2); ;
                    //txtIntegralRealFuncion.Text = "7.131404290765750e-4";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFunOscilantes.SelectedIndex == 5)
               {
                    //txtFuncion.Text = "sin(50*cot(x))*sin(2*x)";
                    //txtNumNodos.Text = "201";
                    //txtA.Text = Convert.ToString(Math.PI / 2);
                    //txtB.Text = Convert.ToString(Math.PI / 2);
                    //txtIntegralRealFuncion.Text = "1.514836588243969e-20";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFunOscilantes.SelectedIndex == 6)
               {
                    //txtFuncion.Text = "sin(100*cot(x))*sin(2*x)";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI / 2);
                    //txtIntegralRealFuncion.Text = "5.843481678531469e-42";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFunOscilantes.SelectedIndex == 0)
               {
                    txtFuncion.Text = "";
                    txtNumNodos.Text = "";
                    txtA.Text = "";
                    txtB.Text = "";
                    txtIntegralRealFuncion.Text = "";
                    txtSipsonAdaptivo.Text = "";
                    txtToleranciaFuncion.Text = "";
                    txtToleranciaIntegral.Text = "";
                    txtNumEntrenamientos.Text = "";
               }
               txtErroIntegral.Text = "";
               txtErroFuncion.Text = "";
               txtNumEntrenAlgoritmo.Text = "";
               txtValorIntegral.Text = "";
               txtTiempoEjecucion.Text = "";
          }

          private void cmbFuncionesconSngularidad_SelectedIndexChanged( object sender, EventArgs e )
          {
               Simpson loSimpson;
               txtToleranciaFuncion.Text = "1e-6";
               txtToleranciaIntegral.Text = "1e-10";
               txtNumEntrenamientos.Text = "2300";
               txtNumNodos.Text = "201";
               if (cmbFuncionesconSngularidad.SelectedIndex == 1)
               {
                    //txtFuncion.Text = "x*tan(x)";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI);
                    //txtIntegralRealFuncion.Text = "-2.177586090303602";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFuncionesconSngularidad.SelectedIndex == 2)
               {
                    //txtFuncion.Text = "sin(3*x)/cos(x)";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI);
                    //txtIntegralRealFuncion.Text = "0";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFuncionesconSngularidad.SelectedIndex == 3)
               {
                    //txtFuncion.Text = "cos(3*x)/cos(x)";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI);
                    //txtIntegralRealFuncion.Text = "-3.141592653589793";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFuncionesconSngularidad.SelectedIndex == 4)
               {
                    //txtFuncion.Text = "cos(3*x)/cos(x)";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI);
                    //txtIntegralRealFuncion.Text = "-3.141592653589793";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFuncionesconSngularidad.SelectedIndex == 5)
               {
                    //txtFuncion.Text = "cos(11*x)/cos(x)";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI);
                    //txtIntegralRealFuncion.Text = "-3.141592653589793";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFuncionesconSngularidad.SelectedIndex == 6)
               {
                    //txtFuncion.Text = "cos(21*x)/cos(x)";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI);
                    //txtIntegralRealFuncion.Text = "3.141592653589793";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFuncionesconSngularidad.SelectedIndex == 6)
               {
                    //txtFuncion.Text = "cos(201*x)/cos(x)";
                    //txtA.Text = "1e-6";
                    //txtB.Text = Convert.ToString(Math.PI);
                    //txtIntegralRealFuncion.Text = "3.141592653589793";
                    //loSimpson = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
                    //txtSipsonAdaptivo.Text = loSimpson.RegresaValorIntegral().ToString();
               }
               if (cmbFuncionesconSngularidad.SelectedIndex == 0)
               {
                    txtFuncion.Text = "";
                    txtNumNodos.Text = "";
                    txtA.Text = "";
                    txtB.Text = "";
                    txtIntegralRealFuncion.Text = "";
                    txtSipsonAdaptivo.Text = "";
                    txtToleranciaFuncion.Text = "";
                    txtToleranciaIntegral.Text = "";
                    txtNumEntrenamientos.Text = "";
               }
               txtErroIntegral.Text = "";
               txtErroFuncion.Text = "";
               txtNumEntrenAlgoritmo.Text = "";
               txtValorIntegral.Text = "";
               txtTiempoEjecucion.Text = "";
          }

          private void cmbAlgoritmosRNAS_MouseClick( object sender, MouseEventArgs e )
          {
               if (cmbOtrosMetodos.SelectedIndex > 0)
                    cmbOtrosMetodos.SelectedIndex = 0;
          }

          private void cmbOtrosMetodos_MouseClick( object sender, MouseEventArgs e )
          {
               if (cmbAlgoritmosRNAS.SelectedIndex > 0)
                    cmbAlgoritmosRNAS.SelectedIndex = 0;
          }

          private void cmdIniciar_Click( object sender, EventArgs e )
          {
               try
               {
                    HabilitaControles(false);
                    Procesar();
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

          private void Procesar()
          {               
               double ldoA;
               double ldoB;
               double ldoIntegral_Real;
               int lin;
               //int liEntrenamientos = 0;
               CambioVariable loCambioVariable;
               string lsFuncionCambioVariable;
               string lsFuncion;            
               if (cmbAlgoritmosRNAS.SelectedIndex == 0 && cmbOtrosMetodos.SelectedIndex == 0)
                    throw new ApplicationException("Falta seleccionar un metodo de integración");

               if (!ValidaCamposLLenos())
                    return;
               //jLabel1.setText("Calculando... Espere .....");
               // iniciar();        
               //long tiempo=System.currentTimeMillis();
               lsFuncion = txtFuncion.Text;
               ldoA = Convert.ToDouble(txtA.Text);
               ldoB = Convert.ToDouble(txtB.Text);
               lin = Convert.ToInt32(txtNumNodos.Text );
               //<@>validar datos del error de la integral
               if (rbtIntegralReal.Checked)
               {
                    ldoIntegral_Real = Simpson.RegresaIntegralRealFuncion(ldoA, ldoB, lsFuncion);
                    txtIntegralRealFuncion.Text = ldoIntegral_Real.ToString();
               }
               else
               {
                    ldoIntegral_Real = Convert.ToDouble(txtIntegralRealFuncion.Text);                    
               }
                    
               loCambioVariable = new CambioVariable();
               lsFuncionCambioVariable = loCambioVariable.Cambio(txtA.Text, txtB.Text, lsFuncion);
               //lsFuncion = lsFuncionCambioVariable;              
               switch(cmbAlgoritmosRNAS.SelectedIndex)
               {
                    case 1:
                         BackPropagation(ldoA, ldoB, lin, lsFuncionCambioVariable, ldoIntegral_Real);                                
                         break;
                    case 2:
                         FactorMomentum(ldoA, ldoB, lin, lsFuncionCambioVariable, ldoIntegral_Real);
                         break;
                    case 3:
                         GradienteConjugados(ldoA, ldoB, lin, lsFuncionCambioVariable, ldoIntegral_Real);
                         break;
                    case 4:
                         GradientesConjugadosResidual(ldoA, ldoB, lin, lsFuncionCambioVariable, ldoIntegral_Real);
                         break;
                    case 5:
                         Newton(ldoA, ldoB, lin, lsFuncionCambioVariable, ldoIntegral_Real);
                         break;
                    case 6:
                         NewtonTruncado(ldoA, ldoB, lin, lsFuncionCambioVariable, ldoIntegral_Real);
                         break; 
              }
               switch (cmbOtrosMetodos.SelectedIndex)
               {
                    case 1:
                         SipmsonAdaptivo(ldoA, ldoB, lin, lsFuncion, ldoIntegral_Real);
                         break;
               }
          }
         

          private void SipmsonAdaptivo( double pdoA, double pdoB, int Pi_n, string psFuncion, double pdoIntegral_Real )
          {               
               //double ldointegral;
               //TimeSpan lostop;
               //TimeSpan lostart;
               //Simpson loSimpsonAdaptivo;
               //loSimpsonAdaptivo = new Simpson(pdoA, pdoB, psFuncion);
               //lostart = new TimeSpan(DateTime.Now.Ticks);
               //lostop = new TimeSpan(DateTime.Now.Ticks);               
               //ldointegral = 0.0;                              
               //lostart = new TimeSpan(DateTime.Now.Ticks);
               //ldointegral = loSimpsonAdaptivo.RegresaValorIntegral();
               //lostop = new TimeSpan(DateTime.Now.Ticks);              
               //txtValorIntegral.Text = Convert.ToString(ldointegral);
               //txtErroIntegral.Text = Convert.ToString(Math.Abs(pdoIntegral_Real - ldointegral));
               //txtTiempoEjecucion.Text = Convert.ToString(lostop - lostart);
          }

          private void BackPropagation(double pdoA,double pdoB,int Pi_n,string psFuncion, double pdoIntegral_Real)
          {
               double ldoToleranciaIntegral;
               double ldointegral; 
               TimeSpan lostop;
               TimeSpan lostart;
               RNA_BP loRNABackPropagation;
               lostart = new TimeSpan(DateTime.Now.Ticks);
               lostop = new TimeSpan(DateTime.Now.Ticks);
               loRNABackPropagation = new RNA_BP(pdoA,pdoB,Pi_n,psFuncion);
               ldointegral = 0.0;
               //<@> validar las tolerancias
               if(rbtTolIntegral.Checked)
               {
                    ldoToleranciaIntegral=Convert.ToDouble(txtToleranciaIntegral.Text);
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    do
                    {
                         //tiene un valor fijo para la tolerancia
                         ldointegral=loRNABackPropagation.Alg_RNABP(1e-10);
                    }while(ldointegral <= ldoToleranciaIntegral);
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if(rbtTolFuncion.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral=loRNABackPropagation.Alg_RNABP(Convert.ToDouble(txtToleranciaFuncion.Text ));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if(rbtNumEntrenamientos.Checked )
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNABP_ent(Convert.ToInt32(txtNumEntrenAlgoritmo.Text ));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               txtValorIntegral.Text = Convert.ToString(ldointegral);
               txtErroIntegral.Text = Convert.ToString(Math.Abs(pdoIntegral_Real-ldointegral));
               txtNumEntrenAlgoritmo.Text = Convert.ToString(loRNABackPropagation.Iteraciones);
               txtErroFuncion.Text = Convert.ToString(Math.Abs(loRNABackPropagation.FError));
               txtTiempoEjecucion.Text = Convert.ToString(lostop - lostart);
          }

          private void FactorMomentum(double pdoA, double pdoB, int Pi_n, string psFuncion, double pdoIntegral_Real)
          {
               double ldoToleranciaIntegral;
               double ldointegral;
               TimeSpan lostop;
               TimeSpan lostart;
               RNA_FM loRNABackPropagation;
               lostart = new TimeSpan(DateTime.Now.Ticks);
               lostop = new TimeSpan(DateTime.Now.Ticks);
               loRNABackPropagation = new RNA_FM(pdoA, pdoB, Pi_n, psFuncion);
               ldointegral = 0.0;
               //<@> validar las tolerancias
               if (rbtTolIntegral.Checked)
               {
                    ldoToleranciaIntegral = Convert.ToDouble(txtToleranciaIntegral.Text);
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    do
                    {
                         //tiene un valor fijo para la tolerancia
                         ldointegral = loRNABackPropagation.Alg_RNAFM(1e-10);
                    } while (ldointegral <= ldoToleranciaIntegral);
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtTolFuncion.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNAFM(Convert.ToDouble(txtToleranciaFuncion.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtNumEntrenamientos.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNAFM_ent(Convert.ToInt32(txtNumEntrenAlgoritmo.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               txtValorIntegral.Text = Convert.ToString(ldointegral);
               txtErroIntegral.Text = Convert.ToString(Math.Abs(pdoIntegral_Real - ldointegral));
               txtNumEntrenAlgoritmo.Text = Convert.ToString(loRNABackPropagation.Iteraciones);
               txtErroFuncion.Text = Convert.ToString(Math.Abs(loRNABackPropagation.FError));
               txtTiempoEjecucion.Text = Convert.ToString(lostop - lostart);
          }

          private void GradienteConjugados(double pdoA, double pdoB, int Pi_n, string psFuncion, double pdoIntegral_Real)
          {
               double ldoToleranciaIntegral;
               double ldointegral;
               TimeSpan lostop;
               TimeSpan lostart;
               RNA_GC loRNABackPropagation;
               lostart = new TimeSpan(DateTime.Now.Ticks);
               lostop = new TimeSpan(DateTime.Now.Ticks);
               loRNABackPropagation = new RNA_GC(pdoA, pdoB, Pi_n, psFuncion);
               ldointegral = 0.0;
               //<@> validar las tolerancias
               if (rbtTolIntegral.Checked)
               {
                    ldoToleranciaIntegral = Convert.ToDouble(txtToleranciaIntegral.Text);
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    do
                    {
                         //tiene un valor fijo para la tolerancia
                         ldointegral = loRNABackPropagation.Alg_RNAGC(1e-10);
                    } while (ldointegral <= ldoToleranciaIntegral);
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtTolFuncion.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNAGC(Convert.ToDouble(txtToleranciaFuncion.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtNumEntrenamientos.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNAGC_ent(Convert.ToInt32(txtNumEntrenAlgoritmo.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               txtValorIntegral.Text = Convert.ToString(ldointegral);
               txtErroIntegral.Text = Convert.ToString(Math.Abs(pdoIntegral_Real - ldointegral));
               txtNumEntrenAlgoritmo.Text = Convert.ToString(loRNABackPropagation.Iteraciones);
               txtErroFuncion.Text = Convert.ToString(Math.Abs(loRNABackPropagation.FError));
               txtTiempoEjecucion.Text = Convert.ToString(lostop - lostart);
          }
          private void GradientesConjugadosResidual(double pdoA, double pdoB, int Pi_n, string psFuncion, double pdoIntegral_Real)
          {
               double ldoToleranciaIntegral;
               double ldointegral;
               TimeSpan lostop;
               TimeSpan lostart;
               RNA_GCR loRNABackPropagation;
               lostart = new TimeSpan(DateTime.Now.Ticks);
               lostop = new TimeSpan(DateTime.Now.Ticks);
               loRNABackPropagation = new RNA_GCR(pdoA, pdoB, Pi_n, psFuncion);
               ldointegral = 0.0;
               //<@> validar las tolerancias
               if (rbtTolIntegral.Checked)
               {
                    ldoToleranciaIntegral = Convert.ToDouble(txtToleranciaIntegral.Text);
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    do
                    {
                         //tiene un valor fijo para la tolerancia
                         ldointegral = loRNABackPropagation.Alg_RNAGCR(1e-10);
                    } while (ldointegral <= ldoToleranciaIntegral);
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtTolFuncion.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNAGCR(Convert.ToDouble(txtToleranciaFuncion.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtNumEntrenamientos.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNAGCR_ent(Convert.ToInt32(txtNumEntrenAlgoritmo.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               txtValorIntegral.Text = Convert.ToString(ldointegral);
               txtErroIntegral.Text = Convert.ToString(Math.Abs(pdoIntegral_Real - ldointegral));
               txtNumEntrenAlgoritmo.Text = Convert.ToString(loRNABackPropagation.Iteraciones);
               txtErroFuncion.Text = Convert.ToString(Math.Abs(loRNABackPropagation.FError));
               txtTiempoEjecucion.Text = Convert.ToString(lostop - lostart);
          }
          private void Newton(double pdoA, double pdoB, int Pi_n, string psFuncion, double pdoIntegral_Real)
          {
               double ldoToleranciaIntegral;
               double ldointegral;
               TimeSpan lostop;
               TimeSpan lostart;
               RNA_N loRNABackPropagation;
               lostart = new TimeSpan(DateTime.Now.Ticks);
               lostop = new TimeSpan(DateTime.Now.Ticks);
               loRNABackPropagation = new RNA_N(pdoA, pdoB, Pi_n, psFuncion);
               ldointegral = 0.0;
               //<@> validar las tolerancias
               if (rbtTolIntegral.Checked)
               {
                    ldoToleranciaIntegral = Convert.ToDouble(txtToleranciaIntegral.Text);
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    do
                    {
                         //tiene un valor fijo para la tolerancia
                         ldointegral = loRNABackPropagation.Alg_RNAN(1e-10);
                    } while (ldointegral <= ldoToleranciaIntegral);
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtTolFuncion.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNAN(Convert.ToDouble(txtToleranciaFuncion.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtNumEntrenamientos.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNAN_ent(Convert.ToInt32(txtNumEntrenAlgoritmo.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               txtValorIntegral.Text = Convert.ToString(ldointegral);
               txtErroIntegral.Text = Convert.ToString(Math.Abs(pdoIntegral_Real - ldointegral));
               txtNumEntrenAlgoritmo.Text = Convert.ToString(loRNABackPropagation.Iteraciones);
               txtErroFuncion.Text = Convert.ToString(Math.Abs(loRNABackPropagation.FError));
               txtTiempoEjecucion.Text = Convert.ToString(lostop - lostart);
          }
          private void NewtonTruncado( double pdoA, double pdoB, int Pi_n, string psFuncion, double pdoIntegral_Real )
          {
               double ldoToleranciaIntegral;
               double ldointegral;
               TimeSpan lostop;
               TimeSpan lostart;
               RNA_NT loRNABackPropagation;
               lostart = new TimeSpan(DateTime.Now.Ticks);
               lostop = new TimeSpan(DateTime.Now.Ticks);
               loRNABackPropagation = new RNA_NT(pdoA, pdoB, Pi_n, psFuncion);
               ldointegral = 0.0;
               //<@> validar las tolerancias
               if (rbtTolIntegral.Checked)
               {
                    ldoToleranciaIntegral = Convert.ToDouble(txtToleranciaIntegral.Text);
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    do
                    {
                         //tiene un valor fijo para la tolerancia
                         ldointegral = loRNABackPropagation.Alg_RNANT(1e-10);
                    } while (ldointegral <= ldoToleranciaIntegral);
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtTolFuncion.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNANT(Convert.ToDouble(txtToleranciaFuncion.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               if (rbtNumEntrenamientos.Checked)
               {
                    lostart = new TimeSpan(DateTime.Now.Ticks);
                    ldointegral = loRNABackPropagation.Alg_RNANT_ent(Convert.ToInt32(txtNumEntrenAlgoritmo.Text));
                    lostop = new TimeSpan(DateTime.Now.Ticks);
               }
               txtValorIntegral.Text = Convert.ToString(ldointegral);
               txtErroIntegral.Text = Convert.ToString(Math.Abs(pdoIntegral_Real - ldointegral));
               txtNumEntrenAlgoritmo.Text = Convert.ToString(loRNABackPropagation.Iteraciones);
               txtErroFuncion.Text = Convert.ToString(Math.Abs(loRNABackPropagation.FError));
               txtTiempoEjecucion.Text = Convert.ToString(lostop - lostart);
          }

          private void HabilitaControles( bool pbHabilita )
          {
               txtA.Enabled = pbHabilita;
               txtB.Enabled = pbHabilita;
               txtErroFuncion.Enabled = pbHabilita;
               txtErroIntegral.Enabled = pbHabilita;
               txtFuncion.Enabled = pbHabilita;
               txtIntegralRealFuncion.Enabled = pbHabilita;
               txtNumEntrenAlgoritmo.Enabled = pbHabilita;
               txtNumEntrenamientos.Enabled = pbHabilita;
               txtNumNodos.Enabled = pbHabilita;
               txtSipsonAdaptivo.Enabled = pbHabilita;
               txtTiempoEjecucion.Enabled = pbHabilita;
               txtToleranciaFuncion.Enabled = pbHabilita;
               txtToleranciaIntegral.Enabled = pbHabilita;
               txtValorIntegral.Enabled = pbHabilita;
               cmbAlgoritmosRNAS.Enabled = pbHabilita;
               cmbFuncionesconSngularidad.Enabled = pbHabilita;
               cmbFuncionesPrueba.Enabled = pbHabilita;
               cmbFunOscilantes.Enabled = pbHabilita;
               cmbOtrosMetodos.Enabled = pbHabilita;
               btnIniciar.Enabled = pbHabilita;
               btnSalir.Enabled = pbHabilita;
               btnConfiguracion.Enabled = pbHabilita;
               btnFFC.Enabled = pbHabilita;
               btnGraficar.Enabled = pbHabilita;
          }

     }
}