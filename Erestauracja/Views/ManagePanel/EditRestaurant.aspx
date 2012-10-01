<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.EditRestaurantModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    EditRestaurant
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Edycja danych restauracji nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset>
                <legend>Dane restauracji</legend>
                <p>
                    Wprowadz nowe dane, a następnie kliknij 'Zapisz', aby edytować dane restauracji.
                </p>

                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Name)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Name)%>
                       </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.Name)%>
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
                         <%: Html.ValidationMessageFor(m => m.DisplayName)%>
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
                        <%: Html.LabelFor(m => m.Town)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.Town)%>
                       </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.Town)%>
                    </li>
                </ul>
                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.PostalCode)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.PostalCode)%>
                       </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.PostalCode)%>
                    </li>
                </ul>
                <ul class="rej">
                    <li class="display-labelR">
                        <%: Html.LabelFor(m => m.Country)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.DropDownListFor(m => m.Country, (IEnumerable<SelectListItem>)ViewData["countryList"])%>
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
                        <%: Html.LabelFor(m => m.DeliveryTime)%>
                    </li>
                    <li class="editor-labelR">
                        <%: Html.TextBoxFor(m => m.DeliveryTime)%>
                       </li>
                    <li class="validation-labelR">
                         <%: Html.ValidationMessageFor(m => m.DeliveryTime)%>
                    </li>
                </ul>
                <p>
                    <input type="submit" value="Zatwierdz zmiany"/>
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>

