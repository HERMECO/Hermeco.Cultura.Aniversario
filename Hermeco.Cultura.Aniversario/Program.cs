using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Hermeco.Cultura.Aniversario
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            wsEmpleados.EmpleadosSAPServiceClient client = new wsEmpleados.EmpleadosSAPServiceClient();
            Task<wsEmpleados.GetEmployeeIntranetResponse> response = client.GetAnniversariesAsync();

            string fromEmail = Configuration["FromEmail"].ToString();
            string smtpServer = Configuration["SMTPServer"].ToString();
            string subject = string.Empty;
          
            foreach (wsEmpleados.EmployeeIntranet empleado in response.Result.EmployeeIntranet)
            {
                Console.WriteLine(String.Format("Aniversario {0}", empleado.FirstName));
                try
                {
                    subject = string.Format("¡{0}, Feliz Aniversario!", empleado.FirstName.Trim().Split(' ')[0]);
                    if (!string.IsNullOrEmpty(empleado.EMail))
                    {
                        Utility.sendEmail(smtpServer, subject, "Email\\Aniversario.html", "", fromEmail, empleado.EMail, "", true, "", null);                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            
        }
    }
}