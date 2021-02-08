using P03_SalesDatabase.Data;
using System;

namespace P03_SalesDatabase
{
    class StartUp
    {
        static void Main(string[] args)
        {
            SalesContext saleContext = new SalesContext();

            using (saleContext)
            { 
                //..
            }
        }
    }
}
