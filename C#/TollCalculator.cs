using System;
using System.Collections.Generic;
using TollFeeCalculator;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day. 
     *      Assumes a sorted list of dates, earliest first
     * @return - the total toll fee for that day
     */


    // Class variables
    List<DateTime> recurrentTollFreeDates = new List<DateTime> 
                               {new DateTime(2000, 1, 1),
                                new DateTime(2000, 4, 30),
                                new DateTime(2000, 5, 1),
                                new DateTime(2000, 6, 6),
                                new DateTime(2000, 12, 24),
                                new DateTime(2000, 12, 25),
                                new DateTime(2000, 12, 26),
                                new DateTime(2000, 12, 31)}; 

    // 2014, 2015, 2016, 2017 too be added to this list
    List<DateTime> YearSpecificTollFreeDates = new List<DateTime>
                               {new DateTime(2013, 3, 28),
                                new DateTime(2013, 3, 29),
                                new DateTime(2013, 5, 8),
                                new DateTime(2013, 5, 9),
                                new DateTime(2013, 6, 5),
                                new DateTime(2013, 6, 21),
                                new DateTime(2013, 11, 1)}; 

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];

        int totalFee = 0;

        long lastPassage = intervalStart.Millisecond;
        totalFee += GetTollFee(intervalStart, vehicle); // Always count the first

        double timeSinceLastPassage;

        foreach (DateTime date in dates)
        {
            timeSinceLastPassage = (date.Millisecond - lastPassage)/(60*1000);

            if (timeSinceLastPassage > 60) {
                totalFee += GetTollFee(date, vehicle);
                lastPassage = date.Millisecond;
            }

        }

        if (totalFee > 60) totalFee = 60;

        return totalFee;

    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicle.GetVehicleType().ToString());
    }

    private int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
    }

    private Boolean IsTollFreeDate(DateTime date)
    {

        // Always TollFree on Saturday, Sunday and in July
        if (date.DayOfWeek == DayOfWeek.Saturday ||
            date.DayOfWeek == DayOfWeek.Sunday ||
            date.Month == 7 ||
            recurrentTollFreeDates.Contains(date) ||
            YearSpecificTollFreeDates.Contains(date)
        {
            return true;
        }
        else{
            return false;
        }

    }

    private enum TollFreeVehicles
    {
        Motorbike,
        Tractor,
        Emergency,
        Diplomat,
        Foreign,
        Military
    }

}