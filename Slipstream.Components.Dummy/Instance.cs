﻿using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Components.Dummy;

public class Instance : IInstance
{
    public EntityName Name { get; private set; }

    public Instance(EntityName name)
        => Name = name;

    public Task MainAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    internal void OnKeyPressEvent(KeyPressEvent @event)
    {
        Console.WriteLine($"  [{Name}] Got keyrress '{@event.Key}'");
    }
}
