<%@ Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.UserDataModel>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <link href="../../Content/themes/base/jquery.ui.datepicker.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/themes/redmond/jquery-ui.css" rel="stylesheet" type="text/css" />

        <div>
            <fieldset>
                <legend>Dane Użytkownika</legend>
                <p>
                   Kliknij przycisk 'Edytuj', aby wprowadzić zmiany do swoich danych,
                </p>
                <p>
                   lub przycisk 'Zmień hasło', aby zmienić hasło do swojego konta.
                </p>
                <div class="editor-labelE">
                Login:  <%: Model.Login %>
                </div>
                <div class="editor-labelE">
                Adres email:  <%: Model.Email%>
                </div>
                <div class="editor-labelE">
                Imię:  <%: Model.Name%>
                </div>
                <div class="editor-labelE">
                Nazwisko:  <%: Model.Surname%>
                </div>
                <div class="editor-labelE">
                Adres:  <%: Model.Address%>
                </div>
                <div class="editor-labelE">
                Miasto:  <%: Model.Town%>
                </div>
                <div class="editor-labelE">
                Kod pocztowy:  <%: Model.PostalCode%>
                </div>
                <div class="editor-labelE">
                Kraj:  <%: Model.Country%>
                </div>
                <div class="editor-labelE">
                Data urodzenia:  <%: Model.Birthdate.ToShortDateString()%>
                </div>
                <div class="editor-labelE">
                Płeć:  <%: Model.Sex%>
                </div>
                <div class="editor-labelE">
                Numer telefonu:  <%: Model.Telephone%>
                </div>
                <li>
                <%: Html.ActionLink("Edytuj", "EditMenager", "ManagePanel")%> 
                <%: Html.ActionLink("Zmień hasło", "ChangePasswordMenager", "ManagePanel")%>
                </li>
            </fieldset>
        </div>

</asp:Content>

