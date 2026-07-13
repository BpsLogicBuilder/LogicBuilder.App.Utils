using LogicBuilder.Domain;
using System;
using System.Collections.Generic;

namespace Contoso.Domain
{
    public class ModelDictionary : Dictionary<string, BaseModel>
    {
        public new BaseModel this[string index]
        {
            get
            {
                if (this.TryGetValue(index, out BaseModel val))
                    return val;
                else
                {

                    base.Add(index, (BaseModel)Activator.CreateInstance(typeof(ModelDictionary).Assembly.GetType(index)));
                    return base[index];
                }
            }
            set
            {
                if (this.ContainsKey(index))
                    base[index] = value;
                else
                    base.Add(index, value);
            }
        }
    }
}
