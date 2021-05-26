namespace VectronsLibrary
{
    /// <summary>
    /// Utilities class.
    /// </summary>
    public static class Utils
    {
        private static readonly string[] Suffix = { "B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        /// <summary>
        /// Formats byte to a more readable notation
        /// ex:  B, kB, MB, GB, TB, PB, EB, ZB, YB.
        /// </summary>
        /// <param name="valueInBytes">The byte to format.</param>
        /// <returns>returns a string in the new format: converted value + Suffix ex: 26.55 GB.</returns>
        public static string FormatBytes(ulong valueInBytes)
            => FormatBytes(valueInBytes, 0);

        /// <summary>
        /// Formats byte to a more readable notation
        /// ex:  B, kB, MB, GB, TB, PB, EB, ZB, YB.
        /// </summary>
        /// <param name="value">the value to format.</param>
        /// <param name="start">the format of the imput, Byte = 0; kilo Byte = 1; Mega Byte = 2 etc.</param>
        /// <returns>returns a string in the new format: converted value + Suffix ex: 26.55 GB.</returns>
        public static string FormatBytes(ulong value, int start)
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

            var index = i + start;
            return index > Suffix.Length - 1
                || index < 0
                ? $"{dblSByte:0.00} ?B"
                : $"{dblSByte:0.00} {Suffix[index]}";
        }
    }
}