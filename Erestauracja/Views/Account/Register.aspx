<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.RegisterModel>" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Rejesteracja
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Utwórz swoje konto, jeśli jeszcze go nie posiadasz.</h2>
    
    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>"
        type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#Birthdate").datepicker();
        });
    </script>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#Birthdate").mask("99/99/9999");
        });
    </script>

    <form runat="server">
    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Rejestracja konta nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset>
                <legend>Dane rejestracji</legend>
                <p>
                    Wprowadz swoje dane, a następnie kliknij 'Załóż konto' aby w pełni wykorzystać możliwości serwisu.
                </p>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Login) %>
                    <%: Html.TextBoxFor(m => m.Login)%>
                    <%: Html.ValidationMessageFor(m => m.Login)%>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Email) %>
                    <%: Html.TextBoxFor(m => m.Email) %>
                    <%: Html.ValidationMessageFor(m => m.Email) %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmEmail) %>
                    <%: Html.TextBoxFor(m => m.ConfirmEmail)%>
                    <%: Html.ValidationMessageFor(m => m.ConfirmEmail)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %> (Minimum <%: Membership.MinRequiredPasswordLength %> znaków.)
                    <%: Html.PasswordFor(m => m.Password) %> 
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                    <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Question) %>
                    <%: Html.TextBoxFor(m => m.Question)%>
                    <%: Html.ValidationMessageFor(m => m.Question)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Answer) %>
                    <%: Html.TextBoxFor(m => m.Answer)%>
                    <%: Html.ValidationMessageFor(m => m.Answer)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Name) %>
                    <%: Html.TextBoxFor(m => m.Name)%>
                    <%: Html.ValidationMessageFor(m => m.Name)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Surname) %>
                    <%: Html.TextBoxFor(m => m.Surname)%>
                    <%: Html.ValidationMessageFor(m => m.Surname)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Address) %>
                    <%: Html.TextBoxFor(m => m.Address)%>
                    <%: Html.ValidationMessageFor(m => m.Address)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.TownID) %>
                    <%: Html.TextBoxFor(m => m.TownID)%>
                    <%: Html.ValidationMessageFor(m => m.TownID)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Country) %>
                    <%: Html.TextBoxFor(m => m.Country)%>
                    <%: Html.ValidationMessageFor(m => m.Country)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Birthdate)%>
                    <%: Html.TextBoxFor(m => m.Birthdate, new { id = "Birthdate" })%>
                    <%: Html.ValidationMessageFor(m => m.Birthdate)%>
                    
                </div>
                
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Sex) %>
                    <%: Html.TextBoxFor(m => m.Sex)%>
                    <%: Html.ValidationMessageFor(m => m.Sex)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Telephone)%>
                    <%: Html.TextBoxFor(m => m.Telephone)%>
                    <%: Html.ValidationMessageFor(m => m.Telephone)%>
                </div>

                <p>
                    <input type="submit" value="Załóż konto"/>
                </p>
            </fieldset>
        </div>
    <% } %>
    </form>
     
</asp:Content>

   



