using System;

namespace Domain
{
    public class BaseInfo
    {
        public BaseInfo()
        {
        }

        public BaseInfo(Guid key, string title)
        {
            Key = key;
            Title = title;
        }

        public Guid Key { get; set; }
        public string Title { get; set; }
    }

    public class BaseSchedule
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}