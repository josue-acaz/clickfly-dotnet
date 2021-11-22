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
using clickfly.Helpers;
using FluentValidation;
using FluentValidation.Results;
using clickfly.Models;

namespace clickfly.Services
{
    public class BaseService
    {
        protected readonly AppSettings _appSettings;
        protected readonly INotificator _notificator;
        protected readonly IInformer _informer;
        protected readonly OneSignalClient _oneSignalClient;
        protected readonly IUtils _utils;

        public BaseService(IOptions<AppSettings> appSettings, INotificator notificator, IInformer informer, IUtils utils)
        {
            _appSettings = appSettings.Value;
            _notificator = notificator;
            _informer = informer;
            _oneSignalClient = new OneSignalClient(_appSettings.OneSignalAppKey);
            _utils = utils;
        }

        protected void Notify(string message)
        {
            Notification notification = new Notification(message);
            _notificator.HandleNotification(notification);
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach(ValidationFailure validationFailure in validationResult.Errors)
            {
                Notify(validationFailure.ErrorMessage);
            }
        }

        protected bool ExecuteValitation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : BaseEntity
        {
            ValidationResult validationResult = validation.Validate(entity);
            if (validationResult.IsValid)
            {
                return true;
            }

            Notify(validationResult);

            return false;
        }
    }
}
