# Uploader

A console app for the static site mode. It watches a folder for IOF XML files, processes them on your own PC and publishes the result to the web host, so the host doesn't have to do any processing.

## How it works

1. Your results software saves an IOF XML file in the watch folder.
2. The uploader waits until the file has stopped changing.
3. It selects the newest file of each type and builds the results, using the same code as the server.
4. It publishes `processed.json` and a compressed `processed.json.gz` to the target.

Each published file is first written under a temporary name and then renamed, so visitors don't download a half-written file.

### Files in the watch folder

- **Types:** `ResultList`, `StartList`, `EntryList`, `ClassList` and `CourseData`, detected from the file's contents. The filename only needs to end in `.xml`.
- **Several files of one type:** the most recently changed one is used. If you delete it, the next newest one is published.
- **Subfolders:** these are ignored.
- **Other XML files:** these are ignored.
- **Damaged or half-written files:** the error is logged, the published results stay online, and the uploader tries again on the next check.

## Running modes

| Mode | Command | Behaviour |
|---|---|---|
| Watch (default) | No extra option | Keeps running and publishes every change until you stop it with Ctrl+C. |
| Once | `--once` | Processes the folder once and exits. The exit code is `0` on success, or `1` if publishing failed or no files were found. |

Watch mode reacts to file changes straight away. It also checks the folder regularly, which catches changes the file watcher misses, such as on network shares, and retries failed uploads.

## Targets

### SFTP (default)

Uploads to a web host over SFTP, such as a MikroTik router with the results folder on a USB stick. The folder is created if it doesn't exist.

Uploading to RouterOS has not been tested yet. If its SFTP server can't replace a file by renaming, the uploader deletes the old file first, which leaves a very short gap where the file is missing.

### Folder

Writes to a folder, which is useful for testing, or for a web host on the same PC or on a network share:

```bash
dotnet run --project MB.EventResults.Blazor/MB.EventResults.Uploader -- --Uploader:WatchFolder=C:\Results\watch --Uploader:Target:Type=Folder --Uploader:Target:Path=C:\Results\site\data
```

## Configuration

Settings are read from `appsettings.json` next to the program, then environment variables, then command line options.

| Setting | Default | Purpose |
|---|---|---|
| `Uploader:WatchFolder` | `watch` | Folder to watch. A relative path starts from the current folder. Created if missing. |
| `Uploader:DebounceSeconds` | `2` | How long a file must stay unchanged before it is processed |
| `Uploader:PollSeconds` | `10` | How often the folder is checked regardless of file events |
| `Uploader:Target:Type` | `Sftp` | `Sftp` or `Folder` |
| `Uploader:Target:Path` | `/usb1/results-data` | Folder to publish to |
| `Uploader:Target:Host` | `192.168.10.1` | SFTP host |
| `Uploader:Target:Port` | `22` | SFTP port |
| `Uploader:Target:Username` | `admin` | SFTP username |
| `Uploader:Target:Password` | Empty | SFTP password |
| `Uploader:Target:HostKeySha256` | Empty | Expected fingerprint of the SFTP host's key |
| `Analyser:MistakeIndex` | `0.88` | Performance index below which a leg counts as a mistake |
| `Analyser:Pack` | `30` | Seconds between runners for them to count as running in a pack |

To keep the password out of the repository, set it as an environment variable instead of in `appsettings.json`. In PowerShell:

```powershell
$env:Uploader__Target__Password = "YOUR_PASSWORD"
```

### Checking the host's identity

If `HostKeySha256` is empty, the uploader accepts any SFTP host and logs its fingerprint. Copy that fingerprint into the setting so that uploads are refused if the host changes.

## Running

From the repository root:

```bash
dotnet run --project MB.EventResults.Blazor/MB.EventResults.Uploader -- --Uploader:WatchFolder=C:\Results\watch
```

To run a published copy without the source code:

```bash
dotnet publish MB.EventResults.Blazor/MB.EventResults.Uploader -c Release -o uploader
```

```bash
uploader/MB.EventResults.Uploader.exe --once --Uploader:WatchFolder=C:\Results\watch
```
