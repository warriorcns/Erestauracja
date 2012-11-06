<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Personnel
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <meta charset="utf-8">

    <script type="text/javascript">
        $(function () {
            $(".accordion").accordion({
                active: false,
                autoHeight: false,
                event: "click",
                collapsible: true
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $(".accordion2").accordion({
                active: false,
                autoHeight: false,
                event: "click",
                collapsible: true
            });
        });
    </script>
    
    <div class="accordion">
        <% foreach (Erestauracja.ServiceReference.Presonnel item in (List<Erestauracja.ServiceReference.Presonnel>)ViewData["pracownicy"]) %>
        <% { %>
                <h3><a href="#"><%: item.RestaurantName %> (<%: item.RestaurantAddress %>, <%: item.RestaurantTown%>)</a></h3>
                <div>
                    <h2>Kliknij <%: Html.ActionLink("tutaj", "AddEmployee", "ManagePanel", new { id = item.RestaurantId }, null)%>, aby dodać nowego pracownika. </h2>
                    <div class="accordion2">
                        <% foreach(Erestauracja.ServiceReference.User user in item.Employees) %>
                        <% { %>
                            <h3><a href="#"><%: user.Name %> <%: user.Surname %> </a></h3>
                            <div>
                                <div class="editor-labelE">
                                    Login:  <%: user.Login %>
                                </div>
                                <div class="editor-labelE">
                                    Adres email:  <%: user.Email%>
                                </div>
                                <div class="editor-labelE">
                                    Imię:  <%: user.Name%>
                                </div>
                                <div class="editor-labelE">
                                    Nazwisko:  <%: user.Surname%>
                                </div>
                                <div class="editor-labelE">
                                    Adres:  <%: user.Address%>
                                </div>
                                <div class="editor-labelE">
                                    Miasto:  <%: user.Town%>
                                </div>
                                <div class="editor-labelE">
                                    Kod pocztowy:  <%: user.PostalCode%>
                                </div>
                                <div class="editor-labelE">
                                    Kraj:  <%: user.Country%>
                                </div>
                                <div class="editor-labelE">
                                    Data urodzenia:  <%: user.Birthdate.ToShortDateString()%>
                                </div>
                                <div class="editor-labelE">
                                    Płeć:  <%: user.Sex%>
                                </div>
                                <div class="editor-labelE">
                                    Numer telefonu:  <%: user.Telephone%>
                                </div>
                            </div>
                        <% } %>
                    </div>
                </div>
        <% } %>
    </div>

</asp:Content>
