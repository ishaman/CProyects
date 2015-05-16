using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RNA_N 
{
     double _doa;
     double _dob;
     double _doaj;
     double _dobj;
     double _doferror;
     double[] _dogk;
     double[,] _dohk;
     double[] _dodk;
     double[] _dopk;
     private double[,] _doL;
     int _iiteraciones;
     int _in;
     Globales _oRNAN;
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
     public RNA_N(double pdoa,double pdob,int n1,string psfuncion)
     {
        //System.out.println("Inicio Datos para RNA_N");
          _doa=pdoa; 
          _dob=pdob; 
          _in=n1; 
          _doaj=0.0; 
          _dobj=0.0;
          _iiteraciones=0; 
          Cs_funcion=psfuncion;
          _oRNAN=new Globales(_in);
          _dogk=new double[_in + 1]; 
          _dohk=new double[_in+1,_in+1];
          _dodk=new double[_in+1]; 
          _dopk=new double[_in+1];
          _doferror = 0.0;
     }
     public RNA_N(double pdoa,double pdob,int Pi_n)
     {
       // System.out.println("Inicio Datos para FFCRNA_N");
          _doa=pdoa; 
          _dob=pdob; 
          _in=Pi_n; 
          _doaj=0.0; 
          _dobj=0.0;
          _iiteraciones=0;
          _oRNAN=new Globales(_in);
          _dogk=new double[_in + 1]; 
          _dohk=new double[_in+1,_in+1];
          _dodk=new double[_in+1]; 
          _dopk=new double[_in+1];
          _doferror = 0.0;
     }
     #endregion
     public double Alg_RNAN(double pdotol)
    {
       // System.out.println("Inicio RNA_N");
          double ldointegral=0.0;
          _oRNAN.generaDatos(Cs_funcion);
          _oRNAN.Coutput(); _oRNAN.Cerror();
          //ferror = 0.5 *( Math.pow(RNAN.normavector2(), 2));
          gk(); 
          hk();
          do
          {
               CholeskyDecomposition(); 
               solve();
               E_Pesos_N();
               _oRNAN.Coutput(); 
               _oRNAN.Cerror();
               _doferror = 0.5 *(Math.Pow(_oRNAN.normavector2(), 2));
               gk();
               _iiteraciones++;
          }while(_doferror > pdotol);
          ldointegral=_oRNAN.Integral(_doa, _dob);
          return ldointegral;
     }

     public double Alg_RNAN_int(double pdotol)
     {
       // System.out.println("Inicio RNA_N");
          double ldointegral=0.0;
          do
          {
               _oRNAN.generaDatos(Cs_funcion);
               _oRNAN.Coutput(); 
               _oRNAN.Cerror();
            //ferror = 0.5 *( Math.pow(RNAN.normavector2(), 2));
               gk(); 
               hk();
               do
               {
                    CholeskyDecomposition(); 
                    solve();
                    E_Pesos_N();
                    _oRNAN.Coutput(); _oRNAN.Cerror();
                    _doferror = 0.5 *(Math.Pow(_oRNAN.normavector2(), 2));
                    gk();
                    _iiteraciones++;
               }while(_doferror > pdotol);
          }while(ldointegral > pdotol);
          ldointegral=_oRNAN.Integral(_doa, _dob);
          return ldointegral;
     }

     public double Alg_RNAN_ent(int Pi_ent)
     {
       // System.out.println("Inicio RNA_N");
          double ldointegral=0.0;
          _iiteraciones=0;
          _oRNAN.generaDatos(Cs_funcion);
          _oRNAN.Coutput(); 
          _oRNAN.Cerror();
        //ferror = 0.5 *( Math.pow(RNAN.normavector2(), 2));
          gk(); 
          hk();
          do
          {
               CholeskyDecomposition(); 
               solve();
               E_Pesos_N();
               _oRNAN.Coutput(); 
               _oRNAN.Cerror();
               _doferror = 0.5 *(Math.Pow(_oRNAN.normavector2(), 2));
               gk();
               _iiteraciones++;
          }while(_iiteraciones < Pi_ent);
          ldointegral=_oRNAN.Integral(_doa, _dob);
          return ldointegral;
     }

     public double Alg_FFCRNAN(double[] pdoy)
     {
        //System.out.println("Inicio RNA_N");
          double ldointegral=0.0;
          _oRNAN.generaDatos(pdoy);
          _oRNAN.Coutput(); 
          _oRNAN.Cerror();
        //ferror = 0.5 *( Math.pow(RNAN.normavector2(), 2));
          gk(); 
          hk();
          do
          {
               CholeskyDecomposition(); 
               solve();
               E_Pesos_N();
               _oRNAN.Coutput(); 
               _oRNAN.Cerror();
               _doferror = 0.5 *(Math.Pow(_oRNAN.normavector2(), 2));
               gk();
               _iiteraciones++;
          }while(_doferror > 1e-10);
          ldointegral=_oRNAN.Integral(_doa, _dob);
          return Math.Pow(ldointegral,2);
     }

     private void gk()
     {
          int lii, lij;
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    _dogk[lii] += _oRNAN.error[lij] * - _oRNAN.C[lij,lii];
     }
     private void hk()
     {
          int lii, lij, lik;
          for (lii = 0; lii < _in + 1 ; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    for (lik = 0; lik < _in + 1; lik++)
                         _dohk[lii,lij] += _oRNAN.C[lii,lik] * _oRNAN.C[lik,lij];
    }
     private void E_Pesos_N()
     {
          int lii;
          for (lii = 0; lii < _in + 1; lii++) 
               _oRNAN.W[lii] = _oRNAN.W[lii]+_dodk[lii];
     }

     private void CholeskyDecomposition ()
     {
          int lii,lij,lik;
          double sum;
          double[,] ldoA;          
          ldoA = new double [_in+1,_in+1];
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    ldoA[lii,lij]=_dohk[lii,lij];          
          _doL = new double[_in+1,_in+1];
          for ( lii = 0; lii < _in+1; lii++)
          {
               for (lij = 0; lij <= lii; lij++)
               {
                    sum = 0.0;
                    for (lik = 0; lik < lij; lik++) 
                         sum += _doL[lii,lik] * _doL[lij,lik];
                    if (lii == lij)
                         _doL[lii,lii] = Math.Sqrt(ldoA[lii,lii] - sum);
                    else
                         _doL[lii,lij] = 1.0 / _doL[lij,lij] * (ldoA[lii,lij] - sum);
               }
               if (_doL[lii,lii] <= 0) 
                throw new ApplicationException("Matrix not positive definite");            
          }
     }
     private void solve()
     {
          int lik, lij;
          double[] ldoX = new double[_in+1]; 
          double ldosuma=0.0;
          for(lik=0;lik<_in+1;lik++)
          {
               ldosuma=0.0;
               for(lij=0;lij<lik;lij++) ldosuma+=_doL[lik,lij]*ldoX[lij];
                    ldoX[lik]=(-_dogk[lik]-ldosuma)/_doL[lik,lik];
          }
          for(lik=_in;lik>=0;lik--)
          {
               ldosuma=0.0;
               for(lij=_in;lij>=0;lij--) 
                   ldosuma+=_doL[lij,lik]*_dodk[lij];
               _dodk[lik]=(ldoX[lik]-ldosuma)/_doL[lik,lik];
          }
     }
}
