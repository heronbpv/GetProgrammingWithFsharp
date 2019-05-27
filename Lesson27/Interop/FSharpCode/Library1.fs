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