<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.MenuModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <meta charset="utf-8">

	<script>
	    $(function () {
	        $("#tabs-left").tabs({
	            heightStyle: 'auto'
            });
	    });
	</script>

    <script>
        $(function () {
            $(".accordion2").accordion({
                active: false,
                autoHeight: false,
                event: "click",
                collapsible: true
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

    <%: Html.ValidationSummary(true, "") %>
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
                            
                            <p> <%: Html.ActionLink("Edytuj", "EditCategoryPage", "ManagePanel", new { id = Model.RestaurantID, cat = kategoria.CategoryID }, null)%> </p>
                            <p> <%: Html.ActionLink("Usuń", "DeleteCategory", "ManagePanel", new { id = Model.RestaurantID, cat = kategoria.CategoryID }, null)%> </p>
	                    </div>
                <% } %>
	        </div>
    </fieldset>
    </br>
    <fieldset class="produkty-fieldset">
    <legend>Produkty - kliknij <%: Html.ActionLink("tutaj", "AddProduct", "ManagePanel", new { id = Model.RestaurantID }, null) %> aby dodać nowy</legend>
    <div id="tabs-left">
	    <ul>
            <% 
            int i = 1;
            foreach (Erestauracja.ServiceReference.Menu menu in Model.Menu) { %>
                <li><a href="#<%: "tabs-"+i %>"> <%: menu.CategoryName %> </a></li>
	        <%  i++; } %>	
	    </ul>
        
        <% int j = 1; %>
        <% foreach (Erestauracja.ServiceReference.Menu menu in Model.Menu) %>
        <% { %>
            <div id="<%: "tabs-"+j %>">
                <p><%: menu.CategoryDescription %></p>
                <div class="accordion2">
                    <% foreach (Erestauracja.ServiceReference.Product product in menu.Products) %>
                    <% { %>
                        <h3><a href="#"><%: product.ProductName %> (
                            <% if(product.IsAvailable == true) %>
                            <% { %>
                                    dostępny)
                            <% } %>
                            <% else %>
                            <% { %>
                                    niedostępny)
                            <% } %>
                        </a></h3>
                            <div>
                            <% if(product.IsEnabled != false) %>
                            <% { %>
                                    <p>Opis: <%: product.ProductDescription %></p>
                                    </br>
                                    <p>Cena:</p>
                                    <% if(product.Price != null) %>
                                    <% { %>
                                            <% if(product.PriceOption == null) %>
                                            <% { %>
                                                    <div> <%: product.Price %> </div>
                                            <% } %>
                                            <% else %>
                                            <% { %>
                                                    <div>
                                                            <% string[] ceny = product.Price.Split('|'); %>
                                                            <% string[] opcje = product.PriceOption.Split(','); %>
                                                            <% if(ceny.Length == opcje.Length) %>
                                                            <% { %>
                                                                    <% for(int x = 0; x < opcje.Length; x++) %>
                                                                    <% { %>
                                                                            <div>
                                                                                <span><%: opcje[x] %>  -</span>
                                                                                <span>  <%: ceny[x] %></span>
                                                                            </div>
                                                                    <% } %>
                                                            <% } %>
                                                            <% else %>
                                                            <% { %>
                                                                    <div> Błąd: Wymaga edycji! </div>
                                                            <% } %>
                                                    </div>
                                            <% } %>
                                    <% } %>
                                    <% else %>
                                    <% { %>
                                           <div> Błąd: Nieznana cena, wymaga edycji! </div>
                                    <% } %> 
                            <% } %>
                            <% else %>
                            <% { %>
                                    <div> Produkt zawiera nieprawidłowe dane i wymaga edycji! </div>
                            <% } %>

                            <p><%: Html.ActionLink("Edytuj", "EditProduct", "ManagePanel", new { id = product.ProductId, cat = product.CategoryId, res= product.RestaurantId }, null) %></p> 
                            <p><%: Html.ActionLink("Usuń", "DeleteProduct", "ManagePanel", new { id = product.ProductId, res= product.RestaurantId }, null) %></p> 
                            </div>
                    <% } %>
                </div>
            </div>
	    <% j++; %>
        <% } %>	
    </div>

    </fieldset>

</asp:Content>
