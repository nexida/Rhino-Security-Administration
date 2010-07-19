using System.Collections.Generic;

namespace Rhino.Security.Mgmt.Infrastructure
{
    public interface IValidator
    {
        IEnumerable<ValidationError> Validate(object entity);
    }
}