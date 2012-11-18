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
                autoHeight: true,
                event: "click",
                collapsible: true
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
           $(".spinner").spinner({
                min: 0,
                max: 100,
                disabled: false
            });
       });
	</script>
    
    <script type="text/javascript">
        $(function () {
            $("#tobasket")
            .button()
            .click(function (event) {
                event.preventDefault();
                //alert("klik");
                //dodaj produkt do koszyka
                //id restauracji, id kategorii, id produktu, wybrana opcja cenowa, 
                //cena w decimal 0.00 , wybrane obie opcje nie wpływające na cene, ilość z selektor i komentarz 

                var str1 = $("#ddl1").val();
                var str2 = $("#ddl2").val();
                var str3 = $("#ddl3").val();
                //alert(str);

                var url = '<%: Url.Action("ToBasket", "Restaurant") %>';
                var data = { id: str };

                if (str.length != 0) {
                    $.post(url, data, function (data) {
                        // TODO: do something with the response from the controller action
                        //alert('the value was successfully sent to the server' + str);
                        window.location.href = data.redirectToUrl;
                    });
                }

            });
        });
    </script>
    

    
    
    
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
                                        <%: Html.DropDownList("DropDownList0", (IEnumerable<SelectListItem>)lista0)%>
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
                                        <%: Html.DropDownList("DropDownList", (IEnumerable<SelectListItem>)lista)%>
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
                                        <%: Html.DropDownList("DropDownList2", (IEnumerable<SelectListItem>)lista2)%>
                                    </div>
                                    <% } %>
                                    <div>
                                        Ilość
                                        <input class="spinner">
                                        <%--<span class="ui-spinner ui-widget ui-widget-content ui-corner-all">
                                        <input class="spinner" class="ui-spinner-input" autocomplete="off" role="spinbutton"/>
                                            <a class="ui-spinner-button ui-spinner-up ui-corner-tr ui-button ui-widget ui-state-default ui-button-text-only" tabindex="-1" role="button" aria-disabled="false">
                                            <a class="ui-spinner-button ui-spinner-down ui-corner-br ui-button ui-widget ui-state-default ui-button-text-only" tabindex="-1" role="button" aria-disabled="false">
                                        </span>--%>
                                    </div>
                                    <div>
                                        <button id="tobasket">Do koszyka</button>
                                        <input type="submit" value="dodaj"/>
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


<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    
    
</asp:Content>