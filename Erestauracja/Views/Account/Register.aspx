﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.RegisterModel>" %>


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
            $("#Birthdate").mask("9999/99/99");
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

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Login) %>
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Login)%>
                        <%: Html.ValidationMessageFor(m => m.Login)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Email)%>
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Email)%>
                        <%: Html.ValidationMessageFor(m => m.Email)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.ConfirmEmail)%>
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.ConfirmEmail)%>
                        <%: Html.ValidationMessageFor(m => m.ConfirmEmail)%>
                    </li>
                </ul>
               
                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Password)%> (Minimum <%: Membership.MinRequiredPasswordLength %> znaków.)
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Password)%>
                        <%: Html.ValidationMessageFor(m => m.Password)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.ConfirmPassword)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.ConfirmPassword)%>
                        <%: Html.ValidationMessageFor(m => m.ConfirmPassword)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Question)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Question)%>
                        <%: Html.ValidationMessageFor(m => m.Question)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Answer)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Answer)%>
                        <%: Html.ValidationMessageFor(m => m.Answer)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Name)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Name)%>
                        <%: Html.ValidationMessageFor(m => m.Name)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Surname)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Surname)%>
                        <%: Html.ValidationMessageFor(m => m.Surname)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Address)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Address)%>
                        <%: Html.ValidationMessageFor(m => m.Address)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.TownID)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.TownID)%>
                        <%: Html.ValidationMessageFor(m => m.TownID)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Country)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Country)%>
                        <%: Html.ValidationMessageFor(m => m.Country)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Birthdate)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Birthdate, new { id = "Birthdate" })%>
                        <%: Html.ValidationMessageFor(m => m.Birthdate)%>
                    </li>
                </ul>
                
                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Sex)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Sex)%>
                        <%: Html.ValidationMessageFor(m => m.Sex)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Telephone)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Telephone)%>
                        <%: Html.ValidationMessageFor(m => m.Telephone)%>
                    </li>
                </ul>

                <p>
                    <input type="submit" value="Załóż konto"/>
                </p>
            </fieldset>
        </div>
    <% } %>
    </form>
     
</asp:Content>

   



