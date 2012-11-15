<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.LogOnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <meta charset="utf-8"/>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <link href="../../Content/style/keyboard.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.keyboard.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-autocomplete.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-mobile.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-navigation.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-typing.js" type="text/javascript"></script>

    <!-- jQuery & jQuery UI + theme (required) -->
    <link href="http://code.jquery.com/ui/1.9.0/themes/ui-darkness/jquery-ui.css" rel="stylesheet"/>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8/jquery.js"/>
    <script src="http://code.jquery.com/ui/1.9.0/jquery-ui.min.js"/>
<!-- keyboard widget css & script (required) -->
<link href="css/keyboard.css" rel="stylesheet"/>
<script src="js/jquery.keyboard.js"/>
<!-- keyboard extensions (optional) -->
<script src="js/jquery.mousewheel.js"/>
<!--
	<script src="js/jquery.keyboard.extension-typing.js"></script>
	<script src="js/jquery.keyboard.extension-autocomplete.js"></script>
	-->
<!-- initialize keyboard (required) -->
<script>
    $(function () {
        $('#keyboard').keyboard();
    });
	</script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#qwerty').keyboard({ layout: 'qwerty' });
        });
   </script>

    <% using (Html.BeginForm()) %>
    <% { %>
    <%: Html.ValidationSummary(true, "Logowanie nie powiodło się. Popraw błędnie wypełnione pola i spróbuj ponownie.")%>
    <div class="locked-login">

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Login) %>
                </div>
                <div class="editor-field">
                    <%--: Html.TextBoxFor(m => m.Login)--%>
                    <%: Html.DropDownListFor(m => m.Login, (IEnumerable<SelectListItem>)ViewData["logins"])%>
                    <%: Html.ValidationMessageFor(m => m.Login)%>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                <%--<%: Html.DropDownListFor(m=>m.Login, ViewData["logins"] as /IEnum /erable<>) %>--%>
                <%--:Html.PasswordFor(m => m.Password)--%>
    
                <%: Html.TextBoxFor(m => m.Password, new { id = "keyboard" })%>

                <input type="submit" value="Zaloguj"/>
    </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
