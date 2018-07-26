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
    public class FlowLayout : Panel{
        #region Properties


        #endregion

        #region Methods
        protected override Size MeasureOverride(Size availableSize) {
            double height = 0;

            foreach (var child in Children)
            {
                child.Measure(availableSize);
                height = child.DesiredSize.Height > height ? child.DesiredSize.Height : height;
            }

            return new Size(availableSize.Width, height);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            double divideWidth = finalSize.Width / Children.Count;
            double t = 0, d = 0;
            foreach (var child in Children)
            {
                t = (divideWidth - child.DesiredSize.Width) / 2;
                child.Arrange(new Rect(t + d, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                Debug.WriteLine(new Rect(t + d, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                d += divideWidth;
            }
            return finalSize;
        }
        #endregion

        #region Constructors
        public FlowLayout() {

        }

        #endregion
    }

}
