<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../Scripts/jquery.tools.min.js" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        // initialize scrollable
        $(".scrollable").scrollable({ vertical: true, mousewheel: true });
    });
    </script>


 <div id="actions">
    <a class="prev">&laquo; Back</a>
    <a class="next">More pictures &raquo;</a>
  </div>
 
  <!-- root element for scrollable -->
  <div class="scrollable vertical">
 
    <!-- root element for the scrollable elements -->
    <div class="items">
 
      <!-- first element. contains three rows -->
      <div>
 
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
 
      </div>
 
      <!-- second element with another three rows (and so on) -->
      <div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
 
      </div>

      <div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
 
      </div>
 
    </div>
 
  </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
