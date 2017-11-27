using System;

namespace TollFeeCalculator
{
    public interface Vehicle
    {
        VehicleType GetVehicleType();
    }

    public enum VehicleType{
        Car,
        Motorbike,
        Ambulance
    }
}