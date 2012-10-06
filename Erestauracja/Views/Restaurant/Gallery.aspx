<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script type="text/javascript" src="/Content/yoxview/yoxview-init.js"></script>

    <script src="../../Scripts/jquery.nailthumb.1.1.js" type="text/javascript"></script>

    <link href="../../Content/CSS/jquery.nailthumb.1.1.css" rel="stylesheet" type="text/css" />

    <style type="text/css" media="screen">
        .square-thumb {
            width: 150px;
            height: 150px;
        }
    </style>
    
    <div class="yoxview">
        <a href="/Content/images/resid1/1.jpg">
            <img class="thumbnail" src="/Content/images/resid1/1.jpg" alt="Zdjecie" />
        </a>
        <a href="/Content/images/resid1/2.jpg">
            <img class="thumbnail" src="/Content/images/resid1/2.jpg" alt="Zdjecie" />
        </a>
    </div>

    <%--<div class="yoxview">
        <div class="thumbnail">
            <img src="/Content/images/resid1/1.jpg" alt="Zdjecie" />
        </div>
        <div class="thumbnail">
            <img src="/Content/images/resid1/2.jpg" alt="Zdjecie" />
        </div>
    </div>--%>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery('.thumbnail').nailthumb({ width: 150, height: 150, method: 'resize', fitDirection: 'center center' });
            $(".yoxview").yoxview();
        });
    </script>

</asp:Content>
