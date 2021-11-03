using System;

namespace clickfly.ViewModels
{
    public class SessionInfo
    {
        public string Key { get; }
        public string Value { get; }

        public SessionInfo(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
