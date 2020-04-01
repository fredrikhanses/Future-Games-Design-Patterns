using System;

[AttributeUsage(validOn: AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class SingletonConfiguration : Attribute
{
    public string ResourcesPath { get; }
    public SingletonConfiguration(string resourcesPath)
    {
        ResourcesPath = resourcesPath;
    }
}