using System.Collections.Generic;
using System.Linq;
using Rhino.Security.Mgmt.Infrastructure;
using NHibernate.Validator.Engine;

namespace Rhino.Security.Mgmt.Infrastructure
{
    public class NHibernateValidator : Rhino.Security.Mgmt.Infrastructure.IValidator
    {
        private readonly ValidatorEngine _engine;

        public NHibernateValidator(ValidatorEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<ValidationError> Validate(object entity)
        {
            return _engine.Validate(entity).Select(x => new ValidationError{
                PropertyPath = x.PropertyPath,
                Message = x.Message
            });
        }
    }
}