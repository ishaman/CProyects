using System.Configuration;

namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// 
     /// </summary>
     public class ConfiguracionDatos : ConfigurationSection
     {
          #region constants
          /// <summary>
          /// 
          /// </summary>
          private const string TITLE_PROPERTY_NAME = "BaseDatosDefault";
          //private const string NUMBER_OF_ITEMS_PROPERTY_NAME = "NumberOfItemsPerPage";
          #endregion

          #region properties
          /// <summary>
          /// 
          /// </summary>
          [ConfigurationProperty(TITLE_PROPERTY_NAME, DefaultValue = "ConexionPrincipal", IsRequired = true)]
          public string BaseDatosDefault
          {     
         
               //Las configuraciones son de solo lectura
               get
               {
                    return (string)this[TITLE_PROPERTY_NAME];
               }
          }          
          #endregion
     }

     public static class ManejadorConfiguraciones
     {
          #region constants
          /// <summary>
          /// 
          /// </summary>
          private static string BLOG_SETTINGS_NODE_NAME = "ConfiguracionDatos";

          #endregion

          #region members
          /// <summary>
          /// 
          /// </summary>
          private static ConfiguracionDatos _settings = ConfigurationManager.GetSection(BLOG_SETTINGS_NODE_NAME) as ConfiguracionDatos;

          #endregion

          #region Properties
          /// <summary>
          /// 
          /// </summary>
          public static ConfiguracionDatos Settings
          {
               get { return _settings; }
          }

          #endregion
     }
}
