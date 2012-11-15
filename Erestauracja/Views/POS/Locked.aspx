<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.LogOnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <meta charset="utf-8"/>

    <!-- jQuery & jQueryUI + theme -->
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/ui-lightness/jquery-ui.css" rel="stylesheet">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
    <link href="../../Content/style/keyboard.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.keyboard.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-autocomplete.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-mobile.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-navigation.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.keyboard.extension-typing.js" type="text/javascript"></script> 
    <script src="../../Scripts/jquery.mousewheel.js" type="text/javascript"></script>



<!-- initialize keyboard (required) -->
    <script type="text/javascript">
        $(function () {
            $('#keyboard').keyboard();
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#keyboard').keyboard({ layout: 'qwerty' });
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
