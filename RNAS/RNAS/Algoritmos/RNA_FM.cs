using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RNA_FM
{
     int _in; 
     int _iiteraciones;
     double _doa, _dob;
     double _doferror;
     double _dolamda;
     Globales _oRNAFM;
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
     public RNA_FM( double pdoa, double pdob, int Pi_n, string psfuncion )
     {
          // System.out.println("Inicio Datos para RNA_FM");
          _doa = pdoa; 
          _dob = pdob; 
          _in = Pi_n; 
          Cs_funcion = psfuncion; 
          _iiteraciones = 0;
          _doferror = 0.0; 
          _dolamda = 1.0;
          _oRNAFM = new Globales(_in);
     }
     public RNA_FM( double pdoa, double pdob, int Pi_n )
     {
          // System.out.println("Inicio Datos para RNA_FM");
          _doa = pdoa; 
          _dob = pdob; 
          _in = Pi_n; 
          _iiteraciones = 0;
          _doferror = 0.0;
          _dolamda = 1.0;
          _oRNAFM = new Globales(_in);
     }
     #endregion
     public double Alg_RNAFM( double pdotol )
     {
          // System.out.println("Inicio RNA_FM");
          double ldointegral = 0.0;
          _oRNAFM.generaDatos(Cs_funcion);
          _oRNAFM.eta = 1.35 / Math.Pow(_oRNAFM.norma2(), 2);
          do
          {
               _oRNAFM.Coutput(); 
               _oRNAFM.Cerror();
               _doferror = 0.5 * (Math.Pow(_oRNAFM.normavector2(), 2));
               _iiteraciones++;
               E_Pesos_FM();
          } while (_doferror > pdotol);
          ldointegral = _oRNAFM.Integral(_doa, _dob);
          return ldointegral;
     }
     public double Alg_RNAFM_int( double pdotol )
     {
          // System.out.println("Inicio RNA_FM");
          double ldointegral = 0.0;
          do
          {
               _oRNAFM.generaDatos(Cs_funcion);
               _oRNAFM.eta = 1.35 / Math.Pow(_oRNAFM.norma2(), 2);
               do
               {
                    _oRNAFM.Coutput(); 
                    _oRNAFM.Cerror();
                    _doferror = 0.5 * (Math.Pow(_oRNAFM.normavector2(), 2));
                    _iiteraciones++;
                    E_Pesos_FM();
               } while (_doferror > pdotol);
               ldointegral = _oRNAFM.Integral(_doa, _dob);
          } while (ldointegral > pdotol);
          return ldointegral;
     }
     public double Alg_RNAFM_ent( int Pi_ent )
     {
          //System.out.println("Inicio RNA_FM");
          double ldointegral = 0.0;
          _iiteraciones = 0;
          _oRNAFM.generaDatos(Cs_funcion);
          _oRNAFM.eta = 1.35 / Math.Pow(_oRNAFM.norma2(), 2);
          do
          {
               _oRNAFM.Coutput(); 
               _oRNAFM.Cerror();
               _doferror = 0.5 * (Math.Pow(_oRNAFM.normavector2(), 2));
               _iiteraciones++;
               E_Pesos_FM();
          } while (_iiteraciones < Pi_ent);
          ldointegral = _oRNAFM.Integral(_doa, _dob);
          return ldointegral;
     }
     public double Alg_FFCRNAFM( double[] pdoy )
     {
          //System.out.println("Inicio RNA_FM");
          double ldointegral = 0.0;
          _oRNAFM.generaDatos(pdoy);
          _oRNAFM.eta = 1.35 / Math.Pow(_oRNAFM.norma2(), 2);
          do
          {
               _oRNAFM.Coutput(); 
               _oRNAFM.Cerror();
               _doferror = 0.5 * (Math.Pow(_oRNAFM.normavector2(), 2));
               _iiteraciones++;
               E_Pesos_FM();
          } while (_doferror > 1e-10);
          ldointegral = _oRNAFM.Integral(_doa, _dob);
          return Math.Pow(ldointegral, 2);
     }
     private void E_Pesos_FM()
     {
          int lii, lij;
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    _oRNAFM.W[lii] = _oRNAFM.W[lii] + (_oRNAFM.eta * (_dolamda + 1) * _oRNAFM.C[lii,lij] * _oRNAFM.error[lij]);
     }
}