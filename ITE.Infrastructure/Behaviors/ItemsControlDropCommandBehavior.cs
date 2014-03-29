using System;
using System.Windows;
using System.Windows.Controls;
using ITE.Infrastructure.Helpers;
using Microsoft.Practices.Prism.Commands;

namespace ITE.Infrastructure.Behaviors
{
    public class ItemsControlDropCommandBehavior : CommandBehaviorBase<ItemsControl>
    {
        public ItemsControlDropCommandBehavior(ItemsControl targetObject)
            : base(targetObject)
        {
            if (targetObject == null)
                throw new ArgumentNullException("targetObject");

            targetObject.AllowDrop = true;
            targetObject.DragEnter += TargetObject_DragEnter;
            targetObject.DragOver += TargetObject_DragOver;
            targetObject.DragLeave += TargetObject_DragLeave;
            targetObject.Drop += TargetObject_Drop;
        }

        private void TargetObject_Drop(object sender, DragEventArgs e)
        {
            if (Command != null)
            {
                object data = null;
                foreach (var dataFormat in AllowableDataFormats)
                {
                    data = e.Data.GetData(dataFormat);
                    if (data != null)
                        break;
                }
                if (data != null)
                {
                    var dropIndex = -1;
                    var dropContainer = sender as ItemsControl;
                    if (dropContainer != null)
                    {
                        var droppedOverItem = dropContainer.GetUIElement(e.GetPosition(dropContainer));
                        if (droppedOverItem != null)
                        {
                            dropIndex = dropContainer.ItemContainerGenerator.IndexFromContainer(droppedOverItem) + 1;
                            if (droppedOverItem.IsPositionAboveElement(e.GetPosition(droppedOverItem))) //if above
                            {
                                dropIndex = dropIndex - 1; //we insert at the index above it
                            }
                        }
                    }

                    var dragCommand = DragDrop.FindDragCommand(data);
                    if (dragCommand != null && dragCommand.CanExecute(data))
                    {
                        dragCommand.Execute(data);
                    }

                    object senderDataContext = null;
                    var frameworkElement = sender as FrameworkElement;
                    if (frameworkElement != null)
                        senderDataContext = frameworkElement.DataContext;

                    CommandParameter = new[] { senderDataContext, data, dropIndex };
                    ExecuteCommand();
                }
            }

            e.Handled = true;
        }

        private void TargetObject_DragLeave(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TargetObject_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = IsAllowDrop(e) ? DragDropEffects.Move : DragDropEffects.None;

            e.Handled = true;
        }

        private void TargetObject_DragEnter(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private bool IsAllowDrop(DragEventArgs e)
        {
            object parameter = null;
            foreach (var dataFormat in AllowableDataFormats.Return(x => x, new string[] { }))
            {
                parameter = e.Data.GetData(dataFormat);
                if (parameter != null)
                    break;
            }

            return parameter != null;
        }

        public string[] AllowableDataFormats { get; set; }
    }
}
