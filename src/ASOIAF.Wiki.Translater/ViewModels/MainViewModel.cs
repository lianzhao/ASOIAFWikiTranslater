namespace ASOIAF.Wiki.Translater
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;

    using Common.Logging;

    using GalaSoft.MvvmLight.Command;

    public class MainViewModel : ViewModelBase
    {
        private const string DictionaryPath = @".\dict";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Inpcs
        #region Dictionaries
        private ObservableCollection<DictionaryViewModel> dictionaries = default(ObservableCollection<DictionaryViewModel>);

        public ObservableCollection<DictionaryViewModel> Dictionaries
        {
            get
            {
                return this.dictionaries;
            }

            set
            {
                this.Set(ref this.dictionaries, value);
            }
        }
        #endregion

        #region Source
        private string source = default(string);

        public string Source
        {
            get
            {
                return this.source;
            }

            set
            {
                this.Set(ref this.source, value);
            }
        }
        #endregion

        #region IsTranslating
        private bool isTranslating = default(bool);

        public bool IsTranslating
        {
            get
            {
                return this.isTranslating;
            }

            set
            {
                this.Set(ref this.isTranslating, value);
            }
        }
        #endregion
        
        #region Dest
        private string dest = default(string);

        public string Dest
        {
            get
            {
                return this.dest;
            }

            set
            {
                this.Set(ref this.dest, value);
            }
        }
        #endregion

        #endregion

        #region Commands
        public ICommand InitializeCommand { get; private set; }

        #region Initialize
        private void Initialize()
        {
            this.Dictionaries = this.Dictionaries ?? new ObservableCollection<DictionaryViewModel>();
            string[] dictionaryFiles = null;
            try
            {
                dictionaryFiles = Directory.GetFiles(DictionaryPath);
            }
            catch (Exception ex)
            {
                Log.Fatal(string.Format("Error occurred while getting file from directory :{0}", DictionaryPath), ex);
                return;
            }

            foreach (var dictionaryFile in dictionaryFiles)
            {
                if (string.IsNullOrWhiteSpace(dictionaryFile))
                {
                    continue;
                }

                try
                {
                    this.Dictionaries.Add(new DictionaryViewModel(dictionaryFile));
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Error occurred while loading dictionary from file :{0}", dictionaryFile), ex);
                    continue;
                }
            }
        }

        private bool CanInitialize()
        {
            return this.Dictionaries == null || !this.Dictionaries.Any();
        }
        #endregion

        public ICommand TranslateCommand { get; private set; }

        #region Translate
        private void Translate()
        {
            if (string.IsNullOrEmpty(this.Source) || this.Dictionaries == null || !this.Dictionaries.Any())
            {
                return;
            }

            this.IsTranslating = true;
            try
            {
                var sb = new StringBuilder(this.Source);
                foreach (var dictionary in this.Dictionaries.Where(dict => dict.IsEnabled))
                {
                    foreach (var kvp in dictionary)
                    {
                        sb.Replace(kvp.Key, kvp.Value);
                    }
                }
                this.Dest = sb.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("Error occurred while translating", ex);
            }
            this.IsTranslating = false;
        }

        private bool CanTranslate()
        {
            return !this.IsTranslating;
        }
        #endregion

        public ICommand PasteCommand { get; private set; }

        #region Paste
        private void Paste()
        {
            this.Source = Clipboard.GetText();
        }

        private bool CanPaste()
        {
            return !this.IsTranslating;
        }
        #endregion

        public ICommand CopyCommand { get; private set; }

        #region Copy
        private void Copy()
        {
            Clipboard.SetText(this.Dest);
        }

        private bool CanCopy()
        {
            return !this.IsTranslating;
        }
        #endregion

        public ICommand AllInOneCommand { get; private set; }

        #region AllInOne
        private void AllInOne()
        {
            this.PasteCommand.TryExecute();
            this.TranslateCommand.TryExecute();
            this.CopyCommand.TryExecute();
        }

        private bool CanAllInOne()
        {
            return !this.IsTranslating;
        }
        #endregion

        // Please move the following line to Ctor
        //


        #endregion

        public MainViewModel()
        {
            this.InitializeCommand = new RelayCommand(this.Initialize, this.CanInitialize);
            this.TranslateCommand = new RelayCommand(this.Translate, this.CanTranslate);
            this.PasteCommand = new RelayCommand(this.Paste, this.CanPaste);
            this.CopyCommand = new RelayCommand(this.Copy, this.CanCopy);
            this.AllInOneCommand = new RelayCommand(this.AllInOne, this.CanAllInOne);
        }
    }
}