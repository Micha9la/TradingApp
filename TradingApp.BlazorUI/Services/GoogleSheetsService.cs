namespace TradingApp.BlazorUI.Services
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using System.IO;
    using System.Threading.Tasks;

    public class GoogleSheetsService
    {
        private readonly SheetsService _sheetsService;
        private readonly string _spreadsheetId;

        public GoogleSheetsService(string spreadsheetId, string serviceAccountJsonPath)
        {
            _spreadsheetId = spreadsheetId;

            GoogleCredential credential;
            using (var stream = new FileStream(serviceAccountJsonPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(SheetsService.Scope.Spreadsheets);
            }

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "TradingApp Google Sheets Integration",
            });
        }

        public async Task<IList<IList<object>>> ReadSheetAsync(string sheetName, string range)
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, $"{sheetName}!{range}");
            var response = await request.ExecuteAsync();
            return response.Values;
        }

        public async Task WriteToSheetAsync(string sheetName, IList<IList<object>> values)
        {
            var body = new ValueRange
            {
                Values = values
            };

            var request = _sheetsService.Spreadsheets.Values.Update(body, _spreadsheetId, $"{sheetName}!A1");
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

            await request.ExecuteAsync();
        }
    }
}
