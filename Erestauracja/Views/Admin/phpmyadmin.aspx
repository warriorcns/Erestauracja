<%@ Page Title="phpmyadmin" Language="C#" MasterPageFile="~/Views/Admin/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminPlaceHolder" runat="server">
    <%--<iframe class="page" src="http://5.153.38.77/phpmyadmin/"></iframe>
    <a href="http://5.153.38.77/phpmyadmin/" target="_blank">Starfall</a>--%>
    <script type="text/javascript">
        setTimeout(function () {
            window.location.replace("http://25.30.184.53/phpmyadmin/");
        },1);
            
        
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
