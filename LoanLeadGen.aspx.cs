using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RMGBCSP_Default : System.Web.UI.Page
{
  SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      if (Convert.ToString(Session["Membertype"]) == "UW")
      {
        Response.Redirect("~/P2P_LOGP/login.aspx");
        return;
      }
      if (Convert.ToString(Session["Membertype"]) == "IS")
      {
        Response.Redirect("~/P2P_LOGP/login.aspx");
        return;
      }
      if (Convert.ToString(Session["Membertype"]) == "ES")
      {
        Response.Redirect("~/P2P_LOGP/login.aspx");
        return;
      }
      if (Convert.ToString(Session["Membertype"]) == "BS")
      {
        Response.Redirect("~/P2P_LOGP/login.aspx");
        return;
      }

      if (Request.QueryString.Count != 0)
      {
        editloan();
        submitbtn.Visible = false;
        updatebtn.Visible = true;
      }
      viewloans();
    }
  }

  public void viewloans()
  {
    try
    {
      string cspid = Session["empcode"].ToString();
      var dt = new DataTable();
      con.Open();
      new SqlDataAdapter("SELECT Id,Name,Phone,Loandate,LoanType,Amount,LoanStatus FROM LoanDetails WHERE cspid = '" + cspid + "' ORDER BY cspid ASC", con).Fill(dt);
      if (dt.Rows.Count > 0)
      {
        StringBuilder tableHtml = new StringBuilder();
        tableHtml.Append("<table id='example1' class='table'>");
        tableHtml.Append("<thead><tr><th>Sr No.</th><th>Name</th><th>Mobile No</th><th>Loan Date</th><th>Loan Type</th><th>Amount</th><th>Loan Status</th><th>Edit</th></tr></thead>");
        tableHtml.Append("<tbody>");

        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
          DateTime dob = Convert.ToDateTime(row["LoanDate"].ToString());
          dob = dob.Date;
          string loandate = dob.ToString("yyyy-MM-dd");
          tableHtml.Append("<tr>");
          tableHtml.Append("<td>" + ++i + "</td>");
          tableHtml.Append("<td>" + row["Name"] + "</td>");
          tableHtml.Append("<td>" + row["Phone"] + "</td>");
          tableHtml.Append("<td>" + loandate + "</td>");
          tableHtml.Append("<td>" + row["LoanType"] + "</td>");
          tableHtml.Append("<td>" + row["Amount"] + "</td>");
          tableHtml.Append("<td>" + row["LoanStatus"] + "</td>");
          if (row["LoanStatus"].ToString() == "Pending")
            tableHtml.Append("<td><a href='LoanLeadGen.aspx?id=" + row["Id"] + "'>Edit</a></td>");
          tableHtml.Append("</tr>");
        }
        tableHtml.Append("</tbody></table>");
        Loan_Details.InnerHtml = tableHtml.ToString();
      }
      con.Close();
    }
    catch { }
  }

  public void editloan()
  {
    try
    {
      string id = Request.QueryString["id"];
      con.Open();
      var dt = new DataTable();
      new SqlDataAdapter("Select * from LoanDetails where id = '" + id + "';", con).Fill(dt);
      if (dt.Rows[0]["CspId"].ToString() == Session["empcode"].ToString() && dt.Rows[0]["LoanStatus"].ToString() == "Pending")
      {
        if (dt.Rows.Count > 0)
        {
          try
          {
            ddl_loantype.ClearSelection();
            ddl_loantype.Items.FindByText(dt.Rows[0]["LoanType"].ToString()).Selected = true;
          }
          catch { }

          txt_amount.Text = dt.Rows[0]["Amount"].ToString();
          txt_name.Text = dt.Rows[0]["Name"].ToString();
          txt_add.Text = dt.Rows[0]["Address"].ToString();
          txt_pin.Text = dt.Rows[0]["Pin"].ToString();
          txt_mob.Text = dt.Rows[0]["Phone"].ToString();
        }
      }
      else
      {
        Response.Redirect("~/P2P_LOGP/login.aspx");
        return;
      }
    }
    catch { }
  }

  protected void subitbtn_Click(object sender, EventArgs e)
  {
    try
    {


      string cspid = Session["empcode"].ToString();
      con.Open();

      var dt1 = new DataTable();
      new SqlDataAdapter("SELECT email FROM login WHERE CspId = '" + cspid + "'", con).Fill(dt1);
      SqlCommand cmd = new SqlCommand("Insert into LoanDetails(LoanDate,Cspid,Name,LoanType,Amount,Address,Pin,Phone,LoanStatus) Values(DATEADD(mi,330,getdate()),'" + cspid + "','" + txt_name.Text + "','" + ddl_loantype.Text + "','" + txt_amount.Text + "','" + txt_add.Text + "','" + txt_pin.Text + "','" + txt_mob.Text + "','Pending');", con);

      int i = cmd.ExecuteNonQuery();

      if (dt1.Rows.Count > 0)
      {

        string mailid = dt1.Rows[0]["email"].ToString();
        mail(mailid);
      }
      if (i > 0)
      {

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Name : " + txt_name.Text.Replace("'", "") + " Has Been Successfully Updated');window.location ='LoanLeadGen.aspx';", true);

      }
      con.Close();
    }
    catch { }
  }

  protected void updatebtn_Click(object sender, EventArgs e)
  {
    try
    {
      string cspid = Session["empcode"].ToString();
      string id = Request.QueryString["id"];
      con.Open();
      var dt2 = new DataTable();
      new SqlDataAdapter("SELECT email FROM login WHERE CspId = '" + cspid + "'", con).Fill(dt2);
      SqlCommand cmd = new SqlCommand("UPDATE [LoanDetails] SET LoanDate = GETDATE(), Name ='" + txt_name.Text + "', LoanType ='" + ddl_loantype.Text + "', Amount ='" + txt_amount.Text + "', Address ='" + txt_add.Text + "' , Pin ='" + txt_pin.Text + "', Phone ='" + txt_mob.Text + "' WHERE Id ='" + id + "'", con);

      int i = cmd.ExecuteNonQuery();

      if (dt2.Rows.Count > 0)
      {

        string mailid = dt2.Rows[0]["email"].ToString();
        mail(mailid);
      }
      if (i > 0)
      {

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Name : " + txt_name.Text.Replace("'", "") + " Has Been Successfully Updated');window.location ='LoanLeadGen.aspx';", true);

      }
    }
    catch { }
  }

  public void mail(string to)
  {
    try
    {
      string cspid = Session["empcode"].ToString();
      string sendmail = ConfigurationManager.AppSettings["EmailID"].ToString();
      

      string cc = "f20202023@pilani.bits-pilani.ac.in";
      string email = to;
      MailMessage message = new MailMessage(sendmail, email);
      //smtp.UseDefaultCredentials = false ;
      message.Subject = "RMGB Loan Leads Genrated";
      string emailBody = "<html><body>";
      emailBody += "<p style='color: black;'>Dear " + Session["LastName"] + " (" + cspid + "),</p><br/>";
      emailBody += "<p style='color: black;'>Following are the loan details entered by you:</p><br/>";

      var tables = new Action(() =>
      {


        StringBuilder tableHtml = new StringBuilder();
        tableHtml.Append("<table id='example1' class='table' style='border-collapse: collapse;width: 80%; margin: 0 auto;'>");
        tableHtml.Append("<thead><tr><th style='border: 1px solid black;'>Loan Details</th><th style='border: 1px solid black;'>Values Entered</th></tr></thead>");
        tableHtml.Append("<tbody>");
        tableHtml.Append("<tr><th style='border: 1px solid black; text-align: center;'>Loan Type</th><td style='border: 1px solid black;text-align: center;'>" + ddl_loantype.Text + "</td></tr>");
        tableHtml.Append("<tr><th style='border: 1px solid black; text-align: center;'>Loan Amount</th><td style='border: 1px solid black;text-align: center;'>" + txt_amount.Text + "</td></tr>");
        tableHtml.Append("<tr><th style='border: 1px solid black; text-align: center;'>Applicant Name</th><td style='border: 1px solid black; text-align: center;'>" + txt_name.Text + "</td></tr>");
        tableHtml.Append("<tr><th style='border: 1px solid black; text-align: center;'>Applicant Address</th><td style='border: 1px solid black; text-align: center;'>" + txt_add.Text + "</td></tr>");
        tableHtml.Append("<tr><th style='border: 1px solid black; text-align: center;'>Pin Code</th><td style='border: 1px solid black; text-align: center;'>" + txt_pin.Text + "</td></tr>");
        tableHtml.Append("<tr><th style='border: 1px solid black; text-align: center;'>Mobile Number</th><td style='border: 1px solid black; text-align: center;'>" + txt_mob.Text + "</td></tr>");
        tableHtml.Append("</tbody></table>");
        Loan_Details.InnerHtml = tableHtml.ToString();
      });

      tables();
      emailBody += Loan_Details.InnerHtml;
      emailBody += "<p style='color: red;'>DATE: " + DateTime.Now.ToString("dd/MM/yyyy") + "</p><br/>";
      emailBody += "<p style='text-align: justify; color: red;'>THANK YOU. THE LOAN DEATILS HAVE BEEN SUBMITTED SUCCESSFULLY. WE WILL CONTACT YOU SHORTLY FOR FURTHER PROCESS.</p><br/>";
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
