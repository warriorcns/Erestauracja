<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.AddProductModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Dodawani produktu nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset>
                <legend>Nowy produkt</legend>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ProductName)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.ProductName)%>
                    <%: Html.ValidationMessageFor(m => m.ProductName)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ProductDescription)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.ProductDescription)%>
                    <%: Html.ValidationMessageFor(m => m.ProductDescription)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Category)%>
                </div>
                <div class="editor-field">
                    <%: Html.DropDownListFor(m => m.Category, (IEnumerable<SelectListItem>)ViewData["categories"])%>
                    <%: Html.ValidationMessageFor(m => m.Category)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Price)%> (
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Price)%>
                    <%: Html.ValidationMessageFor(m => m.Price)%>
                </div>
                

                <%: Html.HiddenFor(m => m.RestaurantID) %>
                </br>
                <p>
                    <input type="submit" value="Dodaj"/>
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
