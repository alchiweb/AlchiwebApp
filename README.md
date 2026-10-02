# AlchiwebApp: a framework based on [BitPlatform](https://bitplatform.dev/)

BitPlatform has made great strides: now the framework is now mature at v10.6 (with the addition of multi-tenancy), the documentation is polished, and the components have evolved significantly...

For instance, [BlazorUI graphical components](https://blazorui.bitplatform.dev/) — it’s very impressive!

## But what exactly is AlchiwebApp?

Before explaining that, let’s start from the beginning.
Currently, you can generate a BitPlatform application:

 - using a project template via a CLI command (https://bitplatform.dev/templates/create-project).<br>
  For example:<br>
`dotnet new bit-bp --name MyApp --database PostgreSQL --api Standalone --pipeline Azure --module Admin --sample --signalR --cloudflare false`

 - with AI assistance via dedicated MCP servers... though that isn't the path I chose.

Once you’ve generated your application using the boilerplate, how do you ensure it stays up to date - especially regarding security?

## Updating a custom application based on BitPlatform

To keep up with BitPlatform's evolution, you could regenerate the application regularly and apply the changes to your custom development. Thanks to Git for comparing file changes!

However, this approach forces you to build an application that adheres strictly to the technical choices made by the BitPlatform team.
That’s not ideal: we often need to make different architectural and technical decisions...

That’s where AlchiwebApp comes in: it acts as a layer on top of BitPlatform, designed to accommodate both BitPlatform version upgrades and the specific requirements of applications built with the framework.

### Use BitPlatform Boilerplate CLI command

See https://bitplatform.dev/templates/create-project to know how to generate a BitPlatform application using the `dotnet new bit-bp` CLI command.

### Use AlchiwebApp CLI commands

Once the application has been generated using `dotnet new bit-bp`, simply run the following commands to convert my sample application according to my own choices regarding software architecture and additional frameworks:
- `AlchiwebApp.exe mod-bp -s . -t .`
- `AlchiwebApp.exe upgrade -s . -t .`

And that's it! All that remains is to apply the Git changes (I am considering an enhancement to apply them automatically).

## Notes

Since Bitplatform version 10.4 (latest version is currently 10.6.2, on october 2026), I have been able to easily update:
- the AlchiwebApp command-line application
- my test sample application

And it works great! So I can maintain an application that is always up-to-date, secure, and fully customized (adapted for DDD, a different UI framework, etc.).
