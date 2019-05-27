using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var car = new Car(4, "Supacar", Tuple.Create<double, double>(1.5, 3.5));
            var wheels = car.Wheels;
            var brand = car.Brand;
            var x = car.Dimensions.Item1;
            var y = car.Dimensions.Item2;

            Console.WriteLine("Car -> Brand {0}; Wheels {1}; Dimension(x, y) ({2}, {3})", brand, wheels, x, y);

            var vehicle = Vehicle.NewMotorbike("Supabike", 12.0);
            //So, in order to access the properties of a Motorbike type, I have to first explicitly cast to it, since the assignment above leads to a vehicle.
            var bike = vehicle.IsMotorbike ? vehicle as Vehicle.Motorbike : null; //Yes, this will lead to null eventualy in other circunstances; Never in production!
            var name = bike.Name;
            var engineSize = bike.EngineSize;

            Console.WriteLine("Motorbike -> Name {0}; Engine Size {1}", name, engineSize);

            var funcCar = Functions.CreateCar(6, "ATV", 2.0, 6.0);

            Console.WriteLine("funcCar -> Brand {0}; Wheels {1}; Dimension(x, y) ({2}, {3})", funcCar.Brand, funcCar.Wheels, funcCar.Dimensions.Item1, funcCar.Dimensions.Item2);

            //Each invoke is an argument being passed, and in the proper order.
            var partFuncCar = Functions.CreateFourWheeledCar.Invoke("Disfunctional").Invoke(2.1).Invoke(2.1);

            Console.WriteLine("desfuncCar -> Brand {0}; Wheels {1}; Dimension(x, y) ({2}, {3})", partFuncCar.Brand, partFuncCar.Wheels, partFuncCar.Dimensions.Item1, partFuncCar.Dimensions.Item2);
        }
    }
}
