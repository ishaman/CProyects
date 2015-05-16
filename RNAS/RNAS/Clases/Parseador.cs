using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

     class Parseador
     {
          //VARIABLES PRIVADAS
	//Guarda la ltima expresin que se tradujo a postfijo para poder
    //evaluar en ella sin dar una nueva expresin
	private string Cs_UltimaParseada;
	//CONSTRUCTORES
	public Parseador(){
		Cs_UltimaParseada="0";
	}
	//FUNCIONES PUBLICAS
	/**
	 *La funcin que parsea la expresin a notacin postfija.
	 *@param expresion El string con la expresin a parsear.
	 *@return Un string con la expresin parseada en notacin postfija.
	 *@exception SintaxException Error de escritura en la expresin.
	 */
	public string Parsear(string psExpresion)
     {  //throws SintaxException{
		System.Collections.Stack loPilaNumeros = new System.Collections.Stack(); //Pila donde se guardarn los nmeros al parsear
		System.Collections.Stack loPilaOperadores = new System.Collections.Stack(); //Pila donde se guardarn los operadores al parsear
		string lsExpr = quitaEspacios(psExpresion.ToLower());  //La expresin sin espacios ni maysculas.
		string lsFragmento; //Guarda el fragmento de texto que se est utilizando en el momento (ya sea un nmero, un operador, una funcin, etc.)
		int liPos=0, liTamano=0; //pos marca la posicin del caracter que se est procesando actualmente en el string. tamano indica el tamao del texto que se procesa en ese momento.
		byte lbCont=1; //contador indica el nmero de caracteres que se sacan del string en un momento indicado, este no puede ser ms de seis (la funcin con ms caracteres tiene seis)
		//Este es un arreglo de strings que guarda todas las funciones y expresiones permitidas, incluso nmeros, y los acomoda en cada posicin de acuerdo a su tamao
		string[] Las_Funciones={"1 2 3 4 5 6 7 8 9 0 ( ) x e + - * / ^ %",
							"pi",
							"ln(",
							"log( abs( sen( sin( cos( tan( sec( csc( cot( sgn(",
							"rnd() asen( asin( acos( atan( asec( acsc( acot( senh( sinh( cosh( tanh( sech( csch( coth( sqrt(",
							"round( asenh( acosh( atanh( asech( acsch( acoth("};
		//Todas las funciones que trabajan como parntesis de apertura estn aqu.
		string lsParentesis="( ln log abs sen sin cos tan sec csc cot sgn asen asin acos atan asec acsc acot senh sinh cosh tanh sech csch coth sqrt round asenh asinh acosh atanh asech acsch acoth";
		/*
		 *Esta variable 'anterior' se utiliza para saber cul haba sido la ltima
		 *expresin parseada y verificar si hay un error en la expresin o si hay
		 *algn menos unario en la expresin, se utiliza:
		 *0 : no ha parseado nada
		 *1 : un nmero (nmero, pi, e, x)
		 *2 : un operador binario (+ - * / ^ %)
		 *3 : un parntesis (( sen( cos( etc.)
		 *4 : cierre de parntesis ())
		 *Si no se ha parseado nada puede ser cualquier cosa menos (+ * / ^ %)
		 *Si el anterior fue un nmero puede seguir cualquier cosa
		 *Si lo anterior fue un operador puede seguir cualquier cosa menos otro operador (con excepcin de -)
		 *Si lo anterior fue un parntesis puede seguir cualquier cosa menos (+ * / ^ %)
		 *Si lo anterior fue un cierre de parntesis debe seguir un operador, un nmero (en cuyo caso hay un por oculto) u otro parntesis (tambin hay un por oculto)
		 */
		byte lbAnterior=0;

		try
          {
			while(liPos<lsExpr.Length)
               { //Haga mientras la posicin sea menor al tamao del string (mientras este dentro del string)
				liTamano=0;
				lbCont=1;
				while (liTamano==0 && lbCont<=6)
                    { //Este while revisa si el pedazo del texto sacado concuerda con algo conocido
					if(liPos+lbCont<=lsExpr.Length && Las_Funciones[lbCont-1].IndexOf(lsExpr.Substring(liPos,liPos+lbCont))!=-1)
                         {
						liTamano=lbCont;
					}
					lbCont++;
				}

				if (liTamano==0)
                    { //Si no encontr nada es por que hubo un error, se pone la ltima parseada en cero y se lanza una excepcin
					Cs_UltimaParseada="0";
					throw new SintaxException("Error en la expresin");
				}else if(liTamano==1)
                    { //Si encontr algo de tamao uno
					if(isNum(lsExpr.Substring(liPos,liPos+liTamano)))
                         { //Si es un nmero se encarga de sacarlo completo
						if(lbAnterior==1 || lbAnterior==4)
                              {//si hay una multiplicacin oculta
							sacaOperadores(loPilaNumeros, loPilaOperadores, "*");
						}
						lsFragmento=""; //aqu se guardar el nmero sacado
						do
                              { //Hgalo mientras lo que siga sea un nmero o un punto o una coma
							lsFragmento=lsFragmento+lsExpr.ElementAt(liPos);
							liPos++;
						}while(liPos<lsExpr.Length && (isNum(lsExpr.Substring(liPos,liPos+liTamano)) || lsExpr.ElementAt(liPos) == '.' || lsExpr.ElementAt(liPos) == ','));
						try
                              { //Trate de convertirlo en un nmero
							Double.Parse(lsFragmento);
                              }
                              catch (FormatException e)
                              { //Si no pudo pasarlo a nmero hay un error
                                   //NumberFormatException se utiliza en java para indicar cuando no se pudo transformar un string en algun tipo de numero
                                   //FormatException se utiliza cuando el formato de algun argumento en la invocacion de un metodo no tiene algun formato correspondiente
							Cs_UltimaParseada="0";
							throw new SintaxException("Numero mal digitado: " + e.Message );
						}
						loPilaNumeros.Push(lsFragmento);
						lbAnterior=1;
						liPos--;
					}else if (lsExpr.ElementAt(liPos)=='x' || lsExpr.ElementAt(liPos)=='e')
                         { //Si es un nmero conocido
						if(lbAnterior==1 || lbAnterior==4)
                              {//si hay una multiplicacin oculta
							sacaOperadores(loPilaNumeros, loPilaOperadores, "*");
						}
						loPilaNumeros.Push(lsExpr.Substring(liPos,liPos+liTamano));
						lbAnterior=1;
					}else if (lsExpr.ElementAt(liPos)=='+' || lsExpr.ElementAt(liPos)=='*' || lsExpr.ElementAt(liPos)=='/' || lsExpr.ElementAt(liPos)=='%')
                         { //Si es suma, multiplicacin o divisin
						if (lbAnterior==0 || lbAnterior==2 || lbAnterior==3)//Hay error si antes de los operadores no hay nada, hay un parntesis de apertura o un operador
							throw new SintaxException("Error en la expresin");
						sacaOperadores(loPilaNumeros, loPilaOperadores, lsExpr.Substring(liPos,liPos+liTamano));
						lbAnterior=2;
					}else if (lsExpr.ElementAt(liPos)=='^')
                         { //Si es una potencia
						if (lbAnterior==0 || lbAnterior==2 || lbAnterior==3) //Hay error si antes de un apotencia no hay nada, hay un parntesis de apertura o un operador
							throw new SintaxException("Error en la expresin");
						loPilaOperadores.Push("^");
						lbAnterior=2;
					}else if (lsExpr.ElementAt(liPos)=='-')
                         { //Si es una resta
						if(lbAnterior==0 || lbAnterior==2 || lbAnterior==3)
                              {//si hay un menos unario
							loPilaNumeros.Push("-1");
							sacaOperadores(loPilaNumeros, loPilaOperadores, "*");
						}else
                              {//si el menos es binario
							sacaOperadores(loPilaNumeros, loPilaOperadores, "-");
						}
						lbAnterior=2;
					}else if (lsExpr.ElementAt(liPos)=='(')
                         {
						if (lbAnterior==1 || lbAnterior == 4)
                              { //si hay una multiplicacin oculta
							sacaOperadores(loPilaNumeros, loPilaOperadores, "*");
						}
						loPilaOperadores.Push("(");
						lbAnterior=3;
					}else if (lsExpr.ElementAt(liPos)==')')
                         {
						if(lbAnterior!=1 && lbAnterior !=4) //Antes de un cierre de parntesis slo puede haber un nmero u otro cierre de parntesis, sino hay un error
							throw new SintaxException("Error en la expresin");
						while(!(loPilaOperadores.Count == 0) && lsParentesis.IndexOf(((string)loPilaOperadores.Peek()))==-1)
                              {
							sacaOperador(loPilaNumeros, loPilaOperadores);
						}
						if(!((string)loPilaOperadores.Peek()).Equals("("))
                              {
							loPilaNumeros.Push(((string)loPilaNumeros.Pop()) + " " + ((string)loPilaOperadores.Pop()));
						}else
                              {
							loPilaOperadores.Pop();
						}
						lbAnterior=4;
					}
				}else if(liTamano>=2)
                    { //Si lo encontrado es de tamao dos o mayor (todas las funciones se manejan igual)
					lsFragmento=lsExpr.Substring(liPos,liPos+liTamano);
					if(lsFragmento.Equals("pi"))
                         { //Si es el nmero pi
						if(lbAnterior==1 || lbAnterior==4)
                              {//si hay una multiplicacin oculta
							sacaOperadores(loPilaNumeros, loPilaOperadores, "*");
						}
						loPilaNumeros.Push(lsFragmento);
						lbAnterior=1;
					}else if(lsFragmento.Equals("rnd()"))
                         { //Si es la funcin que devuelve un nmero aleatorio (la nica funcin que se maneja como un nmero)
						if(lbAnterior==1 || lbAnterior==4)
                              {//si hay una multiplicacin oculta
							sacaOperadores(loPilaNumeros, loPilaOperadores, "*");
						}
						loPilaNumeros.Push("rnd");
						lbAnterior=1;
					}else{ //Si es cualquier otra funcin
						if (lbAnterior==1 || lbAnterior == 4)
                              { //si hay una multiplicacin oculta
							sacaOperadores(loPilaNumeros, loPilaOperadores, "*");
						}
						loPilaOperadores.Push(lsFragmento.Substring(0,lsFragmento.Length-1)); //Se guarda en la pila de funciones sin el parntesis de apertura (en postfijo no se necesita)
						lbAnterior=3;
					}
				}
				liPos+=liTamano;
			}

			//Procesa al final
			while(!(loPilaOperadores.Count == 0))
               { //Saca todos los operadores mientras la pila no est vaca
				if(lsParentesis.IndexOf((string)loPilaOperadores.Peek())!=-1)
					throw new SintaxException("Hay un parntesis de ms");
				sacaOperador(loPilaNumeros, loPilaOperadores);
			}

		}catch(EmptyStackException e)
          { //Si en algn momento se intenta sacar de la pila y est vaca hay un error
			Cs_UltimaParseada="0";
			throw new SintaxException("Expresin mal digitada:" + e.Message );
		}

		Cs_UltimaParseada=((string)loPilaNumeros.Pop()); //Se obtiene el resultado final

		if(!(loPilaNumeros.Count == 0))
          { //Si la pila de nmeros no qued vaca hay un error
			Cs_UltimaParseada="0";
			throw new SintaxException("Error en la expresin");
		}

		return Cs_UltimaParseada; //Se devuelve el resultado evaluado
	}//Parsear

	/**
	 *Esta es la funcin que evala una expresin parseada en un valor double.
	 *@param expresionParseada Esta es una expresin en notacin postfija (se puede obtener con la funcin parsear).
	 *@param x El valor double en el que se evaluar la funcin.
	 *@return El resultado (un valor double) de evaluar x en la expresin.
	 *@exception ArithmeticException Error al evaluar en los reales.
	 */
	public double f(string psExpresionParseada, double pdoX) 
     {
          System.Collections.Stack loPilaEvaluar = new System.Collections.Stack(); //Pila de doubles para evaluar
		double ldoA, ldoB; //Estos valores son los que se van sacando de la pila de doubles
		stringTokenizer tokens=new stringTokenizer(psExpresionParseada); //La expresin partida en tokens
		string lsTokenActual; //El token que se procesa actualmente
          Random Pr_random;
		try{
			while(tokens.HasMoreTokens())
               { //Haga mientras hayan ms tokens
				lsTokenActual=tokens.NextToken();
				/*
				 *La idea aqu es sacar el token que sigue y verificar qu es ese
				 *token y manejarlo de manera:
				 *Si es un nmero se introduce en la pila de nmeros
				 *Si es una funcin se sacan el valor o los valores necesarios de la pila
				 *de nmeros y se mete el valor evaluado en la funcin correspondiente (u
				 *operador correspondiente).
				 */
				if(lsTokenActual.Equals("e"))
                    { //Si es el nmero e
					loPilaEvaluar.Push(Math.E);
				}else if(lsTokenActual.Equals("pi"))
                    {//Si es el nmero pi
					loPilaEvaluar.Push(Math.PI);
				}else if(lsTokenActual.Equals("x"))
                    {//Si es una x se introduce el valor a evaluar por el usuario
					loPilaEvaluar.Push(pdoX);
				}else if(lsTokenActual.Equals("+"))
                    {//Si es una suma se sacan dos nmeros y se suman
					ldoB=((Double)loPilaEvaluar.Pop());
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(ldoA+ldoB);
				}else if(lsTokenActual.Equals("-"))
                    {//Si es resta se sacan dos valores y se restan (as con todos los operadores)
                         ldoB = ((Double)loPilaEvaluar.Pop());
                         ldoA = ((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(ldoA-ldoB);
				}else if(lsTokenActual.Equals("*"))
                    {//Multiplicacin
					ldoB=((Double)loPilaEvaluar.Pop());
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(ldoA*ldoB);
				}else if(lsTokenActual.Equals("/"))
                    {//Divisin
					ldoB=((Double)loPilaEvaluar.Pop());
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(ldoA/ldoB);
				}else if(lsTokenActual.Equals("^"))
                    {//Potencia
					ldoB=((Double)loPilaEvaluar.Pop());
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Pow(ldoA,ldoB));
				}else if(lsTokenActual.Equals("%"))
                    {//Resto de la divisin entera
					ldoB=((Double)loPilaEvaluar.Pop());
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(ldoA%ldoB);
				}else if(lsTokenActual.Equals("ln"))
                    {//Si es logaritmo natural slo se saca un valor de la pila y se evala
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Log(ldoA));
				}else if(lsTokenActual.Equals("log"))
                    {//Logaritmo en base 10
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Log(ldoA)/Math.Log(10));
				}else if(lsTokenActual.Equals("abs"))
                    {//Valor absoluto
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Abs(ldoA));
				}else if(lsTokenActual.Equals("rnd"))
                    {//Un nmero a random simplemente se mete en la pila de nmeros
                         Pr_random = new Random();
                         loPilaEvaluar.Push(Pr_random.NextDouble());
				}else if(lsTokenActual.Equals("sen") || lsTokenActual.Equals("sin"))
                    { //Seno
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Sin(ldoA));
				}else if(lsTokenActual.Equals("cos"))
                    {//Coseno
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Cos(ldoA));
				}else if(lsTokenActual.Equals("tan"))
                    {//Tangente
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Tan(ldoA));
				}else if(lsTokenActual.Equals("sec"))
                    {//Secante
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(1/Math.Cos(ldoA));
				}else if(lsTokenActual.Equals("csc"))
                    {//Cosecante
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(1/Math.Sin(ldoA));
				}else if(lsTokenActual.Equals("cot"))
                    {//Cotangente
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(1/Math.Tan(ldoA));
				}else if(lsTokenActual.Equals("sgn"))
                    {//Signo
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(sgn(ldoA));
				}else if(lsTokenActual.Equals("asen") || lsTokenActual.Equals("asin"))
                    { //Arcoseno
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Asin(ldoA));
				}else if(lsTokenActual.Equals("acos"))
                    {//Arcocoseno
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Acos(ldoA));
				}else if(lsTokenActual.Equals("atan"))
                    {//Arcotangente
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Atan(ldoA));
				}else if(lsTokenActual.Equals("asec"))
                    {//Arcosecante
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Acos(1/ldoA));
				}else if(lsTokenActual.Equals("acsc"))
                    {//Arcocosecante
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Asin(1/ldoA));
				}else if(lsTokenActual.Equals("acot"))
                    {//Arcocotangente
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Atan(1/ldoA));
				}else if(lsTokenActual.Equals("senh") || lsTokenActual.Equals("sinh"))
                    {//Seno hiperblico
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push((Math.Exp(ldoA)-Math.Exp(-ldoA))/2);
				}else if(lsTokenActual.Equals("cosh"))
                    {//Coseno hiperblico
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push((Math.Exp(ldoA)+Math.Exp(-ldoA))/2);
				}else if(lsTokenActual.Equals("tanh"))
                    {//tangente hiperblica
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push((Math.Exp(ldoA)-Math.Exp(-ldoA))/(Math.Exp(ldoA)+Math.Exp(-ldoA)));
				}else if(lsTokenActual.Equals("sech"))
                    {//Secante hiperblica
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(2/(Math.Exp(ldoA)+Math.Exp(-ldoA)));
				}else if(lsTokenActual.Equals("csch"))
                    {//Cosecante hiperblica
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(2/(Math.Exp(ldoA)-Math.Exp(-ldoA)));
				}else if(lsTokenActual.Equals("coth"))
                    {//Cotangente hiperblica
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push((Math.Exp(ldoA)+Math.Exp(-ldoA))/(Math.Exp(ldoA)-Math.Exp(-ldoA)));
				}else if(lsTokenActual.Equals("asenh") || lsTokenActual.Equals("asinh"))
                    { //Arcoseno hiperblico
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Log(ldoA+Math.Sqrt(ldoA*ldoA+1)));
				}else if(lsTokenActual.Equals("acosh"))
                    {//Arcocoseno hiperblico
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Log(ldoA+Math.Sqrt(ldoA*ldoA-1)));
				}else if(lsTokenActual.Equals("atanh"))
                    {//Arcotangente hiperblica
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Log((1+ldoA)/(1-ldoA))/2);
				}else if(lsTokenActual.Equals("asech"))
                    {//Arcosecante hiperblica
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Log((Math.Sqrt(1-ldoA*ldoA)+1)/ldoA));
				}else if(lsTokenActual.Equals("acsch"))
                    {//Arcocosecante hiperblica
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Log((sgn(ldoA)*Math.Sqrt(ldoA*ldoA +1)+1)/ldoA));
				}else if(lsTokenActual.Equals("acoth"))
                    {//Arcocotangente hiperblica
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Log((ldoA+1)/(ldoA-1))/2);
				}else if(lsTokenActual.Equals("sqrt"))
                    {//Raz cuadrada
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Sqrt(ldoA));
				}else if(lsTokenActual.Equals("round"))
                    {//Redondear
					ldoA=((Double)loPilaEvaluar.Pop());
					loPilaEvaluar.Push(Math.Round(ldoA));
				}else
                    {//si es otra cosa tiene que ser un nmero, simplemente se mete en la pila
					loPilaEvaluar.Push(Double.Parse(lsTokenActual));
				}
			}//while
		}catch(EmptyStackException e)
          { //Si en algn momento se acab la pila hay un error
			throw new ArithmeticException("Expresin mal parseada:" + e.Message );
		}catch(FormatException e)
          { //Si hubo error al traducir un nmero hay un error
               throw new ArithmeticException("Expresin mal digitada" + e.Message);
		}catch(ArithmeticException e)
          { //Cualquier otro error de divisin por cero o logaritmo negativo, etc.
               throw new ArithmeticException("Valor no real en la expresin" + e.Message);
		}
		ldoA=((Double)loPilaEvaluar.Pop()); //El valor a devolver
		if(!(loPilaEvaluar.Count == 0)) //Si todava qued otro valor en la pila hay un error
			throw new ArithmeticException("Expresin mal digitada");
		return ldoA;
	}//funcion f

	/**
	 *Esta funcin evalua la ltima expresin parseada en el valor x.
	 *@param x valor a evaluar.
	 *@return la evaluacin del polinomio en el valor x.
	 */
	public double f(double pdoX)
     {//ArithmeticException{
		try
          {
			return f(Cs_UltimaParseada,pdoX);
		}catch(ArithmeticException e)
          {
			throw e;
		}
	}//Fin de la funcion f

	//FUNCIONES PRIVADAS
	/*
	 *sacaOperador es una funcin que se encarga de sacar un operador de la pila
	 *y manejarlo de manera apropiada, ya sea un operador unario o binario
	 */
	private void sacaOperador(System.Collections.Stack loNumeros, System.Collections.Stack looperadores)
     {//: EmptyStackException{
		string lsOperador, lsA, lsB;
		string lsOperadoresBinarios="+ - * / ^ %"; //Lista de los operadores binarios
		try
          {
			lsOperador=(string)looperadores.Pop(); //Saca el operador que se debe evaluar
			if(lsOperadoresBinarios.IndexOf(lsOperador)!=-1)
               { //Si es un operador binario saca dos elementos de la pila y guarda la operacin
				lsB=(string)loNumeros.Pop();
				lsA=(string)loNumeros.Pop();
				loNumeros.Push(lsA +" "+ lsB + " " + lsOperador );
			}else
               { //Sino slo saca un elemento
				lsA=(string)loNumeros.Pop();
				loNumeros.Push(lsA +" "+ lsOperador);
			}
		}catch(EmptyStackException e)
          {
			throw e;
		}
	}//sacaOperador
	/*
	 *sacaOperadores saca los operadores que tienen mayor prioridad y mete el nuevo operador
	 */
	private void sacaOperadores(System.Collections.Stack loPilaNumeros,System.Collections.Stack loPilaOperadores, string psoperador)
     {
		//Todas las funciones que se manejan como parntesis de apertura
		string psparentesis="( ln log abs sen sin cos tan sec csc cot sgn asen asin acos atan asec acsc acot senh sinh cosh tanh sech csch coth sqrt round asenh asinh acosh atanh asech acsch acoth";
		//mientras la pila no est vaca, el operador que sigue no sea un parntesis de apertura, la longitud del operador sea uno (para que sea un operador), y la prioridad indique que tiene que seguir sacando elementos
		while(!(loPilaOperadores.Count == 0) && psparentesis.IndexOf((string)loPilaOperadores.Peek())==-1 && ((string)loPilaOperadores.Peek()).Length==1 && prioridad(((string)loPilaOperadores.Peek()).ElementAt(0))>=prioridad(psoperador.ElementAt(0)))
          {
			sacaOperador(loPilaNumeros, loPilaOperadores); //Saca el siguiente operador
		}
		loPilaOperadores.Push(psoperador);//Al final mete el nuevo operador luego de sacar todo lo que tena que sacar.
	}

	/*
	 *Funcin que devuelve la prioridad de una operacion
	 */
	private int prioridad(char Pc_s) 
     {
		if (Pc_s=='+' || Pc_s=='-') //Si es una suma o una resta devuelve cero
			return 0;
		else if (Pc_s=='*' || Pc_s=='/' || Pc_s=='%') //Si es multiplicacin, divisin o resto de divisin devuelve uno
			return 1;
		else if (Pc_s=='^')//Si es potencia devuelve dos
			return 2;
		return -1; //Si no fue nada de lo anterior devuelve -1
	} //Fin de la funcion prioridad

	/*
	 *Funcin que pregunta si un caracter es un nmero o no
	 */
	private bool isNum(string pss) 
     {
		if (pss.CompareTo("0")>=0 && pss.CompareTo("9")<=0) //Si el caracter est entre 0 y 9 (si es un nmero)
			return true;
		else
			return false;
	} //Fin de la funcion isNum

	/*
	 *Quita los espacios del string con el polinomio
	 */
	private string quitaEspacios(string pspolinomio)
     {
		string lsunspacedstring = "";	//Variable donde lee la funcin
		for(int lii = 0; lii < pspolinomio.Length; lii++)
          {	//Le quita los espacios a la espresin que ley
			if(pspolinomio.ElementAt(lii) != ' ') //Si el caracter no es un espacio lo pone, sino lo quita.
				lsunspacedstring += pspolinomio.ElementAt(lii);
		}//for
		return lsunspacedstring;
	}//quitaEspacios

	/*
	 *Devuelve el signo del nmero dado
	 */
	private double sgn(double pdoa)
     {
		if(pdoa<0) //Si el nmero es negativo devuelve -1
			return -1;
		else if(pdoa>0)//Si es positivo devuelve 1
			return 1;
		else//Si no es negativo ni positivo devuelve cero
			return 0;
	}

	//CLASES PRIVADAS
	//Clase SintaxException
	//Esta es la excepcin que se lanza si hay algn error sintctico en la expresin
	private class SintaxException : ArithmeticException
     { //En realidad extiende la ArithmeticException
		public SintaxException()
               : base("Error de sintaxis en el polinomio")
          { //Si se llama con el mensaje por defecto
               //ArithmeticException("Error de sintaxis en el polinomio"); //El constructor llama a la clase superior
		}

		public SintaxException(string loexception)
               : base (loexception)
          { //si se llama con otro mensaje
               //throw new ApplicationException(e); //El constructor llama a la clase superior
		}
	}

     private class EmptyStackException : ApplicationException
     {
          public EmptyStackException()
               : base("La Pila esta vacia")
          {

          }
          public EmptyStackException( string loexception )
               : base(loexception)
          {

          }
     }
     
     
     /// <summary>
     /// esta clase no es nativa de c#. //me traje la clase directa de java. Al parecer son compatibles 
     /// no la documento como las demas clases para respetar como me la traje directa de las clases de java
     /// </summary>
     private class stringTokenizer
     {
          private int pos;
          private string str;
          private int len;
          private string delim;
          private bool retDelims;

          public stringTokenizer( string str )
               : this(str, " \t\n\r\f", false)
          {
          }

          public stringTokenizer( string str, string delim )
               : this(str, delim, false)
          {
          }

          public stringTokenizer( string str, string delim, bool retDelims )
          {
               len = str.Length;
               this.str = str;
               this.delim = delim;
               this.retDelims = retDelims;
               this.pos = 0;
          }

          public bool HasMoreTokens()
          {
               if (!retDelims)
               {
                    while (pos < len && delim.IndexOf(str[pos]) >= 0)
                         pos++;
               }
               return pos < len;
          }

          public string NextToken( string delim )
          {
               this.delim = delim;
               return NextToken();
          }

          public string NextToken()
          {
               if (pos < len && delim.IndexOf(str[pos]) >= 0)
               {
                    if (retDelims)
                         return str.Substring(pos++, 1);
                    while (++pos < len && delim.IndexOf(str[pos]) >= 0) ;
               }
               if (pos < len)
               {
                    int start = pos;
                    while (++pos < len && delim.IndexOf(str[pos]) < 0) ;

                    return str.Substring(start, pos - start);
               }
               throw new IndexOutOfRangeException();
          }

          public int CountTokens()
          {
               int count = 0;
               int delimiterCount = 0;
               bool tokenFound = false;
               int tmpPos = pos;

               while (tmpPos < len)
               {
                    if (delim.IndexOf(str[tmpPos++]) >= 0)
                    {
                         if (tokenFound)
                         {
                              count++;
                              tokenFound = false;
                         }
                         delimiterCount++;
                    }
                    else
                    {
                         tokenFound = true;
                         while (tmpPos < len
                             && delim.IndexOf(str[tmpPos]) < 0)
                              ++tmpPos;
                    }
               }
               if (tokenFound)
                    count++;
               return retDelims ? count + delimiterCount : count;
          }
     }
     }

