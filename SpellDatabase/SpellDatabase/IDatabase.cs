using System.Collections.ObjectModel;

namespace SpellDatabase
{
    public interface IDatabase
    {
        string Format { get; }
        ObservableCollection<Spell> Spells { get; }
        void Apply();
    }
}
