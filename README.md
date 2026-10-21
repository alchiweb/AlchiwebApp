# AlchiwebApp: a framework based on [BitPlatform](https://bitplatform.dev/)

BitPlatform has made great strides: now the framework is now mature at v10.6 (with the addition of multi-tenancy), the documentation is polished, and the components have evolved significantly...

For instance, [BlazorUI graphical components](https://blazorui.bitplatform.dev/) — it’s very impressive!

## TLTR;

Use the following commands to generate a custom application based on BitPlatform and AlchiwebApp:
```bash
dotnet new install Bit.Boilerplate
dotnet new bit-bp --name MyCustomApp --database PostgreSQL --api Standalone --sample --module Admin
dnx AlchiwebApp.Cli prepare -t .\MyCustomApp\
dnx AlchiwebApp.Cli upgrade -t .\MyCustomApp\
```
Build the solution (`MyCustomApp\MyCustomApp.slnx` solution file), and enjoy your custom Blazor application!

This includes:
- a Blazor server app -> Server + Client
- a Blazor WebAssembly app -> Client
- a MAUI Blazor hybrid app (for Android, ios, macos, Windows and Tizen) -> Client
- a separate Windows Blazor hybrid app -> Client
- an API project -> Server
- an Aspire project
- Core projects for shared code

## First, what is BitPlatform?

BitPlatform includes a base template with libraries for building all kinds of applications on all major platforms using C# (with Blazor).

Everything is open-source under the permissive MIT license.

The team behind this project is highly skilled in C# and Blazor and helps give Blazor all the power and ease of use it should have had.

### Generating a custom application based on BitPlatform Boilerplate

You can generate a BitPlatform application in three different ways:

 - using `dotnet new` CLI command to use a template (main way for BitPlatform before v11).<br>
  For example:
```bash
dotnet new bit-bp --name MyApp --database PostgreSQL --api Standalone --pipeline Azure --module Admin --sample --signalR --cloudflare false
```


> Installation of the .Net template:
> ```bash
> dotnet new install Bit.Boilerplate
> ```

 - using `bit` CLI command to use a template (main way for BitPlatform v11+)  (https://bitplatform.dev/templates/create-project).<br>
  For example:
```bash
bit MyApp --database PostgreSQL --api Standalone --pipeline Azure --module Admin --sample --signalR --cloudflare false
```
> Installation of the `bit` CLI command:
> ```bash
> dotnet tool install --global Bit.Cli --prerelease
> ```
> Or, without installing the Bit CLI, you use it by replacing `bit` with `dnx Bit.Cli --prerelease new`.

 - or with AI assistance via dedicated MCP servers... though that isn't the path I chose.

But... once you’ve generated your application using the boilerplate, how do you ensure it stays up to date - especially regarding security?

### Updating this custom application

To keep up with BitPlatform's evolution, you could regenerate the application regularly and apply the changes to your custom development. Thanks to Git for comparing file changes!

However, this approach forces you to build an application that adheres strictly to the technical choices made by the BitPlatform team.
That’s not ideal: we often need to make different architectural and technical decisions...

## So, what exactly is AlchiwebApp?

That’s where AlchiwebApp comes in: it acts as a layer on top of BitPlatform, designed to accommodate both BitPlatform version upgrades and the specific requirements of applications built with the framework.

### Use AlchiwebApp CLI commands

Once the application has been generated using `dotnet new bit-bp`, simply run the following commands to convert my sample application according to my own choices regarding software architecture and additional frameworks:<br>
  For example:
- Prepare the application with DDD architecture for using AlchiweApp framework:<br>
```bash
alchiwebapp prepare -t .\MyApp\
```

- Upgrade the application in order to use AlchiwebApp framework (NuGet libraries), with separation of "Business" code:<br>
```bash
alchiwebapp upgrade -t .\MyApp\
```

> Installation of the `alchiwebapp` CLI command:
> ```bash
> dotnet tool install --global AlchiwebApp.Cli
> ```
> Or, without installing the AlchiwebApp CLI, you use it by replacing `alchiwebapp` with `dnx AlchiwebApp.Cli`.

And that's it! All that remains is to apply the Git changes.

## Notes

### I'm testing AlchiwebApp with a custom app
Since Bitplatform version 10.4 (latest version is currently 10.6.2, on october 2026), I have been able to easily update a custom test sample application, with the lastest version of BitPlatform.

And it works great! So I can maintain an application that is always up-to-date, secure, and fully customized (adapted for DDD, a different UI framework, etc.).
