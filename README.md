# LassieSharp - .NET library for the Congress LaaS API

This is a relatively thin wrapper over the existing
[Congress REST API](https://docs.lora.engineering/).

## Configuration

Create a file `lassie.cfg` in your home folder (either `$HOME` if you run on
macOS/Linux or `C:\Users\<your-user>` when you run on Windows) and add a line
that says `token=<your-api-token>` to set the default token. You can override
the token with the environment variable `LASSIE_TOKEN`.


## Example

This will create a new client and query the applications:

```cs
var client = new Lassie.Client();
var apps = client.ListApplications();
apps.Wait();
foreach (Lassie.Application app in apps.Result) {
    Console.WriteLine("App EUI: {0} name: {1}", app.ApplicationEUI, app.Tags["name"]);
}
```