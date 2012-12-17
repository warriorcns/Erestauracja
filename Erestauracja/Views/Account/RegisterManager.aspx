﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.RegisterModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Rejestracja
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Uwaga!</h1>
Aby dodać nową restaurację należy stworzyć konto menadżera, które umożliwia nie tylko dodawanie nowych lokali ale również zarządzanie nimi oraz personelem.
</br>Wykorzystując w tym celu istniejące konto, stracisz możliwość wykonywania zamówień.</br></br>

<h2>Utwórz nowe konto menadżera wypełniając poniższy formularz lub wykorzystaj <%: Html.ActionLink("istniejące konto", "ExistingManager", "Account")%></p></h2>
    
    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput-1.3.js" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jQuery.datepicker-pl.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#Birthdate").datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: 'c-100:c+0',
                dateFormat: 'dd/mm/yy'
            });
        });
    </script>
    <script type="text/javascript">
        $(function ($) {
            $("#Birthdate").mask("99/99/9999");      
        });
    </script>


    <div class="polaRejestracji">
        <form class="formaRejestracji" id="Form1" runat="server">
        <% using (Html.BeginForm())
           { %>
                <%: Html.ValidationSummary(true, "Rejestracja konta nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
                <div>
            <fieldset>
                <legend>Dane rejestracji</legend>
                <p>
                    Wprowadz swoje dane, a następnie kliknij 'Załóż konto'.
                </p>
                
                 <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Login) %>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Login)%>
                    </li>
                </ul>
                
                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.ValidationMessageFor(m => m.Login)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Email)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Email)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.ValidationMessageFor(m => m.Email)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.ConfirmEmail)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.ConfirmEmail)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.ValidationMessageFor(m => m.ConfirmEmail)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Password)%>
                        (Minimum
                        <%: Membership.MinRequiredPasswordLength %>
                        znaków.) 
                        </li>
                    <li class="editor-labelR">
                        <%: Html.PasswordFor(m => m.Password)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                       <%: Html.ValidationMessageFor(m => m.Password)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.ConfirmPassword)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.PasswordFor(m => m.ConfirmPassword)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                       <%: Html.ValidationMessageFor(m => m.ConfirmPassword)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Question)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Question)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.ValidationMessageFor(m => m.Question)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Answer)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Answer)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.ValidationMessageFor(m => m.Answer)%>
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
                        <%: Html.DropDownListFor(m => m.Country, (IEnumerable<SelectListItem>)ViewData["countryList"], new { style="width:206px" })%>
                    </li>
                    <li class="validation-labelR">
                        <%: Html.ValidationMessageFor(m => m.Country)%>
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
                        <%: Html.DropDownListFor(m => m.Sex, (IEnumerable<SelectListItem>)ViewData["sex"], new { style = "width:206px" } )%>
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
                    <input type="submit" value="Załóż konto" />
                </p>
            </fieldset>
        </div>
        <% } %>
        </form>

        <div class="mapTowns" id="mapka">
            <% Html.RenderPartial("Map", ViewData["Map"] as IEnumerable<Erestauracja.ServiceReference.Town>);%>
            <%--Renderuje mapke oraz dzialaja inne jQery skrypty--%>
            <% Html.Telerik().ScriptRegistrar().jQuery(false).jQueryValidation(false).OnDocumentReady("$('#mapTowns').dialog();").Render(); %>
        </div>

        <%-- if lista pobranych miast jest > 1 then pokaz mapke - za pomoca js--%>
        <% if (( (IEnumerable<Erestauracja.ServiceReference.Town>)ViewData["Map"] ).Count() > 1)
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
        
    </div>

    <script type="text/javascript">
        function ChoseAndSend(town, postalcode) {
            var TownName = document.getElementById("TownName");
            var PostalCode = document.getElementById("PostalCode");
            TownName.value = town;
            PostalCode.value = postalcode;
        }
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
