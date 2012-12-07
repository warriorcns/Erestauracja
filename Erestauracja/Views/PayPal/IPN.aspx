<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.PayPal>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    IPN
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if (ViewData["alert"] != "")
       {%>
    <div style="color: Red">
        <%: ViewData["alert"] %></div>
    <%}%>

    <% using (Html.BeginForm())
               { %>
    <div>
        Kwota zamowienia:
        <%: Html.DisplayFor(m => m.mc_gross) %>
    </div>
    <div>
        ID transakcji:
        <%: Html.DisplayFor(m => m.txn_id) %>
    </div>
    <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
