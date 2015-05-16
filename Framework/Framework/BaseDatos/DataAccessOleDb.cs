//Valorar si todavia se conservaran estas conexiones 
//using System;
//using System.Collections;
//using System.Data;
//using System.Data.OleDb;
//using System.Data.SqlClient;
//using System.Diagnostics;
//using System.Linq;
//namespace Solucionic.Framework.BaseDatos
//{

//    internal class DataAccessOleDb : IBasesDatos
//    {
//        private OleDbConnection Co_OleDBConexion;
//        private IDbTransaction Co_Transaccion;
//        private bool Cb_MantenenerConexionAbierta;
//        private bool Cb_ExistextsLog;

//        private bool Cb_GrabandoLog;
//        public string RegresaDefinicionTabla(string psTabla)
//        {
//            return "";
//        }
//        public string[] RegresaLlavesTabla(string psTabla)
//        {
//            return null;
//        }
//        public bool MantenenerConexionAbierta
//        {
//            get { return Cb_MantenenerConexionAbierta; }
//            set { Cb_MantenenerConexionAbierta = value; }
//        }
//        public System.Data.IDbConnection Conexion
//        {
//            get { return Co_OleDBConexion; }
//        }
//        public System.Data.ConnectionState RegresaEstado
//        {
//            get
//            {
//                try
//                {
//                    return Co_OleDBConexion.State;
//                }
//                catch (Exception ex)
//                {
//                    //EscribeEventLog(ex.ToString)
//                    //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                    GrabaLog(ex.ToString());
//                    throw new Exception(ex.Message);
//                }
//            }
//        }

//        public DataTable RegresaDataTable( string psQuery, string psTabla )
//        {
//             return null;
//        }

//        public IDbTransaction Transaccion
//        {
//            get { return Co_Transaccion; }
//        }

//        public DataAccessOleDb(OleDbConnection Po_Conexion)
//        {
//            //Co_OleDBConexion = Po_Conexion;
//            //try
//            //{
//            //    Cb_ExistextsLog = ExisteObjetoEnBD("xtsLog");
//            //}
//            //catch (Exception ex)
//            //{
//            //    Cb_ExistextsLog = false;
//            //}
//            //Cb_GrabandoLog = false;
//        }

//        public void IniciaTransaccion()
//        {
//            if (Co_OleDBConexion.State == ConnectionState.Closed)
//            {
//                Co_OleDBConexion.Open();
//            }
//            Co_Transaccion = Co_OleDBConexion.BeginTransaction();
//        }
//        public void ConfirmarTransaccion()
//        {
//            Co_Transaccion.Commit();
//            Co_Transaccion.Dispose();
//            Co_Transaccion = null;
//            Co_OleDBConexion.Close();
//        }
//        public void DeshacerTransaccion()
//        {
//            Co_Transaccion.Rollback();
//            Co_Transaccion.Dispose();
//            Co_Transaccion = null;
//            Co_OleDBConexion.Close();
//        }

//        public void ActualizaParams(string psspNombre, string psClasificacion, string psEmpresa, string psEmpleado, byte[] Pb_Foto)
//        {
//        }
//        public void CambiaBasedeDatos(string psNombreBaseDatos)
//        {
//            try
//            {
//                Co_OleDBConexion.ChangeDatabase(psNombreBaseDatos);
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & psNombreBaseDatos)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//        }
//        public void CreaConsulta(string pscstName, string pscstSQL, bool Pb_Valida = true)
//        {
//            string Ls_Query = null;
//            Ls_Query = "";
//            try
//            {
//                if (Pb_Valida)
//                {
//                    if (ExisteObjetoEnBD(pscstName))
//                    {
//                        if (!EliminaConsulta(pscstName))
//                        {
//                            return;
//                        }
//                    }
//                }
//                Ls_Query = "Create View dbo." + pscstName + " as " + pscstSQL;
//                EjecutaComando(Ls_Query);
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & Ls_Query)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if (Co_Transaccion == null)
//                {
//                   //FuncionesDatos.CierraConexion((IDbConnection)Co_OleDBConexion);
//                }
//            }
//        }
//        public void LlenaDataSet(ref System.Data.DataSet Pds_DataSet, string psQuery, string psTabla)
//        {
//            OleDbCommandBuilder Lo_ComBuilder = null;
//            OleDbDataAdapter Lo_DataAdapter = null;

//            Lo_DataAdapter = null;
//            Lo_ComBuilder = null;
//            try
//            {
//                Lo_DataAdapter = new OleDbDataAdapter(psQuery, Co_OleDBConexion);
//                Lo_ComBuilder = new OleDbCommandBuilder(Lo_DataAdapter);
//                Lo_DataAdapter.SelectCommand.CommandTimeout = 0;
//                //Da tiempo indefinido para cargar lo que viene del select
//                if ((this.Co_Transaccion != null))
//                {
//                    //Lo_DataAdapter.SelectCommand.Transaction =Co_Transaccion;
//                }
//                Lo_DataAdapter.Fill(Pds_DataSet, psTabla);
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & psQuery)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//             //  LiberaObjeto((object)Lo_DataAdapter, true);
//               // LiberaObjeto((object)Lo_ComBuilder, true);
//            }
//        }
//        public void LlenaDataSet(ref System.Data.DataTable Pds_DataTable, string psQuery)
//        {
//            OleDbCommandBuilder Lo_ComBuilder = null;
//            OleDbDataAdapter Lo_DataAdapter = null;

//            Lo_DataAdapter = null;
//            Lo_ComBuilder = null;
//            try
//            {
//                Lo_DataAdapter = new OleDbDataAdapter(psQuery, Co_OleDBConexion);
//                Lo_ComBuilder = new OleDbCommandBuilder(Lo_DataAdapter);
//                Lo_DataAdapter.SelectCommand.CommandTimeout = 0;
//                //Da tiempo indefinido para cargar lo que viene del select
//                if ((this.Co_Transaccion != null))
//                {
//                    //Lo_DataAdapter.SelectCommand.Transaction = this.Co_Transaccion;
//                }
//                Lo_DataAdapter.Fill(Pds_DataTable);
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & psQuery)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//               // LiberaObjeto((object)Lo_DataAdapter, true);
//                //LiberaObjeto((object)Lo_ComBuilder, true);
//            }
//        }
//        public void EjecutaSp(string psNombre, System.Collections.ArrayList Po_Parametros)
//        {
//            int Li_Indice = 0;
//            OleDbDataAdapter Lo_Adaptador = null;

//            Lo_Adaptador = null;
//            try
//            {
//                if (Co_OleDBConexion.State == ConnectionState.Closed)
//                {
//                    Co_OleDBConexion.Open();
//                }
//                Lo_Adaptador = new OleDbDataAdapter(psNombre, Co_OleDBConexion);
//                Lo_Adaptador.SelectCommand.CommandTimeout = 0;
//                Lo_Adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

//                if ((this.Co_Transaccion != null))
//                {
//                  //  Lo_Adaptador.SelectCommand.Transaction = this.Co_Transaccion;
//                }

//                for (Li_Indice = 0; Li_Indice <= Po_Parametros.Count - 1; Li_Indice++)
//                {
//                    //Lo_Adaptador.SelectCommand.Parameters.Add(new OleDbParameter(Convert.ToString(Po_Parametros[Li_Indice].Cs_Parametro), (object)Po_Parametros[Li_Indice].Cs_Valor));
//                }
//                Lo_Adaptador.SelectCommand.ExecuteNonQuery();
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if ((Lo_Adaptador != null))
//                {
//                    Lo_Adaptador.Dispose();
//                    Lo_Adaptador = null;
//                }
//                if (Co_Transaccion == null)
//                {
//                    //CierraConexion((IDbConnection)Co_OleDBConexion);
//                }
//            }
//        }
//        public object EjecutaEscalarSp(string psNombre, System.Collections.ArrayList Po_Parametros)
//        {
//            int Li_Indice = 0;
//            OleDbDataAdapter Lo_Adaptador = null;

//            Lo_Adaptador = null;
//            try
//            {
//                if (Co_OleDBConexion.State == ConnectionState.Closed)
//                {
//                    Co_OleDBConexion.Open();
//                }
//                Lo_Adaptador = new OleDbDataAdapter(psNombre, Co_OleDBConexion);
//                Lo_Adaptador.SelectCommand.CommandTimeout = 0;
//                Lo_Adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

//                if ((this.Co_Transaccion != null))
//                {
//                    //Lo_Adaptador.SelectCommand.Transaction = this.Co_Transaccion;
//                }

//                for (Li_Indice = 0; Li_Indice <= Po_Parametros.Count - 1; Li_Indice++)
//                {
//                 //   Lo_Adaptador.SelectCommand.Parameters.Add(new OleDbParameter(Convert.ToString(Po_Parametros[Li_Indice].Cs_Parametro), (object)Po_Parametros[Li_Indice].Cs_Valor));
//                }
//                return Lo_Adaptador.SelectCommand.ExecuteScalar();
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if ((Lo_Adaptador != null))
//                {
//                    Lo_Adaptador.Dispose();
//                    Lo_Adaptador = null;
//                }
//                if (Co_Transaccion == null)
//                {
//                    //CierraConexion((IDbConnection)Co_OleDBConexion);
//                }
//            }
//        }
//        public int EjecutaComando(string psQuery)
//        {
//            OleDbCommand Lo_Command = null;

//            Lo_Command = null;
//            try
//            {
//                if (Co_OleDBConexion.State == ConnectionState.Closed)
//                {
//                    Co_OleDBConexion.Open();
//                }
//                Lo_Command = new OleDbCommand(psQuery, Co_OleDBConexion);
//                if ((this.Co_Transaccion != null))
//                {
//                    //Lo_Command.Transaction = this.Co_Transaccion;
//                }
//                Lo_Command.CommandTimeout = 0;
//                Lo_Command.CommandType = CommandType.Text;
//                return Lo_Command.ExecuteNonQuery();

//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & psQuery)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if ((Lo_Command != null))
//                {
//                    Lo_Command.Dispose();
//                    Lo_Command = null;
//                }
//                if (Co_Transaccion == null)
//                {
//                //    CierraConexion((IDbConnection)Co_OleDBConexion);
//                }
//            }
//        }
//        public int EjecutaComando(string psQuery, ref IDbTransaction Po_Transaccion)
//        {
//            OleDbCommand Lo_Command = null;
//            try
//            {
//                if (Co_OleDBConexion.State == ConnectionState.Closed)
//                {
//                    Co_OleDBConexion.Open();
//                }
//                Lo_Command = new OleDbCommand(psQuery, Co_OleDBConexion);
//               // Lo_Command.Transaction = Po_Transaccion;
//                Lo_Command.CommandTimeout = 0;
//                Lo_Command.CommandType = CommandType.Text;
//                return Lo_Command.ExecuteNonQuery();

//            }
//            catch (Exception ex)
//            {
//                //'EscribeEventLog(ex.ToString & vbCrLf & psQuery)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                Lo_Command = null;
//               //Utilerias.CierraConexion((IDbConnection)Co_OleDBConexion);
//            }
//        }
//        public object EjecutaEscalar(string psQuery)
//        {
//            OleDbCommand Lo_Command = null;

//            try
//            {
//                if (Co_OleDBConexion.State == ConnectionState.Closed)
//                {
//                    Co_OleDBConexion.Open();
//                }
//                Lo_Command = new OleDbCommand(psQuery, Co_OleDBConexion);
//                if ((this.Co_Transaccion != null))
//                {
//                 //   Lo_Command.Transaction = this.Co_Transaccion;
//                }
//                Lo_Command.CommandTimeout = 0;
//                Lo_Command.CommandType = CommandType.Text;
//                return Lo_Command.ExecuteScalar();
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & psQuery)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if (Co_Transaccion == null)
//                {
//                   // CierraConexion((IDbConnection)Co_OleDBConexion);
//                }
//            }
//        }
//        public bool EliminaConsulta(string pscstName)
//        {
//            //string Ls_Query = null;
//            return false;
//            //try
//            //{
//            //    if (ExisteObjetoEnBD(pscstName))
//            //    {
//            //        Ls_Query = "DROP VIEW " + pscstName;
//            //        if (EjecutaComando(Ls_Query) > 0)
//            //        {
//            //            return true;
//            //        }
//            //        else
//            //        {
//            //            return false;
//            //        }
//            //    }
//            //}
//            //catch (Exception ex)
//            //{
//            //    //EscribeEventLog(ex.ToString & vbCrLf & pscstName)
//            //    //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//            //    GrabaLog(ex.ToString());
//            //    throw new Exception(ex.Message);
//            //}            
//        }

//        public bool ExisteObjetoEnBD(string psObjeto)
//        {
//            string Ls_Query = null;
//            object Lo_Valor = null;

//            Ls_Query = "Select Name From SysObjects  Where Name='" + psObjeto + "'";
//            try
//            {
//                Lo_Valor = EjecutaEscalar(Ls_Query);
//                if (psObjeto.ToUpper().Trim() == (Convert.ToString(Lo_Valor)).Trim().ToUpper() )
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & Ls_Query)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if (Co_Transaccion == null)
//                {
//                    //CierraConexion((IDbConnection)Co_OleDBConexion);
//                }
//            }
//        }
//        public bool ExisteCampoEnTabla(string psTabla, string psCampo, bool Pb_Azure = false)
//        {
//            string Ls_Query = null;

//            Ls_Query = "SELECT     syscolumns.name Columna";
//            Ls_Query += " FROM   syscolumns INNER JOIN";
//            Ls_Query += " sysobjects ON ";
//            Ls_Query += " (syscolumns.id = sysobjects.id) LEFT OUTER JOIN";
//            Ls_Query += " sysindexkeys ON ";
//            Ls_Query += " (syscolumns.id = sysindexkeys.id ";
//            Ls_Query += " AND syscolumns.colid = sysindexkeys.colid ";
//            Ls_Query += " AND (sysindexkeys.indid <= 1)) INNER JOIN";
//            Ls_Query += " systypes ON ";
//            Ls_Query += " (syscolumns.xtype = systypes.xtype ";
//            Ls_Query += " AND systypes.xtype = systypes.xusertype)";
//            Ls_Query += " WHERE     ";
//            Ls_Query += " sysobjects.name = '" + psTabla + "' ";
//            Ls_Query += " AND syscolumns.name = '" + psCampo + "'";

//            try
//            {
//                if (psCampo == Convert.ToString(EjecutaEscalar(Ls_Query)))
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & Ls_Query)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if (Co_Transaccion == null)
//                {
//                    //CierraConexion((IDbConnection)Co_OleDBConexion);
//                }
//            }
//        }
//        public bool ExisteRegistro(string psTabla, string psCampo, string psValor, ref string psFiltroEspecial )
//        {
//            string Ls_Query = null;

//            if (string.IsNullOrEmpty(psTabla.Trim()) || string.IsNullOrEmpty(psCampo.Trim()) || string.IsNullOrEmpty(psValor.Trim()))
//            {
//                return false;
//            }
//            Ls_Query = "SELECT " + psCampo + " FROM " + psTabla + " WHERE " + psCampo + "=" + psValor + " " + psFiltroEspecial;
//            try
//            {
//                Ls_Query = Convert.ToString(EjecutaEscalar(Ls_Query));
//                if (!string.IsNullOrEmpty(Ls_Query.Trim ()))
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & Ls_Query)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if (Co_Transaccion == null)
//                {
//                    //CierraConexion((IDbConnection)Co_OleDBConexion);
//                }
//            }
//        }
//        public System.Data.IDataReader RegresaDataReader(string psQuery)
//        {
//            OleDbCommand Lo_Command = null;

//            try
//            {
//                Lo_Command = new OleDbCommand(psQuery, Co_OleDBConexion);
//                Lo_Command.CommandTimeout = 0;
//                Lo_Command.Connection.Open();
//                if ((this.Co_Transaccion != null))
//                {
//                    //Lo_Command.Transaction = this.Co_Transaccion;
//                }
//                return Lo_Command.ExecuteReader();
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & psQuery)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//        }
//        public System.Data.DataSet RegresaDataSet(string psQuery, string psTabla)
//        {
//            SqlDataAdapter Lo_DataAdapter = null;

//            Lo_DataAdapter = null;
//            try
//            {
//                return RegresaDataSetyDataAdapter(psQuery, psTabla, ref Lo_DataAdapter);
//            }
//            finally
//            {
//                if ((Lo_DataAdapter != null))
//                {
//                   // Lo_DataAdapter.Dispose();
//                    Lo_DataAdapter = null;
//                }
//            }
//        }
//        public System.Data.DataSet RegresaDataSetyDataAdapter(string psQuery, string psTabla, ref SqlDataAdapter Po_DataAdapter)
//        {
//            //OleDbCommandBuilder Lo_ComBuilder = null;
//            //DataSet Lds_Datos = null;
//            //try
//            //{
//            //    Lds_Datos = new DataSet();
//            //    //Po_DataAdapter = new OleDbDataAdapter(psQuery, Co_OleDBConexion);
//            //   // Lo_ComBuilder = new OleDbCommandBuilder(Po_DataAdapter);
//            //    //Po_DataAdapter.SelectCommand.CommandTimeout = 0;
//            //    //Da tiempo indefinido para cargar lo que viene del select
//            //    if ((this.Co_Transaccion != null))
//            //    {
//            //      //  Po_DataAdapter.SelectCommand.Transaction = this.Co_Transaccion;
//            //        //Po_DataAdapter.UpdateCommand.Transaction = Me.Co_Transaccion
//            //        //Po_DataAdapter.InsertCommand.Transaction = Me.Co_Transaccion
//            //        //Po_DataAdapter.DeleteCommand.Transaction = Me.Co_Transaccion
//            //    }
//            //    //Po_DataAdapter.Fill(Lds_Datos, psTabla);
//            //    return Lds_Datos;
//            //}
//            //catch (Exception ex)
//            //{
//            //    //EscribeEventLog(ex.ToString & vbCrLf & psQuery)
//            //    //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//            //    GrabaLog(ex.ToString());
//            //    throw new Exception(ex.Message);
//            //}
//            //finally
//            //{
//            //    if (Co_Transaccion == null)
//            //    {
//            //        //CierraConexion((IDbConnection)Co_OleDBConexion);
//            //    }
//            //    //No se debe eliminar el commandbuilder, porque si no
//            //    //ya no hace los updates el dataset
//            //}
//             return null;
//        }
//        public System.Data.DataView RegresaDataView(string psQuery, string psTabla)
//        {
//            DataSet Lds_Datos = null;
//            try
//            {
//                Lds_Datos = new DataSet();
//                Lds_Datos = RegresaDataSet(psQuery, psTabla);
//                return Lds_Datos.Tables[psTabla].DefaultView;
//            }
//            catch (Exception ex)
//            {
//                //EscribeEventLog(ex.ToString & vbCrLf & psQuery)
//                //16/Julio/2009. Cambio generado para que ya no grabe en el EventViewer sino en la tabla xtsLog
//                GrabaLog(ex.ToString());
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                if (Co_Transaccion == null)
//                {
//                    //CierraConexion((IDbConnection)Co_OleDBConexion);
//                }
//            }
//        }
//        public int UpdateDataSetModificable( ref System.Data.DataSet Pds_Dataset, ref SqlDataAdapter Pda_DataAdapter )
//        {
            
//            //OleDbCommandBuilder Lo_CommandBuilder = null;
//            //try
//            //{
//            //    //Lo_CommandBuilder = new OleDbCommandBuilder(Pda_DataAdapter);
//            //    //Pda_DataAdapter.Update(Pds_Dataset.Tables[0]);
//            //}
//            //catch (Exception ex)
//            //{
//            //    throw new Exception(ex.Message);
//            //}
//            //finally
//            //{
//            //    Lo_CommandBuilder = null;
//            //}
//            return 0;
            
//        }
//        public System.Data.DataSet RegresaDataSet(string psQuery, string psTabla, ArrayList Po_Parametros)
//        {
//            return null;
//        }

//        public void GrabaLog(string psDescripcion)
//        {
//            string Ls_Query = null;
//            if (!Cb_ExistextsLog)
//            {
//                //si no existe el log no hace nada
//                return;
//            }
//            if (Cb_GrabandoLog)
//            {
//                //si esta rutina se invoco cuando se estaba grabando el log
//                //con esta bandera controlamos que no se llame de forma ciclica esta función
//                return;
//            }
//            try
//            {
//                Cb_GrabandoLog = true;
//                Ls_Query = "Insert into xtsLog (Descripcion) values ('" + psDescripcion.Replace("'", "''") + "')";
//                EjecutaComando(Ls_Query);
//            }
//            finally
//            {
//                Cb_GrabandoLog = false;
//            }

//        }
//    }
//}
