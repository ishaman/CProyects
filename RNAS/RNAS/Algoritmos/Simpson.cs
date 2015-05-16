using System;

namespace RNAS
{
     public class Simpson
     {
          private double[] _daoValorFuncion;                    
          private Intervalo _oIntervalos;
          private double _doTamDivision;
          private int _iNumeroNodos;          
          Evaluador _oEvaluador;
          public Simpson(string psfuncion, Intervalo poIntervalos, int? piNumeroNodos = null)
          {
                          
               //Numero de nodos
               if(Object.Equals(piNumeroNodos, null))
                    _iNumeroNodos = 500;               
               else
                    _iNumeroNodos = Convert.ToInt32(piNumeroNodos);
               //Parsear funciones               
               _oEvaluador = new Evaluador(psfuncion);
               GeneraValoresParaFuncion(poIntervalos);
          }
          private double[] GeneraValoresFuncion()
          {
               int liIndice;
               double[] laoValores;
               double ldoDivisiones = _oIntervalos.A;
               laoValores = new double[_iNumeroNodos + 1];
               for (liIndice = 0; liIndice < _iNumeroNodos + 1; liIndice++)
               {
                    laoValores[liIndice] = _oEvaluador.F(ldoDivisiones);
                    ldoDivisiones += _doTamDivision;                
               }
               return laoValores;
          }

          private void GeneraValoresParaFuncion(Intervalo poIntervalos)
          {
               //Intervalos de integracion
               _oIntervalos = poIntervalos;
               _doTamDivision = (_oIntervalos.B - _oIntervalos.A) / _iNumeroNodos;
               //Genera los valores de la funcion
               _daoValorFuncion = GeneraValoresFuncion();
          }

          public double RegresaValorIntegral(Intervalo poIntervalos = null)
          {
               //Metodo de simpson
               double ldoAuxiliar;
               double ldoResuladoIntegral;
               int liIndice;
               if(!Object.Equals(poIntervalos,null))
                    GeneraValoresParaFuncion(poIntervalos);
               ldoAuxiliar = (_daoValorFuncion[0] - _daoValorFuncion[_iNumeroNodos]) / 2;
               liIndice = 1;
               while (liIndice <= (_iNumeroNodos - 1))
               {
                    ldoAuxiliar = ldoAuxiliar + (2 * _daoValorFuncion[liIndice] + _daoValorFuncion[liIndice + 1]);
                    liIndice += 2;
               }
               
               ldoResuladoIntegral = 2 * (_oIntervalos.B - _oIntervalos.A) * (ldoAuxiliar / (3 * _iNumeroNodos));
               return ldoResuladoIntegral;
          }

          public static double RegresaIntegralFuncionesSimples(EnmFuncionesSimples penmFuncion)
          {

               Simpson loMetodoSimpson;
               Intervalo loPunto;
               
               switch (penmFuncion)
               {
                    case EnmFuncionesSimples.XCUADRADA:
                         loPunto = new Intervalo(FuncionesSimples.IntervaloValorCero, FuncionesSimples.IntervaloValroDos);
                         loMetodoSimpson = new Simpson(FuncionesSimples.XCUADRADA, loPunto);
                         return loMetodoSimpson.RegresaValorIntegral();                         
               }
               //txtFuncion.Text = "x^2";
               //txtNumNodos.Text = "201";
               //txtA.Text = "0.0";
               //txtB.Text = "2.0";
               //txtIntegralRealFuncion.Text = "";
               //losimp = new Simpson(Convert.ToDouble(txtA.Text), Convert.ToDouble(txtB.Text), txtFuncion.Text);
               //txtSipsonAdaptivo.Text = losimp.Integral_Simpson().ToString();
               return 0.0;
          }

          public static double RegresaIntegralRealFuncion(double pdoA, double pdoB, string psFuncion)
          {
               Simpson loMetodoSimpson;
               Intervalo loPunto;
               if(String.IsNullOrEmpty(psFuncion))
                    return 0.0;
               loPunto = new Intervalo(pdoA, pdoB);
               loMetodoSimpson = new Simpson(psFuncion, loPunto);              
               return loMetodoSimpson.RegresaValorIntegral();                                        
          }
     }
}