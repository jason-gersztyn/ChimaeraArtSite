using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChimLib.Utils
{
    public enum Status
    {
        Pending = 1,
        Paid = 2,
        Shipped = 3,
        Canceled = 4,
        Error = 5,
        Fulfilling = 6,
        Printing = 7,
        Unpaid = 8,
        Hold = 9,
        Ordered = 10
    }

    public enum Size
    {
        sml = 1,
        med = 2,
        lrg = 3,
        xlg = 4,
        xxl = 5,
        xxxl = 6,
        xxxxl = 7,
        xxxxxl = 8,
        xsl = 9,
        xxs = 10
    }

    public enum DiscountType
    {
        Calculated = 1,
        Flat = 2
    }
}