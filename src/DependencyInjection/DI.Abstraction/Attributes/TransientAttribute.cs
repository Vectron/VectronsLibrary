using System;

namespace VectronsLibrary.DI.Attributes;

/// <summary>
/// Attribute indicating the type should be registered as transient by automatic dependency scanning.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class TransientAttribute : Attribute
{
}