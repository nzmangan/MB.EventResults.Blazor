# Server

An ASP.NET Core app that hosts the [client](../Client/README.md) in its API mode, accepts uploaded IOF XML files and serves the results.

For very small hosts, consider the static site mode instead, described in the [main README](../../README.md#static-site).

## Result modes

Set with `App:Mode`.

| Value | Behaviour |
|---|---|
| `Upload`, or anything except `File` (default) | Files are uploaded to `/api/import`. The server processes them into `processed.json` and serves that. |
| `File` | The server reads `App:ResultsSourceFile` and `App:CourseDataSourceFile` from the upload folder, and processes them again when the results file changes. |

## Storage modes

Set with `App:Storage`.

| Value | Behaviour |
|---|---|
| Anything except `AWS` (default) | Files are stored in `App:UploadFolder`. |
| `AWS` | Files are stored in the S3 bucket `App:S3BucketName`. |

## Upload authentication

Set with `App:AuthMode`. This protects the import, clear and reindex endpoints.

| Value | Behaviour |
|---|---|
| `key` (default) | Requires the header `x-api-key`, matching `App:UploadKey`. |
| `basic` | Requires HTTP basic authentication, matching `App:UploadUsername` and `App:UploadPassword`. |
| `none` | No authentication. |

With the default settings every upload is rejected, because no key is set.

## Configuration

Settings come from `appsettings.json` and environment variables. In environment variables, use `__` instead of `:`, for example `App__UploadKey`.

| Setting | Purpose |
|---|---|
| `App:Mode` | Result mode |
| `App:Storage` | Storage mode |
| `App:AuthMode` | Upload authentication |
| `App:UploadKey` | API key for `key` authentication |
| `App:UploadUsername`, `App:UploadPassword` | Login for `basic` authentication |
| `App:UploadFolder` | Folder for stored files. The container sets it to `/data`. |
| `App:ResultsSourceFile`, `App:CourseDataSourceFile` | Files to read in `File` mode |
| `App:S3BucketName` | Bucket for `AWS` storage |
| `Analyser:MistakeIndex` | Performance index below which a leg counts as a mistake |
| `Analyser:Pack` | Seconds between runners for them to count as running in a pack |
| `AwsHosted` | Run on AWS Lambda |

## API

| Method and path | Purpose | Authenticated |
|---|---|---|
| `GET /api/results` | All results | No |
| `GET /api/results/{id}` | Results for one class | No |
| `GET /api/grades` | List of classes | No |
| `GET /api/stats` | Event statistics | No |
| `POST /api/import` | Upload IOF XML files as a multipart form | Yes |
| `POST /api/reindex` | Rebuild results from the stored files | Yes |
| `POST /api/clear` | Delete the stored event data | Yes |

## Uploading files

```powershell
curl.exe -i -H "x-api-key: YOUR_KEY" -F "file=@results.xml" http://localhost:8080/api/import
```

The file type is detected from its contents. Result lists, start lists, entry lists, class lists and course data are accepted, and each upload replaces the previous file of that type.

| Response | Meaning |
|---|---|
| `200` | Saved and processed |
| `401` | Wrong or missing credentials |
| `404` | Not recognised as a supported IOF XML file |

## Running

From the repository root:

```bash
dotnet run --project MB.EventResults.Blazor/Server
```

With Docker, from the `MB.EventResults.Blazor` folder. The container listens on port 8080 and stores data in `/data`:

```bash
docker build -f Dockerfile.multiarch -t results-server .
```

```bash
docker run --rm -p 8080:8080 -e App__UploadKey=YOUR_KEY -v results-data:/data results-server
```

## AWS

- `AwsHosted=true` runs the app on AWS Lambda behind an HTTP API, and keeps data protection keys in Systems Manager. `Dockerfile` builds this version.
- `App:Storage=AWS` stores files in S3. It can be used separately from `AwsHosted`.

## Known issues

**Results don't appear when using local storage.** `FileService.LastUpdated` has an inverted check: it returns `null` when the file exists.

- In `Upload` mode, an upload returns `200` and saves `processed.json`, but `/api/grades` stays empty and `/api/results` returns `404`.
- In `File` mode, the same check leads to an exception when results are requested. This was found by reading the code, not by running it.

S3 storage uses a separate class, `S3FileService`, which is not affected by this bug.
