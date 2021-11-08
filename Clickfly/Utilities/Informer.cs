using System;
using System.Collections.Generic;
using System.Linq;
using clickfly.ViewModels;
using Newtonsoft.Json;

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

        public Type GetValue<Type>(string key)
        {
            SessionInfo sessionInfo = _sessionInfo.Where(sessionInfo => sessionInfo.Key == key).FirstOrDefault();

            if(sessionInfo == null)
            {
                return (Type)Activator.CreateInstance(typeof(Type));
            }

            return JsonConvert.DeserializeObject<Type>(sessionInfo.Value);
        }
    }
}