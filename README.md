# Panbyte

Panbyte is a C# console tool for conversions between various representations of byte sequences.

## CLI
```
./panbyte [ARGS...]

ARGS:
-f FORMAT     --from=FORMAT           Set input data format
              --from-options=OPTIONS  Set input options
-t FORMAT     --to=FORMAT             Set output data format
              --to-options=OPTIONS    Set output options
-i FILE       --input=FILE            Set input file (default stdin)
-o FILE       --output=FILE           Set output file (default stdout)
-d DELIMITER  --delimiter=DELIMITER   Record delimiter (default newline)
-h            --help                  Print help

FORMATS:
bytes                                 Raw bytes
hex                                   Hex-encoded string
int                                   Integer
bits                                  0,1-represented bits
array                                 Byte array
```

## Project build

To build this project, you need a [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download) to be installed.

### Build
To build the solution use (in /PV286-project/Panbyte):
```bash
dotnet build
```

### Run
To run the application use (in /PV286-project/Panbyte):
```bash
dotnet run --project Panbyte -- [Panbyte args]
```

Or by using:
```bash
dotnet run -- [Panbyte args]
```
in folder /PV286-project/Panbyte/Panbyte


### Test
To run the application use (in /PV286-project/Panbyte):
```bash
dotnet test
```