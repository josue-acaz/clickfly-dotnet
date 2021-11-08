using System;
using clickfly.ViewModels;

namespace clickfly
{
    public interface IInformer
    {
        Type GetValue<Type>(string key);
        void AddInfo(SessionInfo sessionInfo);
    }
}
