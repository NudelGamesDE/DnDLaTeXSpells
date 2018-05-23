using SpellDatabase;
using System;
using System.Windows;

namespace SpellDatabaseGui
{
    /// <summary>
    /// Interaction logic for FormatWindow.xaml
    /// </summary>
    public partial class FormatWindow : Window
    {
        public IDatabase SpellDatabase { get; }
        public FormatWindow(IDatabase aSpellDatabase)
        {
            SpellDatabase = aSpellDatabase ?? throw new ArgumentNullException(nameof(aSpellDatabase));
            DataContext = this;
            InitializeComponent();
        }
    }
}
