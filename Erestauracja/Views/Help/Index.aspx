<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Pomoc
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../Scripts/jquery.gdocsviewer.min.js" type="text/javascript"></script>
    <script type"text/javascript">
        $(document).ready(function () {
            $('a.embed').gdocsViewer({ width: 400, height: 500 });
            $('#embedURL').gdocsViewer();
        });
    
    </script>


    <a href="urltofile.pdf" class="embed">Download file</a>
    <a href="urltofile.pdf" class="embedURL">Download file</a>



</asp:Content>
