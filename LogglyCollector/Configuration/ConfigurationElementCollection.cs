using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;

namespace LogglyCollector.Configuration
{
    public class ConfigurationElementCollection<T> : 
                    ConfigurationElementCollection, IEnumerable<T> where T : NamedConfigurationElement, new() 
            
    {

        public void Add(T item)
        {
            base.BaseAdd(item);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            T result = new T();
            result.Name = elementName;
            return result;
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((T)element).Name;
        }

        public T this[int index]
        {
            get
            {
                return (T)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public T this[string Name]
        {
            get
            {
                return (T)BaseGet(Name);
            }
        }


        public new IEnumerator<T> GetEnumerator()
        {
            foreach (T item in ((IEnumerable)this))
                yield return item;
        }

    }
}
