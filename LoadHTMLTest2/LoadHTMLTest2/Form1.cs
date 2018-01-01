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
            con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=myVirtualSupportAvisor;Integrated Security=True");
            //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=myVirtualSupportAvisor;Integrated Security=True
            //Data Source=uwstrikersserver.database.windows.net;Initial Catalog=PriceStrikers;Persist Security Info=True;User ID=imdaboss;Password=Youdaboss1
            

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();      //Declare a new HTML document object
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();                //Delcare a new HTML web object

            for (int pageNum = 1; pageNum <= 5; pageNum++) //Declare the number of HTML file for running through a list
            {
                doc = web.Load("C:/Users/tungl/Downloads/CSS499/All_Plans/"+pageNum+".html");       //This is where we load HTML link (local file)

                String courseNumber = "";                   //Declare course Number variable
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

                HtmlNodeCollection timePreferenceNode = doc.DocumentNode.SelectNodes("//*[@id='TimePreference']"); //Load the Time Preference
                foreach (var item in timePreferenceNode)
                {
                    TimePreference = item.InnerText;         //assign Time Preference by the inner text of the Xpath 
                    MessageBox.Show(TimePreference);
                }

                HtmlNodeCollection summerPreferenceNode = doc.DocumentNode.SelectNodes("//*[@id='SummerPreference']"); //Load the Summer Preference
                foreach (var item in summerPreferenceNode)
                {
                    SummerPreference = item.InnerText;         //assign Summer Preferece by the inner text of the Xpath 
                    MessageBox.Show(SummerPreference);
                }

                HtmlNodeCollection startQuarterNode = doc.DocumentNode.SelectNodes("//*[@id='StartQuarter']"); //Load the Start Quarter
                foreach (var item in startQuarterNode)
                {
                    StartQuarter = item.InnerText;         //assign Start Quarter by the inner text of the Xpath 
                    MessageBox.Show(StartQuarter);
                }

                HtmlNodeCollection EnrollmentTypeNode = doc.DocumentNode.SelectNodes("//*[@id='EnrollmentType']"); //Load the Enrollment Type
                foreach (var item in EnrollmentTypeNode)
                {
                    EnrollmentType = item.InnerText;         //assign Enrollment Type by the inner text of the Xpath 
                    MessageBox.Show(EnrollmentType);
                }

                HtmlNodeCollection jobTypeNode = doc.DocumentNode.SelectNodes("//*[@id='JobType']"); //Load the Job Type
                foreach (var item in jobTypeNode)
                {
                    JobType = item.InnerText;         //assign Job Type by the inner text of the Xpath 
                    MessageBox.Show(JobType);
                }

                HtmlNodeCollection majorNode = doc.DocumentNode.SelectNodes("//*[@id='Major']"); //Load the Major
                foreach (var item in majorNode)
                {
                    Major = item.InnerText;         //assign Major by the inner text of the Xpath 
                    MessageBox.Show(Major);
                }
                HtmlNodeCollection schoolNode = doc.DocumentNode.SelectNodes("//*[@id='School']"); //Load the School
                foreach (var item in schoolNode)
                {
                    School = item.InnerText;         //assign School by the inner text of the Xpath 
                    MessageBox.Show(School);
                }
                HtmlNodeCollection starsNode = doc.DocumentNode.SelectNodes("//*[@id='Grade']"); //Load the Stars
                foreach (var item in starsNode)
                {
                    Stars = item.InnerText;         //assign Stars by the inner text of the Xpath
                    StarsInt = int.Parse(Stars);
                    StarsInt = Convert.ToInt32(Stars);
                    if (StarsInt == 1)
                        MessageBox.Show(Stars);
                }
                HtmlNodeCollection reasonNode = doc.DocumentNode.SelectNodes("//*[@id='Reason']"); //Load the Reason
                foreach (var item in reasonNode)
                {
                    Reason = item.InnerText;         //assign Reason by the inner text of the Xpath
                    MessageBox.Show(Reason);
                }
                //Create a new parameterSet
                string query = "insert into ParameterSet(MajorID,SchoolID,JobTypeID,TimePreferenceID,QuarterPreferenceID,DateAdded,LastDateModified,Status,SummerPreference,EnrollmentType) select Major.ID,School.ID,JobType.ID,TimePreference.ID,Quarter.QuarterID,DateAdded = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status=1,SummerPreference ='" + SummerPreference + "',EnrollmentType.ID from Major,School,JobType,TimePreference,Quarter,EnrollmentType where Major.Name = '" + Major + "'AND School.Acronymn = '" + School + "'AND JobType.JobType = '" + JobType + "'AND TimePreference.TimePeriod = '" + TimePreference + "'AND Quarter.Quarter = '" + StartQuarter + "'AND EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "';";
                cmd = new SqlCommand(query, con);       //create a new command by query through the connection
                SqlDataReader myreader;                 //Declare a new SQL data reader
                try
                {
                    con.Open();                         //Open the connection
                    myreader = cmd.ExecuteReader();     //Execute the command
                    MessageBox.Show("Saved Parameter Set");
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

                //Create a new GeneratedPlan
                string query2 = "insert into GeneratedPlan(Name,ParameterSetID,DateAdded,LastDateModified,Status) select Name = 'Latest',ParameterSet.ID,DateAdded = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status=1 from ParameterSet where MajorID IN (select Major.ID from Major where Major.Name = '" + Major + "') AND SchoolID IN (select School.ID from School where School.Acronymn = '" + School + "') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '" + JobType + "') AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '" + TimePreference + "') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + StartQuarter + "')AND SummerPreference = '" + SummerPreference + "' AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "'); ";
                cmd = new SqlCommand(query2, con);       //create a new command by query through the connection
                SqlDataReader myreader2;                 //Declare a new SQL data reader
                try
                {
                    con.Open();                         //open the connection
                    myreader2 = cmd.ExecuteReader();        //Execute the command
                    MessageBox.Show("Saved Generated Plan");
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

                //Create a new StudentStudyPlan
                string query3 = "insert into StudentStudyPlan(StudentID,PlanID,CreationDate,LastDateModified,Status) select StudentID = 777, GeneratedPlan.ID,creationDate = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status = 1 from GeneratedPlan where GeneratedPlan.ID IN (select GeneratedPlan.ID from GeneratedPlan where ParameterSetID IN( select ParameterSet.ID from ParameterSet where MajorID IN (select Major.ID from Major where Major.Name = '" + Major + "') AND SchoolID IN (select School.ID from School where School.Acronymn = '" + School + "') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '" + JobType + "') AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '" + TimePreference + "') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + StartQuarter + "') AND SummerPreference = '" + SummerPreference + "' AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "'))); ";
                cmd = new SqlCommand(query3, con);       //create a new command by query through the connection
                SqlDataReader myreader3;                 //Declare a new SQL data reader
                try
                {
                    con.Open();                         //Open connection
                    myreader3 = cmd.ExecuteReader();    //Execute the command
                    MessageBox.Show("Saved Student Study Plan");
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

                //Create a new plan Rating
                //string query4 = "insert into PlanRating(PlanID,DescriptionID,Stars) select GeneratedPlan.ID,PlanRatingDescription.ID,Stars = "+StarsInt+ " from GeneratedPlan,PlanRatingDescription where GeneratedPlan.ID IN (select GeneratedPlan.ID from GeneratedPlan where ParameterSetID IN( select ParameterSet.ID from ParameterSet where MajorID IN (select Major.ID from Major where Major.Name = '" + Major + "') AND SchoolID IN (select School.ID from School where School.Acronymn = '" + School + "') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '" + JobType + "') AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '" + TimePreference + "') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + StartQuarter + "') AND SummerPreference = '" + SummerPreference + "' AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "')))AND PlanRatingDescription.ID IN (select ID from PlanRatingDescription where Description = '"+Reason+"'); ";
                string query4 = "insert into PlanRating(PlanID,DescriptionID,Stars) select GeneratedPlan.ID,PlanRatingDescription.ID,Stars = " + StarsInt + " from GeneratedPlan, PlanRatingDescription where GeneratedPlan.ID IN (select GeneratedPlan.ID from GeneratedPlan where ParameterSetID IN ( select ParameterSet.ID from ParameterSet where MajorID IN(select Major.ID from Major where Major.Name = '" + Major + "') AND SchoolID IN(select School.ID from School where School.Acronymn = '" + School + "') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '" + JobType + "') AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '" + TimePreference + "') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + StartQuarter + "') AND SummerPreference = '" + SummerPreference + "'AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "'))) AND PlanRatingDescription.ID IN ( select ID from PlanRatingDescription where Description = '" + Reason + "'); ";
                cmd = new SqlCommand(query4, con);       //create a new command by query through the connection
                SqlDataReader myreader4;                 //Declare a new SQL data reader
                try
                {
                    con.Open();                         //Open the connection
                    myreader4 = cmd.ExecuteReader();            //Execute the command
                    MessageBox.Show("Saved Student Plan Rating");   
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
                        String xpath2 = "//*[@id='QP" + i + "_TB_Line" + j + "']";      //Load the course number
                        String val = doc.DocumentNode.SelectSingleNode(xpath2).GetAttributeValue("value", "default");
                        if (val != " ")
                        {
                            foreach (char z in val)
                            {
                                if (!z.Equals('/'))                 //only read the first course Number, co-requisite would not be considered in this case
                                    courseNumber += z;
                                else
                                    break;
                            }
                            //MessageBox.Show(courseNumber);
                            //ADD a new StudyPlan
                            string query5 = "insert into StudyPlan(PlanID,QuarterID,YearID,CourseID,DateAdded,LastDateModified) select GeneratedPlan.ID,Quarter.QuarterID,YearID = 2016,course.CourseID,DateAdded = CURRENT_TIMESTAMP, LastDateModified = CURRENT_TIMESTAMP from GeneratedPlan,Quarter,course where GeneratedPlan.ID IN (select GeneratedPlan.ID from GeneratedPlan where ParameterSetID IN ( select ParameterSet.ID from ParameterSet where MajorID IN (select Major.ID from Major where Major.Name = '" + Major + "') AND SchoolID IN (select School.ID from School where School.Acronymn = '" + School + "') AND JobTypeID IN (select JobType.ID from JobType where JobType.JobType = '" + JobType + "') AND TimePreferenceID IN (select TimePreference.ID from TimePreference where TimePreference.TimePeriod = '" + TimePreference + "') AND QuarterPreferenceID IN (Select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + StartQuarter + "')AND SummerPreference = '" + SummerPreference + "' AND EnrollmentType IN (select EnrollmentType.ID from EnrollmentType where EnrollmentType.EnrollmentDescription = '" + EnrollmentType + "'))) AND Quarter.QuarterID IN (select Quarter.QuarterID from Quarter where Quarter.Quarter = '" + quarter + "') AND course.CourseID IN (select course.CourseID from course where course.CourseNumber= '" + courseNumber + "'); ";
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
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
