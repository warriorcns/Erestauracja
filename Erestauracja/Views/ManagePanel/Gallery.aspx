<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.MainPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<script type="text/javascript" src="/Content/yoxview/yoxview-init.js"></script>

    <script src="../../Scripts/jquery.nailthumb.1.1.js" type="text/javascript"></script>
    
    <script src="../../Scripts/galleria-1.2.8.min.js" type="text/javascript"></script>
    <link href="../../Content/CSS/jquery.nailthumb.1.1.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/CSS/managepanel.css" rel="stylesheet" type="text/css" />
    
    

    <style type="text/css" media="screen">
        .square-thumb {
            width: 150px;
            height: 150px;
        }
    </style>
    <style>
        #galleria
        {
            width: 700px;
            height: 400px;
            background: #000;
        }
    </style>

    
    <div>Dodaj zdjęcia(akceptowalne formaty: jpg, png):
        <% using (Html.BeginForm("Gallery", "ManagePanel", FormMethod.Post, new { enctype = "multipart/form-data", id=Model.RestaurantID }))
           {%>
        <%--<%: Html.HiddenFor(m => m.RestaurantID) %>--%>
        <%: Html.HiddenFor(m => m.RestaurantID) %>
        <input type="file" id="fileUploadID" name="fileUpload" />
        <input type="submit" />
        <%}%>
    </div>
    <ul class="yoxview">
        <% foreach (Uri link in (IEnumerable)ViewData["imagesuris"])
           { %>
        <li class="galleryElement"><a href="<%:link.AbsoluteUri%>">
            <img class="thumbnail" src="<%:link.AbsoluteUri%>" alt="Zdjecie" style="display: inline;" />
        </a></li>
        <%--<button class="delphoto" onclick="deleteFile('<%:link.AbsoluteUri %>','<%: ViewData["id"] %>')">send</button>--%>
        <%} %>
    </ul>
   
    

    <script type="text/javascript">

    jQuery('.thumbnail').nailthumb({ width: 150, height: 150, method: 'resize', fitDirection: 'center center' });
        jQuery(document).ready(function ($) { 
            jQuery.noConflict();
            $(".yoxview").yoxview({ lang: 'pl', linkToOriginalContext: false,  });
        });
    </script>

    <script type="text/javascript">
        function deleteFile(plik, resid) {
            //alert(link);
            //alert(resid);
            var url = '<%: Url.Action("FileDelete", "ManagePanel") %>';
            var data = { plik: plik, resid: resid };
            $.get(url, data, function (data) {
                // TODO: do something with the response from the controller action
                alert('the value was successfully sent to the server');
                //window.location.href = data.redirectToUrl;
            });


        }
   
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".delphoto")
            .button()
            .click(function (event) {
                event.preventDefault();
            });
        });
            
        
    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
    Galeria
</asp:Content>



    
<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.9.1/themes/base/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script src="http://code.jquery.com/ui/1.9.1/jquery-ui.js"></script>
</asp:Content>