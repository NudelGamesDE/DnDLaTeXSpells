using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SpellDatabase;
using SpellDatabaseGui.Annotations;

namespace SpellDatabaseGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IDatabase SpellContainer;
        public ObservableCollection<Spell> Spells { get; }

        public IList<ESpellType> SpellTypes => Enum.GetValues(typeof(ESpellType)).Cast<ESpellType>().ToList();
        public IList<ESpellLevel> SpellLevels => Enum.GetValues(typeof(ESpellLevel)).Cast<ESpellLevel>().ToList();

        public MainWindow()
        {
            SpellContainer = FromFile.Open("spells.json");
            Spells = new ObservableCollection<Spell>(SpellContainer.Spells);
            SpellContainer.Spells.CollectionChanged += SpellsOnCollectionChanged;
            SpellsOnCollectionChanged(this, null);

            DataContext = this;
            InitializeComponent();

            void SpellsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
            {
                Spells.Clear();
                foreach (var spell in SpellContainer.Spells.OrderBy(aSpell => aSpell.Name))
                    Spells.Add(spell);
                SpellSelected(Spells.FirstOrDefault());
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SpellContainer.Apply();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SpellSelected(e.AddedItems.OfType<Spell>().FirstOrDefault());
        }

        public Spell CurrentSpell { get; private set; }
        private void SpellSelected(Spell aSpell)
        {
            CurrentSpell = aSpell ?? new Spell { Name = "Null Spell" };
            OnPropertyChanged(nameof(CurrentSpell));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            SpellContainer.Spells.Add(new Spell { Name = "New Spell" });
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SpellContainer.Spells.Remove(CurrentSpell);
        }
    }
}
