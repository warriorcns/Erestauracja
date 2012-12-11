<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Account/Account.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.UserDataModel>" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="AccountPlaceHolder" runat="server">


    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#Birthdate").datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: '-100:+0',
                dateFormat: 'dd/mm/yy'
            });
        });
    </script>
    <script type="text/javascript">
    $(function () {
        jQuery(function ($) {
            $("#Birthdate").mask("99/99/9999");
        });
    });
    </script>
    <script type="text/javascript">
        $(function () {
            //window.location.href = window.location.href.split('?', 1)[0];
        });
    </script>
    <div class="polaRejestracji">
        <div id="Form1" class="formaRejestracji" runat="server">
            <% using (Html.BeginForm())
               { %>
            <%: Html.ValidationSummary(true, "Edycja danych nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
            <div>
                <fieldset>
                    <legend>Edycja danych</legend>
                    <p>
                        Wprowadz nowe dane, a następnie kliknij 'Zapisz'.
                    </p>
                    <div class="editor-label">
                        <%: Html.LabelFor(m => m.Email) %>
                        <%: Html.TextBoxFor(m => m.Email)%>
                        <%: Html.ValidationMessageFor(m => m.Email)%>
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
                        <%: Html.LabelFor(m => m.Town) %>
                        <%: Html.TextBoxFor(m => m.Town, new { id="TownName" })%>
                        <%: Html.ValidationMessageFor(m => m.Town)%>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(m => m.PostalCode) %>
                        <%: Html.TextBoxFor(m => m.PostalCode, new { id = "PostalCode" })%>
                        <%: Html.ValidationMessageFor(m => m.PostalCode)%>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(m => m.Country) %>
                        <%: Html.DropDownListFor(m => m.Country, (IEnumerable<SelectListItem>)ViewData["countryList"])%>
                        <%: Html.ValidationMessageFor(m => m.Country)%>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(m => m.Birthdate)%>
                        <%: Html.TextBoxFor(m => m.Birthdate, new { id = "Birthdate" })%>
                        <%: Html.ValidationMessageFor(m => m.Birthdate)%>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(m => m.Sex) %>
                        <%: Html.DropDownListFor(m=>m.Sex, (IEnumerable<SelectListItem>)ViewData["sex"])%>
                        <%: Html.ValidationMessageFor(m => m.Sex)%>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(m => m.Telephone)%>
                        <%: Html.TextBoxFor(m => m.Telephone)%>
                        <%: Html.ValidationMessageFor(m => m.Telephone)%>
                    </div>
                    <p>
                        <input type="submit" value="Zapisz" />
                    </p>
                </fieldset>
            </div>
            <% } %>
        </div>

        <div class="mapTowns" id="mapka">
            <%--<%:
            Html.Telerik().GoogleMap().Name("map")
            .Width(400).Height(400)  %>--%>
            <% Html.RenderPartial("Map", ViewData["Map"] as IEnumerable<Erestauracja.ServiceReference.Town>);%>
            <%--Renderuje mapke oraz dzialaja inne jQery skrypty--%>
            <% Html.Telerik().ScriptRegistrar().jQuery(false).jQueryValidation(false).OnDocumentReady("$('#mapTowns').dialog();").Render(); %>
        </div>


        <%-- if lista pobranych miast jest > 1 then pokaz mapke - za pomoca js--%>
        <% //foreach(Erestauracja.ServiceReference.Town x in (IEnumerable<Erestauracja.ServiceReference.Town>)ViewData["miasta"])
            //{
            if (( (IEnumerable<Erestauracja.ServiceReference.Town>)ViewData["Map"] ).Count() > 1)
            //if(true)
            {%>
        <script type="text/javascript">
            document.getElementById('mapka').style.display = "block";
        </script>
        <%}
                else
                {%>
        <script type="text/javascript">
            document.getElementById('mapka').style.display = "none";
        </script>
        <%}%>
        <% //} %>
    </div>

    <script type="text/javascript">
        function ChoseAndSend(town, postalcode) {
            //wstawia pola ze znacznika do textboxow
            var TownName = document.getElementById("TownName");
            var PostalCode = document.getElementById("PostalCode");
            //tutaj potrzebujemy wklepac te wartosci do textboxow...
            TownName.value = town;
            PostalCode.value = postalcode;
        }
    </script>
</asp:Content>

