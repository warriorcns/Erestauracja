<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.MainPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <div>
        <div style="color:Red"><%: ViewData["alert"] %></div>
        Zdjęcie główne:
        <% using (Html.BeginForm("UploadLogo", "ManagePanel", FormMethod.Post, new { enctype = "multipart/form-data", id = (int)ViewData["id"] }))
           {%>
        <input type="file" id="logofile" name="logofile" />
        <input type="submit" />
        <%}%>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="server">
    Zdjęcie główne
</asp:Content>
