<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <meta charset="utf-8">

	<script>
	    $(function () {
	        $("#tabs").tabs();
	    });
	</script>

    <script>
        $(function () {
            $("#accordion").accordion({
                autoHeight: false,
                active:false,
                event: "mouseover"
            });
        });
	</script>
<fieldset>
    <legend>Menu - kliknij <%: Html.ActionLink("tutaj", "EditMenuPage", "ManagePanel", new { id = 1 }, null) %> aby edytować</legend>
     
<div id="tabs">
	<ul>
		<li><a href="#tabs-1">PIZZA</a></li>
		<li><a href="#tabs-2">CALZONE (PIERÓG)</a></li>
		<li><a href="#tabs-3">DANIA OBIADOWE DRÓB</a></li>
        <li><a href="#tabs-4">DANIA OBIADOWE WIEPRZOWNIA</a></li>
		<li><a href="#tabs-5">RYBY</a></li>
		<li><a href="#tabs-6">MAKARONY</a></li>
        <li><a href="#tabs-7">SAŁATKA</a></li>
        <li><a href="#tabs-8">INNE</a></li>
	</ul>
	<div id="tabs-1">
    <p>Wszystkie pizze zawierają sos pomidorowy oraz ser mazzarella, do wyboru zwykłe lub podwójne ciasto.</p>
        <div id="accordion">
	        <h3><a href="#">Margaritta</a></h3>
	            <div>
		            <p>(sos pomidorowy, ser mozzarella, kompozycja ziół)</p>
                    </br>
                    <p>
                        <pre>Cena: Ciasto zwykłe   - 11,00 zł</pre>
                        <pre>      Ciasto podwójne - 11,00 zł</pre>
                    </p>
                    </br>
                    <p>Ilość sztuk:
                        <asp:DropDownList ID="DropDownList1" runat="server">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>Ciasto:
                        <asp:DropDownList ID="DropDownList2" runat="server">
                            <asp:ListItem>Zwykłe</asp:ListItem>
                            <asp:ListItem>Podwójne</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <asp:Button ID="Button1" runat="server" Text="Dodaj do koszyka" />
                    </p>
	            </div>
	        <h3><a href="#">Pieczarkowa</a></h3>
	            <div>
		            <p>Seducibus urna. </p>
	            </div>
	        <h3><a href="#">Hawaii</a></h3>
	            <div>
		            <p>Nam </p>
	            </div>
	        <h3><a href="#">Salami</a></h3>
	            <div>
		            <p>Crasvel est. </p>
	            </div>
        </div>
	</div>
	
    
    <div id="tabs-2">
		<p>Morbi tincidunt, dui sit amet facilisis feugiat, odio metus gravida ante, ut pharetra massa metus id nunc. Duis scelerisque molestie turpis. Sed fringilla, massa eget luctus malesuada, metus eros molestie lectus, ut tempus eros massa ut dolor. Aenean aliquet fringilla sem. Suspendisse sed ligula in ligula suscipit aliquam. Praesent in eros vestibulum mi adipiscing adipiscing. Morbi facilisis. Curabitur ornare consequat nunc. Aenean vel metus. Ut posuere viverra nulla. Aliquam erat volutpat. Pellentesque convallis. Maecenas feugiat, tellus pellentesque pretium posuere, felis lorem euismod felis, eu ornare leo nisi vel felis. Mauris consectetur tortor et purus.</p>
	</div>
	<div id="tabs-3">
		<p>Mauris eleifend est et turpis. Duis id erat. Suspendisse potenti. Aliquam vulputate, pede vel vehicula accumsan, mi neque rutrum erat, eu congue orci lorem eget lorem. Vestibulum non ante. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Fusce sodales. Quisque eu urna vel enim commodo pellentesque. Praesent eu risus hendrerit ligula tempus pretium. Curabitur lorem enim, pretium nec, feugiat nec, luctus a, lacus.</p>
		<p>Duis cursus. Maecenas ligula eros, blandit nec, pharetra at, semper at, magna. Nullam ac lacus. Nulla facilisi. Praesent viverra justo vitae neque. Praesent blandit aretra blandit, magna ligula faucibus eros, id euismod lacus dolor eget odio. Nam scelerisque. Donec non libero sed nulla mattis commodo. Ut sagittis. Donec nisi lectus, feugiat porttitor, tempor ac, tempor vitae, pede. Aenean vehicula velit eu tellus interdum rutrum. Maecenas commodo. Pellentesque nec elit. Fusce in lacus. Vivamus a libero vitae lectus hendrerit hendrerit.</p>
	</div>
</div>

</fieldset>
</asp:Content>
