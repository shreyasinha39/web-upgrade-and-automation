<%@ Page Title="" Language="C#" MasterPageFile="~/RMGBCSP/MasterPageSBICSP.master" AutoEventWireup="true" CodeFile="Account_Opening.aspx.cs" Inherits="RMGBCSP_Account_Opening" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">

    <style>

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

    <div class="row">

        <div class="form-group col-md-12">

            <div class="card">

                <div class="card-header">

                    <h4 class="card-title"style= "font-size: 50px">ACCOUNT OPENINGS</h4>

                    

                </div>

                <div class="card-body">

                    <div class="row">

                        <div class="form-group col-md-12">

                            <table class="table">

                                <thead>

                                    <tr>

                                        <th></th>

                                        <th>NUMBER</th>

                                        <th>AMOUNT (&#8377)</th>

                                    </tr>

                                </thead>

                                <tbody>

                                    <tr>

                                        <td>PMJDY<span style="color:red">*</span></td>

                                        <td><asp:TextBox runat="server" placeholder="Enter PMJDY" ID="PMJDYInput" CssClass="form-control" MaxLength="3" oninput="restrictInput(this);" /></td>

                                       

                                    </tr>

                                    <tr>

                                        <td>RD<span style="color:red">*</span></td>

                                        <td><asp:TextBox runat="server" ID="RDInput" placeholder="Enter RD Number" CssClass="form-control" MaxLength="3" oninput="restrictInput(this);" /></td>

                                        <td><asp:TextBox runat="server" ID="RDAmountInput" CssClass="form-control" placeholder="Enter RD Amount" oninput="restrictInput(this)" /></td>

                                    </tr>

                                    <tr>

                                        <td>FD<span style="color:red">*</span></td>

                                        <td><asp:TextBox runat="server" ID="FDInput" CssClass="form-control" MaxLength="3" placeholder="Enter FD Number" oninput="restrictInput(this);" /></td>

                                        <td><asp:TextBox runat="server" ID="FDAmountInput" CssClass="form-control" placeholder="Enter FD Amount" oninput="restrictInput(this)"/></td>

                                    </tr>

                                    <!-- Add more rows as needed -->

                                </tbody>

                            </table>

                            <asp:Button runat="server" class="btn btn-sm btn-primary" OnClick="submit_Click" ID="submit" Text="Submit" />

                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                       
                          <asp:Label runat="server" ID="errorMessage" CssClass="text-danger" Visible="false" />

                          </div>
                       <div class="col-sm-12">
                      <div runat="server" class ="mt-4 overflow-auto" id ="AccTable"></div>
                         </div>
                    </div>

                </div>

            </div>

        </div>

    </div>
  
    <script>

      function restrictInput(element) {

        element.value = element.value.replace(/[^0-9]/g, '');

      }

    </script>

</asp:Content>

