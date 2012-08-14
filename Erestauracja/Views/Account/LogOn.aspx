<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Logowanie
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Zaloguj się aby w pełni wykorzystać możliwości serwisu.</h2>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Logowanie nie powiodło się. Popraw błędnie wypełnione pola i spróbuj ponownie.")%>
        <div>
            <fieldset>
                <legend>Dane logowania</legend>
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
                
                <div class="editor-label">
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe) %>
                </div>
                <p>
                    <input type="submit" value="Zaloguj"/>
                </p>
                </br>
                <p>
                    Zapomiałeś hasła? <%: Html.ActionLink("Tutaj", "PasswordReset", "Account")%> możesz odzyskać dostęp do konta.
                </p>
                </br>
                <p>
                    Jeśli nie posiadasz jeszcze konta możesz je założyć <%: Html.ActionLink("tutaj", "Register", "Account") %>.
                </p>
            </fieldset>
        </div>
    
    <% } %>
</asp:Content>
