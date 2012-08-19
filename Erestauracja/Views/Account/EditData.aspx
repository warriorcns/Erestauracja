﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Account/Account.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.UserDataModel>" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="AccountPlaceHolder" runat="server">

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <link href="../../Content/themes/base/jquery.ui.datepicker.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/themes/redmond/jquery-ui.css" rel="stylesheet" type="text/css" />

<% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Edycja danych nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset>
                <legend>Edycja danych</legend>
                <p>
                    Wprowadz nowe dane, a następnie kliknij 'Zapisz'.
                </p>

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
                    <%: Html.EditorFor(m => m.Birthdate)%>
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
                    <input type="submit" value="Zapisz"/>
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>