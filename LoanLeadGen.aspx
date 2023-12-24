<%@ Page Title="" Language="C#" MasterPageFile="~/RMGBCSP/MasterPageSBICSP.master" AutoEventWireup="true" CodeFile="LoanLeadGen.aspx.cs" Inherits="RMGBCSP_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
  
<style> 

input[type=text] {

    border-radius: 5px ;
    border: 2px solid lightgray ; 
    
}

input[type=multiline] {

    border-radius: 5px;
    border: 1px solid lightgray;

 }

.rcorners2 {

    border-radius: 5px ;
    border: 2px solid lightgray ;
       
}

.tt {

    border-radius: 5px ;
    border: 2px solid lightgray ;
       
}

</style>
  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

   <div class="row">
               <div class="form-group col-md-12">
            <div class="card">
              <div style="background-color: #f5f5dc;" class="card-header">
                <h3 class="card-title">Loan Lead Generator</h3>
              </div>
              <div class="card-body">
                   <div class="row">

                       <div class="form-group col-md-2" runat="server"> <label for="ddl_loantype" style="color:gray;font-size:small">Loan Type</label><span style="color:red">*</span> 
                                 
                                  <asp:DropDownList runat="server" ID="ddl_loantype" CssClass="form-control form-control-sm">

                                      <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>

                                      <asp:ListItem Text="KCC" Value="KCC"></asp:ListItem>
                                      <asp:ListItem Text="4 Wheeler" Value="4 Wheeler"></asp:ListItem>
                                      <asp:ListItem Text="Personal" Value="Personal"></asp:ListItem>
                                      <asp:ListItem Text="Tractor" Value="Tractor"></asp:ListItem>
                                      <asp:ListItem Text="2 Wheeler" Value="2 Wheeler"></asp:ListItem>
                                      <asp:ListItem Text="Housing" Value="Housing"></asp:ListItem>
                                    <asp:ListItem Text="SHG" Value="SHG"></asp:ListItem>
                                    <asp:ListItem Text="Gold" Value="Gold"></asp:ListItem>
                                    <asp:ListItem Text="Business" Value="Business"></asp:ListItem>
                                     
                                   </asp:DropDownList>  

                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="Validators" Display="None" ControlToValidate="ddl_loantype" InitialValue="0" runat="server" ErrorMessage="Please Enter Loan Type"></asp:RequiredFieldValidator>
                                 </div> 

                     <div class="form-group col-md-2"> <label for="txt_amount" style="color:gray;font-size:small"> Amount </label><span style="color:red">*</span>   <asp:TextBox runat="server" ID="txt_amount" oninput="validateNumericInput(this)" CssClass="form-control form-control-sm" placeholder="Please Enter Amount" ></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator11" CssClass="Validators" Display="None" ControlToValidate="txt_amount" runat="server" ErrorMessage="Please Enter Amount"></asp:RequiredFieldValidator>
                                 </div> 

                               <div class="form-group col-md-3" id="username" runat="server" visible="true"> <label for="txt_name" style="color:gray;font-size:small">Applicant Name</label><span style="color:red">*</span>  <asp:TextBox runat="server"  ID="txt_name"  CssClass="form-control form-control-sm" placeholder="Enter Name"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator7" CssClass="Validators" Display="None" ControlToValidate="txt_name" runat="server" ErrorMessage="Please Enter Name"></asp:RequiredFieldValidator>
                                 </div> 
                        
                               <div class="form-group col-md-5"> <label for="txt_add" style="color:gray;font-size:small">Applicant Address </label><span style="color:red">*</span>   <asp:TextBox runat="server" ID="txt_add" CssClass="form-control form-control-sm" placeholder="Please Enter Address"  ></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="Validators" Display="None" ControlToValidate="txt_add" runat="server" ErrorMessage="Please Enter Address"></asp:RequiredFieldValidator>
                                 </div> 
                              
                     <div class="form-group col-md-2" runat="server"> <label for="txt_pin" style="color:gray;font-size:small">Pin Code</label> <span style="color:red">*</span>  <asp:TextBox runat="server" ID="txt_pin" oninput="validateNumericInput(this)" CssClass="form-control form-control-sm" MaxLength="6" placeholder="Enter Pin Code"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="Validators" Display="None" ControlToValidate="txt_pin" runat="server" ErrorMessage="Pin Code is required."></asp:RequiredFieldValidator>
                       <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt_pin"  
                    Display="Dynamic" ForeColor="Red" ErrorMessage="Invalid Pin Code" ValidationExpression="\d{6}"></asp:RegularExpressionValidator>
                                 </div> 
                              
                    <div class="form-group col-md-2" runat="server"> <label for="txt_mob" style="color:gray;font-size:small">Mobile Number</label> <span style="color:red">*</span>  <asp:TextBox runat="server" ID="txt_mob" oninput="validateNumericInput(this)" CssClass="form-control form-control-sm" Maxlength="10" placeholder="Enter Mobile Number"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="Validators" Display="None" ControlToValidate="txt_mob" runat="server" ErrorMessage="Mobile Number is required."></asp:RequiredFieldValidator>
                       <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_mob"  
                    Display="Dynamic" ForeColor="Red" ErrorMessage="Invalid Mobile Number" ValidationExpression="\d{10}"></asp:RegularExpressionValidator>
                                 </div> 
                    </div>

                      <div class="form-group col-md-4">
                        
                         <asp:ValidationSummary runat="server" ShowMessageBox="true" ShowSummary="false" />

  <asp:Button runat="server" ID="submitbtn" class = "btn btn-sm btn-primary" OnClick="subitbtn_Click" Text="Submit" />

<asp:Button runat="server" ID="updatebtn" class = "btn btn-sm btn-primary" OnClick="updatebtn_Click" Text="Update" visible ="false"/>
                      </div>
                 <div class="col-sm-12">
                <div id ="Loan_Details" runat="server" class ="mt-4 overflow-auto" ></div>
                </div>
                </div>
              </div>
                 </div>
     </div>
  <script>
function validateNumericInput(input) {
    // Remove non-numeric characters from the input
    input.value = input.value.replace(/\D/g, '');
}
  </script>
   
</asp:Content>
