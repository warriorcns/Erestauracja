<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Account/Account.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.UserDataModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AccountPlaceHolder" runat="server">

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <link href="../../Content/themes/base/jquery.ui.datepicker.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/themes/redmond/jquery-ui.css" rel="stylesheet" type="text/css" />

    <% UserDataModel model = (UserDataModel)ViewData["model"];  %>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Rejestracja konta nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset>
                <legend>Dane Użytkownika</legend>
                <p>
                   Kliknij przycisk 'Edytuj', aby wprowadzić zmiany do swoich danych,
                </p>
                <p>
                   lub przycisk 'Zmień hasło', aby zmienić hasło do swojego konta.
                </p>
                <div class="editor-label">
                Login:  <%: model.Login %>
                </div>
                <div class="editor-label">
                Adres email:  <%: model.Email %>
                </div>
                <div class="editor-label">
                Imię:  <%: model.Name %>
                </div>
                <div class="editor-label">
                Nazwisko:  <%: model.Surname %>
                </div>
                <div class="editor-label">
                Adres:  <%: model.Address %>
                </div>
                <div class="editor-label">
                Miasto:  <%: model.TownID %>
                </div>
                <div class="editor-label">
                Kraj:  <%: model.Country %>
                </div>
                <div class="editor-label">
                Data urodzenia:  <%: model.Birthdate.ToShortDateString() %>
                </div>
                <div class="editor-label">
                Płeć:  <%: model.Sex %>
                </div>
                <div class="editor-label">
                Numer telefonu:  <%: model.Telephone %>
                </div>
                <li>
                <%: Html.ActionLink("Edytuj", "EditData", "Account",model,null)%>  <%: Html.ActionLink("Zmień hasło", "ChangePassword", "Account")%></li>

            </fieldset>
        </div>
    <% } %>

</asp:Content>
