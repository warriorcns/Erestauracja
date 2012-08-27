<%@ Page Title="phpmyadmin" Language="C#" MasterPageFile="~/Views/Admin/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminPlaceHolder" runat="server">
    <iframe class="page" src="http://5.32.56.82/phpmyadmin/"></iframe>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
