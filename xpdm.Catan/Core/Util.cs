using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core
{
    static class Util
    {
        public static int GCD(int a, int b)
        {
            Contract.Requires<ArgumentOutOfRangeException>(a != 0);
            Contract.Requires<ArgumentOutOfRangeException>(b != 0);
            Contract.Ensures(Contract.Result<int>() != 0);
            a = Math.Abs(a);
            b = Math.Abs(b);
            if (a == b || b > a && b % a == 0) return a;
            else if (a > b && a % b == 0) return b;

            int gcd = 1;
            while (b != 0)
            {
                gcd = b;
                b = a%b;
                a = gcd;
            }
            return gcd;
        }

        public static int LCM(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            a = checked((int) (a/GCD(a, b)));
            return checked((int) a*b);
        }
    }
}
