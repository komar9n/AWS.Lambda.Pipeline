using System;
using System.Collections.Generic;
using AWS.Lambda.Pipeline.Attributes;

namespace AWS.Lambda.Pipeline
{
    public sealed class PipelineContext
    {
        private readonly Dictionary<string, object> _items;

        public PipelineContext(Type requestType, IList<BaseRequestPipelineAttribute> requestAttributes)
        {
            RequestType = requestType;
            RequestAttributes = requestAttributes;
            _items = new Dictionary<string, object>();
        }

        public Type RequestType { get; }

        public IList<BaseRequestPipelineAttribute> RequestAttributes { get; }

        public T GetItem<T>(string key)
        {
            return _items.TryGetValue(key, out var result) ? (T)result : default;
        }

        public void SetItem(string key, object value)
        {
            _items.Add(key, value);
        }
    }
}
