namespace Runnable
{
    /// <summary>
    /// Represents a void return type for Action-based pipelines.
    /// Similar to F#'s unit type or Reactive Extensions' Unit.
    /// </summary>
    public struct Unit
    {
        /// <summary>
        /// The default (and only) value of Unit.
        /// </summary>
        public static readonly Unit Default = new Unit();

        /// <summary>
        /// Implicit conversion from any value to Unit.
        /// Allows void methods to be used in pipelines.
        /// </summary>
        public static implicit operator Unit(ValueTuple _) => Default;

        /// <summary>
        /// Returns a string representation of Unit.
        /// </summary>
        public override string ToString() => "()";

        /// <summary>
        /// Checks equality with another Unit (always true).
        /// </summary>
        public override bool Equals(object? obj) => obj is Unit;

        /// <summary>
        /// Gets the hash code (always 0).
        /// </summary>
        public override int GetHashCode() => 0;
    }
}
