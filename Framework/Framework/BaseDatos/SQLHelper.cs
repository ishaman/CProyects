﻿using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
namespace Solucionic.Framework.BaseDatos
{
    // ===============================================================================
    // Release history
    // VERSION	DESCRIPTION
    //   2.0	Added support for FillDataset, UpdateDataset and "Param" helper methods
    //
    // ===============================================================================
     /// <summary>
     /// The SqlHelper class is intended to encapsulate high performance, scalable best practices for  common uses of SqlClient.
     /// </summary>
    public sealed class SqlHelper
    {

        #region "private utility methods & constructors"

        // Since this class provides only static methods, make the default constructor private to prevent 
        // instances from being created with "new SqlHelper()".
        private SqlHelper()
        {
        }
        // New

        // This method is used to attach array of SqlParameters to a SqlCommand.
        // This method will assign a value of DbNull to any parameter with a direction of
        // InputOutput and a value of null.  
        // This behavior will prevent default values from being used, but
        // this will be the less common case than an intended pure output parameter (derived as InputOutput)
        // where the user provided no input value.
        // Parameters:
        // -command - The command to which the parameters will be added
        // -commandParameters - an array of SqlParameters to be added to command
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if ((command == null))
                throw new ArgumentNullException("command");
            if (((commandParameters != null)))
            {
                SqlParameter p = null;
                foreach (SqlParameter p_loopVariable in commandParameters)
                {
                    p = p_loopVariable;
                    if (((p != null)))
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && p.Value == null)
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
        // AttachParameters

        // This method assigns dataRow column values to an array of SqlParameters.
        // Parameters:
        // -commandParameters: Array of SqlParameters to be assigned values
        // -dataRow: the dataRow used to hold the stored procedure' s parameter values

        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if (commandParameters == null || dataRow == null)
            {
                // Do nothing if we get no data    
                return;
            }

            // Set the parameters values
            SqlParameter commandParameter = null;
            int i = 0;
            foreach (SqlParameter commandParameter_loopVariable in commandParameters)
            {
                commandParameter = commandParameter_loopVariable;
                // Check the parameter name
                if ((commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1))
                {
                    throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: ' {1}' .", i, commandParameter.ParameterName));
                }
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                {
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                }
                i = i + 1;
            }
        }

        // This method assigns an array of values to an array of SqlParameters.
        // Parameters:
        // -commandParameters - array of SqlParameters to be assigned values
        // -array of objects holding the values to be assigned

        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            int i = 0;
            int j = 0;

            if ((commandParameters == null) && (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Value array
            j = commandParameters.Length - 1;
            for (i = 0; i <= j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if ((paramInstance.Value == null))
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if ((parameterValues[i] == null))
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }
        // AssignParameterValues

        // This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        // to the provided command.
        // Parameters:
        // -command - the SqlCommand to be prepared
        // -connection - a valid SqlConnection, on which to execute this command
        // -transaction - a valid SqlTransaction, or ' null' 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // -commandParameters - an array of SqlParameters to be associated with the command or ' null' if no parameters are required

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, ref bool mustCloseConnection)
        {
            if ((command == null))
                throw new ArgumentNullException("command");
            if ((commandText == null || commandText.Length == 0))
                throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                mustCloseConnection = true;
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;
            command.CommandTimeout = 0;
            //14/Junio/2007 Iván para que espere indefinidamente

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it.
            if ((transaction != null))
            {
                if (transaction.Connection == null)
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;


            // Attach the command parameters if they are provided
            if ((commandParameters != null))
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }
        // PrepareCommand

        #endregion

        #region "ExecuteNonQuery"

        // Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.:  
        //  Dim result As Integer =  ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders")
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteNonQuery

        // Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.:  
        // Dim result As Integer = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // -commandParameters - an array of SqlParamters used to execute the command
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            // Create & open a SqlConnection, and dispose of it after we are done
            SqlConnection connection = null;
            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if ((connection != null))
                    connection.Dispose();
            }
        }
        // ExecuteNonQuery

        // Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
        // the connection string using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        //  Dim result As Integer = ExecuteNonQuery(connString, "PublishOrders", 24, 36)
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection
        // -spName - the name of the stored procedure
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {

            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)

                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteNonQuery

        // Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection. 
        // e.g.:  
        // Dim result As Integer = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders")
        // Parameters:
        // -connection - a valid SqlConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command 
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);

        }
        // ExecuteNonQuery

        // Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
        // using the provided parameters.
        // e.g.:  
        //  Dim result As Integer = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            if ((connection == null))
                throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            int retval = 0;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            if ((mustCloseConnection))
                connection.Close();

            return retval;
        }
        // ExecuteNonQuery

        // Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
        // using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        //  Dim result As integer = ExecuteNonQuery(conn, "PublishOrders", 24, 36)
        // Parameters:
        // -connection - a valid SqlConnection
        // -spName - the name of the stored procedure 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");
            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }

        }
        // ExecuteNonQuery

        // Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlTransaction.
        // e.g.:  
        //  Dim result As Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders")
        // Parameters:
        // -transaction - a valid SqlTransaction associated with the connection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteNonQuery

        // Execute a SqlCommand (that returns no resultset) against the specified SqlTransaction
        // using the provided parameters.
        // e.g.:  
        // Dim result As Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -transaction - a valid SqlTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            int retval = 0;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            return retval;
        }
        // ExecuteNonQuery

        // Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlTransaction 
        // using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim result As Integer = SqlHelper.ExecuteNonQuery(trans, "PublishOrders", 24, 36)
        // Parameters:
        // -transaction - a valid SqlTransaction 
        // -spName - the name of the stored procedure 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteNonQuery

        #endregion

        #region "ExecuteDataset"

        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.:  
        // Dim ds As DataSet = SqlHelper.ExecuteDataset("", commandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteDataset

        // Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.:  
        // Dim ds As Dataset = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // -commandParameters - an array of SqlParamters used to execute the command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            // Create & open a SqlConnection, and dispose of it after we are done
            SqlConnection connection = null;
            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if ((connection != null))
                    connection.Dispose();
            }
        }
        // ExecuteDataset

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        // the connection string using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim ds As Dataset= ExecuteDataset(connString, "GetOrders", 24, 36)
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection
        // -spName - the name of the stored procedure
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {

            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");
            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteDataset

        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        // e.g.:  
        // Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connection - a valid SqlConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {

            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteDataset

        // Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the provided parameters.
        // e.g.:  
        // Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid SqlConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // -commandParameters - an array of SqlParamters used to execute the command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter dataAdatpter = null;
            bool mustCloseConnection = false;

            dataAdatpter = null;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            try
            {
                // Create the DataAdapter & DataSet
                dataAdatpter = new SqlDataAdapter(cmd);

                // Fill the DataSet using default values for DataTable names, etc
                dataAdatpter.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();
            }
            finally
            {
                if (((dataAdatpter != null)))
                    dataAdatpter.Dispose();
            }
            if ((mustCloseConnection))
                connection.Close();

            // Return the dataset
            return ds;
        }
        // ExecuteDataset

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim ds As Dataset = ExecuteDataset(conn, "GetOrders", 24, 36)
        // Parameters:
        // -connection - a valid SqlConnection
        // -spName - the name of the stored procedure
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {

            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go

            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }

        }
        // ExecuteDataset

        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        // e.g.:  
        // Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders")
        // Parameters
        // -transaction - a valid SqlTransaction
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteDataset

        // Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        // using the provided parameters.
        // e.g.:  
        // Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        // Parameters
        // -transaction - a valid SqlTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // -commandParameters - an array of SqlParamters used to execute the command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter dataAdatpter = null;
            bool mustCloseConnection = false;
            dataAdatpter = null;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            try
            {
                // Create the DataAdapter & DataSet
                dataAdatpter = new SqlDataAdapter(cmd);

                // Fill the DataSet using default values for DataTable names, etc
                dataAdatpter.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();
            }
            finally
            {
                if (((dataAdatpter != null)))
                    dataAdatpter.Dispose();
            }

            // Return the dataset
            return ds;

        }
        // ExecuteDataset

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        // SqlTransaction using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim ds As Dataset = ExecuteDataset(trans, "GetOrders", 24, 36)
        // Parameters:
        // -transaction - a valid SqlTransaction 
        // -spName - the name of the stored procedure
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {

            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteDataset

        #endregion

        #region "ExecuteReader"
        // this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
        // we can set the appropriate CommandBehavior when calling ExecuteReader()
        private enum SqlConnectionOwnership
        {
            // Connection is owned and managed by SqlHelper
            Internal,
            // Connection is owned and managed by the caller
            External
        }
        // SqlConnectionOwnership

        // Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
        // If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        // If the caller provided the connection, we want to leave it to them to manage.
        // Parameters:
        // -connection - a valid SqlConnection, on which to execute this command 
        // -transaction - a valid SqlTransaction, or ' null' 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParameters to be associated with the command or ' null' if no parameters are required 
        // -connectionOwnership - indicates whether the connection parameter was provided by the caller, or created by SqlHelper 
        // Returns: SqlDataReader containing the results of the command 
        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {

            if ((connection == null))
                throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            try
            {
                // Create a reader
                SqlDataReader dataReader = null;

                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

                // Call ExecuteReader with the appropriate CommandBehavior
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                // Detach the SqlParameters from the command object, so they can be used again
                bool canClear = true;
                SqlParameter commandParameter = null;
                foreach (SqlParameter commandParameter_loopVariable in cmd.Parameters)
                {
                    commandParameter = commandParameter_loopVariable;
                    if (commandParameter.Direction != ParameterDirection.Input)
                    {
                        canClear = false;
                    }
                }

                if ((canClear))
                    cmd.Parameters.Clear();

                return dataReader;
            }
            catch
            {
                if ((mustCloseConnection))
                    connection.Close();
                throw;
            }
        }
        // ExecuteReader

        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.:  
        // Dim dr As SqlDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // Returns: A SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteReader

        // Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.:  
        // Dim dr As SqlDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: A SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            // Create & open a SqlConnection
            SqlConnection connection = null;
            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                // Call the private overload that takes an internally owned connection in place of the connection string
                return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch
            {
                // If we fail to return the SqlDatReader, we need to close the connection ourselves
                if ((connection != null))
                    connection.Dispose();
                throw;
            }
        }
        // ExecuteReader

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        // the connection string using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim dr As SqlDataReader = ExecuteReader(connString, "GetOrders", 24, 36)
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection 
        // -spName - the name of the stored procedure 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        // Returns: A SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteReader

        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        // e.g.:  
        // Dim dr As SqlDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connection - a valid SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // Returns: A SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {

            return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);

        }
        // ExecuteReader

        // Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the provided parameters.
        // e.g.:  
        // Dim dr As SqlDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: A SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            // Pass through the call to private overload using a null transaction value
            return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);

        }
        // ExecuteReader

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim dr As SqlDataReader = ExecuteReader(conn, "GetOrders", 24, 36)
        // Parameters:
        // -connection - a valid SqlConnection 
        // -spName - the name of the stored procedure 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        // Returns: A SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;
            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }

        }
        // ExecuteReader

        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction.
        // e.g.:  
        // Dim dr As SqlDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -transaction - a valid SqlTransaction  
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // Returns: A SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteReader

        // Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        // using the provided parameters.
        // e.g.:  
        // Dim dr As SqlDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -transaction - a valid SqlTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: A SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            // Pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }
        // ExecuteReader

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        // using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim dr As SqlDataReader = ExecuteReader(trans, "GetOrders", 24, 36)
        // Parameters:
        // -transaction - a valid SqlTransaction 
        // -spName - the name of the stored procedure 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        // Returns: A SqlDataReader containing the resultset generated by the command
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteReader

        #endregion

        #region "ExecuteScalar"

        // Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.:  
        // Dim orderCount As Integer = CInt(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount"))
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteScalar

        // Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.:  
        // Dim orderCount As Integer = Cint(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            // Create & open a SqlConnection, and dispose of it after we are done.
            SqlConnection connection = null;
            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if ((connection != null))
                    connection.Dispose();
            }
        }
        // ExecuteScalar

        // Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
        // the connection string using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim orderCount As Integer = CInt(ExecuteScalar(connString, "GetOrderCount", 24, 36))
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection 
        // -spName - the name of the stored procedure 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteScalar

        // Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection. 
        // e.g.:  
        // Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount"))
        // Parameters:
        // -connection - a valid SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteScalar

        // Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        // using the provided parameters.
        // e.g.:  
        // Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
        // Parameters:
        // -connection - a valid SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            if ((connection == null))
                throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            object retval = null;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Execute the command & return the results
            retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            if ((mustCloseConnection))
                connection.Close();

            return retval;

        }
        // ExecuteScalar

        // Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        // using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim orderCount As Integer = CInt(ExecuteScalar(conn, "GetOrderCount", 24, 36))
        // Parameters:
        // -connection - a valid SqlConnection 
        // -spName - the name of the stored procedure 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }

        }
        // ExecuteScalar

        // Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction.
        // e.g.:  
        // Dim orderCount As Integer  = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount"))
        // Parameters:
        // -transaction - a valid SqlTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteScalar

        // Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        // using the provided parameters.
        // e.g.:  
        // Dim orderCount As Integer = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
        // Parameters:
        // -transaction - a valid SqlTransaction  
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            object retval = null;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Execute the command & return the results
            retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            return retval;
        }
        // ExecuteScalar

        // Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction 
        // using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim orderCount As Integer = CInt(ExecuteScalar(trans, "GetOrderCount", 24, 36))
        // Parameters:
        // -transaction - a valid SqlTransaction 
        // -spName - the name of the stored procedure 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;
            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteScalar

        #endregion

        #region "ExecuteXmlReader"

        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        // e.g.:  
        // Dim r As XmlReader = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connection - a valid SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        // Returns: An XmlReader containing the resultset generated by the command 
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteXmlReader

        // Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the provided parameters.
        // e.g.:  
        // Dim r As XmlReader = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid SqlConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: An XmlReader containing the resultset generated by the command 
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            // Pass through the call using a null transaction value
            if ((connection == null))
                throw new ArgumentNullException("connection");
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            try
            {
                XmlReader retval = null;

                PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

                // Create the DataAdapter & DataSet
                retval = cmd.ExecuteXmlReader();

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                return retval;
            }
            catch
            {
                if ((mustCloseConnection))
                    connection.Close();
                throw;
            }

        }
        // ExecuteXmlReader

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim r As XmlReader = ExecuteXmlReader(conn, "GetOrders", 24, 36)
        // Parameters:
        // -connection - a valid SqlConnection 
        // -spName - the name of the stored procedure using "FOR XML AUTO" 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        // Returns: An XmlReader containing the resultset generated by the command 
        public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteXmlReader


        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction
        // e.g.:  
        // Dim r As XmlReader = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -transaction - a valid SqlTransaction
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        // Returns: An XmlReader containing the resultset generated by the command 
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
        }
        // ExecuteXmlReader

        // Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        // using the provided parameters.
        // e.g.:  
        // Dim r As XmlReader = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        // Parameters:
        // -transaction - a valid SqlTransaction
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        // -commandParameters - an array of SqlParamters used to execute the command 
        // Returns: An XmlReader containing the resultset generated by the command
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand cmd = new SqlCommand();

            XmlReader retval = null;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Create the DataAdapter & DataSet
            retval = cmd.ExecuteXmlReader();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            return retval;

        }
        // ExecuteXmlReader

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        // using the provided parameter values.  This method will discover the parameters for the 
        // stored procedure, and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // Dim r As XmlReader = ExecuteXmlReader(trans, "GetOrders", 24, 36)
        // Parameters:
        // -transaction - a valid SqlTransaction
        // -spName - the name of the stored procedure 
        // -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        // Returns: A dataset containing the resultset generated by the command
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters = null;

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        // ExecuteXmlReader

        #endregion

        #region "FillDataset"
        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.:  
        //   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
        // Parameters:    
        // -connectionString: A valid connection string for a SqlConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        //               by a user defined name (probably the actual table name)

        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((dataSet == null))
                throw new ArgumentNullException("dataSet");

            // Create & open a SqlConnection, and dispose of it after we are done
            SqlConnection connection = null;
            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
            finally
            {
                if ((connection != null))
                    connection.Dispose();
            }
        }

        // Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.:  
        //   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() = {"orders"}, new SqlParameter("@prodid", 24))
        // Parameters:    
        // -connectionString: A valid connection string for a SqlConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        //               by a user defined name (probably the actual table name)
        // -commandParameters: An array of SqlParamters used to execute the command

        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((dataSet == null))
                throw new ArgumentNullException("dataSet");

            // Create & open a SqlConnection, and dispose of it after we are done
            SqlConnection connection = null;
            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
            finally
            {
                if ((connection != null))
                    connection.Dispose();
            }
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        // the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        //   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, 24)
        // Parameters:
        // -connectionString: A valid connection string for a SqlConnection
        // -spName: the name of the stored procedure
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        //             by a user defined name (probably the actual table name)
        // -parameterValues: An array of objects to be assigned As the input values of the stored procedure

        public static void FillDataset(string connectionString, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((dataSet == null))
                throw new ArgumentNullException("dataSet");

            // Create & open a SqlConnection, and dispose of it after we are done
            SqlConnection connection = null;
            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, spName, dataSet, tableNames, parameterValues);
            }
            finally
            {
                if ((connection != null))
                    connection.Dispose();
            }
        }

        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        // e.g.:  
        //   FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
        // Parameters:
        // -connection: A valid SqlConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)

        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        // Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the provided parameters.
        // e.g.:  
        //   FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new SqlParameter("@prodid", 24))
        // Parameters:
        // -connection: A valid SqlConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of SqlParamters used to execute the command

        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);

        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the provided parameter values.  This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        // FillDataset (conn, "GetOrders", ds, new string() {"orders"}, 24, 36)
        // Parameters:
        // -connection: A valid SqlConnection
        // -spName: the name of the stored procedure
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        //             by a user defined name (probably the actual table name)
        // -parameterValues: An array of objects to be assigned as the input values of the stored procedure

        public static void FillDataset(SqlConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((dataSet == null))
                throw new ArgumentNullException("dataSet");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go

            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }

        }

        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        // e.g.:  
        //   FillDataset (trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"})
        // Parameters:
        // -transaction: A valid SqlTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        //             by a user defined name (probably the actual table name)

        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        // Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        // using the provided parameters.
        // e.g.:  
        //   FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"}, new SqlParameter("@prodid", 24))
        // Parameters:
        // -transaction: A valid SqlTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of SqlParamters used to execute the command

        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);

        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
        // SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // This method provides no access to output parameters or the stored procedure' s return value parameter.
        // e.g.:  
        //   FillDataset(trans, "GetOrders", ds, new String(){"orders"}, 24, 36)
        // Parameters:
        // -transaction: A valid SqlTransaction
        // -spName: the name of the stored procedure
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        //             by a user defined name (probably the actual table name)
        // -parameterValues: An array of objects to be assigned as the input values of the stored procedure

        public static void FillDataset(SqlTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((dataSet == null))
                throw new ArgumentNullException("dataSet");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go

            if ((parameterValues != null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
                // Otherwise we can just call the SP without params
            }
            else
            {
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        // Private helper method that execute a SqlCommand (that returns a resultset) against the specified SqlTransaction and SqlConnection
        // using the provided parameters.
        // e.g.:  
        //   FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new SqlParameter("@prodid", 24))
        // Parameters:
        // -connection: A valid SqlConnection
        // -transaction: A valid SqlTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        //             by a user defined name (probably the actual table name)
        // -commandParameters: An array of SqlParamters used to execute the command

        private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((dataSet == null))
                throw new ArgumentNullException("dataSet");
            // Create a command and prepare it for execution
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            // Create the DataAdapter & DataSet
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            try
            {
                // Add the table mappings specified by the user

                if ((tableNames != null) && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    int index = 0;

                    for (index = 0; index <= tableNames.Length - 1; index++)
                    {
                        if ((tableNames[index] == null || tableNames[index].Length == 0))
                            throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName = tableName + (index + 1).ToString();
                    }
                }
                // Fill the DataSet using default values for DataTable names, etc
                dataAdapter.Fill(dataSet);
                // Detach the SqlParameters from the command object, so they can be used again
                command.Parameters.Clear();
            }
            finally
            {
                if (((dataAdapter != null)))
                    dataAdapter.Dispose();
            }
            if ((mustCloseConnection))
                connection.Close();
        }
        #endregion

        #region "UpdateDataset"
        // Executes the respective command for each inserted, updated, or deleted row in the DataSet.
        // e.g.:  
        //   UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order")
        // Parameters:
        // -insertCommand: A valid transact-SQL statement or stored procedure to insert new records into the data source
        // -deleteCommand: A valid transact-SQL statement or stored procedure to delete records from the data source
        // -updateCommand: A valid transact-SQL statement or stored procedure used to update records in the data source
        // -dataSet: the DataSet used to update the data source
        // -tableName: the DataTable used to update the data source

        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if ((insertCommand == null))
                throw new ArgumentNullException("insertCommand");
            if ((deleteCommand == null))
                throw new ArgumentNullException("deleteCommand");
            if ((updateCommand == null))
                throw new ArgumentNullException("updateCommand");
            if ((dataSet == null))
                throw new ArgumentNullException("dataSet");
            if ((tableName == null || tableName.Length == 0))
                throw new ArgumentNullException("tableName");

            // Create a SqlDataAdapter, and dispose of it after we are done
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            try
            {
                // Set the data adapter commands
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // Update the dataset changes in the data source
                dataAdapter.Update(dataSet, tableName);

                // Commit all the changes made to the DataSet
                dataSet.AcceptChanges();
            }
            finally
            {
                if (((dataAdapter != null)))
                    dataAdapter.Dispose();
            }
        }
        #endregion

        #region "CreateCommand"
        // Simplify the creation of a Sql command object by allowing
        // a stored procedure and optional parameters to be provided
        // e.g.:  
        // Dim command As SqlCommand = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName")
        // Parameters:
        // -connection: A valid SqlConnection object
        // -spName: the name of the stored procedure
        // -sourceColumns: An array of string to be assigned as the source columns of the stored procedure parameters
        // Returns:
        // a valid SqlCommand object
        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
        {

            if ((connection == null))
                throw new ArgumentNullException("connection");
            // Create a SqlCommand
            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // If we receive parameter values, we need to figure out where they go

            if ((sourceColumns != null) && sourceColumns.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided source columns to these parameters based on parameter order
                int index = 0;
                for (index = 0; index <= sourceColumns.Length - 1; index++)
                {
                    commandParameters[index].SourceColumn = sourceColumns[index];
                }

                // Attach the discovered parameters to the SqlCommand object
                AttachParameters(cmd, commandParameters);
            }
            cmd.CommandTimeout = 0;
            //14/Junio/2007 para que dure todo el tiempo la espera por el query
            return cmd;
        }
        #endregion

        #region "ExecuteNonQueryTypedParams"
        // Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
        // the connection string using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        // Parameters:
        // -connectionString: A valid connection string for a SqlConnection
        // -spName: the name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values
        // Returns:
        // an int representing the number of rows affected by the command
        public static int ExecuteNonQueryTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            int functionReturnValue = 0;
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
        // using the dataRow column values as the stored procedure' s parameters values.  
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        // Parameters:
        // -connection:a valid SqlConnection object
        // -spName: the name of the stored procedure
        // -dataRow:The dataRow used to hold the stored procedure' s parameter values.
        // Returns:
        // an int representing the number of rows affected by the command
        public static int ExecuteNonQueryTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            int functionReturnValue = 0;
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified
        // SqlTransaction using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        // Parameters:
        // -transaction:a valid SqlTransaction object
        // -spName:the name of the stored procedure
        // -dataRow:The dataRow used to hold the stored procedure' s parameter values.
        // Returns:
        // an int representing the number of rows affected by the command
        public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            int functionReturnValue = 0;

            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);

            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }
        #endregion

        #region "ExecuteDatasetTypedParams"
        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        // the connection string using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        // Parameters:
        // -connectionString: A valid connection string for a SqlConnection
        // -spName: the name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns:
        // a dataset containing the resultset generated by the command
        public static DataSet ExecuteDatasetTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            DataSet functionReturnValue = null;
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);

            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the dataRow column values as the store procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        // Parameters:
        // -connection: A valid SqlConnection object
        // -spName: the name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns:
        // a dataset containing the resultset generated by the command
        public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            DataSet functionReturnValue = null;

            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);

            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        // using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        // Parameters:
        // -transaction: A valid SqlTransaction object
        // -spName: the name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns:
        // a dataset containing the resultset generated by the command
        public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            DataSet functionReturnValue = null;
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);

            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }
        #endregion

        #region "ExecuteReaderTypedParams"
        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        // the connection string using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // Parameters:
        // -connectionString: A valid connection string for a SqlConnection
        // -spName: the name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns:
        // a SqlDataReader containing the resultset generated by the command
        public static SqlDataReader ExecuteReaderTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            SqlDataReader functionReturnValue = null;
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // Parameters:
        // -connection: A valid SqlConnection object
        // -spName: The name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns:
        // a SqlDataReader containing the resultset generated by the command
        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            SqlDataReader functionReturnValue = null;
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        // using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // Parameters:
        // -transaction: A valid SqlTransaction object
        // -spName" The name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns:
        // a SqlDataReader containing the resultset generated by the command
        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            SqlDataReader functionReturnValue = null;
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }
        #endregion

        #region "ExecuteScalarTypedParams"
        // Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
        // the connection string using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // Parameters:
        // -connectionString: A valid connection string for a SqlConnection
        // -spName: The name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns:
        // An object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalarTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            object functionReturnValue = null;
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");
            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        // using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // Parameters:
        // -connection: A valid SqlConnection object
        // -spName: the name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns: 
        // an object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalarTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            object functionReturnValue = null;
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        // using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // Parameters:
        // -transaction: A valid SqlTransaction object
        // -spName: the name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns: 
        // an object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalarTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            object functionReturnValue = null;
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }
        #endregion

        #region "ExecuteXmlReaderTypedParams"
        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        // using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // Parameters:
        // -connection: A valid SqlConnection object
        // -spName: the name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns: 
        // an XmlReader containing the resultset generated by the command
        public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            XmlReader functionReturnValue = null;
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");
            // If the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        // using the dataRow column values as the stored procedure' s parameters values.
        // This method will query the database to discover the parameters for the 
        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        // Parameters:
        // -transaction: A valid SqlTransaction object
        // -spName: the name of the stored procedure
        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.
        // Returns: 
        // an XmlReader containing the resultset generated by the command
        public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            XmlReader functionReturnValue = null;
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");
            // if the row has values, the store procedure parameters must be initialized

            if (((dataRow != null) && dataRow.ItemArray.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                functionReturnValue = SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                functionReturnValue = SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
            return functionReturnValue;
        }
        #endregion

        #region "ExecuteDataTable"
        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.:  
        // Dim ds As Datatable = SqlHelper.ExecuteDatatable("", commandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-SQL command
        // Returns: A datatable containing the result generated by the command
          public static DataTable ExecuteDataTable( string connectionString, CommandType commandType, string commandText)
          {
               // Pass through the call providing null for the set of SqlParameters
               return ExecuteDataTable(connectionString, commandType, commandText, (SqlParameter[])null);
          }
          // Execute a SqlCommand (that returns a result) against the database specified in the connection string 
          // using the provided parameters.
          // e.g.:  
          // Dim ds As Datatable = ExecuteDataTable(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
          // Parameters:
          // -connectionString - a valid connection string for a SqlConnection
          // -commandType - the CommandType (stored procedure, text, etc.)
          // -commandText - the stored procedure name or T-SQL command
          // -commandParameters - an array of SqlParamters used to execute the command
          // Returns: A datatable containing the result generated by the command
          public static DataTable ExecuteDataTable( string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters )
          {
               if ((connectionString == null || connectionString.Length == 0))
                    throw new ArgumentNullException("connectionString");

               // Create & open a SqlConnection, and dispose of it after we are done
               SqlConnection connection = null;
               connection = null;
               try
               {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    // Call the overload that takes a connection in place of the connection string
                    return ExecuteDataTable(connection, commandType, commandText, commandParameters);
               }
               finally
               {
                    if ((connection != null))
                         connection.Dispose();
               }
          }
          // Execute a SqlCommand (that returns a result) against the specified SqlConnection 
          // using the provided parameters.
          // e.g.:  
          // Dim ds As DataTable = ExecuteDataTable(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
          // Parameters:
          // -connection - a valid SqlConnection
          // -commandType - the CommandType (stored procedure, text, etc.)
          // -commandText - the stored procedure name or T-SQL command
          // -commandParameters - an array of SqlParamters used to execute the command
          // Returns: A datatable containing the result generated by the command
          public static DataTable ExecuteDataTable( SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
          {            
               if ((connection == null))
                    throw new ArgumentNullException("connection");
               // Create a command and prepare it for execution
               SqlCommand cmd = new SqlCommand();
               DataTable dt = new DataTable();
               SqlDataAdapter dataAdatpter = null;
               bool mustCloseConnection = false;
               dataAdatpter = null;
               PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);
               try
               {
                    // Create the DataAdapter & Datatable
                    dataAdatpter = new SqlDataAdapter(cmd);
                    // Fill the DataSet using default values for DataTable names, etc
                    dataAdatpter.Fill(dt);
                    // Detach the SqlParameters from the command object, so they can be used again
                    cmd.Parameters.Clear();
               }
               finally
               {
                    if (((dataAdatpter != null)))
                         dataAdatpter.Dispose();
               }
               if ((mustCloseConnection))
                    connection.Close();
               // Return the datatable
               return dt;
        }        
        #endregion

        #region "FillDataTable"
          // Execute a SqlCommand (that returns a result and takes no parameters) against the provided SqlConnection. 
          // e.g.:  
          //   FillDatatable (conn, CommandType.StoredProcedure, "GetOrders", dt, new String() {"orders"})
          // Parameters:
          // -connection: A valid SqlConnection
          // -commandType: the CommandType (stored procedure, text, etc.)
          // -commandText: the stored procedure name or T-SQL command
          // -dataTable: A dataset wich will contain the result generated by the command
          // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
          // by a user defined name (probably the actual table name)
        public static void FillDataTable( SqlConnection connection, CommandType commandType, string commandText, DataTable dataTable, string[] tableNames )
        {
          FillDataTable(connection, commandType, commandText, dataTable, tableNames, null);
        }
        // Execute a SqlCommand (that returns a result) against the specified SqlConnection 
        // using the provided parameters.
        // e.g.:  
        //   FillDatatable (conn, CommandType.StoredProcedure, "GetOrders", dt, new String() {"orders"}, new SqlParameter("@prodid", 24))
        // Parameters:
        // -connection: A valid SqlConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataTable: A dataset wich will contain the result generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of SqlParamters used to execute the command
        public static void FillDataTable( SqlConnection connection, CommandType commandType, string commandText, DataTable dataSet, string[] tableNames, params SqlParameter[] commandParameters )
        {
             FillDataTable(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }
        // Private helper method that execute a SqlCommand (that returns a result) against the specified SqlTransaction and SqlConnection
        // using the provided parameters.
        // e.g.:  
        //   FillDatatable(conn, trans, CommandType.StoredProcedure, "GetOrders", dt, new String() {"orders"}, new SqlParameter("@prodid", 24))
        // Parameters:
        // -connection: A valid SqlConnection
        // -transaction: A valid SqlTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataTable: A dataset wich will contain the result generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        //             by a user defined name (probably the actual table name)
        // -commandParameters: An array of SqlParamters used to execute the command
        private static void FillDataTable( SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, DataTable dataTable, string[] tableNames, params SqlParameter[] commandParameters )
        {
             if ((connection == null))
                  throw new ArgumentNullException("connection");
             if ((dataTable == null))
                  throw new ArgumentNullException("dataTable");
             // Create a command and prepare it for execution
             SqlCommand command = new SqlCommand();
             bool mustCloseConnection = false;
             PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
             // Create the DataAdapter & DataSet
             SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
             try
             {
                  // Add the table mappings specified by the user
                  if ((tableNames != null) && tableNames.Length > 0)
                  {
                       string tableName = "Table";
                       int index = 0;

                       for (index = 0; index <= tableNames.Length - 1; index++)
                       {
                            if ((tableNames[index] == null || tableNames[index].Length == 0))
                                 throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                            dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                            tableName = tableName + (index + 1).ToString();
                       }
                  }
                  // Fill the DataSet using default values for DataTable names, etc
                  dataAdapter.Fill(dataTable);
                  // Detach the SqlParameters from the command object, so they can be used again
                  command.Parameters.Clear();
             }
             finally
             {
                  if (((dataAdapter != null)))
                       dataAdapter.Dispose();
             }
             if ((mustCloseConnection))
                  connection.Close();
        }
        // Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        // e.g.:  
        //   FillDatatable (trans, CommandType.StoredProcedure, "GetOrders", dt, new string() {"orders"})
        // Parameters:
        // -transaction: A valid SqlTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataTable: A dataset wich will contain the result generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        //             by a user defined name (probably the actual table name)
        public static void FillDataTable( SqlTransaction transaction, CommandType commandType, string commandText, DataTable dataTable, string[] tableNames )
        {
             FillDataTable(transaction, commandType, commandText, dataTable, tableNames, null);
        }

        // Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        // using the provided parameters.
        // e.g.:  
        //   FillDataTable(trans, CommandType.StoredProcedure, "GetOrders", ts, new string() {"orders"}, new SqlParameter("@prodid", 24))
        // Parameters:
        // -transaction: A valid SqlTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-SQL command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of SqlParamters used to execute the command
        public static void FillDataTable( SqlTransaction transaction, CommandType commandType, string commandText, DataTable dataTable, string[] tableNames, params SqlParameter[] commandParameters )
        {
             if ((transaction == null))
                  throw new ArgumentNullException("transaction");
             if ((transaction != null) && (transaction.Connection == null))
                  throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
             FillDataTable(transaction.Connection, transaction, commandType, commandText, dataTable, tableNames, commandParameters);
        }

       
        #endregion

    }
}
namespace Solucionic.Framework.BaseDatos
{
     /// <summary>
     /// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the ability to discover parameters for stored procedures at run-time.
     /// </summary>
    public sealed class SqlHelperParameterCache
    {

        #region "private methods, variables, and constructors"


        // Since this class provides only static methods, make the default constructor private to prevent 
        // instances from being created with "new SqlHelperParameterCache()".
        private SqlHelperParameterCache()
        {
        }
        // New 


        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
        // resolve at run time the appropriate set of SqlParameters for a stored procedure
        // Parameters:
        // - connectionString - a valid connection string for a SqlConnection
        // - spName - the name of the stored procedure
        // - includeReturnValueParameter - whether or not to include their return value parameter>
        // Returns: SqlParameter()
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter, params object[] parameterValues)
        {

            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");
            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] discoveredParameters = null;
            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();
            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            discoveredParameters = new SqlParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            SqlParameter discoveredParameter = null;
            foreach (SqlParameter discoveredParameter_loopVariable in discoveredParameters)
            {
                discoveredParameter = discoveredParameter_loopVariable;
                discoveredParameter.Value = DBNull.Value;
            }

            return discoveredParameters;

        }
        // DiscoverSpParameterSet

        // Deep copy of cached SqlParameter array
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {

            int i = 0;
            int j = originalParameters.Length - 1;
            SqlParameter[] clonedParameters = new SqlParameter[j + 1];

            for (i = 0; i <= j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }
        // CloneParameters

        #endregion

        #region "caching functions"

        // add parameter array to the cache
        // Parameters
        // -connectionString - a valid connection string for a SqlConnection 
        // -commandText - the stored procedure name or T-SQL command 
        // -commandParameters - an array of SqlParamters to be cached 
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((commandText == null || commandText.Length == 0))
                throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }
        // CacheParameterSet

        // retrieve a parameter array from the cache
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection 
        // -commandText - the stored procedure name or T-SQL command 
        // Returns: An array of SqlParamters 
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((commandText == null || commandText.Length == 0))
                throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;
            SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }
        // GetCachedParameterSet

        #endregion

        #region "Parameter Discovery Functions"
        // Retrieves the set of SqlParameters appropriate for the stored procedure.
        // This method will query the database for this information, and then store it in a cache for future requests.
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection 
        // -spName - the name of the stored procedure 
        // Returns: An array of SqlParameters
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }
        // GetSpParameterSet 

        // Retrieves the set of SqlParameters appropriate for the stored procedure.
        // This method will query the database for this information, and then store it in a cache for future requests.
        // Parameters:
        // -connectionString - a valid connection string for a SqlConnection
        // -spName - the name of the stored procedure 
        // -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        // Returns: An array of SqlParameters 
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            SqlParameter[] functionReturnValue = null;
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            SqlConnection connection = null;
            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                functionReturnValue = GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
            finally
            {
                if ((connection != null))
                    connection.Dispose();
            }
            return functionReturnValue;
        }
        // GetSpParameterSet

        // Retrieves the set of SqlParameters appropriate for the stored procedure.
        // This method will query the database for this information, and then store it in a cache for future requests.
        // Parameters:
        // -connection - a valid SqlConnection object
        // -spName - the name of the stored procedure 
        // -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        // Returns: An array of SqlParameters 
        public static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {

            return GetSpParameterSet(connection, spName, false);
        }
        // GetSpParameterSet

        // Retrieves the set of SqlParameters appropriate for the stored procedure.
        // This method will query the database for this information, and then store it in a cache for future requests.
        // Parameters:
        // -connection - a valid SqlConnection object
        // -spName - the name of the stored procedure 
        // -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        // Returns: An array of SqlParameters 
        public static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            SqlParameter[] functionReturnValue = null;
            if ((connection == null))
                throw new ArgumentNullException("connection");
            SqlConnection clonedConnection = null;
            clonedConnection = null;
            try
            {
                clonedConnection = (SqlConnection)(((ICloneable)connection).Clone());
                functionReturnValue = GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
            finally
            {
                if ((clonedConnection != null))
                    clonedConnection.Dispose();
            }
            return functionReturnValue;
        }
        // GetSpParameterSet

        // Retrieves the set of SqlParameters appropriate for the stored procedure.
        // This method will query the database for this information, and then store it in a cache for future requests.
        // Parameters:
        // -connection - a valid SqlConnection object
        // -spName - the name of the stored procedure 
        // -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        // Returns: An array of SqlParameters 
        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {

            if ((connection == null))
                throw new ArgumentNullException("connection");

            SqlParameter[] cachedParameters = null;
            string hashKey = null;

            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter == true ? ":include ReturnValue Parameter" : "").ToString();

            cachedParameters = (SqlParameter[])paramCache[hashKey];

            if ((cachedParameters == null))
            {
                SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;

            }

            return CloneParameters(cachedParameters);

        }
        // GetSpParameterSet
        #endregion

    }
}
