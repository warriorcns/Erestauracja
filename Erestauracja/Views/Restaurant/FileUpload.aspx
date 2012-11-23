<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Restaurant/Restaurant.Master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.MainPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    FileUpload
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">

<h2>FileUpload</h2>



<% using (Html.BeginForm("FileUpload", "Restaurant", FormMethod.Post, new {enctype = "multipart/form-data"})){%>
    <%--<%: Html.HiddenFor(m => m.RestaurantID) %>--%>
    <input type="file" name="fileUp" id="fileUpID"/>
    <input type="submit" />
<%}%>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
