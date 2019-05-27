namespace Model

/// A Standard F# record of a Car.
type Car =
    {
        /// The Number of wheels on the car.
        Wheels:int
        /// The brand of the car.
        Brand:string
        /// The x/y of the car in meters
        Dimensions:float * float
    }

type Vehicle = 
      /// A car is a type of vehicle.
    | Motorcar of Car
      /// A bike is also a type of vehicle
    | Motorbike of Name:string * EngineSize:float

module Functions = 
    //Functions in full pascal case (like this: PascalCase; instead of the camelCase), since this is interop with C# and that's their tradition.
    /// Creates a car.
    let CreateCar wheels brand x y =
        { Wheels = wheels; Brand = brand; Dimensions = x, y }
    /// Creates a car with four wheels.
    let CreateFourWheeledCar = CreateCar 4