﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
   public class OverDto
    {
        public int winIdentity;
        public List<int> winUidList;
        public int beenCount;

        public OverDto() { }
    }
}
