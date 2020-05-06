namespace VectronsLibrary
{
    public static class Utils
    {
        private static readonly string[] suffix = { "B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        /// <summary>
        /// Formats byte to a more readable notation
        /// ex:  B, kB, MB, GB, TB, PB, EB, ZB, YB
        /// </summary>
        /// <param name="bytes">The byte to format</param>
        /// <param name="start">the format of the imput, Byte = 0; kilo Byte = 1; Mega Byte = 2 etc</param>
        /// <returns>returns a string in the new format: converted value + Suffix ex: 26.55 GB</returns>
        public static string FormatBytes(ulong valueInBytes)
            => FormatBytes(valueInBytes, 0);

        /// <summary>
        /// Formats byte to a more readable notation
        /// ex:  B, kB, MB, GB, TB, PB, EB, ZB, YB
        /// </summary>
        /// <param name="value">the value to format</param>
        /// <param name="start">the format of the imput, Byte = 0; kilo Byte = 1; Mega Byte = 2 etc</param>
        /// <returns>returns a string in the new format: converted value + Suffix ex: 26.55 GB</returns>
        public static string FormatBytes(ulong value, int start)
        {
            int i = 0;
            var bytes = value;
            var dblSByte = (double)bytes;

            while (bytes / 100 > 0)
            {
                dblSByte = bytes / 1024D;
                bytes /= 1024;
                i++;
            }

            var index = i + start;
            if (index > suffix.Length - 1
                || index < 0)
            {
                return $"{dblSByte:0.00} ?B";
            }

            return $"{dblSByte:0.00} {suffix[index]}";
        }
    }
}