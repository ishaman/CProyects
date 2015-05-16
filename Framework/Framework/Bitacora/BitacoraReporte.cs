using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
namespace Solucionic.Framework.Bitacora
{
     public class BitacoraReporte
     {
          #region Variables
          private List<MensajeReporte> _oMensajes;
          #endregion

          #region Propiedades          
          public string RutaSalida { get; set; }
          public string NombreSalidaReporteBitacora
          {
               get
               {
                    return GeneraNombreArchivoSalida();
               }               
          }
          public string NombreReporteBitacora { get; set; }
          
         
          public string RazonSocialEmpresa { get; set; }
          public string NombreUsuario { get; set; }
          
          #endregion

          public BitacoraReporte(string psNombreReporte, string psRutaTMP)
          {
               _oMensajes = new List<MensajeReporte>();               
               NombreReporteBitacora = psNombreReporte;
               RutaSalida = psRutaTMP;
          }
          public BitacoraReporte()
          {
               _oMensajes = new List<MensajeReporte>();               
          }
          
          public void GrabaMensaje( string psTipo, string psMensaje )
          {
               MensajeReporte loRegistro;
               loRegistro = new MensajeReporte();
               loRegistro.MensajeId = psTipo;
               loRegistro.Comentario = psMensaje;
               _oMensajes.Add(loRegistro);
          }

          private Document ArmaDocumento()
          {
               OrdenaLista();
               return new Document(PageSize.LEGAL, 10, 10, 10, 10);
          }
          /// <summary>
          /// Metodo que genera la bitacora en memoria 
          /// </summary>
          /// <returns>regresa un archivo binrario de la bitacora</returns>
          public byte[] ImprimirArchivoBinario()
          {
               Document loDocumento;              
               if (_oMensajes.Count == 0)
                    return null;
               loDocumento = ArmaDocumento();
               using (var loMemoria = new MemoryStream())
               {
                    using (PdfWriter loWriter = PdfWriter.GetInstance(loDocumento, loMemoria))
                    {
                         loDocumento.Open();
                         //Agregando información del documento
                         EncabezadoReportePdf(loDocumento);
                         Resultados(loDocumento);
                         loDocumento.Close();                         
                    }
                    return  loMemoria.ToArray();
               }                
          }
          private string GeneraNombreArchivoSalida()
          {
               return NombreReporteBitacora + DateTime.Now.Year.ToString()
                    + DateTime.Now.Month.ToString()
                    + DateTime.Now.Day.ToString()
                    + DateTime.Now.Second.ToString()
                    + DateTime.Now.Millisecond.ToString()
                    + DateTime.Now.Ticks.ToString()                    
                    + ".pdf";
          }

          /// <summary>
          /// Metodo que escribe en disco la bitacora con un nombre especifco
          /// </summary>
          /// <returns></returns>
          public string Imprimir() 
          {
               string lsfileName;
               Document loDocumento;
               //PdfWriter writer;               
               string lsMensaje = "";
               lsfileName = "";               
               //Solo si hubo errores debe mostrar el reporte en caso contrario no
               if( _oMensajes.Count == 0 )
                    return lsfileName;                             
                    
                    
               if (String.IsNullOrEmpty(RutaSalida))
               {
                    if (System.IO.Directory.Exists(RutaSalida))
                         lsMensaje = "El directorio de salida " + RutaSalida + " del reporte no es valido";                              
                    throw new ApplicationException(lsMensaje);
               }
               lsfileName = RutaSalida + NombreReporteBitacora 
                    + DateTime.Now.Year.ToString()
                    + DateTime.Now.Month.ToString()
                    + DateTime.Now.Day.ToString()
                    + DateTime.Now.Second.ToString()
                    + DateTime.Now.Millisecond.ToString()
                    + DateTime.Now.Ticks.ToString()
                    //+ Guid.NewGuid().ToString() 
                    + ".pdf";
               loDocumento = ArmaDocumento();
               using (FileStream loArchivo = new FileStream(lsfileName, FileMode.Create))
               {
                    using (PdfWriter loWriter = PdfWriter.GetInstance(loDocumento, loArchivo))
                    {
                         loDocumento.Open();
                         //Agregando información del documento
                         EncabezadoReportePdf(loDocumento);
                         Resultados(loDocumento);
                         loDocumento.Close();
                         // writer.Close();
                    }
               }
               return lsfileName;                             
                              
          }

          private void OrdenaLista()
          {
               _oMensajes.Sort(delegate( MensajeReporte x, MensajeReporte y )
               {
                    if (x.MensajeId == null && y.MensajeId == null) 
                         return 0;
                    else if (x.MensajeId == null) 
                         return -1;
                    else if (y.MensajeId == null) 
                         return 1;
                    else 
                         return x.MensajeId.CompareTo(y.MensajeId);
               });
          }

          private void EncabezadoReportePdf( Document poDocumento )
          {
               poDocumento.AddTitle("Reporte de Bitácora");
               poDocumento.AddAuthor("Solución Integral Computarizada");
               iTextSharp.text.Font loStandardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

               PdfPTable tblPrueba = new PdfPTable(1);
               PdfPCell clTipo = new PdfPCell(new Phrase("Usuario: " + NombreUsuario, loStandardFont));
               tblPrueba.WidthPercentage = 100;
               clTipo.BorderWidth = 0;
               tblPrueba.AddCell(clTipo);
               clTipo = new PdfPCell(new Phrase("Reporte: " + NombreReporteBitacora, loStandardFont));
               clTipo.BorderWidth = 0;
               tblPrueba.AddCell(clTipo);
               clTipo = new PdfPCell(new Phrase("Empresa: " + RazonSocialEmpresa, loStandardFont));
               clTipo.BorderWidth = 0;
               tblPrueba.AddCell(clTipo);
               clTipo = new PdfPCell(new Phrase("Hora: " + DateTime.Now, loStandardFont));
               clTipo.BorderWidth = 0;
               tblPrueba.AddCell(clTipo);

               poDocumento.Add(tblPrueba);
               poDocumento.Add(Chunk.NEWLINE);           
               
          }

          private void Resultados(Document poDocumento)
          {
               PdfPTable tblPrueba;
               PdfPCell clTipo;
               PdfPCell clMensaje;
               string lsTipo;
               int liMensajes;
               int liTotal;
               float[] lfAnchoColumnas= {20,80};
               //TODO: Acomodar mejor el tamaño de las columnas del reporte (en el mensaje)
               //Retornar la linea donde trono y pq trono
               iTextSharp.text.Font loStandardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
               // Creamos una tabla que contendrá el nombre, apellido y país 
               // de nuestros visitante.
               tblPrueba = new PdfPTable(2);
               tblPrueba.WidthPercentage = 100;
               tblPrueba.CalculateHeights();
               tblPrueba.SpacingBefore = 1;
               //Asignamos el ancho de las columnas
               tblPrueba.SetWidths(lfAnchoColumnas);
               //tblPrueba.SetWidthPercentage(lfAnchoColumnas, poDocumento.PageSize);
               //tblPrueba.

               // Configuramos el título de las columnas de la tabla
               clTipo = new PdfPCell(new Phrase("Tipo", loStandardFont));
               //clTipo.Colspan = 2;
               clTipo.BorderWidth = 0;               
               clTipo.BorderWidthBottom = 0.75f;

               clMensaje = new PdfPCell(new Phrase("Mensaje", loStandardFont));
               clMensaje.BorderWidth = 0;
               clMensaje.BorderWidthBottom = 0.75f;
               
               tblPrueba.AddCell(clTipo);
               tblPrueba.AddCell(clMensaje);

               // Llenamos la tabla con información
               //Ordenar la lista con el mensaje e imprimir el tipo como un grupo
               
               lsTipo = _oMensajes[0].MensajeId;
               clTipo = new PdfPCell(new Phrase(_oMensajes[0].MensajeId, loStandardFont));
               clTipo.BorderWidth = 0;
               clMensaje = new PdfPCell(new Phrase(_oMensajes[0].Comentario, loStandardFont));
               clMensaje.BorderWidth = 0;
               tblPrueba.AddCell(clTipo);
               tblPrueba.AddCell(clMensaje);
               liTotal = 1;
               for (liMensajes = 1; liMensajes < _oMensajes.Count; liMensajes++)
               {
                    if (lsTipo.Equals(_oMensajes[liMensajes].MensajeId))
                    {
                         clTipo = new PdfPCell(new Phrase("", loStandardFont));
                         clTipo.BorderWidth = 0;
                         liTotal++;
                    }
                    else
                    {
                         //Agregamos el total de la columna
                         clTipo = new PdfPCell(new Phrase("", loStandardFont));
                         clTipo.BorderWidthBottom = 0.75f;
                         clTipo.BorderWidth = 0;
                         clMensaje = new PdfPCell(new Phrase("Total: " + liTotal, loStandardFont));
                         clMensaje.BorderWidthBottom = 0.75f;
                         clMensaje.BorderWidth = 0;
                         tblPrueba.AddCell(clTipo);
                         tblPrueba.AddCell(clMensaje);


                         clTipo = new PdfPCell(new Phrase(_oMensajes[liMensajes].MensajeId, loStandardFont));
                         clTipo.BorderWidth = 0;
                         lsTipo = _oMensajes[liMensajes].MensajeId;
                         liTotal = 1;
                         
                    }
                    clMensaje = new PdfPCell(new Phrase(_oMensajes[liMensajes].Comentario, loStandardFont));
                    clMensaje.BorderWidth = 0;
                    //clMensaje.BorderWidthBottom = 0.75f;
                    tblPrueba.AddCell(clTipo);
                    tblPrueba.AddCell(clMensaje);
               }
               //Valor del ultimo registro
               clTipo = new PdfPCell(new Phrase("", loStandardFont));
               clTipo.BorderWidthBottom = 0.75f;
               clTipo.BorderWidth = 0;
               clMensaje = new PdfPCell(new Phrase("Total: " + liTotal, loStandardFont));
               clMensaje.BorderWidthBottom = 0.75f;
               clMensaje.BorderWidth = 0;
               tblPrueba.AddCell(clTipo);
               tblPrueba.AddCell(clMensaje);               
               poDocumento.Add(tblPrueba);
          }
     }
}
