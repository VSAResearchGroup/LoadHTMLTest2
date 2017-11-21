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
        SqlCommand cmd;
        SqlConnection con;
        //SqlDataAdapter da;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCrawl_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Data Source=uwstrikersserver.database.windows.net;Initial Catalog=PriceStrikers;Persist Security Info=True;User ID=imdaboss;Password=Youdaboss1");
            //con.Open();
            String planTitle="";
            //String courseName = "MATH 142";
            String quarterPlanTitle="Fall2019";

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            doc = web.Load("C:/Users/tungl/Desktop/DegreeAudit_12.html");
            HtmlNodeCollection planTitleNode = doc.DocumentNode.SelectNodes("//*[@id='DD_PlanTitle']");
            foreach (var item in planTitleNode)
            {
                planTitle = item.InnerText;
                MessageBox.Show(planTitle);
            }
            for(int i = 1; i <= 20; i++)
            {
                String xpath = "//*[@id='QP" + i + "_LBL_QPlanTitle']";
                HtmlNodeCollection quarterPlanTitleNode = doc.DocumentNode.SelectNodes(xpath);
                foreach (var item in quarterPlanTitleNode)
                {
                    quarterPlanTitle = item.InnerText;
                    MessageBox.Show(quarterPlanTitle);
                }
                for (int j = 1; j <= 5; j++)
                {
                    String xpath2 = "//*[@id='QP"+i+"_TB_Line" + j + "']";
                    String val = doc.DocumentNode.SelectSingleNode(xpath2).GetAttributeValue("value", "default");
                    if (val != "") 
                        MessageBox.Show(val);

                }
            }

            /*string query = "insert into studyPlan select planID, courseID  from PLANS, course where planName = '"+planTitle+"' AND courseName = '"+courseName+"'; ";
            cmd = new SqlCommand(query, con);
            SqlDataReader myreader;
            try
            {
                con.Open();
                myreader = cmd.ExecuteReader();
                MessageBox.Show("Saved");
                while (myreader.Read())
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/

            /*string query = "INSERT INTO AcademicPlan VALUES('" + studentName + "','" + studentID + "','" + planTitle + "','" + quarterPlanTitle + "');";
            cmd = new SqlCommand(query, con);
            SqlDataReader myreader;
            try
            {
                con.Open();
                myreader = cmd.ExecuteReader();
                MessageBox.Show("Saved");
                while (myreader.Read())
                {

                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }*/
             
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
