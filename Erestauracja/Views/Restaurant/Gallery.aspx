<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script type="text/javascript" src="/Content/yoxview/yoxview-init.js"></script>

    <script src="../../Scripts/jquery.nailthumb.1.1.js" type="text/javascript"></script>
    
    <script src="../../Scripts/galleria-1.2.8.min.js" type="text/javascript"></script>
    <link href="../../Content/CSS/jquery.nailthumb.1.1.css" rel="stylesheet" type="text/css" />

    <style type="text/css" media="screen">
        .square-thumb {
            width: 150px;
            height: 150px;
            padding: 20px;
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
    

    <%--<div id="galleria">
        <% foreach (Uri link in (IEnumerable)ViewData["imagesuris"])
           { %>
                <img src="<%:link.AbsoluteUri%>" alt="obrazek">
        <% } %>
    </div>
--%>
    <%--<div class="yoxview">
        <a href="/Content/images/resid1/1.jpg">
            <img class="thumbnail" src="/Content/images/resid1/1.jpg" alt="Zdjecie" style="display: inline;" />
        </a><a href="/Content/images/resid1/2.jpg">
            <img class="thumbnail" src="/Content/images/resid1/2.jpg" alt="Zdjecie" style="display: inline;" />
        </a>
    </div>--%>
    <div style="color: Red"><%: ViewData["alert"] %></div>
    <span class="yoxview" >
        <% foreach (Uri link in (IEnumerable)ViewData["imagesuris"])
           { %>
       <a href="<%:link.AbsoluteUri%>">
           <span><img class="thumbnail" src="<%:link.AbsoluteUri%>" alt="Zdjecie" style="display: inline;" /></span>
       </a>
        <%} %>
    </span>
    

    <script type="text/javascript">

    jQuery('.thumbnail').nailthumb({ width: 150, height: 150, method: 'resize', fitDirection: 'center center' });
        jQuery(document).ready(function ($) { 
            jQuery.noConflict();
            $(".yoxview").yoxview({ lang: 'pl', linkToOriginalContext: false  });
        });
    </script>
    <%--<script type="text/javascript">
        Galleria.loadTheme('../../Scripts/themes/classic/galleria.classic.min.js');
        Galleria.run('#galleria');
    </script>--%>
</asp:Content>
