<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.MenuModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <meta charset="utf-8">

	<script>
	    $(function () {
	        $("#tabs").tabs();
	    });
	</script>

    <script>
        $(function () {
            $(".accordion2").accordion({
                autoHeight: false,
                active: false,
                event: "mouseover"
            });
        });
	</script>

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
    <legend>Produkty - kliknij <%: Html.ActionLink("tutaj", "AddProduct", "ManagePanel", new { id = Model.RestaurantID }, null) %> aby dodać nowy</legend>
    <div id="tabs">
	    <ul>
            <% 
            int i = 1;
            foreach (Erestauracja.ServiceReference.Menu menu in Model.Menu) { %>
                <li><a href="#<%: "tabs-"+i %>"> <%: menu.CategoryName %> </a></li>
	        <%  i++; } %>	
	    </ul>
        <% 
        int j = 1;
        foreach (Erestauracja.ServiceReference.Menu menu in Model.Menu)
        { %>
            <div id="<%: "tabs-"+j %>">
                <p><%: menu.CategoryDescription %></p>
                <div class="accordion2">
                    <% foreach (Erestauracja.ServiceReference.Product product in menu.Products) { %>
                        <h3><a href="#"><%: product.ProductName %></a></h3>
                            <div>
                                <p><%: product.ProductDescription %></p>
                                </br>
                                <p>Cena:</p>
                                <div>
                                    <% if (product.PriceOption != null)
                                       {%>
                                        <%  string[] ceny = product.Price.Split('|');
                                            string[] opcje = product.PriceOption.Split(',');%>
                                        <% if(ceny.Length == opcje.Length) {%>
                                            <% for (int x = 0; x < opcje.Length; x++) { %>
                                                <div>
                                                    <span><%: opcje[x] %>  -</span>
                                                    <span>  <%: ceny[x] %></span>
                                                </div>
                                            <% } %>
                                        <% }
                                           else{ %>
                                           <div> Błąd </div>
                                            <% } %>
                                    <% }
                                        else{ %>
                                           <div> Błąd </div>
                                    <% } %>
                                </div>  
                            </div>
                    <% } %>
                </div>
            </div>
	    <%  j++;  } %>	
    </div>

    </fieldset>

</asp:Content>
