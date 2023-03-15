using System.Text;
using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

[TestClass]
public class BytesConverterTest
{
    private readonly BytesConverter _converter = new BytesConverter(new Bytes());

    [TestMethod]
    public void ConvertShortBytes()
    {
        var testBytes = "test";
        
        Assert.AreEqual("test", _converter.ConvertTo(testBytes, new Bytes()));
        Assert.AreEqual("74657374", _converter.ConvertTo(testBytes, new Hex()));
        Assert.AreEqual("1952805748", _converter.ConvertTo(testBytes, new Int(Endianness.BigEndian)));
        Assert.AreEqual("1953719668", _converter.ConvertTo(testBytes, new Int(Endianness.LittleEndian)));
        Assert.AreEqual("01110100011001010111001101110100", _converter.ConvertTo(testBytes, new Bits()));
        
        Assert.AreEqual("{0b1110100, 0b1100101, 0b1110011, 0b1110100}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Binary, Brackets.Curly)));
        Assert.AreEqual("(0b1110100, 0b1100101, 0b1110011, 0b1110100)", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Binary, Brackets.Regular)));
        Assert.AreEqual("[0b1110100, 0b1100101, 0b1110011, 0b1110100]", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Binary, Brackets.Square)));

        Assert.AreEqual("{0x74, 0x65, 0x73, 0x74}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Hex, Brackets.Curly)));
        Assert.AreEqual("(0x74, 0x65, 0x73, 0x74)", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Hex, Brackets.Regular)));
        Assert.AreEqual("[0x74, 0x65, 0x73, 0x74]", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Hex, Brackets.Square)));
        
        Assert.AreEqual("{116, 101, 115, 116}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)));
        Assert.AreEqual("(116, 101, 115, 116)", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Regular)));
        Assert.AreEqual("[116, 101, 115, 116]", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Square)));
        
        Assert.AreEqual("{'t', 'e', 's', 't'}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Char, Brackets.Curly)));
        Assert.AreEqual("('t', 'e', 's', 't')", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Char, Brackets.Regular)));
        Assert.AreEqual("['t', 'e', 's', 't']",
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Char, Brackets.Square)));
    }
    
    [TestMethod]
    public void ConvertLongBytes()
    {
        var testBytes = @"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Nulla non arcu lacinia neque faucibus fringilla. Suspendisse nisl. Aenean fermentum risus id tortor. Nullam eget nisl. Fusce wisi. Etiam neque. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Curabitur vitae diam non enim vestibulum interdum. Etiam quis quam. Nullam rhoncus aliquam metus. Sed ac dolor sit amet purus malesuada congue. Maecenas ipsum velit, consectetuer eu lobortis ut, dictum at dui. Aliquam ornare wisi eu metus. Duis viverra diam non justo. Nulla est. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Duis pulvinar. Morbi leo mi, nonummy eget tristique non, rhoncus non leo. Ut tempus purus at lorem.";
        
        Assert.AreEqual(testBytes, _converter.ConvertTo(testBytes, new Bytes()));
        Assert.AreEqual("4c6f72656d20697073756d20646f6c6f722073697420616d65742c20636f6e7365637465747565722061646970697363696e6720656c69742e204e756c6c61206e6f6e2061726375206c6163696e6961206e65717565206661756369627573206672696e67696c6c612e2053757370656e6469737365206e69736c2e2041656e65616e206665726d656e74756d20726973757320696420746f72746f722e204e756c6c616d2065676574206e69736c2e20467573636520776973692e20457469616d206e657175652e204e616d206c696265726f2074656d706f72652c2063756d20736f6c757461206e6f6269732065737420656c6967656e6469206f7074696f2063756d717565206e6968696c20696d70656469742071756f206d696e75732069642071756f64206d6178696d6520706c61636561742066616365726520706f7373696d75732c206f6d6e697320766f6c757074617320617373756d656e6461206573742c206f6d6e697320646f6c6f7220726570656c6c656e6475732e20437572616269747572207669746165206469616d206e6f6e20656e696d20766573746962756c756d20696e74657264756d2e20457469616d2071756973207175616d2e204e756c6c616d2072686f6e63757320616c697175616d206d657475732e2053656420616320646f6c6f722073697420616d6574207075727573206d616c65737561646120636f6e6775652e204d616563656e617320697073756d2076656c69742c20636f6e736563746574756572206575206c6f626f727469732075742c2064696374756d206174206475692e20416c697175616d206f726e6172652077697369206575206d657475732e20447569732076697665727261206469616d206e6f6e206a7573746f2e204e756c6c61206573742e204578636570746575722073696e74206f6363616563617420637570696461746174206e6f6e2070726f6964656e742c2073756e7420696e2063756c706120717569206f666669636961206465736572756e74206d6f6c6c697420616e696d20696420657374206c61626f72756d2e20447569732070756c76696e61722e204d6f726269206c656f206d692c206e6f6e756d6d79206567657420747269737469717565206e6f6e2c2072686f6e637573206e6f6e206c656f2e2055742074656d707573207075727573206174206c6f72656d2e", 
            _converter.ConvertTo(testBytes, new Hex()));
        Assert.AreEqual("18914023095181622104357430007989934591946627121635631285091430723273970512595700194976940591133996021163411580026964872170186785165890027197781815217691913096982134148509229312212484098435483605630964413480156314933334684247846566828519076841922528292074120177883770939438771356167606907871674998499043943652158037882381924563038468079967723817344211138212001081862084702550641603478102047703998558836391295612873268092514306251448116168008426077194418592426168887745759238346610605928327719191331688619138687603124906261222517567355909681642468506303526456469346389675531249857478774000333576511038573448707716748809053268268814793515718681876940517890684742137272232074463631660923074376026963371106783430766598604667429980893750872934421195022438370865416126575800052196247607329581798389954007431429884853456307659103259544610720219972987051261492650066012227313761545288968365961306505214821943309985439215309426925314576026255845117872406188518068641993478101785768856021447464209448557566636020191181601544281003626165107055082080048585336479969631834510077943822001282011742665099977204639313413956075940499705808368526593017544944112272144052323239952441415847564481425995896851241093994332008854636650130987763085954841785231835890980502485569994610132323458880710612870524431743030011528363927719502344822464171991058961109925168227305130719356549838655488468510482066741296986760409997247496680270838923506636349156674731075958062447752787061232354762854614761363621525736579699786120667165286866454480066388975198398687014845929051922123143415498626244831252570284621517896684774179700255635199891847612321278117268204469950842009799503524993309818063235182500218799650759080018122154351123385911998989227157810936635289912836231962636774628382437421903629968174188686693312879009294372285369622551411889831512138750760166787266973000995802435515920191772502848122925131091319466436064198753119540166622737565900692960270392851152601031824810875340469932322120463042791920281136052560419384768263409916957896897865871523391942220830201618218965473246567660338056924890309401106272228775128366", 
            _converter.ConvertTo(testBytes, new Int(Endianness.BigEndian)));
        Assert.AreEqual("11488502372985027448701351643356224946370002972792602319917900315687517558197562791414989227171652316031410012546580257155997326840454831161418146936125790538692854139663412407290552142342382522609909273539358316778987527323168047501550091050108892868329624770535127962387482728271718654037291077355805542355125066613592389567268962159694782080760661147708425355244759481441974093007218219380719768257712762744961852795435078900103327786049175380985825672641685412938780955086452605045631115621236007522112204189670247390261286072017761836144794692033929671957071660051943528689770242421511335903679120098863156111356685806691931775570979635361464933024459082639260633811876287032066076870754276138080750844963237025741028959807605366913714995198559648997151881679462293775200519777663549271415481776638393755976253275383645263741994951262361432817353471271913586923169371701714505191725600235118377472911559858436386068762248499139637969552088885000627748349506772223377555110841640557633281338981935570238036873302548906177388901920907720203001518588598838384219232159787449718118259816309579327264295353008407445342236137483678911621470210341026977495712048832742423445686879787992565570645935966397426672809899977776145963025312606848467717129579409522195783609735012620170537627136772577993430427520670841991170328607568662223270789432127925030821754814742669014761851276397560954722849485689985332888003899876001783087205129762497231421541351251235131389314759997009251651660398928625045707612986672010997239273101035516467066760863677889846264460868264707543189700047345071466538018787804844997460935611852652525606698080828585678239590718425364303065737935871231875389304118831457079739699628631904331202187649055689193206983348143980255770573779781695604855556591231071421019978291731778937146639028676690757407007800098531559635995482052770594199905809599584178690187594382008167009453846386807367303576125463783971084036580510291324932247076077589028889509404763513318107909886705141167006218219423901094571235548769621255566822056278723952840577954141968206931061492971714460155585632285126476", 
            _converter.ConvertTo(testBytes, new Int(Endianness.LittleEndian)));
        Assert.AreEqual("01001100011011110111001001100101011011010010000001101001011100000111001101110101011011010010000001100100011011110110110001101111011100100010000001110011011010010111010000100000011000010110110101100101011101000010110000100000011000110110111101101110011100110110010101100011011101000110010101110100011101010110010101110010001000000110000101100100011010010111000001101001011100110110001101101001011011100110011100100000011001010110110001101001011101000010111000100000010011100111010101101100011011000110000100100000011011100110111101101110001000000110000101110010011000110111010100100000011011000110000101100011011010010110111001101001011000010010000001101110011001010111000101110101011001010010000001100110011000010111010101100011011010010110001001110101011100110010000001100110011100100110100101101110011001110110100101101100011011000110000100101110001000000101001101110101011100110111000001100101011011100110010001101001011100110111001101100101001000000110111001101001011100110110110000101110001000000100000101100101011011100110010101100001011011100010000001100110011001010111001001101101011001010110111001110100011101010110110100100000011100100110100101110011011101010111001100100000011010010110010000100000011101000110111101110010011101000110111101110010001011100010000001001110011101010110110001101100011000010110110100100000011001010110011101100101011101000010000001101110011010010111001101101100001011100010000001000110011101010111001101100011011001010010000001110111011010010111001101101001001011100010000001000101011101000110100101100001011011010010000001101110011001010111000101110101011001010010111000100000010011100110000101101101001000000110110001101001011000100110010101110010011011110010000001110100011001010110110101110000011011110111001001100101001011000010000001100011011101010110110100100000011100110110111101101100011101010111010001100001001000000110111001101111011000100110100101110011001000000110010101110011011101000010000001100101011011000110100101100111011001010110111001100100011010010010000001101111011100000111010001101001011011110010000001100011011101010110110101110001011101010110010100100000011011100110100101101000011010010110110000100000011010010110110101110000011001010110010001101001011101000010000001110001011101010110111100100000011011010110100101101110011101010111001100100000011010010110010000100000011100010111010101101111011001000010000001101101011000010111100001101001011011010110010100100000011100000110110001100001011000110110010101100001011101000010000001100110011000010110001101100101011100100110010100100000011100000110111101110011011100110110100101101101011101010111001100101100001000000110111101101101011011100110100101110011001000000111011001101111011011000111010101110000011101000110000101110011001000000110000101110011011100110111010101101101011001010110111001100100011000010010000001100101011100110111010000101100001000000110111101101101011011100110100101110011001000000110010001101111011011000110111101110010001000000111001001100101011100000110010101101100011011000110010101101110011001000111010101110011001011100010000001000011011101010111001001100001011000100110100101110100011101010111001000100000011101100110100101110100011000010110010100100000011001000110100101100001011011010010000001101110011011110110111000100000011001010110111001101001011011010010000001110110011001010111001101110100011010010110001001110101011011000111010101101101001000000110100101101110011101000110010101110010011001000111010101101101001011100010000001000101011101000110100101100001011011010010000001110001011101010110100101110011001000000111000101110101011000010110110100101110001000000100111001110101011011000110110001100001011011010010000001110010011010000110111101101110011000110111010101110011001000000110000101101100011010010111000101110101011000010110110100100000011011010110010101110100011101010111001100101110001000000101001101100101011001000010000001100001011000110010000001100100011011110110110001101111011100100010000001110011011010010111010000100000011000010110110101100101011101000010000001110000011101010111001001110101011100110010000001101101011000010110110001100101011100110111010101100001011001000110000100100000011000110110111101101110011001110111010101100101001011100010000001001101011000010110010101100011011001010110111001100001011100110010000001101001011100000111001101110101011011010010000001110110011001010110110001101001011101000010110000100000011000110110111101101110011100110110010101100011011101000110010101110100011101010110010101110010001000000110010101110101001000000110110001101111011000100110111101110010011101000110100101110011001000000111010101110100001011000010000001100100011010010110001101110100011101010110110100100000011000010111010000100000011001000111010101101001001011100010000001000001011011000110100101110001011101010110000101101101001000000110111101110010011011100110000101110010011001010010000001110111011010010111001101101001001000000110010101110101001000000110110101100101011101000111010101110011001011100010000001000100011101010110100101110011001000000111011001101001011101100110010101110010011100100110000100100000011001000110100101100001011011010010000001101110011011110110111000100000011010100111010101110011011101000110111100101110001000000100111001110101011011000110110001100001001000000110010101110011011101000010111000100000010001010111100001100011011001010111000001110100011001010111010101110010001000000111001101101001011011100111010000100000011011110110001101100011011000010110010101100011011000010111010000100000011000110111010101110000011010010110010001100001011101000110000101110100001000000110111001101111011011100010000001110000011100100110111101101001011001000110010101101110011101000010110000100000011100110111010101101110011101000010000001101001011011100010000001100011011101010110110001110000011000010010000001110001011101010110100100100000011011110110011001100110011010010110001101101001011000010010000001100100011001010111001101100101011100100111010101101110011101000010000001101101011011110110110001101100011010010111010000100000011000010110111001101001011011010010000001101001011001000010000001100101011100110111010000100000011011000110000101100010011011110111001001110101011011010010111000100000010001000111010101101001011100110010000001110000011101010110110001110110011010010110111001100001011100100010111000100000010011010110111101110010011000100110100100100000011011000110010101101111001000000110110101101001001011000010000001101110011011110110111001110101011011010110110101111001001000000110010101100111011001010111010000100000011101000111001001101001011100110111010001101001011100010111010101100101001000000110111001101111011011100010110000100000011100100110100001101111011011100110001101110101011100110010000001101110011011110110111000100000011011000110010101101111001011100010000001010101011101000010000001110100011001010110110101110000011101010111001100100000011100000111010101110010011101010111001100100000011000010111010000100000011011000110111101110010011001010110110100101110", 
            _converter.ConvertTo(testBytes, new Bits()));
    }

    [TestMethod]
    public void ConvertUtf8Bytes()
    {
        var testBytes = "Letošní léto se opravdu vydařilo. Obilí zlátne.";
       
        // Output coding is not 
        Assert.AreEqual(new string(Encoding.UTF8.GetBytes(testBytes).Select(b => (char) b).ToArray()), _converter.ConvertTo(testBytes, new Bytes()));
        Assert.AreEqual("4c65746fc5a16ec3ad206cc3a9746f207365206f7072617664752076796461c599696c6f2e204f62696cc3ad207a6cc3a1746e652e", 
            _converter.ConvertTo(testBytes, new Hex()));
        Assert.AreEqual("12928572250051199991006890251084864487240443404417906070222007612035131208412619286145937174814089016190968302490399468692596014", 
            _converter.ConvertTo(testBytes, new Int(Endianness.BigEndian)));
        Assert.AreEqual("7851646965264149641681454384653649984179139774721130416446614914477773275001631836667530899706627130124024212053950748062672204", 
            _converter.ConvertTo(testBytes, new Int(Endianness.LittleEndian)));
        Assert.AreEqual("0100110001100101011101000110111111000101101000010110111011000011101011010010000001101100110000111010100101110100011011110010000001110011011001010010000001101111011100000111001001100001011101100110010001110101001000000111011001111001011001000110000111000101100110010110100101101100011011110010111000100000010011110110001001101001011011001100001110101101001000000111101001101100110000111010000101110100011011100110010100101110", 
            _converter.ConvertTo(testBytes, new Bits()));
    }
    
    [TestMethod]
    public void ConvertSpecialChars()
    {
        var testBytes = "\x01\x02\x03\x04\x05\x06\x07\x08\x09";
        
        Assert.AreEqual(testBytes, _converter.ConvertTo(testBytes, new Bytes()));
        Assert.AreEqual("010203040506070809", _converter.ConvertTo(testBytes, new Hex()));
        Assert.AreEqual("18591708106338011145", _converter.ConvertTo(testBytes, new Int(Endianness.BigEndian)));
        Assert.AreEqual("166599134359138271745", _converter.ConvertTo(testBytes, new Int(Endianness.LittleEndian)));
        Assert.AreEqual("000000010000001000000011000001000000010100000110000001110000100000001001", _converter.ConvertTo(testBytes, new Bits()));
        
        Assert.AreEqual("{0b1, 0b10, 0b11, 0b100, 0b101, 0b110, 0b111, 0b1000, 0b1001}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Binary, Brackets.Curly)));
        Assert.AreEqual("(0b1, 0b10, 0b11, 0b100, 0b101, 0b110, 0b111, 0b1000, 0b1001)", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Binary, Brackets.Regular)));
        Assert.AreEqual("[0b1, 0b10, 0b11, 0b100, 0b101, 0b110, 0b111, 0b1000, 0b1001]", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Binary, Brackets.Square)));

        Assert.AreEqual("{0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Hex, Brackets.Curly)));
        Assert.AreEqual("(0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09)", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Hex, Brackets.Regular)));
        Assert.AreEqual("[0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09]", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Hex, Brackets.Square)));
        
        Assert.AreEqual("{1, 2, 3, 4, 5, 6, 7, 8, 9}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)));
        Assert.AreEqual("(1, 2, 3, 4, 5, 6, 7, 8, 9)", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Regular)));
        Assert.AreEqual("[1, 2, 3, 4, 5, 6, 7, 8, 9]", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Square)));
        
        Assert.AreEqual("{'\\x01', '\\x02', '\\x03', '\\x04', '\\x05', '\\x06', '\\x07', '\\x08', '\\x09'}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Char, Brackets.Curly)));
        Assert.AreEqual("('\\x01', '\\x02', '\\x03', '\\x04', '\\x05', '\\x06', '\\x07', '\\x08', '\\x09')", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Char, Brackets.Regular)));
        Assert.AreEqual("['\\x01', '\\x02', '\\x03', '\\x04', '\\x05', '\\x06', '\\x07', '\\x08', '\\x09']",
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Char, Brackets.Square)));
    }
    
    [TestMethod]
    public void ConvertEmptyBytes()
    {
        var testBytes = "";
        
        Assert.AreEqual("", _converter.ConvertTo(testBytes, new Bytes()));
        Assert.AreEqual("", _converter.ConvertTo(testBytes, new Hex()));
        Assert.AreEqual("", _converter.ConvertTo(testBytes, new Int(Endianness.BigEndian)));
        Assert.AreEqual("", _converter.ConvertTo(testBytes, new Int(Endianness.LittleEndian)));
        Assert.AreEqual("", _converter.ConvertTo(testBytes, new Bits()));
        
        Assert.AreEqual("{}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Binary, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Binary, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Binary, Brackets.Square)));

        Assert.AreEqual("{}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Hex, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Hex, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Hex, Brackets.Square)));
        
        Assert.AreEqual("{}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Square)));
        
        Assert.AreEqual("{}", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Char, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Char, Brackets.Regular)));
        Assert.AreEqual("[]",
            _converter.ConvertTo(testBytes, new ByteArray(ArrayFormat.Char, Brackets.Square)));
    }
    
    [TestMethod]
    public void ConvertToBits()
    {
        var testString = "OK";
        Assert.AreEqual("0100111101001011", _converter.ConvertTo(testString, new Bits()));
    }
}