﻿<%@ Page Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.ChangeResPasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ChangePassword
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(function ($) {
            $("#fcs").focus();
        });
    </script>
    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Zmiana hasła nie powiodła się. Wprowadź poprawne dane i spróbuj jeszcze raz.") %>
        <div>
            <fieldset>
                <legend>Zmiana hasła</legend>
                <p>
                    Wprowadz dotychczasowe i nowe hasło oraz kliknij 'Zatwierdź' w celu zmiany. 
                </p>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.OldPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.OldPassword, new { @id = "fcs" })%>
                    <%: Html.ValidationMessageFor(m => m.OldPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.NewPassword) %> (Minimum <%: Membership.MinRequiredPasswordLength %> znaków.)
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.NewPassword) %>
                    <%: Html.ValidationMessageFor(m => m.NewPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                <div> <%: Html.HiddenFor(m=>m.Login) %></div>
                <p>
                    <input type="submit" value="Zatwierdź" />
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
