using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ITE.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace ITE.ConvertModule.ViewModels
{
    public class ConvertViewModel: NotificationObject
    {
        private readonly IConvertService convertService;
        private readonly DelegateCommand<object[]> convertCommand;
        private readonly ObservableCollection<string> processedFiles; 
        
        public ConvertViewModel(IConvertService convertService)
        {
            if(convertService == null)
                throw new ArgumentNullException("convertService");

            this.convertService = convertService;
            convertCommand = new DelegateCommand<object[]>(x => convertService.ConvertCommand.Execute(x[1]));

            processedFiles = convertService.RetrieveProcessedFiles();
        }

        public ObservableCollection<string> ProcessedFiles { get { return processedFiles; } }

        public string AllowableDataFormats { get { return DataFormats.FileDrop; } }

        public ICommand ProcessFilesCommand { get { return convertCommand; } }
    }
}
