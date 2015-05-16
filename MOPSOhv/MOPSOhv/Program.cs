using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
     static void Main( string[] args )
     {
          MOPSOhv_ _oMOPSO;
          
          int t;
         // int fun, gen;
          //clock_t  startTime, endTime;
         // double duration;
         // double clocktime;
          /*Iniciar variables*/
          _oMOPSO = new MOPSOhv_(100, 100, 1, FuncionObjetivo.Deb1, 100, 0.5);
          //startTime = clock();
          /* Iniciar contador de generaciones*/
          t = 0;
          /* Iniciar valores de la poblacion de manera aleatoria*/
          _oMOPSO.InicializaPoblacion();
          /* Calcular velocidad inicial*/
          _oMOPSO.InicializaVelocidadParticulas();
          /* Evaluar las particulas de la poblacion*/
          _oMOPSO.EvaluaFuncionObjetivo();
          /* Almacenar el pBest inicial (variables and valor de aptitud) de las particulas*/
          //store_pbests();
          /* Insertar las particulas no domindas de la poblacion en el archivo*/
          _oMOPSO.InsertaParticulasNoDominadasArchivo();
          /*Ciclo Principal*/
          while(t <= 100)
          {
               //clocktime = (clock() - startTime)/(double)CLOCKS_PER_SEC;
               //if((verbose > 0 && t%printevery==0) || t == maxgen){
               //     fprintf(stdout,"Generation %d Time: %.2f sec\n",t,clocktime);
               //     fflush(stdout);
               //}
         /* Calcular la nueva velocidad de cada particula en la pooblacion*/
               _oMOPSO.CalculandoVelocidadParticulas();
         /* Calcular la nueva posicion de cada particula en la poblacion*/
               _oMOPSO.CalcularPosicionParticulas();
         /* Mantener las particulas en la poblacion de la poblacion en el espacio de busqueda*/
               _oMOPSO.MantenerParticulasPoblacionEspacioBusqueda();
         /* Pertubar las particulas en la poblacion*/
              if(t < 100 * 0.5)
                   _oMOPSO.PerturbacionParticulas(t);
         /* Evaluar las particulas en la poblacion*/
              _oMOPSO.EvaluaFuncionObjetivo();
         /* Insertar nuevas particulas no domindas en el archivo*/
              _oMOPSO.InsertaNuevasPartiuclasNoDominadsenArchivo();
         /* Actualizar el pBest de las particulas en la poblacion*/
               //update_pbests();
         /* Escribir resultados del mejor hasta ahora*/
         //verbose > 0 && t%printevery==0 || t == maxgen
               //if(t%printevery==0 || t == maxgen)
               //{
               //     //fprintf(outfile, "Size of Pareto Set: %d\n", nondomCtr);
               //     fprintf(fitfile, "%d\n",t);
               //     fprintf(varfile, "%d\n",t);
               //     for(i = 0; i < nondomCtr; i++)
               //     {
               //         for(j = 0; j < maxfun; j++)
               //          fprintf(fitfile, "%f ", archiveFit[i][j]);
               //      fprintf(fitfile, "\n");
               //     }
               //     fprintf(fitfile, "\n\n");
               //     fflush(fitfile);
               //     for(i = 0; i < nondomCtr; i++)
               //     {
               //         for(j = 0; j < maxfun; j++)
               //          fprintf(varfile, "%f ", archiveVar[i][j]);
               //      fprintf(varfile, "\n");
               //     }
               //     fprintf(varfile, "\n\n");
               //     fflush(varfile);
               //}
         /* Incrementar contador de generaciones*/
             t++;

          }
      /* Escribir resultados en el archivo */
          //save_results(archiveName);
          //endTime = clock();
          //duration = ( endTime - startTime ) / (double)CLOCKS_PER_SEC;
          //fprintf(stdout, "%lf sec\n", duration);
          //fclose(fitfile);
          //fclose(varfile);
          //free_memory();
          //return EXIT_SUCCESS;
         }
     }

