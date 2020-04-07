using System;

[AttributeUsage(validOn: AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class MonoSingletonConfigurationAttribute : Attribute
{
    public string ResourcesPath { get; }
    public MonoSingletonConfigurationAttribute(string resourcesPath)
    {
        ResourcesPath = resourcesPath;
    }
}