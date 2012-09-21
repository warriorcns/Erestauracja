<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Account/Account.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.UserDataModel>" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="AccountPlaceHolder" runat="server">


    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#Birthdate").datepicker();
        });
    </script>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#Birthdate").mask("9999/99/99");
        });
    </script>
    <script type="text/javascript">
        $(function () {
            //window.location.href = window.location.href.split('?', 1)[0];
        });
    </script>
    

<% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Edycja danych nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
        <div>
            <fieldset>
                <legend>Edycja danych</legend>
                <p>
                    Wprowadz nowe dane, a następnie kliknij 'Zapisz'.
                </p>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Name) %>
                    <%: Html.TextBoxFor(m => m.Name)%>

                    <%: Html.ValidationMessageFor(m => m.Name)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Surname) %>
                    <%: Html.TextBoxFor(m => m.Surname)%>
                    <%: Html.ValidationMessageFor(m => m.Surname)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Address) %>
                    <%: Html.TextBoxFor(m => m.Address)%>
                    <%: Html.ValidationMessageFor(m => m.Address)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.TownID) %>
                    <%: Html.TextBoxFor(m => m.TownID)%>
                    <%: Html.ValidationMessageFor(m => m.TownID)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Country) %>
                    <%--<%: Html.TextBoxFor(m => m.Country)%>--%>
                    <%--Tu pobrac liste panstw z Bazy.--%>
                    <%: Html.DropDownListFor(m => m.Country, (IEnumerable<SelectListItem>)ViewData["countryList"])%>
                    
                    <%: Html.ValidationMessageFor(m => m.Country)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Birthdate)%>
                    <%: Html.TextBoxFor(m => m.Birthdate, new { id = "Birthdate" })%>
                    <%: Html.ValidationMessageFor(m => m.Birthdate)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Sex) %>
                    <%--<%: Html.TextBoxFor(m => m.Sex)%>--%>
                    <%=Html.DropDownListFor(m=>m.Sex, (IEnumerable<SelectListItem>)ViewData["sex"])%>

                    <%: Html.ValidationMessageFor(m => m.Sex)%>
                </div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Telephone)%>
                    <%: Html.TextBoxFor(m => m.Telephone)%>
                    <%: Html.ValidationMessageFor(m => m.Telephone)%>
                </div>

                <p>
                    <input type="submit" value="Zapisz"/>
                </p>
            </fieldset>
        </div>
    <% } %>
  
</asp:Content>

