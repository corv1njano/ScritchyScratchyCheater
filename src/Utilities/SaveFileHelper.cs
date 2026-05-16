namespace ScritchyScratchyCheater.Utilities
{
    /// <summary>
    /// Provides helper methods for working with the save file content.
    /// </summary>
    public static class SaveFileHelper
    {
        /// <summary>
        /// Returns a sanitized double value by replacing NaN or infinity with zero.
        /// </summary>
        /// <param name="value">The double value to sanitize. If the value is NaN or infinity, it will be replaced with zero.</param>
        /// <returns>The original value if it is a finite number; otherwise, zero.</returns>
        public static double SanitizeDouble(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value)) return 0;

            return value;
        }
    }
}
