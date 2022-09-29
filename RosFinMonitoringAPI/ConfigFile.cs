using Newtonsoft.Json;

namespace RosFinMonitoringAPI
{
    partial class Program
    {
        /// <summary>
        /// Получает данные из config.json файла, если файл не существует, создает его.
        /// </summary>
        static void CheckConfig()
        {
            try
            {
                // Проверяем наличие файла и открываем его для чтения
                StreamReader sr = new StreamReader($"{Directory.GetCurrentDirectory()}\\config.json");

                // Если он есть и открылся, считываем из него данные
                JSONcfg = JsonConvert.DeserializeObject(sr.ReadToEnd());

                // Обращаемся к объекту с запросом на чтение, тем самым инициализируем его.
                // НЕ УДАЛЯТЬ! Обязательно нужно хотя-бы 1 раз прочитать объект, 
                // перед тем как обращаться к его полям.
                Console.WriteLine(JSONcfg);

                sr.Close();

                // Проверка файла config.json на корректность
                if (string.IsNullOrEmpty(JSONcfg?.outputPath?.Value) ||
                    string.IsNullOrEmpty(JSONcfg?.userName?.Value) ||
                    string.IsNullOrEmpty(JSONcfg?.password?.Value) ||
                    string.IsNullOrEmpty(JSONcfg?.certName?.Value))
                    throw new Exception("Некорректный формат файла config.json! Удалите файл и запустите программу еще раз." + 
                        $"\n{Directory.GetCurrentDirectory()}\\config.json");

                // Проверка файла config.json на заполнение данных
                if (JSONcfg?.userName?.Value == "YourUsernameHere" ||
                    JSONcfg?.password?.Value == "YourPasswordHere" ||
                    JSONcfg?.certName?.Value == "FileName.cer")
                    throw new Exception("Необходимо поменять данные для авторизации в файле и перезапустить программу." + 
                        $"\n{Directory.GetCurrentDirectory()}\\config.json");
            }
            catch (System.IO.FileNotFoundException)     // Если файл не найден
            {
                // Задаем шаблон файла
                var defaultCfg = new Dictionary<string, string>
                {
                    { "outputPath", "C:\\" },
                    { "userName", "YourUsernameHere" },
                    { "password", "YourPasswordHere" },
                    { "certName", "FileName.cer" }
                };

                // Создаем новый файл и открываем его для записи
                StreamWriter sw = new StreamWriter($"{Directory.GetCurrentDirectory()}\\config.json");
                
                // Записываем шаблонные данные в файл
                sw.Write(JsonConvert.SerializeObject(defaultCfg, Formatting.Indented));
                sw.Close();

                // Выводим уведомление о создании файла.
                throw new Exception("Был создан новый файл config.json, так как не удалось найти старый.\n" +
                    "Необходимо поменять данные для авторизации в файле и перезапустить программу." + 
                    $"\n{Directory.GetCurrentDirectory()}\\config.json");
            }
        }
    }
}