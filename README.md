# MB.EventResults.Blazor

Publishes orienteering results from IOF XML 3.0 files as a website with result tables, split analysis, graphs and statistics.

## Apps

| Project | What it is | Details |
|---|---|---|
| `MB.EventResults.Blazor/Client` | Blazor WebAssembly site. Runs in the visitor's browser. | [README](MB.EventResults.Blazor/Client/README.md) |
| `MB.EventResults.Blazor/Server` | ASP.NET Core app. Hosts the client, accepts uploads and serves the results API. | [README](MB.EventResults.Blazor/Server/README.md) |
| `MB.EventResults.Blazor/MB.EventResults.Uploader` | Console app. Watches a folder for IOF XML files, processes them and publishes the result to a static site. | [README](MB.EventResults.Blazor/MB.EventResults.Uploader/README.md) |

Two libraries support them:

- `MB.OResults.Core`: reads IOF XML data, builds the results and calculates the analysis.
- `Shared`: models, URLs and JSON serialization shared by the apps.

## Choosing a mode

| Mode | Where results are processed | What the host runs | Use it when |
|---|---|---|---|
| [Static site](#static-site) | The uploader, on your own PC | Any static web server, such as nginx | The host is small, like a router or a Raspberry Pi |
| [Server with uploads](#server-with-uploads) | The server | .NET | Results should be uploaded over HTTP |
| [Server reading a file](#server-reading-a-file) | The server | .NET | The results software writes directly to a folder the server can read |
| [AWS](#aws) | The server, on AWS Lambda | AWS Lambda and S3 | Results should be hosted in the cloud |

### Static site

```text
Results software -> watch folder -> Uploader -> processed.json -> static web server -> browser
```

The host only serves files. Visitors have no upload address to reach, because publishing happens outside the website.

Client build: `-p:StaticSite=true`. Container image: `-static` tags, built from `Dockerfile.static`.

The nginx image and SFTP uploads to RouterOS have not been tested yet.

### Server with uploads

```text
Results software -> POST /api/import -> Server -> processed.json -> browser
```

This is the server's default mode (`App:Mode` is anything except `File`).

### Server reading a file

```text
Results software -> file in the upload folder -> Server -> browser
```

Set `App:Mode` to `File`. The server processes the file when it changes.

### AWS

Set `AwsHosted` to `true` to run on Lambda, and `App:Storage` to `AWS` to store files in S3. See the [server README](MB.EventResults.Blazor/Server/README.md#aws).

> **Known issue:** both server modes fail to show results when using local storage. See the [server README](MB.EventResults.Blazor/Server/README.md#known-issues).

## Building

Requires the .NET 10 SDK.

```bash
dotnet build MB.EventResults.Blazor.sln
```

## Container images

GitHub Actions builds images for `linux/amd64`, `linux/arm64` and `linux/arm/v7`, and pushes them to GitHub Container Registry:

| Image tag | Contents | Dockerfile |
|---|---|---|
| `ghcr.io/nzmangan/mb.eventresults.blazor:develop` | Server and client | `Dockerfile.multiarch` |
| `ghcr.io/nzmangan/mb.eventresults.blazor:develop-static` | Client on nginx, no .NET | `Dockerfile.static` |

Branches are tagged by name. Pushes to `master` also get `latest` and `latest-static`.
