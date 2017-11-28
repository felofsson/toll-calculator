using System;
using System.Collections.Generic;
using TollFeeCalculator;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

            long diffInMillies = date.Millisecond - intervalStart.Millisecond;
            long minutes = diffInMillies/1000/60;

            if (minutes <= 60)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    public bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicle.GetVehicleType().ToString());
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
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

    public Boolean IsTollFreeDate(DateTime date)
    {

        // Always TollFree on Saturday, Sunday and in July
        if (date.DayOfWeek == DayOfWeek.Saturday ||
            date.DayOfWeek == DayOfWeek.Sunday ||
            date.Month == 7 ||
            IsRecurrentTollFreeDate(date) ||
            IsYearSpecificTollFreeDates(date))
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

    public bool IsRecurrentTollFreeDate(DateTime myDate)
    {

        DateTime compareDate = new DateTime(2000, myDate.Month, myDate.Day);

        List<DateTime> recurrentTollFreeDates = new List<DateTime>();

        recurrentTollFreeDates.Add(new DateTime(2000, 1, 1));
        recurrentTollFreeDates.Add(new DateTime(2000, 4, 30));
        recurrentTollFreeDates.Add(new DateTime(2000, 5, 1));
        recurrentTollFreeDates.Add(new DateTime(2000, 6, 6));
        recurrentTollFreeDates.Add(new DateTime(2000, 12, 24));
        recurrentTollFreeDates.Add(new DateTime(2000, 12, 25));
        recurrentTollFreeDates.Add(new DateTime(2000, 12, 26));
        recurrentTollFreeDates.Add(new DateTime(2000, 12, 31));

        return recurrentTollFreeDates.Contains(compareDate);
    }

    private bool IsYearSpecificTollFreeDates(DateTime date)
    {
        List<DateTime> YearSpecificTollFreeDates = new List<DateTime>();

        // 2013
        YearSpecificTollFreeDates.Add(new DateTime(2013, 3, 28));
        YearSpecificTollFreeDates.Add(new DateTime(2013, 3, 29));
        YearSpecificTollFreeDates.Add(new DateTime(2013, 5, 8));
        YearSpecificTollFreeDates.Add(new DateTime(2013, 5, 9));
        YearSpecificTollFreeDates.Add(new DateTime(2013, 6, 5));
        YearSpecificTollFreeDates.Add(new DateTime(2013, 6, 21));
        YearSpecificTollFreeDates.Add(new DateTime(2013, 11, 1));

        // 2014
        // 2015
        // 2016
        // 2017 to be added

        return YearSpecificTollFreeDates.Contains(date);
    }

}