# Stride.Diagnostics.Sentry
Sentry integration for Stride - I suggest you copy the project over to your solution and customize it to your needs.

In the `Example.csproj` there's a target that adds git hash to assembly metadata. See `Program.cs` and `ThrowingScript` for usage examples. Remember to include the `.appsettings` file with your Sentry API Endpoint url (requires [stride #878](https://github.com/stride3d/stride/pull/878)).

The example project currently fails in a different spot than expected, you can still use it to test your Sentry endpoint.

Big thanks to [@bruno-garcia](https://github.com/bruno-garcia/) on whose work I have based this.
