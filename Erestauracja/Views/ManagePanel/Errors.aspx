<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.ErrorModels>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Zgłaszanie błędów
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Podaj swój adres email oraz treść zgłoszenia, a następnie kliknij "Wyślij", aby powiadomić nas o znalexionym błędzie.</h2>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% using (Html.BeginForm()) %>
    <% { %>
        <%: Html.ValidationSummary(true, "Wysyłanie zgłoszenia nie powiodło się. Popraw błędnie wypełnione pola i spróbuj ponownie.")%>
        <div>
            <fieldset>
                <legend>Formularz zgłoszenia</legend>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Email) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email)%>
                    <%: Html.ValidationMessageFor(m => m.Email)%>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Text) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(m => m.Text, 15, 40, null)%>
                    <%: Html.ValidationMessageFor(m => m.Text)%>
                </div>
                
                <p>
                    <input type="submit" value="Wyślij"/>
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
