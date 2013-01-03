using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Diagnostics;

namespace Contract
{
    class Email
    {
        private string eventSource = "EresWindowsService";
        private string eventLog = "Erestauracja";

        private SmtpClient smtp = new SmtpClient("smtp.gmail.com");
        private string eresEmail = "erestauracja@gmail.com";
        private string eresError = "erestauracja.bledy@gmail.com";
        int port = 587;
        System.Net.NetworkCredential credential = new System.Net.NetworkCredential("erestauracja", "Erestauracja123");

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        public Email()
        {
            //zczytać konfiguracje z config
        }

        /// <summary>
        /// Wysyła nowe hasło na podany email
        /// </summary>
        /// <param name="email">Adres email użytkownika</param>
        /// <param name="password">Nowe hasło</param>
        /// <returns>True jeśli metoda wykonała się poprawnie</returns>
        public bool SendPassword(string email, string password)
        {
            SmtpClient klient = smtp;
            MailMessage wiadomosc = new MailMessage();
            try
            {
                wiadomosc.From = new MailAddress(eresEmail);
                wiadomosc.To.Add(email);
                wiadomosc.Subject = "Erestauracja - restet hasła.";
                wiadomosc.Body = "Nowe hasło: " + password;

                klient.Port = port;
                klient.Credentials = credential;
                klient.EnableSsl = true;
                klient.Send(wiadomosc);

                return true;
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string info = "Błąd podczas wysyłania wiadomości email z nowym hasłem";
                info += "Action: " + "SendPassword" + "\n\n";
                info += "Exception: " + ex.ToString();

                return false;
            }
        }

        /// <summary>
        /// Wysyła zgłoszenie błędu
        /// </summary>
        /// <param name="email">Informacja o użytkowniku, który wysłał zgłoszenie (email lub login)</param>
        /// <param name="text">Treść zgłoszenia</param>
        /// <returns>True jeśli metoda wykonała się poprawnie</returns>
        public bool SendError(string email, string text)
        {
            SmtpClient klient = smtp;
            MailMessage wiadomosc = new MailMessage();
            try
            {
                wiadomosc.From = new MailAddress(eresEmail);
                wiadomosc.To.Add(eresError);
                wiadomosc.Subject = "Error - " + email;
                wiadomosc.Body = "Treść zgłoszenia: " + System.Environment.NewLine + text + System.Environment.NewLine + "Wysłał: " + email;

                klient.Port = port;
                klient.Credentials = credential;
                klient.EnableSsl = true;
                klient.Send(wiadomosc);

               return true;
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string info = "Błąd podczas wysyłania wiadomości email z błędem";
                info += "Action: " + "SendError" + "\n\n";
                info += "Exception: " + ex.ToString();
            
                return false;
            }
        }

        /// <summary>
        /// Wysyła zgłoszenie nadużycia w komentarzu
        /// </summary>
        /// <param name="id">Id komentarza</param>
        /// <param name="resId">Id restauracji</param>
        /// <param name="userLogin">Login użytkownika</param>
        /// <param name="comment">Treść komentarza</param>
        /// <param name="report">Treść zgłoszenia</param>
        /// <param name="login">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie</returns>
        public bool SendReportComment(int id, int resId, string userLogin, string comment, string report, string login)
        {
            SmtpClient klient = smtp;
            MailMessage wiadomosc = new MailMessage();
            try
            {
                wiadomosc.From = new MailAddress(eresEmail);
                wiadomosc.To.Add(eresError);
                wiadomosc.Subject = "ReportComment - " + login + " (" + resId + ")";
                wiadomosc.Body = "Treść zgłoszenia: " + System.Environment.NewLine + " " + report + System.Environment.NewLine + "Komentarz: " + id +
                    System.Environment.NewLine + "  Użytkownik: " + userLogin + System.Environment.NewLine + "  Treść komentarza: " + comment;

                klient.Port = port;
                klient.Credentials = credential;
                klient.EnableSsl = true;
                klient.Send(wiadomosc);

                return true;
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string info = "Błąd podczas wysyłania wiadomości email z zgłoszeniem nadużycia";
                info += "Action: " + "SendReportComment" + "\n\n";
                info += "Exception: " + ex.ToString();

                return false;
            }
        }

        /// <summary>
        /// Wysyła zamówienie na adres email restauracji
        /// </summary>
        /// <param name="email">Adres email</param>
        /// <param name="order">Zamówienie typu Order</param>
        /// <returns>True jeśli metoda wykonała się poprawnie</returns>
        public bool SendOrder(string email, Order order)
        {
            SmtpClient klient = smtp;
            MailMessage wiadomosc = new MailMessage();
            try
            {
                wiadomosc.From = new MailAddress(eresEmail);
                wiadomosc.To.Add(email);
                wiadomosc.Subject = "Zamówienie - " + order.OrderId + " - Data: " + order.OrderDate.ToShortDateString() + " " + order.OrderDate.ToShortTimeString();
                string zamowienie = "Zamówienie od " + System.Environment.NewLine;
                zamowienie += order.UserName + " " + order.UserSurname + System.Environment.NewLine;
                zamowienie += order.UserAdderss + " " + order.UserTown + " " + order.UserPostal + System.Environment.NewLine;
                zamowienie += order.UserTelephone + System.Environment.NewLine + System.Environment.NewLine;
                zamowienie += "Produkty:" + System.Environment.NewLine + System.Environment.NewLine;
                foreach(OrderedProduct item in order.Products)
                {
                    zamowienie += item.ProductName + " x" + item.Count + System.Environment.NewLine;
                    if (!String.IsNullOrWhiteSpace(item.PriceOption))
                        zamowienie += item.PriceOption + System.Environment.NewLine;
                    if (!String.IsNullOrWhiteSpace(item.NonPriceOption))
                        zamowienie += item.NonPriceOption + System.Environment.NewLine;
                    if (!String.IsNullOrWhiteSpace(item.NonPriceOption2))
                        zamowienie += item.NonPriceOption2 + System.Environment.NewLine;
                    if (!String.IsNullOrWhiteSpace(item.Comment))
                        zamowienie += "Komentarz do produktu: " + item.Comment + System.Environment.NewLine;
                    zamowienie += System.Environment.NewLine;
                }
                zamowienie += System.Environment.NewLine + "Komentarz do zamówienia:" + order.Comment + System.Environment.NewLine;
                zamowienie += System.Environment.NewLine + "Razem:" + order.Price + System.Environment.NewLine;
                zamowienie += "Płatność: ";
                if(order.Payment=="cash")
                    zamowienie += "Gotówką przy odbiorze";
                else if(order.Payment.Contains("PayPal"))
                    zamowienie += "PayPal";
                else
                    zamowienie += "Inna";

                wiadomosc.Body = zamowienie;
                klient.Port = port;
                klient.Credentials = credential;
                klient.EnableSsl = true;
                klient.Send(wiadomosc);

                return true;
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string info = "Błąd podczas wysyłania wiadomości email z zamówieniem ";
                info += "Action: " + "SendReportComment" + "\n\n";
                info += "Exception: " + ex.ToString();

                return false;
            }
        }
    }
}
