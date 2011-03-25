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
    public class HexagonalGrid : Panel
    {
        static HexagonalGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HexagonalGrid), new FrameworkPropertyMetadata(typeof(HexagonalGrid)));
            HexagonalGrid.RowProperty = DependencyProperty.RegisterAttached("Row", typeof(int), typeof(HexagonalGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            HexagonalGrid.ColumnProperty = DependencyProperty.RegisterAttached("Column", typeof(int), typeof(HexagonalGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            HexagonalGrid.GridEdgeLengthProperty = DependencyProperty.Register("GridEdgeLength", typeof(GridLength), typeof(HexagonalGrid), new FrameworkPropertyMetadata(GridLength.Auto, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
            HexagonalGrid.OffsetColumnProperty = DependencyProperty.Register("OffsetColumn", typeof(int), typeof(HexagonalGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnOffsetColumnChanged, CoerceOffsetColumn), IsValidOffsetColumn);
            HexagonalGrid.RowsProperty = DependencyProperty.Register("Rows", typeof(int), typeof(HexagonalGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnRowsChanged, CoerceRows), IsValidRows);
            HexagonalGrid.ColumnsProperty = DependencyProperty.Register("Columns", typeof(int), typeof(HexagonalGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnColumnsChanged, CoerceColumns), IsValidColumns);
        }

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

        #region Rows property
        public static readonly DependencyProperty RowsProperty;

        public int Rows
        {
            get { return (int)GetValue(HexagonalGrid.RowsProperty); }
            set { SetValue(HexagonalGrid.RowsProperty, value); }
        }

        public static bool IsValidRows(object value)
        {
            int val = (int)value;
            return val >= 0;
        }

        private static object CoerceRows(DependencyObject o, object baseValue)
        {
            int val = (int)baseValue;
            return IsValidRows(val) ? val : 0;
        }

        protected static void OnRowsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region Columns property
        public static readonly DependencyProperty ColumnsProperty;

        public int Columns
        {
            get { return (int)GetValue(HexagonalGrid.ColumnsProperty); }
            set { SetValue(HexagonalGrid.ColumnsProperty, value); }
        }

        public static bool IsValidColumns(object value)
        {
            int val = (int)value;
            return val >= 0;
        }

        private static object CoerceColumns(DependencyObject o, object baseValue)
        {
            int val = (int)baseValue;
            return IsValidColumns(val) ? val : 0;
        }

        protected static void OnColumnsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region OffsetColumn property
        public static readonly DependencyProperty OffsetColumnProperty;

        public int OffsetColumn
        {
            get { return (int)GetValue(HexagonalGrid.OffsetColumnProperty); }
            set { SetValue(HexagonalGrid.OffsetColumnProperty, value); }
        }

        public static bool IsValidOffsetColumn(object value)
        {
            int val = (int)value;
            return val == 0 || val == 1;
        }

        private static object CoerceOffsetColumn(DependencyObject o, object baseValue)
        {
            int val = (int)baseValue;
            return val % 2;
        }

        protected static void OnOffsetColumnChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        private static readonly Size InfiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
        private const double HexMinRadiusToMaxRadiusRatio = 0.8660254037844386467637231707;

        private Size _childSize = new Size(0, 0);

        protected override Size MeasureOverride(Size availableSize)
        {
            var hexRectBounds = new Size(0, 0);
            var maxRow = 1;
            var maxColumn = 1;

            if (GridEdgeLength.IsAuto || GridEdgeLength.IsStar)
            {
                foreach (UIElement child in Children)
                {
                    child.Measure(InfiniteSize);
                    hexRectBounds.Width = Math.Max(hexRectBounds.Width, child.DesiredSize.Width);
                    hexRectBounds.Height = Math.Max(hexRectBounds.Height, child.DesiredSize.Height);
                    maxRow = Math.Max(maxRow, HexagonalGrid.GetRow(child));
                    maxColumn = Math.Max(maxColumn, HexagonalGrid.GetColumn(child));
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

            var cols = (Columns == 0 ? maxColumn : Columns) * 0.75 + 0.25;
            var rows = (Rows == 0 ? maxRow : Rows) + 0.5;

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
                var yPos = _childSize.Height * (childRow + (this.IsOffsetColumn(childColumn) ? 0.5 : 0));
                child.Arrange(new Rect(xPos, yPos, _childSize.Width, _childSize.Height));
            }
            return arrangeSize;
        }

        public bool IsOffsetColumn(int column)
        {
            return (column % 2 == OffsetColumn);
        }
    }
}
