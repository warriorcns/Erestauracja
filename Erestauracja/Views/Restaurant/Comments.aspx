<%@ Page Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.master" Inherits="System.Web.Mvc.ViewPage<List<Erestauracja.ServiceReference.Comment>>" %>
<%@ Import Namespace="Erestauracja.ServiceReference" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">

    <script src="../../Scripts/jquery.raty.js" type="text/javascript"></script>

<%: Html.ValidationSummary(true, "Błąd.")%>

<script type="text/javascript">
    $(function () {
        $('#star').raty({
            half: true,
            path: "../../Content/images/"
        });
    });
</script>

<%if (User.Identity.IsAuthenticated) %>
<% { %>
<div>
    <fieldset>
    <legend>Dodaj komentarz</legend>
    <div>Ocena:</div>
    <div id="star"></div>
    <div>Treść komentarza:</div>
    <div><textarea></textarea></div>
    <div><button>Przycisk :D</button></div>
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
                <div>
                    <div>Treść komentarza:</div>
                    <div><%: comm.CommentText %></div>
                </div>
            </div>
        <% } %>
        <br />
        </div>
    <% } %>
<% } %>

</asp:Content>

