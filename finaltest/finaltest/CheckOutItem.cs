using FinalProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class CheckOutItem
    {
        public LibraryItem Item { get; set; }
        public int DaysDue { get; set; }
        public int DaysLate { get; set; }
        public CheckOutItem(LibraryItem item, int daysDue, int daysLate)
        {
            Item = item;
            DaysDue = daysDue;
            DaysLate = daysLate;
            if (item.Type == "Book")
            {
                DaysDue = 7;
            }
            if (item.Type == "EBook")
            {
                DaysDue = 6;
            }
            if (item.Type == "Audiobook")
            {
                DaysDue = 5;
            }
            if (item.Type == "DVD")
            {
                DaysDue = 3;
            }
        }

        public double LateFee()
        {
            double lateFee = Item.DailyLateFee * DaysLate;
            if (DaysLate <= 0)
            {
                return 0;
            }
            return lateFee;
        }
        public override string ToString()
        {
            return ($"ID: {Item.ID}  TITLE: {Item.Title}  TYPE: {Item.Type}  DAYS LATE: {DaysLate}   ESTIMATED FEE: ${LateFee():F2}");
        }
    }
}
