using System.Data.SqlClient;
namespace Solucionic.Framework.BaseDatos
{
     public static class Utilerias
     {
          public static string ObteneCadenadeConexion(string psNombreConexion)
          {
               SqlConnectionStringBuilder loCadConexion;
               string lsBDServidor;
               string lsBDNombre;
               string lsBDUsuario;
               string lsBDPassword;
               string lsCadenaConexion;
               lsCadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings[psNombreConexion].ConnectionString;
               if (lsCadenaConexion != null)
               {
                    loCadConexion = new SqlConnectionStringBuilder(lsCadenaConexion);
                    lsBDServidor = loCadConexion.DataSource;
                    lsBDNombre = loCadConexion.InitialCatalog;
                    lsBDUsuario = loCadConexion.UserID;
                    lsBDPassword = loCadConexion.Password;
                    lsCadenaConexion = "Persist Security Info=True";
                    lsCadenaConexion += ";Initial Catalog=" + lsBDNombre;
                    lsCadenaConexion += ";Data Source=" + lsBDServidor;
                    if (lsBDUsuario != null)
                    {
                         lsCadenaConexion += ";User id=" + lsBDUsuario;
                         lsCadenaConexion += ";Password=" + lsBDPassword;
                    }
                    else
                    {
                         lsCadenaConexion += " ;Integrated Security=True ";
                         lsCadenaConexion += " ;MultipleActiveResultSets=True ";
                    }
               }
               else
               {
                    lsCadenaConexion = "";
               }
               return lsCadenaConexion;
          }
     }
}
