<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    QuestionAndAnswer
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        $(function ($) {
            $("#fcs").focus();
        });
    </script>
    <h2>Podaj odpowiedź na pytanie wybrane w czase rejestracji aby zresetować hasło.</h2>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Błąd")%>
        <div>
            <fieldset>
                <legend>Odzyskiwanie hasła</legend>

                <div class="editor-label">
                Login: <%= ViewData["Login"] %>
                </div>

                <div class="editor-label">
                Pytanie: <%= ViewData["Pytanie"]%>
                </div>

                <div class="editor-label">
                Odpowiedź: 
                </div>
                <div class="editor-field">
                    <%= Html.TextBox("Odpowiedz", null, new { @id = "fcs" }) %>
                </div>

                <p>
                    <input type="submit" value="Dalej"/>
                </p>
            </fieldset>
        </div>
    
    <% } %>

</asp:Content>
