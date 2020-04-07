using System;

[AttributeUsage(validOn:AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class MonoSecureSingletonAttribute : Attribute { }
