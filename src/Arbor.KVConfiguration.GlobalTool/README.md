# Arbor.KVConfiguration.GlobalTool

A .NET tool for creating and updating [Arbor.KVConfiguration](https://github.com/niklaslundberg/Arbor.KVConfiguration) JSON key-value configuration files from the command line.

## Install

```shell
dotnet tool install --global Arbor.KVConfiguration.GlobalTool
```

The installed command is `dotnet-arborkv`.

## Usage

```shell
dotnet-arborkv <path-to-json-file> <key>=<value> [<key>=<value> ...]
```

Example:

```shell
dotnet-arborkv ./applicationMetadata.json myKey=myValue myKey2=myValue2
```

This writes a configuration file such as:

```json
{
  "version": "1.0",
  "keys": [
    { "key": "myKey", "value": "myValue" },
    { "key": "myKey2", "value": "myValue2" }
  ]
}
```

If the file already exists, its existing keys are kept. Keys passed on the command line replace existing keys with the same name (case-insensitive). Keys are written sorted by name.

## Options

| Argument  | Description                  |
|-----------|------------------------------|
| `--debug` | Enable debug-level logging.  |

## Exit codes

| Code | Meaning                                  |
|------|------------------------------------------|
| 0    | Success                                  |
| 1    | The application host could not be created |
| 2    | No `key=value` pairs were supplied       |
| 3    | Missing required arguments               |

## License

MIT
