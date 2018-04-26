﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
   public class GrabDto
    {
        public int userId;
        public List<CardDto> tableCarList;

        public GrabDto()
        {

        }
        public GrabDto(int userId, List<CardDto> tableCarList)
        {
            this.userId = userId;
            this.tableCarList = tableCarList;
        }
    }
}
