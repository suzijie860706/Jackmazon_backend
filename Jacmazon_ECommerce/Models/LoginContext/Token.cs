﻿using System;
using System.Collections.Generic;

namespace Jacmazon_ECommerce.Models.LoginContext;

public partial class Token
{
    public int Id { get; set; }

    /// <summary>
    /// 長期Token
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    public DateTime ExpiredDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }
}
