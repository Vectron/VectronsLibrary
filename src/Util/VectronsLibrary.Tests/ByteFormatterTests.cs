using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectronsLibrary.Tests;

/// <summary>
/// Tests for the <see cref="ByteFormatter"/> class.
/// </summary>
[TestClass]
public class ByteFormatterTests
{
    /// <summary>
    /// Test if format byte returns the smallest possible value.
    /// </summary>
    /// <param name="value">The value to change.</param>
    /// <param name="expected">The expected value.</param>
    [DataTestMethod]
    [DataRow(ulong.MinValue, "0.00 B")]
    [DataRow(1ul, "1.00 B")]
    [DataRow(10ul, "10.00 B")]
    [DataRow(100ul, "0.10 kB")]
    [DataRow(1000ul, "0.98 kB")]
    [DataRow(1000_0ul, "9.77 kB")]
    [DataRow(1000_00ul, "97.66 kB")]
    [DataRow(1000_000ul, "0.95 MB")]
    [DataRow(1000_000_0ul, "9.54 MB")]
    [DataRow(1000_000_00ul, "95.37 MB")]
    [DataRow(1000_000_000ul, "0.93 GB")]
    [DataRow(1000_000_000_000ul, "0.91 TB")]
    [DataRow(1000_000_000_000_000ul, "0.89 PB")]
    [DataRow(1000_000_000_000_000_000ul, "0.87 EB")]
    [DataRow(1000_000_000_000_000_000_0ul, "8.67 EB")]
    [DataRow(ulong.MaxValue, "16.00 EB")]
    public void FormatBytesReturnsRightValue(ulong value, string expected)
    {
        var result = ByteFormatter.Format(value, CultureInfo.InvariantCulture);
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Test if format byte returns the smallest possible value.
    /// </summary>
    /// <param name="value">The value to change.</param>
    /// <param name="start">The value of the input.</param>
    /// <param name="expected">The expected value.</param>
    [DataTestMethod]
    [DataRow(ulong.MinValue, ByteFormatter.Start.Byte, "0.00 B")]
    [DataRow(1ul, ByteFormatter.Start.Byte, "1.00 B")]
    [DataRow(10ul, ByteFormatter.Start.Byte, "10.00 B")]
    [DataRow(100ul, ByteFormatter.Start.Byte, "0.10 kB")]
    [DataRow(1000ul, ByteFormatter.Start.Byte, "0.98 kB")]
    [DataRow(1000_0ul, ByteFormatter.Start.Byte, "9.77 kB")]
    [DataRow(1000_00ul, ByteFormatter.Start.Byte, "97.66 kB")]
    [DataRow(1000_000ul, ByteFormatter.Start.Byte, "0.95 MB")]
    [DataRow(1000_000_0ul, ByteFormatter.Start.Byte, "9.54 MB")]
    [DataRow(1000_000_00ul, ByteFormatter.Start.Byte, "95.37 MB")]
    [DataRow(1000_000_000ul, ByteFormatter.Start.Byte, "0.93 GB")]
    [DataRow(1000_000_000_000ul, ByteFormatter.Start.Byte, "0.91 TB")]
    [DataRow(1000_000_000_000_000ul, ByteFormatter.Start.Byte, "0.89 PB")]
    [DataRow(1000_000_000_000_000_000ul, ByteFormatter.Start.Byte, "0.87 EB")]
    [DataRow(1000_000_000_000_000_000_0ul, ByteFormatter.Start.Byte, "8.67 EB")]
    [DataRow(ulong.MaxValue, ByteFormatter.Start.Byte, "16.00 EB")]
    [DataRow(ulong.MinValue, ByteFormatter.Start.KiloByte, "0.00 kB")]
    [DataRow(100ul, ByteFormatter.Start.KiloByte, "0.10 MB")]
    [DataRow(1000ul, ByteFormatter.Start.KiloByte, "0.98 MB")]
    [DataRow(1000_000ul, ByteFormatter.Start.KiloByte, "0.95 GB")]
    [DataRow(1000_000_000ul, ByteFormatter.Start.KiloByte, "0.93 TB")]
    [DataRow(1000_000_000_000ul, ByteFormatter.Start.KiloByte, "0.91 PB")]
    [DataRow(1000_000_000_000_000ul, ByteFormatter.Start.KiloByte, "0.89 EB")]
    [DataRow(1000_000_000_000_000_000ul, ByteFormatter.Start.KiloByte, "0.87 ZB")]
    [DataRow(1000_000_000_000_000_000_0ul, ByteFormatter.Start.KiloByte, "8.67 ZB")]
    [DataRow(ulong.MaxValue, ByteFormatter.Start.KiloByte, "16.00 ZB")]
    [DataRow(ulong.MinValue, ByteFormatter.Start.MegaByte, "0.00 MB")]
    [DataRow(100ul, ByteFormatter.Start.MegaByte, "0.10 GB")]
    [DataRow(1000ul, ByteFormatter.Start.MegaByte, "0.98 GB")]
    [DataRow(1000_000ul, ByteFormatter.Start.MegaByte, "0.95 TB")]
    [DataRow(1000_000_000ul, ByteFormatter.Start.MegaByte, "0.93 PB")]
    [DataRow(1000_000_000_000ul, ByteFormatter.Start.MegaByte, "0.91 EB")]
    [DataRow(1000_000_000_000_000ul, ByteFormatter.Start.MegaByte, "0.89 ZB")]
    [DataRow(1000_000_000_000_000_000ul, ByteFormatter.Start.MegaByte, "0.87 YB")]
    [DataRow(1000_000_000_000_000_000_0ul, ByteFormatter.Start.MegaByte, "8.67 YB")]
    [DataRow(ulong.MaxValue, ByteFormatter.Start.MegaByte, "16.00 YB")]
    [DataRow(ulong.MinValue, ByteFormatter.Start.GigaByte, "0.00 GB")]
    [DataRow(100ul, ByteFormatter.Start.GigaByte, "0.10 TB")]
    [DataRow(1000ul, ByteFormatter.Start.GigaByte, "0.98 TB")]
    [DataRow(1000_000ul, ByteFormatter.Start.GigaByte, "0.95 PB")]
    [DataRow(1000_000_000ul, ByteFormatter.Start.GigaByte, "0.93 EB")]
    [DataRow(1000_000_000_000ul, ByteFormatter.Start.GigaByte, "0.91 ZB")]
    [DataRow(1000_000_000_000_000ul, ByteFormatter.Start.GigaByte, "0.89 YB")]
    [DataRow(1000_000_000_000_000_000ul, ByteFormatter.Start.GigaByte, "0.87 ?B")]
    [DataRow(1000_000_000_000_000_000_0ul, ByteFormatter.Start.GigaByte, "8.67 ?B")]
    [DataRow(ulong.MaxValue, ByteFormatter.Start.GigaByte, "16.00 ?B")]
    [DataRow(ulong.MinValue, 4, "0.00 TB")]
    [DataRow(100ul, ByteFormatter.Start.TeraByte, "0.10 PB")]
    [DataRow(1000ul, ByteFormatter.Start.TeraByte, "0.98 PB")]
    [DataRow(1000_000ul, ByteFormatter.Start.TeraByte, "0.95 EB")]
    [DataRow(1000_000_000ul, ByteFormatter.Start.TeraByte, "0.93 ZB")]
    [DataRow(1000_000_000_000ul, ByteFormatter.Start.TeraByte, "0.91 YB")]
    [DataRow(1000_000_000_000_000ul, ByteFormatter.Start.TeraByte, "0.89 ?B")]
    [DataRow(1000_000_000_000_000_000ul, ByteFormatter.Start.TeraByte, "0.87 ?B")]
    [DataRow(1000_000_000_000_000_000_0ul, ByteFormatter.Start.TeraByte, "8.67 ?B")]
    [DataRow(ulong.MaxValue, ByteFormatter.Start.TeraByte, "16.00 ?B")]
    [DataRow(ulong.MinValue, ByteFormatter.Start.PetaByte, "0.00 PB")]
    [DataRow(100ul, ByteFormatter.Start.PetaByte, "0.10 EB")]
    [DataRow(1000ul, ByteFormatter.Start.PetaByte, "0.98 EB")]
    [DataRow(1000_000ul, ByteFormatter.Start.PetaByte, "0.95 ZB")]
    [DataRow(1000_000_000ul, ByteFormatter.Start.PetaByte, "0.93 YB")]
    [DataRow(1000_000_000_000ul, ByteFormatter.Start.PetaByte, "0.91 ?B")]
    [DataRow(1000_000_000_000_000ul, ByteFormatter.Start.PetaByte, "0.89 ?B")]
    [DataRow(1000_000_000_000_000_000ul, ByteFormatter.Start.PetaByte, "0.87 ?B")]
    [DataRow(1000_000_000_000_000_000_0ul, ByteFormatter.Start.PetaByte, "8.67 ?B")]
    [DataRow(ulong.MaxValue, ByteFormatter.Start.PetaByte, "16.00 ?B")]
    [DataRow(ulong.MinValue, ByteFormatter.Start.ExaByte, "0.00 EB")]
    [DataRow(100ul, ByteFormatter.Start.ExaByte, "0.10 ZB")]
    [DataRow(1000ul, ByteFormatter.Start.ExaByte, "0.98 ZB")]
    [DataRow(1000_000ul, ByteFormatter.Start.ExaByte, "0.95 YB")]
    [DataRow(1000_000_000ul, ByteFormatter.Start.ExaByte, "0.93 ?B")]
    [DataRow(1000_000_000_000ul, ByteFormatter.Start.ExaByte, "0.91 ?B")]
    [DataRow(1000_000_000_000_000ul, ByteFormatter.Start.ExaByte, "0.89 ?B")]
    [DataRow(1000_000_000_000_000_000ul, ByteFormatter.Start.ExaByte, "0.87 ?B")]
    [DataRow(1000_000_000_000_000_000_0ul, ByteFormatter.Start.ExaByte, "8.67 ?B")]
    [DataRow(ulong.MaxValue, ByteFormatter.Start.ExaByte, "16.00 ?B")]
    [DataRow(ulong.MinValue, ByteFormatter.Start.ZetaByte, "0.00 ZB")]
    [DataRow(100ul, ByteFormatter.Start.ZetaByte, "0.10 YB")]
    [DataRow(1000ul, ByteFormatter.Start.ZetaByte, "0.98 YB")]
    [DataRow(1000_000ul, ByteFormatter.Start.ZetaByte, "0.95 ?B")]
    [DataRow(1000_000_000ul, ByteFormatter.Start.ZetaByte, "0.93 ?B")]
    [DataRow(1000_000_000_000ul, ByteFormatter.Start.ZetaByte, "0.91 ?B")]
    [DataRow(1000_000_000_000_000ul, ByteFormatter.Start.ZetaByte, "0.89 ?B")]
    [DataRow(1000_000_000_000_000_000ul, ByteFormatter.Start.ZetaByte, "0.87 ?B")]
    [DataRow(1000_000_000_000_000_000_0ul, ByteFormatter.Start.ZetaByte, "8.67 ?B")]
    [DataRow(ulong.MaxValue, ByteFormatter.Start.ZetaByte, "16.00 ?B")]
    [DataRow(ulong.MinValue, ByteFormatter.Start.YotaByte, "0.00 YB")]
    [DataRow(100ul, ByteFormatter.Start.YotaByte, "0.10 ?B")]
    [DataRow(1000ul, ByteFormatter.Start.YotaByte, "0.98 ?B")]
    [DataRow(1000_000ul, ByteFormatter.Start.YotaByte, "0.95 ?B")]
    [DataRow(1000_000_000ul, ByteFormatter.Start.YotaByte, "0.93 ?B")]
    [DataRow(1000_000_000_000ul, ByteFormatter.Start.YotaByte, "0.91 ?B")]
    [DataRow(1000_000_000_000_000ul, ByteFormatter.Start.YotaByte, "0.89 ?B")]
    [DataRow(1000_000_000_000_000_000ul, ByteFormatter.Start.YotaByte, "0.87 ?B")]
    [DataRow(1000_000_000_000_000_000_0ul, ByteFormatter.Start.YotaByte, "8.67 ?B")]
    [DataRow(ulong.MaxValue, ByteFormatter.Start.YotaByte, "16.00 ?B")]
    [DataRow(ulong.MinValue, 9, "0.00 ?B")]
    [DataRow(100ul, 9, "0.10 ?B")]
    [DataRow(1000ul, 9, "0.98 ?B")]
    [DataRow(1000_000ul, 9, "0.95 ?B")]
    [DataRow(1000_000_000ul, 9, "0.93 ?B")]
    [DataRow(1000_000_000_000ul, 9, "0.91 ?B")]
    [DataRow(1000_000_000_000_000ul, 9, "0.89 ?B")]
    [DataRow(1000_000_000_000_000_000ul, 9, "0.87 ?B")]
    [DataRow(1000_000_000_000_000_000_0ul, 9, "8.67 ?B")]
    [DataRow(ulong.MaxValue, 9, "16.00 ?B")]
    [DataRow(ulong.MaxValue, int.MinValue, "16.00 ?B")]
    [DataRow(ulong.MaxValue, int.MaxValue, "16.00 ?B")]
    public void FormatBytesReturnsRightValueWithStart(ulong value, ByteFormatter.Start start, string expected)
    {
        var result = ByteFormatter.Format(value, start, CultureInfo.InvariantCulture);
        Assert.AreEqual(expected, result);
    }
}
