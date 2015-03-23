﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUDash.Models
{
    public enum ChartMonths
    {
        Jan = 1,
        Feb = 2,
        Mar = 3,
        Apr = 4,
        May = 5,
        Jun = 6,
        Jul = 7,
        Aug = 8,
        Sep = 9,
        Oct = 10,
        Nov = 11,
        Dec = 12
    }

    public enum RequestedAction
    {
        Delete = 1,
        Upsert = 2
    }

    public enum StorageKeys
    {
        Projects,
        Resources
    }

    public enum ProjectAttribute
    {
        Resources,
        Project
    }

}