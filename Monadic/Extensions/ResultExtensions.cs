using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Result And(this Result self, Result result)
        {
            if (self.Succeeded && result.Succeeded)
            {
                return Result.Success;
            }

            var errors = self
                .Errors
                .Concat(result.Errors)
                .ToArray();

            return Result.Failed(errors);
        }
    }
}
