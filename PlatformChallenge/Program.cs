namespace PlatformChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            CsvInterface csvInput = new CsvInterface();
            csvInput.LoadCVSData();

            WebhookInterface.SendDataListToWebhook(csvInput.csvData);

        }
    }
}
