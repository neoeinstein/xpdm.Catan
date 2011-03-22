using System;
using C5;
using SCG=System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    public class ProductionChit
    {
        static readonly IList<ProductionChit> defaultChits = new GuardedList<ProductionChit>(new ArrayList<ProductionChit> 
        { 
            new ProductionChit(5, "A"),
            new ProductionChit(2, "B"),
            new ProductionChit(6, "C"),
            new ProductionChit(3, "D"),
            new ProductionChit(8, "E"),
            new ProductionChit(10, "F"),
            new ProductionChit(9, "G"),
            new ProductionChit(12, "H"),
            new ProductionChit(11, "I"),
            new ProductionChit(4, "J"),
            new ProductionChit(8, "K"),
            new ProductionChit(10, "L"),
            new ProductionChit(9, "M"),
            new ProductionChit(4, "N"),
            new ProductionChit(5, "O"),
            new ProductionChit(6, "P"),
            new ProductionChit(3, "Q"),
            new ProductionChit(11, "R"),
        });
        public static IList<ProductionChit> DefaultChits
        {
            get { return defaultChits; }
        }
        static readonly IList<ProductionChit> extendedChits = new GuardedList<ProductionChit>(new ArrayList<ProductionChit>
        { 
            new ProductionChit(2, "A"),
            new ProductionChit(5, "B"),
            new ProductionChit(4, "C"),
            new ProductionChit(6, "D"),
            new ProductionChit(3, "E"),
            new ProductionChit(9, "F"),
            new ProductionChit(8, "G"),
            new ProductionChit(11, "H"),
            new ProductionChit(11, "I"),
            new ProductionChit(10, "J"),
            new ProductionChit(6, "K"),
            new ProductionChit(3, "L"),
            new ProductionChit(8, "M"),
            new ProductionChit(4, "N"),
            new ProductionChit(8, "O"),
            new ProductionChit(10, "P"),
            new ProductionChit(11, "Q"),
            new ProductionChit(12, "R"),
            new ProductionChit(10, "S"),
            new ProductionChit(5, "T"),
            new ProductionChit(4, "U"),
            new ProductionChit(9, "V"),
            new ProductionChit(5, "W"),
            new ProductionChit(9, "X"),
            new ProductionChit(12, "Y"),
            new ProductionChit(3, "Za"),
            new ProductionChit(2, "Zb"),
            new ProductionChit(6, "Zc"),
        });
        public static IList<ProductionChit> ExtendedChits
        {
            get { return extendedChits; }
        }

        //private const char Pip = '•';
        private const char Pip = '●';

        public ProductionChit(int producesOn, string alphaOrder)
        {
            if (producesOn > 12 || producesOn < 2)
                throw new ArgumentOutOfRangeException("producesOn", producesOn, "Value is not possible on two six-sided dice.");
            this.ProducesOn = producesOn;
            this.AlphaOrder = alphaOrder;
            this.Pips = new string(Pip, PipValue);
        }

        public int ProducesOn
        {
            get;
            protected set;
        }

        public string Pips
        {
            get;
            protected set;
        }

        public int PipValue
        {
            get
            {
                return PipCount(ProducesOn);
            }
        }

        public static int PipCount(int rollValue)
        {
            return Math.Max(0, 6 - Math.Abs(rollValue - 7));
        }

        public bool IsCommon
        {
            get { return PipValue >= 5; }
        }

        public string AlphaOrder
        {
            get;
            protected set;
        }

        public override string ToString()
        {
            return string.Format("{{{0},{1}}}", ProducesOn, AlphaOrder);
        }
    }
}
