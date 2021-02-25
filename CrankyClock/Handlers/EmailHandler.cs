using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace CrankyClock
{
    class EmailHandler
    {
        /*static void Main(string[] args)
        {

            SendEmailAsync().GetAwaiter();
            Console.Read();
        }*/

        public static void GetData(int id = 1)
        {
            DatabaseElement element = new DatabaseElement();

            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-UG8MHT3; Initial Catalog=CrankyClockDB; Integrated Security=SSPI; Persist Security Info=False;"))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandTimeout = 15;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM [CrankyClockDB].[dbo].[CrankyClock]";

                connection.Open();
                    
                /*SqlCommand command = new SqlCommand();*/
                    
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("{0}, {1}, {2}, {3}",
                        reader[0], reader[1], reader[2], reader[3]));
                        
                    SendEmail(String.Format("{0}",reader[2]));
                }
            }
        }

        public static void SendEmail(String email)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("crankyclock@gmail.com", "Cranky Clock");

            // кому отправляем
            MailAddress to = new MailAddress(email);
            string a = to.Address;

            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);

            // тема письма
            m.Subject = "Тест";

            // текст письма
            m.Body = "Это тестовое <h2>сообщение</h2>";

            // письмо представляет код html
            m.IsBodyHtml = true;

            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 25); // 587

            configurationElement config = new configurationElement();

            // логин и пароль
            smtp.Credentials = new NetworkCredential(config.email, config.password);

            smtp.EnableSsl = true;
            smtp.Send(m);
            Console.WriteLine("[LOG: " + DateTime.Now.ToString("h:mm:ss tt") + "] Email sent to: " + a);
        }
    }
}