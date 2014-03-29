using System.Collections.ObjectModel;
using System.Windows.Input;
using ITE.Infrastructure.Interfaces;

namespace ITE.ConvertModule.Services
{
    public class ConvertService: IConvertService
    {
        private readonly ObservableCollection<string> processedFiles;

        public ConvertService()
        {
            processedFiles = new ObservableCollection<string>();
        }

        public ObservableCollection<string> RetrieveProcessedFiles()
        {
            return processedFiles;
        }

        public ICommand ConvertCommand { get; private set; }
    }
}
