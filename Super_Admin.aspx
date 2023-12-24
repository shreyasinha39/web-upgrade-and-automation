<%@ Page Title="" Language="C#" MasterPageFile="~/P2PSuperAdmin/MasterPageSuperadmin.master" AutoEventWireup="true" CodeFile="Super_Admin.aspx.cs" Inherits="P2PSuperAdmin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
  <style> 


input[type=text] {
    border-radius: 5px;
    border: 2px solid lightgray;
     
}
input[type=multiline] {
    border-radius: 5px ;
    border: 1px solid lightgray ;
     
}

.rcorners2 {
    border-radius: 5px;
    border: 2px solid lightgray;
       
}

.tt {
    border-radius: 5px;
    border: 2px solid lightgray;
       
}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <br />
  <br />

  <!-- Dropdown for Bank Selection -->
    <div class="col m-md-2" runat="server">
      <label for="ddl_bank" style="color: gray; font-size: large;">Select Bank</label>

      <asp:DropDownList runat="server" ID="ddl_bank" style="width: 12%;" CssClass="form-control form-control-sm" OnTextChanged="ddl_bank_TextChanged" AutoPostBack="true">
        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
        <asp:ListItem Text="RMGB" Value="ES"></asp:ListItem>
        <asp:ListItem Text="SBI" Value="U"></asp:ListItem>
        <asp:ListItem Text="Aaryavart" Value="A"></asp:ListItem>
       
      </asp:DropDownList>

    

    </div>

  <br />
  <!-- Dropdown for Bank Selection end -->
  <div class="col m-md-2" runat="server">
  <asp:RadioButtonList runat ="server" ID="rad" OnTextChanged="rad_TextChanged" AutoPostBack ="true">
       
 </asp:RadioButtonList>
  </div>




  <!-- Excel Export Button -->
   <asp:Button runat="server" OnClick="btnexport_Click"  ID="btnexport"  Text="Excel Sheet" />
  <!-- Excel Export Button end -->

  <div runat="server" >
            <div class="card">
            <div class="card-header">
              <h3 class="card-title">CSP Details</h3>
            </div>
            <!-- /.card-header -->
            <div class="card-body">
             <div class="col-sm-12">
                      <!-- Empty space To Display User Details -->
         <div runat="server" id="DetailsTable" class="mt-4 overflow-auto"></div>
                    </div>         
            </div>
            <!-- /.card-body -->
          </div>
    </div>

</asp:Content>

