<%@ Page Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.RegisterRestaurantModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Jmelosegui.Mvc.Controls" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI" %>
<%@ Import Namespace="Erestauracja.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Dodaj nową restaurację
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Wypełnij poniższy formularz oraz kliknij 'Zapisz', aby dodać nową restaurację.</h2>
    
    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <script src="../../Scripts/jquery.price_format.1.7.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function ($) {
            $("#fcs").focus();
        });
    </script>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#DeliveryTimeID").mask("99:99:99");
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#spinner").spinner({
                min: 1,
                max: 100,
                disabled: false
            });
        });
	</script>

    <script type="text/javascript">
    $(document).ready(function () {
        $('#price').priceFormat({
            prefix: '',
            centsSeparator: ',',
            thousandsSeparator: ''
        });
    });
   </script>

   <script type="text/javascript">
       $(document).ready(function () {
           $("#spinner").val('0.00');
       });
   </script>

    <div class="polaRejestracji">

        <form class="formaRejestracji" runat="server">
        <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Rejestracja restauracji nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset class="polaRejestracji polaRejWidth">
                <legend>Dane rejestracji</legend>
                
                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Login)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Login, new { @id = "fcs" })%>
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
                        <%: Html.LabelFor(m => m.Password)%> (Minimum <%: Membership.MinRequiredPasswordLength %> znaków.)
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
                        <%: Html.LabelFor(m => m.DisplayName)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.DisplayName)%>   
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                         <%: Html.ValidationMessageFor(m => m.DisplayName)%>
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
                        <%: Html.TextBoxFor(m => m.Town, new { id = "TownName"})%>
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
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                         <%: Html.ValidationMessageFor(m => m.Country)%>
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


                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Nip)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Nip)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                         <%: Html.ValidationMessageFor(m => m.Nip)%>
                    </li>
                </ul>


                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Regon)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Regon)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                         <%: Html.ValidationMessageFor(m => m.Regon)%>
                    </li>
                </ul>


                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.DeliveryTime)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.DeliveryTime, new { id = "DeliveryTimeID" })%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                         <%: Html.ValidationMessageFor(m => m.DeliveryTime)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.DeliveryPrice)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.DeliveryPrice, new { id = "price" })%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                         <%: Html.ValidationMessageFor(m => m.DeliveryPrice)%>
                    </li>
                </ul>
                <p>
                    <input type="submit" value="Zapisz"/>
                </p>
            </fieldset>
        </div>
        <% } %>
        </form>


        <div class="mapTowns" id="mapka">
        
        <% Html.RenderPartial("Map", ViewData["Map"] as IEnumerable<Erestauracja.ServiceReference.Town>); %>
        <%--Renderuje mapke oraz dzialaja inne jQery skrypty--%> 
        <% Html.Telerik().ScriptRegistrar().jQuery(false).jQueryValidation(false).OnDocumentReady("$('#mapTowns').dialog();").Render(); %>
    </div>
        
        <%-- if lista pobranych miast jest > 1 then pokaz mapke - za pomoca js--%>
        <% if(((IEnumerable<Erestauracja.ServiceReference.Town>)ViewData["Map"]).Count() > 1) { %>
                <script type="text/javascript"> document.getElementById('mapka').style.display = "block"; </script>
        <% } else { %> 
                <script type="text/javascript"> document.getElementById('mapka').style.display = "none"; </script>
        <% } %> 
                
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
