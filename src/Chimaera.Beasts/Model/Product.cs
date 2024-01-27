using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Beasts.Model
{
    public class Product
    {
        public int ProductID;
        public Design Design;
        public Type Type;
        public Color Color;
        public Size[] Sizes;
        public int PrintAuraID;
        public string ProofURL;
        public decimal UnitPrice;
        public bool Available;
        public DateTime DateCreated;
        public BulletPoint[] BulletPoints;
    }
}
