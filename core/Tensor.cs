namespace TTC.Core;

public readonly struct Tensor(int rowCount, int columnCount, int layerCount, float[] storage)
{
    public ref float this[int row, int column, int layer] => ref Storage[GetFlatIndex(row, column, layer)];
    public ref float this[int flatIndex] => ref Storage[flatIndex];
    public ref float this[nuint flatIndex] => ref Storage[flatIndex];

    public int RowCount { get; } = rowCount;
    public int ColumnCount { get; } = columnCount;
    public int LayerCount { get; } = layerCount;

    public int FlatCount => Storage.Length;
    public float[] Storage { get; } = storage;

    public void Fill(float value) => AsSpan().Fill(value);

    public Span<float> AsSpan() => Storage.AsSpan();

    public static Tensor Create(int rowCount, int columnCount, int layerCount) => new Tensor(rowCount, columnCount, layerCount, new float[rowCount * columnCount * layerCount]);

    internal int GetFlatIndex(int row, int column, int layer)
    {
        Guard.InRange(row, 0, RowCount - 1);
        Guard.InRange(column, 0, ColumnCount - 1);
        Guard.InRange(layer, 0, LayerCount - 1);

        return layer * RowCount * ColumnCount + row * ColumnCount + column;
    }
}
