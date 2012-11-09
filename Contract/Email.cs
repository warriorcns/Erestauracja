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

        public Email()
        {
            //zczytać konfiguracje z config
        }

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
    }
}
