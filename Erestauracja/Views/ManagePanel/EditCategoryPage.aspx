<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.Category>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Rejestracja restauracji nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset>
                <legend>Edytuj kategorie</legend>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.CategoryName)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.CategoryName)%>
                    <%: Html.ValidationMessageFor(m => m.CategoryName)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.CategoryDescription)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.CategoryDescription)%>
                    <%: Html.ValidationMessageFor(m => m.CategoryDescription)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.PriceOption)%> (dowolna ilość opcji oddzielonych przecinkiem ',' np. mała,średnia,duża)
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.PriceOption)%>
                    <%: Html.ValidationMessageFor(m => m.PriceOption)%>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.NonPriceOption)%> (dowolna ilość opcji oddzielonych przecinkiem ',' np. ketchup,sos pomidorowy)
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.NonPriceOption)%>
                    <%: Html.ValidationMessageFor(m => m.NonPriceOption)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.NonPriceOption2)%> (dowolna ilość opcji oddzielonych przecinkiem ',' np. ciasto cieknie,ciasto średnie,podwójne ciasto)
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.NonPriceOption2)%>
                    <%: Html.ValidationMessageFor(m => m.NonPriceOption2)%>
                </div>

                <%: Html.HiddenFor(m => m.RestaurantID) %>
                <%: Html.HiddenFor(m => m.CategoryID) %>
                </br>
                <p>
                    <input type="submit" value="Zapisz"/>
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
