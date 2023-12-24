using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

public partial class RMGBCSP_SSS_Report : System.Web.UI.Page
{
  SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

  protected void Page_Load(object sender, EventArgs e)
  {
    viewSSSReport();
  }

  protected void submit_Click(object sender, EventArgs e)
  {
    try
    {
      // Get the CSP ID from the session
      string cspId = Session["Name"] as string;

      if (!string.IsNullOrEmpty(cspId))
      {
        // Check if values are provided for APY, SBY, and JDY
        if (string.IsNullOrEmpty(apyInput.Text) || string.IsNullOrEmpty(sbyInput.Text) || string.IsNullOrEmpty(jjbyInput.Text))
        {
          // Display an alert and return
          ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please fill in all the required fields.');", true);
          return;
        }

        // Your existing connection setup
        con.Open();

        var dt = new DataTable();
        var dt1 = new DataTable();

        // Your SQL insert command without parameterized queries
        DateTime dst1 = DateTime.Now;
        string dst2 = dst1.ToString("yyyy-MM-dd");

        new SqlDataAdapter("Select * from SSS_Report where ReportDate BETWEEN '" + dst2 + " 00:00:00:000' AND '" + dst2 + " 23:59:59:000' AND CspId ='" + cspId + "';", con).Fill(dt);
        new SqlDataAdapter("SELECT email FROM login WHERE CspId = '" + cspId + "'", con).Fill(dt1);
        if (dt.Rows.Count > 0)
        {
          string id = dt.Rows[0]["Id"].ToString();
          SqlCommand cmd = new SqlCommand("UPDATE [SSS_Report] SET ReportDate = GETDATE(), APY ='" + apyInput.Text + "', SBY ='" + sbyInput.Text + "', JJBY ='" + jjbyInput.Text + "' WHERE Id ='" + id + "';", con);

          int i = cmd.ExecuteNonQuery();
          if (i > 0)
          {
            // Display an alert using ScriptManager
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Details Updated Successfully!');window.location ='SSS_Report.aspx';", true);
          }
        }

        else
        {
          SqlCommand cmd = new SqlCommand("Insert into SSS_Report(MemType,ReportDate, CspId, APY, SBY, JJBY) Values('E',GETDATE(),'" + cspId + "','" + apyInput.Text + "','" + sbyInput.Text + "','" + jjbyInput.Text + "');", con);

          int i = cmd.ExecuteNonQuery();
          if (i > 0)
          {
            // Display an alert using ScriptManager
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Details Submitted Successfully!');window.location ='SSS_Report.aspx';", true);
          }
        }

        if (dt1.Rows.Count > 0)
        {

          string mailit = dt1.Rows[0]["email"].ToString();
          mail(mailit);
            }

          // Close the connection
          con.Close();

        // Clear the input fields after successful insertion
        apyInput.Text = string.Empty;
        sbyInput.Text = string.Empty;
        jjbyInput.Text = string.Empty;
      }
      else
      {
        // Handle the case where CSP ID is not available in the session
      }
    }
    catch (Exception ex)
    {
      // Handle the exception, show error message, etc.
    }
  }

  public void viewSSSReport()
  {
    try
    {
      string cspid = Session["empcode"].ToString();
      var dt = new DataTable();
      con.Open();
      new SqlDataAdapter("SELECT Id, ReportDate, CspId, APY, SBY, JJBY FROM SSS_Report WHERE CspId = '" + cspid + "' ORDER BY ReportDate ASC", con).Fill(dt);
      if (dt.Rows.Count > 0)
      {
        StringBuilder tableHtml = new StringBuilder();
        tableHtml.Append("<table id='example1' class='table'>");
        tableHtml.Append("<thead><tr><th>Sr No.</th><th>Csp ID</th><th>Date</th><th>Scheme-APY</th><th>Scheme-SBY</th><th>Scheme-JJBY</th></tr></thead>");
        tableHtml.Append("<tbody>");

        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
          DateTime dob = Convert.ToDateTime(row["ReportDate"].ToString());
          dob = dob.Date;
          string reportdate = dob.ToString("yyyy-MM-dd");
          tableHtml.Append("<tr>");
          tableHtml.Append("<td>" + ++i + "</td>");
          tableHtml.Append("<td>" + row["CspId"] + "</td>");
          tableHtml.Append("<td>" + reportdate + "</td>");
          tableHtml.Append("<td>" + row["APY"] + "</td>");
          tableHtml.Append("<td>" + row["SBY"] + "</td>");
          tableHtml.Append("<td>" + row["JJBY"] + "</td>");
          tableHtml.Append("</tr>");
        }
        tableHtml.Append("</tbody></table>");
        SSSReport_Details.InnerHtml = tableHtml.ToString();
      }
      con.Close();
    }
    catch { }

  }
  public void mail(string to)
  {
    try
    {
      string cspid = Session["empcode"].ToString();
      string sendmail = ConfigurationManager.AppSettings["EmailID"].ToString();
      Literal1.Text = "";

      string cc = "f20202023@pilani.bits-pilani.ac.in";
      string email = to;
      MailMessage message = new MailMessage(sendmail, email);
      //smtp.UseDefaultCredentials = false ;
      message.Subject = "RMGB SSS_Report Schemes";
      string emailBody = "<html><body>";
      emailBody += "<p style='color: black;'>Dear " + Session["LastName"] + " (" + cspid + "),</p><br/>";
      emailBody += "<p style='color: black;'>Following are the details entered by you:</p><br/>";
      var tables = new Action(() =>
      {


        StringBuilder tableHtml = new StringBuilder();
        tableHtml.Append("<table id='example1' class='table' style='border-collapse: collapse;width: 80%; margin: 0 auto;'>");
        tableHtml.Append("<thead><tr><th style='color: black; border: 1px solid black; width: 30%;'>SSS Schemes</th><th style='border: 1px solid black; width: 30%;'>Daily Target</th><th style='border: 1px solid black; width: 30%;'>Actual Number of Schemes Sold</th></tr></thead>");
        tableHtml.Append("<tbody>");
        tableHtml.Append("<tr><th style='color: black; border: 1px solid black; text-align: center;'>APY</th><th style='border: 1px solid black; text-align: center;'>5</th><td style='border: 1px solid black; text-align: center; color: " + (int.Parse(apyInput.Text) < 5 ? "red;" : "black;") + "'>" + apyInput.Text + "</td></tr>");
        tableHtml.Append("<tr><th style='color: black; border: 1px solid black; text-align: center;'>SBY</th><th style='border: 1px solid black; text-align: center;'>10</th><td style='border: 1px solid black; text-align: center; color: " + (int.Parse(sbyInput.Text) < 10 ? "red;" : "black;") + "'>" + sbyInput.Text + "</td></tr>");
        tableHtml.Append("<tr><th style='color: black; border: 1px solid black; text-align: center;'>JJBY</th><th style='border: 1px solid black; text-align: center;'>5</th><td style='border: 1px solid black; text-align: center; color: " + (int.Parse(jjbyInput.Text) < 5 ? "red;" : "black;") + "'>" + jjbyInput.Text + "</td></tr>");
        tableHtml.Append("</tbody></table>");
        SSSReport_Details.InnerHtml = tableHtml.ToString();
      });

      tables();
      emailBody += SSSReport_Details.InnerHtml;
      var apyValue = int.Parse(apyInput.Text);
      var sbyValue = int.Parse(sbyInput.Text);
      var jjbyValue = int.Parse(jjbyInput.Text);
      emailBody += "<br/>";
      var targets = new Dictionary<string, int>
{
    { "APY", int.Parse(apyInput.Text) },
    { "SBY", int.Parse(sbyInput.Text) },
    { "JJBY", int.Parse(jjbyInput.Text) }
};
      emailBody += "<p style='color: red;'>DATE: " + DateTime.Now.ToString("dd/MM/yyyy") + "</p><br/>";
      var warningMessage = "<div style='text-align: justify;'><span style='color: red; font-weight: bold;'>WARNING NOTICE: YOU HAVE NOT MET THE DAILY TARGET FOR ";
      var firstTargetNotMet = true;

      foreach (var target in targets)
      {
        string condition = "";
        if (target.Key == "APY" && target.Value < 5)
        {
          condition = "APY < 5";
        }
        else if (target.Key == "SBY" && target.Value < 10)
        {
          condition = "SBY < 10";
        }
        else if (target.Key == "JJBY" && target.Value < 5)
        {
          condition = "JJBY < 5";
        }

        if (!string.IsNullOrEmpty(condition))
        {
          if (!firstTargetNotMet)
          {
            warningMessage += ", ";
          }
          warningMessage += target.Key;
          firstTargetNotMet = false;
        }
      }

      if (!firstTargetNotMet)
      {
        warningMessage += ". IF YOU CONTINUE TO NOT MEET THE DAILY TARGETS, THEN YOUR CSP ID WILL BE PERMANENTLY DELETED, FOR WHICH YOU WILL BE SOLELY RESPONSIBLE. AFTER THAT THE PROCESS OF YOUR REPLACEMENT WILL BEGIN IMMEDIATELY.</span></div>";
        emailBody += warningMessage;
      }
      emailBody += "<p style='color: black;'>Regards,<br/>P2P Microfinance &amp; Allied Services</p>";
      emailBody += "</body></html>";

      // Set the email body and specify that it's HTML
      message.Body = emailBody;
      message.IsBodyHtml = true;
      message.CC.Add(new MailAddress(cc));
      SmtpClient smtp = new SmtpClient();
      smtp.Send(message);
    }
    catch (Exception ex)
    {
      try
      {

        if (con.State == ConnectionState.Closed) { con.Open(); }
        StackTrace st = new StackTrace();
        StackFrame sf = st.GetFrame(0);
        StackFrame stackFrame = new StackFrame(1, true);

        string method = stackFrame.GetMethod().ToString();
        int line = stackFrame.GetFileLineNumber();
        MethodBase currentMethodName = sf.GetMethod();
        string url = HttpContext.Current.Request.Url.AbsoluteUri;
        SqlCommand cmd1234 = new SqlCommand("insert into Error_15_16(module,date,IP,message,Loginid,pageurl)values('" + currentMethodName + "LINE NO:- " + line + "',DATEADD(mi,330,getdate()),'" + Page.Request.ServerVariables["REMOTE_ADDR"] + "',@message,'" + Session["loginid"] + "','" + url.ToString() + "')", con);
        if (ex.Message.ToString().Length >= 1000)
        {
          cmd1234.Parameters.AddWithValue("@message", ex.Message.Substring(1, 148));
        }
        else
        {
          cmd1234.Parameters.AddWithValue("@message", ex.Message.ToString());
        }

        cmd1234.ExecuteNonQuery();
        cmd1234.Dispose();
      }

      catch
      {

      }
    }

  }
}
