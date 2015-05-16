using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class RandomLibrary
{
     static bool _bFALSE = false;
     static bool _bTRUE = true;

/*
   This Random Number Generator is based on the algorithm in a FORTRAN
   version published by George Marsaglia and Arif Zaman, Florida State
   University; ref.: see original comments below.
   At the fhw (Fachhochschule Wiesbaden, W.Germany), Dept. of Computer
   Science, we have written sources in further languages (C, Modula-2
   Turbo-Pascal(3.0, 5.0), Basic and Ada) to get exactly the same test
   results compared with the original FORTRAN version.
   April 1989
   Karl-L. Noell <NOELL@DWIFH1.BITNET>
      and  Helmut  Weber <WEBER@DWIFH1.BITNET>

   This random number generator originally appeared in "Toward a Universal
   Random Number Generator" by George Marsaglia and Arif Zaman.
   Florida State University Report: FSU-SCRI-87-50 (1987)
   It was later modified by F. James and published in "A Review of Pseudo-
   random Number Generators"
   THIS IS THE BEST KNOWN RANDOM NUMBER GENERATOR AVAILABLE.
   (However, a newly discovered technique can yield
   a period of 10^600. But that is still in the development stage.)
   It passes ALL of the tests for random number generators and has a period
   of 2^144, is completely portable (gives bit identical results on all
   machines with at least 24-bit mantissas in the floating point
   representation).
   The algorithm is a combination of a Fibonacci sequence (with lags of 97
   and 33, and operation "subtraction plus one, modulo one") and an
   "arithmetic sequence" (using subtraction).

   Use IJ = 1802 & KL = 9373 to test the random number generator. The
   subroutine RANMAR should be used to generate 20000 random numbers.
   Then display the next six random numbers generated multiplied by 4096*4096
   If the random number generator is working properly, the random numbers
   should be:
           6533892.0  14220222.0  7275067.0
           6172232.0  8354498.0   10633180.0
*/

/* Globals */
     double[] Cdoa_u = new double[97];
     double _doc;
     double _docd;
     double _docm;
     int _ii97;
     int _ij97;
     bool _btest = _bFALSE;

/*
   This is the initialization routine for the random number generator.
   NOTE: The seed variables can have values between:    0 <= IJ <= 31328
                                                        0 <= KL <= 30081
   The random number sequences created by these two seeds are of sufficient
   length to complete an entire calculation with. For example, if sveral
   different groups are working on different parts of the same calculation,
   each group could be assigned its own IJ seed. This would leave each group
   with 30000 choices for the second seed. That is to say, this random
   number generator can create 900 million different subsequences -- with
   each subsequence having a length of approximately 10^30.
*/

     public RandomLibrary( int piij, int pikl )
     {
          RandomInitialise(piij, pikl);
     }

     void RandomInitialise(int piij,int pikl)
     {
          double ldos;
          double ldot;
          int liii;
          int lii;
          int lij;
          int lik;
          int lil;
          int lijj;
          int lim;

        /*
           Handle the seed range errors
              First random number seed must be between 0 and 31328
              Second seed must have a value between 0 and 30081
        */
          if (piij < 0 || piij > 31328 || pikl < 0 || pikl > 30081) 
          {
               piij = 1802;
               pikl = 9373;
          }

          lii = (piij / 177) % 177 + 2;
          lij = (piij % 177)       + 2;
          lik = (pikl / 169) % 178 + 1;
          lil = (pikl % 169);

          for (liii=0; liii<97; liii++) 
          {
               ldos = 0.0;
               ldot = 0.5;
               for (lijj=0; lijj<24; lijj++) 
               {
                    lim = (((lii * lij) % 179) * lik) % 179;
                    lii = lij;
                    lij = lik;
                    lik = lim;
                    lil = (53 * lil + 1) % 169;
                    if (((lil * lim % 64)) >= 32)
                         ldos += ldot;
                    ldot *= 0.5;
               }
               Cdoa_u[liii] = ldos;
          }
          _doc    = 362436.0 / 16777216.0;
          _docd   = 7654321.0 / 16777216.0;
          _docm   = 16777213.0 / 16777216.0;
          _ii97  = 97;
          _ij97  = 33;
          _btest = _bTRUE;
     }

/*
   This is the random number generator proposed by George Marsaglia in
   Florida State University Report: FSU-SCRI-87-50
*/
     public double RandomUniform()
     {
          double ldouni;

        /* Make sure the initialisation routine has been called */
          if (!_btest)
               RandomInitialise(1802,9373);
          ldouni = Cdoa_u[_ii97-1] - Cdoa_u[_ij97-1];
          if (ldouni <= 0.0)
               ldouni++;
          Cdoa_u[_ii97-1] = ldouni;
          _ii97--;
          if (_ii97 == 0)
               _ii97 = 97;
          _ij97--;
          if (_ij97 == 0)
               _ij97 = 97;
          _doc -= _docd;
          if (_doc < 0.0)
               _doc += _docm;
          ldouni -= _doc;
          if (ldouni < 0.0)
               ldouni++;
          return(ldouni);
     }

/*
  ALGORITHM 712, COLLECTED ALGORITHMS FROM ACM.
  THIS WORK PUBLISHED IN TRANSACTIONS ON MATHEMATICAL SOFTWARE,
  VOL. 18, NO. 4, DECEMBER, 1992, PP. 434-435.
  The function returns a normally distributed pseudo-random number
  with a given mean and standard devaiation.  Calls are made to a
  function subprogram which must return independent random
  numbers uniform in the interval (0,1).
  The algorithm uses the ratio of uniforms method of A.J. Kinderman
  and J.F. Monahan augmented with quadratic bounding curves.
*/
     public double RandomGaussian(double pdomean,double pdostddev)
     {
          double ldoq;
          double ldou;
          double ldov;
          double ldox;
          double ldoy;        
        /*
           Generate P = (u,v) uniform in rect. enclosing acceptance region
           Make sure that any random numbers <= 0 are rejected, since
           gaussian() requires uniforms > 0, but RandomUniform() delivers >= 0.
        */
          do
          {
               ldou = RandomUniform();
               ldov = RandomUniform();
               if (ldou <= 0.0 || ldov <= 0.0) 
               {
                    ldou = 1.0;
                    ldov = 1.0;
               }
               ldov = 1.7156 * (ldov - 0.5);
           /*  Evaluate the quadratic form */
               ldox = ldou - 0.449871;
               ldoy = Math.Abs(ldov) + 0.386595;  //fabs(v) + 0.386595; 
               ldoq = ldox * ldox + ldoy * (0.19600 * ldoy - 0.25472 * ldox);
           /* Accept P if inside inner ellipse */
               if (ldoq < 0.27597)
                    break;
           /*  Reject P if outside outer ellipse, or outside acceptance region */
          } while ((ldoq > 0.27846) || (ldov * ldov > -4.0 * Math.Log(ldou) * ldou * ldou));
         /*  Return ratio of P's coordinates as the normal deviate */
         return (pdomean + pdostddev * ldov / ldou);
     }

/*
   Return random integer within a range, lower -> upper INCLUSIVE
*/
     public int RandomInt(int pilower, int piupper)
     {
          return((int)(RandomUniform() * (piupper - pilower + 1)) + pilower);
     }

/*
   Return random float within a range, lower -> upper
*/
     public double RandomDouble(double pdolower, double pdoupper)
     {
          return((pdoupper - pdolower) * RandomUniform() + pdolower);
     }


     public bool  flip(double pdopf)
     {
          if(RandomDouble(0.0,1.0)<=pdopf)
               return true;
          else 
               return false;
     }

}
