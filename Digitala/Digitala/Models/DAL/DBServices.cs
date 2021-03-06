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

        public List<Targets> ReadTargets()
        {

            SqlConnection con = null;
            List<Targets> targetList = new List<Targets>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select T.Target, T.Suitability, T.Originality, T.NumOfUses, FA.FunctionArea, SFA.SubFunctionArea from Targets T"+
                                    " inner join FunctionAreas FA on T.FASerial = FA.FASerial  inner join SubFunctionAreas SFA on T.SFASerial = SFA.SFASerial";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Targets t = new Targets();

                    t.Target = (string)(dr["TSerial"]);
                    t.FunctionArea = (string)(dr["FASerial"]);
                    t.SubFunctionArea = (string)(dr["SFASerial"]);
                    t.NumOfUses = Convert.ToInt32(dr["NumOfUses"]);
                    t.Suitability = Convert.ToDouble(dr["Suitability"]);
                    t.Originality = Convert.ToDouble(dr["Originality"]);

                    targetList.Add(t);
                }

                return targetList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET List of campaigns", ex);
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