<%@ Page Title="" Language="C#" MasterPageFile="~/RMGBCSP/MasterPageSBICSP.master" AutoEventWireup="true" CodeFile="SSS_Report.aspx.cs" Inherits="RMGBCSP_SSS_Report" %>

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

        /* Responsive table layout */

        .table-responsive {

            overflow-x: auto;
        }

        /* Center content in columns for small screens */

        .table-responsive td,

        .table-responsive th {

            text-align: left;

        }

        /* Adjust form layout for small screens */

        @media screen and (max-width: 767px) {

            .form-group.col-md-2,

            .form-group.col-md-3,

            .form-group.col-md-4,

            .form-group.col-md-6 {

                width: 100%;

                margin-bottom: 15px;

            }

        }

    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <br />

    <div class="row">

        <div class="form-group col-md-12">

            <div class="card">

                <div class="card-header">

                    <h3 class="card-title"style= "font-size: 50px">SSS Progress Report</h3>

                </div>

                <div class="card-body">

                    <div class="row">

                        <div class="form-group col-md-12">

                            <h4>Enter Progress Report</h4>

                            <div class="table-responsive">

                                <table class="table">

                                    <thead>

                                        <tr>

                                            <th>SSS Schemes</th> <!-- Empty header cell for spacing -->

                                            <th>Values</th> <!-- Header for values -->

                                        </tr>

                                    </thead>

                                    <tbody>

                                        <tr>

                                            <th>APY<span style="color:red">*</span></th>

                                            <td>

                                                <asp:TextBox runat="server" ID="apyInput" CssClass="form-control" placeholder="Enter No. of APY Schemes" MaxLength="3" oninput="this.value = this.value.replace(/[^0-9]/g, '');"/>

                                            </td>

                                        </tr>

                                        <tr>

                                            <th>SBY<span style="color:red">*</span></th>

                                            <td>

                                                <asp:TextBox runat="server" ID="sbyInput" CssClass="form-control" placeholder="Enter No. of SBY Schemes" MaxLength="3" oninput="this.value = this.value.replace(/[^0-9]/g, '');"/>

                                            </td>

                                        </tr>

                                        <tr>

                                            <th>JJBY<span style="color:red">*</span></th>

                                            <td>

                                                <asp:TextBox runat="server" ID="jjbyInput" CssClass="form-control" placeholder="Enter No. of JJBY Schemes" MaxLength="3" oninput="this.value = this.value.replace(/[^0-9]/g, '');"/>

                                            </td>

                                        </tr>

                                    </tbody>

                                </table>

                            </div>

                            <asp:Button runat="server" class="btn btn-sm btn-primary" OnClick="submit_Click" ID="submit" Text="Submit" />
                          <asp:Literal ID="Literal1" runat="server"></asp:Literal>

                          <asp:Label runat="server" ID="errorMessage" CssClass="text-danger" Visible="false" />

                        </div>

                    </div>

                   <div class="col-sm-12">
                <div id ="SSSReport_Details" runat="server" class ="mt-4 overflow-auto" ></div>
                </div>

                </div>

            </div>

        </div>

    </div>

</asp:Content>
