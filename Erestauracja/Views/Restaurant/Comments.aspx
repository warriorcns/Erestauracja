<%@ Page Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<List<Erestauracja.ServiceReference.Comment>>" %>
<%@ Import Namespace="Erestauracja.ServiceReference" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">

    
    <script src="http://code.jquery.com/jquery-1.8.3.js"></script>
    <script src="../../Scripts/jquery-ui.js" type="text/javascript"></script>

    <script src="../../Scripts/jquery.raty.js" type="text/javascript"></script>

<%: Html.ValidationSummary(true, "Błąd.")%>

    <script type="text/javascript">
        $(function () {
            $('#star').raty({
                half: true,
                path: "../../Content/images/",
                click : function(score, evt) {
                    var target = $('#starRating');

                    if (score === null) {
                      target.html('');
                    } else if (score === undefined) {
                      target.empty();
                    } else {
                      target.html(score);
                    }
                  }
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#send")
            .button()
            .click(function (event) {

                var comm = $("#commentText").val();
                var stars = $("#starRating").text();
                var ResID = $("#ResID").val();
                var url = '<%: Url.Action("AddComment", "Restaurant") %>';
                var data = { id: ResID, stars:stars, comm:comm };

                $.post(url, data, function (data) {
                    window.location.href = data.redirectToUrl;
                   // location.reload();
                });
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            
        });
    </script>


<%if (User.Identity.IsAuthenticated) %>
<% { %>
<div>
    <fieldset>
    <legend>Dodaj komentarz</legend>
    <div>Ocena:</div>
    <div id="star"></div>
    <input type="hidden" id="starRating" />
    <input type="hidden" id="ResID" value="<%: ViewData["id"] %>"/>
    <div>Treść komentarza:</div>
    <div><textarea id="commentText"></textarea></div>
    <div><button id="send">Wyślij</button></div>
    </fieldset>
</div>
<% } %>

<% if (Model == null) %>
<% { %>
    <h2>Pobieranie komentarzy nie powiodło się. Przepraszamy za problemy, spróbuj później.</h2>
<% } %>
<% else %>
<% { %>
    <% if (Model.Count == 0) %>
    <% { %>
        <h2>Brak wystawionych komentarzy.</h2>
    <% } %>
    <% else %>
    <% { %>
        <div>
        <% foreach (Comment comm in Model) %>
        <% { %>
            <hr />
            <div>
                <span>Numer komentarza: <%: comm.Id %>.</span>
                <span>Wstawiony przez: <%: comm.UserLogin %></span>
                <span><%: comm.Date %></span>
                <div>
                    <span>Ocena:</span>
                    <span><%: comm.Rating %></span>
                </div>
                <% if (!String.IsNullOrWhiteSpace(comm.CommentText)) %>
                <% { %>
                <div>
                    <div>Treść komentarza:</div>
                    <div><%: comm.CommentText%></div>
                </div>
                <% } %>
            </div>
        <% } %>
        <br />
        </div>
    <% } %>
<% } %>

</asp:Content>

