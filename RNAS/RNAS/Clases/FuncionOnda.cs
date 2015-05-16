using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FuncionOnda
{
     int _in;
     double _doIntegral, _dolim_a,_dolim_b,_doNv1,_doNv2;
     double[] Cdoa_we = new double[2];
     double[] we_xe = new double[2];
     double[] Cdoa_re = new double[2];
     double[] Cdoa_mu= new double[2];
     double[] Cdoa_ka= new double[2];
     double[] Cdoa_b= new double[2];
     double[] Cdoa_g= new double[2];
     double[] Cdoa_X;
     double[] Cdoa_Y;
     double _doh;


     public double[] Y
     {
          get { return Cdoa_Y; }
          
     }
     public double[] X
     {
          get { return Cdoa_X; }
     }
     public double Integral
     {
          get { return _doIntegral; }

     }
     public FuncionOnda() 
     { 
     }

     public FuncionOnda(double pdowe_1,double pdowe_2,double pdowe_xe_1,double pdowe_xe_2,double pdore_1,double pdore_2,double pdomu,double pdoA, double pdoB,int Pi_N)
     {
          _dolim_a = pdoA;
          _dolim_b = pdoB;
          _in = Pi_N;
          _doNv1 = 0;
          _doNv2 = 0;
          _doh = Math.Abs((_dolim_b - _dolim_a) / _in);
          Cdoa_Y = new double[_in + 1];
          Cdoa_X = new double[_in + 1];
          Cdoa_we[0]=pdowe_1; 
          Cdoa_we[1]=pdowe_2;
	     we_xe[0]=pdowe_xe_1; 
          we_xe[1]=pdowe_xe_2;
	     Cdoa_re[0]=pdore_1; 
          Cdoa_re[1]=pdore_2;
	     Cdoa_mu[0]=pdomu; 
          Cdoa_mu[1]=pdomu;
     }
     public double K (int i)
     {
    	     return Cdoa_we[i]/we_xe[i];
     }

     public double beta(int i)
     {
    	     double cte = 1.21777513710683e-1;
    	     return cte*Math.Sqrt(4*we_xe[i]*Cdoa_mu[i]);
     }

     public double gamma(double ldox)
     {
    	     double ldoz=0.0,ldofs=1.0,ldoF=0.0,ldologgamma=0.0;
          if (ldox < 7)
          {
               ldofs = 1.0;
               ldoz = ldox;
               while (ldoz < 7.0)
               {
                    ldox = ldoz;
                    ldofs = ldofs * ldoz;
                    ldoz = ldoz + 1.0;
               }
               ldox = ldox + 1.0;
               ldofs = -Math.Log(ldofs);
          }
          else
          {
               ldofs = 0;
          }
          ldoz = 1.0 / (ldox * ldox);
          ldologgamma = ldofs + (ldox - 0.5) * Math.Log(ldox) - ldox + 0.918938533204673 + (((-0.000595238095238 * ldoz + 0.000793650793651) * ldoz - 0.002777777777778) * ldoz + 0.083333333333333) / ldox;
        //e = ((x - 0.5) * Math.log(x));
          ldoF= Math.Exp(ldologgamma);
        //System.out.println(F);
          return ldoF;
     }

     public void variables_iniciales()
     {
    	     Cdoa_ka[0]=K(0);    	
    	     Cdoa_ka[1]=K(1);    	
    	     Cdoa_b[0]=beta(0);    	
    	     Cdoa_b[1]=beta(1);    	
    	     Cdoa_g[0]=gamma(Cdoa_ka[0]-1);    	
    	     Cdoa_g[1]=gamma(Cdoa_ka[1]-1);    	
    }

    public double cte_normalizacion(int Pi_v, int Pi_z)
    {
        int lii;
        double ldocte_normalizacion, ldonum=0.0, ldoden=0.0;
        ldocte_normalizacion=Math.Sqrt(Cdoa_b[Pi_z]/Cdoa_g[Pi_z]);
        for (lii=0;lii<=Pi_v-1;lii++)
        {
            ldonum = (Cdoa_ka[Pi_z]-2.0*lii-3.0)*(Cdoa_ka[Pi_z]-lii-1.0);
            ldoden = (lii+1.0)*(Cdoa_ka[Pi_z]-2.0*lii-1.0);
            ldocte_normalizacion = ldocte_normalizacion*Math.Sqrt(ldonum/ldoden);
        }
        return ldocte_normalizacion;
    }
    public double fun_onda(double pdor,double pdoNv, int Pi_v, int Pi_z)
    {
        double ldop=0.0,ldox=0.0,ldoc2=0.0,ldoc3=0.0,ldoc4=0.0,ldosum=0.0;
        double ldocoef_binomial=0.0,ldocoef_superior=0.0,ldocoef_inferior=0.0;
        double ldoaux1=0.0,ldoaux=0.0,ldoaux2=0.0,ldofun_onda=0.0,ldolaguerre=0.0;
        int Pi_j=1;
        ldop = pdor - Cdoa_re[Pi_z];
        ldox = Cdoa_ka[Pi_z] * Math.Exp((-1.0)*Cdoa_b[Pi_z]*ldop);
        ldoc2 = Math.Exp(-ldox/2.0);
        ldoc3 = (Cdoa_ka[Pi_z]-2.0*Pi_v-1.0)/2.0;
        ldoc3= Math.Pow(ldox,ldoc3);
        if (Pi_v != 0)
        {
            ldosum = 0.0;
            for (Pi_j=1; Pi_j<=Pi_v;Pi_j++)
            {
           //calculo del coeficiente binomial
                if (Pi_v == Pi_j)
                    ldocoef_binomial = 1.0;
                else
                {//(v1 >= j) & (j >= 0)
                    ldocoef_superior = gamma(Pi_v + 1);
                    ldocoef_inferior = gamma(Pi_j+1) * gamma ((Pi_v-Pi_j)+1);
                    ldocoef_binomial = ldocoef_superior / ldocoef_inferior;
                }
                ldoaux1 = Math.Pow(-1,Pi_j)*ldocoef_binomial * Math.Pow(ldox,(Pi_v-Pi_j));
                ldoaux = 1.0;
                for (ldop=1;ldop<=Pi_j;ldop++)
                    ldoaux = ldoaux * (Cdoa_ka[Pi_z] - Pi_v - ldop);
                ldoaux2=ldoaux;
                ldoaux1 = ldoaux1 * ldoaux2;
                ldosum = ldosum + ldoaux1;
            }
            ldolaguerre = Math.Pow(ldox, Pi_v) + ldosum;
        }
        else
            ldolaguerre = 1;
        ldoc4=ldolaguerre;
        ldofun_onda = pdoNv * ldoc2 * ldoc3 * ldoc4;
        return ldofun_onda;
    }


     public void genera_fn_integracion(int Pi_v1,int Pi_v2)
     {
          int Pi_i;
          double pdofun_onda1,pdofun_onda2,pdor=_dolim_a;
          variables_iniciales();
    	     for (Pi_i=0;Pi_i<_in;Pi_i++)
          {
               _doNv1=cte_normalizacion(Pi_v1,0);
               _doNv2=cte_normalizacion(Pi_v2,1);
               pdofun_onda1=fun_onda(pdor,_doNv1, Pi_v1,0);
               pdofun_onda2=fun_onda(pdor,_doNv2, Pi_v2,1);
               Cdoa_X[Pi_i] = pdor;
               Cdoa_Y[Pi_i]=pdofun_onda1*pdofun_onda2;
               pdor=pdor+_doh;
          }
    }

     public void Simpson()
     {
    	     double pdos=0,pdoresul;
    	     int Pi_i;
    	     pdos=(Cdoa_Y[2]-Cdoa_Y[_in])/2;
       //System.out.println(" "+s);
    	     Pi_i=1;
    	     while(Pi_i<=(_in-1))
    	     {
    		     pdos=pdos+(2*Cdoa_Y[Pi_i]+Cdoa_Y[Pi_i+1]);
    		     Pi_i+=2;
    	     }
          pdoresul=2*(_dolim_b-_dolim_a)*(pdos/(3*_in));
        //System.out.println(" "+resul);
    	     _doIntegral= Math.Pow(pdoresul,2);
    }
    
     public void FFC_RNABP()
     {
          _doIntegral = 0.0;
          RNA_BP RNABP = new RNA_BP(_dolim_a,_dolim_b,_in);
          _doIntegral = RNABP.Alg_FFCRNABP(Cdoa_Y);          
     }
     public void FFC_RNAFM()
     {
          _doIntegral = 0.0;
          RNA_FM RNAFM = new RNA_FM(_dolim_a,_dolim_b,_in);
          _doIntegral = RNAFM.Alg_FFCRNAFM(Cdoa_Y);          
     }
     public void FFC_RNAGC()
     {
          _doIntegral = 0.0;
          RNA_GC RNAGC = new RNA_GC(_dolim_a,_dolim_b,_in);
          _doIntegral = RNAGC.Alg_FFCRNAGC(Cdoa_Y);          
     }
     public void FFC_RNAGCR()
     {
          _doIntegral = 0.0;
          RNA_GCR RNAGCR = new RNA_GCR(_dolim_a,_dolim_b,_in);
          _doIntegral = RNAGCR.Alg_FFCRNAGCR(Cdoa_Y);          
     }
     public void FFC_RNAN()
     {
          _doIntegral = 0.0;
          RNA_N RNAN = new RNA_N(_dolim_a,_dolim_b,_in);
          _doIntegral = RNAN.Alg_FFCRNAN(Cdoa_Y);          
     }
     public void FFC_RNANT()
     {
          _doIntegral = 0.0;
          RNA_NT RNANT = new RNA_NT(_dolim_a,_dolim_b,_in);
          _doIntegral = RNANT.Alg_FFCRNANT(Cdoa_Y);
     }
}

