using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Globales
{
     private double _doeta;
     private double _doa;
     private double _dob;
     private double _dotolerancia;
     private double[] _doY;
     private double[] _doX;
     private double[] _doW;
     private double[] _doF;
     private double[,] _doC;
     private double[] _doerror;
     private int _in; // 
     //private Parseador _omiFuncion; //Constructor del parseador
     
     private string Cs_expresion; //Expresión a parsear

     #region propiedades
     public double eta
     {
          get
          {
               return _doeta;
          }
          set
          {
               _doeta = value;
          }
     }
     public double[] W
     {
          get
          {
               return _doW;
          }
          set
          {
               _doW = value;
          }
     }

     public double[] error
     {
          get
          {
               return _doerror;
          }
          set
          {
               _doerror = value;
          }
     }
     public double[,] C
     {
          get
          {
               return _doC;
          }
          set
          {
               _doC = value;
          }
     }
     #endregion

     #region contructores
     public Globales( int Pi_n )
     {
        //System.out.println("Inicializo Variables");
          //_omiFuncion = new Parseador();
          _dotolerancia = 10e-6;
          _in = Pi_n;
          _doa=0;
          _dob=Math.PI;
          _doX = new double[Pi_n + 1];
          _doY = new double[Pi_n + 1];
          _doW = new double[Pi_n + 1]; //Pesos
          _doF = new double[Pi_n + 1]; //Salidas Esperadas
          _doC = new double[Pi_n + 1,Pi_n + 1]; //MAtriz de Activación
          _doerror = new double[Pi_n + 1];
          _doeta=0.0;
     }
     #endregion

     #region metodos
     public double norma2()
     {
          int lii, lij;
          double ldovalor = 0;
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    ldovalor = ldovalor + Math.Pow(_doC[lii,lij], 2);
          ldovalor = Math.Sqrt(ldovalor);
          return ldovalor;
     }
     public double normavector2()
     {
          int lii;
          double valor = 0;
          for (lii = 0; lii < _in + 1; lii++)
               valor = valor + Math.Pow(_doerror[lii], 2);
          valor = Math.Sqrt(valor);
          return valor;
     }
     public void generaDatos( string psfuncion ) // genera los datos necesarios para el calculo
     {
          double ldosuma = 0.0;
          int lii, lij;
          Evaluador loEvaluador;
          Random Lr_Aleatorio = new Random();
          // System.out.println("Genero datos");
          //_omiFuncion.Parsear(psfuncion);
          loEvaluador = new Evaluador(psfuncion);
          double partes = Math.Abs(Math.PI / _in);
          _doX[0] = 0.0;
          _doW[0] = Lr_Aleatorio.NextDouble();
          //_doF[0] = _omiFuncion.f(0.0);
          _doF[0] = loEvaluador.F(0.0);
          //System.out.println(F[0]);
          for (lii = 1; lii < _in; lii++)
          {
               _doX[lii] = ldosuma + lii * partes;
               //System.out.println(X[i]);
               _doW[lii] = Lr_Aleatorio.NextDouble();
               //_doF[lii] = _omiFuncion.f(_doX[lii]);
               _doF[lii] = loEvaluador.F(_doX[lii]);
               //System.out.println(F[i]);
          }
          _doX[_in] = Math.PI;
          _doW[_in] = Lr_Aleatorio.NextDouble();
          //_doF[_in] = _omiFuncion.f(Math.PI);
          _doF[_in] = loEvaluador.F(Math.PI);
          //System.out.println(F[n]);
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    _doC[lii,lij] = Math.Cos(lii * _doX[lij]);
     }
     public void generaDatos( double[] pdoy ) // genera los datos necesarios para el calculo
     {
          double ldosuma = 0.0;
          int lii, lij;
          Random Lr_Aleatorio = new Random();
          //System.out.println("Genero datos");
          //  miFuncion.parsear(funcion);
          double ldopartes = Math.Abs(Math.PI / _in);
          _doX[0] = 0.0;
          _doW[0] = Lr_Aleatorio.NextDouble();
          _doF[0] = pdoy[0];
          //System.out.println(F[0]);
          for (lii = 1; lii < _in; lii++)
          {
               _doX[lii] = ldosuma + lii * ldopartes;
               //System.out.println(X[i]);
               _doW[lii] = Lr_Aleatorio.NextDouble();
               _doF[lii] = pdoy[lii];
               //System.out.println(F[i]);
          }
          _doX[_in] = Math.PI;
          _doW[_in] = Lr_Aleatorio.NextDouble();
          _doF[_in] = pdoy[_in];
          //System.out.println(F[n]);
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    _doC[lii,lij] = Math.Cos(lii * _doX[lij]);
     }
     public void Cerror()
     {
          int lii;
          for (lii = 0; lii < _in + 1; lii++)
               _doerror[lii] = _doF[lii] - _doY[lii];
     }
     public void Coutput()
     {
          int lii, lij;
          _doY = new double[_in + 1]; // Salidas Generadas
          for (lii = 0; lii < _in + 1; lii++)
               for (lij = 0; lij < _in + 1; lij++)
                    _doY[lii] += _doW[lij] * _doC[lij,lii];
     }
     public double Integral( double pdoa, double pdob )
     {
          int lij;
          double ldointegral = 0.0, ldok = 0.0, ldoc1 = 0.0, ldoc2 = 0.0, ldoc3 = 0.0, ldoc4 = 0.0;
          /*if (a1==0 && b1 < Math.PI)
          {
              inte=b1*W[0];
              for (j=1;j<n+1;j++)
                 inte=inte+W[j]/j*Math.sin(j*b1);
          }
         else if (a1==0 && b1 == Math.PI)
             inte=Math.PI*W[0];
         else if (a1>0 && b1 == Math.PI)
         {
             inte=(Math.PI-a1)*W[0];
             for (j=1;j<n+1;j++)
                 inte=inte+W[j]/j*Math.sin(j*a1);
         }
         else
         {*/
          ldok = (pdob - pdoa);
          ldointegral = ldok * _doW[0];
          for (lij = 1; lij < _in + 1; lij++)
          {
               ldoc1 = ldok / (lij * Math.PI);
               ldoc2 = Math.Sin(lij * Math.PI);
               ldoc3 = ldoc1 * ldoc2;
               ldoc4 = _doW[lij] * ldoc3;
               ldointegral = ldointegral + ldoc4;
               //inte=inte+W[j]/j*(Math.sin(j*b1)-Math.sin(j*a1));
          }

          // }
          return ldointegral;
     }
     #endregion
}