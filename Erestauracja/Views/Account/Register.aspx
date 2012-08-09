<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.RegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Register
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Create a New Account</h2>
    <p>
        Use the form below to create a new account. 
    </p>
    <p>
        Passwords are required to be a minimum of <%: Membership.MinRequiredPasswordLength %> characters in length.
    </p>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Account creation was unsuccessful. Please correct the errors and try again.") %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Login) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Login)%>
                    <%: Html.ValidationMessageFor(m => m.Login)%>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Email) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email) %>
                    <%: Html.ValidationMessageFor(m => m.Email) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmEmail) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmEmail)%>
                    <%: Html.ValidationMessageFor(m => m.ConfirmEmail)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Question) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Question)%>
                    <%: Html.ValidationMessageFor(m => m.Question)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Answer) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Answer)%>
                    <%: Html.ValidationMessageFor(m => m.Answer)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Name)%>
                    <%: Html.ValidationMessageFor(m => m.Name)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Surname) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Surname)%>
                    <%: Html.ValidationMessageFor(m => m.Surname)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Address) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Address)%>
                    <%: Html.ValidationMessageFor(m => m.Address)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.TownID) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.TownID)%>
                    <%: Html.ValidationMessageFor(m => m.TownID)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Country) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Country)%>
                    <%: Html.ValidationMessageFor(m => m.Country)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Birthdate)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Birthdate)%>
                    <%: Html.ValidationMessageFor(m => m.Birthdate)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Sex) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Sex)%>
                    <%: Html.ValidationMessageFor(m => m.Sex)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Telephone)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Telephone)%>
                    <%: Html.ValidationMessageFor(m => m.Telephone)%>
                </div>

                <p>
                    <input type="submit" value="Register" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
