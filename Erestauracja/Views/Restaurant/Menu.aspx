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
                min: 1,
                max: 100,
                disabled: false
            });
       });
	</script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".spinner").val('1');
        });
   </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".tobasket")
            .button()
            .click(function (event) {
                event.preventDefault();
            });
        });
            
        
    </script>

    <script type="text/javascript">

        function go(resid, catid, prodid) {

            //dodaj produkt do koszyka
            //id restauracji, id kategorii, id produktu, wybrana opcja cenowa, 
            //cena w decimal 0.00 , wybrane obie opcje nie wpływające na cene, ilość z selektor i komentarz 

            //alert("go()" + resid + catid + prodid);

            //1 DDL - wybrana opcja cenowa
            //alert($("#Cena" + prodid).val());
            var opcjacenowa = $("#Cena" + prodid).val();

            //2 DDL
            //alert($("#Dod" + prodid).val());
            var dodatki = $("#Dod" + prodid).val();

            //3 DDL
            //alert($("#Opcja" + prodid).val());
            var opcje = $("#Opcja" + prodid).val();

            //selector
            //alert($("#selector" + prodid).val());
            var count = $("#selector" + prodid).val();

            //textarea - komentarz
            //alert($("#textarea" + prodid).val());
            var comm = $("#textarea" + prodid).val();
            
            var url = '<%: Url.Action("ToBasket", "Basket") %>';
            var data = { resid: resid, catid: catid, prodid: prodid, opcjacenowa: opcjacenowa, dodatki: dodatki, opcje: opcje, count: count, comm: comm };

            if (count.length != 0) {
                $.post(url, data, function (data) {
                    // TODO: do something with the response from the controller action
                    //alert('the value was successfully sent to the server' + str);
                    window.location.href = data.redirectToUrl;
                });
            } else {
                alert("Wybierz ilość.");
            }
        }
    </script>
    

    
    
    <% using (Html.BeginForm()) %>
        <% { %>
    <fieldset class="produkty-fieldset">
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
                                    <% if (!String.IsNullOrWhiteSpace(product.ProductDescription)) %>
                                    <% { %>
                                    <p>Opis: <%: product.ProductDescription%></p>
                                    </br>
                                    <% } %>
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
                                    </br>
                                    <div>Do wyboru:
                                        <% List<SelectListItem> lista0 = new List<SelectListItem>(); %>
                                        <% foreach(string item in menu.PriceOption.Split(',')) %>
                                        <% { %>
                                                <% lista0.Add(new SelectListItem{Text = item, Value = item}); %>
                                        <% } %>
                                        <%: Html.DropDownList("DropDownList0", (IEnumerable<SelectListItem>)lista0, new { id = "Cena" + product.ProductId })%>
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
                                        <%: Html.DropDownList("DropDownList", (IEnumerable<SelectListItem>)lista, new { id = "Dod" + product.ProductId })%>
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
                                        <%: Html.DropDownList("DropDownList2", (IEnumerable<SelectListItem>)lista2, new { id = "Opcja" + product.ProductId })%>
                                    </div>
                                    <% } %>
                                    <% if (User.Identity.IsAuthenticated) %>
                                    <% { %>
                                    </br>
                                    <div>
                                        Ilość
                                        <input class="spinner" id="selector<%:product.ProductId %>">
                                        <%--<span class="ui-spinner ui-widget ui-widget-content ui-corner-all">
                                        <input class="spinner" class="ui-spinner-input" autocomplete="off" role="spinbutton"/>
                                            <a class="ui-spinner-button ui-spinner-up ui-corner-tr ui-button ui-widget ui-state-default ui-button-text-only" tabindex="-1" role="button" aria-disabled="false">
                                            <a class="ui-spinner-button ui-spinner-down ui-corner-br ui-button ui-widget ui-state-default ui-button-text-only" tabindex="-1" role="button" aria-disabled="false">
                                        </span>--%>
                                    </div>
                                    <div>
                                        Komentarz do produktu:
                                    </div>
                                    <div>
                                        <textarea id="textarea<%: product.ProductId %>"></textarea>
                                    </div>
                                    </br>
                                    <div>
                                        <button class="tobasket" onclick="go('<%: Model.RestaurantID %>','<%: menu.CategoryID %>','<%: product.ProductId %>')">Do koszyka</button>
                                    </div>
                                    <% } %>
                            </div>
                    <% } %>
                </div>
            </div>
	    <% j++; %>
        <% } %>	
    </div>

    </fieldset>
    <% }%>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    
    
</asp:Content>