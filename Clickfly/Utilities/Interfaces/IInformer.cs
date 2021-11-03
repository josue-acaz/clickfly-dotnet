using System;
using clickfly.ViewModels;

namespace clickfly
{
    public interface IInformer
    {
        SessionInfo GetValue(string key);
        void AddInfo(SessionInfo sessionInfo);
    }
}
