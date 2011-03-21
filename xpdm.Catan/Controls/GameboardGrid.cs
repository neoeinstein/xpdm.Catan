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
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:xpdm.Catan.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:xpdm.Catan.Controls;assembly=xpdm.Catan.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:GameboardGrid/>
    ///
    /// </summary>
    class GameboardGrid : Canvas
    {
        public static DependencyProperty GameboardProperty;

        static GameboardGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GameboardGrid), new FrameworkPropertyMetadata(typeof(GameboardGrid)));
            GameboardGrid.GameboardProperty = DependencyProperty.Register("Gameboard", typeof(Gameboard), typeof(GameboardGrid));
        }

        public Gameboard Gameboard
        {
            get { return (Gameboard)GetValue(GameboardGrid.GameboardProperty); }
            set { SetValue(GameboardGrid.GameboardProperty, value); }
        }



        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

    }
}
