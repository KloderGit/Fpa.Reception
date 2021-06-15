using System;

namespace Domain
{
    public class KeyEntity
    {
        public KeyEntity(){}
        public KeyEntity(Guid key)
        {
            Key = key;
        }
        public Guid Key { get; set; }
    }

    public class BaseInfo : KeyEntity
    {
        public BaseInfo() : base()
        {
        }

        public BaseInfo(Guid key, string title)
           : base(key)
        {
            Title = title;
        }

    public string Title { get; set; }
}

public class BaseSchedule
{
    public int Id { get; set; }
    public string Title { get; set; }
}
}