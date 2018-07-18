using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExReaderPlus.View.Layout {
    public class ViewItemLayout : Panel {
        #region Properties
        bool _hasSepLine = false;
        int _sepLine = 0;
        #endregion

        #region Methods
        protected override Size MeasureOverride(Size availableSize) {
            double width = 0;
            Debug.WriteLine(availableSize.Width + "  " + availableSize.Height);

            foreach (var child in Children)
            {
                child.Measure(availableSize);
                if ((child is NavigationViewItemSeparator) && (child as NavigationViewItemSeparator).Name.Equals("End")) {
                    _sepLine = Children.IndexOf(child);
                    _hasSepLine = true;
                }
                if ((child is NavigationViewItem))
                {
                    Debug.WriteLine("sds");
                }
                width = child.DesiredSize.Width > width ? child.DesiredSize.Width : width;
               // Debug.WriteLine((child as Control).Width + "  " + (child as Control).Height);
            }

            Debug.WriteLine(availableSize.Width + "  " + availableSize.Height);
            return new Size(width, availableSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            double divideWidth = finalSize.Width / Children.Count;
            double t = 0, d = 0;
            Debug.WriteLine(finalSize.Width + "  " + finalSize.Height);
            foreach (var child in Children)
            {
                t = (divideWidth - child.DesiredSize.Width) / 2;
                child.Arrange(new Rect(t + d, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                d += divideWidth;
            }
            return finalSize;
        }
        #endregion

        #region Constructors
        #endregion
    }

}
