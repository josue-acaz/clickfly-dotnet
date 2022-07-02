using PagarmeCoreApi.Standard.Models;
namespace PagarmeCoreApi.Standard
{
    public partial class Configuration
    {


        //The base Uri for API calls
        public static string BaseUri = "https://api.pagar.me/core/v5";

        //The username to use with basic authentication
        //TODO: Replace the BasicAuthUserName with an appropriate value
        public static string BasicAuthUserName = "sk_test_1xMW8RaH9MH95vR6"; //pk_test_2ko9m6aik3idnbpG

        //The password to use with basic authentication
        //TODO: Replace the BasicAuthPassword with an appropriate value
        public static string BasicAuthPassword = "";

    }
}