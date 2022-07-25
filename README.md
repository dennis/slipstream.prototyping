# Slipstream

## Architecture

In many ways Slipstream is a "**If** something **then** act" engine. This 
document will try to define the different concepts within Slipstream.

### Event

At the heart of Slipstream you'll find events. These are when something
changes. These are data only objects with properties that describe the change.

### Entities

Another core concept is entities. These describes the functionality that
can react to events.

Instances, Triggers, Actions, Rules are all entities, just with different
responsibilities.

Let's take them one by one. But fist, let's define a plugin.

A **plugin** is a collection of all the different types mentioned above. These
provides all functionality with Slipstream's integration against something.

As the name implies this is not a part of the "core" Slipstream software.  But
plugins are what makes Slipstream useful. A plugin is basically the glue
between Slipstream and something else. This might be "Audio", "IRacing", etc.

#### Instances

An Instance is a specific configured instance of a plugin. Imagine if you
got an Audio Plugin; One Instance would be targeting a specific audio 
output device and another would targeting a different output device (assuming 
you got two output devices configured). For some Plugins there might only 
be one Instance, e.g.  IRacing (there can only be one IRacing running on 
the computer at a time).

#### Triggers

Triggers reacts on something. Typically a plugin can provide triggers that acts
on its own events. Triggers can be configured, so they only fires when a
condition is met.

TODO: Some conditions might need to be easily reusable, so that we can do
equal, greater-than, lesser-than, etc.

#### Actions

Similar to triggers, a plugin will also often provide some actions. This will
(likely) change state of something. Taking an Audio plugin, it might provide
a way to play audio, change volume, etc.

TODO: Should events refer back to the instance. So you can send a message back,
to the same sender of an event (e.g same audio device). Or how should actions
show who is the receiver.

#### Rules

Finally. Once you've configured instances, triggers and actions, we need to glue
these together. These are rules.

Each rule got a **trigger** and if that fires it will invoke an **action**.

TODO: How should data be provided to the action? It could deliver the event
that fired the trigger, but that would be quite limiting. 

#### ActionSets

This is very much WIP. In addition to Actions we could also provide ActionSets. 
A ActionSets is a collection of actions. They could possible be a type:

These actions can be invoked:
- in sequence
- picks one action and execute it
- next (first time used, it will pick the first action. 2nd invocation will do the next one.. etc)
