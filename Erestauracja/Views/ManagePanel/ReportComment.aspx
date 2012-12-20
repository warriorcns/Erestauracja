<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePageContent.master" Inherits="System.Web.Mvc.ViewPage<Erestauracja.Models.ReportCommentModel>" %>

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
    <script type="text/javascript">
        $(function ($) {
            $("#fcs").focus();
        });
    </script>
<% if (Model == null) %>
<% { %>
    <h2>Pobieranie komentarza nie powiodło się. Przepraszamy za problemy, spróbuj później.</h2>
<% } %>
<% else %>
<% { %>
        <div>
            <div>
                <span>Numer komentarza: <%: Model.Id %>.</span>
                <span>Wstawiony przez: <%: Model.UserLogin%></span>
                <span><%: Model.Date%></span>
                <div>
                    <span>Ocena:</span>
                    <span class="stars" data-rating="<%: Model.Rating.ToString("F",System.Globalization.CultureInfo.CreateSpecificCulture("en-CA")) %>"></span>
                </div>
                <% if (!String.IsNullOrWhiteSpace(Model.Comment)) %>
                <% { %>
                <div>
                    <div>Treść komentarza:</div>
                    <div><%: Model.Comment%></div>
                </div>
                <% } %>
            </div>
        <hr />
        <br />
        </div>
<% } %>

<div>
<% using (Html.BeginForm())
               { %>
            <%: Html.ValidationSummary(true, "Zgłaszanie nadużycia nie powiodła się. Popraw błędnie wypełnione pola i spróbuj ponownie.") %>
            <div>
                <fieldset>
                    <legend>Zgłaszanie nadużycia w treści komentarza</legend>
                    <%: Html.HiddenFor(m => m.RestaurantId) %>
                    <%: Html.HiddenFor(m => m.Address) %>
                    <%: Html.HiddenFor(m => m.Comment) %>
                    <%: Html.HiddenFor(m => m.Date) %>
                    <%: Html.HiddenFor(m => m.DisplayName) %>
                    <%: Html.HiddenFor(m => m.Id) %>
                    <%: Html.HiddenFor(m => m.Postal) %>
                    <%: Html.HiddenFor(m => m.Rating) %>
                    <%: Html.HiddenFor(m => m.Town) %>
                    <%: Html.HiddenFor(m => m.UserLogin) %>
                   
                    <div class="editor-label">
                        <%: Html.LabelFor(m => m.Report) %>
                        <%: Html.TextAreaFor(m => m.Report, new { @id = "fcs" })%>
                        <%: Html.ValidationMessageFor(m => m.Report)%>
                    </div>

                    <p>
                        <input type="submit" value="Wyślij" />
                    </p>
                </fieldset>
            </div>
            <% } %>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="server">
</asp:Content>
