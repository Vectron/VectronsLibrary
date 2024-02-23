namespace VectronsLibrary;

/// <summary>
/// Utilities class.
/// </summary>
public static class ByteFormatter
{
    private static readonly string[] Suffix = ["B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];

    /// <summary>
    /// The start value of the input.
    /// </summary>
    public enum Start
    {
        /// <summary>
        /// Byte value.
        /// </summary>
        Byte = 0,

        /// <summary>
        /// Kilo byte value.
        /// </summary>
        KiloByte = 1,

        /// <summary>
        /// Mega byte value.
        /// </summary>
        MegaByte = 2,

        /// <summary>
        /// Giga byte value.
        /// </summary>
        GigaByte = 3,

        /// <summary>
        /// Tera byte value.
        /// </summary>
        TeraByte = 4,

        /// <summary>
        /// Peta byte value.
        /// </summary>
        PetaByte = 5,

        /// <summary>
        /// Exa byte value.
        /// </summary>
        ExaByte = 6,

        /// <summary>
        /// Zeta byte value.
        /// </summary>
        ZetaByte = 7,

        /// <summary>
        /// Yota byte value.
        /// </summary>
        YotaByte = 8,
    }

    /// <summary>
    /// Formats byte to a more readable notation
    /// ex:  B, kB, MB, GB, TB, PB, EB, ZB, YB.
    /// </summary>
    /// <param name="valueInBytes">The byte to format.</param>
    /// <returns>returns a string in the new format: converted value + Suffix ex: 26.55 GB.</returns>
    public static string Format(ulong valueInBytes)
        => Format(valueInBytes, Start.Byte);

    /// <summary>
    /// Formats byte to a more readable notation
    /// ex:  B, kB, MB, GB, TB, PB, EB, ZB, YB.
    /// </summary>
    /// <param name="value">the value to format.</param>
    /// <param name="start">The <see cref="Start"/> of the <paramref name="value"/>.</param>
    /// <returns>returns a string in the new format: converted value + Suffix ex: 26.55 GB.</returns>
    public static string Format(ulong value, Start start)
    {
        var i = 0;
        var bytes = value;
        var dblSByte = (double)bytes;

        while (bytes / 100 > 0)
        {
            dblSByte = bytes / 1024D;
            bytes /= 1024;
            i++;
        }

        var index = i + (uint)start;
        return index > Suffix.Length - 1
            || index < 0
            ? $"{dblSByte:0.00} ?B"
            : $"{dblSByte:0.00} {Suffix[index]}";
    }
}
