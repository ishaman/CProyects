using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RNA_NT 
{
     double _doa;
     double _dob;
     double _doepsi;
     double _dodeltaj;
     double _doaj;
     double _dobj;
     double _doferror;
     double[] _dopj;
     double[] _dogk;
     double[,] _dohk;
     double[] _dorj;
     double[] _dodj;
     double[] _doqj;
     double[] _dorj1;
     double[] _dodk;
     int _iiteraciones;
     int _iiteracionesj;
     int _in;
     Globales _oRNANT;
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
     public RNA_NT(double pdoa,double pdob,int Pi_n,string psfuncion)
     {
       // System.out.println("Inicio Datos para RNA Newton Truncado");
          _doa=pdoa; 
          _dob=pdob; 
          Cs_funcion=psfuncion; 
          _in=Pi_n; 
          _doaj=0.0; 
          _dobj=0.0;
          _iiteraciones=0; 
          _iiteracionesj=0; 
          _doepsi=1e-10;
          _oRNANT=new Globales(_in);
          _dopj=new double[_in + 1]; 
          _dorj=new double[_in + 1];
          _dodj=new double[_in + 1]; 
          _doqj=new double[_in + 1];
          _dogk=new double[_in + 1]; 
          _dodk=new double[_in + 1];
          _dorj1= new double[_in + 1]; 
          _dohk=new double[_in+1,_in+1];        
     }
     public RNA_NT(double pdoa,double pdob,int Pi_n)
     {
       // System.out.println("Inicio Datos para RNA Newton Truncado");
          _doa=pdoa; 
          _dob=pdob;
          _in=Pi_n; 
          _doaj=0.0; 
          _dobj=0.0;
          _iiteraciones=0; 
          _iiteracionesj=0; 
          _doepsi=1e-10;
          _oRNANT=new Globales(_in);
          _dopj=new double[_in + 1]; 
          _dorj=new double[_in + 1];
          _dodj=new double[_in + 1]; 
          _doqj=new double[_in + 1];
          _dogk=new double[_in + 1]; 
          _dodk=new double[_in + 1];
          _dorj1= new double[_in + 1]; 
          _dohk=new double[_in+1,_in+1];
    }
     #endregion
     public double Alg_RNANT(double pdotol)
     {
       // System.out.println("Inicio Red Neuronal Neuton Truncado");
          int lii;
          double ldointegral=0.0, ldovalor1=0.0,ldovalor2=0.0;
          _oRNANT.generaDatos(Cs_funcion);
          _oRNANT.Coutput(); 
          _oRNANT.Cerror();
          gk(); 
          hk(); 
          p0(); 
          dk0(); 
          r0(); 
          d0(); 
          _dodeltaj=0.0;
          for (lii = 0; lii < _in + 1; lii++) 
               _dodeltaj += Math.Pow(_dorj[lii], 2);
          do
          {
               E_Pesos_NT(); 
               ldovalor1=0.0; 
               ldovalor2=0.0;
               while(true)
               {
                    qj();
                    for (lii = 0; lii < _in + 1; lii++) 
                         ldovalor1+=_dodj[lii]*_doqj[lii];
                    ldovalor2=_doepsi*_dodeltaj;               
                    if (ldovalor1<=ldovalor2)
                    {
                    if(_iiteracionesj==0)
                         for (lii = 0; lii < _in + 1; lii++) 
                              _dodk[lii]=-_dogk[lii];
                    else
                         for (lii = 0; lii < _in + 1; lii++) 
                              _dodk[lii]=_dopj[lii];
                    break;
               }
               alfaj(); 
               pj();
               for (lii = 0; lii < _in + 1; lii++) 
                    _dorj1[lii]=_dorj[lii];
               rj(); 
               ldovalor1=0.0; 
               ldovalor2=0.0;
               for (lii = 0; lii < _in + 1; lii++) 
                    ldovalor1 += Math.Pow(_dorj[lii], 2);
               for (lii = 0; lii < _in + 1; lii++) 
                    ldovalor2 += Math.Pow(_dogk[lii], 2);
               ldovalor2*=_doepsi;               
               if (ldovalor1 <= ldovalor2 || _iiteracionesj > _in )
               {
                    for (lii = 0; lii < _in + 1; lii++) 
                         _dodk[lii]=_dopj[lii];
                    break;
               }
               betaj(); 
               dj();
               if (_iiteracionesj == _in)
               {
                    for (lii = 0; lii < _in + 1; lii++) 
                         _dodk[lii]=_dopj[lii];
                    break;
               }
               _iiteracionesj++;
            }
            _oRNANT.Coutput();
            _oRNANT.Cerror();
            _doferror = 0.5 *( Math.Pow(_oRNANT.normavector2(), 2));
            _iiteraciones++;
        }while(_doferror > pdotol);
        ldointegral=_oRNANT.Integral(_doa, _dob);
        return ldointegral;
    }
    public double Alg_RNANT_int(double pdotol)
    {
      //  System.out.println("Inicio Red Neuronal Neuton Truncado");
          int lii;
          double ldointegral=0.0,ldovalor1=0.0,ldovalor2=0.0;
          do
          {
               _oRNANT.generaDatos(Cs_funcion);
               _oRNANT.Coutput(); _oRNANT.Cerror();
               gk(); 
               hk(); 
               p0(); 
               dk0(); 
               r0(); 
               d0(); 
               _dodeltaj=0.0;
               for (lii = 0; lii < _in + 1; lii++) 
                    _dodeltaj += Math.Pow(_dorj[lii], 2);
               do
               {
                    E_Pesos_NT();
                    ldovalor1=0.0; 
                    ldovalor2=0.0;
                    while(true)
                    {
                         qj();
                         for (lii = 0; lii < _in + 1; lii++) 
                              ldovalor1+=_dodj[lii]*_doqj[lii];
                         ldovalor2=_doepsi*_dodeltaj;
                         if (ldovalor1<=ldovalor2)
                         {
                              if(_iiteracionesj==0)
                                   for (lii = 0; lii < _in + 1; lii++) 
                                        _dodk[lii]=-_dogk[lii];
                              else
                                   for (lii = 0; lii < _in + 1; lii++) 
                                        _dodk[lii]=_dopj[lii];
                              break;
                         }
                         alfaj(); 
                         pj();
                         for (lii = 0; lii < _in + 1; lii++) 
                              _dorj1[lii]=_dorj[lii];
                         rj(); 
                         ldovalor1=0.0; 
                         ldovalor2=0.0;
                         for (lii = 0; lii < _in + 1; lii++) 
                              ldovalor1 += Math.Pow(_dorj[lii], 2);
                         for (lii = 0; lii < _in + 1; lii++) 
                              ldovalor2 += Math.Pow(_dogk[lii], 2);
                         ldovalor2*=_doepsi;
                         if (ldovalor1 <= ldovalor2 || _iiteracionesj > _in )
                         {
                              for (lii = 0; lii < _in + 1; lii++) 
                                   _dodk[lii]=_dopj[lii];
                              break;
                         }
                         betaj(); 
                         dj();
                         if (_iiteracionesj == _in)
                         {
                              for (lii = 0; lii < _in + 1; lii++) 
                                   _dodk[lii]=_dopj[lii];
                              break;
                         }
                         _iiteracionesj++;
                    }
                    _oRNANT.Coutput();
                    _oRNANT.Cerror();
                    _doferror = 0.5 *( Math.Pow(_oRNANT.normavector2(), 2));
                    _iiteraciones++;
               }while(_doferror > pdotol);
               ldointegral=_oRNANT.Integral(_doa, _dob);
          }while(ldointegral > pdotol);
          return ldointegral;
     }
     public double Alg_RNANT_ent(int ent)
     {
      //  System.out.println("Inicio Red Neuronal Neuton Truncado");
          int lii;
          double ldointegral=0.0,ldovalor1=0.0,ldovalor2=0.0;
          _iiteraciones=0;
          _oRNANT.generaDatos(Cs_funcion);
          _oRNANT.Coutput(); _oRNANT.Cerror();
          gk(); 
          hk(); 
          p0(); 
          dk0(); 
          r0(); 
          d0(); 
          _dodeltaj=0.0;
          for (lii = 0; lii < _in + 1; lii++) 
               _dodeltaj += Math.Pow(_dorj[lii], 2);
          do
          {
               E_Pesos_NT();
               ldovalor1=0.0; ldovalor2=0.0;
               while(true)
               {
                    qj();
                    for (lii = 0; lii < _in + 1; lii++) 
                         ldovalor1+=_dodj[lii]*_doqj[lii];
                    ldovalor2=_doepsi*_dodeltaj;
                    if (ldovalor1<=ldovalor2)
                    {
                         if(_iiteracionesj==0)
                              for (lii = 0; lii < _in + 1; lii++) 
                                   _dodk[lii]=-_dogk[lii];
                         else
                              for (lii = 0; lii < _in + 1; lii++) 
                                   _dodk[lii]=_dopj[lii];
                         break;
                    }
                    alfaj(); 
                    pj();
                    for (lii = 0; lii < _in + 1; lii++) 
                         _dorj1[lii]=_dorj[lii];
                    rj(); 
                    ldovalor1=0.0; 
                    ldovalor2=0.0;
                    for (lii = 0; lii < _in + 1; lii++) 
                         ldovalor1 += Math.Pow(_dorj[lii], 2);
                    for (lii = 0; lii < _in + 1; lii++) 
                         ldovalor2 += Math.Pow(_dogk[lii], 2);
                    ldovalor2*=_doepsi;
                    if (ldovalor1 <= ldovalor2 || _iiteracionesj > _in )
                    {
                         for (lii = 0; lii < _in + 1; lii++) 
                              _dodk[lii]=_dopj[lii];
                         break;
                    }
                    betaj(); 
                    dj();
                    if (_iiteracionesj == _in)
                    {
                         for (lii = 0; lii < _in + 1; lii++) 
                              _dodk[lii]=_dopj[lii];
                         break;
                    }
                    _iiteracionesj++;
               }
               _oRNANT.Coutput();
               _oRNANT.Cerror();
               _doferror = 0.5 *( Math.Pow(_oRNANT.normavector2(), 2));
               _iiteraciones++;
          }while(_iiteraciones < ent);
          ldointegral=_oRNANT.Integral(_doa, _dob);
          return ldointegral;
     }
     public double Alg_FFCRNANT(double[] pdoy)
     {
        //System.out.println("Inicio Red Neuronal Neuton Truncado");
          int lii;
          double ldointegral=0.0,ldovalor1=0.0,ldovalor2=0.0;
          _oRNANT.generaDatos(pdoy);
          _oRNANT.Coutput(); _oRNANT.Cerror();
          gk(); 
          hk(); 
          p0(); 
          dk0(); 
          r0(); 
          d0(); 
          _dodeltaj=0.0;
          for (lii = 0; lii < _in + 1; lii++) 
               _dodeltaj += Math.Pow(_dorj[lii], 2);
          do
          {
               E_Pesos_NT();
               ldovalor1=0.0; 
               ldovalor2=0.0;
               while(true)
               {
                    qj();
                    for (lii = 0; lii < _in + 1; lii++) 
                         ldovalor1+=_dodj[lii]*_doqj[lii];
                    ldovalor2=_doepsi*_dodeltaj;
                    if (ldovalor1<=ldovalor2)
                    {
                         if(_iiteracionesj==0)
                              for (lii = 0; lii < _in + 1; lii++) 
                                   _dodk[lii]=-_dogk[lii];
                         else
                              for (lii = 0; lii < _in + 1; lii++) 
                                   _dodk[lii]=_dopj[lii];
                         break;
                    }
                    alfaj(); 
                    pj();
                    for (lii = 0; lii < _in + 1; lii++) 
                         _dorj1[lii]=_dorj[lii];
                    rj(); 
                    ldovalor1=0.0; 
                    ldovalor2=0.0;
                    for (lii = 0; lii < _in + 1; lii++) 
                         ldovalor1 += Math.Pow(_dorj[lii], 2);
                    for (lii = 0; lii < _in + 1; lii++) 
                         ldovalor2 += Math.Pow(_dogk[lii], 2);
                    ldovalor2*=_doepsi;
                    if (ldovalor1 <= ldovalor2 || _iiteracionesj > _in )
                    {
                         for (lii = 0; lii < _in + 1; lii++) 
                              _dodk[lii]=_dopj[lii];
                         break;
                    }
                    betaj();
                    dj();
                    if (_iiteracionesj == _in)
                    {
                         for (lii = 0; lii < _in + 1; lii++) 
                              _dodk[lii]=_dopj[lii];
                         break;
                    }
                    _iiteracionesj++;
               }
               _oRNANT.Coutput();
               _oRNANT.Cerror();
               _doferror = 0.5 *( Math.Pow(_oRNANT.normavector2(), 2));
               _iiteraciones++;
          }while(_doferror > 1e-10);
          ldointegral=_oRNANT.Integral(_doa, _dob);
          return Math.Pow(ldointegral,2);
     }
     private void dk0()//
     {
          int lii;
          _dodj = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
               _dodk[lii]=_dopj[lii];
    }
     private void gk()
     {
         int lii,lij;
          for (lii = 0; lii < _in + 1; lii++)
            for (lij = 0; lij < _in + 1; lij++)
                _dogk[lii] += _oRNANT.error[lij] * - _oRNANT.C[lij,lii];
    }
     private void hk()
    {
         int lii,lij,lik;
          for (lii = 0; lii < _in + 1 ; lii++)
            for (lij = 0; lij < _in + 1; lij++)
                for (lik = 0; lik < _in + 1; lik++)
                    _dohk[lii,lij] += _oRNANT.C[lii,lik] * _oRNANT.C[lik,lij];
    }
    private void r0()
    {
          int lii;
          _dorj = new double[_in + 1];
          for (lii = 0; lii < _in + 1; lii++)
               _dorj[lii]=-_dogk[lii];
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
               _dopj[lii] = 0;
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
    private void E_Pesos_NT()
    {
          int lii;
          for (lii = 0; lii < _in + 1; lii++)
               _oRNANT.W[lii] += _dodk[lii];
    }
}

