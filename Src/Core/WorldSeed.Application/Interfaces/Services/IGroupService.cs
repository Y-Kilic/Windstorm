﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.DTOS;

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IGroupService
    {
        public bool CreateGroup(CreateGroupDTO createGroupDTO);
    }
}
