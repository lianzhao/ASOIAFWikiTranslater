namespace ASOIAF.Wiki.Translater
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Common.Logging;

    using Newtonsoft.Json;

    public class DictionaryViewModel : ViewModelBase, IEnumerable<KeyValuePair<string, string>>
    {
        private const char Separator = '#';

        private const string CommentCharacter = "#";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        // allows duplicate key
        protected IDictionary<string, string> Dictionary { get; private set; }

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
            var fileContent = File.ReadAllText(path);
            this.Dictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(fileContent);
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