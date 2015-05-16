using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

     public class RNA_BP
     {          
          int _in;
          int _iiteraciones;
          double _doa;
          double _dob;
          double _doferror;
          Globales _oRNABP;
          string Cs_funcion;

          #region Propiedades

          public int Iteraciones
          {
               get { return _iiteraciones; }
               set { _iiteraciones = value; }
          }
          public double FError
          {
               get { return _doferror; }
               set { _doferror = value; }
          }
          #endregion

          #region Contructores
          public RNA_BP( double pdoa, double pdob, int Pi_n, string psfuncion )
          {
               // System.out.println("Inicio Datos para  RNA_BP");
               _doa = pdoa; 
               _dob = pdob; 
               _in = Pi_n; 
               Cs_funcion = psfuncion; 
               _iiteraciones = 0;
               _oRNABP = new Globales(_in);
          }
          public RNA_BP( double pdoa, double pdob, int Pi_n )
          {
               // System.out.println("Inicio Datos para  RNA_BP");
               _doa = pdoa; 
               _dob = pdob; 
               _in = Pi_n; 
               _iiteraciones = 0;
               _oRNABP = new Globales(_in);
          }
          #endregion

          #region Metodos

          public double Alg_RNABP( double pdotol )
          {
               // System.out.println("Inicio RNA_BP");
               double ldointegral = 0.0;
               _oRNABP.generaDatos(Cs_funcion);
               _oRNABP.eta = 1.35 / Math.Pow(_oRNABP.norma2(), 2);
               do
               {
                    _oRNABP.Coutput(); 
                    _oRNABP.Cerror();
                    _doferror = 0.5 * (Math.Pow(_oRNABP.normavector2(), 2));
                    _iiteraciones++;
                    E_Pesos_BP();
               } while (_doferror > pdotol);
               ldointegral = _oRNABP.Integral(_doa, _dob);
               return ldointegral;
          }
          public double Alg_RNABP_int( double pdotol )
          {
               // System.out.println("Inicio RNA_BP");
               double ldointegral = 0.0;
               do
               {
                    _oRNABP.generaDatos(Cs_funcion);
                    _oRNABP.eta = 1.35 / Math.Pow(_oRNABP.norma2(), 2);
                    do
                    {
                         _oRNABP.Coutput(); 
                         _oRNABP.Cerror();
                         _doferror = 0.5 * (Math.Pow(_oRNABP.normavector2(), 2));
                         _iiteraciones++;
                         E_Pesos_BP();
                    } while (_doferror > pdotol);
                    ldointegral = _oRNABP.Integral(_doa, _dob);
               } while (ldointegral > pdotol);
               return ldointegral;
          }
          public double Alg_RNABP_ent( int Pi_ent )
          {
               // System.out.println("Inicio RNA_BP");
               double ldointegral = 0.0;
               _iiteraciones = 0;
               _oRNABP.generaDatos(Cs_funcion);
               _oRNABP.eta = 1.35 / Math.Pow(_oRNABP.norma2(), 2);
               do
               {
                    _oRNABP.Coutput(); 
                    _oRNABP.Cerror();
                    _doferror = 0.5 * (Math.Pow(_oRNABP.normavector2(), 2));
                    _iiteraciones++;
                    E_Pesos_BP();
               } while (_iiteraciones < Pi_ent);
               ldointegral = _oRNABP.Integral(_doa, _dob);
               return ldointegral;
          }
          public double Alg_FFCRNABP( double[] ldoy )
          {
               // System.out.println("Inicio RNA_BP");
               double ldointegral = 0.0;
               _oRNABP.generaDatos(ldoy);
               _oRNABP.eta = 1.35 / Math.Pow(_oRNABP.norma2(), 2);
               do
               {
                    _oRNABP.Coutput();
                    _oRNABP.Cerror();
                    _doferror = 0.5 * (Math.Pow(_oRNABP.normavector2(), 2));
                    _iiteraciones++;
                    E_Pesos_BP();
               } while (_doferror > 1e-10);
               ldointegral = _oRNABP.Integral(_doa, _dob);
               return Math.Pow(ldointegral, 2);
          }

          private void E_Pesos_BP()
          {
               int lii,lij;
               for (lii = 0; lii < _in + 1; lii++)
                    for (lij = 0; lij < _in + 1; lij++)
                         _oRNABP.W[lii] = _oRNABP.W[lii] + (_oRNABP.eta * _oRNABP.C[lii,lij] * _oRNABP.error[lij]);
          }
          #endregion
     }

