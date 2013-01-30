<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.LogOnModel>" %>
<%@ Import Namespace="Erestauracja.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    

    <!-- initialize keyboard (required) -->
    <script type="text/javascript">
        $(function () {
            $('#keyboard').keyboard({
            layout: 'international',
            autoAccept: 'true'
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            if (history.length > 0) history.go(+1);
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            var name = '<%:User.Identity.Name  %>';

            //if ($("#user:contains('|')")) {
            if (name.indexOf("|") >= 0){
                //redirect to pos/locked
                var url = '<%: Url.Action("locked", "POS") %>';
                var data = { };

                //alert("Zawiera |");
                $.post(url, data, function (data) {
                    // TODO: do something with the response from the controller action
                    //alert('the value was successfully sent to the server' + str);
                    //alert("Przekierowuje");
                    //window.location.href = "/POS/locked";
                });

            }
            else {
                //nothing
            }
        });

    </script>

    
    <% using (Html.BeginForm()) %>
    <% { %>
    <%: Html.ValidationSummary(true, "Logowanie nie powiodło się. Popraw błędnie wypełnione pola i spróbuj ponownie.")%>
    <div class="locked-login">

                <%: Html.Hidden(User.Identity.Name, null, new { id = "user" })%>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Login, new { @class = "LargeFont" })%>
                   
                </div>
                <div class="editor-field">
                    <%--: Html.TextBoxFor(m => m.Login)--%>
                    <%: Html.DropDownListFor(m => m.Login, (IEnumerable<SelectListItem>)ViewData["logins"], new { @class = "locked-fields" })%>
                    <%: Html.ValidationMessageFor(m => m.Login)%>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password, new { @class = "LargeFont" })%>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password, new { id = "keyboard", @class = "locked-fields"})%>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>

                <input type="submit" value="Zaloguj" class="locked-button"/>
    </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
