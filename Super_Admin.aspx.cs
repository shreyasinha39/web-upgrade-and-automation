using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Web.Services;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
using System.Net.Mail;
using ClosedXML.Excel;
using System.Net;
using DocumentFormat.OpenXml.Office.Word;
using System.Drawing;
using System.Activities.Expressions;

public partial class P2PSuperAdmin_Default : System.Web.UI.Page
{
  SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
  //SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring1"].ConnectionString);
  TurtAPI OBI = new TurtAPI();
  protected void Page_Load(object sender, EventArgs e)
  {
    //Test();
    if (!IsPostBack)
    {
      if (Convert.ToString(Session["Membertype"]) == "A")
      {
        Response.Redirect("~/P2P_LOGP/login.aspx");
        return;
      }
      if (Convert.ToString(Session["MemFbertype"]) == "ES")
      {
        Response.Redirect("~/P2P_LOGP/login.aspx");
        return;
      }
      if (Convert.ToString(Session["Membertype"]) == "U")
      {
        Response.Redirect("~/P2P_LOGP/login.aspx");
        return;
      }
    }

  }

  protected void ddl_bank_TextChanged(object sender, EventArgs e)
    {
    string bank = ddl_bank.SelectedValue;
    rad.Items.Clear();

    if(bank != "0")
    {
      rad.Items.Add(new ListItem("SSS Report", "0"));
      rad.Items.Add(new ListItem("Account Openings", "1"));

      if(bank == "ES")
      {
        rad.Items.Add(new ListItem("Loan Lead Generator", "2"));

      }
    }

  }

  protected void btnexport_Click(object sender, EventArgs e)
  {

    string memberType = ddl_bank.SelectedValue;
    string tab = rad.SelectedValue;

    if (tab == "0")
    { 
    using (SqlCommand cmd = new SqlCommand("SELECT * FROM SSS_Report WHERE membertype = '" + memberType + "';"))
    {
      using (SqlDataAdapter sda = new SqlDataAdapter())
      {
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        using (DataTable dt = new DataTable())
        {
          sda.Fill(dt);
          using (XLWorkbook wb = new XLWorkbook())
          {
            wb.Worksheets.Add(dt, "Customers");
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=SSS_Report_"+ ddl_bank.Text + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
              wb.SaveAs(MyMemoryStream);
              MyMemoryStream.WriteTo(Response.OutputStream);
              Response.Flush();
              Response.End();
            }
          }
        }
      }
    }
    }

    else if (tab == "1")
    {
     
        using (SqlCommand cmd = new SqlCommand("SELECT * FROM Account_Opening WHERE membertype = '" + memberType + "';"))
        {
          using (SqlDataAdapter sda = new SqlDataAdapter())
          {
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            using (DataTable dt = new DataTable())
            {
              sda.Fill(dt);
              using (XLWorkbook wb = new XLWorkbook())
              {
                wb.Worksheets.Add(dt, "Customers");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Account_Opening_" + ddl_bank.Text + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                  wb.SaveAs(MyMemoryStream);
                  MyMemoryStream.WriteTo(Response.OutputStream);
                  Response.Flush();
                  Response.End();
                }
              }
            }
          }
        }
      }

    else 
    {

      using (SqlCommand cmd = new SqlCommand("SELECT * FROM Loan_Lead WHERE membertype = '" + memberType + "';"))
      {
        using (SqlDataAdapter sda = new SqlDataAdapter())
        {
          cmd.Connection = con;
          sda.SelectCommand = cmd;
          using (DataTable dt = new DataTable())
          {
            sda.Fill(dt);
            using (XLWorkbook wb = new XLWorkbook())
            {
              wb.Worksheets.Add(dt, "Customers");
              Response.Clear();
              Response.Buffer = true;
              Response.Charset = "";
              Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
              Response.AddHeader("content-disposition", "attachment;filename=Loan_Lead_" + ddl_bank.Text + ".xlsx");
              using (MemoryStream MyMemoryStream = new MemoryStream())
              {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
              }
            }
          }
        }
      }
    }

  }

  protected void rad_TextChanged(object sender, EventArgs e)
  {
    var dt = new DataTable();
    con.Open();
    string memberType = ddl_bank.SelectedValue;
    string tab = rad.SelectedValue;
    if (tab == "0")
    {
      SqlCommand cmd = new SqlCommand("SELECT * from SSS_Report WHERE membertype = @MemberType ; ", con);
      cmd.Parameters.AddWithValue("@MemberType", memberType);
      SqlDataAdapter sda = new SqlDataAdapter();
      sda.SelectCommand = cmd;
      sda.Fill(dt);
      if (dt.Rows.Count > 0)
    {
      StringBuilder tableHtml = new StringBuilder();
      tableHtml.Append("<table id='example1' class='table'>");
      tableHtml.Append("<thead><tr><th>Sr No.</th><th>Csp ID</th><th>Date</th><th>Scheme-APY</th><th>Scheme-SBY</th><th>Scheme-JDY</th></tr></thead>");
      tableHtml.Append("<tbody>");

      int i = 0;
      foreach (DataRow row in dt.Rows)
      {
        DateTime dob = Convert.ToDateTime(row["ReportDate"].ToString());
        dob = dob.Date;
        string reportdate = dob.ToString("yyyy-MM-dd");
        tableHtml.Append("<tr>");
        tableHtml.Append("<td>" + ++i + "</td>");
        tableHtml.Append("<td>" + row["cspid"] + "</td>");
        tableHtml.Append("<td>" + reportdate + "</td>");
        tableHtml.Append("<td>" + row["apy"] + "</td>");
        tableHtml.Append("<td>" + row["sby"] + "</td>");
        tableHtml.Append("<td>" + row["jdy"] + "</td>");
        tableHtml.Append("</tr>");
      }
      tableHtml.Append("</tbody></table>");
      DetailsTable.InnerHtml = tableHtml.ToString();
    }
    }
    else if (tab == "1")
    {
      SqlCommand cmd = new SqlCommand("SELECT * from Account_Opening WHERE membertype = @MemberType ; ", con);
      cmd.Parameters.AddWithValue("@MemberType", memberType);
      SqlDataAdapter sda = new SqlDataAdapter();
      sda.SelectCommand = cmd;
      sda.Fill(dt);
      if (dt.Rows.Count > 0)
      {
        StringBuilder tableHtml = new StringBuilder();
        tableHtml.Append("<table id='example1' class='table'>");
        tableHtml.Append("<thead><tr><th>Sr No.</th><th>Csp ID</th><th>Date</th><th>PMJDY</th><th>RD_Number</th><th>FD_Number</th><th>RD_Amount</th><th>FD_Amount</th></tr></thead>");
        tableHtml.Append("<tbody>");

        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
          DateTime dob = Convert.ToDateTime(row["ReportDate"].ToString());
          dob = dob.Date;
          string reportdate = dob.ToString("yyyy-MM-dd");
          tableHtml.Append("<tr>");
          tableHtml.Append("<td>" + ++i + "</td>");
          tableHtml.Append("<td>" + row["cspid"] + "</td>");
          tableHtml.Append("<td>" + reportdate + "</td>");
          tableHtml.Append("<td>" + row["PMJDY"] + "</td>");
          tableHtml.Append("<td>" + row["RD_NUMBER"] + "</td>");
          tableHtml.Append("<td>" + row["FD_NUMBER"] + "</td>");
          tableHtml.Append("<td>" + row["RD_AMOUNT"] + "</td>");
          tableHtml.Append("<td>" + row["FD_AMOUNT"] + "</td>");
          tableHtml.Append("</tr>");
        }
        tableHtml.Append("</tbody></table>");
        DetailsTable.InnerHtml = tableHtml.ToString();
      }
    }
    else
    {
      SqlCommand cmd = new SqlCommand("SELECT * from Loan_Lead WHERE membertype = @MemberType ; ", con);
      cmd.Parameters.AddWithValue("@MemberType", memberType);
      SqlDataAdapter sda = new SqlDataAdapter();
      sda.SelectCommand = cmd; 
      sda.Fill(dt);
      if (dt.Rows.Count > 0)
      {
        StringBuilder tableHtml = new StringBuilder();
        tableHtml.Append("<table id='example1' class='table'>");
        tableHtml.Append("<thead><tr><th>Sr No.</th><th>Csp ID</th><th>Date</th><th>Loan Type</th><th>Amount</th><th>Applicant Name</th><th>Address</th><th>Pincode</th><th>Mobile</th></tr></thead>");
        tableHtml.Append("<tbody>");

        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
          DateTime dob = Convert.ToDateTime(row["ReportDate"].ToString());
          dob = dob.Date;
          string reportdate = dob.ToString("yyyy-MM-dd");
          tableHtml.Append("<tr>");
          tableHtml.Append("<td>" + ++i + "</td>");
          tableHtml.Append("<td>" + row["cspid"] + "</td>");
          tableHtml.Append("<td>" + reportdate + "</td>");
          tableHtml.Append("<td>" + row["Loan_Type"] + "</td>");
          tableHtml.Append("<td>" + row["Amount"] + "</td>");
          tableHtml.Append("<td>" + row["Applicant_Name"] + "</td>");
          tableHtml.Append("<td>" + row["Address"] + "</td>");
          tableHtml.Append("<td>" + row["Pincode"] + "</td>");
          tableHtml.Append("<td>" + row["Mobile"] + "</td>");
          tableHtml.Append("</tr>");
        }
        tableHtml.Append("</tbody></table>");
        DetailsTable.InnerHtml = tableHtml.ToString();
      }
    }

  }

}
