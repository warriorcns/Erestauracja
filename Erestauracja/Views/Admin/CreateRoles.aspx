﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Admin/Index.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.UserRoleModel>" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="AdminPlaceHolder" runat="server">

    <% using (Html.BeginForm())
       { %>
    <%: Html.ValidationSummary(true, "Rejestracja konta nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.")%>
    <div class="divfields">
        <fieldset>
            
            <ul class="roles">
                <li>
                    <p class="rolesHeader">
                        Utwórz Nową Rolę</p>
                </li>
                <li>
                    <p>
                        Nowa nazwa roli:</p>
                </li>
                <li>
                    <%: Html.TextBoxFor(m => m.RoleName, new { id = "RoleName" })%></li>
                <li>
                    <input type="submit" value="Dodaj rolę" /></li>
            </ul>
        </fieldset>
    </div>
    <%} %>
    <div class="divfields">
    <% using (Html.BeginForm())
       { %>
        <fieldset>
            <ul class="roles">
                <li>
                    <p class="rolesHeader">
                        Nazwa roli</p>
                </li>
                <%foreach (string id in (IEnumerable<string>)ViewData["Roles"])
                  { %>
                <li>
                    <%: Html.Label(id)%>
                    <%: Html.ActionLink("usuń rolę", "deleteRole", "Admin", new { role = id }, null)%>
                    </li>
                <% }%>
            </ul>
        </fieldset>
        <% } %>
    </div>
</asp:Content>
