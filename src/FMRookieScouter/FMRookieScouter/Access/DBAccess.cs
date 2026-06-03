using FMRookieScouter.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FMRookieScouter.Access
{
    public class DBAccess
    {
        #region Const Field
        public const string DB_PATH = @"DB\";
        #endregion

        #region Properties
        public Dictionary<int, Sesson> Sessons { get; }
        #endregion

        #region Constructor
        public DBAccess()
        {
            if (!Directory.Exists(DB_PATH))
                throw new Exception("DB Path is empty");

            Sessons = LoadSessions(DB_PATH);
        }
        #endregion

        #region Functions
        private static Dictionary<int, Sesson> LoadSessions(string dirPath)
        {
            var filePaths = Directory.GetFiles(DB_PATH);
            var sessons = new List<Sesson>();

            foreach (var filePath in filePaths)
            {
                if (!File.Exists(filePath))
                    continue;

                var doc = XDocument.Load(filePath);
                var sesson = new Sesson();

                sesson.Load(doc.Root);
                sessons.Add(sesson);
            }

            return sessons.ToDictionary(s => s.Year);
        }

        public Sesson GetSesson(int year)
        {
            if (!Sessons.TryGetValue(year, out Sesson sesson))
                return null;

            return sesson;
        }
        #endregion
    }
}

