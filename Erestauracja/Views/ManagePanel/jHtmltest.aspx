<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.TestModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        /* body { background: #ccc;} */
        div.jHtmlArea .ToolBar ul li a.custom_disk_button 
        {
            background: url(images/disk.png) no-repeat;
            background-position: 0 0;
        }
        
        div.jHtmlArea { border: solid 1px #ccc; background-color: White; }
    </style>
    
    <script type="text/javascript">
        $(function () {
            $("#test").htmlarea();
        });
    </script>
    
    <textarea id="test" cols="50" rows="15"></textarea>
    <%: Html.TextAreaFor(m => m.Html, new { id = "test" })%>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
   
    
</asp:Content>
