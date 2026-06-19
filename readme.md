# Hue.CSharp

An early .NET library for controlling a Philips Hue installation.

## Status

Version 0.1, pre-alpha.

This project was written as a learning and design exercise using SOLID principles and test-driven development.

## Structure

- `ChrisBrooksbank.HueInterfaces` - interfaces for the Hue abstractions.
- `ChrisBrooksbank.Hue.Implementation` - concrete implementations.
- `Hue.Csharp.BridgeTests` - tests around bridge behaviour.
- `Hue.CSharp.sln` - Visual Studio solution.

## Design notes

The project follows a stairway pattern: interfaces are defined separately from their implementations so consumers can depend on abstractions.
