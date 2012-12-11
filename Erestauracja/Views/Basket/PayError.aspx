<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    PayError
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Wystąpił nieznany błąd</h2>
<br />
<div>
<p>Wystąpił nieznany błąd podczas wysyłania twojego zamówienia.</p>
<br />
<% if (ViewData["id"] != null) %>
<% { %>
    <p>Numer Twojego zamówienia to <b><%: (int)ViewData["id"]%></b>.</p>
    <p><strong><b>UWAGA!</b></strong></p>
    <p>Zachowaj ten numer gdyż jest on najważniejszą informacją w procesie odzyskiwania kosztów zamówienia.</p>
    <br />
<% } %>
<p>Jeśli wybrałeś formę płatności "Płatność przy odbiorze":</p>
<p> - Wykonaj zamówienie ponownie,</p>
<p>lub</p>
<p> - Skontaktuj się z restauracją telefonicznie.</p>
<br />
<p>Jeśli wybrałeś formę płatności "PayPal":</p>
<p> - Jeżeli jest taka możliowść, anuluj płatność i wykonaj zamówienie ponownie,</p>
<p>lub</p>
<p> 
<div>- Skontaktuj się z restauracją telefonicznie, restauracja będzie miał możliwość zwrotu kosztów zamówienia,</div>
<% if (ViewData["id"] != null) %>
<% { %>
    <div>jeśli otrzyma numer zamówienia podany powyżej.</div>
<% } %>
<% else %>
<% { %>
    <div>jeśli otrzyma dane dotyczące zamówienia.</div>
<% } %>
</p>
</div>
<br />
<div>
W razie jakichkolwiek problemów związanych z nieprawidłowym funkcjonowaniem serwisu prosimy o kontakt z administratorami systemu.
</div>
<div>Przepraszamy za kłopot ;(</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
