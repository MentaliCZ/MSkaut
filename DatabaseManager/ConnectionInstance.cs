using Supabase;

namespace DatabaseManager
{
    public class ConnectionInstance
    {
        public Client Client { get; private set; }

        private ConnectionInstance() { }

        public static async Task<ConnectionInstance> CreateInstance()
        {
            ConnectionInstance instance = new();
            await instance.InitClient();
            return instance;
        }

        private async Task InitClient()
        {
            string publicKey =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJoZm55dWlxY3lkdXp1Ym92ZmlnIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQ5NzA4MzcsImV4cCI6MjA2MDU0NjgzN30.vzBBxZxRRHa0HbAeG7mcK_4t-EbaJ7-rDHKvezw2zZ4";
            string urlString = "https://rhfnyuiqcyduzubovfig.supabase.co";

            var url = Environment.GetEnvironmentVariable(urlString);
            var key = Environment.GetEnvironmentVariable(publicKey);

            Client = new Client(urlString, publicKey);
            await Client.InitializeAsync();
        }
    }
}
