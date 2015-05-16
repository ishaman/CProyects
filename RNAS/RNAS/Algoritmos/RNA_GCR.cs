using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RNA_GCR {
     int _in;
     int _iiteraciones;
     double _doa;
     double _dob;
     double _doaj;
     double _dobj;
     double _doferror;
     double[] _dopj;
     double[] _dogk1;
     double[,] _dohk;
     double[] _dorj; 
     double[] _dodj;
     double[] _doqj;
     double[] _dorj1;
     Globales _oRNAGCR;
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
     public RNA_GCR(double pdoa,double pdob,int Pi_n,string psfuncion)
     {
        //System.out.println("Inicio Datos para RNA_GCR");
          _doa=pdoa; 
          _dob=pdob;_in=Pi_n;
          _oRNAGCR = new Globales(_in);        
          Cs_funcion = psfuncion;
          _in=Pi_n; 
          _doaj=0.0; 
          _dobj=0.0;
          _dopj=new double[_in + 1]; 
          _dorj=new double[_in + 1];
          _dodj=new double[_in + 1]; 
          _doqj=new double[_in + 1];
          _dogk1=new double[_in + 1]; 
          _dorj1= new double[_in + 1];
          _dohk=new double[_in+1,_in+1];
          _iiteraciones=0; 
     }
     public RNA_GCR(double Pdi_a,double pdob,int Pi_n)
     {
        //System.out.println("Inicio Datos para RNA_GCR");
          _doa=Pdi_a; 
          _dob=pdob; 
          _in=Pi_n;
          _oRNAGCR=new Globales(_in);
          _in=Pi_n; 
          _doaj=0.0; 
          _dobj=0.0;
          _dopj=new double[_in + 1]; 
          _dorj=new double[_in + 1];
          _dodj=new double[_in + 1]; 
          _doqj=new double[_in + 1];
          _dogk1=new double[_in + 1]; 
          _dorj1= new double[_in + 1];
          _dohk=new double[_in+1,_in+1];
          _iiteraciones=0;
     }
     #endregion
     public double Alg_RNAGCR(double pdotol)
     {
       // System.out.println("Inicio RNA_GCR");
          int lii;
          double ldointegral=0.0;
          _oRNAGCR.generaDatos(Cs_funcion);
          _oRNAGCR.Coutput();        
          _oRNAGCR.Cerror();
        //ferror = 0.5 *( Math.pow(RNAGCR_.normavector2(), 2));
          gk();
          hk();
          p0();
          r0();
          d0();
          do
          {
               qj();
               alfaj();
               pj();
               for (lii = 0; lii < _in + 1; lii++)
                    _dorj1[lii]=_dorj[lii];
               rj();
               betaj();
               dj();
               E_Pesos_GCR();
               _oRNAGCR.Coutput();
               _oRNAGCR.Cerror();
               _doferror = 0.5 *( Math.Pow(_oRNAGCR.normavector2(), 2));
               _iiteraciones++;
          }while(_doferror > pdotol);
          ldointegral=_oRNAGCR.Integral(_doa, _dob);
          return ldointegral;
     }
     public double Alg_RNAGCR_int(double Pi_tol)
     {
       // System.out.println("Inicio RNA_GCR");
          int lii;
          double ldointegral=0.0;
          do
          {
               _oRNAGCR.generaDatos(Cs_funcion);
               _oRNAGCR.Coutput(); _oRNAGCR.Cerror();
            //ferror = 0.5 *(Math.pow(RNAGCR.normavector2(), 2));
               gk(); 
               hk(); 
               p0(); 
               r0(); 
               d0();
               do
               {
                    qj(); 
                    alfaj(); 
                    pj();
                    for (lii = 0; lii < _in + 1; lii++) 
                        _dorj1[lii]=_dorj[lii];
                    rj(); 
                    betaj(); 
                    dj();
                    E_Pesos_GCR();
                    _oRNAGCR.Coutput();
                    _oRNAGCR.Cerror();
                    _doferror = 0.5 *(Math.Pow(_oRNAGCR.normavector2(), 2));
                    _iiteraciones++;
               }while(_doferror > Pi_tol);
               ldointegral=_oRNAGCR.Integral(_doa, _dob);
          }while(ldointegral > Pi_tol);
          return ldointegral;
     }

     public double Alg_RNAGCR_ent(int Pi_ent)
     {
        //System.out.println("Inicio RNA_GCR");
          int lii;
          double ldointegral=0.0;
          _iiteraciones=0;
          _oRNAGCR.generaDatos(Cs_funcion);
          _oRNAGCR.Coutput(); 
          _oRNAGCR.Cerror();
        //ferror = 0.5 *(Math.pow(RNAGCR.normavector2(), 2));
          gk(); 
          hk(); 
          p0(); 
          r0(); 
          d0();
          do
          {
               qj(); 
               alfaj(); 
               pj();
               for (lii = 0; lii < _in + 1; lii++) 
                    _dorj1[lii]=_dorj[lii];
               rj(); 
               betaj(); 
               dj();
               E_Pesos_GCR();
               _oRNAGCR.Coutput();
               _oRNAGCR.Cerror();
               _doferror = 0.5 *( Math.Pow(_oRNAGCR.normavector2(), 2));
               _iiteraciones++;
          }while(_iiteraciones < Pi_ent);
          ldointegral=_oRNAGCR.Integral(_doa, _dob);
          return ldointegral;
     }
     public double Alg_FFCRNAGCR(double[] pdoy)
     {
       // System.out.println("Inicio RNA_GCR");
          int lii;
          double ldointegral=0.0;
          _oRNAGCR.generaDatos(pdoy);
          _oRNAGCR.Coutput();
          _oRNAGCR.Cerror();
        //ferror = 0.5 *( Math.pow(RNAGCR_.normavector2(), 2));
          gk();
          hk();
          p0();
          r0();
          d0();
          do
          {
               qj();
               alfaj();
               pj();
               for (lii = 0; lii < _in + 1; lii++)
                    _dorj1[lii]=_dorj[lii];
               rj();
               betaj();
               dj();
               E_Pesos_GCR();
               _oRNAGCR.Coutput();
               _oRNAGCR.Cerror();
               _doferror = 0.5 *( Math.Pow(_oRNAGCR.normavector2(), 2));
               _iiteraciones++;
          }while(_doferror > 1e-10);
          ldointegral=_oRNAGCR.Integral(_doa, _dob);
          return Math.Pow(ldointegral,2);
     }
     private void gk()
     {
          int lii,lij;
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    _dogk1[lii] += _oRNAGCR.error[lij] * - _oRNAGCR.C[lij,lii];       
     }
     private void hk()
     {
          int lii, lij, lik;
          for (lii = 0; lii < _in + 1 ; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    for (lik = 0; lik < _in + 1; lik++)
                         _dohk[lii,lij] += _oRNAGCR.C[lii,lik] * _oRNAGCR.C[lik,lij];
     }
     private void r0()
     {
          int lii;
          _dorj = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
               _dorj[lii]=-_dogk1[lii];
     }
     private void d0()
     {
          int lii;
          _dodj = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
               _dodj[lii]=_dorj[lii];
     }
     private void p0()
     {
          int lii;
          _dopj = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
               _dopj[lii] = _oRNAGCR.W[lii];
     }
     private void pj()
     {
          int lii;
          for (lii = 0; lii < _in + 1; lii++)
               _dopj[lii]+= _doaj*_dodj[lii];
     }
     private void qj()
     {
          int lii,lij;
          _doqj = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
             for(lij = 0; lij < _in + 1; lij++)
                _doqj[lii] += _dohk[lii,lij]* _dodj[lij];
     }
     private void rj()
     {
          int lii;
          for (lii = 0; lii < _in + 1; lii++)
               _dorj[lii]+= -_doaj*_doqj[lii];
     }
     private void dj()
     {
          int lii;
          for (lii = 0; lii < _in + 1; lii++)
               _dodj[lii] = _dorj[lii]+_dobj*_dodj[lii];
     }
     private void alfaj()
     {
          int lii;
          double ldoalfa1 = 0.0, ldoalfa2=0.0;
          for (lii = 0; lii < _in + 1; lii++)
               ldoalfa1 += Math.Pow(_dorj[lii], 2);
          for (lii = 0; lii < _in + 1; lii++)
               ldoalfa2 += _dodj[lii]*_doqj[lii];
          _doaj= ldoalfa1/ldoalfa2;
     }     
     private void betaj()
     {
          int lii;
          double ldobeta1 = 0.0, ldobeta2=0.0;
          for (lii = 0; lii < _in + 1; lii++)
               ldobeta1 += Math.Pow(_dorj[lii], 2);
          for (lii = 0; lii < _in + 1; lii++)
               ldobeta2 +=  Math.Pow(_dorj1[lii], 2);
          _dobj= ldobeta1/ldobeta2;
    }
    private void E_Pesos_GCR()
    {
          int lii;
          for (lii = 0; lii < _in + 1; lii++)
               _oRNAGCR.W[lii] = _dopj[lii];
    }
}
