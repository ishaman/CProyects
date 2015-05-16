using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CambioVariable
{
     public CambioVariable()
     {

     }
     public string Cambio( string psRangoA, string psRangoB, string psFuncion )
    {
    	     //int lii;
    	     char[] Lc_buffer;          
    	     //string[] pasarrayfun = psFuncion.Split("x"); //Separador de palabras en java, hay que cambiarlo en c#
	     string lscamvariable;
	     string lsfuncambio;
          string[] Lsa_arrayfun;

          Lc_buffer = new char[psFuncion.Length];
          Lc_buffer = psFuncion.ToCharArray();
          Lsa_arrayfun = psFuncion.Split(new char [] {'x'}); //Hacemos que se delimiten los caracteres
          lscamvariable = "((" + psRangoA + ")+" + "((" + psRangoB + ")-(" + psRangoA + "))/pi*x)";

          lsfuncambio = psFuncion.Replace("x", lscamvariable);

          //lsfuncambio = "";          
          //for (lii = 0; lii < Lsa_arrayfun.Length-1; lii++)
          //{
          //     lsfuncambio+=Lsa_arrayfun[lii];
          //     lsfuncambio+=lscamvariable;
          //}
          //if(!(psFuncion.Length==1))
          //     lsfuncambio+=Lsa_arrayfun[lii];
          //if(Lc_buffer[psFuncion.Length-1]=='x')
          //     lsfuncambio+=lscamvariable;
	// System.out.println("cambio="+funcambio);
          return lsfuncambio;
    }
}

