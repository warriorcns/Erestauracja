<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <script>
        $(function () {
            $("#datepicker").datepicker();
        });
	</script>



<div class="demo">

<p>Date: <input type="text" id="datepicker"></p>

</div><!-- End demo -->
   
</asp:Content>
