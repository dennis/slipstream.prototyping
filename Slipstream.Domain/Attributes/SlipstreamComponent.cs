namespace Slipstream.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class SlipstreamComponent : Attribute
{
    public Type InstanceFactoryType { get; }
    public Type ConfigurationType { get; }

    public SlipstreamComponent(Type instanceFactoryType, Type configurationType)
        => (InstanceFactoryType, ConfigurationType) 
            = (instanceFactoryType, configurationType);
}
