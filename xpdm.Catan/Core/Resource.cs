using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core
{
    [Flags]
    enum Resource
    {
        None = 0,
        Wood = 1,
        Wheat = 2,
        Sheep = 4,
        Ore = 8,
        Brick = 16,
        AnyResource = Wood | Wheat | Sheep | Ore | Brick,
        Cloth = 32,
        Paper = 64,
        Coin = 128,
        AnyCommodity = Cloth | Paper | Coin,
        Fish = 256,
        Gold = 512,
    }

    static class EnumExtensions
    {
        public static bool IncludesAny(this Enum res, Enum other)
        {
            return res.HasFlag(other);
        }

        public static bool IncludesAll(this Enum res, Enum other)
        {
            return (Convert.ToInt32(res) & Convert.ToInt32(other)) == Convert.ToInt32(res);
        }
    }
}
