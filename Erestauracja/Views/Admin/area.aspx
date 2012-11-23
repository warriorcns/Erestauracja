<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Admin/Index.master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminPlaceHolder" runat="server">

<h2>area</h2>
 <img id="photo" class="123" src="../../Content/images/resid1/1.jpg" alt="logo" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        $("img#photo").imgAreaSelect({
            
            onInit: function () {
                alert("dupa");
            }
        });
    });
</script>
    
    
</asp:Content>