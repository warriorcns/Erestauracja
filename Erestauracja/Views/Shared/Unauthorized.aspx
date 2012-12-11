<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Unauthorized
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    setTimeout(function () {
        window.location.href = "Index";
    },3000);
</script>
    <div>
        <h3>Ups, nie masz uprawnien by obejrzeć tę stronę ;( </h3>
        <h5>Za chwilę nastąpi przekierowanie do strony początkowej...</h5>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
