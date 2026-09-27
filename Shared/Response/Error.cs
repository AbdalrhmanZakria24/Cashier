using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Response
{
    public record Error(string Code, string Description, ErrorType Type)
    {
        public static readonly Error None =
            new(string.Empty, string.Empty, ErrorType.None);

        public static readonly Error NullValue =
            new("General.Null", "Value cannot be null.", ErrorType.Failure);

        public static readonly Error NotFound =
            new("General.Not found", "cannot found Value.", ErrorType.NotFound);

        public static readonly Error AlreadyExist =
            new("General.AlreadyExist", "Resource already exists.", ErrorType.Conflict);

        public static readonly Error Validation =
            new("General.Validation", "One or more validation errors occurred.", ErrorType.Validation);

        public static readonly Error BadRequest =
            new("General.BadRequest", "Bad request.", ErrorType.Failure);

        public static readonly Error Unauthorized =
            new("General.Unauthorized", "Unauthorized.", ErrorType.Unauthorized);

        public static readonly Error Forbidden =
            new("General.Forbidden", "Forbidden.", ErrorType.Forbidden);

    }
}
