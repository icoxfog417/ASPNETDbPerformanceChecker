<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css"/>
    <title>Database Performance Test</title>
</head>
<body>
    <form id="form1" runat="server">
      <div class="container bs-docs-container">
        <div class="page-header">
          <h1>Database Performance Test</h1>
        </div>
        <div class="row">
            <div class="col-lg-2">
                <asp:DropDownList ID="ddlConnection" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-lg-2">
                <div class="input-group">
                    <span class="input-group-addon">Record</span>
                    <asp:textbox ID="txtRecordCount" runat="server" CssClass="form-control" Text="1000" style="text-align:right"></asp:textbox>
                </div>
            </div>
            <div class="col-lg-4"></div>
            <div class="col-lg-2">
                <asp:Button ID="btnRun" runat="server" CssClass="btn btn-primary" Text=" Run " width="100"/>
            </div>
        </div>
        <br/><br/>
        <asp:Table ID="tblLog" runat="server" CssClass="table">
        </asp:Table>
      </div>
    </form>
</body>
</html>
