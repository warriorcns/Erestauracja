<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Odzyskiwanie hasła
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Podaj swój login, a następnie kliknij 'Dalej' aby przejść do resetowania hasła.</h2>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "?")%>
        <div>
            <fieldset>
                <legend>Odzyskiwanie hasła</legend>

                <div class="editor-label">
                    Wprowadź swój login
                </div>
                <div class="editor-field">
                    <%: Html.TextBox("login")%>
                </div>
                <p>
                    <input type="submit" value="Dalej"/>
                </p>
            </fieldset>
        </div>
    
    <% } %>

</asp:Content>

