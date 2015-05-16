using System;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
namespace Solucionic.Framework.Utilerias
{
     public static class CorreoElectronico
     {

          static bool _binvalida = false;
          /// <summary>
          /// 
          /// </summary>
          /// <param name="pssubject"></param>
          /// <param name="psmessageBody"></param>
          /// <param name="psfromAddress"></param>
          /// <param name="pstoAddress"></param>
          /// <param name="psccAddress"></param>
          /// <param name="psHost"></param>
          /// <param name="psAttachment"></param>
          /// <param name="psPort"></param>
          /// <param name="psUserSMTP"></param>
          /// <param name="psPasswordSMTP"></param>
          /// <param name="pbSSL"></param>
          /// <param name="piTiempoSMTP"></param>
          /// <param name="psccoAddress"></param>
          public static void SendMessage( string pssubject
               , string psmessageBody
               , string psfromAddress
               , string pstoAddress
               , string psccAddress
               , string psHost
               , string[] psAttachment = null
               , string psPort = ""
               , string psUserSMTP = ""
               , string psPasswordSMTP = ""
               , bool pbSSL = false
               , int piTiempoSMTP = 100000
               , string psccoAddress = "" )
          {
               MailMessage lomessage = null;
               SmtpClient loclient = null;
               System.Net.NetworkCredential loSMTPUserInfo = null;
               int liIndex = 0;
               lomessage = new MailMessage();
               loclient = new SmtpClient();
               try
               {
                    //Set the sender's address
                    lomessage.From = new MailAddress(psfromAddress);
                    //Allow multiple "To" addresses to be separated by a semi-colon
                    if ((pstoAddress.Trim().Length > 0))                    
                         foreach (string addr in pstoAddress.Split(new char[] { ';' }))                         
                              lomessage.To.Add(new MailAddress(addr));                                             
                    //Allow multiple "Cc" addresses to be separated by a semi-colon
                    if ((psccAddress.Trim().Length > 0))                    
                         foreach (string addr in psccAddress.Split(';'))                         
                              lomessage.CC.Add(new MailAddress(addr));                                             
                    // agrega copia oculta para poder 
                    //verificar que se envién los correo electronicos pero sin que se den cuenta
                    //de la copia.
                    if ((psccoAddress.Trim().Length > 0))                    
                         foreach (string addr in psccoAddress.Split(';'))                         
                              lomessage.Bcc.Add(new MailAddress(addr));                                             
                    //Set the subject and message body text
                    lomessage.Subject = pssubject;
                    lomessage.Body = psmessageBody;
                    lomessage.BodyEncoding = System.Text.Encoding.UTF8;
                    if ((psAttachment != null))                    
                         for (liIndex = 0; liIndex <= psAttachment.GetUpperBound(0); liIndex++)                         
                              if (psAttachment[liIndex].Trim().Length > 0 & File.Exists(psAttachment[liIndex]))                              
                                   lomessage.Attachments.Add(new Attachment(psAttachment[liIndex]));                
                    //TODO: *** Modify for your SMTP server ***
                    //Set the SMTP server to be used to send the message
                    loclient.Host = psHost;
                    if (psPasswordSMTP.Trim().Length > 0 & psUserSMTP.Trim().Length > 0)
                    {
                         System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(validarCertificado);
                         loSMTPUserInfo = new System.Net.NetworkCredential(psUserSMTP, psPasswordSMTP);
                         loclient.UseDefaultCredentials = false;
                         loclient.Credentials = loSMTPUserInfo;
                    }
                    loclient.EnableSsl = pbSSL;
                    //Tiempo de espera para el servidor smtp al momento de enviar el msg
                    loclient.Timeout = piTiempoSMTP;
                    if (psPort.Trim().Length > 0)                    
                         loclient.Port = int.Parse(psPort);                    
                    //Send the e-mail message
                    loclient.Send(lomessage);
               }
               catch (ApplicationException ex)
               {
                    throw new ApplicationException("Se creó satisfactoriamente pero el correo no se pudo enviar por esta causa: " + ex.Message);
               }
               catch (Exception ex)
               {
                    throw new ApplicationException("Se creó satisfactoriamente pero el correo no se pudo enviar por esta causa: " + ex.Message);
               }
               finally
               {
                    if ((lomessage != null))
                    {
                         lomessage.Dispose();
                         lomessage = null;
                    }
                    if ((loclient != null))                    
                         loclient = null;
                    
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="subject"></param>
          /// <param name="messageBody"></param>
          /// <param name="fromAddress"></param>
          /// <param name="toAddress"></param>
          /// <param name="ccAddress"></param>
          /// <param name="Host"></param>
          public static void EnviarCorreoElectronico( string subject, string messageBody, string fromAddress, string toAddress, string ccAddress, string Host )
          {
               MailMessage message = new MailMessage();
               SmtpClient client = new SmtpClient();
               try
               {
                    //Set the sender's address
                    message.From = new MailAddress(fromAddress);
                    //Allow multiple "To" addresses to be separated by a semi-colon
                    if ((toAddress.Trim().Length > 0))                    
                         foreach (string addr in toAddress.Split(';'))                         
                              message.To.Add(new MailAddress(addr));                                             
                    //Allow multiple "Cc" addresses to be separated by a semi-colon
                    if ((ccAddress.Trim().Length > 0))                    
                         foreach (string addr in ccAddress.Split(';'))                         
                              message.CC.Add(new MailAddress(addr));                                             
                    //Set the subject and message body text
                    message.Subject = subject;
                    message.Body = messageBody;
                    //message.Attachments.Add(New Attachment("C:\WINDOWS\KARDEXESTADO.log"))
                    //TODO: *** Modify for your SMTP server ***
                    //Set the SMTP server to be used to send the message
                    client.Host = Host;
                    string Puerto = null;
                    Puerto = client.Port.ToString();
                    //Send the e-mail message
                    client.Send(message);
               }
               catch (ApplicationException ex)
               {
                    throw new ApplicationException(ex.Message);
               }
               catch (Exception ex)
               {
                    throw new ApplicationException(ex.Message);
               }
          }

          public static void EnviarMensajeConOutlook( string psDestinatario, string psCC, string psAsunto, string psMensaje, bool pbDesplegarMensaje, string psArchivoAnexo = "", object Po_OutApp = null )
          {
               ////Para que podamos meter esta función sin registrar outlook
               ////los declaro como object
               ////para hacer lo que se conoce como "Late Binding"
               ////Dim loOutApp As Object 'Outlook.Application
               //object loOutMailItem = null;
               ////Outlook.MailItem
               //object loOutRecip = null;
               ////Outlook.Recipient
               //object loOutAttach = null;
               ////Outlook.Attachment
               //bool Lb_InstancioLocalmente = false;


               //try {

               //    psArchivoAnexo = Strings.Trim(psArchivoAnexo);
               //    if (string.IsNullOrEmpty(psArchivoAnexo) || string.IsNullOrEmpty(FileSystem.Dir(psArchivoAnexo))) 
               //    {
               //        Microsoft.VisualBasic.Information.Err().Raise (Constants.vbObjectError, null, "Se requiere la ruta del archivo a anexar");

               //    }
               //    if (Po_OutApp == null) {
               //        // Create the Outlook session.
               //        Po_OutApp = Interaction.CreateObject("Outlook.Application");
               //        Lb_InstancioLocalmente = true;
               //    }
               //    // Create the message.
               //    loOutMailItem = Po_OutApp.CreateItem(0);


               //    //olMailItem

               //    var _with2 = loOutMailItem;

               //    // Add the To recipient(s) to the message.
               //    loOutRecip = _with2.Recipients.Add(psDestinatario);
               //    loOutRecip.Type = 1;
               //    //olTo

               //    // Add the CC recipient(s) to the message.
               //    psCC = Strings.Trim(psCC);
               //    if (!string.IsNullOrEmpty(psCC)) {
               //        loOutRecip = _with2.Recipients.Add(psCC);
               //        loOutRecip.Type = 2;
               //        //olCC
               //    }
               //    //        ' Add the BCC recipient(s) to the message.
               //    //Puede ser por nombre
               //    //         Set loOutRecip = .Recipients.Add("Andrew Fuller")
               //    //         loOutRecip.Type = olBCC

               //    // Set the Subject, Body, and Importance of the message.
               //    _with2.Subject = psAsunto;
               //    _with2.Body = psMensaje;
               //    _with2.Importance = 2;
               //    //olImportanceHigh  'High importance

               //    // Add attachments to the message.
               //    loOutAttach = _with2.Attachments.Add(psArchivoAnexo);

               //    // Resolve each Recipient's name.
               //    foreach (object loOutRecip_loopVariable in _with2.Recipients) {
               //        loOutRecip = loOutRecip_loopVariable;
               //        loOutRecip.Resolve();
               //    }

               //    // Should we display the message before sending?
               //    if (pbDesplegarMensaje) {
               //        _with2.Display();
               //    } else {
               //        _with2.Save();
               //        _with2.Send();
               //    }
               //} catch (Exception ex) {
               //    Interaction.MsgBox(ex.ToString());
               //} finally {
               //    loOutRecip = null;
               //    loOutAttach = null;
               //    loOutMailItem = null;
               //    if (Lb_InstancioLocalmente) {
               //        Po_OutApp = null;
               //    }
               //}
          }
          /// <summary>
          /// Comprobar si las cadenas tienen un formato de correo electrónico válido.
          /// Devuelve true si la cadena contiene una dirección de correo electrónico válida y false si no es válida, 
          /// pero no realiza ninguna otra acción. Para comprobar que la dirección de correo electrónico es válida, el 
          /// método llama al método Regex.IsMatch(String, String) para comprobar que la dirección cumple un modelo de 
          /// expresión regular. Puede usarse para filtrar las direcciones de correo electrónico que contienen caracteres no 
          /// válidos.
          /// http://msdn.microsoft.com/es-es/library/vstudio/01escwtf%28v=vs.100%29.aspx
          /// </summary>
          /// <param name="psCorreoElectronico">Correo Electronico</param>
          /// <returns>Correo Valido (true), correo Invalido (false)</returns>
          /// <remarks></remarks>
          public static bool EsCorreoValido( string psCorreoElectronico )
          {
               _binvalida = false;
               if (string.IsNullOrEmpty(psCorreoElectronico))               
                    return false;               
               //Use IdnMapping class to convert Unicode domain names.
               try
               {
                    psCorreoElectronico = System.Text.RegularExpressions.Regex.Replace(psCorreoElectronico, "(@)(.+)$", DomainMapper, RegexOptions.None);
               }
               catch
               {
                    return false;
               }
               if (_binvalida)               
                    return false;               
               // Return true if strIn is in valid e-mail format.
               try
               {
                    return Regex.IsMatch(psCorreoElectronico, "^(?(\")(\"[^\"]+?\"@)||(([0-9a-z]((\\.(?!\\.))||[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\||~\\w])*)(?<=[0-9a-z])@))" + "(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])||(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9]{2,17}))$", RegexOptions.IgnoreCase);
               }
               catch
               {
                    return false;
               }
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="Po_match"></param>
          /// <returns></returns>
          private static string DomainMapper( Match Po_match )
          {
               //IdnMapping class with default property values.
               IdnMapping loidn = new IdnMapping();
               string lsdomainName = Po_match.Groups[2].Value;
               try
               {
                    lsdomainName = loidn.GetAscii(lsdomainName);
               }
               catch
               {
                    _binvalida = true;
               }
               return Po_match.Groups[1].Value + lsdomainName;
          }
          /// <summary>
          /// 
          /// </summary>
          /// <param name="sender"></param>
          /// <param name="certificado"></param>
          /// <param name="cadena"></param>
          /// <param name="sslErrores"></param>
          /// <returns></returns>
          private static bool validarCertificado( object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificado, System.Security.Cryptography.X509Certificates.X509Chain cadena, System.Net.Security.SslPolicyErrors sslErrores )
          {
               return true;
          }
     }
}
