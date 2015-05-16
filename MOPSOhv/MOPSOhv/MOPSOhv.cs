using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MOPSOhv_
{

    int _iTamanoPoblacion;         /* numero de particulas en la poblacion */
    int _iTamanoArchivo;    /* tamaño del archivo */
    FuncionObjetivo _oTipoFunction;       /* codigo de la funcion objetivo */
    Optimizacion _oOptimization;   /* tipo de optimizacion, 0 minimizacion, 1 maximizacion*/
    RandomLibrary _oAleatorios;
    IndicadorHype _oHype;
    double _doPI;
    bool _bAlgoritmoLimitado;
    int _iNumFunObjetivo;      /* numero maximo de funciones objetivo */
    int _iNumVariables;              /* numero maximo de variables */
    int _iNumGeneraciones;         /* numero maximo de generaciones */
    double[,] _doaVariablesArchivo; /* valores de las variables de las particulas en el archivo*/
    double[,] _doaAptitudArchivo; /* valores de aptitud de las particulas en el archivo */
    double[,] _doaVariablesPoblacion;	 /* valores de las variables de las particulas en la poblacion*/
    double[,] _doaAptitudPoblacion;     /* valores de aptitud de las particulas en la poblacion*/
    double[,] _doaVariablesPbests;	 /* valor pBest de las particulas en la poblacion*/
    double[,] _doaAptitudPbests;	 /* valor de aptitud pBest de las particulas en la poblacion*/
    double[,] _doaVelocidadParticulas;	 /* velocidad de las particulas en la poblacion*/
    double _doProbabilidadMutacion;	 /* probabilidad de mutacion*/
    int _iNumNoDominados;  /* numero de soluciones no dominadas en el archivo*/

    /* Variables para el calculo del hipervolumen */
    double[] _doaContribucionHipervolumen;
    double[] _doaAptitudArchivoArreglo;
    double[] _doaPuntosReferencia;
    double[] _doaAuxilizarPuntosReferencia;
    double _dodelta = 1.0;
    int _iparam_k;
    double _doLimiteInferior = 0.0; //lowerbound
    double _doLimiteSuperior; //upperbound

    public MOPSOhv_(int piPoblacion, int piArchivo, int piAcotado, FuncionObjetivo piFuncion, int piNumertoGeneraciones, double pdoMutacion)
    {
        int lij;
        _iTamanoPoblacion = piPoblacion;         /* numero de particulas en la poblacion */
        _iTamanoArchivo = piArchivo;    /* tamaño del archivo */
        _doPI = Math.PI; // 3.14159265358979          
        _bAlgoritmoLimitado = true; // 1
        _doProbabilidadMutacion = pdoMutacion;	 /* probabilidad de mutacion*/
        _iNumNoDominados = 0;  /* numero de soluciones no dominadas en el archivo*/
        InicializaDatos(piFuncion, piNumertoGeneraciones);
        InicializaMemoria(piPoblacion, piArchivo);
        _oHype = new IndicadorHype();
        for (lij = 0; lij < _iNumFunObjetivo; lij++)
            _doaAuxilizarPuntosReferencia[lij] = Double.MinValue;
        /* Iniciar generador de aleatorios*/
        InicializaAletorios();
    }

    void InicializaDatos(FuncionObjetivo piFuncion, int piNumertoGeneraciones)
    {
        _oTipoFunction = piFuncion;
        _iNumGeneraciones = piNumertoGeneraciones;         /* numero maximo de generaciones */

        switch (piFuncion)
        {
            case FuncionObjetivo.Kita: /* Kita */
                _iNumFunObjetivo = 2;
                _iNumVariables = 2;
                _oOptimization = Optimizacion.MAXIMIZACION;
                break;
            case FuncionObjetivo.Kursawe: /* Kursawe */
                _iNumFunObjetivo = 2;
                _iNumVariables = 3;
                _oOptimization = Optimizacion.MINIMIZACION;
                break;
            case FuncionObjetivo.Deb1: /* Deb 1 */
            case FuncionObjetivo.Deb2:
            case FuncionObjetivo.Deb3:
            case FuncionObjetivo.Fonseca2:
                _iNumFunObjetivo = 2;
                _iNumVariables = 2;
                _oOptimization = Optimizacion.MINIMIZACION;
                break;
            case FuncionObjetivo.zdt1:
            case FuncionObjetivo.zdt2:  /* zdt1 */
            case FuncionObjetivo.zdt3:
                _iNumFunObjetivo = 2;
                _iNumVariables = 30;
                _oOptimization = Optimizacion.MINIMIZACION;
                break;
            case FuncionObjetivo.zdt4: /* zdt4 */
            case FuncionObjetivo.zdt6:
                _iNumFunObjetivo = 2;
                _iNumVariables = 10;
                _oOptimization = Optimizacion.MINIMIZACION;
                break;
            case FuncionObjetivo.DTLZ1: /* DTLZ1*/
                _iNumFunObjetivo = 3;
                _iNumVariables = 7;
                _oOptimization = Optimizacion.MINIMIZACION;
                break;
            case FuncionObjetivo.DTLZ2: /* DTLZ2*/
            case FuncionObjetivo.DTLZ3:
            case FuncionObjetivo.DTLZ4:
            case FuncionObjetivo.DTLZ5:
            case FuncionObjetivo.DTLZ6:
                _iNumFunObjetivo = 3;
                _iNumVariables = 12;
                _oOptimization = Optimizacion.MINIMIZACION;
                break;
            case FuncionObjetivo.DTLZ7: /* DTLZ7*/
                _iNumFunObjetivo = 3;
                _iNumVariables = 22;
                _oOptimization = 0;
                break;
            //case 800:
            //     maxfun = 2;
            //     maxvar = 11;
            //     optimization = 0;
            //     break;
            //case 805:
            //     maxfun = 3;
            //     maxvar = 12;
            //     optimization = 0;
            //     break;
            //case 810:
            //     maxfun = 4;
            //     maxvar = 13;
            //     optimization = 0;
            //     break;
            //case 815:
            //     maxfun = 5;
            //     maxvar = 14;
            //     optimization = 0;
            //     break;
            //case 821:
            //     maxfun = 6;
            //     maxvar = 15;
            //     optimization = 0;
            //     break;
            //case 831:
            //     maxfun = 7;
            //     maxvar = 16;
            //     optimization = 0;
            //     break;
            //case 841:
            //     maxfun = 8;
            //     maxvar = 17;
            //     optimization = 0;
            //     break;
            //case 851:
            //     maxfun = 9;
            //     maxvar = 18;
            //     optimization = 0;
            //     break;
            //case 861:
            //     maxfun = 10;
            //     maxvar = 19;
            //     optimization = 0;
            //     break;
            /** Agregar mas aqui ***/
        }
    }

    void InicializaMemoria(int piPoblacion, int piArchivo)
    {
        /*reservando Memoria*/
        _doaVariablesArchivo = new double[piArchivo, _iNumVariables];
        _doaAptitudArchivo = new double[piArchivo, _iNumFunObjetivo];
        _doaVariablesPoblacion = new double[piPoblacion, _iNumVariables];
        _doaAptitudPoblacion = new double[piPoblacion, _iNumFunObjetivo];
        _doaVariablesPbests = new double[piPoblacion, _iNumVariables];
        _doaAptitudPbests = new double[piPoblacion, _iNumFunObjetivo];
        _doaVelocidadParticulas = new double[piPoblacion, _iNumVariables];
        _doaPuntosReferencia = new double[_iNumFunObjetivo];
        _doaAuxilizarPuntosReferencia = new double[_iNumFunObjetivo];
        _doaContribucionHipervolumen = new double[_iTamanoArchivo];
        _doaAptitudArchivoArreglo = new double[_iTamanoArchivo * _iNumFunObjetivo];  //Posiblemente este no este bien dimensionado
    }

    void InicializaAletorios() /* Iniciando el generador de aleatorios randomlib.h */
    {
        int pii, pij;
        Random Lr_Aleatorio = new Random();
        //srand(( int)time((time_t *)NULL));
        //i = ( int) (31329.0 * rand() / (RAND_MAX + 1.0));
        pii = (int)(31329.0 * Lr_Aleatorio.NextDouble() / (Lr_Aleatorio.Next(2147483647) + 1.0));
        pij = (int)(30082.0 * Lr_Aleatorio.NextDouble() / (Lr_Aleatorio.Next(2147483647) + 1.0));
        _oAleatorios = new RandomLibrary(pii, pij);
    }

    public void InicializaPoblacion() /* Iniciando la poblacion */
    {
        int lii, lij;
        switch (_oTipoFunction)
        {
            case FuncionObjetivo.Kita: /* Kita */
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(0.0, 7.0);
                break;
            case FuncionObjetivo.Kursawe: /* Kursawe */
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(-5.0, 5.0);
                break;
            case FuncionObjetivo.Deb1: /* Deb 1 */
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(0.1, 0.8191);
                break;
            case FuncionObjetivo.Deb2: /* Deb 2 */
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(0.0, 1.0);// 0.8191);
                break;
            case FuncionObjetivo.Deb3: /* Deb 3 */
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(0.0, 1.0);
                break;
            case FuncionObjetivo.Fonseca2: /* Fonseca 2 */
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(-4.0, 4.0);
                break;
            case FuncionObjetivo.zdt1: /* zdt1 */
            case FuncionObjetivo.zdt2: /* zdt2 */
            case FuncionObjetivo.zdt3: /* zdt3 */
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(0.0, 1.0);
                break;

            case FuncionObjetivo.zdt4: /* zdt4 */
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                {
                    _doaVariablesPoblacion[lii, 0] = _oAleatorios.RandomDouble(0.0, 1.0);
                    for (lij = 1; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(-5.0, 5.0);
                }
                break;
            case FuncionObjetivo.zdt6: /* zdt6 */
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(0.0, 1.0);
                break;
            case FuncionObjetivo.DTLZ1: /* dtlz1 */
            case FuncionObjetivo.DTLZ2:
            case FuncionObjetivo.DTLZ3:
            case FuncionObjetivo.DTLZ4:
            case FuncionObjetivo.DTLZ5:
            case FuncionObjetivo.DTLZ6:
            case FuncionObjetivo.DTLZ7:
                for (lii = 0; lii < _iTamanoPoblacion; lii++)
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesPoblacion[lii, lij] = _oAleatorios.RandomDouble(0.0, 1.0);
                break;
            //case 800:
            //case 805:
            //case 810:
            //case 815:
            //case 821:/* zdt2_2 */
            //  case 831:
            //case 841:
            //case 851:
            //case 861:/* zdt2_2 */

            //       for(i=0; i < popsize; i++)
            //            for(j =0; j < maxvar; j++)
            //                 popVar[i,j] = RandomDouble(0.0, 1.0);
            //       break;
            /** Agregar maa aqui ***/
        }
    }

    public void InicializaVelocidadParticulas() /* Velocidad inicial igual a cero */
    {
        int lii, lij;
        for (lii = 0; lii < _iTamanoPoblacion; lii++)
            for (lij = 0; lij < _iNumVariables; lij++)
                _doaVelocidadParticulas[lii, lij] = 0.0;
    }

    public void EvaluaFuncionObjetivo() /* Evaluar las particulas de la poblacion */
    {
        int lii;
        for (lii = 0; lii < _iTamanoPoblacion; lii++)
        {
            switch (_oTipoFunction)
            {
                case FuncionObjetivo.Kita:	/* Kita*/
                    kita(lii);
                    break;
                case FuncionObjetivo.Kursawe:	/* Kursawe*/
                    kursawe(lii);
                    break;
                case FuncionObjetivo.Deb1:	/* Deb's*/
                    deb_1(lii);
                    break;
                case FuncionObjetivo.Deb2:
                    deb_2(lii);
                    break;
                case FuncionObjetivo.Deb3:
                    deb_3(lii);
                    break;
                case FuncionObjetivo.Fonseca2:
                    fonseca_2(lii);
                    break;
                case FuncionObjetivo.zdt1:	/* zdt's*/
                    ZDT1(lii);
                    break;
                case FuncionObjetivo.zdt2:
                    ZDT2(lii);
                    break;
                case FuncionObjetivo.zdt3:
                    ZDT3(lii);
                    break;
                case FuncionObjetivo.zdt4:
                    ZDT4(lii);
                    break;
                case FuncionObjetivo.zdt6:
                    ZDT6(lii);
                    break;
                case FuncionObjetivo.DTLZ1: /*DTLZ's*/
                    DTLZ1(lii);
                    break;
                case FuncionObjetivo.DTLZ2:
                    DTLZ2(lii);
                    break;
                case FuncionObjetivo.DTLZ3:
                    DTLZ3(lii);
                    break;
                case FuncionObjetivo.DTLZ4:
                    DTLZ4(lii);
                    break;
                case FuncionObjetivo.DTLZ5:
                    DTLZ5(lii);
                    break;
                case FuncionObjetivo.DTLZ6:
                    DTLZ6(lii);
                    break;
                case FuncionObjetivo.DTLZ7:
                    DTLZ7(lii);
                    break;
                /*DTLZ2 escalamiento*/
                //case 800:
                //case 805:
                //case 810:
                //case 815:
                //case 821:
                //case 831:
                //case 841:
                //case 851:
                //case 861:
                //     DTLZ2_Escala(i);
                //     break;
            }
            /** Agregar mas aqui **/
        }
    }

    void AlmancenaPbestsdelCumulo() /* Almacenar pBest (variables y valores de fitness) */
    {
        int lii, lij;
        /* Almacenar los valores de las variable */
        for (lii = 0; lii < _iTamanoPoblacion; lii++)
            for (lij = 0; lij < _iNumVariables; lij++)
                _doaVariablesPbests[lii, lij] = _doaVariablesPoblacion[lii, lij];
        /* Almacenar el fitness */
        for (lii = 0; lii < _iTamanoPoblacion; lii++)
            for (lij = 0; lij < _iNumFunObjetivo; lij++)
                _doaAptitudPbests[lii, lij] = _doaAptitudPoblacion[lii, lij];
    }

    public void InsertaParticulasNoDominadasArchivo() /* Insertar particulas no dominadas en el archivo */
    {
        int lii;
        int lij;
        int lik;
        int pitotal;
        int piFondo;  //bottom
        int piContador;
        bool Lb_InsertarParticulaFactibleNoDominada;

        double[] _doaAuxiliarVariablesArchivo = new double[_iNumVariables];
        double[] _doaAuxiliarVariablesPoblacion = new double[_iNumVariables];

        Lb_InsertarParticulaFactibleNoDominada = false;
        for (lii = 0; lii < _iTamanoPoblacion; lii++)
        {
            if (_iNumNoDominados == 0)
            { /* Si el archivo esta vacio insertar la particula en el archivo */
                for (lij = 0; lij < _iNumVariables; lij++)
                    _doaVariablesArchivo[_iNumNoDominados, lij] = _doaVariablesPoblacion[lii, lij];
                for (lij = 0; lij < _iNumFunObjetivo; lij++)
                    _doaAptitudArchivo[_iNumNoDominados, lij] = _doaAptitudPoblacion[lii, lij];
                _iNumNoDominados += 1;
            }
            else
            { /* Si el archivo no esta vacio */
                Lb_InsertarParticulaFactibleNoDominada = true;

                for (lik = 0; lik < _iNumNoDominados; lik++)
                {/*Para cada particula en el archivo */
                    /* Primero, verificar si es factibe */
                    for (lij = 0; lij < _iNumVariables; lij++)
                    {
                        _doaAuxiliarVariablesPoblacion[lij] = _doaVariablesPoblacion[lii, lij];
                        _doaAuxiliarVariablesArchivo[lij] = _doaVariablesArchivo[lik, lij];
                    }
                    /* Si ambas partculas no son factibles */
                    if ((VerificarRestriccionFactibilidad(_doaAuxiliarVariablesArchivo) > 0) && (VerificarRestriccionFactibilidad(_doaAuxiliarVariablesPoblacion) > 0))
                    {
                        EliminarParticulaCumulo(lik); /* Eliminar particula del archivo */
                        Lb_InsertarParticulaFactibleNoDominada = false;		/* No insertar la particula en la poblacion */
                        break;
                    }
                    else if (VerificarRestriccionFactibilidad(_doaAuxiliarVariablesPoblacion) > 0)
                    { /* Si la particula de la poblacion en infactible */
                        Lb_InsertarParticulaFactibleNoDominada = false;/* No insertar la particula en la poblacion */
                        break;
                    }
                    else if (VerificarRestriccionFactibilidad(_doaAuxiliarVariablesArchivo) > 0)
                    { /* Si la particula en el archivo es infactible */
                        EliminarParticulaCumulo(lik); /* Borrar la particula en el archivo  */
                        if ((_iNumNoDominados != 0) || (lik != _iNumNoDominados - 1))
                            lik--;
                        continue;
                    }
                    /* Segundo, verificar dominancia*/
                    pitotal = 0;
                    piContador = 0;
                    /* Si ambos son factiblesf, vereficar no diminancia*/
                    for (lij = 0; lij < _iNumFunObjetivo; lij++)
                    {
                        if ((
                                     (_doaAptitudPoblacion[lii, lij] < _doaAptitudArchivo[lik, lij])
                                       && (_oOptimization == 0))
                                       || ((_doaAptitudPoblacion[lii, lij] > _doaAptitudArchivo[lik, lij])
                                       && (_oOptimization == Optimizacion.MAXIMIZACION)
                               ))
                            pitotal += 1;
                    }
                    for (lij = 0; lij < _iNumFunObjetivo; lij++)
                    {
                        if ((_doaAptitudPoblacion[lii, lij] == _doaAptitudArchivo[lik, lij]))
                        {
                            piContador++;     /* No insertar la particula en la poblacion */
                            Lb_InsertarParticulaFactibleNoDominada = false;
                        }
                    }
                    if (pitotal == _iNumFunObjetivo || piContador == _iNumFunObjetivo)   /* Si la particula es totalmente dominada     */
                        EliminarParticulaCumulo(lik); /* Borrar particula del archivo*/
                    else if (pitotal == 0)
                    {  /* Si la particula en dominada en el archivo*/
                        Lb_InsertarParticulaFactibleNoDominada = false;     /* No insertar la particula en la poblacion */
                        break;
                    }

                } /* Termina la comparacion de una particula en la poblacion con el archivo */
            }
            /* Insertar la particula si es factible y no dominada */
            if (Lb_InsertarParticulaFactibleNoDominada)
            {
                /* Si la memoria no esta llena, insertar particula */
                if (_iNumNoDominados < _iTamanoArchivo)
                {
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesArchivo[_iNumNoDominados, lij] = _doaVariablesPoblacion[lii, lij];
                    for (lij = 0; lij < _iNumFunObjetivo; lij++)
                        _doaAptitudArchivo[_iNumNoDominados, lij] = _doaAptitudPoblacion[lii, lij];
                    _iNumNoDominados += 1;
                }
                else
                {
                    /* Calcular hipervolumen*/
                    piFondo = (int)((_iNumNoDominados - 1) * 0.90);
                    /* Selecionar aleatoriamente un lugar para reemplazar */
                    lik = _oAleatorios.RandomInt(piFondo, _iNumNoDominados - 1);
                    /* Insertar nueva particula en el archivo */
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesArchivo[lik, lij] = _doaVariablesPoblacion[lii, lij];
                    for (lij = 0; lij < _iNumFunObjetivo; lij++)
                        _doaAptitudArchivo[lik, lij] = _doaAptitudPoblacion[lii, lij];
                }
            }
        } /* Termina la comparacion de particulas de la poblacion en el archivo */
    }


    void EliminarParticulaCumulo(int piParticula) /* Eliminar Particula en el archivo */
    {
        int lij;
        /* Si la particula es infactible o la ultima particula del archivo o solo una particula en el archivo */
        if ((_iNumNoDominados == 1) || (piParticula == (_iNumNoDominados - 1)))
        {
            _iNumNoDominados -= 1;
        }
        else
        {
            /*mover la ultima particula en el lugar de la particula infactible */
            for (lij = 0; lij < _iNumVariables; lij++)
                _doaVariablesArchivo[piParticula, lij] = _doaVariablesArchivo[_iNumNoDominados - 1, lij];
            for (lij = 0; lij < _iNumFunObjetivo; lij++)
                _doaAptitudArchivo[piParticula, lij] = _doaAptitudArchivo[_iNumNoDominados - 1, lij];
            _iNumNoDominados -= 1;
        }
    }

    int VerificarRestriccionFactibilidad(double[] _doaVariables) /* Verificar restricciones q detereminan la factibilidad*/
    {
        int piviolations = 0;
        switch (_oTipoFunction)
        {
            case FuncionObjetivo.Kita: /* Kita  */
                if (0 < (double)(_doaVariables[0] / 6.0 + _doaVariables[1] - 13.0 / 2.0))
                    piviolations++;
                if (0 < (double)(_doaVariables[0] / 2.0 + _doaVariables[1] - 15.0 / 2.0))
                    piviolations++;
                if (0 < (double)(5.0 * _doaVariables[0] + _doaVariables[1] - 30.0))
                    piviolations++;
                return piviolations;
            /** Agregar mas aqui**/
            default:
                return 0;
        }
    }

    void CalcularContribucionHipervolumen()
    {
        /*Calcula al gBest conforme a la mejor contribucion*/
        int lii, lij, liInicio;
        _iparam_k = 1;
        for (lij = 0; lij < _iNumFunObjetivo; lij++)
        {
            _doaPuntosReferencia[lij] = _doaAptitudPoblacion[0, lij];
            for (lii = 0; lii < _iNumNoDominados; lii++)
                if (_doaAptitudPoblacion[lii, lij] > _doaPuntosReferencia[lij])
                    _doaPuntosReferencia[lij] = _doaAptitudPoblacion[lii, lij];
        }
        for (lij = 0; lij < _iNumFunObjetivo; lij++)
        {
            if ((_doaPuntosReferencia[lij] + _dodelta) > _doaAuxilizarPuntosReferencia[lij])
            {
                _doaPuntosReferencia[lij] = _doaPuntosReferencia[lij] + _dodelta;
                _doaAuxilizarPuntosReferencia[lij] = _doaPuntosReferencia[lij] + _dodelta;
            }
            else
                _doaPuntosReferencia[lij] = _doaAuxilizarPuntosReferencia[lij];

        }
        if (_iNumFunObjetivo > 2)
        {
            //obtiene los valores mas grandes en todas las dimensiones
            //esto solo es usado para el vencidario 
            //cuando las dimensiones es mas a 3
            //analizar mas a detalle
            _doLimiteSuperior = _doaPuntosReferencia[0];
            for (lij = 0; lij < _iNumFunObjetivo; lij++)
                if (_doaPuntosReferencia[lij] > _doLimiteSuperior)
                    _doLimiteSuperior = _doaPuntosReferencia[lij];
            _doaPuntosReferencia[0] = _doLimiteInferior;
            _doaPuntosReferencia[1] = _doLimiteSuperior + _dodelta;
        }
        _oHype.hypeIndicator(_doaContribucionHipervolumen, _iNumNoDominados, _doaPuntosReferencia, _iparam_k, _doaAptitudArchivoArreglo, _iNumFunObjetivo);
        /* Sort crowding distance values */
        liInicio = 0;
        OrdenarParticulasContribucionHipervolumen(liInicio, _iNumNoDominados);
    }

    /* Parametros para dtlz6
    top = ( int)((nondomCtr-1) * 1);
        gBest =  0;
        velocity[i,j] = 0.4 * velocity[i,j] + 3.0 * RandomDouble(0.0, 1.0) * (archiveVar[lBest,j] - popVar[i,j]) + 1.0 * RandomDouble(0.0, 1.0) * (archiveVar[gBest,j] - popVar[i,j]);
         , */

    public void CalculandoVelocidadParticulas() /* Calcular la nueva velocidad de cada particula en la poblacion */
    {
        int lii, lij, ligBest, litop, lilBest;
        CalcularContribucionHipervolumen();
        litop = (int)((_iNumNoDominados - 1) * 1);
        ligBest = 0;
        for (lii = 0; lii < _iTamanoPoblacion; lii++)
        {
            lilBest = _oAleatorios.RandomInt(1, litop);
            for (lij = 0; lij < _iNumVariables; lij++)
                /* W* Vi + C1  * RandomDouble(0.0, 1.0) * (pBest - Xi) + C2  * RandomDouble(0.0, 1.0) * (gBest  -  Xi) */
                _doaVelocidadParticulas[lii, lij] = 0.4 * _doaVelocidadParticulas[lii, lij]
              + 1.0 * _oAleatorios.RandomDouble(0.0, 1.0) * (_doaVariablesArchivo[lilBest, lij] - _doaVariablesPoblacion[lii, lij])
              + 1.0 * _oAleatorios.RandomDouble(0.0, 1.0) * (_doaVariablesArchivo[ligBest, lij] - _doaVariablesPoblacion[lii, lij]);
        }
    }

    public void CalcularPosicionParticulas()
    {
        int lii, lij;
        /* Calcular la nueva posicion de las particulas */
        for (lii = 0; lii < _iTamanoPoblacion; lii++)
            for (lij = 0; lij < _iNumVariables; lij++)
                _doaVariablesPoblacion[lii, lij] = _doaVariablesPoblacion[lii, lij] + _doaVelocidadParticulas[lii, lij];
    }

    public void PerturbacionParticulas(int piGeneracion) /* Mutacion de PArticulas MOPSO */
    {
        int lii;
        int lidimension = 0;
        double[] _doaValorMinimo = new double[_iNumVariables];
        double[] _doaValorMaximo = new double[_iNumVariables];
        double _doMinvaluetemp, _doMaxvaluetemp, _doRange;
        double _doValtemp = 0;
        ObtenerRangoValoresVariables(_doaValorMinimo, _doaValorMaximo);
        for (lii = 0; lii < _iTamanoPoblacion; lii++)
        {
            if (_oAleatorios.flip(Math.Pow(1.0 - (double)piGeneracion / (_iNumGeneraciones * _doProbabilidadMutacion), 1.5)))
            {
                lidimension = _oAleatorios.RandomInt(0, _iNumVariables - 1);
                _doRange = (_doaValorMaximo[lidimension]
                        - _doaValorMinimo[lidimension])
                        * Math.Pow(1.0 - (double)piGeneracion
                        / (_iNumGeneraciones * _doProbabilidadMutacion), 1.5) / 2;
                _doValtemp = _oAleatorios.RandomDouble(_doRange, -_doRange);
                if (_doaVariablesPoblacion[lii, lidimension] - _doRange < _doaValorMinimo[lidimension])
                    _doMinvaluetemp = _doaValorMinimo[lidimension];
                else
                    _doMinvaluetemp = _doaVariablesPoblacion[lii, lidimension] - _doRange;
                if (_doaVariablesPoblacion[lii, lidimension] + _doRange > _doaValorMaximo[lidimension])
                    _doMaxvaluetemp = _doaValorMaximo[lidimension];
                else
                    _doMaxvaluetemp = _doaVariablesPoblacion[lii, lidimension] + _doRange;
                _doaVariablesPoblacion[lii, lidimension] = _oAleatorios.RandomDouble(_doMinvaluetemp, _doMaxvaluetemp);
            }
        }
    }

    public void InsertaNuevasPartiuclasNoDominadsenArchivo()  /* Insertar nuevas particulas no dominadas de la poblacion en el archivo */
    {
        int lii;
        int lij;
        int liParticula;
        int liPosicionMenorParticula;
        int liPosicionAleatoria;
        int liTotalElementosDeMatriz;
        int liPosicionPeorParticula;
        double Ldo_ContribucionPeorParticula;

        liPosicionPeorParticula = 0;
        if (_bAlgoritmoLimitado)
        {
            for (lii = 0, liTotalElementosDeMatriz = 0; lii < _iNumNoDominados; lii++)
                for (lij = 0; lij < _iNumFunObjetivo; lij++)
                {
                    _doaAptitudArchivoArreglo[liTotalElementosDeMatriz] = _doaAptitudArchivo[lii, lij];
                    liTotalElementosDeMatriz++;
                }
            _oHype.hypeIndicator(_doaContribucionHipervolumen, _iNumNoDominados, _doaPuntosReferencia, _iparam_k, _doaAptitudArchivoArreglo, _iNumFunObjetivo);
            Ldo_ContribucionPeorParticula = _doaContribucionHipervolumen[0];
            liPosicionPeorParticula = 0;
            for (lii = 1; lii < _iNumNoDominados; lii++)
            {
                if (_doaContribucionHipervolumen[lii] < Ldo_ContribucionPeorParticula)
                {
                    Ldo_ContribucionPeorParticula = _doaContribucionHipervolumen[lii];
                    liPosicionPeorParticula = lii;
                }
            }

        }
        /* para cada particula en la poblacion */
        for (liParticula = 0; liParticula < _iTamanoPoblacion; liParticula++)
        {
            /* Si la particula en la poblacion es no dominada */
            if (VerificaFactibilidadNoDominanciaParticulasPoblacion(liParticula))
            {
                /* Si la memoria no esta llena, insertar particula */
                if (_iNumNoDominados < _iTamanoArchivo)
                {
                    lii = _iNumNoDominados;
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesArchivo[lii, lij] = _doaVariablesPoblacion[liParticula, lij];
                    for (lij = 0; lij < _iNumFunObjetivo; lij++)
                        _doaAptitudArchivo[lii, lij] = _doaAptitudPoblacion[liParticula, lij];
                    _iNumNoDominados += 1;
                }
                else
                {      // Calcular hipervolumen
                    if (_bAlgoritmoLimitado)
                    {
                        for (lij = 0; lij < _iNumVariables; lij++)
                            _doaVariablesArchivo[liPosicionPeorParticula, lij] = _doaVariablesPoblacion[liParticula, lij];
                        for (lij = 0; lij < _iNumFunObjetivo; lij++)
                            _doaAptitudArchivo[liPosicionPeorParticula, lij] = _doaAptitudPoblacion[liParticula, lij];
                        for (lii = 0, liTotalElementosDeMatriz = 0; lii < _iNumNoDominados; lii++)
                            for (lij = 0; lij < _iNumFunObjetivo; lij++)
                            {
                                _doaAptitudArchivoArreglo[liTotalElementosDeMatriz] = _doaAptitudArchivo[lii, lij];
                                liTotalElementosDeMatriz++;
                            }
                        _oHype.hypeIndicator(_doaContribucionHipervolumen, _iNumNoDominados, _doaPuntosReferencia, _iparam_k, _doaAptitudArchivoArreglo, _iNumFunObjetivo);
                        Ldo_ContribucionPeorParticula = _doaContribucionHipervolumen[0];
                        liPosicionPeorParticula = 0;
                        for (lii = 1; lii < _iNumNoDominados; lii++)
                        {
                            if (_doaContribucionHipervolumen[lii] < Ldo_ContribucionPeorParticula)
                            {
                                Ldo_ContribucionPeorParticula = _doaContribucionHipervolumen[lii];
                                liPosicionPeorParticula = lii;
                            }
                        }
                    }
                    else
                    {
                        liPosicionMenorParticula = (int)((_iNumNoDominados - 1) * 0.90);
                        /* Selecionar aleatoriamente un lugar para reemplazar */
                        liPosicionAleatoria = _oAleatorios.RandomInt(liPosicionMenorParticula, _iNumNoDominados - 1);
                        for (lij = 0; lij < _iNumVariables; lij++)
                            _doaVariablesArchivo[liPosicionAleatoria, lij] = _doaVariablesPoblacion[liParticula, lij];
                        for (lij = 0; lij < _iNumFunObjetivo; lij++)
                            _doaAptitudArchivo[liPosicionAleatoria, lij] = _doaAptitudPoblacion[liParticula, lij];
                    }
                }
            }
        }
        if (_oTipoFunction == FuncionObjetivo.zdt6)
            for (lii = 0; lii < _iNumNoDominados; lii++)
            {
                if (_doaVariablesArchivo[lii, 1] > 1.0)
                {       /* mover la ultima particula en el lugar de la particula infactible */
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesArchivo[lii, lij] = _doaVariablesArchivo[_iNumNoDominados - 1, lij];
                    for (lij = 0; lij < _iNumFunObjetivo; lij++)
                        _doaAptitudArchivo[lii, lij] = _doaAptitudArchivo[_iNumNoDominados - 1, lij];
                    _iNumNoDominados -= 1;
                }
            }
    }

    bool VerificaFactibilidadNoDominanciaParticulasPoblacion(int piParticula) /* Check for feasibility and nondomination of particles in pop and archive */
    {
        int liTotalNoFactibilidad;
        int lii;
        int lij;
        double[] Ldoa_AuxiliaVariablesArchivo = new double[_iNumVariables];
        double[] Ldoa_AuxiliarVariablesPoblacion = new double[_iNumVariables];
        lii = 0;
        do
        {
            liTotalNoFactibilidad = 0;
            for (lij = 0; lij < _iNumVariables; lij++)
            {
                Ldoa_AuxiliaVariablesArchivo[lij] = _doaVariablesArchivo[lii, lij];
                Ldoa_AuxiliarVariablesPoblacion[lij] = _doaVariablesPoblacion[piParticula, lij];
            }
            /* Si la particula en el archivo tiene menos restricciones violadas */
            if (VerificarRestriccionFactibilidad(Ldoa_AuxiliaVariablesArchivo) < VerificarRestriccionFactibilidad(Ldoa_AuxiliarVariablesPoblacion))
                return false;
            /* Si la particula tiene mas restriccion, eliminar*/
            if (VerificarRestriccionFactibilidad(Ldoa_AuxiliaVariablesArchivo) > VerificarRestriccionFactibilidad(Ldoa_AuxiliarVariablesPoblacion))
            {
                for (lij = 0; lij < _iNumVariables; lij++)
                    _doaVariablesArchivo[lii, lij] = _doaVariablesArchivo[_iNumNoDominados - 1, lij];
                for (lij = 0; lij < _iNumFunObjetivo; lij++)
                    _doaAptitudArchivo[lii, lij] = _doaAptitudArchivo[_iNumNoDominados - 1, lij];
                _iNumNoDominados -= 1;
            }
            else /*Si tienen las mismas violacion de restricciones*/
            {
                for (lij = 0; lij < _iNumFunObjetivo; lij++)
                {
                    if (((_doaAptitudArchivo[lii, lij] < _doaAptitudPoblacion[piParticula, lij]) && (_oOptimization == 0)) || ((_doaAptitudArchivo[lii, lij] > _doaAptitudPoblacion[piParticula, lij]) && (_oOptimization == Optimizacion.MAXIMIZACION)))
                        liTotalNoFactibilidad += 1;
                }
                if (liTotalNoFactibilidad == _iNumFunObjetivo)	/* Si la particula en el archio es dominada */
                    return false;
                else if (liTotalNoFactibilidad == 0)
                {	/* Si la particula en el archivo es domianda borrar *///Revisar
                    for (lij = 0; lij < _iNumVariables; lij++)
                        _doaVariablesArchivo[lii, lij] = _doaVariablesArchivo[_iNumNoDominados - 1, lij];
                    for (lij = 0; lij < _iNumFunObjetivo; lij++)
                        _doaAptitudArchivo[lii, lij] = _doaAptitudArchivo[_iNumNoDominados - 1, lij];
                    _iNumNoDominados -= 1;
                }
                else
                {
                    lii += 1;
                }
            }
        } while (lii < _iNumNoDominados);
        return true;
    }

    void ObtenerRangoValoresVariables(double[] Pdoa_ValorMinimo, double[] Pdoa_ValorMaximo) /* Obtener rango de valores de las variables */
    {
        int lii;
        switch (_oTipoFunction)
        {
            case FuncionObjetivo.Kita:	/* Kita */
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Pdoa_ValorMinimo[lii] = 0.0;
                    Pdoa_ValorMaximo[lii] = 7.0;
                }
                break;
            case FuncionObjetivo.Kursawe:   /* Kursawe */
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Pdoa_ValorMinimo[lii] = -5.0;
                    Pdoa_ValorMaximo[lii] = 5.0;
                }
                break;
            case FuncionObjetivo.Deb1:
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Pdoa_ValorMinimo[lii] = 0.1;
                    Pdoa_ValorMaximo[lii] = 0.8191;
                }
                break;
            case FuncionObjetivo.Deb2:
            case FuncionObjetivo.Deb3:	/* Deb 2*/
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Pdoa_ValorMinimo[lii] = 0.0;
                    Pdoa_ValorMaximo[lii] = 1.0;
                }
                break;
            case FuncionObjetivo.Fonseca2:	/* Fonseca 2 */
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Pdoa_ValorMinimo[lii] = -4.0;
                    Pdoa_ValorMaximo[lii] = 4.0;
                }
                break;
            case FuncionObjetivo.zdt1:
            case FuncionObjetivo.zdt2:
            case FuncionObjetivo.zdt3: /* zdt*/
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Pdoa_ValorMinimo[lii] = -0.0;
                    Pdoa_ValorMaximo[lii] = 1.0;
                }
                break;
            case FuncionObjetivo.zdt4: /*zdt4*/
                Pdoa_ValorMinimo[0] = 0.0;
                Pdoa_ValorMaximo[0] = 1.0;
                for (lii = 1; lii < _iNumVariables; lii++)
                {
                    Pdoa_ValorMinimo[lii] = -5.0;
                    Pdoa_ValorMaximo[lii] = 5.0;
                }
                break;
            case FuncionObjetivo.zdt6:	/* zdt6*/
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Pdoa_ValorMinimo[lii] = 0.0;
                    Pdoa_ValorMaximo[lii] = 1.0;
                }
                break;
            case FuncionObjetivo.DTLZ1:
            case FuncionObjetivo.DTLZ2:
            case FuncionObjetivo.DTLZ3:
            case FuncionObjetivo.DTLZ4:
            case FuncionObjetivo.DTLZ5:	/* dtlz*/
            case FuncionObjetivo.DTLZ6:
            case FuncionObjetivo.DTLZ7:
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Pdoa_ValorMinimo[lii] = 0.0;
                    Pdoa_ValorMaximo[lii] = 1.0;
                }
                break;
            //case 800: case 805: case 810: case 815: case 821:/* zdt2_2 */
            //case 831:
            // case 841:
            // case 851:
            // case 861:/* zdt2_2 */

            //    for(i=0; i < maxvar; i++)
            //      {
            //           minvalue[i] = 0.0;
            //           maxvalue[i] = 1.0;
            //      }
            //           break;
        }
    }

    public void MantenerParticulasPoblacionEspacioBusqueda()  /* Mantener las particulas de la poblacion en el espacio de busqueda */
    {
        int lii, lij;
        double[] Ldoa_ValorMinimo = new double[_iNumVariables];
        double[] Ldoa_ValorMaximo = new double[_iNumVariables];
        //
        /* Verifica la estabilidad de la poblacion al mantener las partículas en sus fronteras */
        switch (_oTipoFunction)
        {
            case FuncionObjetivo.Kita: /* Kita */
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Ldoa_ValorMinimo[lii] = 0.0;
                    Ldoa_ValorMaximo[lii] = 7.0;
                }
                break;
            case FuncionObjetivo.Kursawe: /* Kursawe */
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Ldoa_ValorMinimo[lii] = -5.0;
                    Ldoa_ValorMaximo[lii] = 5.0;
                }
                break;
            case FuncionObjetivo.Deb1:
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Ldoa_ValorMinimo[lii] = 0.1;
                    Ldoa_ValorMaximo[lii] = 0.8191;
                }
                break;
            case FuncionObjetivo.Deb2:
            case FuncionObjetivo.Deb3:/* Deb */
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Ldoa_ValorMinimo[lii] = 0.0;
                    Ldoa_ValorMaximo[lii] = 1.0;
                }
                break;
            case FuncionObjetivo.Fonseca2:	/* Fonseca 2 */
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Ldoa_ValorMinimo[lii] = -4.0;
                    Ldoa_ValorMaximo[lii] = 4.0;
                }
                break;
            case FuncionObjetivo.zdt1:
            case FuncionObjetivo.zdt2:
            case FuncionObjetivo.zdt3: /* zdt*/
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Ldoa_ValorMinimo[lii] = -0.0;
                    Ldoa_ValorMaximo[lii] = 1.0;
                }
                break;
            case FuncionObjetivo.zdt4:
                Ldoa_ValorMinimo[0] = 0.0;
                Ldoa_ValorMaximo[0] = 1.0;
                for (lii = 1; lii < _iNumVariables; lii++)
                {
                    Ldoa_ValorMinimo[lii] = -5.0;
                    Ldoa_ValorMaximo[lii] = 5.0;
                }
                break;
            case FuncionObjetivo.zdt6:	/* zdt6*/
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Ldoa_ValorMinimo[lii] = 0.0;
                    Ldoa_ValorMaximo[lii] = 1.0;
                }
                break;
            case FuncionObjetivo.DTLZ1:
            case FuncionObjetivo.DTLZ2:
            case FuncionObjetivo.DTLZ3:
            case FuncionObjetivo.DTLZ4:
            case FuncionObjetivo.DTLZ5:
            case FuncionObjetivo.DTLZ6:
            case FuncionObjetivo.DTLZ7:
                for (lii = 0; lii < _iNumVariables; lii++)
                {
                    Ldoa_ValorMinimo[lii] = 0.0;
                    Ldoa_ValorMaximo[lii] = 1.0;
                }
                break;
            //   case 800: case 805: case 810: case 815: case 821:/* zdt2_2 */
            //   case 831:
            //case 841:
            //case 851:
            //case 861:/* zdt2_2 */

            //          for(i = 0; i < maxvar; i++)
            //       {
            //           minvalue[i] = 0.0;
            //           maxvalue[i] = 1.0;
            //       }
            //       break;
            /** Agregar mas aqui! **/
        }
        for (lii = 0; lii < _iTamanoPoblacion; lii++)
        {
            for (lij = 0; lij < _iNumVariables; lij++)
            {
                /* Si la particula se va mas lejos del valor minimo */
                if (_doaVariablesPoblacion[lii, lij] < Ldoa_ValorMinimo[lij])
                {
                    /* HAcer igual al valor minimo */
                    _doaVariablesPoblacion[lii, lij] = Ldoa_ValorMinimo[lij];
                    /* Cambiar la direccion de manera opuesta */
                    _doaVelocidadParticulas[lii, lij] = -_doaVelocidadParticulas[lii, lij];
                    /*cambiar la velocidad de la particula a cero*/
                    //velocity[i,j] = 0;
                }
                /* Si la particula se va mas lejos del valor maximo */
                if (_doaVariablesPoblacion[lii, lij] > Ldoa_ValorMaximo[lij])
                {
                    /* Hacer igual al valor maximo*/
                    _doaVariablesPoblacion[lii, lij] = Ldoa_ValorMaximo[lij];
                    /* Cambiar la direccion de manera opuesta */
                    _doaVelocidadParticulas[lii, lij] = -_doaVelocidadParticulas[lii, lij];
                    /*cambiar la velocidad de la particula a cero*/
                    //velocity[i,j] = 0;
                }
            }
        }
    }

    void ActualizaPBestsPoblacion() /* Update personal bests of particles in the population */
    {
        int lii;
        int lij;
        int liTotalMejorParticula;
        int liPosicionMejorParticula;

        for (lii = 0; lii < _iTamanoPoblacion; lii++)
        {
            liTotalMejorParticula = 0;
            for (lij = 0; lij < _iNumFunObjetivo; lij++)
            {
                if (((_doaAptitudPoblacion[lii, lij] < _doaAptitudPbests[lii, lij]) && (_oOptimization == 0)) || ((_doaAptitudPoblacion[lii, lij] > _doaAptitudPbests[lii, lij]) && (_oOptimization == Optimizacion.MAXIMIZACION)))
                    liTotalMejorParticula += 1;
            }
            if (liTotalMejorParticula == _iNumFunObjetivo)
            {
                liPosicionMejorParticula = 0;
            }
            //&& pContribution[i] < bpContribution[i])
            else
            {
                if (liTotalMejorParticula == 0)//&& pContribution[i] > bpContribution[i])
                    liPosicionMejorParticula = 1;
                else
                    liPosicionMejorParticula = _oAleatorios.RandomInt(0, 1);
            }
            if (liPosicionMejorParticula == 0)
            {
                for (lij = 0; lij < _iNumFunObjetivo; lij++)
                    _doaAptitudPbests[lii, lij] = _doaAptitudPoblacion[lii, lij];
                for (lij = 0; lij < _iNumVariables; lij++)
                    _doaVariablesPbests[lii, lij] = _doaVariablesPoblacion[lii, lij];
            }
        }
    }

    void save_results(char[] archiveName)  /* Escribir resultados en el archivo */
    {
        //  int i, j;
        // FILE *fp;
        ///* Abrir archivo para escritura */
        // fp = fopen(archiveName, "w");
        // for(i = 0; i < nondomCtr; i++)
        // {
        //      for(j = 0; j < maxfun; j++)
        //           fprintf(fp, "%e ", archiveFit[i,j]);
        //      fprintf(fp, "\n");
        // }
        // fclose(fp);
    }

    void OrdenarParticulaArchivoconMejorAptitudqsort(int pif, int piInicio, int piUltimaParte) /* Sort fitness values of particles in archive */
    {
        int lil = piInicio + 1;
        int lir = piUltimaParte;
        double Ldo_Pivote = _doaAptitudArchivo[piInicio, pif];
        int lik;
        double[] Ldoa_tempF = new double[_iNumFunObjetivo];
        double[] Loda_tempP = new double[_iNumVariables];
        double Ldo_temp;

        while (lil < lir)
        {
            if (_doaAptitudArchivo[lil, pif] <= Ldo_Pivote)
                lil++;
            else
            {
                lir--;
                /* swap */
                /* Exchange fitness positions of two particles in the archiveFit */
                for (lik = 0; lik < _iNumFunObjetivo; lik++)
                {
                    Ldoa_tempF[lik] = _doaAptitudArchivo[lil, lik];
                    _doaAptitudArchivo[lil, lik] = _doaAptitudArchivo[lir, lik];
                }
                for (lik = 0; lik < _iNumFunObjetivo; lik++)
                    _doaAptitudArchivo[lir, lik] = Ldoa_tempF[lik];
                /* Also exchange particle positions in archiveVar */
                for (lik = 0; lik < _iNumVariables; lik++)
                {
                    Loda_tempP[lik] = _doaVariablesArchivo[lil, lik];
                    _doaVariablesArchivo[lil, lik] = _doaVariablesArchivo[lir, lik];
                }
                for (lik = 0; lik < _iNumVariables; lik++)
                    _doaVariablesArchivo[lir, lik] = Loda_tempP[lik];
                /* Also exchange their crowding distance */
                Ldo_temp = _doaContribucionHipervolumen[lil];
                _doaContribucionHipervolumen[lil] = _doaContribucionHipervolumen[lir];
                _doaContribucionHipervolumen[lir] = Ldo_temp;
            }
        }
        lil--;
        /* Exchange fitness positions of two particles in the array archiveFit */
        for (lik = 0; lik < _iNumFunObjetivo; lik++)
        {
            Ldoa_tempF[lik] = _doaAptitudArchivo[piInicio, lik];
            _doaAptitudArchivo[piInicio, lik] = _doaAptitudArchivo[lil, lik];
        }
        for (lik = 0; lik < _iNumFunObjetivo; lik++)
            _doaAptitudArchivo[lil, lik] = Ldoa_tempF[lik];
        /* Also exchange particle positions in archiveVar */
        for (lik = 0; lik < _iNumVariables; lik++)
        {
            Loda_tempP[lik] = _doaVariablesArchivo[piInicio, lik];
            _doaVariablesArchivo[piInicio, lik] = _doaVariablesArchivo[lil, lik];
        }
        for (lik = 0; lik < _iNumVariables; lik++)
            _doaVariablesArchivo[lil, lik] = Loda_tempP[lik];
        /* Also exchange their crowding distance */
        Ldo_temp = _doaContribucionHipervolumen[piInicio];
        _doaContribucionHipervolumen[piInicio] = _doaContribucionHipervolumen[lil];
        _doaContribucionHipervolumen[lil] = Ldo_temp;
        if (lil - piInicio > 1)
            OrdenarParticulaArchivoconMejorAptitudqsort(pif, piInicio, lil);
        if (piUltimaParte - lir > 1)
            OrdenarParticulaArchivoconMejorAptitudqsort(pif, lir, piUltimaParte);
    }

    void OrdenarParticulasContribucionHipervolumen(int pibegin, int pilastPart) /* Sort crowding distance values */
    {
        int lil = pibegin + 1;
        int lir = pilastPart;
        double Ldo_Pivot = _doaContribucionHipervolumen[pibegin];
        int lik;
        double Ldo_temp;
        double[] Loda_tempP = new double[_iNumVariables];
        double[] Ldoa_tempF = new double[_iNumFunObjetivo];

        while (lil < lir)
        {
            if (_doaContribucionHipervolumen[lil] >= Ldo_Pivot)
                lil++;
            else
            {
                lir--;
                /* exchange their crowding distance values */
                Ldo_temp = _doaContribucionHipervolumen[lil];
                _doaContribucionHipervolumen[lil] = _doaContribucionHipervolumen[lir];
                _doaContribucionHipervolumen[lir] = Ldo_temp;
                /* Exchange fitness positions of two particles in the array archiveFit */
                for (lik = 0; lik < _iNumFunObjetivo; lik++)
                {
                    Ldoa_tempF[lik] = _doaAptitudArchivo[lil, lik];
                    _doaAptitudArchivo[lil, lik] = _doaAptitudArchivo[lir, lik];
                }
                for (lik = 0; lik < _iNumFunObjetivo; lik++)
                    _doaAptitudArchivo[lir, lik] = Ldoa_tempF[lik];
                /* Also exchange particle positions in archiveVar */
                for (lik = 0; lik < _iNumVariables; lik++)
                {
                    Loda_tempP[lik] = _doaVariablesArchivo[lil, lik];
                    _doaVariablesArchivo[lil, lik] = _doaVariablesArchivo[lir, lik];
                }
                for (lik = 0; lik < _iNumVariables; lik++)
                    _doaVariablesArchivo[lir, lik] = Loda_tempP[lik];
            }
        }
        lil--;
        /* Exchange fitness positions of two particles in the array archiveVar */
        for (lik = 0; lik < _iNumFunObjetivo; lik++)
        {
            Ldoa_tempF[lik] = _doaAptitudArchivo[pibegin, lik];
            _doaAptitudArchivo[pibegin, lik] = _doaAptitudArchivo[lil, lik];
        }
        for (lik = 0; lik < _iNumFunObjetivo; lik++)
            _doaAptitudArchivo[lil, lik] = Ldoa_tempF[lik];
        /* Also exchange particle positions in archiveVar */
        for (lik = 0; lik < _iNumVariables; lik++)
        {
            Loda_tempP[lik] = _doaVariablesArchivo[pibegin, lik];
            _doaVariablesArchivo[pibegin, lik] = _doaVariablesArchivo[lil, lik];
        }
        for (lik = 0; lik < _iNumVariables; lik++)
            _doaVariablesArchivo[lil, lik] = Loda_tempP[lik];
        /* Also exchange their crowding distance */
        Ldo_temp = _doaContribucionHipervolumen[pibegin];
        _doaContribucionHipervolumen[pibegin] = _doaContribucionHipervolumen[lil];
        _doaContribucionHipervolumen[lil] = Ldo_temp;
        if (lil - pibegin > 1)
            OrdenarParticulasContribucionHipervolumen(pibegin, lil);
        if (pilastPart - lir > 1)
            OrdenarParticulasContribucionHipervolumen(lir, pilastPart);
    }

    /****** test problems *******/

    void kita(int piParticula)
    {
        _doaAptitudPoblacion[piParticula, 0] = -(_doaVariablesPoblacion[piParticula, 0] * _doaVariablesPoblacion[piParticula, 0]) + _doaVariablesPoblacion[piParticula, 1];
        _doaAptitudPoblacion[piParticula, 1] = (_doaVariablesPoblacion[piParticula, 0] / 2.0) + _doaVariablesPoblacion[piParticula, 1] + 1;
    }

    void kursawe(int piParticula)
    {
        double Ldo_r = 0.0;
        int lij;
        for (lij = 0; lij < 2; lij++)
            Ldo_r += -10.0 * Math.Exp(-0.2 * Math.Sqrt(Math.Pow(_doaVariablesPoblacion[piParticula, lij], 2) + Math.Pow(_doaVariablesPoblacion[piParticula, lij + 1], 2)));
        _doaAptitudPoblacion[piParticula, 0] = Ldo_r;
        Ldo_r = 0.0;
        for (lij = 0; lij < 3; lij++)
            Ldo_r += Math.Pow(Math.Abs(_doaVariablesPoblacion[piParticula, lij]), 0.8) + 5.0 * Math.Sin(Math.Pow(_doaVariablesPoblacion[piParticula, lij], 3));
        _doaAptitudPoblacion[piParticula, 1] = Ldo_r;
    }

    void deb_1(int piParticula)
    {
        double Ldo_g = 2.0 - Math.Exp(-Math.Pow(((_doaVariablesPoblacion[piParticula, 1] - 0.2) / 0.004), 2)) - 0.8 * Math.Exp(-Math.Pow(((_doaVariablesPoblacion[piParticula, 1] - 0.6) / 0.4), 2));
        _doaAptitudPoblacion[piParticula, 0] = _doaVariablesPoblacion[piParticula, 0];
        _doaAptitudPoblacion[piParticula, 1] = (double)Ldo_g / _doaVariablesPoblacion[piParticula, 0];
    }

    void deb_2(int piParticula)
    {
        double Ldo_g;
        double Ldo_h;
        double Ldo_f1;

        Ldo_f1 = _doaVariablesPoblacion[piParticula, 0];
        Ldo_g = 1.0 + 10.0 * _doaVariablesPoblacion[piParticula, 1];
        Ldo_h = 1.0 - Math.Pow((Ldo_f1 / Ldo_g), 2.0) - (Ldo_f1 / Ldo_g) * Math.Sin(12.0 * _doPI * Ldo_f1);

        _doaAptitudPoblacion[piParticula, 0] = Ldo_f1;
        _doaAptitudPoblacion[piParticula, 1] = Ldo_g * Ldo_h;
    }

    void deb_3(int piParticula)
    {
        double Ldo_g;
        double Ldo_h;
        double Ldo_f1;
        double Ldo_alfa;
        double Ldo_beta;
        double Ldo_seno;
        //double pi = 3.1415926535;
        double Ldo_argumento;
        double Ldo_q;

        Ldo_alfa = 10.0;
        Ldo_q = 10.0;
        Ldo_beta = 1.0;

        Ldo_argumento = Ldo_q * _doPI * _doaVariablesPoblacion[piParticula, 0];
        Ldo_seno = Math.Sin(Ldo_argumento);
        Ldo_f1 = 1.0 - Math.Exp(-4.0 * _doaVariablesPoblacion[piParticula, 0]) * Math.Pow(Ldo_seno, 4);
        Ldo_g = 1.0 + _doaVariablesPoblacion[piParticula, 1] * _doaVariablesPoblacion[piParticula, 1];
        if (Ldo_f1 <= Ldo_beta * Ldo_g)
            Ldo_h = 1.0 - Math.Pow((Ldo_f1 / (Ldo_beta * Ldo_g)), Ldo_alfa);
        else
            Ldo_h = 0.0;
        _doaAptitudPoblacion[piParticula, 0] = Ldo_f1;
        _doaAptitudPoblacion[piParticula, 1] = Ldo_g * Ldo_h;
    }

    void fonseca_2(int piParticula)
    {
        double Ldo_s1, Ldo_s2;
        int lij;
        Ldo_s1 = Ldo_s2 = 0.0;
        for (lij = 0; lij < _iNumVariables; lij++)
        {
            Ldo_s1 += Math.Pow((_doaVariablesPoblacion[piParticula, lij] - (1.0 / Math.Sqrt((double)_iNumVariables))), 2.0);
            Ldo_s2 += Math.Pow((_doaVariablesPoblacion[piParticula, lij] + (1.0 / Math.Sqrt((double)_iNumVariables))), 2.0);
        }
        _doaAptitudPoblacion[piParticula, 0] = 1.0 - Math.Exp(-Ldo_s1);
        _doaAptitudPoblacion[piParticula, 1] = 1.0 - Math.Exp(-Ldo_s2);

    }

    /*****test problem ZDT*****/

    void ZDT1(int i)
    {
        double f1, f2, g, h, sum;
        int j;

        f1 = _doaVariablesPoblacion[i, 0];
        sum = 0.0;
        for (j = 1; j < _iNumVariables; j++)
        {
            sum += _doaVariablesPoblacion[i, j];
        }
        g = 1.0 + 9.0 * sum / ((double)_iNumVariables - 1.0);
        h = 1.0 - Math.Sqrt(f1 / g);
        f2 = g * h;
        _doaAptitudPoblacion[i, 0] = f1;
        _doaAptitudPoblacion[i, 1] = f2;

    }

    void ZDT2(int i)
    {
        double f1, f2, g, h, sum;
        int j;

        f1 = _doaVariablesPoblacion[i, 0];
        sum = 0.0;
        for (j = 1; j < _iNumVariables; j++)
        {
            sum += _doaVariablesPoblacion[i, j];
        }
        g = 1.0 + 9.0 * sum / ((double)_iNumVariables - 1.0);
        h = 1.0 - Math.Pow((f1 / g), 2.0);
        f2 = g * h;
        _doaAptitudPoblacion[i, 0] = f1;
        _doaAptitudPoblacion[i, 1] = f2;

    }

    void ZDT3(int i)
    {
        double f1, f2, g, h, sum;
        int j;

        f1 = _doaVariablesPoblacion[i, 0];
        sum = 0.0;
        for (j = 1; j < _iNumVariables; j++)
        {
            sum += _doaVariablesPoblacion[i, j];
        }
        g = 1.0 + 9.0 * sum / ((double)_iNumVariables - 1.0);
        h = 1.0 - Math.Sqrt(f1 / g) - (f1 / g) * Math.Sin(10.0 * _doPI * f1);
        f2 = g * h;
        _doaAptitudPoblacion[i, 0] = f1;
        _doaAptitudPoblacion[i, 1] = f2;

    }

    void ZDT4(int i)
    {
        double f1, f2, g, h, sum;
        int j;

        f1 = _doaVariablesPoblacion[i, 0];
        sum = 0.0;
        for (j = 1; j < _iNumVariables; j++)
        {
            sum += (Math.Pow(_doaVariablesPoblacion[i, j], 2.0) - 10.0 * Math.Cos(4.0 * _doPI * _doaVariablesPoblacion[i, j]));
        }
        g = 1.0 + 10.0 * ((double)_iNumVariables - 1.0) + sum;
        h = 1.0 - Math.Sqrt(f1 / g);
        f2 = g * h;
        _doaAptitudPoblacion[i, 0] = f1;
        _doaAptitudPoblacion[i, 1] = f2;

    }

    void ZDT6(int i)
    {
        double f1, f2, g, h, sum;
        int j;

        f1 = 1.0 - Math.Exp(-4.0 * _doaVariablesPoblacion[i, 0]) * Math.Pow(Math.Sin(6.0 * _doPI * _doaVariablesPoblacion[i, 0]), 6.0);
        sum = 0.0;
        for (j = 1; j < _iNumVariables; j++)
        {
            sum += _doaVariablesPoblacion[i, j];
        }
        g = 1.0 + 9.0 * Math.Pow((sum / ((double)_iNumVariables - 1.0)), 0.25);
        h = 1.0 - Math.Pow((f1 / g), 2.0);
        f2 = g * h;
        _doaAptitudPoblacion[i, 0] = f1;
        _doaAptitudPoblacion[i, 1] = f2;
        _doaAptitudPoblacion[i, 1] = f2;

    }


    /***** problem test DTLZ*****/

    void DTLZ1(int i)
    {
        int j;
        double sum;
        double g;
        int n = _iNumVariables;
        int m = _iNumFunObjetivo;
        int k = n - m + 1;

        sum = 0.0;
        for (j = m - 1; j < n; j++)
        {
            sum += Math.Pow(_doaVariablesPoblacion[i, j] - 0.5, 2.0) - Math.Cos(20.0 * _doPI * (_doaVariablesPoblacion[i, j] - 0.5));
        }
        g = 100.0 * (k + sum);

        _doaAptitudPoblacion[i, 0] = (0.5 * (1.0 + g) * _doaVariablesPoblacion[i, 0] * _doaVariablesPoblacion[i, 1]);
        _doaAptitudPoblacion[i, 1] = 0.5 * (1.0 + g) * (1.0 - _doaVariablesPoblacion[i, 1]) * _doaVariablesPoblacion[i, 0];
        _doaAptitudPoblacion[i, 2] = 0.5 * (1.0 + g) * (1.0 - _doaVariablesPoblacion[i, 0]);

    }

    void DTLZ2(int i)
    {
        int j;
        double sum;
        double g;
        int n = _iNumVariables;
        int m = _iNumFunObjetivo;

        sum = 0.0;
        for (j = m - 1; j < n; j++)
        {
            sum += Math.Pow(_doaVariablesPoblacion[i, j] - 0.5, 2.0);
        }
        g = sum;

        _doaAptitudPoblacion[i, 0] = (1.0 + g) * Math.Cos(_doaVariablesPoblacion[i, 0] * _doPI * 0.5) * Math.Cos(_doaVariablesPoblacion[i, 1] * _doPI * 0.5);
        _doaAptitudPoblacion[i, 1] = (1.0 + g) * Math.Cos(_doaVariablesPoblacion[i, 0] * _doPI * 0.5) * Math.Sin(_doaVariablesPoblacion[i, 1] * _doPI * 0.5);
        _doaAptitudPoblacion[i, 2] = (1.0 + g) * Math.Sin(_doaVariablesPoblacion[i, 0] * _doPI * 0.5);

    }

    void DTLZ3(int i)
    {
        int j;
        double sum;
        double g;
        int n = _iNumVariables;
        int m = _iNumFunObjetivo;
        int k = n - m + 1;

        sum = 0.0;
        for (j = m - 1; j < n; j++)
        {
            sum += Math.Pow(_doaVariablesPoblacion[i, j] - 0.5, 2.0) - Math.Cos(20.0 * _doPI * (_doaVariablesPoblacion[i, j] - 0.5));
        }
        g = 100.0 * (k + sum);

        _doaAptitudPoblacion[i, 0] = (1.0 + g) * Math.Cos(_doaVariablesPoblacion[i, 0] * _doPI * 0.5) * Math.Cos(_doaVariablesPoblacion[i, 1] * _doPI * 0.5);
        _doaAptitudPoblacion[i, 1] = (1.0 + g) * Math.Cos(_doaVariablesPoblacion[i, 0] * _doPI * 0.5) * Math.Sin(_doaVariablesPoblacion[i, 1] * _doPI * 0.5);
        _doaAptitudPoblacion[i, 2] = (1.0 + g) * Math.Sin(_doaVariablesPoblacion[i, 0] * _doPI * 0.5);

    }

    void DTLZ4(int i)
    {
        int j;
        double alpha;
        double g, factor1, factor2;
        int n = _iNumVariables;
        int m = _iNumFunObjetivo;

        alpha = 100;
        g = 0.0;
        for (j = m - 1; j < n; j++)
        {
            g += Math.Pow(_doaVariablesPoblacion[i, j] - 0.5, 2.0);
        }

        factor1 = Math.Pow(_doaVariablesPoblacion[i, 0], alpha);
        factor2 = Math.Pow(_doaVariablesPoblacion[i, 1], alpha);
        _doaAptitudPoblacion[i, 0] = (1.0 + g) * Math.Cos(factor1 * _doPI * 0.5) * Math.Cos(factor2 * _doPI * 0.5);
        _doaAptitudPoblacion[i, 1] = (1.0 + g) * Math.Cos(factor1 * _doPI * 0.5) * Math.Sin(factor2 * _doPI * 0.5);
        _doaAptitudPoblacion[i, 2] = (1.0 + g) * Math.Sin(factor1 * _doPI * 0.5);

    }

    void DTLZ5(int i)
    {
        int j;
        double g;
        double[] tetha = new double[3];
        int n = _iNumVariables;
        int m = _iNumFunObjetivo;

        g = 0.0;
        for (j = m - 1; j < n; j++)
        {
            g += Math.Pow(_doaVariablesPoblacion[i, j] - 0.5, 2.0);
        }

        tetha[0] = (_doPI * 0.5) * _doaVariablesPoblacion[i, 0];
        tetha[1] = (_doPI / (4.0 * (1.0 + g))) * (1.0 + 2.0 * g * _doaVariablesPoblacion[i, 1]);

        _doaAptitudPoblacion[i, 0] = (1.0 + g) * Math.Cos(tetha[0]) * Math.Cos(tetha[1]);
        _doaAptitudPoblacion[i, 1] = (1.0 + g) * Math.Cos(tetha[0]) * Math.Sin(tetha[1]);
        _doaAptitudPoblacion[i, 2] = (1.0 + g) * Math.Sin(tetha[0]);

    }

    void DTLZ6(int i)
    {
        int j;
        double g;
        double[] tetha = new double[3];
        int n = _iNumVariables;
        int m = _iNumFunObjetivo;

        g = 0.0;
        for (j = m - 1; j < n; j++)
        {
            g += Math.Pow(_doaVariablesPoblacion[i, j], 0.1);
        }

        tetha[0] = (_doPI * 0.5) * _doaVariablesPoblacion[i, 0];
        tetha[1] = (_doPI / (4.0 * (1.0 + g))) * (1.0 + 2.0 * g * _doaVariablesPoblacion[i, 1]);
        _doaAptitudPoblacion[i, 0] = (1.0 + g) * Math.Cos(tetha[0]) * Math.Cos(tetha[1]);
        _doaAptitudPoblacion[i, 1] = (1.0 + g) * Math.Cos(tetha[0]) * Math.Sin(tetha[1]);
        _doaAptitudPoblacion[i, 2] = (1.0 + g) * Math.Sin(tetha[0]);

    }

    void DTLZ7(int i)
    {
        int j;
        double sum;
        double g, h;
        int n = _iNumVariables;
        int m = _iNumFunObjetivo;
        int k = n - m + 1;

        sum = 0.0;
        for (j = m - 1; j < n; j++)
        {
            sum += _doaVariablesPoblacion[i, j];
        }
        g = 1.0 + (9.0 / (double)k) * sum;

        _doaAptitudPoblacion[i, 0] = _doaVariablesPoblacion[i, 0];
        _doaAptitudPoblacion[i, 1] = _doaVariablesPoblacion[i, 1];
        h = m - (((_doaAptitudPoblacion[i, 0] / (1.0 + g)) * (1.0 + Math.Sin(3.0 * _doPI * _doaAptitudPoblacion[i, 0]))) + ((_doaAptitudPoblacion[i, 1] / (1.0 + g)) * (1.0 + Math.Sin(3.0 * _doPI * _doaAptitudPoblacion[i, 1]))));
        _doaAptitudPoblacion[i, 2] = (1.0 + g) * h;

    }

    /*****DTLZ2 para escalamieto*****/
    void DTLZ2_Escala(int i)
    {
        int nbFun = _iNumFunObjetivo;
        int nbVar = _iNumVariables;
        int j, l, k;
        double g;
        double f;
        k = nbVar - nbFun + 1;
        g = 0.0;
        for (j = nbVar - k + 1; j <= nbVar; j++)
            g += Math.Pow(_doaVariablesPoblacion[i, j - 1] - 0.5, 2);
        for (j = 1; j <= nbFun; j++)
        {
            f = (1 + g);
            for (l = nbFun - j; l >= 1; l--)
                f *= Math.Cos(_doaVariablesPoblacion[i, l - 1] * _doPI / 2);
            if (j > 1)
                f *= Math.Sin(_doaVariablesPoblacion[i, (nbFun - j + 1) - 1] * _doPI / 2);
            _doaAptitudPoblacion[i, j - 1] = f;
        }
        return;
    }


}

