﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dijkstra
{
    public struct Pair<T, U>
    {
        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };
}
