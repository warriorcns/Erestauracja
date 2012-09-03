<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.LogOnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Rejestracja
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Wprowadź login oraz hasło do swojego konta a następnie kliknij 'Zarejestruj' aby wykorzystać to konto.</h2>
<script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Rejestracja z wykorzystaniemistniejącego konta nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.")%>
        <div>
            <fieldset>
                <legend>Dane rejestracji</legend>
                <p>
                    Wprowadź swój login oraz hasło.
                </p>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Login) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Login)%>
                    <%: Html.ValidationMessageFor(m => m.Login)%>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
               
                <p>
                    <input type="submit" value="Zarejestruj"/>
                </p>
                </br>
                <p>
                    Zapomiałeś hasła? <%: Html.ActionLink("Tutaj", "PasswordReset", "Account")%> możesz odzyskać dostęp do konta.
                </p>
            </fieldset>
        </div>
    
    <% } %>
</asp:Content>
