using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ITE.Infrastructure.Helpers;
using ITE.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace ITE.ConvertModule.ViewModels
{
    public class ConvertViewModel: NotificationObject
    {
        private readonly IConvertService convertService;
        private readonly DelegateCommand<string[]> processFilesCommand; 

        public ConvertViewModel(IConvertService convertService)
        {
            if(convertService ==null)
                throw new ArgumentNullException("convertService");

            this.convertService = convertService;
            processFilesCommand = new DelegateCommand<string[]>(ProcessFiles);
        }

        private void ProcessFiles(string[] files)
        {
            foreach (var imageFile in files.Where(x =>
                FileHelper.IsImageFile(x) && convertService.ConvertCommand.CanExecute(x))
                .ToList())
            {
                convertService.ConvertCommand.Execute(imageFile);
            }
        }

        public string AllowableDataFormats { get { return DataFormats.FileDrop; } }

        public ICommand ProcessFilesCommand { get { return processFilesCommand; } }
    }
}
