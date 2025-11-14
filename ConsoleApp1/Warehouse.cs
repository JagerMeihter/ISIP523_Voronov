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

        public void AddPart(CarPart part) { }
        public bool HasPart(string partName) { return false; }
        public void RemovePart(string partName) { }
    }
}
