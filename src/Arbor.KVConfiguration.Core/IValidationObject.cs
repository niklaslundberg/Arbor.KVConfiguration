using System.Collections.Generic;

namespace Arbor.KVConfiguration.Core
{
    public interface IValidationObject
    {
        IEnumerable<ValidationError> Validate();
    }
}
