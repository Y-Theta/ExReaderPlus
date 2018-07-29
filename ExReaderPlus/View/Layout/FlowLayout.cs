using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExReaderPlus.View.Layout {
    public class FlowLayout : Panel{
        #region Properties
        private int _cellcounts;

        private List<List<UIElement>> _panelcells;

        public double CellWidth {
            get { return (double)GetValue(CellWidthProperty); }
            set { SetValue(CellWidthProperty, value); }
        }
        public static readonly DependencyProperty CellWidthProperty =
            DependencyProperty.Register("CellWidth", typeof(double),
                typeof(FlowLayout), new PropertyMetadata(360.0));


        public double CellHeight {
            get { return (double)GetValue(CellHeightProperty); }
            set { SetValue(CellHeightProperty, value); }
        }
        public static readonly DependencyProperty CellHeightProperty =
            DependencyProperty.Register("CellHeight", typeof(double), 
                typeof(FlowLayout), new PropertyMetadata(120.0));
        #endregion

        #region Methods
        protected override Size MeasureOverride(Size availableSize) {
            _panelcells.Clear();
            if ((int)(availableSize.Width /  CellWidth) != _cellcounts) 
                _cellcounts = (int)(availableSize.Width / CellWidth);

            int k = 0;
            List<UIElement> cellline = null;
            foreach (var child in Children)
            {
                if (k == 0)
                    cellline = new List<UIElement>();
                child.Measure(availableSize);
                cellline.Add(child);
                k = k + 1 > _cellcounts ? 0 : k + 1;
                if (k == 0)
                    _panelcells.Add(cellline);
            }

            if (_panelcells.Count * _cellcounts < Children.Count)
                _panelcells.Add(cellline);

            return new Size(availableSize.Width, CellHeight * _panelcells.Count);
        }

        protected override Size ArrangeOverride(Size finalSize) {
          
            foreach (var child in _panelcells)
            {
                int line = _panelcells.IndexOf(child);
                double start = 0.0;
                foreach (var ui in child) {
                    ui.Arrange(new Rect(start, line * CellHeight, finalSize.Width / (_cellcounts + 1), CellHeight));
                    start += (finalSize.Width / (_cellcounts + 1));
                }
            }

            return finalSize;
        }
        #endregion

        #region Constructors
        public FlowLayout() {
            _panelcells = new List<List<UIElement>>();
        }

        #endregion
    }

}
