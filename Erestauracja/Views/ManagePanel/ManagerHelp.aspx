<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Pomoc
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            window.onload = function () {
                document.getElementById("iframepdf").src = "../../Content/Manual-menadżer.pdf";
            }
        });
    </script>

    <div class="main">
        <iframe id="iframepdf" style="width:100%; height: 600px;"></iframe>
    </div>

</asp:Content>
