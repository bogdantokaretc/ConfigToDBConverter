using ConfigToDBConverter.Data;
using ConfigToDBConverter.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConfigToDBConverter.Controllers
{
    public class ConfigController: Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfigController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
			// Отримати всі конфігураційні дані з бази даних
			List<ConfigDataModel> configData = _context.ConfigData.ToList();

			// Передати дані до перегляду (View)
			return View(configData);
        }

        [HttpPost]
        public IActionResult UploadConfigFile(IFormFile configFile)
        {
            if (configFile != null && configFile.Length > 0)
            {
                try
                {
                    // Отримати вміст файлу
                    using (var reader = new StreamReader(configFile.OpenReadStream()))
                    {
                        var content = reader.ReadToEnd();

                        // Розпакувати JSON в об'єкт
                        var configObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

                        // Зберегти конфігурацію в базу даних
                        SaveConfigRecursively(configObject);

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    // Обробити помилку, наприклад, вивести її в лог чи повідомити користувача
                    return RedirectToAction("Error");
                }
            }

            return RedirectToAction("Index");
        }


        private void SaveConfigRecursively(Dictionary<string, object> configObject, int? parentID = null)
        {
            foreach (var entry in configObject)
            {
                var configData = new ConfigDataModel
                {
                    ParentID = parentID,
                    Name = entry.Key,
                    Value = entry.Value?.ToString()
                };

                _context.ConfigData.Add(configData);
                _context.SaveChanges();
                //Dictionary<string, object> nestedObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(entry.ToString());

                if (entry.Value is JObject)
                {
                    entry.Value.GetType();
                    var nestedObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(entry.Value.ToString());
                    //((Newtonsoft.Json.Linq.JContainer)entry.Value).First;
                    // Рекурсивно обробляємо вкладений об'єкт
                    SaveConfigRecursively(nestedObject, configData.ID);
                }
                

            }
            

        }
    }
}
