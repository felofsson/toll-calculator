using System;

namespace TollFeeCalculator
{
    public class Car : Vehicle
    {
        VehicleType Vehicle.GetVehicleType()
        {
            return VehicleType.Car;
        }
    }
}