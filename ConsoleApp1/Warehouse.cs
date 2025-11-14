using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Warehouse
    {
        public List<CarPart> Parts { get; set; }
        public decimal Balance { get; set; }

        public bool HasPart(string partName)
        {
            return Parts.Any(p => p.Name == partName && p.Quantity > 0);
        }

        public void RemovePart(string partName)
        {
            var part = Parts.FirstOrDefault(p => p.Name == partName);
            if (part != null && part.Quantity > 0)
                part.Quantity--;
        }
    }
}
