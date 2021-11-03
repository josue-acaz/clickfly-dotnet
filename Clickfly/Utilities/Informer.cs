using System;
using System.Collections.Generic;
using System.Linq;
using clickfly.ViewModels;

namespace clickfly
{
    public class Informer : IInformer
    {
        private List<SessionInfo> _sessionInfo;

        public Informer()
        {
            _sessionInfo = new List<SessionInfo>();
        }
        
        public void AddInfo(SessionInfo sessionInfo)
        {
            _sessionInfo.Add(sessionInfo);
        }

        public SessionInfo GetValue(string key)
        {
            SessionInfo sessionInfo = _sessionInfo.Where(sessionInfo => sessionInfo.Key == key).FirstOrDefault();

            if(sessionInfo == null)
            {
                return new SessionInfo("", "");
            }

            return sessionInfo;
        }
    }
}