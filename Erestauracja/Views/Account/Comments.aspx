<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Account/Account.master" Inherits="System.Web.Mvc.ViewPage<List<Erestauracja.ServiceReference.Comment>>" %>
<%@ Import Namespace="Erestauracja.ServiceReference" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AccountPlaceHolder" runat="server">

    <link href="../../Content/CSS/Account.css" rel="stylesheet" type="text/css" />
    <%--<script src="http://code.jquery.com/jquery-1.8.3.js"></script>
    <script src="../../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.raty.js" type="text/javascript"></script>--%>
    <script src="../../Scripts/jquery.raty.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('.stars').raty({
                half: true,
                path: "../../Content/images/",
                readOnly: true,
                score: function () { return $(this).attr('data-rating'); }
            });
        });      
    </script>

    <%: Html.ValidationSummary(true, "Błąd.")%>

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
                        <span class="CommTitle">Numer komentarza: </span>
                        <span><%: comm.Id %>.</span>

                        <span class="CommTitle">Wstawiony dla: </span>
                        <span><%: comm.DisplayName %> (<%: comm.Town %>)</span>
                        <span><%: comm.Date %></span>
                        <div>
                            <span class="CommTitle">Ocena:</span>
                            <span class="stars" data-rating="<%: comm.Rating.ToString("F",System.Globalization.CultureInfo.CreateSpecificCulture("en-CA")) %>"></span>
                        </div>
                        <% if (!String.IsNullOrWhiteSpace(comm.CommentText)) %>
                        <% { %>
                        <div>
                            <div class="CommTitle">Treść komentarza:</div>
                            <div class="CommContent"><%: comm.CommentText%></div>
                        </div>
                        <% } %>
                        <%: Html.ActionLink("Usuń", "DeleteComment", "Account", new { id = comm.Id }, null)%>
                    </div>
                <% } %>
                <br />
        </div>
        <% } %>
    <% } %>
</asp:Content>
