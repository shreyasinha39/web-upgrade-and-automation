using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RMGBCSP_Account_Opening : System.Web.UI.Page
{
  SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

  protected void Page_Load(object sender, EventArgs e)
  {
    viewaccs();
  }

  protected void submit_Click(object sender, EventArgs e)

  {
    try
    {
      string cspId = Session["Name"] as string;

      if (!string.IsNullOrEmpty(cspId))
      {

        if (string.IsNullOrEmpty(PMJDYInput.Text) || string.IsNullOrEmpty(RDInput.Text) || string.IsNullOrEmpty(FDInput.Text) || string.IsNullOrEmpty(RDAmountInput.Text) || string.IsNullOrEmpty(FDAmountInput.Text))

        {
          ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Please fill in all the required fields.');", true);
          return;
        }

        con.Open();


        var dt = new DataTable();
        var dt1 = new DataTable();

        // Your SQL insert command without parameterized queries
        DateTime dst1 = DateTime.Now;
        string dst2 = dst1.ToString("yyyy-MM-dd");

        new SqlDataAdapter("Select * from Acc_Openings where ReportDate BETWEEN '" + dst2 + " 00:00:00:000' AND '" + dst2 + " 23:59:59:000' AND CspId ='" + cspId + "';", con).Fill(dt);
        new SqlDataAdapter("SELECT email FROM login WHERE CspId = '" + cspId + "'", con).Fill(dt1);

        if (dt1.Rows.Count > 0)
        {

          string mailid = dt1.Rows[0]["email"].ToString();
          mail(mailid);
        }

        if (dt.Rows.Count > 0)
        {
          string id = dt.Rows[0]["Id"].ToString();
          SqlCommand cmd = new SqlCommand("UPDATE [Acc_Openings] SET ReportDate = GETDATE(), PMJDY = '" + PMJDYInput.Text + "', RD_NUMBER = '" + RDInput.Text + "', FD_NUMBER = '" + FDInput.Text + "', RD_AMOUNT = '" + RDAmountInput.Text + "', FD_AMOUNT = '" + FDAmountInput.Text + "' WHERE Id = '" + id + "';", con);

          int i = cmd.ExecuteNonQuery();
          if (i > 0)
          {
            // Display an alert using ScriptManager
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Details Updated Successfully!');window.location ='Account_Opening.aspx';", true);
          }
        }

        else
        {
          SqlCommand cmd = new SqlCommand("Insert into Acc_Openings(MemType,ReportDate, CspId, PMJDY, RD_NUMBER, FD_NUMBER, RD_AMOUNT, FD_AMOUNT) Values('E',GETDATE(),'" + cspId + "','" + PMJDYInput.Text + "','" + RDInput.Text + "','" + FDInput.Text + "','" + RDAmountInput.Text + "','" + FDAmountInput.Text + "');", con);

          int i = cmd.ExecuteNonQuery();
          if (i > 0)
          {
            // Display an alert using ScriptManager
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Details Submitted Successfully!');window.location ='Account_Opening.aspx';", true);
          }
        }

        con.Close();

      }
      else
      {
        Response.Redirect("~/P2P_LOGP/login.aspx");
        return;
      }
    }

    catch (Exception ex)

    {
    }
  }

  public void viewaccs()
  {
    try
    {
      string cspid = Session["Name"].ToString();
      var dt = new DataTable();
      con.Open();
      new SqlDataAdapter("SELECT Id, ReportDate, PMJDY , RD_NUMBER, FD_NUMBER, RD_AMOUNT, FD_AMOUNT FROM Acc_Openings WHERE CspId = '" + cspid + "' ORDER BY ReportDate ASC", con).Fill(dt);
      if (dt.Rows.Count > 0)
      {
        StringBuilder tableHtml = new StringBuilder();
        tableHtml.Append("<table id='example1' class='table'>");
        tableHtml.Append("<thead><tr><th>Sr No.</th><th>Report Date</th><th>PMJDY</th><th>RD Number</th><th>RD Amount</th><th>FD Number</th><th>FD Amount</th></tr></thead>");
        tableHtml.Append("<tbody>");

        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
          DateTime dob = Convert.ToDateTime(row["ReportDate"].ToString());
          string reportdate = dob.ToString("yyyy-MM-dd");

          tableHtml.Append("<tr>");
          tableHtml.Append("<td>" + ++i + "</td>");
          tableHtml.Append("<td>" + reportdate + "</td>");
          tableHtml.Append("<td>" + row["PMJDY"] + "</td>");
          tableHtml.Append("<td>" + row["RD_NUMBER"] + "</td>");
          tableHtml.Append("<td>" + row["RD_AMOUNT"] + "</td>");
          tableHtml.Append("<td>" + row["FD_NUMBER"] + "</td>");
          tableHtml.Append("<td>" + row["FD_AMOUNT"] + "</td>");
          tableHtml.Append("</tr>");
        }
        tableHtml.Append("</tbody></table>");
        AccTable.InnerHtml = tableHtml.ToString();
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
      message.Subject = "RMGB Account_Openings";
      string emailBody = "<html><body>";
      emailBody += "<p style='color: black;'>Dear " + Session["LastName"] + " (" + cspid + "),</p><br/>";
      emailBody += "<p style='color: black;'>Following are the details entered by you:</p><br/>";
      var tables = new Action(() =>
      {


        StringBuilder tableHtml = new StringBuilder();
        tableHtml.Append("<table id='example1' class='table' style='border-collapse: collapse;width: 80%; margin: 0 auto;'>");
        tableHtml.Append("<thead><tr><th style='border: 1px solid black; width: 30%;'>Schemes</th><th style='border: 1px solid black; width: 30%;'>Daily Target</th><th style='border: 1px solid black; width: 30%;'>Number of Accounts Opened</th><th style='border: 1px solid black; width: 30%;'>Total Amount</th></tr></thead>");
        tableHtml.Append("<tbody>");
        tableHtml.Append("<tr><th style='border: 1px solid black;'>PMJDY</th><th style='border: 1px solid black; text-align: center;'>5</th><td style='border: 1px solid black; text-align: center; color: " + (int.Parse(PMJDYInput.Text) < 5 ? "red;" : "black;") + "'>" + PMJDYInput.Text + "</td></tr>");
        tableHtml.Append("<tr><th style='border: 1px solid black;'>RD</th><th style='border: 1px solid black; text-align: center;'>5</th><td style='border: 1px solid black; text-align: center; color: " + (int.Parse(RDInput.Text) < 5 ? "red;" : "black;") + "'>" + RDInput.Text + "</td><td style='border: 1px solid black; text-align: center;'>" + RDAmountInput.Text + "</td></tr>");
        tableHtml.Append("<tr><th style='border: 1px solid black;'>FD</th><th style='border: 1px solid black; text-align: center;'>5</th><td style='border: 1px solid black; text-align: center; color: " + (int.Parse(FDInput.Text) < 5 ? "red;" : "black;") + "'>" + FDInput.Text + "</td><td style='border: 1px solid black; text-align: center;'>" + FDAmountInput.Text + "</td></tr>");
        tableHtml.Append("</tbody></table>");
        AccTable.InnerHtml = tableHtml.ToString();
      });

      tables();
      emailBody += AccTable.InnerHtml;
      var pmjdyValue = int.Parse(PMJDYInput.Text);
      var rdValue = int.Parse(RDInput.Text);
      var fdValue = int.Parse(FDInput.Text);
      emailBody += "<br/>";
      var targets = new Dictionary<string, int>
{
    { "PMJDY", int.Parse(PMJDYInput.Text) },
    { "RD", int.Parse(RDInput.Text) },
    { "FD", int.Parse(FDInput.Text) }
};
      emailBody += "<p style='color: red;'>DATE: " + DateTime.Now.ToString("dd/MM/yyyy") + "</p><br/>";
      var warningMessage = "<div style='text-align: justify;'><span style='color: red; font-weight: bold;'>WARNING NOTICE: YOU HAVE NOT MET THE DAILY TARGET FOR ";
      var firstTargetNotMet = true;

      foreach (var target in targets)
      {
        string condition = "";
        if (target.Key == "PMJDY" && target.Value < 5)
        {
          condition = "PMJDY < 5";
        }
        else if (target.Key == "RD" && target.Value < 10)
        {
          condition = "RD < 5";
        }
        else if (target.Key == "FD" && target.Value < 5)
        {
          condition = "FD < 5";
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
