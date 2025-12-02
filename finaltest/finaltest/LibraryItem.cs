using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class LibraryItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public double DailyLateFee { get; set; }
        public LibraryItem(int Id, string title, string type, double dailyLateFee)
        {
            ID = Id;
            Title = title;
            Type = type;
            DailyLateFee = dailyLateFee;
        }
        public override string ToString()
        {
            return $"ID: {ID}  TITLE: {Title}  TYPE: {Type}  DAILY LATE FEE: ${DailyLateFee:F2}";
        }

    }
}
