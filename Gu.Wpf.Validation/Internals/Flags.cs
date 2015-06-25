namespace Gu.Wpf.Validation.Internals
{
    using System.Collections.Generic;
    using System.Linq;

    public class Flags
    {
        private readonly IReadOnlyList<object> _triggerValues;

        public Flags(object[] triggerValues)
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

        public Flags Update(object[] triggerValues)
        {
            if (_triggerValues.Count != triggerValues.Length)
            {
                return new Flags(triggerValues);
            }
            for (int i = 0; i < triggerValues.Length; i++)
            {
                if (!Equals(_triggerValues[i], triggerValues[i]))
                {
                    return new Flags(triggerValues);
                }
            }
            return this;
        }
    }
}