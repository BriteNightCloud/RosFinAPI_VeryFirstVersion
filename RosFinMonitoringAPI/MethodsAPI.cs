using Newtonsoft.Json;

namespace RosFinMonitoringAPI
{
    partial class Program
    {
        /// <summary>
        /// Производит авторизацию в API, необходимо для отправки дальнейших запросов к API.
        /// </summary>
        /// <param name="client">Http клиент с добавленым сертификатом для доступа к API.</param>
        /// <returns></returns>
        static async Task Authenticate(HttpClient client)
        {
            // Указываем данные, отправляемые в запросе.
            var values = new Dictionary<string, string>
            {
                { "userName", JSONcfg?.userName.Value },
                { "password", JSONcfg?.password.Value }
            };

            var content = new FormUrlEncodedContent(values);

            // Отправляем запрос на указанный адрес сервиса и получаем ответ.
            var response = await client.PostAsync($"{APIaddress}/authenticate", content);

            var responseString = await response.Content.ReadAsStringAsync();

            // Устанавливает текущий токен сессии.
            // Проще говоря запоминает авторизацию.
            dynamic? JSONobject = JsonConvert.DeserializeObject(responseString);
            SetHttpAuthHeader(client, JSONobject?.value.accessToken);

            Console.WriteLine(responseString);
        }

        /// <summary>
        /// Перечисление перечней:
        /// TE2 - в отношении которых имеются сведения об их причастности к экстремистской деятельности или терроризму.
        /// MVK - в отношении которых действует решение Комиссии о замораживании (блокировании) принадлежащих им денежных средств или иного имущества.
        /// OMU - в отношении которых имеются сведения об их причастности к распространению оружия массового уничтожения.
        /// </summary>
        enum CatalogType
        {
            TE2,
            MVK,
            OMU
        }

        /// <summary>
        /// Возвращает API адрес метода для запроса сведений об актуальных перечнях.
        /// </summary>
        /// <param name="catalogT">Какой именно перечень необходимо получить.</param>
        /// <returns>Адрес сервера API для конкретного перечня.</returns>
        static string GetCatalogAPI(CatalogType catalogT)
        {
            if (catalogT == CatalogType.TE2) return $"{APIaddress}/suspect-catalogs/current-te2-catalog";
            else if (catalogT == CatalogType.MVK) return $"{APIaddress}/suspect-catalogs/current-mvk-catalog";
            else if (catalogT == CatalogType.OMU) return $"{APIaddress}/suspect-catalogs/current-omu-catalog";
            else throw new Exception("Название метода (получение сведений) для указанного перечня не определено!");
        }

        /// <summary>
        /// Возвращает API адрес метода для скачивания актуальных перечней.
        /// </summary>
        /// <param name="catalogT">Какой именно перечень необходимо получить.</param>
        /// <returns>Адрес сервера API для конкретного перечня.</returns>
        static string GetFileAPI(CatalogType catalogT)
        {
            if (catalogT == CatalogType.TE2) return $"{APIaddress}/suspect-catalogs/current-te2-file";
            else if (catalogT == CatalogType.MVK) return $"{APIaddress}/suspect-catalogs/current-mvk-file-zip";
            else if (catalogT == CatalogType.OMU) return $"{APIaddress}/suspect-catalogs/current-omu-file-zip";
            else throw new Exception("Название метода (скачивание файла) для указанного перечня не определено!");
        }

        /// <summary>
        /// Получает zip-файл Перечня организаций и физических лиц.
        /// </summary>
        /// <param name="client">Http клиент с добавленым сертификатом для доступа к API.</param>
        /// <param name="catalogT">Какой именно перечень необходимо скачать.</param>
        /// <returns></returns>
        static async Task GetFile(HttpClient client, CatalogType catalogT)
        {
            // Указываем данные, отправляемые в запросе.
            var values = new Dictionary<string, string> { };

            var content = new FormUrlEncodedContent(values);

            // Отправляем запрос на указанный адрес сервиса и получаем ответ.
            var response = await client.PostAsync(GetCatalogAPI(catalogT), content);

            var responseString = await response.Content.ReadAsStringAsync();

            // Парсит Json ответ в объект, из которого потом можно получить значения по ключу.
            dynamic? JSONobject = JsonConvert.DeserializeObject(responseString);

            Console.WriteLine(JSONobject);

            // Указываем данные, отправляемые в запросе.
            values = new Dictionary<string, string>
            {
                { "id", JSONobject?.idXml }
            };

            content = new FormUrlEncodedContent(values);

            // Отправляем запрос на указанный адрес сервиса и получаем ответ.
            response = await client.PostAsync(GetFileAPI(catalogT), content);

            responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            // Записываем полученный с сервера файл.
            StreamWriter sw = new StreamWriter($"{JSONcfg?.outputPath.Value}{Enum.GetName(catalogT)}-{DateTime.Parse(JSONobject?.date).ToString("dd.MM.yyyy")}.zip");
            sw.Write(response);
            sw.Close();
        }
    }
}
