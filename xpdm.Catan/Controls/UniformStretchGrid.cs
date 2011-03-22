using System;
using System.Diagnostics;
using C5;
using SCG = System.Collections.Generic;
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
using xpdm.Catan.Core;

namespace xpdm.Catan.Controls
{
    public class UniformStretchGrid : ItemsControl
    {
        //Multiplier = items % columns
        private ItemsPresenter ItemsPresenter;

        private int _majorColumns = 0;
        private int _minorRows = 0;
        private int _majMultiplier = 1;

        static UniformStretchGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UniformStretchGrid), new FrameworkPropertyMetadata(typeof(UniformStretchGrid)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ItemsPresenter = (ItemsPresenter) GetTemplateChild("ItemsPresenter");
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            UpdateMeasure();
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            ArrangePanel();
            return base.ArrangeOverride(arrangeBounds);
        }

        protected void UpdateMeasure()
        {
            int items = 0;
            if (ItemsPresenter != null)
            {
                items = VisualTreeHelper.GetChildrenCount(ItemsPresenter);
                RecalculateRowColumns();
            }

            for (int i = 0; i < items; ++i)
            {
                var target = VisualTreeHelper.GetChild(ItemsPresenter, i) as Grid;
                if (target != null)
                {
                    target.RowDefinitions.Clear();
                    for (int r = 0; r < (ChildrenFlow == Orientation.Horizontal ? _minorRows : _majorColumns * _majMultiplier); ++r)
                        target.RowDefinitions.Add(new RowDefinition());

                    target.ColumnDefinitions.Clear();
                    for (int c = 0; c < (ChildrenFlow != Orientation.Horizontal ? _minorRows : _majorColumns * _majMultiplier); ++c)
                        target.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
        }

        protected void RecalculateRowColumns()
        {
            switch (Items.Count)
            {
                case 0:
                    _majorColumns = 0;
                    _minorRows = 0;
                    break;
                case 1:
                    _majorColumns = 1;
                    _minorRows = 1;
                    break;
                case 2:
                    _majorColumns = 2;
                    _minorRows = 1;
                    break;
                case 3:
                case 4:
                    _majorColumns = 2;
                    _minorRows = 2;
                    break;
                case 5:
                case 6:
                    _majorColumns = 3;
                    _minorRows = 2;
                    break;
                case 7:
                case 8:
                case 9:
                    _majorColumns = 3;
                    _minorRows = 3;
                    break;
                default:
                    _majorColumns = Convert.ToInt32(Math.Ceiling(Math.Sqrt(Items.Count)));
                    _minorRows = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Items.Count) / _majorColumns));
                    break;
            }

            int itemsInLastMajor = (_majorColumns > 0 ? Items.Count % _majorColumns : 0);

            _majMultiplier = (itemsInLastMajor == 0) ? 1 : Util.LCM(_majorColumns, itemsInLastMajor) / _majorColumns;
        }

        protected void ArrangePanel()
        {
            RecalculateRowColumns();

            int itemsInLastMajor = (_majorColumns > 0 ? Items.Count % _majorColumns : 0);

            int fullMajors = _minorRows - (itemsInLastMajor == 0 ? 0 : 1);
            int itemsInFullMajors = fullMajors * _majorColumns;

            Debug.Assert(itemsInLastMajor == Items.Count - itemsInFullMajors);

            for (int i = 0; i < itemsInFullMajors; ++i)
            {
                var c = this.GetItemContainerForObject(Items[i]);
                if (c != null)
                {
                    ArrangeItem(c, i / _majorColumns, (i % _majorColumns) * _majMultiplier, _majMultiplier);
                }
            }

            for (int i = 0; i < itemsInLastMajor; ++i)
            {
                var c = this.GetItemContainerForObject(Items[itemsInFullMajors + i]);
                if (c != null)
                {
                    ArrangeItem(c, _minorRows - 1, i * _majMultiplier * _majorColumns / itemsInLastMajor, _majMultiplier * _majorColumns / itemsInLastMajor);
                }
            }
        }

        protected void ArrangeItem(ItemContainer c, int majorPos, int minorPos, int majorSpan)
        {
            c.SetValue((ChildrenFlow == Orientation.Horizontal ? Grid.ColumnSpanProperty : Grid.RowSpanProperty), majorSpan);
            c.SetValue((ChildrenFlow != Orientation.Horizontal ? Grid.ColumnProperty : Grid.RowProperty), majorPos);
            c.SetValue((ChildrenFlow == Orientation.Horizontal ? Grid.ColumnProperty : Grid.RowProperty), minorPos);
        }

        #region ChildrenFlow property
        public static readonly DependencyProperty ChildrenFlowProperty = DependencyProperty.Register("ChildrenFlow",
                                                                                             typeof (Orientation),
                                                                                             typeof (UniformStretchGrid),
                                                                                             new FrameworkPropertyMetadata
                                                                                                 (Orientation.Horizontal,
                                                                                                  UniformStretchGrid.
                                                                                                      OnChildrenFlowChanged));

        public Orientation ChildrenFlow
        {
            get { return (Orientation) GetValue(UniformStretchGrid.ChildrenFlowProperty); }
            set { SetValue(UniformStretchGrid.ChildrenFlowProperty, value); }
        }

        protected static void OnChildrenFlowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var g = d as UniformStretchGrid;
            if (g != null)
                g.OnChildrenFlowChanged((Orientation)e.OldValue, (Orientation)e.NewValue);
        }

        internal virtual void OnChildrenFlowChanged(Orientation oldValue, Orientation newValue)
        {
            if (oldValue != newValue)
            {
                ArrangePanel();
            }
        }
        #endregion

        #region VerticalFlowDirection property
        public static readonly DependencyProperty VerticalFlowDirectionProperty = DependencyProperty.Register("VerticalFlowDirection",
                                                                                             typeof(VerticalFlowDirection),
                                                                                             typeof(UniformStretchGrid),
                                                                                             new FrameworkPropertyMetadata
                                                                                                 (Controls.VerticalFlowDirection.TopToBottom,
                                                                                                  UniformStretchGrid.
                                                                                                      OnVerticalFlowDirectionChanged));

        public VerticalFlowDirection VerticalFlowDirection
        {
            get { return (VerticalFlowDirection)GetValue(UniformStretchGrid.VerticalFlowDirectionProperty); }
            set { SetValue(UniformStretchGrid.VerticalFlowDirectionProperty, value); }
        }

        protected static void OnVerticalFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var g = d as UniformStretchGrid;
            if (g != null)
                g.OnVerticalFlowDirectionChanged((VerticalFlowDirection)e.OldValue, (VerticalFlowDirection)e.NewValue);
        }

        internal virtual void OnVerticalFlowDirectionChanged(VerticalFlowDirection oldValue, VerticalFlowDirection newValue)
        {
            if (oldValue != newValue)
            {
                ArrangePanel();
            }
        }
        #endregion

        #region ItemContainerStyle property overrides
        protected override void OnItemContainerStyleChanged(Style oldValue, Style newValue)
        {
            if (oldValue != newValue)
            {
                foreach (object o in Items)
                {
                    var c = this.GetItemContainerForObject(o);
                    if (c != null && (c.Style == null || oldValue == c.Style))
                    {
                        c.Style = newValue;
                    }
                }
            }
        }
        #endregion

        private IDictionary<object, ItemContainer> _objectToItemContainer;

        private ItemContainer GetItemContainerForObject(object value)
        {
            ItemContainer item = value as ItemContainer;
            if (item == null)
            {
                this.ObjectToItemContainer.Find(value, out item);
            }
            return item;
        }

        private IDictionary<object, ItemContainer> ObjectToItemContainer
        {
            get
            {
                if (this._objectToItemContainer == null)
                {
                    this._objectToItemContainer = new HashDictionary<object, ItemContainer>();
                }
                return this._objectToItemContainer;
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            ItemContainer item = new ItemContainer();
            if (this.ItemContainerStyle != null)
            {
                item.Style = this.ItemContainerStyle;
            }
            return item;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ItemContainer);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            ItemContainer item2 = element as ItemContainer;
            if (item2 != item)
            {
                bool flag = true;
                if (base.ItemTemplate != null)
                {
                    item2.ContentTemplate = base.ItemTemplate;
                }
                else if (!string.IsNullOrEmpty(base.DisplayMemberPath))
                {
                    Binding binding = new Binding(base.DisplayMemberPath);
                    item2.SetBinding(ContentControl.ContentProperty, binding);
                    flag = false;
                }
                if (flag)
                {
                    item2.Content = item;
                }
                this.ObjectToItemContainer[item] = item2;

                this.ArrangePanel();
            }
            if (this.ItemContainerStyle != null && item2.Style == null)
            {
                item2.Style = this.ItemContainerStyle;
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            ItemContainer item2 = element as ItemContainer;
            if (item == null)
            {
                item = (item2.Content == null) ? item2 : item2.Content;
            }
            if (item2 != item)
            {
                this.ObjectToItemContainer.Remove(item);
                UpdateMeasure();
            }
        }
    }
}
