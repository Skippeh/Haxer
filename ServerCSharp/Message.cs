using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServerCSharp
{
    public class Message
    {
        private Dictionary<string, object> data;

        public Message(Dictionary<string, object> data)
        {
            this.data = data;
        }

        public Message(params object[] data)
        {
            var dataResult = new Dictionary<string, object>();

            if (data.Length % 2 != 0)
            {
                throw new ArgumentException("Uneven amount of params supplied to Message constructor. Each key needs a value!");
            }

            for (int i = 0; i < data.Length; i += 2)
            {
                dataResult.Add((string)data[i], data[i + 1]);
            }

            this.data = dataResult;
        }

        public bool Contains(string key)
        {
            return data.ContainsKey(key);
        }

        public object this[string key]
        {
            get
            {
                if (!data.ContainsKey(key))
                    return null;

                return data[key];
            }
            set
            {
                if (data.ContainsKey(key))
                    data[key] = value;
                else
                    data.Add(key, value);
            }
        }

        public string GenerateJson()
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}