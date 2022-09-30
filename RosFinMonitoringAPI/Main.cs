// Если определено, вызывает тестовые методы API
// Если не определено, вызывает методы API в штатном режиме
#define testconturAPI

using System.Text;

namespace RosFinMonitoringAPI
{
    internal partial class Program
    {
        /// <summary>
        /// Адрес API сервера.
        /// </summary>
#if testconturAPI
        static readonly string APIaddress = "https://portal.fedsfm.ru:8081/Services/fedsfm-service/test-contur";
#else
        static readonly string APIaddress = "https://portal.fedsfm.ru:8081/Services/fedsfm-service";
#endif

        /// <summary>
        /// Подгружает логин, пароль и сертификат из config файла.
        /// </summary>
        static dynamic? JSONcfg;

        static async Task MainAsync()
        {
            // Создает защищенное Https-соединение с использованием сертификата пользователя
            HttpClient client = CreateHttpClient(GetCert());

            await Authenticate(client);     // Авторизация в API

            Thread.Sleep(10000);
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                // Получает данные из config.json файла, если файл не существует, создает его
                CheckConfig();
                // Запускаем асинхронный метод Main
                MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + (ex.InnerException != null ? "\nInner Exception: " + ex.InnerException?.Message : String.Empty));
                Console.ReadKey();
            }
        }
    }
}