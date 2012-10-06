<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.MenuModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <meta charset="utf-8">
	
    <script>
        $(function () {
            $("#accordion").accordion({
                collapsible: true,
                active: false,
                autoHeight: false
            });
        });
	</script>

    <fieldset>
    <legend>Kategorie - kliknij <%: Html.ActionLink("tutaj", "AddCategory", "ManagePanel", new { id = Model.RestaurantID }, null) %> aby dodać nową</legend>
        <div id="accordion">
		        <% foreach (Erestauracja.ServiceReference.Category kategoria in Model.Kategorie)
             { %>
                    <h3><a href="#"><%: kategoria.CategoryName %></a></h3>
	                    <div>
		                    <p><%: kategoria.CategoryDescription %></p>
                            <br />
                            <% if (kategoria.PriceOption != null){ %>
                                <p>Opcja wpływająca na cenę:
                                    <% SelectList list = new SelectList(kategoria.PriceOption.Split(','));  %>
                                    <%: Html.DropDownList("myList", list as SelectList)%>
                                </p>
                            <% } %>
                            <br />
                            
                            <% if (kategoria.NonPriceOption != null){ %>
                                <p>Opcja niewpływająca na cenę:
                                    <% SelectList list = new SelectList(kategoria.NonPriceOption.Split(','));  %>
                                    <%: Html.DropDownList("myList", list as SelectList)%>
                                </p>
                            <% } %>
                            <br />
                            
                            <% if (kategoria.NonPriceOption2 != null){ %>
                                <p>Opcja niewpływająca na cenę:
                                    <% SelectList list = new SelectList(kategoria.NonPriceOption2.Split(','));  %>
                                    <%: Html.DropDownList("myList", list as SelectList)%>
                                </p>
                           <% } %>
                            
                            <p> <%: Html.ActionLink("edytuj", "EditCategoryPage", "ManagePanel", new { id = Model.RestaurantID, cat = kategoria.CategoryID }, null)%> </p>
                            <p> <%: Html.ActionLink("usun", "DeleteCategory", "ManagePanel", new { id = Model.RestaurantID, cat = kategoria.CategoryID }, null)%> </p>
	                    </div>
                <% } %>
	        </div>
    </fieldset>
    </br>
    <fieldset>
    <legend>Produkty - kliknij <%: Html.ActionLink("tutaj", "EditContactPage", "ManagePanel", new { id = Model.RestaurantID }, null) %> aby dodać nowy</legend>

    </fieldset>

</asp:Content>
