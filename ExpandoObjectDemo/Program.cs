using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace ExpandoObjectDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var varNames = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("CardCode","C00001"),
                new KeyValuePair<string, object>("CardName","Mario")
            };
            var exObj = CreateExpandoObject(varNames);

            foreach (var prop in exObj)
                Console.WriteLine($"{prop.Key} - {prop.Value}");

            Console.WriteLine();

            var order = new Order
            {
                CardCode = "C00001",
                CardName = "Mario",
                Lines = new List<OrderLine>
                {
                    new OrderLine { Quantity = 5, PriceUnit = 100 },
                    new OrderLine { Quantity = 7, PriceUnit = 120 },
                    new OrderLine { Quantity = 8, PriceUnit = 100 },
                }
            };
            var newExpObj = ConvertDocToExpandoObject(order);
            var propertList = ((IDictionary<String, Object>)newExpObj).ToList();
            var propertlist1 = ((IDictionary<String, Object>)newExpObj)["Lines"] as List<OrderLine>;

            foreach (var prop in newExpObj)
                Console.WriteLine($"{prop.Key} - {prop.Value}");

            Console.WriteLine();
            Console.WriteLine("Clique para finalizar");
            Console.ReadKey();
        }

        /// <summary>
        /// Cria ExpandoObject
        /// </summary>
        /// <returns></returns>
        static ExpandoObject CreateExpandoObject(List<KeyValuePair<string, object>> varNames)
        {
            var exObj = new ExpandoObject();

            foreach (var varName in varNames)
                ((IDictionary<string, object>)exObj)[varName.Key] = varName.Value;

            return exObj;
        }

        /// <summary>
        /// Converte objeto para ExpandoObject
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static ExpandoObject ConvertDocToExpandoObject(object document)
        {
            var exObj = new ExpandoObject();

            foreach (var property in document.GetType().GetProperties())
                ((IDictionary<string, object>)exObj)[property.Name] = property.GetValue(document, null);

            return exObj;
        }
    }

    class Order
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }

        public List<OrderLine> Lines { get; set; }
    }

    class OrderLine
    {
        public int Quantity { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal Total { get { return Quantity * PriceUnit; } }
    }
}
