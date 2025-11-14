using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CarPart
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }


        public CarPart(string name, decimal price, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название детали не может быть пустым");
            if (price <= 0)
                throw new ArgumentException("Цена должна быть положительной");
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным");

            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}
