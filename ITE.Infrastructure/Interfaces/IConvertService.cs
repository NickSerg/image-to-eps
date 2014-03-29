using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ITE.Infrastructure.Interfaces
{
    public interface IConvertService
    {
        ObservableCollection<string> RetrieveProcessedFiles();

        ICommand ConvertCommand { get; }
    }
}
