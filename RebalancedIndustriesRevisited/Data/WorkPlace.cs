using System;

namespace RebalancedIndustriesRevisited.Data;

public struct WorkPlace : IEquatable<WorkPlace> {
    public int UneducatedWorkers;
    public int EducatedWorkers;
    public int WellEducatedWorkers;
    public int HighlyEducatedWorkers;

    public WorkPlace(int uneducatedWorkers, int educatedWorkers, int wellEducatedWorkers, int highlyEducatedWorkers) {
        UneducatedWorkers = uneducatedWorkers;
        EducatedWorkers = educatedWorkers;
        WellEducatedWorkers = wellEducatedWorkers;
        HighlyEducatedWorkers = highlyEducatedWorkers;
    }

    public override string ToString() => $"{UneducatedWorkers} {EducatedWorkers} {WellEducatedWorkers} {HighlyEducatedWorkers}";

    public override bool Equals(object obj) => obj is WorkPlace other && Equals(other);

    public override int GetHashCode() {
        var hashCode = -1224015237;
        hashCode = hashCode * -1521134295 + UneducatedWorkers.GetHashCode();
        hashCode = hashCode * -1521134295 + EducatedWorkers.GetHashCode();
        hashCode = hashCode * -1521134295 + WellEducatedWorkers.GetHashCode();
        hashCode = hashCode * -1521134295 + HighlyEducatedWorkers.GetHashCode();
        return hashCode;
    }

    public WorkPlace WithUneducatedWorkers(int v) => new(v, EducatedWorkers, WellEducatedWorkers, HighlyEducatedWorkers);

    public WorkPlace WithEducatedWorkers(int v) => new(UneducatedWorkers, v, WellEducatedWorkers, HighlyEducatedWorkers);

    public WorkPlace WithWellEducatedWorkers(int v) => new(UneducatedWorkers, EducatedWorkers, v, HighlyEducatedWorkers);

    public WorkPlace WithHighlyEducatedWorkers(int v) => new(UneducatedWorkers, EducatedWorkers, WellEducatedWorkers, v);

    public bool Equals(WorkPlace other) =>
        UneducatedWorkers == other.UneducatedWorkers &&
        EducatedWorkers == other.EducatedWorkers &&
        WellEducatedWorkers == other.WellEducatedWorkers &&
        HighlyEducatedWorkers == other.HighlyEducatedWorkers;

    public int Sum() => UneducatedWorkers + EducatedWorkers + WellEducatedWorkers + HighlyEducatedWorkers;

    public static bool operator ==(WorkPlace a, WorkPlace b) => a.Equals(b);

    public static bool operator !=(WorkPlace a, WorkPlace b) => !a.Equals(b);
}