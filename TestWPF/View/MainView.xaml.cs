using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using TestWPF.MVVM;

namespace TestWPF.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        [Import("MainViewModel")]
        public IViewModel ViewModel
        {
            set
            {
                DataContext = value;
            }
        }

        public MainView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
                Bootstrapper.ComposeParts(this);
        }
    }
}
