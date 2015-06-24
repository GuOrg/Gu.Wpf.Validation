namespace Gu.Wpf.Validation.Internals
{
    using System.Collections.Generic;
    using System.Linq;

    public class UpdateValidationFlags
    {
        private readonly IReadOnlyList<object> _triggerValues;

        public UpdateValidationFlags(object[] triggerValues)
        {
            if (triggerValues != null)
            {
                _triggerValues = triggerValues.ToArray();
            }
            else
            {
                _triggerValues = new object[0];                
            }
        }

        public UpdateValidationFlags Update(object[] triggerValues)
        {
            if (_triggerValues.Count != triggerValues.Length)
            {
                return new UpdateValidationFlags(triggerValues);
            }
            for (int i = 0; i < triggerValues.Length; i++)
            {
                if (!Equals(_triggerValues[i], triggerValues[i]))
                {
                    return new UpdateValidationFlags(triggerValues);
                }
            }
            return this;
        }
    }
}