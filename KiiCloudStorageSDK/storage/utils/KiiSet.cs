using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    internal class KiiSet<T> : List<T>
    {
        public KiiSet ()
        {
        }

        /// <summary>
        /// Add the specified item.
        /// </summary>
        /// <param name='item'>
        /// Item.
        /// </param>
        public new void Add (T item)
        {
            for (int i = 0 ; i < Count ; ++i)
            {
                if (this[i].Equals(item))
                {
                    this[i] = item;
                    return;
                }
            }
            base.Add(item);
        }
    }
}

