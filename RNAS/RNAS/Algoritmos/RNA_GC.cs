using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RNA_GC
{
     int _in;
     int _iiteraciones;
     double _doa;
     double _dob;
     double _doferror;
     double _doak;
     double _dobk;
     double[] _dopk;
     double[] _dogk;
     double[] _dogk1;
     Globales _oRNAGC;
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
     public RNA_GC( double pdoa, double pdob, int Pi_n, string psfuncion )
     {
          //System.out.println("Inicio Datos para RNA_GC");
          _doa = pdoa;
          _dob = pdob;
          Cs_funcion = psfuncion;
          _in = Pi_n;
          _iiteraciones = 0;
          _doak = 0.0;
          _dobk = 0.0;
          _oRNAGC = new Globales(_in);
          _dopk = new double[_in + 1];
          _dogk = new double[_in + 1];
          _dogk1 = new double[_in + 1];
     }
     public RNA_GC( double pdoa, double pdob, int Pi_n )
     {
          // System.out.println("Inicio Datos para RNA_GC");
          _doa = pdoa;
          _dob = pdob;
          _in = Pi_n;
          _iiteraciones = 0;
          _doak = 0.0;
          _dobk = 0.0;
          _oRNAGC = new Globales(_in);
          _dopk = new double[_in + 1];
          _dogk = new double[_in + 1];
          _dogk1 = new double[_in + 1];
     }
     #endregion
     public double Alg_RNAGC( double pdotol )
     {
          //  System.out.println("Inicio RNA_GC");
          double ldointegral = 0.0;
          int lii;
          _oRNAGC.generaDatos(Cs_funcion);
          _oRNAGC.Coutput();
          _oRNAGC.Cerror();
          _doferror = 0.5 * (Math.Pow(_oRNAGC.normavector2(), 2));
          p0();
          g0();
          do
          {
               alfak();
               E_Pesos_GC();
               _oRNAGC.Coutput();
               _oRNAGC.Cerror();
               _doferror = 0.5 * (Math.Pow(_oRNAGC.normavector2(), 2));
               for (lii = 0; lii < _in + 1; lii++)
                    _dogk1[lii] = _dogk[lii];
               gk();
               betak();
               pk();
               _iiteraciones++;
          } while (_doferror > pdotol);
          ldointegral = _oRNAGC.Integral(_doa, _dob);
          return ldointegral;
     }
     public double Alg_RNAGC_int( double pdotol )
     {
          //  System.out.println("Inicio RNA_GC");
          double ldointegral = 0.0;
          int lii;
          do
          {
               _oRNAGC.generaDatos(Cs_funcion);
               _oRNAGC.Coutput();
               _oRNAGC.Cerror();
               _doferror = 0.5 * (Math.Pow(_oRNAGC.normavector2(), 2));
               p0();
               g0();
               do
               {
                    alfak();
                    E_Pesos_GC();
                    _oRNAGC.Coutput();
                    _oRNAGC.Cerror();
                    _doferror = 0.5 * (Math.Pow(_oRNAGC.normavector2(), 2));
                    for (lii = 0; lii < _in + 1; lii++)
                         _dogk1[lii] = _dogk[lii];
                    gk();
                    betak();
                    pk();
                    _iiteraciones++;
               } while (_doferror > pdotol);
               ldointegral = _oRNAGC.Integral(_doa, _dob);
          } while (ldointegral > pdotol);
          return ldointegral;
     }
     public double Alg_RNAGC_ent( int Pi_ent )
     {
          // System.out.println("Inicio RNA_GC");
          double ldointegral = 0.0;
          int lii;
          _iiteraciones = 0;
          _oRNAGC.generaDatos(Cs_funcion);
          _oRNAGC.Coutput();
          _oRNAGC.Cerror();
          _doferror = 0.5 * (Math.Pow(_oRNAGC.normavector2(), 2));
          p0();
          g0();
          do
          {
               alfak();
               E_Pesos_GC();
               _oRNAGC.Coutput();
               _oRNAGC.Cerror();
               _doferror = 0.5 * (Math.Pow(_oRNAGC.normavector2(), 2));
               for (lii = 0; lii < _in + 1; lii++)
                    _dogk1[lii] = _dogk[lii];
               gk();
               betak();
               pk();
               _iiteraciones++;
          } while (_iiteraciones < Pi_ent);
          ldointegral = _oRNAGC.Integral(_doa, _dob);
          return ldointegral;
     }
     public double Alg_FFCRNAGC( double[] pdoy )
     {
          // System.out.println("Inicio RNA_GC");
          double integral = 0.0;
          int lii;
          _oRNAGC.generaDatos(pdoy);
          _oRNAGC.Coutput();
          _oRNAGC.Cerror();
          _doferror = 0.5 * (Math.Pow(_oRNAGC.normavector2(), 2));
          p0();
          g0();
          do
          {
               alfak();
               E_Pesos_GC();
               _oRNAGC.Coutput();
               _oRNAGC.Cerror();
               _doferror = 0.5 * (Math.Pow(_oRNAGC.normavector2(), 2));
               for (lii = 0; lii < _in + 1; lii++)
                    _dogk1[lii] = _dogk[lii];
               gk();
               betak();
               pk();
               _iiteraciones++;
          } while (_doferror > 1e-10);
          integral = _oRNAGC.Integral(_doa, _dob);
          return Math.Pow(integral, 2);
     }

     private void g0()
     {
          int lii;
          _dogk = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
               _dogk[lii] = -_dopk[lii];
     }
     private void p0()
     {
          int lii, lij;
          _dopk = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    _dopk[lii] += _oRNAGC.error[lij] * _oRNAGC.C[lij, lii];
     }

     private void pk()
     {
          int lii;
          _dopk = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
               _dopk[lii] = -_dogk[lii] + _dobk * _dopk[lii];
     }
     private void gk()
     {
          int lii, lij;
          _dogk = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    _dogk[lii] += _oRNAGC.error[lij] * -_oRNAGC.C[lij, lii];
     }
     private void alfak()
     {
          int lii;
          double ldovalor = 0.0;
          for (lii = 0; lii < _in + 1; lii++)
               ldovalor = ldovalor + Math.Pow(_dogk[lii], 2);
          _doak = _doferror / ldovalor;
     }
     private void betak()
     {
          int lii;
          double ldovalor = 0.0, ldovalor1 = 0.0;
          for (lii = 0; lii < _in + 1; lii++)
               ldovalor = ldovalor + Math.Pow(_dogk[lii], 2);
          for (lii = 0; lii < _in + 1; lii++)
               ldovalor1 = ldovalor1 + Math.Pow(_dogk1[lii], 2);
          _dobk = ldovalor / ldovalor1;
     }
     private void E_Pesos_GC()
     {
          int lii;
          for (lii = 0; lii < _in + 1; lii++)
               _oRNAGC.W[lii] = _oRNAGC.W[lii] + _doak * _dopk[lii];
     }
}