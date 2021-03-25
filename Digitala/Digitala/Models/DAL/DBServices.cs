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

                String selectSTR = "SELECT * FROM Chararcteristics";
                
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
                //values += ", '{" + i + "}'";
                ones += ", 1";
                columns += ", [char_" + item.CharacteristicKey + "]";
                i++;
            }
            //values += ")";
            columns += ")";
            ones+= ")";
            command = "INSERT INTO CharacteristicsMatrix " + columns + " VALUES (" + SId+", "+ year + ones;

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

            sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')", s.Dis1st, s.Dis2nd, s.StudentId, s.SFirstName, s.SLastName, s.SEmail, s.SGender, s.SAddress, s.SPhone, s.SDescripion, s.SBirthDate);
            prefix = "INSERT INTO Student " + "([1stDis], [2ndDis], [StudentId], [SFirstName], [SLastName], [SEmail], [SGender], [SAddress], [SPhone], [SDescripion], [SBirthDate])";
           

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
            command = "UPDATE Student SET 1stDis = " + s.Dis1st + ", 2ndDis = " + s.Dis2nd + ", SDescripion = '" + s.SDescripion + "', SGender = " + s.SGender +
                ", SFirstName = '" + s.SFirstName + "', SLastName = '" + s.SLastName + "', SEmail = '" + s.SEmail + "', SAddress = '" + s.SAddress +
                "', SPhone = " + s.SPhone + ", SBirthDate = " + s.SBirthDate +  " WHERE StudentId = " + s.StudentId;
            return command;
        }

        public void SendMailToUser(string UserMail, string UserRandomPassword)
        {

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("rupi41.2021@gmail.com");
                mail.To.Add(UserMail);
                mail.Subject = "Welcome to SpeakUp";
                mail.Body = "שלום" + Environment.NewLine +
                "הסיסמא הזמנית הינה:  " + UserRandomPassword;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("rupi41.2021@gmail.com", "igroup41_45920");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                //MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                throw (ex);
                //MessageBox.Show(ex.ToString());
            }
        }

        public List<Targets> ReadTargetsById(RecommendedTargets r)
        {

            SqlConnection con = null;
            List<Targets> targetList = new List<Targets>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select * from TargetsInTala where StudentId = " + r.MatchStudentId + " and TYear = " + r.MatchYear;

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
            int disMatch = 0;
            double score = 0;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "Select cm.* from CharacteristicsMatrix cm left join Student s on s.StudentId = cm.StudentId where s.[1stDis] = " + dis1 + " or s.[1stDis] = " + dis2 + " or s.[2ndDis] = " + dis2 + " or s.[2ndDis] = " + dis1;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                if (dr.Depth > 0)
                {
                    while (dr.Read())
                    {
                        if ((string)dr["StudentId"] != rt.NewStudentId)
                        {
                            for (int j = 0; j < rt.NewStudentChars.Count; j++)
                            {
                                for (int k = 1; k <= dr.FieldCount; k++)
                                {
                                    if ("char_" + rt.NewStudentChars[j].CharacteristicKey == "char_" + k)
                                    {
                                        if (Convert.ToInt32(dr["char_" + k]) == 1)
                                            countMatch++;
                                        else
                                            disMatch++;
                                    }
                                }
                            }

                            score = countMatch * 0.75 + (dr.FieldCount - 2 - countMatch - disMatch) * 0.25;

                            RecommendedTargets tempR = new RecommendedTargets();

                            tempR.CountMatch = score;
                            tempR.MatchStudentId = (string)dr["StudentId"];
                            tempR.MatchYear = Convert.ToInt32(dr["SCYear"]);

                            MatchStudentsList.Add(tempR);

                            countMatch = 0;
                            disMatch = 0;
                            score = 0;
                        }
                    }

                    double max = 0;
                    //we can check => to max and count it. if there are more than numOfStudent/5 similar - to bring random student between them
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
                    Chosen.Recommendations = ReadTargetsById(Chosen);
                }
                else
                {
                    List <Targets> TargetsList = ReadTargets();
                    List<Chararcteristics> chars= ReadChararcteristics();
                    List<Targets> recommended = new List<Targets>();
                    for (int i = 0; i < rt.NewStudentChars.Count; i++)
                    {
                        for (int j = 0; j < chars.Count; j++)
                        {
                            if (rt.NewStudentChars[i].CharacteristicKey == chars[j].CharacteristicKey)
                                rt.NewStudentChars[i].SfaSerial = chars[j].SfaSerial;
                        }
                    }

                    int max5 = 0;
                    for (int i = 0; i < rt.NewStudentChars.Count; i++)
                    {
                        for (int j = 0; j < TargetsList.Count; j++)
                        {
                            if (rt.NewStudentChars[i].SfaSerial == TargetsList[j].SfaSerial)
                            {
                                max5++;
                                recommended.Add(TargetsList[j]);
                                break;
                            }
                        }

                        if (max5 >= 5)
                            break;
                    }
                    Chosen.Recommendations = recommended;
                }
                            
                Chosen.CurrentYear = rt.CurrentYear;
                Chosen.NewStudentId = rt.NewStudentId;
                Chosen.NewStudentChars = rt.NewStudentChars;

                //*******MOVE TO INSERT TALA FUNCTION*********//

                InsertChararcteristics(rt.NewStudentChars, rt.NewStudentId, rt.CurrentYear);


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

        //public int TEMP(int id)
        //{

        //    SqlConnection con;
        //    SqlCommand cmd;

        //    try
        //    {
        //        con = connect("DBConnectionString"); // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Could not connect to DB", ex);
        //    }

        //    String cStr = BuildTEMPCommand(id);      // helper method to build the insert string

        //    cmd = CreateCommand(cStr, con);             // create the command

        //    try
        //    {
        //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
        //        return numEffected;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Faild removing item", ex);
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            // close the db connection
        //            con.Close();
        //        }
        //    }

        //}

        //private String BuildTEMPCommand(int id)
        //{
        //    String command;
        //    command = "ALTER TABLE dbo.[CharacteristicsMatrix] ADD char_" + id + " INT DEFAULT(0) NULL";
        //    return command;


        //}

    }
   
}

