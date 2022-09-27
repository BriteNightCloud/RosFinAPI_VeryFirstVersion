using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;

namespace RosFinMonitoringAPI
{
    partial class Program
    {
        /// <summary>
        /// Создает HTTP клиент, через который будут отправляться запросы к API.
        /// </summary>
        /// <param name="cert">Сертификат для доступа к API (такой же как для доступа к Личному Кабинету).</param>
        /// <returns></returns>
        static HttpClient CreateHttpClient(X509Certificate2 cert)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);   // Добавляем наш сертификат
            return new HttpClient(handler);         // Возвращаем клиент
        }

        /// <summary>
        /// Запоминает токен авторизации для текущей сессии.
        /// </summary>
        /// <param name="client">Http клиент с добавленым сертификатом для доступа к API.</param>
        /// <param name="token">Токен авторизации текущей сессии.</param>
        static void SetHttpAuthHeader(HttpClient client, string token)
        {
            if (string.IsNullOrEmpty(token)) 
                throw new Exception("Ошибка авторизации. Токен сессии пуст или равен NULL.");     // Проверка токена на NULL

            client.DefaultRequestHeaders.Authorization =                                          // Устанавливает токен текущей сессии.
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}