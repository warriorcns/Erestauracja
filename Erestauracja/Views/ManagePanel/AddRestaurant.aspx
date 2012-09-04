<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.RegisterRestaurantModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    AddRestaurant
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Wypełnij poniższy formularz oraz kliknij 'Zapisz', aby dodać nową restaurację.</h2>
    
    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>"
        type="text/javascript"></script>

    <form id="Form1" runat="server">
    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Rejestracja restauracji nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset>
                <legend>Dane rejestracji</legend>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Name)%>
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Name)%>
                        <%: Html.ValidationMessageFor(m => m.Name)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.DisplayName)%>
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.DisplayName)%>
                        <%: Html.ValidationMessageFor(m => m.DisplayName)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Address)%>
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Address)%>
                        <%: Html.ValidationMessageFor(m => m.Address)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.TownId)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.TownId)%>
                        <%: Html.ValidationMessageFor(m => m.TownId)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.CountryId)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.CountryId)%>
                        <%: Html.ValidationMessageFor(m => m.CountryId)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Telephone)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Telephone)%>
                        <%: Html.ValidationMessageFor(m => m.Telephone)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Email)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Email)%>
                        <%: Html.ValidationMessageFor(m => m.Email)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Nip)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Nip)%>
                        <%: Html.ValidationMessageFor(m => m.Nip)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Regon)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Regon)%>
                        <%: Html.ValidationMessageFor(m => m.Regon)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.Password)%> (Minimum <%: Membership.MinRequiredPasswordLength %> znaków.)
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.Password)%>
                        <%: Html.ValidationMessageFor(m => m.Password)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.ConfirmPassword)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.ConfirmPassword)%>
                        <%: Html.ValidationMessageFor(m => m.ConfirmPassword)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-label">
                        <%: Html.LabelFor(m => m.DeliveryTime)%> 
                    </li>
                    <li class="editor-label">
                        <%: Html.TextBoxFor(m => m.DeliveryTime)%>
                        <%: Html.ValidationMessageFor(m => m.DeliveryTime)%>
                    </li>
                </ul>

                <p>
                    <input type="submit" value="Zapisz"/>
                </p>
            </fieldset>
        </div>
    <% } %>
    </form>
     

</asp:Content>
