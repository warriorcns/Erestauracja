<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.AddProductModel>" %>
<%@ Import Namespace="Erestauracja.Helpers" %>
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
                    <%: Html.DropDownListFor(m => m.Category, (IEnumerable<SelectListItem>)ViewData["categories"], new { id = "kategorie", selected = "selected" })%>
                    <%: Html.ValidationMessageFor(m => m.Category)%>
                </div>

                <div id="cenalabele" class="editor-label" style="display:none;">
                
                    <%: Html.LabelFor(m => m.Price)%>
                    <label id="pricelbl"></label>
                </div>
                <div id="cenatb" class="editor-field" style="display:none;">
                    <%: Html.TextBoxFor(m => m.Price, new { id = "pricetxb" })%>
                    <%: Html.ValidationMessageFor(m => m.Price)%>
                </div>
                

                <%: Html.HiddenFor(m => m.RestaurantID, new { id = "resid"}) %>
                </br>
                <p>
                    <input type="submit" value="Dodaj"/>
                </p>
            </fieldset>
        </div>
    <% } %>

         
    <script type="text/javascript">

        $("#kategorie").change(function () {
            var id = "";
            var txt = "";

            $("select option:selected").each(function () {
                id = $(this).val() + " ";
                txt = $(this).text() + " ";
            });

            //alert('id:' + id + 'text:' + txt);

            var resid = $("#resid").val();
            var url = '<%: Url.Action("GetPrices", "ManagePanel") %>';
            var data = { id: id, txt: txt, resid: resid };

            if (!$(id).val()) {
                $.post(url, data, function (data) {
                    // TODO: do something with the response from the controller action
                    //alert('the value was successfully sent to the server' + id);

                    $('#cenatb').show();
                    $('#cenalabele').show();
                    $('#pricelbl').text(data);
                });
            }
        }).trigger('change');

    </script>
</asp:Content>
