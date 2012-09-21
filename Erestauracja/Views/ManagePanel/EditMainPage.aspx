<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.TestModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
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

<div class="PaneleOpisRestauracji">
<fieldset>
    <legend>Strona główna - edycja</legend>
        <asp:Panel ID="Panel1" class="PanelOpisRestauracji" runat="server" ScrollBars="Auto">
           <p style="position: relative; padding: 10px 10px 10px 10px; font-size: 15px;"> 
             <!--   <textarea id="test" cols="50" rows="15"></textarea> -->
                <%: Html.TextAreaFor(m => m.Html, new { id = "test" }) %>
           </p> 
                
        </asp:Panel>
        <asp:Panel ID="Panel2" class="PanelTotoPromocje" runat="server" ScrollBars="Auto">
            <asp:Panel ID="Panel3" class="PanelToto" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Foto)%>
            </asp:Panel>
            <asp:Panel ID="Panel4" class="PanelPromocje" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Promocje)%>
            </asp:Panel>
        </asp:Panel>
    </fieldset>
</div>

</asp:Content>
