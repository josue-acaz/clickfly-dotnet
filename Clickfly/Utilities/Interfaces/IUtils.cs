using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly
{
    public interface IUtils
    {
        string RandomBytes(int length);
        string GetFieldsSql(GetFieldsSqlParams getFieldsSqlParams);
        string GetBulkSql(string[] ids);
        Installment[] GetInstallments(float subtotal, int max = 8);
        PaginationResult<Type> CreatePaginationResult<Type>(List<Type> data, PaginationFilter filter, int totalRecords);
        string GetBookingStatus(string chargeStatus);
        string RemoveWhiteSpaces(string value);
        bool IsValidEmail(string email);
        string GetExpCard(int exp_month, int exp_year);
        string DateTimeToSql(DateTime dateTime);
        string GenerateQRCode(string value);
    }
}
