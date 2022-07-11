namespace Slipstream.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class SlipstreamPlugin : Attribute
{
    public Type InstanceFactoryType { get; }
    public Type ConfigurationType { get; }

    public SlipstreamPlugin(Type instanceFactoryType, Type configurationType)
        => (InstanceFactoryType, ConfigurationType) 
            = (instanceFactoryType, configurationType);
}
