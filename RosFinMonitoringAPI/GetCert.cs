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
            //
            // TODO:
            //
            // Спиздить ключ от личного кабинета из КриптоПРО
            //
            // Сейчас "new X509Certificate2()" тупо как заглушка, потом убрать это
            // и присвоить туда сертификат, полученый из КриптоПРО.
            //

            X509Certificate2 cert = new X509Certificate2();

            return cert;
        }
    }
}
