using SpellDatabase.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpellDatabase
{
    public sealed class Spell : INotifyPropertyChanged
    {
        private string NameValue; public string Name { get => NameValue; set { NameValue = value; OnPropertyChanged(); OnPropertyChanged(nameof(Display)); } }
        private ESpellType TypeValue; public ESpellType Type { get => TypeValue; set { TypeValue = value; OnPropertyChanged(); OnPropertyChanged(nameof(Display)); } }
        private ESpellLevel LevelValue; public ESpellLevel Level { get => LevelValue; set { LevelValue = value; OnPropertyChanged(); OnPropertyChanged(nameof(Display)); } }
        private bool RitualValue; public bool Ritual { get => RitualValue; set { RitualValue = value; OnPropertyChanged(); } }
        private string CastingTimeValue; public string CastingTime { get => CastingTimeValue; set { CastingTimeValue = value; OnPropertyChanged(); } }
        private string RangeValue; public string Range { get => RangeValue; set { RangeValue = value; OnPropertyChanged(); } }
        private bool ComponentVValue; public bool ComponentV { get => ComponentVValue; set { ComponentVValue = value; OnPropertyChanged() ; } }
        private bool ComponentSValue; public bool ComponentS { get => ComponentSValue; set { ComponentSValue = value; OnPropertyChanged(); } }
        private string ComponentMValue; public string ComponentM { get => ComponentMValue; set { ComponentMValue = value; OnPropertyChanged(); } }
        private string DurationValue; public string Duration { get => DurationValue; set { DurationValue = value; OnPropertyChanged(); } }
        private string DescriptionValue; public string Description { get => DescriptionValue; set { DescriptionValue = value; OnPropertyChanged(); } }
        private string AtHigherLevelsValue; public string AtHigherLevels { get => AtHigherLevelsValue; set { AtHigherLevelsValue = value; OnPropertyChanged(); } }

        public string Display => ToString();
        public override string ToString() => $"{Name} ({Type} {Level})";

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
