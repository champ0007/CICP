<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TESTConnection.aspx.cs" Inherits="AUDash.TESTConnection" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=.;Initial Catalog=AUDashboard;User ID=sa;Password=India@123" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
