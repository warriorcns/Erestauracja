<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Admin/Index.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminPlaceHolder" runat="server">
 <div class="divfields">
    <% using (Html.BeginForm())
       { %>
        <fieldset>
            <ul class="roles">
                <li>
                    <p class="rolesHeader">
                        Nazwa użytkownika</p>
                </li>
                <%foreach (CustomMembershipUser id in (MembershipUserCollection)ViewData["users"])
                  { %>
                <li class="mainRole">
                    <div class="roleName">
                        <%: Html.Label(id.ToString())%></div>
                    <div class="deleteAction">
                        <%: Html.ActionLink("usuń", "deleteUser", "Admin", new { user = id }, null)%></div>
                </li>
                  <% }%>
            </ul>
        </fieldset>
        <% } %>
    </div>

</asp:Content>
