<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    areaselect
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <img id="photo" src="../../Content/images/resid1/1.jpg" alt="logo" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#photo').imgAreaSelect({
                instance: true               
            });
        });
</script>
    
    <link href="../../Content/style/areaselect/imgareaselect-default.css" rel="stylesheet"
        type="text/css" />

    <script src="../../Scripts/areaselect/jquery.min.js" type="text/javascript"></script>
    <script src="../../Scripts/areaselect/jquery.imgareaselect.pack.js" type="text/javascript"></script>

  
  
</asp:Content>