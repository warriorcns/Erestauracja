<%@ Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.UserDataModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

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
        jQuery(function ($) {
            $("#Birthdate").mask("99/99/9999");
        });
    </script>

    <script type="text/javascript">
        $(function () {
            //window.location.href = window.location.href.split('?', 1)[0];
        });
    </script>
    <script type="text/javascript">
        $(function ($) {
            $("#fcs").focus();
        });
    </script>
    <div class="polaRejestracji">
        <div id="Form1" class="formaRejestracji" runat="server">
            <% using (Html.BeginForm()) %>
            <% { %>
            <%: Html.ValidationSummary(true, "Edycja danych nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
            <div>
                <fieldset>
                    <legend>Edycja danych</legend>
                    <p>
                        Wprowadz nowe dane, a następnie kliknij 'Zapisz'.
                    </p>
                    <div>
                        <%: Html.HiddenFor(m=>m.Login) %>
                    </div>
                    <!-- nie edytuje emaila -->
                    
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Email)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Email, new { @id = "fcs" })%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.Email)%>
                        </li>
                    </ul>


                   
                    
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Name)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Name)%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.Name)%>
                        </li>
                    </ul>


                   
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Surname)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Surname)%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.Surname)%>
                        </li>
                    </ul>


                    
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Address)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Address)%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.Address)%>
                        </li>
                    </ul>


                   
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Town)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Town, new { id = "TownName" })%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.Town)%>
                        </li>
                    </ul>


                    
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.PostalCode)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.PostalCode, new { id = "PostalCode" })%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.PostalCode)%>
                        </li>
                    </ul>


                    
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Country)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.DropDownListFor(m => m.Country, (IEnumerable<SelectListItem>)ViewData["countryList"], new { @style="width: 206px;" })%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.Country)%>
                        </li>
                    </ul>


                    
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Birthdate)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Birthdate, new { id = "Birthdate" })%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.Birthdate)%>
                        </li>
                    </ul>


                    
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Sex)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.DropDownListFor(m => m.Sex, (IEnumerable<SelectListItem>)ViewData["sex"], new { @style = "width: 206px;" })%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.Sex)%>
                        </li>
                    </ul>


                    
                    <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Telephone)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Telephone)%>
                    </li>
                    </ul>
                
                    <ul class="rej">
                        <li class="display-labelR">
                            <%: Html.ValidationMessageFor(m => m.Telephone)%>
                        </li>
                    </ul>
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

