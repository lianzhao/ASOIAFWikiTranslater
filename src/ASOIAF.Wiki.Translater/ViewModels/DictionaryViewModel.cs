namespace ASOIAF.Wiki.Translater
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Common.Logging;

    public class DictionaryViewModel : ViewModelBase, IEnumerable<KeyValuePair<string, string>>
    {
        private const char Separator = '#';

        private const string CommentCharacter = "#";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        // allows duplicate key
        protected List<KeyValuePair<string, string>> Dictionary { get; private set; }

        #region Inpcs
        #region Name
        private string name = default(string);

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.Set(ref this.name, value);
            }
        }
        #endregion

        #region IsEnabled
        private bool isEnabled = default(bool);

        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                this.Set(ref this.isEnabled, value);
            }
        }
        #endregion

        #endregion

        public DictionaryViewModel(string path)
        {
            var fi = new FileInfo(path);
            this.Name = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            this.IsEnabled = true;
            var fileContent = File.ReadAllLines(path);
            this.Dictionary = new List<KeyValuePair<string, string>>();
            foreach (var line in fileContent)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    // ignore blank line
                    continue;
                }

                if (line.StartsWith(CommentCharacter))
                {
                    // ignore comment line
                    Log.Debug(string.Format("Ignore comment line :{0}", line));
                    continue;
                }

                var parts = line.Split(Separator);
                if (parts.Length != 2)
                {
                    // ignore invalid dictionary entry
                    Log.Warn(string.Format("Invalid dictionary entry:{0}", line));
                    continue;
                }

                this.Dictionary.Add(new KeyValuePair<string, string>(parts[0], parts[1]));
            }
        }

        #region Implement IEnumerable<KeyValuePair<string, string>>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}