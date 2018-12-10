using System;
using System.Collections.Generic;

namespace Monadic.Extensions
{
    public static class ResultExtensions
    {
        public static void ThrowIfUnsuccessful(this Result result, Func<IEnumerable<Error>, Exception> ex)
        {
            if (!result.Succeeded)
            {
                throw ex(result.Errors);
            }
        }
    }
}
