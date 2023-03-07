using Panbyte.ArgParsing;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;


[TestClass]
public class ArgParserTest
{
    private readonly ArgParser _parser = new();
    
    [TestMethod]
    public void ParseCorrectFormats()
    {
        var args = new[] { "-f", "bytes", "-t", "bytes" };
        var opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Bytes));
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(Bytes));
        
        args = new[] { "-f", "bits", "--to=bits" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Bits));
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(Bits));
        
        args = new[] { "--from=hex", "-t", "hex" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Hex));
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(Hex));
        
        args = new[] { "--from=int", "--to=int" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Int));
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(Int));
        
        args = new[] { "-f", "array", "-t", "array" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(ByteArray));
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(ByteArray));
        
        args = new[] { "-f", "bytes", "-t", "hex" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Bytes));
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(Hex));
        
        args = new[] { "-f", "array", "-t", "int" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(ByteArray));
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(Int));
        
        args = new[] { "-f", "hex", "-t", "hex", "--from=array", "--to=int" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(ByteArray));
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(Int));
    }

    [TestMethod]
    public void ParseInvalidFormats()
    {
        var args = new[] { "-f", "somerandomformat", "-t", "bytes" };

        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));

        args = new[] { "-f", "bytes", "--to=test test" };

        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));
        
        args = new[] { "--from=bytes", "-t" };

        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));
        
        args = new[] { "-f", "-t", "test" };

        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));

        args = new[] { "" };

        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));

        args = Array.Empty<string>();
        
        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));
    }
    
    [TestMethod]
    public void ParseCorrectFormatOptions()
    {
        //int
        var args = new[] { "-f", "int", "-t", "int", "--from-options=big", "--to-options=little" };
        var opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Int));
        var intInput = (Int) opts.InputFormat;
        Assert.AreEqual(intInput.Endianness, Endianness.BigEndian);
            
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(Int));
        var intOutput = (Int) opts.OutputFormat;
        Assert.AreEqual(intOutput.Endianness, Endianness.LittleEndian);
        
        args = new[] { "-f", "int", "-t", "int", "--from-options=little", "--to-options=big" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Int));
        intInput = (Int) opts.InputFormat;
        Assert.AreEqual(intInput.Endianness, Endianness.LittleEndian);
            
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(Int));
        intOutput = (Int) opts.OutputFormat;
        Assert.AreEqual(intOutput.Endianness, Endianness.BigEndian);
        
        //bits
        args = new[] { "-f", "bits", "-t", "int"};
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Bits));
        var bitsInput = (Bits) opts.InputFormat;
        Assert.AreEqual(bitsInput.BitPadding, BitPadding.Left);
        
        args = new[] { "-f", "bits", "-t", "int", "--from-options=left" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Bits));
        bitsInput = (Bits) opts.InputFormat;
        Assert.AreEqual(bitsInput.BitPadding, BitPadding.Left);


        args = new[] { "-f", "bits", "-t", "int", "--from-options=right" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.InputFormat.GetType() == typeof(Bits));
        bitsInput = (Bits) opts.InputFormat;
        Assert.AreEqual(bitsInput.BitPadding, BitPadding.Right);
        
        // array
        args = new[] { "-f", "bits", "-t", "array"};
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(ByteArray));
        var arrayOutput = (ByteArray) opts.OutputFormat;
        Assert.AreEqual(arrayOutput.ArrayFormat, ArrayFormat.Hex);
        Assert.AreEqual(arrayOutput.Brackets, Brackets.Curly);
        
        args = new[] { "-f", "bits", "-t", "array", "--to-options=0x", "--to-options=["};
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(ByteArray));
        arrayOutput = (ByteArray) opts.OutputFormat;
        Assert.AreEqual(arrayOutput.ArrayFormat, ArrayFormat.Hex);
        Assert.AreEqual(arrayOutput.Brackets, Brackets.Square);
        
        args = new[] { "-f", "bits", "-t", "array", "--to-options=0", "--to-options={}"};
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(ByteArray));
        arrayOutput = (ByteArray) opts.OutputFormat;
        Assert.AreEqual(arrayOutput.ArrayFormat, ArrayFormat.Decimal);
        Assert.AreEqual(arrayOutput.Brackets, Brackets.Curly);
        
        args = new[] { "-f", "bits", "-t", "array", "--to-options=a", "--to-options=)"};
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(ByteArray));
        arrayOutput = (ByteArray) opts.OutputFormat;
        Assert.AreEqual(arrayOutput.ArrayFormat, ArrayFormat.Char);
        Assert.AreEqual(arrayOutput.Brackets, Brackets.Regular);
        
        args = new[] { "-f", "bits", "-t", "array", "--to-options=0b", "--to-options=]"};
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(ByteArray));
        arrayOutput = (ByteArray) opts.OutputFormat;
        Assert.AreEqual(arrayOutput.ArrayFormat, ArrayFormat.Binary);
        Assert.AreEqual(arrayOutput.Brackets, Brackets.Square);
        
        args = new[] { "-f", "bits", "-t", "array", "--to-options=0", "--to-options=[]"};
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(ByteArray));
        arrayOutput = (ByteArray) opts.OutputFormat;
        Assert.AreEqual(arrayOutput.ArrayFormat, ArrayFormat.Decimal);
        Assert.AreEqual(arrayOutput.Brackets, Brackets.Square);
        
        args = new[] { "-f", "bits", "-t", "array", "--to-options=0x", "--to-options=()"};
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.OutputFormat.GetType() == typeof(ByteArray));
        arrayOutput = (ByteArray) opts.OutputFormat;
        Assert.AreEqual(arrayOutput.ArrayFormat, ArrayFormat.Hex);
        Assert.AreEqual(arrayOutput.Brackets, Brackets.Regular);
    }
    
    [TestMethod]
    public void ParseInvalidFormatOptions()
    {
        var args = new[] { "-f", "bytes", "-t", "array", "--from-options=big", "--to-options=little" };
        
        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));

        args = new[] { "-f", "array", "-t", "int", "--from-options=left" };
        
        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));
        
        args = new[] { "-f", "array", "-t", "int", "--from-options=", "--to-options=" };
        
        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));
        
        args = new[] { "-f", "array", "-t", "int", "--from-options={" };
        
        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));
    }
    
    [TestMethod]
    public void ParseCorrectOptions()
    {
        var args = new[] { "-f", "bytes", "-t", "array", "-h" };
        var opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.Help);
        
        args = new[] { "-f", "bytes", "-t", "array", "--help" };
        opts = _parser.ParseArguments(args);
        
        Assert.IsTrue(opts.Help);
        
        args = new[] { "-f", "bytes", "-t", "array", "-i", "/test" };
        opts = _parser.ParseArguments(args);
        
        Assert.AreEqual("/test", opts.InputFilePath);
        
        args = new[] { "-f", "bytes", "-t", "array", "--input=/test" };
        opts = _parser.ParseArguments(args);
        
        Assert.AreEqual("/test", opts.InputFilePath);
        
        args = new[] { "-f", "bytes", "-t", "array", "-o", "/test" };
        opts = _parser.ParseArguments(args);
        
        Assert.AreEqual("/test", opts.OutputFilePath);
        
        args = new[] { "-f", "bytes", "-t", "array", "--output=/test" };
        opts = _parser.ParseArguments(args);
        
        Assert.AreEqual("/test", opts.OutputFilePath);
        
        args = new[] { "-f", "bytes", "-t", "array", "-d", "|" };
        opts = _parser.ParseArguments(args);
        
        Assert.AreEqual("|", opts.Delimiter);
        
        args = new[] { "-f", "bytes", "-t", "array", "--delimiter=|" };
        opts = _parser.ParseArguments(args);
        
        Assert.AreEqual("|", opts.Delimiter);
    }
    
    [TestMethod]
    public void ParseInvalidOptions()
    {
        var args = new[] { "-f", "bytes", "-t", "array", "-m" };
        
        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));
        
        args = new[] { "-f", "bytes", "-t", "array", "-f" };
        
        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));
        
        args = new[] { "-f", "bytes", "-t", "array", "--test=test" };
        
        Assert.ThrowsException<ArgumentException>(() => _parser.ParseArguments(args));
    }
}