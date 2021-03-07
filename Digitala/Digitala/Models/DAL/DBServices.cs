using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Digitala.Models.DAL
{
    public class DBServices
    {
        public SqlDataAdapter da;
        public DataTable dt;

        public SqlConnection connect(String conString)
        {
            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }

        public List<Targets> ReadTargets()
        {

            SqlConnection con = null;
            List<Targets> targetList = new List<Targets>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select T.*, FA.FunctionArea, SFA.SubFunctionArea from Targets T"+
                                    " inner join FunctionAreas FA on T.FASerial = FA.FASerial  inner join SubFunctionAreas SFA on T.SFASerial = SFA.SFASerial";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Targets t = new Targets();

                    t.TarSerial = Convert.ToInt32(dr["TSerial"]);
                    t.FaSerial = Convert.ToInt32(dr["FASerial"]);
                    t.SfaSerial = Convert.ToInt32(dr["SFASerial"]);
                    t.Target = (string)(dr["Target"]);
                    t.Suitability = Convert.ToDouble(dr["Suitability"]);
                    t.Originality = Convert.ToDouble(dr["Originality"]);
                    t.NumOfUses = Convert.ToInt32(dr["NumOfUses"]);
                    t.FunctionArea = (string)(dr["FunctionArea"]);
                    t.SubFunctionArea = (string)(dr["SubFunctionArea"]);
                    
                    targetList.Add(t);
                }

                return targetList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Targets from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public int DeleteTarget(int id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildDeleteCommand(id);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {

                throw new Exception("Faild removing item", ex);
            }
            finally
            {
                if (con != null)
                {

                    // close the db connection
                    con.Close();
                }
            }

        }

        private String BuildDeleteCommand(int id)
        {
            String command;
            command = "DELETE from Targets where Tserial = " + id;
            return command;
        }

        public List<Schools> ReadSchools()
        {

            SqlConnection con = null;
            List<Schools> schoolList = new List<Schools>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select * from School";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Schools s = new Schools();
                    s.SchoolName = (string)(dr["SchName"]);
                    s.SchoolId = Convert.ToInt32(dr["SchId"]);
                    schoolList.Add(s);
                }

                return schoolList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Schools from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

    }
    
}

