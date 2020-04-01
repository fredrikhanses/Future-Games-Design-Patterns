using System;

[AttributeUsage(validOn:AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class SecureSingletonAttribute : Attribute { }
