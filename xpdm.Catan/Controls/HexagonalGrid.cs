using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using xpdm.Catan.Core.Board;

namespace xpdm.Catan.Controls
{
    class HexagonalGrid : Panel
    {
        static HexagonalGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HexagonalGrid), new FrameworkPropertyMetadata(typeof(HexagonalGrid)));
            HexagonalGrid.GameboardProperty = DependencyProperty.Register("Gameboard", typeof(Gameboard), typeof(HexagonalGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
            HexagonalGrid.RowProperty = DependencyProperty.RegisterAttached("Row", typeof(int), typeof(HexagonalGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            HexagonalGrid.ColumnProperty = DependencyProperty.RegisterAttached("Column", typeof(int), typeof(HexagonalGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            HexagonalGrid.GridEdgeLengthProperty = DependencyProperty.Register("GridEdgeLength", typeof(GridLength), typeof(HexagonalGrid), new FrameworkPropertyMetadata(GridLength.Auto, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        }

        #region Gameboard property
        public static readonly DependencyProperty GameboardProperty;

        public Gameboard Gameboard
        {
            get { return (Gameboard)GetValue(HexagonalGrid.GameboardProperty); }
            set { SetValue(HexagonalGrid.GameboardProperty, value); }
        }
        #endregion

        #region Row attached property
        public static readonly DependencyProperty RowProperty;

        public static int GetRow(DependencyObject o)
        {
            return (int) o.GetValue(HexagonalGrid.RowProperty);
        }
        public static void SetRow(DependencyObject o, int value)
        {
            o.SetValue(HexagonalGrid.RowProperty, value);
        }
        #endregion

        #region Column attached property
        public static readonly DependencyProperty ColumnProperty;

        public static int GetColumn(DependencyObject o)
        {
            return (int)o.GetValue(HexagonalGrid.ColumnProperty);
        }
        public static void SetColumn(DependencyObject o, int value)
        {
            o.SetValue(HexagonalGrid.ColumnProperty, value);
        }
        #endregion

        #region GridEdgeLength property
        public static readonly DependencyProperty GridEdgeLengthProperty;

        public GridLength GridEdgeLength
        {
            get { return (GridLength)GetValue(HexagonalGrid.GridEdgeLengthProperty); }
            set { SetValue(HexagonalGrid.GridEdgeLengthProperty, value); }
        }
        #endregion

        private static readonly Size InfiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
        private const double HexMinRadiusToMaxRadiusRatio = 0.8660254037844386467637231707;

        private Size _childSize = new Size(0, 0);

        protected override Size MeasureOverride(Size availableSize)
        {
            var hexRectBounds = new Size(0, 0);

            if (GridEdgeLength.IsAuto || GridEdgeLength.IsStar)
            {
                foreach (UIElement child in Children)
                {
                    child.Measure(InfiniteSize);
                    hexRectBounds.Width = Math.Max(hexRectBounds.Width, child.DesiredSize.Width);
                    hexRectBounds.Height = Math.Max(hexRectBounds.Height, child.DesiredSize.Height);
                }
            }
            else
            {
                hexRectBounds = new Size(GridEdgeLength.Value * 2, GridEdgeLength.Value * 2 * HexMinRadiusToMaxRadiusRatio);

                foreach (UIElement child in Children)
                {
                    child.Measure(hexRectBounds);
                }
            }

            _childSize = hexRectBounds;

            var cols = (Gameboard != null) ? Gameboard.Columns * 0.75 + 0.25 : 1;
            var rows = (Gameboard != null) ? Gameboard.Rows + 0.5 : 1;

            var resultSize = new Size(_childSize.Width * cols, _childSize.Height * rows);

            resultSize.Width = double.IsPositiveInfinity(availableSize.Width)
                                   ? resultSize.Width
                                   : availableSize.Width;

            resultSize.Height = double.IsPositiveInfinity(availableSize.Height)
                                    ? resultSize.Height
                                    : availableSize.Height;

            return resultSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement child in Children)
            {
                var childRow = HexagonalGrid.GetRow(child);
                var childColumn = HexagonalGrid.GetColumn(child);
                var xPos = childColumn*_childSize.Width*0.75;
                var yPos = _childSize.Height * (childRow + (Gameboard.IsOffset(childColumn) ? 0.5 : 0));
                child.Arrange(new Rect(xPos, yPos, _childSize.Width, _childSize.Height));
            }
            return arrangeSize;
        }
    }
}
