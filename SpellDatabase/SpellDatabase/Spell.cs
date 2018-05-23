using SpellDatabase.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
        private bool ComponentVValue; public bool ComponentV { get => ComponentVValue; set { ComponentVValue = value; OnPropertyChanged(); } }
        private bool ComponentSValue; public bool ComponentS { get => ComponentSValue; set { ComponentSValue = value; OnPropertyChanged(); } }
        private string ComponentMValue; public string ComponentM { get => ComponentMValue; set { ComponentMValue = value; OnPropertyChanged(); } }
        private string DurationValue; public string Duration { get => DurationValue; set { DurationValue = value; OnPropertyChanged(); } }
        private bool ConcentrationValue; public bool Concentration { get => ConcentrationValue; set { ConcentrationValue = value; OnPropertyChanged(); } }
        private string DescriptionValue; public string Description { get => DescriptionValue; set { DescriptionValue = value; OnPropertyChanged(); } }
        private string AtHigherLevelsValue; public string AtHigherLevels { get => AtHigherLevelsValue; set { AtHigherLevelsValue = value; OnPropertyChanged(); } }

        public string Display => ToString();
        public override string ToString() => $"{Name} ({Type} {Level})";

        public event PropertyChangedEventHandler PropertyChanged;

        public string GetFormatted(string aFormat)
        {
            if (aFormat == null) throw new ArgumentNullException(nameof(aFormat));
            if (aFormat.Length <= 0) throw new ArgumentException(nameof(aFormat));
            const string ft = "\\ft{";

            var formatParams = new List<string>();
            formatParams.Add(Name ?? "");

            var level = Level;
            if (level == ESpellLevel.Cantrip)
                formatParams.Add($"{Type} {ESpellLevel.Cantrip.ToString().ToLower()}");
            else
            {
                var postfix = "th";
                switch (level)
                {
                    case ESpellLevel.Level1: postfix = "st"; break;
                    case ESpellLevel.Level2: postfix = "nd"; break;
                    case ESpellLevel.Level3: postfix = "rd"; break;
                }

                formatParams.Add($"{(int)level}{postfix}-level {Type.ToString().ToLower()}");
            }

            formatParams.Add(CastingTime ?? "");
            var range = Range;
            if (int.TryParse(range, out var intRange))
                formatParams.Add(ft + intRange + "}");
            else
                formatParams.Add(range ?? "");

            var material = ComponentM;
            var components = new List<string>();
            if (ComponentV) components.Add("V");
            if (ComponentS) components.Add("S");
            if (!string.IsNullOrWhiteSpace(material)) components.Add($"M ({material})");
            formatParams.Add(string.Join(", ", components.Cast<object>().ToArray()));

            formatParams.Add(Duration ?? "");
            formatParams.Add(Description ?? "");

            var atHigherLevels = AtHigherLevels;
            formatParams.Add(atHigherLevels ?? "");

            formatParams.Add(string.IsNullOrWhiteSpace(atHigherLevels) ? "" : "At Higher Levels.");

            var splitter = aFormat[0];
            var ret = string.Join("", aFormat
                .Split(splitter)
                .Select(aSplit =>
                {
                    if (int.TryParse(aSplit, out var index))
                        return formatParams[index];
                    return aSplit;
                })
                .Cast<object>()
                .ToArray());

            var startSearch = 0;
            while (true)
            {

                var feetIndex = ret.IndexOf(ft, startSearch);
                if (feetIndex < 0) break;

                var endFeetIndex = ret.IndexOf("}", feetIndex);
                if (endFeetIndex < 0) break;

                var innerStart = feetIndex + ft.Length;
                if (int.TryParse(ret.Substring(innerStart, endFeetIndex - innerStart), out var innerFt))
                {
                    ret = $"{ret.Substring(0, feetIndex)} {innerFt} ft. ({Math.Round(innerFt / 3f, 1).ToString(CultureInfo.InvariantCulture)}m) {ret.Substring(endFeetIndex + 1)}";
                    startSearch = feetIndex + 1;
                }
                else
                    startSearch = endFeetIndex;
            }

            return ret;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
