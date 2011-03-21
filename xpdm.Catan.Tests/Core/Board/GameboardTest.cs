using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using xpdm.Catan.Core.Board;

namespace xpdm.Catan.Tests.Core.Board
{
    [TestFixture]
    public class GameboardTest
    {
        [Test]
        public void GenerateABoard()
        {
            var board = new Gameboard(9, 8);
        }

        [Test]
        //Identity
        [Row(0, 0, 0, 0, 0)]
        //Commutative
        [Row(0, 0, 1, 0, 1)]
        [Row(0, 0, 0, 1, 1)]
        [Row(1, 0, 0, 0, 1)]
        [Row(0, 1, 0, 0, 1)]
        //Circle of 1
        [Row(1, 1, 1, 0, 1)]
        [Row(1, 1, 2, 1, 1)]
        [Row(1, 1, 2, 2, 1)]
        [Row(1, 1, 1, 2, 1)]
        [Row(1, 1, 0, 2, 1)]
        [Row(1, 1, 0, 1, 1)]
        //Circle of 2
        [Row(2, 2, 2, 0, 2)]
        [Row(2, 2, 3, 0, 2)]
        [Row(2, 2, 4, 1, 2)]
        [Row(2, 2, 4, 2, 2)]
        [Row(2, 2, 4, 3, 2)]
        [Row(2, 2, 3, 3, 2)]
        [Row(2, 2, 2, 4, 2)]
        [Row(2, 2, 1, 3, 2)]
        [Row(2, 2, 0, 3, 2)]
        [Row(2, 2, 0, 2, 2)]
        [Row(2, 2, 0, 1, 2)]
        [Row(2, 2, 1, 0, 2)]
        //Circle of 3
        [Row(3, 3, 3, 0, 3)]
        [Row(3, 3, 4, 1, 3)]
        [Row(3, 3, 5, 1, 3)]
        [Row(3, 3, 6, 2, 3)]
        [Row(3, 3, 6, 3, 3)]
        [Row(3, 3, 6, 4, 3)]
        [Row(3, 3, 6, 5, 3)]
        [Row(3, 3, 5, 5, 3)]
        [Row(3, 3, 4, 6, 3)]
        [Row(3, 3, 3, 6, 3)]
        [Row(3, 3, 2, 6, 3)]
        [Row(3, 3, 1, 5, 3)]
        [Row(3, 3, 0, 5, 3)]
        [Row(3, 3, 0, 4, 3)]
        [Row(3, 3, 0, 3, 3)]
        [Row(3, 3, 0, 2, 3)]
        [Row(3, 3, 1, 1, 3)]
        [Row(3, 3, 2, 1, 3)]
        //Circle of 4
        [Row(4, 4, 4, 0, 4)]
        [Row(4, 4, 5, 0, 4)]
        [Row(4, 4, 6, 1, 4)]
        [Row(4, 4, 7, 1, 4)]
        [Row(4, 4, 8, 2, 4)]
        [Row(4, 4, 8, 3, 4)]
        [Row(4, 4, 8, 4, 4)]
        [Row(4, 4, 8, 5, 4)]
        [Row(4, 4, 8, 6, 4)]
        [Row(4, 4, 7, 6, 4)]
        [Row(4, 4, 6, 7, 4)]
        [Row(4, 4, 5, 7, 4)]
        [Row(4, 4, 4, 8, 4)]
        [Row(4, 4, 3, 7, 4)]
        [Row(4, 4, 2, 7, 4)]
        [Row(4, 4, 1, 6, 4)]
        [Row(4, 4, 0, 6, 4)]
        [Row(4, 4, 0, 5, 4)]
        [Row(4, 4, 0, 4, 4)]
        [Row(4, 4, 0, 3, 4)]
        [Row(4, 4, 0, 2, 4)]
        [Row(4, 4, 1, 1, 4)]
        [Row(4, 4, 2, 1, 4)]
        [Row(4, 4, 3, 0, 4)]
        public void DistanceTest(int x1, int y1, int x2, int y2, int dist)
        {
            var result = Gameboard.DistanceBetween(x1, y1, x2, y2);
            Assert.LessThanOrEqualTo(0, result);
            Assert.AreEqual(dist, result);
        }

        [Test]
        [Row(0, false)]
        [Row(1, true)]
        [Row(2384729, true)]
        [Row(509302, false)]
        public void OffsetTest(int column, bool isOffset)
        {
            var result = Gameboard.IsOffset(column);
            Assert.AreEqual(isOffset, result);
        }
    }
}
