using System.Collections.ObjectModel;

namespace SpellDatabase
{
    public interface IDatabase
    {
        string Format { get; set; }
        ObservableCollection<Spell> Spells { get; }
        void Apply();
    }
}
