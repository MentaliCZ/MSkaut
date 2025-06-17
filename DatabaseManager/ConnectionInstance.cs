using Supabase;

namespace DatabaseManager
{
    public class ConnectionInstance
    {
        public Client Client { get; private set; }
        private static string URL_FILE = "url.txt";
        private static string PUBLIC_KEY_FILE = "key.txt";

        private ConnectionInstance() { }

        public static async Task<ConnectionInstance> CreateInstance()
        {
            ConnectionInstance instance = new();
            await instance.InitClient();
            return instance;
        }

        private async Task InitClient()
        {
            string publicKey = File.ReadAllText(PUBLIC_KEY_FILE);
            string urlString = File.ReadAllText(URL_FILE);

            Client = new Client(urlString, publicKey);
            await Client.InitializeAsync();
        }
    }
}
