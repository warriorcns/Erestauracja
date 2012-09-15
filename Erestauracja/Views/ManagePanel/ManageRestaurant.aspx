<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.ServiceReference.Restaurant>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ManageRestaurant
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>ManageRestaurant</h2>

<script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
<script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

<% using (Html.BeginForm()) { %>
    <%: Html.ValidationSummary(true) %>
    <fieldset>
        <legend>Restaurant</legend>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Address) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Address) %>
            <%: Html.ValidationMessageFor(model => model.Address) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.AverageRating) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.AverageRating) %>
            <%: Html.ValidationMessageFor(model => model.AverageRating) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Country) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Country) %>
            <%: Html.ValidationMessageFor(model => model.Country) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.CreationDate) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.CreationDate) %>
            <%: Html.ValidationMessageFor(model => model.CreationDate) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.CurrentDeliveryTime) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.CurrentDeliveryTime) %>
            <%: Html.ValidationMessageFor(model => model.CurrentDeliveryTime) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.DeliveryTime) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.DeliveryTime) %>
            <%: Html.ValidationMessageFor(model => model.DeliveryTime) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.DisplayName) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.DisplayName) %>
            <%: Html.ValidationMessageFor(model => model.DisplayName) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Email) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Email) %>
            <%: Html.ValidationMessageFor(model => model.Email) %>
        </div>

        <%: Html.HiddenFor(model => model.ID) %>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.InputsCount) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.InputsCount) %>
            <%: Html.ValidationMessageFor(model => model.InputsCount) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.IsApproved) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.IsApproved) %>
            <%: Html.ValidationMessageFor(model => model.IsApproved) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.IsLockedOut) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.IsLockedOut) %>
            <%: Html.ValidationMessageFor(model => model.IsLockedOut) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.LastActivityDate) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.LastActivityDate) %>
            <%: Html.ValidationMessageFor(model => model.LastActivityDate) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.LastLockedOutDate) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.LastLockedOutDate) %>
            <%: Html.ValidationMessageFor(model => model.LastLockedOutDate) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.MenagerId) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.MenagerId) %>
            <%: Html.ValidationMessageFor(model => model.MenagerId) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Name) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Name) %>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Nip) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Nip) %>
            <%: Html.ValidationMessageFor(model => model.Nip) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Password) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Password) %>
            <%: Html.ValidationMessageFor(model => model.Password) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Regon) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Regon) %>
            <%: Html.ValidationMessageFor(model => model.Regon) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Telephone) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Telephone) %>
            <%: Html.ValidationMessageFor(model => model.Telephone) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.TownID) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.TownID) %>
            <%: Html.ValidationMessageFor(model => model.TownID) %>
        </div>

        <p>
            <input type="submit" value="Save" />
        </p>
    </fieldset>
<% } %>

<div>
    <%: Html.ActionLink("Back to List", "Index") %>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
