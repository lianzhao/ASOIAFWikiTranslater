namespace ASOIAF.Wiki.Translater
{
    using System.Runtime.CompilerServices;

    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            return this.Set(propertyName, ref field, value);
        }
    }
}