using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

using System.IO;
using System.Threading.Tasks;
using System.Transactions;

namespace LibraryOfMoira.DataManagement
{
    public struct GoogleSheetsQuery
    {
        public string sheet;
        public string range;
    }

    public class GoogleSheetsDataPlugin : DataQuery<ValueRange, GoogleSheetsQuery>
    {
        private static readonly string GoogleSheetsAPIKey = "AIzaSyBZuzixPpRmAxBvqjxt318G79rGHI1pres";
        private static readonly string GoogleSheetsDocURI = "1jW8h6EbHVnlZISEbbobhgurLEgQicNDO2SmdYMhxHoo";
        private static readonly string GoogleSheetsName = "LibraryOfMoira";
        private static readonly string[] GoogleSheetsScopes = { SheetsService.Scope.Spreadsheets };

        private SheetsService _service;

        protected GoogleCredential GetCredentialsFromFile()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("google_client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(GoogleSheetsScopes);
            }
            return credential;
        }

        public GoogleSheetsDataPlugin()
        {
            var googleInit = new BaseClientService.Initializer();
            googleInit.ApiKey = GoogleSheetsAPIKey;
            googleInit.ApplicationName = GoogleSheetsName;

            var credential = GetCredentialsFromFile();
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = GoogleSheetsName,
            });
        }

        public async Task<ValueRange> GetAsync(GoogleSheetsQuery scope)
        {
            try
            {
                return await _service.Spreadsheets.Values.Get(GoogleSheetsDocURI, $"{scope.sheet}!{scope.range}").ExecuteAsync();
            }
            catch
            {
                return null;
            }
        }

        public ValueRange GetSync(GoogleSheetsQuery scope)
        {
            var results = Task.Run(async () => await GetAsync(scope));
            results.Wait(-1);

            if (results.IsCompletedSuccessfully)
            {
                return results.Result;
            }
            else
            {
                return null;
            }
        }
    }

}
