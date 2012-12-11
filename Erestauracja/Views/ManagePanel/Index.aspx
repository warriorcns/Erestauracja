<%@ Page Title="" Language="C#" MasterPageFile="~/Views/ManagePanel/ManagePanel.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2> Witamy w panelu menadżera.</h2>
<div>
    <div>
    <p>Zakładka "Twoje restauracje" pozwala na dodawanie nowych restauracji oraz zarządzanie nimi.</p>
    <p>Po dodaniu restauracji masz możliwość zarządzania zawartością strony restauracji, do której dostęp mają kliencji.</p>
    <p>Intuicyjne w użyciu edytory tekstu pozwolą Ci sformatować treść strony, a możliwość dodawania zdjęć do galerii
    pozwoli na raprezentowanie wystoju Twojego lokalu.</p>
    <p>Możliwość dodawania kategorii oraz produktów powala odwzorować niemal każde menu.</p>
    <p>Sekcja "Komentarze" zapewnia szybki podgląd komentarzy i ocen jakie wystawili klienci twojwj restauracji.</p>
    <p>Jeśli treść komentarza jest obraźliwa lub w inny sopsób nie odpowiednia wybierz opcje "Zgłoś nadużycie" a sprawą zajmą się 
    administratorzy strony.</p>
    </div>
    <br />
    <div>
    <p>W zakładce "Pracownicy" możesz dodawać nowych pracowników jednocześnie dając im dostęp do Panelu obsługi restauracji.</p>
    </div>
</div>

</asp:Content>

