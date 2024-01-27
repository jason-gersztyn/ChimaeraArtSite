using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChimLib.Database;
using ChimLib.Utils;

namespace ChimTests
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicUtils.SendUpdate("4");
        }
    }
}