using ITE.ConvertModule.ViewModels;
using Microsoft.Practices.Unity;

namespace ITE.ConvertModule.Views
{
    /// <summary>
    /// Interaction logic for ConvertCommandsView.xaml
    /// </summary>
    public partial class ConvertCommandsView
    {
        public ConvertCommandsView()
        {
            InitializeComponent();
        }

        [Dependency]
        public ConvertCommandsViewModel ViewModel
        {
            get { return DataContext as ConvertCommandsViewModel; }
            set
            {
                DataContext = value;
            }
        }
    }
}
