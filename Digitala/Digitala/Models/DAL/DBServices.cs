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

                String selectSTR = "Select * from School ORDER BY SchName ASC";

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

        public List<FuncAreas> ReadSubAreas()
        {

            SqlConnection con = null;
            List<FuncAreas> subAreas = new List<FuncAreas>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT SFA.SFASerial, SFA.FASerial, SFA.SubFunctionArea, FA.FunctionArea FROM SubFunctionAreas SFA inner join FunctionAreas FA " +
                    "on SFA.FASerial = FA.FASerial ORDER BY SFA.FASerial DESC";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    FuncAreas s = new FuncAreas();

                    s.SubArea = (string)(dr["SubFunctionArea"]);
                    s.Area = (string)(dr["FunctionArea"]);
                    s.SubAreaId = Convert.ToInt32(dr["SFASerial"]);
                    s.AreaId = Convert.ToInt32(dr["FASerial"]);

                    subAreas.Add(s);
                }

                return subAreas;
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

        public int Insert(Teachers teacher)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildInsertCommand(teacher);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("המשתמש נמצא כרשום למערכת", ex);
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

        private String BuildInsertCommand(Teachers teacher)
        {
            //check if id alredy exsist in DB
            if (CheckId(teacher.TeacherID))
            {
                return null;
            }
            //check if email alredy exsist in DB
            else if (CheckEmail(teacher.TeacherEmail))
            {
                return null;
            }
            else
            {
                String command;
                String prefix;
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', '{4}')", teacher.TeacherID, teacher.TeacherFname, teacher.TeacherSurName, teacher.TeacherEmail, teacher.TeacherPassword);
                prefix = "INSERT INTO Teachers " + "([TId], [TFirstName], [TLastName], [TEmail], [TPassword])";


                command = prefix + sb.ToString();

                return command;
            }    
        }

        private String BuildInsertCommand(string area)
        {
            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}')", area);
            prefix = "INSERT INTO FunctionAreas " + "([FunctionArea])";


            command = prefix + sb.ToString();

            return command;

        }

        private String BuildInsertCommand(int areaId, string subArea)
        {
            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}')", areaId, subArea);
            prefix = "INSERT INTO SubFunctionAreas " + "([FASerial], [SubFunctionArea])";


            command = prefix + sb.ToString();

            return command;

        }

        public int InsertArea(string area)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildInsertCommand(area);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not insert new Area", ex);
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

        public int InsertSubArea(int areaId, string subArea)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildInsertCommand(areaId, subArea);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not insert new Sub-Area", ex);
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

        public bool CheckId(int id)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT * FROM Teachers where [TId] = " + id;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("תעודת הזהות נמצאת במערכת", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public bool CheckEmail(string email)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT * FROM Teachers where [TEmail] = " + "'" + email + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("האימייל נמצא כרשום במערכת", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<Teachers> ReadTeachers()
        {

            SqlConnection con = null;
            List<Teachers> teachersList = new List<Teachers>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Teachers";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Teachers t = new Teachers();
                        t.TeacherID = Convert.ToInt32(dr["TId"]);
                        t.TeacherFname = (string)dr["TFirstName"];
                        t.TeacherSurName = (string)dr["TLastName"];
                        t.TeacherEmail = (string)dr["TEmail"];
                        t.TeacherPassword = (string)dr["TPassword"];
                        t.TeacherAdmin = (bool)dr["TIsAdmin"];
                    teachersList.Add(t);
                }
                return teachersList;
            }

            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET teachers list", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public int Insert(Targets target)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildInsertCommand(target);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("מטרה לא התווספה", ex);
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

        private String BuildInsertCommand(Targets t)
        {
            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", t.FaSerial, t.SfaSerial, t.Target, t.Suitability, t.Originality, t.NumOfUses);
            prefix = "INSERT INTO Targets " + "([FASerial], [SFASerial], [Target], [Suitability], [Originality], [NumOfUses])";

            command = prefix + sb.ToString();

            return command;

        }

        public int Update(Targets target)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildUpdateCommand(target);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("מטרה לא התעדכנה", ex);
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

        private String BuildUpdateCommand(Targets t)
        {
            String command;
            command = "UPDATE Targets SET FASerial = " + t.FaSerial +
                ", SFASerial = " + t.SfaSerial + ", Target = " + t.Target + ", Suitability = " + t.Suitability +
                ", Originality = " + t.Originality + " WHERE Tserial = " + t.TarSerial;

            return command;
        }
    }
   
}

