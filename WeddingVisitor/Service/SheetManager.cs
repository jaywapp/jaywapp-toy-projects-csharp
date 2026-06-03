using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using WeddingVisitor.Model;

namespace WeddingVisitor.Service
{
    public class SheetManager
    {
        #region Internal Field
        private readonly string _key;
        private readonly string _applicationName;
        private readonly string _sheetID;
        private readonly string _sheetName;

        private readonly char _startColumn;
        private readonly char _endColumn;
        private readonly int _startRow;
        #endregion

        #region Static Field
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        #endregion

        #region Properties
        public SheetsService SheetsService { get; }
        #endregion

        #region Constructor
        public SheetManager(string key, string appName, string sheetID, string sheetName)
        {
            (_startColumn, _endColumn) = Guest.GetRange();
            _startRow = 2;

            _key = key;
            _applicationName = appName;
            _sheetID = sheetID;
            _sheetName = sheetName;

            UserCredential credential;

            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                var credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            SheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
        }
        #endregion

        #region Functions
        public void Sync(IList<Guest> guests)
        {
            var valueRange = new ValueRange()
            {
                MajorDimension = "ROWS",
                Values = guests
                    .Select(g => g.GetRow())
                    .ToList(),
            };

            Update(valueRange);
        }

        public void Update(ValueRange valueRange)
        {
            var range = $"{_sheetName}!{_startColumn}{_startRow}:{_endColumn}{_startRow + valueRange.Values.Count()}";
            var update = SheetsService.Spreadsheets.Values.Update(valueRange, _sheetID, range);

            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            update.Execute();
        }

        public void Clear()
        {
            if (SheetsService == null)
                return;

            var request = SheetsService.Spreadsheets.Values.Get(_sheetID, $"{_sheetName}!{_startColumn}{_startRow}:{_endColumn}");
            var response = request.Execute();
            var values = response.Values;

            if (values == null || values.Count <= 0)
                return;

            var valueRange = new ValueRange()
            {
                MajorDimension = "ROWS",
                Values = values
                    .Select(v => GetEmptyRow(_startColumn, _endColumn))
                    .ToList(),
            };

            Update(valueRange);
        }

        public IEnumerable<Guest> Load()
        {
            if (SheetsService == null)
                yield break;

            var request = SheetsService.Spreadsheets.Values.Get(_sheetID, $"{_sheetName}!{_startColumn}{_startRow}:{_endColumn}");

            var response = request.Execute();
            var values = response.Values;

            if (values == null || values.Count <= 0)
                yield break;

            foreach (var value in values)
                yield return Guest.Create(value);
        }

        private static IList<object> GetEmptyRow(char startCol, char endCol)
        {
            var row = new List<object>();
            for (var c = startCol; c <= endCol; c++)
                row.Add("");

            return row;
        }
        #endregion
    }
}
