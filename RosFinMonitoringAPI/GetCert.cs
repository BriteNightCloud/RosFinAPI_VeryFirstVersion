using System.Security.Cryptography.X509Certificates;

namespace RosFinMonitoringAPI
{
    partial class Program
    {
        /// <summary>
        /// Получает сертификат для доступа к API из КриптоПРО.
        /// </summary>
        /// <returns></returns>
        static X509Certificate2 GetCert()
        {
            // Считываем сертификат из файла
            byte[] rawData = File.ReadAllBytes($"{Directory.GetCurrentDirectory()}\\{JSONcfg?.certName?.Value}");
            // Создаем объект сертификата
            X509Certificate2 cert = new X509Certificate2(rawData);

            // Возвращаем объект сертификата
            return cert;
        }
    }
}
