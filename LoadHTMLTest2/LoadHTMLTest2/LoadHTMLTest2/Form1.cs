using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HtmlAgilityPack;
using System.Xml;
using System.Data.Sql;
using System.Data.SqlClient;

namespace LoadHTMLTest2
{
    public partial class Form1 : Form
    {
        SqlCommand cmd;         //Declare Sql command to run query in SQL
        SqlConnection con;      //Declare the SQL connection to 
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCrawl_Click(object sender, EventArgs e)
        {
            //Load the Data source and server here include the username and password
            con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
            //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=myVirtualSupportAvisor;Integrated Security=True
            //Data Source=uwstrikersserver.database.windows.net;Initial Catalog=PriceStrikers;Persist Security Info=True;User ID=imdaboss;Password=Youdaboss1


            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();      //Declare a new HTML document object
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();                //Delcare a new HTML web object

            for (int pageNum = 21; pageNum <= 30; pageNum++) //Declare the number of HTML file for running through a list
            {
                //D:\Dell D-Drive\Tai Lieu Mon Hoc\UW Bothell\CSS 498 - Independent Study (Pattern Recognition)\1-48
                //C:/Users/tungl/Downloads/CSS499/Plans_fixed/
                doc = web.Load("C:/Plans/" + pageNum+".html");       //This is where we load HTML link (local file)

                #region Variables Field
                bool parameterResult = false;
                String courseLetter = "";                   //Declare courseLetter variable
                String courseNumber = "";                   //Declare course Number variable
                String courseNumberYes = "";                //Declare course Number with &
                String courseNumberNo = "";                 //Declare course Number without &
                String quarterPlanTitle = "";               //Declare quarter Plan Title
                String quarter = "";                        //Declare quarter variable
                String year = "";                           //Declare year variable
                String TimePreference = "";                 //Declare Time Preference Variable
                String SummerPreference = "";               //Declare Summer Preference Variable
                String StartQuarter = "";                   //Declare Start Quarter Variable   
                String EnrollmentType = "";                 //Declare Enrollment Type Variable
                String JobType = "";                        //Declare Job Type Variable

                String Major = "";                          //Declare Major Variable
                String School = "";                         //Declare School Variable
                String Stars = "";                          //Declare Stars String Variable
                String Reason = "";                         //Declare Reason Variable                           
                int StarsInt = 1;                           //Declare Starr Int Variable
                #endregion

                #region Load data from HTML using XPath
                HtmlNodeCollection timePreferenceNode = doc.DocumentNode.SelectNodes("//*[@id='TimePreference']"); //Load the Time Preference
                foreach (var item in timePreferenceNode)
                {
                    TimePreference = item.InnerText;         //assign Time Preference by the inner text of the Xpath 
                    //MessageBox.Show(TimePreference);
                }

                HtmlNodeCollection summerPreferenceNode = doc.DocumentNode.SelectNodes("//*[@id='SummerPreference']"); //Load the Summer Preference
                foreach (var item in summerPreferenceNode)
                {
                    SummerPreference = item.InnerText;         //assign Summer Preferece by the inner text of the Xpath 
                    //MessageBox.Show(SummerPreference);
                }

                HtmlNodeCollection startQuarterNode = doc.DocumentNode.SelectNodes("//*[@id='StartQuarter']"); //Load the Start Quarter
                foreach (var item in startQuarterNode)
                {
                    StartQuarter = item.InnerText;         //assign Start Quarter by the inner text of the Xpath 
                    //MessageBox.Show(StartQuarter);
                }

                HtmlNodeCollection EnrollmentTypeNode = doc.DocumentNode.SelectNodes("//*[@id='EnrollmentType']"); //Load the Enrollment Type
                foreach (var item in EnrollmentTypeNode)
                {
                    EnrollmentType = item.InnerText;         //assign Enrollment Type by the inner text of the Xpath 
                    //MessageBox.Show(EnrollmentType);
                }

                HtmlNodeCollection jobTypeNode = doc.DocumentNode.SelectNodes("//*[@id='JobType']"); //Load the Job Type
                foreach (var item in jobTypeNode)
                {
                    JobType = item.InnerText;         //assign Job Type by the inner text of the Xpath 
                    //MessageBox.Show(JobType);
                }

                HtmlNodeCollection majorNode = doc.DocumentNode.SelectNodes("//*[@id='Major']"); //Load the Major
                foreach (var item in majorNode)
                {
                    Major = item.InnerText;         //assign Major by the inner text of the Xpath 
                    //MessageBox.Show(Major);
                }
                HtmlNodeCollection schoolNode = doc.DocumentNode.SelectNodes("//*[@id='School']"); //Load the School
                foreach (var item in schoolNode)
                {
                    School = item.InnerText;         //assign School by the inner text of the Xpath 
                    //MessageBox.Show(School);
                }
                HtmlNodeCollection starsNode = doc.DocumentNode.SelectNodes("//*[@id='Grade']"); //Load the Stars
                foreach (var item in starsNode)
                {
                    Stars = item.InnerText;         //assign Stars by the inner text of the Xpath
                    StarsInt = int.Parse(Stars);
                    StarsInt = Convert.ToInt32(Stars);
                    //if (StarsInt == 1)
                    //  MessageBox.Show(Stars);
                }
                HtmlNodeCollection reasonNode = doc.DocumentNode.SelectNodes("//*[@id='Reason']"); //Load the Reason
                foreach (var item in reasonNode)
                {
                    Reason = item.InnerText;         //assign Reason by the inner text of the Xpath
                    //MessageBox.Show(Reason);
                }
                #endregion

                //Check parameterSet in database first, if no match parameter then we create a new parameterSet
                //string query6 = "select DescriptionID,Stars from planRating;";
                string query6 = "select ParameterSet.ID from ParameterSet where MajorID IN (select Major.ID from Major where Major.Name = '"+Major+"') AND SchoolID IN (select School.ID from School where School.Acronymn = '"+School+"') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '"+JobType+"')AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '"+TimePreference+"') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '"+StartQuarter+"') AND SummerPreference = '"+SummerPreference+"' AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '"+EnrollmentType+"'); ";
                #region First method to save data from query
                /*cmd = new SqlCommand(query6, con);       //create a new command by query through the connection
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);                 //Declare a new SQL data reader
                try
                {
                    con.Open();
                    // Now you have a collection of rows that you can iterate over
                    foreach (DataRow row in table.Rows)
                    {
                        parameterResult = row["ParameterSet.ID"].ToString();
                        MessageBox.Show("parameterID=" + parameterResult);
                    }

                }
                catch (Exception ex)                    //Catch the exception
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();                        //Close the connection
                }
                MessageBox.Show("parameterID=" + parameterResult);*/
                #endregion

                #region Second method to save data from SQL query
                /*List<string> tempList = new List<string>();
                try
                {

                    SqlCommand command = new SqlCommand(query6, con);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            tempList.Add(reader[0].ToString());
                        }
                    }
                    reader.Close();
                }
                finally
                {
                    con.Close();                        //Close the connection
                }
                string[] parameterID = tempList.ToArray();
                foreach (string param in parameterID)
                {
                    MessageBox.Show(param);
                }*/
                #endregion

                #region Third method to save data from SQL query
                try
                {
                    SqlCommand comm = new SqlCommand(query6, con);
                    con.Open();

                    SqlDataReader reader = comm.ExecuteReader();
                    List<string> str = new List<string>();
                    int numColumns = 1;
                    while (reader.Read())
                    {
                       
                        for(int i=0; i<numColumns; i++)
                        str.Add(reader.GetValue(i).ToString());
                        
                    }
                    reader.Close();
                    foreach(string s in str) {
                        MessageBox.Show(s);
                    }
                    if (str.Count != 0)
                        parameterResult = true;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }

                #endregion

                #region ParameterSet Creation
                //Create a new parameterSet unless there is no match paramaterSet in the database
                if (parameterResult == false)
                {
                    string query = "insert into ParameterSet(MajorID,SchoolID,JobTypeID,TimePreferenceID,QuarterPreferenceID,DateAdded,LastDateModified,Status,SummerPreference,EnrollmentType) select Major.ID,School.ID,JobType.ID,TimePreference.ID,Quarter.QuarterID,DateAdded = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status=1,SummerPreference ='" + SummerPreference + "',EnrollmentType.ID from Major,School,JobType,TimePreference,Quarter,EnrollmentType where Major.Name = '" + Major + "'AND School.Acronymn = '" + School + "'AND JobType.JobType = '" + JobType + "'AND TimePreference.TimePeriod = '" + TimePreference + "'AND Quarter.Quarter = '" + StartQuarter + "'AND EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "';";
                    cmd = new SqlCommand(query, con);       //create a new command by query through the connection
                    SqlDataReader myreader;                 //Declare a new SQL data reader
                    try
                    {
                        con.Open();                         //Open the connection
                        myreader = cmd.ExecuteReader();     //Execute the command
                                                            //MessageBox.Show("Saved Parameter Set");
                        while (myreader.Read())
                        {

                        }
                    }
                    catch (Exception ex)                    //Catch the exception
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        con.Close();                        //Close the connection
                    }
                }
                else {
                    parameterResult = false;
                }
                #endregion

                #region GeneratedPlan Creation
                //Create a new GeneratedPlan
                string query2 = "insert into GeneratedPlan(Name,ParameterSetID,DateAdded,LastDateModified,Status) select Name = 'Latest',ParameterSet.ID,DateAdded = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status=1 from ParameterSet where MajorID IN (select Major.ID from Major where Major.Name = '" + Major + "') AND SchoolID IN (select School.ID from School where School.Acronymn = '" + School + "') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '" + JobType + "') AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '" + TimePreference + "') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + StartQuarter + "')AND SummerPreference = '" + SummerPreference + "' AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "'); ";
                cmd = new SqlCommand(query2, con);       //create a new command by query through the connection
                SqlDataReader myreader2;                 //Declare a new SQL data reader
                try
                {
                    con.Open();                         //open the connection
                    myreader2 = cmd.ExecuteReader();        //Execute the command
                    //MessageBox.Show("Saved Generated Plan");
                    while (myreader2.Read())
                    {

                    }
                }
                catch (Exception ex)                //Catch the exception
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();                    //Close the connection
                }
                #endregion

                #region StudentStudyPlan
                //Create a new StudentStudyPlan
                string query3 = "insert into StudentStudyPlan(StudentID,PlanID,CreationDate,LastDateModified,Status) select StudentID = 777, GeneratedPlan.ID,creationDate = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status = 1 from GeneratedPlan where GeneratedPlan.ID IN (select GeneratedPlan.ID from GeneratedPlan where ParameterSetID IN( select ParameterSet.ID from ParameterSet where MajorID IN (select Major.ID from Major where Major.Name = '" + Major + "') AND SchoolID IN (select School.ID from School where School.Acronymn = '" + School + "') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '" + JobType + "') AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '" + TimePreference + "') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + StartQuarter + "') AND SummerPreference = '" + SummerPreference + "' AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "'))); ";
                cmd = new SqlCommand(query3, con);       //create a new command by query through the connection
                SqlDataReader myreader3;                 //Declare a new SQL data reader
                try
                {
                    con.Open();                         //Open connection
                    myreader3 = cmd.ExecuteReader();    //Execute the command
                    //MessageBox.Show("Saved Student Study Plan");
                    while (myreader3.Read())
                    {

                    }
                }
                catch (Exception ex)                    //Catch the exception
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();                        //Close the connection
                }
                #endregion

                #region PlanRating Creation
                //Create a new plan Rating
                //Modify planRatingDescription to all lower case first
                string temp = "";
                foreach (char z in Reason)
                {
                    temp += Char.ToLower(z);
                }
                Reason = temp;

                //Find the latest Generate Plan
                string query7 = "select MAX(ID) from GeneratedPlan;";
                String currentPlanString = "";
                try
                {
                    SqlCommand comm = new SqlCommand(query7, con);
                    con.Open();

                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                            currentPlanString = (reader.GetValue(0).ToString());

                    }
                    reader.Close();
                    //MessageBox.Show(currentPlanString);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }
                //Convert from string result to Integer
                int currentPlanInt = Int32.Parse(currentPlanString);
                //string query4 = "insert into PlanRating(PlanID,DescriptionID,Stars) select GeneratedPlan.ID,PlanRatingDescription.ID,Stars = " + StarsInt + " from GeneratedPlan, PlanRatingDescription where GeneratedPlan.ID IN (select GeneratedPlan.ID from GeneratedPlan where ParameterSetID IN ( select ParameterSet.ID from ParameterSet where MajorID IN(select Major.ID from Major where Major.Name = '" + Major + "') AND SchoolID IN(select School.ID from School where School.Acronymn = '" + School + "') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '" + JobType + "') AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '" + TimePreference + "') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + StartQuarter + "') AND SummerPreference = '" + SummerPreference + "'AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "'))) AND PlanRatingDescription.ID IN ( select ID from PlanRatingDescription where Description = '" + Reason + "'); ";
                //string query4 = "insert into PlanRating(PlanID,DescriptionID,Stars) select GeneratedPlan.ID = "+pageNum+",PlanRatingDescription.ID,Stars = "+StarsInt+" from PlanRatingDescription where PlanRatingDescription.ID IN (select ID from PlanRatingDescription where Description = '"+Reason+"'); ";
                string query4 = "insert into PlanRating(PlanID,DescriptionID,Stars) select PlanID = " + currentPlanInt + ", PlanRatingDescription.ID,Stars = " + StarsInt + " from PlanRatingDescription where PlanRatingDescription.ID IN (select ID from PlanRatingDescription where Description = '" + Reason + "'); ";
                cmd = new SqlCommand(query4, con);       //create a new command by query through the connection
                SqlDataReader myreader4;                 //Declare a new SQL data reader
                try
                {
                    con.Open();                         //Open the connection
                    myreader4 = cmd.ExecuteReader();            //Execute the command
                    //MessageBox.Show("Saved Student Plan Rating");   
                    while (myreader4.Read())
                    {

                    }
                }
                catch (Exception ex)                //Catch the exception
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();                    //Close the connection
                }
                #endregion

                #region StudyPlan creation
                for (int i = 1; i <= 20; i++)           //Loop through all quarter and year in a single HTML file
                {
                    quarter = "";
                    year = "";
                    String xpath = "//*[@id='QP" + i + "_LBL_QPlanTitle']";     //Load the quarter Plan title
                    HtmlNodeCollection quarterPlanTitleNode = doc.DocumentNode.SelectNodes(xpath);
                    foreach (var item in quarterPlanTitleNode)
                    {
                        quarterPlanTitle = item.InnerText;
                        foreach (char z in quarterPlanTitle)
                        {
                            if (Char.IsDigit(z))                //Year is the digit after character in the quarterPlanTitle String
                                year += z;
                            else
                                quarter += z;
                        }
                    }
                    for (int j = 1; j <= 5; j++)
                    {
                        courseNumber = "";
                        courseLetter = "";
                        courseNumberNo = "";
                        courseNumberYes = "";
                        String xpath2 = "//*[@id='QP" + i + "_TB_Line" + j + "']";      //Load the course number
                        String val = doc.DocumentNode.SelectSingleNode(xpath2).GetAttributeValue("value", "default");
                        if (val != " ")
                        {
                            bool check = false;
                            foreach (char z in val)
                            {
                                if (z.Equals('&'))
                                {
                                    check = true;
                                }
                            }
                            foreach (char z in val)
                            {
                                if (!z.Equals('/'))
                                {                 //only read the first course Number, co-requisite would not be considered in this case
                                    if (check == true)  //In case the original string does have &
                                    {
                                        if (z.Equals('&'))
                                        {
                                            courseNumberYes += Char.ToUpper(z);     //Add original courseNumberYes only
                                        }
                                        else
                                        {
                                            courseNumberYes += Char.ToUpper(z);     //Add to both CourseNumber
                                            courseNumberNo += Char.ToUpper(z);
                                        }
                                    }
                                    else
                                    {      //In case the original string does not have &
                                        if (Char.IsLetter(z))
                                        {
                                            courseLetter += z;
                                        }
                                        else if (Char.IsDigit(z))
                                            courseNumber += z;
                                    }
                                }
                                else
                                    break;
                            }
                            if (!check)
                            {
                                courseNumberYes = courseLetter + "& " + courseNumber;
                                courseNumberNo = courseLetter + " " + courseNumber;
                            }
                            //MessageBox.Show(courseNumberYes);
                            //MessageBox.Show(courseNumberNo);
                            //ADD a new StudyPlan
                            string query5 = "insert into StudyPlan(PlanID,QuarterID,YearID,CourseID,DateAdded,LastDateModified) select GeneratedPlan.ID,Quarter.QuarterID,YearID = " + year + ",course.CourseID,DateAdded = CURRENT_TIMESTAMP, LastDateModified = CURRENT_TIMESTAMP from GeneratedPlan,Quarter,course where GeneratedPlan.ID IN (select GeneratedPlan.ID from GeneratedPlan where ParameterSetID IN ( select ParameterSet.ID from ParameterSet where MajorID IN (select Major.ID from Major where Major.Name = '" + Major + "') AND SchoolID IN (select School.ID from School where School.Acronymn = '" + School + "') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '" + JobType + "') AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '" + TimePreference + "') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + StartQuarter + "')AND SummerPreference = '" + SummerPreference + "' AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "'))) AND Quarter.QuarterID IN (select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + quarter + "') AND course.CourseID IN (select course.CourseID from course where course.CourseNumber= '" + courseNumberNo + "'OR course.CourseNumber = '" + courseNumberYes + "'); ";
                            cmd = new SqlCommand(query5, con);       //create a new command by query through the connection
                            SqlDataReader myreader5;                 //Declare a new SQL data reader
                            try
                            {
                                con.Open();                         //Open the connection
                                myreader5 = cmd.ExecuteReader();        //Execute the command
                                //MessageBox.Show("Saved Study Plan");      //using to test StudyPlan
                                while (myreader5.Read())
                                {

                                }
                            }
                            catch (Exception ex)                    //Catch the exception
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                con.Close();                        //Close the connection
                            }
                        }
                    }

                }
            }
            MessageBox.Show("Done");
            #endregion
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
