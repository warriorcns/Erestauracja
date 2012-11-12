<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
<script type="text/javascript" src="http://code.jquery.com/ui/1.9.1/jquery-ui.js"></script>

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
 
        <!-- first row -->
        <div class="item">
 
          <!-- image -->
          <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" />
        </div>
 
        <!-- 2:nd and 3:rd rows -->
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
 
      </div>
 
      <!-- second element with another three rows (and so on) -->
      <div>
        <div class="item">
 
          <!-- image -->
          <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" />
        </div>
 
        <!-- 2:nd and 3:rd rows -->
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
      </div>

      <div>
        <div class="item">
 
          <!-- image -->
          <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" />
        </div>
 
        <!-- 2:nd and 3:rd rows -->
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
        <div class="item"> <img src="http://pierre.chachatelier.fr/programmation/images/mozodojo-original-image.jpg" /> </div>
      </div>
 
    </div>
 
  </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
