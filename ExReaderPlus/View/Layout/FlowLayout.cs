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


        public double MinCellWidth {
            get { return (double)GetValue(MinCellWidthProperty); }
            set { SetValue(MinCellWidthProperty, value); }
        }
        public static readonly DependencyProperty MinCellWidthProperty =
            DependencyProperty.Register("MinCellWidth",
                typeof(double), typeof(FlowLayout), new PropertyMetadata(240.0));

        public double CellHeight {
            get { return (double)GetValue(CellHeightProperty); }
            set { SetValue(CellHeightProperty, value); }
        }
        public static readonly DependencyProperty CellHeightProperty =
            DependencyProperty.Register("CellHeight", typeof(double), 
                typeof(FlowLayout), new PropertyMetadata(120.0));

        public double CellScale {
            get { return (double)GetValue(CellScaleProperty); }
            set { SetValue(CellScaleProperty, value); }
        }
        public static readonly DependencyProperty CellScaleProperty =
            DependencyProperty.Register("CellScale", typeof(double), 
                typeof(FlowLayout), new PropertyMetadata(3.0));
        #endregion

        #region Methods
        protected override Size MeasureOverride(Size availableSize) {
            _panelcells.Clear();
            if ((int)(availableSize.Width / CellWidth) + 1 != _cellcounts )
                _cellcounts = (int)(availableSize.Width / CellWidth) + 1;
            if ((availableSize.Width / _cellcounts) < MinCellWidth)
                _cellcounts -= 1;

            int k = 1;
            List<UIElement> cellline = null;
            foreach (var child in Children)
            {
                if (k == 1)
                    cellline = new List<UIElement>();
                child.Measure(availableSize);
                cellline.Add(child);
                k = k + 1 > _cellcounts ? 1 : k + 1;
                if (k == 1)
                    _panelcells.Add(cellline);
            }

            if (cellline != null && cellline[0].DesiredSize.Height > CellHeight)
                CellHeight = cellline[0].DesiredSize.Height;

            if ( _panelcells.Count * _cellcounts < Children.Count) 
                _panelcells.Add(cellline);

            if (availableSize.Width / _cellcounts > CellScale * CellHeight)
                return new Size(availableSize.Width, availableSize.Width / (_cellcounts * CellScale) * _panelcells.Count);
            else
                return new Size(availableSize.Width, CellHeight * _panelcells.Count);
        }

        protected override Size ArrangeOverride(Size finalSize) {
          
            foreach (var child in _panelcells)
            {
                int line = _panelcells.IndexOf(child);
                double start = 0.0;
                foreach (var ui in child) {
                    if (finalSize.Width / _cellcounts > CellScale * CellHeight)
                        ui.Arrange(new Rect(start, line * (finalSize.Width / (_cellcounts * CellScale)), finalSize.Width / _cellcounts, (finalSize.Width / (_cellcounts * CellScale))));
                    else
                        ui.Arrange(new Rect(start, line * CellHeight, finalSize.Width / _cellcounts, CellHeight));
                    start += (finalSize.Width / _cellcounts);
                }
            }

            if (finalSize.Width / _cellcounts > CellScale * CellHeight)
                return new Size(finalSize.Width, finalSize.Width / CellScale * _panelcells.Count);
            else
                return new Size(finalSize.Width, CellHeight * _panelcells.Count);
        }
        #endregion

        #region Constructors
        public FlowLayout() {
            _panelcells = new List<List<UIElement>>();
        }
        #endregion
    }
}
