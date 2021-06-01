using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

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
                                    " inner join FunctionAreas FA on T.FASerial = FA.FASerial  inner join SubFunctionAreas SFA on T.SFASerial = SFA.SFASerial ORDER BY T.NumOfUses DESC";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Targets t = new Targets();

                    t.TarSerial = Convert.ToInt32(dr["TSerial"]);
                    t.FaSerial = Convert.ToInt32(dr["FASerial"]);
                    t.SfaSerial = Convert.ToInt32(dr["SFASerial"]);
                    t.Target = (string)(dr["TargetText"]);
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
        
        public List<Privileges> ReadPrivileges()
        {

            SqlConnection con = null;
            List<Privileges> dList = new List<Privileges>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select * from Privileges";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Privileges p = new Privileges();

                    p.DCode = Convert.ToInt32(dr["PId"]);
                    p.DPhrase = (string)(dr["Privilege"]);

                    dList.Add(p);
                }

                return dList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Privileges from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Privileges> ReadPrivileges(string studentId, string year)
        {

            SqlConnection con = null;
            List<Privileges> dList = new List<Privileges>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select P.* from PrivilegesForStudent PFS inner join Privileges P on PFS.PId = P.PId Where PFS.StudentId = " + studentId + " and PFS.Year = " + year;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Privileges p = new Privileges();

                    p.DCode = Convert.ToInt32(dr["PId"]);
                    p.DPhrase = (string)(dr["Privilege"]);

                    dList.Add(p);
                }

                return dList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Privileges from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public int InsertPrivileges(Privileges p)
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

            String cStr = BuildInsertPrivilegesCommand(p);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("התאמות לא נוספות למערכת", ex);
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

        private String BuildInsertPrivilegesCommand(Privileges p)
        {

            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', '{2}')", p.DCode, p.SId, p.Year);
            prefix = "INSERT INTO PrivilegesForStudent " + "([PId], [StudentId], [Year])";

            command = prefix + sb.ToString();

            return command;
        }

        public List<TargetsSurvey> ReadTargetsSurvey(string teacherId)
        {

            SqlConnection con = null;
            List<TargetsSurvey> targetList = new List<TargetsSurvey>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select T.*, FA.FunctionArea, SFA.SubFunctionArea from TargetsSurvey T" +
                                    " inner join FunctionAreas FA on T.FASerial = FA.FASerial inner join SubFunctionAreas SFA on T.SFASerial = SFA.SFASerial WHERE TeacherId = " + teacherId + "and Suitability=0 and Originality=0";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    TargetsSurvey t = new TargetsSurvey();

                    t.TarSerial = Convert.ToInt32(dr["Serial"]);
                    t.TarId = Convert.ToInt32(dr["TargetId"]);
                    t.FaSerial = Convert.ToInt32(dr["FASerial"]);
                    t.SfaSerial = Convert.ToInt32(dr["SFASerial"]);
                    t.Target = (string)(dr["TargetText"]);
                    t.Suitability = Convert.ToDouble(dr["Suitability"]);
                    t.Originality = Convert.ToDouble(dr["Originality"]);
                    t.NumOfUses = Convert.ToInt32(dr["NumOfUses"]);
                    t.FunctionArea = (string)(dr["FunctionArea"]);
                    t.SubFunctionArea = (string)(dr["SubFunctionArea"]);
                    t.TeacherId = (string)(dr["TeacherId"]);
                    t.CreationDate = Convert.ToDateTime(dr["CreationDate"]);

                    targetList.Add(t);
                }

                return targetList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Survey Targets from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public int DeleteSurvey(int id)
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

            String cStr = BuildDeleteSurveyCommand(id);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR updating surveys table", ex);
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

        private String BuildDeleteSurveyCommand(int id)
        {
            String command;            
            command = "Delete FROM TargetsSurvey WHERE TargetId = " +id;
            return command;
        }

        public int DeletePrivileges(Privileges p)
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

            String cStr = BuildDeletePrivileges(p);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR deleting Privileges for student", ex);
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

        private String BuildDeletePrivileges(Privileges p)
        {
            String command;
            command = "Delete FROM PrivilegesForStudent WHERE PId = " + p.DCode + " and StudentId = "+ p.SId + " and Year = "+ p.Year;

            return command;
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

        public List<FuncAreas> ReadAllAreas()
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
                throw new Exception("Could not GET Sub-Areas from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<FuncAreas> ReadAreas()
        {

            SqlConnection con = null;
            List<FuncAreas> subAreas = new List<FuncAreas>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM FunctionAreas FA ORDER BY FA.FASerial DESC";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    FuncAreas s = new FuncAreas();

                    s.Area = (string)(dr["FunctionArea"]);
                    s.AreaId = Convert.ToInt32(dr["FASerial"]);

                    subAreas.Add(s);
                }

                return subAreas;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Areas from DB", ex);
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

                String selectSTR = "SELECT * FROM SubFunctionAreas SFA ORDER BY SFA.SFASerial DESC";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    FuncAreas s = new FuncAreas();

                    s.SubArea = (string)(dr["SubFunctionArea"]);
                    s.AreaId = Convert.ToInt32(dr["FASerial"]);
                    s.SubAreaId = Convert.ToInt32(dr["SFASerial"]);

                    subAreas.Add(s);
                }

                return subAreas;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET SubAreas from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Chararcteristics> ReadChararcteristics()
        {

            SqlConnection con = null;
            List<Chararcteristics> Chars = new List<Chararcteristics>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Chararcteristics ORDER BY SFASerial ASC";
                
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Chararcteristics c = new Chararcteristics();

                    c.CharacteristicKey = Convert.ToInt32(dr["CharacteristicKey"]);
                    c.Chararcteristic = (string)(dr["Chararcteristic"]);
                    c.FaSerial = Convert.ToInt32(dr["FASerial"]);
                    c.SfaSerial = Convert.ToInt32(dr["SFASerial"]);
                    c.IsWeakness = Convert.ToBoolean(dr["Weakness"]);

                    Chars.Add(c);
                }

                return Chars;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Chararcteristics from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Chararcteristics> ReadFreeChararcteristics(string studentID, int year)
        {

            SqlConnection con = null;
            List<Chararcteristics> Chars = new List<Chararcteristics>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM [ChararcteristicsForStudent] where [StudentId]= "+ studentID + " and [year]= "+ year;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Chararcteristics c = new Chararcteristics();

                    c.CharacteristicKey = Convert.ToInt32(dr["CharacteristicKey"]);
                    c.Chararcteristic = (string)(dr["Chararcteristic"]);

                    Chars.Add(c);
                }

                return Chars;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Chararcteristics from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<int> ReadChararcteristics(string studentID, int year)
        {
            SqlConnection con = null;
            List<int> Chars = new List<int>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM CharacteristicsMatrix Where [StudentId]="+ studentID + " and [SCYear]="+year;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Chararcteristics c = new Chararcteristics();
                    for (int i = 1; i < dr.FieldCount-2; i++)
                    {
                        if (Convert.ToInt32(dr["char_" + i]) == 1)
                            Chars.Add(i);
                    }
                }

                return Chars;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Chararcteristics from DB", ex);
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

        public int Insert(Talas tala)
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

            String cStr = BuildInsertCommand(tala);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                InsertChararcteristics(tala.StudentChars, tala.StudentId, tala.CurrentYear);
                for (int i = 0; i < tala.Targets.Count(); i++)
                {
                    InsertToTala(tala.Targets[i], tala.StudentId, tala.CurrentYear);
                }

                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("תלא לא התווספה למערכת", ex);
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

        private String BuildInsertCommand(Talas tala)
        {
           
                String command;
                String prefix;
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("Values('{0}', '{1}')", tala.CurrentYear, tala.StudentId);
                prefix = "INSERT INTO Tala " + "([TYear], [StudentId])";

                command = prefix + sb.ToString();

                return command;
        }

        public int InsertToTala(Targets target, string StudentId, int CurrentYear)
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

            String cStr = BuildInsertToTalaCommand(target, StudentId, CurrentYear);      // helper method to build the insert string

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

        private String BuildInsertToTalaCommand(Targets t, string StudentId, int CurrentYear)
        {
            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            if (t.NewPhrase)
            {
                sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", t.TarSerial, t.FaSerial, t.SfaSerial, t.Target, StudentId, CurrentYear);
                prefix = "INSERT INTO TargetsInTala " + "([Tserial], [FASerial], [SFASerial], [NewPhrase], [StudentId], [TYear])";

            }
            else if (t.NewTar)
            {
                sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", 0, t.FaSerial, t.SfaSerial, t.Target, StudentId, CurrentYear);
                prefix = "INSERT INTO TargetsInTala " + "([Tserial], [FASerial], [SFASerial], [NewPhrase], [StudentId], [TYear])";

                TargetsSurvey TS = new TargetsSurvey();
                TS.FaSerial = t.FaSerial;
                TS.SfaSerial = t.SfaSerial;
                TS.Target = t.Target;

                Insert(TS);
            }
            else
            {
                sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', '{4}')", t.TarSerial, t.FaSerial, t.SfaSerial, StudentId, CurrentYear);
                prefix = "INSERT INTO TargetsInTala " + "([Tserial], [FASerial], [SFASerial], [StudentId], [TYear])";

                UpdateNumOfUses(t.TarSerial);
            }

            command = prefix + sb.ToString();

            return command;
        }

        public int Update(TargetsSurvey ST)
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

            String cStr = BuildUpdateCommand(ST);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("דירוג סקר לא התעדכן", ex);
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

        private String BuildUpdateCommand(TargetsSurvey ST)
        {
            String command;
            command = "UPDATE TargetsSurvey SET [Originality] = " + ST.Originality + ", [Suitability] = " + ST.Suitability + " WHERE Serial = " + ST.TarSerial;
            return command;
        }

        public int Insert(TargetsSurvey TS)
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

            String cStr = BuildInsertCommand(TS);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("מטרה לא התווספה לסקר", ex);
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

        private String BuildInsertCommand(TargetsSurvey t)
        {
            String command ="";
            String prefix;
            StringBuilder sb = new StringBuilder();

            List<Teachers> surveyTeachers = helpPickTechersForSurvey();
            int TargetID = helpPickTargetIDForSurvey();

            for (int i = 0; i < surveyTeachers.Count; i++)
            {
                sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', '{4}')", t.FaSerial, t.SfaSerial, t.Target, TargetID, surveyTeachers[i].TeacherID);
                prefix = "INSERT INTO TargetsSurvey " + "([FASerial], [SFASerial], [TargetText], [TargetId], [TeacherId])";
                command = command + " " + prefix + sb.ToString();
            }
            
           return command;

        }

        public int helpPickTargetIDForSurvey()
        {
            List<TargetsSurvey> targetForSurvey = ReadTargetsForSurveys();
            if (targetForSurvey.Count == 0)
                return 1;
            else
                return targetForSurvey[targetForSurvey.Count - 1].TarSerial;
        }

        public List<Teachers> helpPickTechersForSurvey()
        {
            Random r = new Random();

            List<Teachers> allteachers = ReadTeachers();
            List<Teachers> surveyTeachers = new List<Teachers>();
            float c = allteachers.Count;
            for (int i = 0; i < Math.Round(c * 0.33); i++)
            {
                surveyTeachers.Add(allteachers[r.Next(0, allteachers.Count-1)]);
            }

            return surveyTeachers;
        }

        public List<TargetsSurvey> ReadTargetsForSurveys()
        {

            SqlConnection con = null;
            List<TargetsSurvey> targetList = new List<TargetsSurvey>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select T.*, FA.FunctionArea, SFA.SubFunctionArea from TargetsSurvey T" +
                                    " inner join FunctionAreas FA on T.FASerial = FA.FASerial  inner join SubFunctionAreas SFA on T.SFASerial = SFA.SFASerial ORDER BY T.TargetId DESC";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    TargetsSurvey t = new TargetsSurvey();

                    t.TarSerial = Convert.ToInt32(dr["Serial"]);
                    t.TarId = Convert.ToInt32(dr["TargetId"]);
                    t.FaSerial = Convert.ToInt32(dr["FASerial"]);
                    t.SfaSerial = Convert.ToInt32(dr["SFASerial"]);
                    t.Target = (string)(dr["TargetText"]);
                    t.Suitability = Convert.ToDouble(dr["Suitability"]);
                    t.Originality = Convert.ToDouble(dr["Originality"]);
                    t.NumOfUses = Convert.ToInt32(dr["NumOfUses"]);
                    t.FunctionArea = (string)(dr["FunctionArea"]);
                    t.SubFunctionArea = (string)(dr["SubFunctionArea"]);
                    t.TeacherId = (string)(dr["TeacherId"]);
                    t.CreationDate = Convert.ToDateTime(dr["CreationDate"]);

                    targetList.Add(t);
                }

                return targetList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Targets For Survey from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public void InsertToolsAndGoals(List<Targets> targets, string StudentId, int CurrentYear)
        {

            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM TargetsInTala WHERE StudentId = " + StudentId + " and TYear = " + CurrentYear;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    if (Convert.ToInt32(dr["Tserial"]) > 0)
                    {
                        for (int i = 0; i < targets.Count(); i++)
                        {
                            if (targets[i].TarSerial == Convert.ToInt32(dr["Tserial"]))
                            {
                                for (int j = 0; j < targets[i].Goals.Count; j++)
                                {
                                    InsertGoal(Convert.ToInt32(dr["Tindex"]), StudentId, targets[i].Goals[j], CurrentYear);
                                }
                                for (int j = 0; j < targets[i].Tools.Count; j++)
                                {
                                    InsertTool(Convert.ToInt32(dr["Tindex"]), StudentId, targets[i].Tools[j], CurrentYear);
                                }
                            }
                        }  
                    }
                    else
                    {
                        for (int i = 0; i < targets.Count(); i++)
                        {
                            if (targets[i].Target == (string)(dr["NewPhrase"]))
                            {
                                for (int j = 0; j < targets[i].Goals.Count; j++)
                                {
                                    InsertGoal(Convert.ToInt32(dr["Tindex"]), StudentId, targets[i].Goals[j], CurrentYear);
                                }
                                for (int j = 0; j < targets[i].Tools.Count; j++)
                                {
                                    InsertTool(Convert.ToInt32(dr["Tindex"]), StudentId, targets[i].Tools[j], CurrentYear);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET TT Areas from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public int InsertGoal(int ttSerial, string StudentId, string goal, int CurrentYear)
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

            String cStr = BuildInsertGoalCommand(goal, ttSerial, StudentId, CurrentYear);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("תלא לא התווספה למערכת", ex);
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

        private String BuildInsertGoalCommand(string goal, int ttSerial, string StudentId, int CurrentYear)
        {

            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}')", goal, ttSerial, StudentId, CurrentYear);
            prefix = "INSERT INTO Goals " + "([Goal], [TalaSerial], [StudentId], [TYear])";

            command = prefix + sb.ToString();

            return command;
        }

        public int InsertTool(int ttSerial, string StudentId, string tool, int CurrentYear)
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

            String cStr = BuildInsertToolCommand(tool, ttSerial, StudentId, CurrentYear);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("תלא לא התווספה למערכת", ex);
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

        private String BuildInsertToolCommand(string tool, int ttSerial, string StudentId, int CurrentYear)
        {

            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}')", tool, ttSerial, StudentId, CurrentYear);
            prefix = "INSERT INTO Tools " + "([Tool], [TalaSerial], [StudentId], [TYear])";

            command = prefix + sb.ToString();

            return command;
        }

        public int UpdateNumOfUses(int targetSer)
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

            String cStr = BuildUpdateNumOfUsesCommand(targetSer);      // helper method to build the insert string

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

        private String BuildUpdateNumOfUsesCommand(int targetSer)
        {
            String command;
            command = "UPDATE Targets SET NumOfUses = NumOfUses+1 WHERE Tserial = " + targetSer;

            return command;
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

                sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", teacher.TeacherID, teacher.TeacherFname, teacher.TeacherSurName, teacher.TeacherEmail, teacher.TeacherPassword, teacher.TeacherSchoolId);
                prefix = "INSERT INTO Teachers " + "([TId], [TFirstName], [TLastName], [TEmail], [TPassword], [TSchool])";


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

        public int InsertChararcteristics(List<Chararcteristics> Chararcteristic, string SId, int year)
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

            String cStr = BuildInsertCommand(Chararcteristic, SId, year);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not insert new Character for a student", ex);
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

        private String BuildInsertCommand(List<Chararcteristics> c, string SId, int year)
        {
            String command;
            //String prefix;
            StringBuilder sb = new StringBuilder();

            string ones="";
            string columns = "([StudentId], [SCYear]";
            int i = 2;
            foreach (var item in c)
            {
                ones += ", 1";
                columns += ", [char_" + item.CharacteristicKey + "]";
                i++;

                if (item.CharacteristicKey < 0)
                    insertFreeChar(item, SId, year);
            }
            columns += ")";
            ones+= ")";
            command = "INSERT INTO CharacteristicsMatrix " + columns + " VALUES (" + SId+", "+ year + ones;

            return command;
        }

        public int insertFreeChar(Chararcteristics Chararcteristic, string SId, int year)
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

            String cStr = BuildInsertFreeCharCommand(Chararcteristic, SId, year);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not insert new Character for a student", ex);
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

        private String BuildInsertFreeCharCommand(Chararcteristics c, string SId, int year)
        {
            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', '{2}')", c.Chararcteristic, SId, year);
            prefix = "INSERT INTO ChararcteristicsForStudent " + "([Chararcteristic], [StudentId], [year])";

            command = prefix + sb.ToString();

            return command;
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

        public int setNewPass(string tPass, string tEmail)
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

            String cStr = BuildUpdateCommand(tPass,tEmail);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("לא ניתן לעדכן סיסמא", ex);
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
     
        private String BuildUpdateCommand(string tPass, string tMail)
        {
            String command;
            command = "UPDATE Teachers SET TPassword = '" + tPass +"' WHERE TEmail = '" + tMail + "'";
            return command;
        }

        public int updateTinfo(int tS, string tC, int tY, string tE)
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

            String cStr = BuildUpdateCommand(tS, tC, tY, tE);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("לא ניתן לעדכן כיתה", ex);
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

        private String BuildUpdateCommand(int tS, string tC, int tY, string tE)
        {
            String command;
            command = "UPDATE Teachers SET TSchool = '" + tS + "', TYear ='" +tY + "', TClass ='" + tC +  "' WHERE TEmail = '" + tE + "'";
            return command;
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
                        t.TeacherSchoolId = Convert.ToInt32(dr["TSchool"]);
                        t.TeacherYear = Convert.ToInt32(dr["TYear"]);
                        t.TeacherClass = (string)dr["TClass"];

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
            prefix = "INSERT INTO Targets " + "([FASerial], [SFASerial], [TargetText], [Suitability], [Originality], [NumOfUses])";
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
                ", SFASerial = " + t.SfaSerial + ", TargetText = '" + t.Target + "', Suitability = " + t.Suitability +
                ", Originality = " + t.Originality + " WHERE Tserial = " + t.TarSerial;

            return command;
        }

        public int DeleteChars(Chararcteristics chars, string SId, int year)
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

            String cStr = BuildDeleteCommand(chars, SId, year);      // helper method to build the insert string

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

        private String BuildDeleteCommand(Chararcteristics c, string SId, int year)
        {
            String command;
            command = "UPDATE CharacteristicsMatrix SET char_"+c.CharacteristicKey +"=0 WHERE StudentId = '" + SId + "' and SCYear = "+ year;
            return command;
        }

        public List<Students> ReadStudents()
        {

            SqlConnection con = null;
            List<Students> StudentsList = new List<Students>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Student ORDER BY SFirstName ASC";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Students s = new Students();

                    s.Dis1st = Convert.ToInt32(dr["1stDis"]);
                    s.Dis2nd = Convert.ToInt32(dr["2ndDis"]);
                    s.StudentId = (string)(dr["StudentId"]);
                    s.SFirstName = (string)(dr["SFirstName"]);
                    s.SLastName = (string)(dr["SLastName"]);
                    s.SEmail = (string)(dr["SEmail"]);
                    s.SGender = (string)(dr["SGender"]);
                    s.SAddress = (string)(dr["SAddress"]);
                    s.SPhone = (string)(dr["SPhone"]);
                    s.SDescripion = (string)(dr["SDescripion"]);
                    s.SBirthDate = Convert.ToDateTime(dr["SBirthDate"]);
                    s.MedicalSituation = (string)(dr["MedicalSituation"]);

                    StudentsList.Add(s);
                }

                return StudentsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Students from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public List<Students> ReadStudentsPerTecher(string teacherId, int year)
        {
            List<Students> StudentsList = new List<Students>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT S.* FROM Teachers T inner join StudentsInClass SIC on T.TSchool = SIC.SCSchId and T.TYear = SIC.SCYear and T.TClass = SIC.SCName " +
                                    "inner join Student S on SIC.SCstdID = S.StudentId WHERE T.TId = " + teacherId + " and T.TYear = " + year + " ORDER BY SLastName ASC";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Students s = new Students();

                    s.Dis1st = Convert.ToInt32(dr["1stDis"]);
                    s.Dis2nd = Convert.ToInt32(dr["2ndDis"]);
                    s.StudentId = (string)(dr["StudentId"]);
                    s.SFirstName = (string)(dr["SFirstName"]);
                    s.SLastName = (string)(dr["SLastName"]);
                    s.SEmail = (string)(dr["SEmail"]);
                    s.SGender = (string)(dr["SGender"]);
                    s.SAddress = (string)(dr["SAddress"]);
                    s.SPhone = (string)(dr["SPhone"]);
                    s.SDescripion = (string)(dr["SDescripion"]);
                    s.SBirthDate = Convert.ToDateTime(dr["SBirthDate"]);
                    s.MedicalSituation = (string)(dr["MedicalSituation"]);

                    StudentsList.Add(s);
                }

                return StudentsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Students from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public Talas ReadTala(string studentId, int year)
        {
            Talas t = new Talas();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Tala WHERE StudentId="+ studentId +" and TYear="+ year;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row

                    t.CurrentYear = Convert.ToInt32(dr["TYear"]);
                    t.StudentId = (string)(dr["StudentId"]);     
                }

                return t;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Students from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public Talas CheckIfHasTala(int SID)
        {
            Talas t = new Talas();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Tala WHERE StudentId=" + SID;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    t.StudentId = (string)(dr["StudentId"]);
                }

                return t;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Students from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public List<Goals> ReadGoals(string studentId, int year)
        {
            List<Goals> gList = new List<Goals>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Goals WHERE StudentId=" + studentId + " and TYear=" + year;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Goals g = new Goals();

                    g.GoalId = Convert.ToInt32(dr["GId"]);
                    g.SerialTarget = Convert.ToInt32(dr["TalaSerial"]);
                    g.Year = Convert.ToInt32(dr["TYear"]);
                    g.StudentId = (string)(dr["StudentId"]);
                    g.Goal = (string)(dr["Goal"]);
                    g.GoalStatus = (string)(dr["Status"]);

                    gList.Add(g);
                }

                return gList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Goals for student from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public int Insert(Students Student)
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

            String cStr = BuildInsertCommand(Student);      // helper method to build the insert string

           cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("לא נוצר תלמיד חדש במערכת", ex);
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

        private String BuildInsertCommand(Students s)
        {
            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();
            if (s.MedicalSituation == null)
                s.MedicalSituation = "";
            sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}' , '{10}', '{11}')", s.Dis1st, s.Dis2nd, s.StudentId, s.SFirstName, s.SLastName, s.SEmail, s.SGender, s.SAddress, s.MedicalSituation, s.SPhone, s.SDescripion,  s.SBirthDate.Year + "/" + s.SBirthDate.Month + "/" + s.SBirthDate.Day);
            prefix = "INSERT INTO Student " + "([1stDis], [2ndDis], [StudentId], [SFirstName], [SLastName], [SEmail], [SGender], [SAddress], [MedicalSituation], [SPhone], [SDescripion], [SBirthDate])";

            command = prefix + sb.ToString();

           return command;

        }

        public int Update(Students Student)
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

            String cStr = BuildUpdateCommand(Student);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("תלמיד לא עודכן", ex);
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

        private String BuildUpdateCommand(Students s)
        {
            String command;
            if (s.MedicalSituation == null)
                s.MedicalSituation = "";
            command = "UPDATE Student SET [1stDis] = " + s.Dis1st + ", [2ndDis] = " + s.Dis2nd + ", SDescripion = '" + s.SDescripion + "', SGender = '" + s.SGender +
                "', SFirstName = '" + s.SFirstName + "', SLastName = '" + s.SLastName + "', SEmail = '" + s.SEmail + "', SAddress = '" + s.SAddress + "', MedicalSituation = '" + s.MedicalSituation +
                "', SPhone = '" + s.SPhone + "', SBirthDate = '" + s.SBirthDate.Year+'/'+ s.SBirthDate.Month + '/' + s.SBirthDate.Day +  "' WHERE StudentId = " + s.StudentId;
            return command;
        }

        public string SendMailToUser(string UserMail, string UserRandomPassword)
        {

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("rupi41.2021@gmail.com");
                mail.To.Add(UserMail);
                mail.Subject = "שיחזור סיסמא דיגיתלא";
                mail.Body = "שלום" + Environment.NewLine +
                "הסיסמא הזמנית הינה:  " + UserRandomPassword;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("rupi41.2021@gmail.com", "igroup41_45920");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                //MessageBox.Show("mail Send");

                return UserRandomPassword;
            }
            catch (Exception ex)
            {
                throw (ex);
                //MessageBox.Show(ex.ToString());
            }
        }

        public List<Targets> ReadTargetsById(string matchStudentId, int matchYear)
        {

            SqlConnection con = null;
            List<Targets> targetList = new List<Targets>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select TT.*, T.Suitability, T.Originality, T.NumOfUses, T.TargetText, FA.FunctionArea from TargetsInTala TT left join Targets T on T.Tserial=TT.Tserial join FunctionAreas FA on FA.FASerial=TT.FASerial"
                    + " where TT.StudentId = " + matchStudentId + " and TT.TYear = " + matchYear;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Targets t = new Targets();                 
                    if (dr["NewPhrase"] == DBNull.Value)
                        t.Target = (string)(dr["TargetText"]);                  
                    else
                        t.Target = (string)(dr["NewPhrase"]);

                    t.TarTalaIndex = Convert.ToInt32(dr["Tindex"]);
                    t.TarSerial = Convert.ToInt32(dr["TSerial"]);
                    t.FaSerial = Convert.ToInt32(dr["FASerial"]);
                    t.SfaSerial = Convert.ToInt32(dr["SFASerial"]);
                    if (t.TarSerial == 0)
                    {
                        t.Suitability = 0;
                        t.Originality = 0;
                        t.NumOfUses = 0;
                    }
                    else
                    {
                        t.Suitability = Convert.ToDouble(dr["Suitability"]);
                        t.Originality = Convert.ToDouble(dr["Originality"]);
                        t.NumOfUses = Convert.ToInt32(dr["NumOfUses"]);
                    }

                    t.FunctionArea = (string)(dr["FunctionArea"]);

                    targetList.Add(t);
                }

                return targetList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Targets From Tala from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public RecommendedTargets ActivateRecommendation(RecommendedTargets rt, int dis1, int dis2)
        {

            SqlConnection con = null;
            List<RecommendedTargets> MatchStudentsList = new List<RecommendedTargets>();
            RecommendedTargets Chosen = new RecommendedTargets();
            int countMatch = 0;
            int missmatch = 0;
            double score = 0;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select cm.* from CharacteristicsMatrix cm left join Student s on s.StudentId = cm.StudentId where s.[1stDis] = " + dis1 + " or s.[1stDis] = " + dis2 + " or s.[2ndDis] = " + dis2 + " or s.[2ndDis] = " + dis1;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                while (dr.Read())
                {
                    if ((string)dr["StudentId"] != rt.NewStudentId)
                    {
                        for (int i = 1; i <= dr.FieldCount-2; i++)
                        {
                            if (Convert.ToInt32(dr["char_" + i]) == 1)
                                missmatch++;
                        }
                        for (int j = 0; j < rt.NewStudentChars.Count; j++)
                        {
                            if (rt.NewStudentChars[j].CharacteristicKey < dr.FieldCount - 2)
                            {
                                for (int k = 1; k < dr.FieldCount; k++)
                                {
                                    string c = "char_" + k;
                                    if ("char_" + rt.NewStudentChars[j].CharacteristicKey == c)
                                    {
                                        if (Convert.ToInt32(dr[c]) == 1)
                                        {
                                            countMatch++;
                                            missmatch--;
                                        }                                                                              
                                        break;
                                    }
                                }
                            }
                        }

                        if (countMatch > missmatch)
                            score = countMatch * 0.8 + (dr.FieldCount - 2 - countMatch - missmatch) * 0.2;
                        else
                            score = 0;

                        RecommendedTargets tempR = new RecommendedTargets();

                        tempR.CountMatch = score;
                        tempR.MatchStudentId = (string)dr["StudentId"];
                        tempR.MatchYear = Convert.ToInt32(dr["SCYear"]);

                        MatchStudentsList.Add(tempR);

                        countMatch = 0;
                        missmatch = 0;
                        score = 0;
                    }
                }

                double max = 0;
                foreach (var item in MatchStudentsList)
                {
                    if (item.CountMatch > max)
                    {
                        max = item.CountMatch;
                        Chosen.CountMatch = max;
                        Chosen.MatchStudentId = item.MatchStudentId;
                        Chosen.MatchYear = item.MatchYear;
                    }
                }

                if(Chosen.CountMatch > 0)
                    Chosen.Recommendations = ReadTargetsById(Chosen.MatchStudentId, Chosen.MatchYear);

                else
                {
                    List <Targets> TargetsList = ReadTargets();
                    List<Chararcteristics> chars = ReadChararcteristics();
                    List<Targets> recommended = new List<Targets>();

                    for (int i = 0; i < rt.NewStudentChars.Count; i++)
                    {
                        for (int j = 0; j < chars.Count; j++)
                        {
                            if (rt.NewStudentChars[i].CharacteristicKey == chars[j].CharacteristicKey) {
                                if (chars[j].IsWeakness == true)
                                    rt.NewStudentChars[i].SfaSerial = chars[j].SfaSerial;
                                else
                                    rt.NewStudentChars[i].SfaSerial = 0;
                            }
                        }
                    }

                    int max6 = 0;
                    int max2 = 0;
                    for (int i = 0; i < rt.NewStudentChars.Count; i++)
                    {
                        max2 = 0;
                        for (int j = 0; j < TargetsList.Count; j++)
                        {
                            if (rt.NewStudentChars[i].SfaSerial == TargetsList[j].SfaSerial)
                            {
                                max6++;
                                max2++;
                                recommended.Add(TargetsList[j]);
                                if(max2 == 2)
                                    break;
                            }
                        }

                        if (max6 == 6)
                            break;
                    }
                    Chosen.Recommendations = recommended;
                }
                            
                Chosen.CurrentYear = rt.CurrentYear;
                Chosen.NewStudentId = rt.NewStudentId;
                Chosen.NewStudentChars = rt.NewStudentChars;

                return Chosen;

            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Targets To Tala from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public int DeleteFreeCharsForStudent(Chararcteristics rt)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildDeleteFreeCharsCommand(rt.CharacteristicKey);

            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("לא ניתן לעדכן מאפייני תלמיד", ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        private String BuildDeleteFreeCharsCommand(int key)
        {
            String command;
            //String prefix;
            StringBuilder sb = new StringBuilder();

            command = "Delete FROM [ChararcteristicsForStudent] WHERE [CharacteristicKey] = "+key;

            return command;
        }

        public int DeleteCharsForStudent(int key, string SId, int year)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildDeleteCharsCommand(key, SId, year);

            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("לא ניתן לעדכן מאפייני תלמיד", ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        private String BuildDeleteCharsCommand(int key, string SId, int year)
        {
            String command;
            //String prefix;
            StringBuilder sb = new StringBuilder();

            command = "UPDATE CharacteristicsMatrix SET [char_" + key + "]=0 WHERE [StudentId] = " + SId + " and [SCYear] = " + year;

            return command;
        }

        public int Update(Goals goal)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildUpdateCommand(goal);   

            cmd = CreateCommand(cStr, con);  

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); 
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("יעד לא עודכן", ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        private String BuildUpdateCommand(Goals g)
        {
            String command;
            command = "UPDATE Goals SET Goal = '" + g.Goal +
                "', Status = '" + g.GoalStatus + "', TalaSerial = " + g.SerialTarget + ", StudentId = '" + g.StudentId +
                "', TYear = " + g.Year + " WHERE GId = " + g.GoalId;

            return command;
        }

        public int Insert(Custodian Cust)
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

            String cStr = BuildInsertCommand(Cust);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("לא נוצר אופוטרופוס חדש במערכת", ex);
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

        private String BuildInsertCommand(Custodian c)
        {
            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}')", c.CEmail, c.CName, c.CPhone, c.CSID);
            prefix = "INSERT INTO Custodian " + "([CEmail], [CFullName], [CPhone], [StudentId])";


            command = prefix + sb.ToString();

            return command;

        }

        public int insertSinClass(int s, string c, int y, int i)
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

            String cStr = BuildInsertCommand(s, c, y, i);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("לא ניתן לעדכן כיתה", ex);
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

        private String BuildInsertCommand(int s, string c, int y, int i)
        {
            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}')", s, c, y, i );
            prefix = "INSERT INTO StudentsInClass " + "([SCSchId], [SCName], [SCYear], [SCstdID])";


            command = prefix + sb.ToString();

            return command;

        }

        public int DeleteStudent(int sid)
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

            String cStr = BuildDeleteStudentCommand(sid);      // helper method to build the insert string

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

        private String BuildDeleteStudentCommand(int sID)
        {
            String command;
            command = "Delete FROM Custodian WHERE StudentId = '" + sID + "' Delete FROM StudentsInClass WHERE SCstdID = '" + sID + "' Delete FROM Student WHERE StudentId = '" + sID + "'";
            return command;
        }

        public Custodian ReadCustodiansForStudent(string studentId)
        {
            Custodian c = new Custodian();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Custodian WHERE StudentId=" + studentId;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row

                    c.CSID = (string)(dr["StudentId"]);
                    c.CEmail = (string)(dr["CEmail"]);
                    c.CName = (string)(dr["CFullName"]);
                    c.CPhone = (string)(dr["CPhone"]);

                }

                return c;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Custodians from DB", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public int Update(Custodian cust)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildUpdateCommand(cust);

            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("איש קשר לא עודכן", ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        private String BuildUpdateCommand(Custodian c)
        {
            String command;
            command = "UPDATE Custodian SET CEmail = '" + c.CEmail + "', CFullName = '" + c.CName + "', CPhone = '" + c.CPhone + "' WHERE StudentId = " + c.CSID;

            return command;
        }

        public int UpdateTargetsAchievement(int serialTar, char status)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not connect to DB", ex);
            }

            String cStr = BuildUpdateAchievementCommand(serialTar, status);

            cmd = CreateCommand(cStr, con);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("סטטוס מטרה לא עודכן", ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        private String BuildUpdateAchievementCommand(int serialTar, char status)
        {
            String command;
            command = "UPDATE TargetsInTala SET Achieved = '" + status + "' WHERE Tindex = " + serialTar;

            return command;
        }


        public List<Tools> ReadTools(int sid, int syear)
        {

            SqlConnection con = null;
            List<Tools> toolsList = new List<Tools>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Tools t where t.StudentId=" + sid +  "and t.TYear=" + syear;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Tools t = new Tools();

                    t.StudentId = Convert.ToInt32(dr["StudentId"]);
                    t.Tid = Convert.ToInt32(dr["TId"]);
                    t.Tserial = Convert.ToInt32(dr["TalaSerial"]);
                    t.Year = Convert.ToInt32(dr["TYear"]);
                    t.Tool = (string)(dr["Tool"]);

                    toolsList.Add(t);
                }

                return toolsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Could not GET Students from DB", ex);
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

