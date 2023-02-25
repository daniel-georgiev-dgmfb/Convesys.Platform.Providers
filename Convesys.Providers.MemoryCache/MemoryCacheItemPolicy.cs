using Microsoft.Extensions.Caching.Memory;
using System;
using Twilight.Kernel.Caching;

namespace Twilight.MemoryCacheProvider
{
    public class MemoryCacheItemPolicy : MemoryCacheEntryOptions, ICacheEntryOptions
    {
        public MemoryCacheItemPolicy()
        {
            this.Size = 1;
        }

        DateTimeOffset ICacheEntryOptions.AbsoluteExpiration
        {
            get
            {
                return base.AbsoluteExpiration.GetValueOrDefault();
            }
            set
            {
                base.AbsoluteExpiration = value;
            }
        }
        TimeSpan ICacheEntryOptions.SlidingExpiration
        {
            get
            {
                return base.SlidingExpiration.GetValueOrDefault();
            }
            set
            {
                base.SlidingExpiration = value;
            }
        }
    }
}