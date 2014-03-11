namespace ASOIAF.Wiki.Translater
{
    using System;

    public class ViewModelLocator
    {
        private static readonly Lazy<MainViewModel> MainLazy = new Lazy<MainViewModel>();

        public MainViewModel Main
        {
            get
            {
                return MainLazy.Value;
            }
        }
    }
}