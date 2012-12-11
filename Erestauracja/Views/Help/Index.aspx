<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Pomoc
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            window.onload = function () {
                document.getElementById("iframepdf").src = "../../Content/Manual-klient.pdf";
            }
        });
    </script>
    <div class="main">
        <iframe id="iframepdf" style="min-width: 900px; height: 600px; margin-left: auto; margin-right: auto;"></iframe>
    </div>
</asp:Content>
