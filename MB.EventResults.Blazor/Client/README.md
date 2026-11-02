# Client

The Blazor WebAssembly site that visitors use. It runs in the browser and shows:

- result tables with splits for each class
- graphs such as mistakes, pack, performance index, positions and time
- head to head and hall of fame comparisons
- statistics for each leg, on the `/statistics` page

The client has two data modes, chosen when it is built.

## Data modes

| Mode | Build | Data source | Class used |
|---|---|---|---|
| API (default) | Normal build | The server's `/api/*` endpoints | `DataClient` |
| Static | `-p:StaticSite=true` | The file `data/processed.json` | `StaticDataClient` |

The property defines the `STATIC_SITE` compile constant, and `Program.cs` uses it to register the matching `IDataClient`.

### API mode

The [server](../Server/README.md) hosts the client, so there is nothing to deploy separately.

### Static mode

The client downloads one JSON file, created by the [uploader](../MB.EventResults.Uploader/README.md), and does everything else in the browser.

- **Updates:** a new file is picked up when a visitor selects a class or reloads. An open page doesn't refresh by itself.
- **Bandwidth:** the file is only downloaded again when its `ETag` changes, as long as the web server supports it.
- **Connection problems:** if a refresh fails, the page keeps showing the last results it loaded.
- **No results yet:** a missing file (`404`) shows an empty class list.

## Building the static site

From the repository root:

```bash
dotnet publish MB.EventResults.Blazor/Client/MB.EventResults.Blazor.Client.csproj -c Release -p:StaticSite=true -o publish
```

Serve the contents of `publish/wwwroot` from any static web server, with the results at `data/processed.json`. The web server must:

- serve `.wasm` files as `application/wasm`
- send `index.html` for unknown paths, so that links such as `/statistics` work
- ideally serve the pre-compressed `.gz` files created by the build, to save CPU and bandwidth

[`nginx.static.conf`](../nginx.static.conf) does all of this, and also serves `/data/` from the container's `/data` folder.

## Running the static image

From the `MB.EventResults.Blazor` folder:

```bash
docker build -f Dockerfile.static -t results-static .
```

```bash
docker run --rm -p 8080:8080 -v ./results-data:/data results-static
```

Then publish results to the `results-data` folder with the uploader. The site is at `http://localhost:8080`.
