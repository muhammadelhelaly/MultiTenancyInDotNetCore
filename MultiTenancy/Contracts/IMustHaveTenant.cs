﻿namespace MultiTenancy.Contracts;

public interface IMustHaveTenant
{
    public string TenantId { get; set; }
}