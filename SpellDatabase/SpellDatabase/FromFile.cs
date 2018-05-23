using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SpellDatabase
{
    public class FromFile : IDatabase
    {
        public string Format { get; set; }
        ObservableCollection<Spell> IDatabase.Spells => Spells;
        [JsonIgnore]
        public ObservableCollection<Spell> Spells { get; }

        [JsonProperty("Spells")]
        public Spell[] JsonSpells
        {
            get => Spells.ToArray();
            set
            {
                Spells.Clear();
                foreach (var spell in value ?? new Spell[0])
                    Spells.Add(spell);
            }
        }

        public FromFile() => Spells = new ObservableCollection<Spell>();

        public static FromFile Open(string aPath)
        {
            if (aPath == null) throw new ArgumentNullException(nameof(aPath));
            FromFile ret;
            try
            {
                string fileContent;
                using (var file = new StreamReader(aPath))
                    fileContent = file.ReadToEnd();

                ret = JsonConvert.DeserializeObject<FromFile>(fileContent) ?? new FromFile();
            }
            catch (Exception)
            {
                ret = new FromFile();
            }
            ret.FilePath = aPath;
            return ret;
        }

        private string FilePath;
        public void Apply()
        {
            if (FilePath == null) throw new Exception();

            using (var file = new StreamWriter(FilePath))
                file.Write(JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));
        }
    }
}
