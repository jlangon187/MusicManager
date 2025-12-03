namespace MusicManager.Modelos
{
    public class ApiConfig
    {
        public bool usar_iTunes { get; set; } = true;
        public bool usar_lastFm { get; set; } = true;
        public string lastFm_apiKey { get; set; } = "";
    }
}