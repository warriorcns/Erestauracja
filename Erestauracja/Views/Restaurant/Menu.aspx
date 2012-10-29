<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.ClientMenuModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <meta charset="utf-8">

	<script type="text/javascript">
	    $(function () {
	        $("#tabs-left").tabs({
	            heightStyle: 'auto'
	        });
	    });
	</script>

    <script type="text/javascript">
        $(function () {
            $(".accordion2").accordion({
                active: false,
                autoHeight: false,
                event: "click",
                collapsible: true
            });
        });
    </script>

    <script type="text/javascript">
       $(function () {
            $(".selector").spinner({
                min: 0,
                max: 100,
                icons: { down: "custom-down-icon", up: "custom-up-icon" },
                disabled: false
            });
       });
	</script>

    <%--<input id="selector">--%>

    <fieldset class="produkty-fieldset">
    <legend>Menu - kliknij <%: Html.ActionLink("tutaj", "EditMenuPage", "ManagePanel", new { id = Model.RestaurantID }, null) %> aby edytować</legend>
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
                    <% foreach(Erestauracja.ServiceReference.Product product in menu.Products) %>
                    <% { %>
                        <h3><a href="#"><%: product.ProductName %> </a></h3>
                            <div>
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
                                                                    <div> Błąd: Zgłoś </div>
                                                            <% } %>
                                                    </div>
                                            <% } %>
                                    <% } %>
                                    <% else %>
                                    <% { %>
                                           <div> Błąd: Nieznana cena, zgłoś błąd ?? </div>
                                    <% } %> 

                                    <% if(menu.PriceOption != null) %>
                                    <% { %>
                                    <div>Do wyboru:
                                        <% List<SelectListItem> lista0 = new List<SelectListItem>(); %>
                                        <% foreach(string item in menu.PriceOption.Split(',')) %>
                                        <% { %>
                                                <% lista0.Add(new SelectListItem{Text = item, Value = item}); %>
                                        <% } %>
                                        <%: Html.DropDownList("DropDownList0", (IEnumerable<SelectListItem>)lista0 )%>
                                    </div>
                                    <% } %>
                                    <% if(menu.NonPriceOption != null) %>
                                    <% { %>
                                    <div>Do wyboru:
                                        <% List<SelectListItem> lista = new List<SelectListItem>(); %>
                                        <% foreach(string item in menu.NonPriceOption.Split(',')) %>
                                        <% { %>
                                                <% lista.Add(new SelectListItem{Text = item, Value = item}); %>
                                        <% } %>
                                        <%: Html.DropDownList("DropDownList", (IEnumerable<SelectListItem>)lista )%>
                                    </div>
                                    <% } %>
                                    <% if(menu.NonPriceOption2 != null) %>
                                    <% { %>
                                    <div>Do wyboru:
                                        <% List<SelectListItem> lista2 = new List<SelectListItem>(); %>
                                        <% foreach(string item in menu.NonPriceOption2.Split(',')) %>
                                        <% { %>
                                                <% lista2.Add(new SelectListItem{Text = item, Value = item}); %>
                                        <% } %>
                                        <%: Html.DropDownList("DropDownList2", (IEnumerable<SelectListItem>)lista2 )%>
                                    </div>
                                    <% } %>
                                    <div>
                                        Ilość
                                        <input class="selector">
                                        <%--<span class="ui-spinner ui-widget ui-widget-content ui-corner-all">
                                        <input class="spinner" class="ui-spinner-input" autocomplete="off" role="spinbutton"/>
                                            <a class="ui-spinner-button ui-spinner-up ui-corner-tr ui-button ui-widget ui-state-default ui-button-text-only" tabindex="-1" role="button" aria-disabled="false">
                                            <a class="ui-spinner-button ui-spinner-down ui-corner-br ui-button ui-widget ui-state-default ui-button-text-only" tabindex="-1" role="button" aria-disabled="false">
                                        </span>--%>
                                    </div>
                            </div>
                    <% } %>
                </div>
            </div>
	    <% j++; %>
        <% } %>	
    </div>

    </fieldset>

</asp:Content>
