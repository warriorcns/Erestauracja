﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.ClientMenuModel>" %>

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
                event: "mouseover",
                collapsible: true
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

                                    <% if(menu.NonPriceOption != null) %>
                                    <% { %>
                                    <div>Do wyboru:
                                        <asp:DropDownList ID="DropDownList" runat="server">

                                                <asp:ListItem>Zwykłe</asp:ListItem>

                                        </asp:DropDownList>
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

</asp:Content>
