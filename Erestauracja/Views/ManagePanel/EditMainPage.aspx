<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.TestModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <script src="../../Scripts/jHtmlArea-0.7.5.min.js" type="text/javascript"></script>
    <link href="../../Content/style/jHtmlArea.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jHtmlArea.ColorPickerMenu-0.7.0.js" type="text/javascript"></script>
    <link href="../../Content/style/jHtmlArea.ColorPickerMenu.css" rel="stylesheet" type="text/css" />

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
            $("#htmlarea").htmlarea({
                
              toolbar: [
                ["bold", "italic", "underline", "strikethrough"],
                ["justifyleft", "justifycenter", "justifyright"],
                ["indent", "outdent"],
                ["superscript", "subscript"],
                ["orderedList", "unorderedList"],
                ["increasefontsize", "decreasefontsize"],
                ["forecolor", "horizontalrule"],
                ],

              toolbarText: $.extend({}, jHtmlArea.defaultOptions.toolbarText, {
                "bold": "Pogrubienie",
                "italic": "Kursywa",
                "underline": "Podkreślenie",
                "strikethrough": "Skreślenie",
                "justifyleft": "Wyrównanie do lewej",
                "justifycenter": "Wyśrodkowanie",
                "justifyright": "Wyrównanie do prawej",
                "indent": "Wcięcie",
                "outdent": "Cofnij wcięcie",
                "superscript": "Indeks górny",
                "subscript": "Indeks dolny",
                "orderedlist": "Wyliczenie",
                "unorderedlist": "Wypunktowanie",
                "increasefontsize": "Zwiększ czcionke",
                "decreasefontsize": "Zmniejsz czcionke",
                "forecolor": "Kolor czcionki",
                "horizontalrule": "Linie pozioma"
                })
            });
        });
    </script>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Logowanie nie powiodło się. Popraw błędnie wypełnione pola i spróbuj ponownie.")%>
<div class="PaneleOpisRestauracji">
<fieldset>
    <legend>Strona główna - edycja</legend>

        <asp:Panel ID="Panel1" class="PanelOpisRestauracji" runat="server" ScrollBars="Auto">
             <!--   <textarea id="test" cols="50" rows="15"></textarea> -->
                <%: Html.TextAreaFor(m => m.Html, new { id = "htmlarea", style = "width: 268px", rows="19" })%>
                
        </asp:Panel>
        <asp:Panel ID="Panel2" class="PanelTotoPromocje" runat="server" ScrollBars="Auto">
            <asp:Panel ID="Panel3" class="PanelToto" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Foto)%>
            </asp:Panel>
            <asp:Panel ID="Panel4" class="PanelPromocje" runat="server" ScrollBars="Auto">
                <%= Html.DisplayFor(m => m.Promocje)%>
            </asp:Panel>
        </asp:Panel>
        <p>
                    <input type="submit" value="Zaloguj"/>
                </p>
    </fieldset>
</div>
<% } %>
</asp:Content>
