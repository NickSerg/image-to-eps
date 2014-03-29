using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace ITE.Infrastructure.Behaviors
{
    public class FrameworkElementDragCommandBehavior : CommandBehaviorBase<FrameworkElement>
    {
        private bool isMouseClicked;

        public FrameworkElementDragCommandBehavior(FrameworkElement targetObject)
            : base(targetObject)
        {
            if (targetObject == null)
                throw new ArgumentNullException("targetObject");

            targetObject.MouseLeftButtonDown += TargetObject_MouseLeftButtonDown;
            targetObject.MouseLeftButtonUp += TargetObject_MouseLeftButtonUp;
            targetObject.MouseLeave += TargetObject_MouseLeave;
        }

        private void TargetObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isMouseClicked)
            {
                var data = new DataObject();
                data.SetData(TargetObject.DataContext.GetType(), TargetObject.DataContext);
                System.Windows.DragDrop.DoDragDrop(TargetObject, data, DragDropEffects.Move);
            }
            isMouseClicked = false;
        }

        private void TargetObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = false;
        }

        private void TargetObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = true;
        }
    }
}
