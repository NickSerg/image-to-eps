using ITE.ConvertModule.ViewModels;
using Microsoft.Practices.Unity;

namespace ITE.ConvertModule.Views
{
    /// <summary>
    /// Interaction logic for ConvertView.xaml
    /// </summary>
    public partial class ConvertView
    {
        public ConvertView()
        {
            InitializeComponent();
        }

        [Dependency]
        public ConvertViewModel ViewModel
        {
            get { return DataContext as ConvertViewModel; }
            set
            {
                DataContext = value;
            }
        }
    }
}
