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
                { "userName", JSONcfg.userName.Value },
                { "password", JSONcfg.password.Value }
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
        /// Получает zip-файл Перечня организаций и физических лиц, 
        /// в отношении которых имеются сведения об их причастности к экстремистской деятельности или терроризму.
        /// </summary>
        /// <param name="client">Http клиент с добавленым сертификатом для доступа к API.</param>
        /// <returns></returns>
        static async Task Te2GetFile(HttpClient client)
        {
            // Указываем данные, отправляемые в запросе.
            var values = new Dictionary<string, string>{};

            var content = new FormUrlEncodedContent(values);

            // Отправляем запрос на указанный адрес сервиса и получаем ответ.
            var response = await client.PostAsync($"{APIaddress}/suspect-catalogs/current-te2-catalog", content);

            var responseString = await response.Content.ReadAsStringAsync();

            // Парсит Json ответ в объект, из которого потом можно получить значения по ключу.
            dynamic? JSONobject = JsonConvert.DeserializeObject(responseString);

            Console.WriteLine(responseString);

            // Указываем данные, отправляемые в запросе.
            values = new Dictionary<string, string>
            {
                { "id", JSONobject?.idXml }
            }; 
            
            content = new FormUrlEncodedContent(values);

            // Отправляем запрос на указанный адрес сервиса и получаем ответ.
            response = await client.PostAsync($"{APIaddress}/suspect-catalogs/current-te2-file", content);

            responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            // Парсит Json ответ в объект, из которого потом можно получить значения по ключу.
            JSONobject = JsonConvert.DeserializeObject(responseString);

            // Записываем полученный с сервера файл.
            StreamWriter sw = new StreamWriter($"{outputPath}\\TE2-{DateTime.Parse(JSONobject?.date).ToString("dd.MM.yyyy")}.zip");
            sw.Write(response);
            sw.Close();
        }


        /// <summary>
        /// Получает zip-файл Перечня организаций и физических лиц, 
        /// в отношении которых действует решение Комиссии о замораживании (блокировании) принадлежащих им денежных средств или иного имущества.
        /// </summary>
        /// <param name="client">Http клиент с добавленым сертификатом для доступа к API.</param>
        /// <returns></returns>
        static async Task MvkGetFile(HttpClient client)
        {
            // Указываем данные, отправляемые в запросе.
            var values = new Dictionary<string, string>{};

            var content = new FormUrlEncodedContent(values);

            // Отправляем запрос на указанный адрес сервиса и получаем ответ.
            var response = await client.PostAsync($"{APIaddress}/suspect-catalogs/current-mvk-catalog", content);

            var responseString = await response.Content.ReadAsStringAsync();

            // Парсит Json ответ в объект, из которого потом можно получить значения по ключу.
            dynamic? JSONobject = JsonConvert.DeserializeObject(responseString);

            Console.WriteLine(responseString);

            // Указываем данные, отправляемые в запросе.
            values = new Dictionary<string, string>
            {
                { "id", JSONobject?.idXml }
            }; 
            
            content = new FormUrlEncodedContent(values);

            // Отправляем запрос на указанный адрес сервиса и получаем ответ.
            response = await client.PostAsync($"{APIaddress}/suspect-catalogs/current-mvk-file-zip", content);

            responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            // Парсит Json ответ в объект, из которого потом можно получить значения по ключу.
            JSONobject = JsonConvert.DeserializeObject(responseString);

            // Записываем полученный с сервера файл.
            StreamWriter sw = new StreamWriter($"{outputPath}\\MVK-{DateTime.Parse(JSONobject?.date).ToString("dd.MM.yyyy")}.zip");
            sw.Write(response);
            sw.Close();
        }


        /// <summary>
        /// Получает zip-файл Перечня организаций и физических лиц, 
        /// в отношении которых имеются сведения об их причастности к распространению оружия массового уничтожения.
        /// </summary>
        /// <param name="client">Http клиент с добавленым сертификатом для доступа к API.</param>
        /// <returns></returns>
        static async Task OmuGetFile(HttpClient client)
        {
            // Указываем данные, отправляемые в запросе.
            var values = new Dictionary<string, string>{};

            var content = new FormUrlEncodedContent(values);

            // Отправляем запрос на указанный адрес сервиса и получаем ответ.
            var response = await client.PostAsync($"{APIaddress}/suspect-catalogs/current-omu-catalog", content);

            var responseString = await response.Content.ReadAsStringAsync();

            // Парсит Json ответ в объект, из которого потом можно получить значения по ключу.
            dynamic? JSONobject = JsonConvert.DeserializeObject(responseString);

            Console.WriteLine(responseString);

            // Указываем данные, отправляемые в запросе.
            values = new Dictionary<string, string>
            {
                { "id", JSONobject?.idXml }
            }; 
            
            content = new FormUrlEncodedContent(values);

            // Отправляем запрос на указанный адрес сервиса и получаем ответ.
            response = await client.PostAsync($"{APIaddress}/suspect-catalogs/current-omu-file-zip", content);

            responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            // Парсит Json ответ в объект, из которого потом можно получить значения по ключу.
            JSONobject = JsonConvert.DeserializeObject(responseString);

            // Записываем полученный с сервера файл.
            StreamWriter sw = new StreamWriter($"{outputPath}\\OMU-{DateTime.Parse(JSONobject?.date).ToString("dd.MM.yyyy")}.zip");
            sw.Write(response);
            sw.Close();
        }
    }
}
