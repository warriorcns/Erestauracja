<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.EmployeePasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    EditPassword
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <div class="polaRejestracji">

        <form id="Form1" class="formaRejestracji" runat="server">
        <% using (Html.BeginForm()) %>
        <% { %>
        <%: Html.ValidationSummary(true, "Rejestracja konta nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset class="polaRejestracji polaRejWidth">
                <legend>Zmiana hasła pracownika</legend>
                <p>
                    Wprowadz hasło, a następnie kliknij "Zapisz hasło".
                </p>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Password)%>
                        (Minimum
                        <%: Membership.MinRequiredPasswordLength %>
                        znaków.) 
                        </li>
                    <li class="editor-labelR">
                        <%: Html.PasswordFor(m => m.Password)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                       <%: Html.ValidationMessageFor(m => m.Password)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.ConfirmPassword)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.PasswordFor(m => m.ConfirmPassword)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                       <%: Html.ValidationMessageFor(m => m.ConfirmPassword)%>
                    </li>
                </ul>
                <div> <%: Html.HiddenFor(m=> m.EmployeeLogin) %></div>
                 
                 <p>
                    <input type="submit" value="Zapisz hasło" />
                </p>
            </fieldset>
        </div>
        <% } %>
        </form>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
