using System;
using clickfly.ViewModels;

namespace clickfly.Helpers
{
    public interface IInformer
    {
        Type GetValue<Type>(string key);
        void AddInfo(SessionInfo sessionInfo);
    }
}
