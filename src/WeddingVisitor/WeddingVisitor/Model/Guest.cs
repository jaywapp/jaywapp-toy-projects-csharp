using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WeddingVisitor.Model
{
    public interface IGuest
    {
        string Name { get; }
        int Money { get; }
        int AdultTicketCount { get; }
        int ChildTicketCount { get; }
        DateTime Time { get; }
    }

    public class Guest : ReactiveObject, IGuest
    {
        private string _name = "";
        private int _money = 0;
        private int _adultTicketCount = 0;
        private int _childTicketCount = 0;
        private DateTime _time = new DateTime();

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public int Money
        {
            get => _money;
            set => this.RaiseAndSetIfChanged(ref _money, value);
        }

        public int AdultTicketCount
        {
            get => _adultTicketCount;
            set => this.RaiseAndSetIfChanged(ref _adultTicketCount, value);
        }

        public int ChildTicketCount
        {
            get => _childTicketCount;
            set => this.RaiseAndSetIfChanged(ref _childTicketCount, value);
        }

        public DateTime Time
        {
            get => _time;
            set => this.RaiseAndSetIfChanged(ref _time, value);
        }

        public Guest()
        {
            Time = DateTime.Now;
        }

        public static (char, char) GetRange()
        {
            var type = typeof(IGuest);
            var properties = type.GetProperties();

            var last = 'A' + properties.Length - 1;

            return ('A', (char)last);
        }

        public IList<object> GetRow()
        {
            var type = typeof(IGuest);
            var properties = type.GetProperties();

            var row = new List<object>();

            foreach(var property in properties)
                row.Add((object)property.GetValue(this));

            return row;
        }

        public static Guest Create(IList<object> row)
        {
            var name = row[0].ToString();
            if (string.IsNullOrEmpty(name))
                return null;

            var money = int.TryParse(row.ElementAtOrDefault(1)?.ToString() ?? string.Empty, out int temp1) ? temp1 : 0;
            var adultCount = int.TryParse(row.ElementAtOrDefault(2)?.ToString() ?? string.Empty, out int temp2) ? temp2 : 0;
            var childCount = int.TryParse(row.ElementAtOrDefault(3)?.ToString() ?? string.Empty, out int temp3) ? temp3 : 0;
            var time = DateTime.TryParse(row.ElementAtOrDefault(4)?.ToString() ?? string.Empty, out DateTime temp4) ? temp4 : new DateTime();

            var guest = new Guest()
            {
                Name = name,
                Money = money,
                AdultTicketCount = adultCount,
                ChildTicketCount = childCount,
                Time = time,
            };

            return guest;
        }
    }
}
