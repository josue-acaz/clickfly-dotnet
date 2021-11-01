using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;
using OneSignal.RestAPIv3.Client;
using OneSignal.RestAPIv3.Client.Resources.Notifications;
using OneSignal.RestAPIv3.Client.Resources;
using System.Collections.Generic;

namespace clickfly.Services
{
    public class BaseService
    {
        protected AppSettings _appSettings { get; set; }
        protected IUtils _utils { get; set; }
        protected OneSignalClient _oneSignalClient { get; set; }

        public BaseService(IOptions<AppSettings> appSettings, IUtils utils)
        {
            _appSettings = appSettings.Value;
            _utils = utils;
            _oneSignalClient = new OneSignalClient(_appSettings.OneSignalAppKey);
        }
    }
}
