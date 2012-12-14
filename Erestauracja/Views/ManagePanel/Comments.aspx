﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<List<Erestauracja.ServiceReference.Comment>>" %>
<%@ Import Namespace="Erestauracja.ServiceReference" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
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
                    <span class="stars" data-rating="<%: comm.Rating.ToString("F",System.Globalization.CultureInfo.CreateSpecificCulture("en-CA")) %>"></span>
                </div>
                <% if (!String.IsNullOrWhiteSpace(comm.CommentText)) %>
                <% { %>
                <div>
                    <div>Treść komentarza:</div>
                    <div><%: comm.CommentText%></div>
                </div>
                <% } %>
                <%: Html.ActionLink("Zgłoś nadużycie", "ReportComment", "ManagePanel", new { id = (int)ViewData["id"], comm = comm.Id }, null)%>
            </div>
        <% } %>
        <br />
        </div>
    <% } %>
<% } %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="server">
</asp:Content>
