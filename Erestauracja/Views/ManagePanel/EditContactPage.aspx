<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.ContactPageModel>" %>

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
        <%: Html.ValidationSummary(true, "Edycja danych nie powiodło się. Popraw błędnie wypełnione pola i spróbuj ponownie.")%>
<fieldset>
    <legend>Kontakt - edycja</legend>
<asp:Panel ID="Panel5" class="PanelDowoz" runat="server" ScrollBars="Auto">
       
                <%: Html.TextAreaFor(m => m.Contact, new { id = "htmlarea", style = "width: 800px", rows = "19" })%>

    </asp:Panel>
        <%: Html.HiddenFor(m => m.RestaurantID) %>
    </fieldset>
<p>
   <input type="submit" value="Zapisz"/>
</p>
<% } %>

</asp:Content>
