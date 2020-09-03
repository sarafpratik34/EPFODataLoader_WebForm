﻿using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
namespace EPFODataLoader_WebForm
{
    public partial class _Default : Page
    {
        public static int monthIndex;
        DataTable dt = new DataTable();
        string fileName = string.Empty;
        string result = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            HelloWorldLabel.Text = "Hello ";

            foreach (BAL.MonthOption mo in Enum.GetValues(typeof(BAL.MonthOption)))
            {
                ListItem item = new ListItem(Enum.GetName(typeof(BAL.MonthOption), mo), mo.ToString());
                Month.Items.Add(item);
            }
        }

        protected void GreetBotton_Click(object sender, EventArgs e)
        {
            HelloWorldLabel.Text = "Hello " + TextInput.Text;
            TextInput.Text = string.Empty;

            if (FileUpload1.HasFile)
            {
                string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                fileName = FileUpload1.FileName;

                //getting the path of the file   
                FileUpload1.SaveAs(Server.MapPath("~/MyFolder/" + FileUpload1.FileName));
                string path = Server.MapPath("~/MyFolder/" + FileUpload1.FileName);

                //DataTable dt = new DataTable();
                dt = ExcelUtilities.ReadDBFfile(path, monthIndex);

                //return something to UI
                gvExcelFile.DataSource = dt;
                gvExcelFile.DataBind();

                Label1.Text = "File Uploaded: " + FileUpload1.FileName;
            }
            else
            {
                Label1.Text = "No File Uploaded.";
            }
        }

        /// <summary>
        /// Help link: //https://stackoverflow.com/a/25718674
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DownloadTxt(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                fileName = FileUpload1.FileName;

                //getting the path of the file   
                FileUpload1.SaveAs(Server.MapPath("~/MyFolder/" + FileUpload1.FileName));
                string path = Server.MapPath("~/MyFolder/" + FileUpload1.FileName);

                //DataTable dt = new DataTable();
                dt = ExcelUtilities.ReadDBFfile(path, monthIndex);

                //DownloadTxt(fileName);
                result = ExcelUtilities.DumpDBFToTxt(dt).ToString();
                string txtFilePath = fileName.Replace("DBF", "txt");
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename= ~/MyFolder/" + txtFilePath);
                Response.AddHeader("content-type", "text/plain");

                using (StreamWriter writer = new StreamWriter(Response.OutputStream))
                {
                    writer.WriteLine(result);
                }
                Response.End();
            }
            else
            {
                Label1.Text = "No File Uploaded.";
            }
        }

        protected void GreetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            HelloWorldLabel.Text = "Hello " + Month.SelectedValue;
            monthIndex = Month.SelectedIndex;
            monthIndex++;   //incrementing index by 1 as ddl list starts by index zero
        }
    }
}
