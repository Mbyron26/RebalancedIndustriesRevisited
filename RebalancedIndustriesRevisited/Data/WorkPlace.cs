using System;

namespace RebalancedIndustriesRevisited.Data;

public class WorkPlace : IEquatable<WorkPlace> {
    public int UneducatedWorkers { get; set; }
    public int EducatedWorkers { get; set; }
    public int WellEducatedWorkers { get; set; }
    public int HighlyEducatedWorkers { get; set; }

    public WorkPlace() { }

    public WorkPlace(int uneducatedWorkers, int educatedWorkers, int wellEducatedWorkers, int highlyEducatedWorkers) {
        UneducatedWorkers = uneducatedWorkers;
        EducatedWorkers = educatedWorkers;
        WellEducatedWorkers = wellEducatedWorkers;
        HighlyEducatedWorkers = highlyEducatedWorkers;
    }

    public WorkPlace Set(int uneducatedWorkers, int educatedWorkers, int wellEducatedWorkers, int highlyEducatedWorkers) {
        UneducatedWorkers = uneducatedWorkers;
        EducatedWorkers = educatedWorkers;
        WellEducatedWorkers = wellEducatedWorkers;
        HighlyEducatedWorkers = highlyEducatedWorkers;
        return this;
    }

    public WorkPlace Copy(WorkPlace from) {
        if (from is null)
            throw new ArgumentNullException(nameof(from), "Cannot copy WorkPlace from a null object");
        UneducatedWorkers = from.UneducatedWorkers;
        EducatedWorkers = from.EducatedWorkers;
        WellEducatedWorkers = from.WellEducatedWorkers;
        HighlyEducatedWorkers = from.HighlyEducatedWorkers;
        return this;
    }

    public WorkPlace Clone(WorkPlace to) {
        if (to is null)
            throw new ArgumentNullException(nameof(to), "Cannot clone to null object.");
        to.UneducatedWorkers = UneducatedWorkers;
        to.EducatedWorkers = EducatedWorkers;
        to.WellEducatedWorkers = WellEducatedWorkers;
        to.HighlyEducatedWorkers = HighlyEducatedWorkers;
        return to;
    }

    public string SimpleString() => $"{UneducatedWorkers} {EducatedWorkers} {WellEducatedWorkers} {HighlyEducatedWorkers}";

    public bool Equals(WorkPlace other) {
        return Equals(this, other);
    }

    public int Sum() {
        return UneducatedWorkers + EducatedWorkers + WellEducatedWorkers + HighlyEducatedWorkers;
    }

    public override string ToString() {
        return $"UneducatedWorkers: {UneducatedWorkers}, EducatedWorkers: {EducatedWorkers}, WellEducatedWorkers: {WellEducatedWorkers}, HighlyEducatedWorkers: {HighlyEducatedWorkers}";
    }

    public override bool Equals(object obj) {
        if (obj is not WorkPlace other) return false;
        return UneducatedWorkers == other.UneducatedWorkers && EducatedWorkers == other.EducatedWorkers && WellEducatedWorkers == other.WellEducatedWorkers && HighlyEducatedWorkers == other.HighlyEducatedWorkers;
    }

    public override int GetHashCode() {
        var hashCode = -1224015237;
        hashCode = hashCode * -1521134295 + UneducatedWorkers.GetHashCode();
        hashCode = hashCode * -1521134295 + EducatedWorkers.GetHashCode();
        hashCode = hashCode * -1521134295 + WellEducatedWorkers.GetHashCode();
        hashCode = hashCode * -1521134295 + HighlyEducatedWorkers.GetHashCode();
        return hashCode;
    }

    public static bool operator ==(WorkPlace a, WorkPlace b) {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(WorkPlace a, WorkPlace b) {
        return !(a == b);
    }
}