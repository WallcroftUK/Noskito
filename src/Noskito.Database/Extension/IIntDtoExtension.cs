﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noskito.Database.Extension
{
    public interface IIntDtoExtension : IDtoExtension
    {
        int Id { get; set; }
    }
}