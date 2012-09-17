<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.RegisterRestaurantModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    AddRestaurant
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Wypełnij poniższy formularz oraz kliknij 'Zapisz', aby dodać nową restaurację.</h2>
    
    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>"
        type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#DeliveryTime").mask("99:99:99");
        });
    </script>

    <form id="Form1" runat="server">
    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Rejestracja restauracji nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset>
                <legend>Dane rejestracji</legend>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Name)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Name)%>
                        
                    </li>
                    <li class="validation-labelR">
                         
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.DisplayName)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.DisplayName)%>
                        
                    </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.Name)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Address)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Address)%>
                    </li>
                    <li class="validation-labelR">
                          <%: Html.ValidationMessageFor(m => m.Address)%>
                    </li>

                    
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.TownId)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.TownId)%>
                        
                    </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.TownId)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Country)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Country)%>
                        
                    </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.Country)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Telephone)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Telephone)%>
                        
                    </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.Telephone)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Email)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Email)%>
                        
                    </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.Email)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Nip)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Nip)%>
                        
                    </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.Nip)%>
                    </li>

                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Regon)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Regon)%>
                        
                    </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.Regon)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Password)%> (Minimum <%: Membership.MinRequiredPasswordLength %> znaków.)
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Password)%>
                        
                    </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.Password)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.ConfirmPassword)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.ConfirmPassword)%>
                        
                    </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.ConfirmPassword)%>
                    </li>
                </ul>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.DeliveryTime)%> 
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.DeliveryTime, new { id = "DeliveryTime" })%>
                    </li>
                    <li class="validation-labelR">
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
