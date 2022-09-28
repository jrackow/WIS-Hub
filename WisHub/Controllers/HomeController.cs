using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WisHub.Models;
using MySql.Data.MySqlClient;
namespace WisHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
                List<DataSet> data = new List<DataSet>();
                string ConnectionString = "server=127.0.0.1;user=wishub;database=wis;port=3306;password=Test1.";
                MySqlConnection con = new MySqlConnection(ConnectionString);

                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM SensorData ORDER BY DateAndTime DESC LIMIT 0,5;", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime dt = DateTime.Parse(reader.GetString(1));
                    data.Add(new DataSet
                    {
                        ID = Int32.Parse(reader.GetString(0)),
                        Timestamp = dt.ToString("HH:mm"),
                        Temperature = Convert.ToInt16(reader.GetFloat(2)),
                        Humidity = Convert.ToInt16(reader.GetFloat(3)),
                        Moisture1 = Newpercent(reader.GetInt16(4)),
                        Moisture2 = Newpercent(reader.GetInt16(5))
                    });
               }
            ViewBag.Data = data;
            reader.Close();
            cmd.Dispose();
            con.Close();
            return View();
        }

        private static double Newpercent(double moisture)
        {
            double percmoisture = (1-((moisture-280)/320))*100;
            percmoisture = Convert.ToInt16(percmoisture);
            return percmoisture;
        }

    }
}